using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models;
using ModelGenerator.Models.Core.Helpers;
using Newtonsoft.Json;
using RtfPipe;

namespace ModelGenerator.Controllers
{
    public class UploadController : Controller
    {

        private readonly ThreatsDbContext _context;

        public UploadController(ThreatsDbContext context)
        {
            _context = context;
        }

        // GET: Upload
        [HttpGet("FileUpload")]
        public ActionResult Index(Guid Id)
        {
            TempData["IdforFile"] = Id;
            return View();
        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(IFormFile formFile)
        {
            var modelId = new Guid(TempData["IdforFile"].ToString()!);
            if (formFile.Length > 0)
            {
                await using var memoryStream = new MemoryStream();

                await formFile.CopyToAsync(memoryStream);
                var file = new SecurityTestResult
                {
                    Id = Guid.NewGuid(),
                    Name = formFile.FileName,
                    Data = memoryStream.ToArray()
                };
                _context.SecurityTestResults.Add(file);
                memoryStream.Position = 0;
                var model = _context.Model.First(x => x.Id == modelId);
                model.SecurityTestResult = file;
                await _context.SaveChangesAsync();

                await ParseFile(memoryStream, modelId);
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return RedirectToAction("Index", "Home");
        }

        private async Task ParseFile(Stream stream, Guid modelId)
        {
            var html = Rtf.ToHtml(new RtfSource(new StreamReader(stream, Encoding.GetEncoding("windows-1251"))));

            var htmlSnippet = new HtmlDocument();
            htmlSnippet.LoadHtml(html);
            var t = htmlSnippet.DocumentNode.ChildNodes.First().ChildNodes;
            var r = t.ToList();
            var startIndex = t.ToList().FindIndex(x => x.InnerText.Equals("Cервисы и уязвимости"));
            var tables = t.Where((x, i) => i > startIndex).Where(x => x.Name.Equals("table")).Where(x => x.InnerText.StartsWith("Уязвимость"));
            var modelLines = _context.ModelLine
                .Include(x => x.Vulnerabilities)
                .Include(x => x.Threat)
                .Where(x => x.ModelId == modelId).ToList();
            foreach (var htmlNode in tables)
            {
                var nodes = htmlNode.ChildNodes;
                var name = nodes[2].ChildNodes.ToList()[1].InnerText.Trim();
                var host = nodes[2].ChildNodes.ToList()[3].InnerText.Trim();
                var dangerous = nodes[3].ChildNodes.ToList()[1].InnerText.Trim();
                var service = nodes[3].ChildNodes.ToList()[3].InnerText.Trim();
                var description = "";
                try
                {
                    var index = nodes[4].ChildNodes.First().ChildNodes.Select((x, i) => new {x, i}).First(x => x.x.InnerText == "Описание").i;
                    description = nodes[4].ChildNodes.First().ChildNodes[++index].InnerText.Trim();
                }
                catch (Exception e)
                {
                    
                }
                var solution = "";
                try
                {
                    var index = nodes[4].ChildNodes.First().ChildNodes.Select((x, i) => new { x, i }).First(x => x.x.InnerText == "Решение").i;
                    solution = nodes[4].ChildNodes.First().ChildNodes[++index].InnerText.Trim();
                }
                catch (Exception e)
                {

                }

                var lineId = Guid.Empty;
                var max = double.MinValue;
                var client = new HttpClient();
                foreach (var modelLine1 in modelLines.Where(x => x.IsActual))
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri("https://api.dandelion.eu/datatxt/sim/v1"),
                        Content = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                            { "text1", description },
                            {"text2", modelLine1.Threat.Description},
                            {"token", "3b47a253c1da4118affb8286845c2836"}

                        }),
                    };
                    using var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var similarity = 0d;
                    try
                    {
                        similarity = double.Parse(JsonConvert.DeserializeObject<Dictionary<string, string>>(body)["similarity"], CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }

                    if (similarity > max)
                    {
                        max = similarity;
                        lineId = modelLine1.Id;
                    }
                }

                _context.ModelLineVulnerability.Add(new ModelLineVulnerability
                {
                    Id = Guid.NewGuid(),
                    ModelLineId = lineId,
                    Name = name,
                    Level = Resolver.ResolveVulnerabilityLevel(dangerous),
                    HostIp = host,
                    Service = service,
                    FixNotes = solution
                });
                _context.SaveChanges();

            }
        }
    }
}