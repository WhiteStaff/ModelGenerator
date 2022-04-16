using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models;

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
                await using var stream1 = new MemoryStream();

                await formFile.CopyToAsync(stream1);
                var file = new SecurityTestResult
                {
                    Id = Guid.NewGuid(),
                    Name = formFile.Name,
                    Data = stream1.ToArray()
                };
                _context.SecurityTestResults.Add(file);
                var model = _context.Model.First(x => x.Id == modelId);
                model.SecurityTestResult = file;
                await _context.SaveChangesAsync();
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return RedirectToAction("Index", "Home");
        }
    }
}