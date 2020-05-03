using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ML;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Models.Core.Entities;
using ModelGenerator.Views.Creation;
using Newtonsoft.Json;
using ThreatsParser.Entities;
using ThreatsParser.FileActions;
using TreatsParser.Core;
using TreatsParser.Core.Helpers;

namespace ModelGenerator.Controllers
{
    [Authorize]
    public class CreationController : Controller
    {
        private readonly ThreatsDbContext _context;

        public GlobalPreferences Preferences { get; set; }

        public CreationController(ThreatsDbContext context)
        {
            _context = context;
        }

        public IActionResult Start()
        {
            Preferences = Creator.Initialize(_context, Guid.Empty);
            var json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("FirstStep", new ForthStepModel(Preferences));
        }

        public IActionResult FirstStep(LocationCharacteristic LocationCharacteristic,
            NetworkCharacteristic NetworkCharacteristic,
            PersonalDataActionCharacteristics PersonalDataActionCharacteristics,
            PersonalDataPermissionSplit PersonalDataPermissionSplit,
            OtherDBConnections OtherDBConnections,
            AnonymityLevel AnonymityLevel,
            PersonalDataSharingLevel PersonalDataSharingLevel)
        {
            var json = TempData["qw"] as string ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            Preferences.InitialSecurityLevel.AnonymityLevel = AnonymityLevel;
            Preferences.InitialSecurityLevel.NetworkCharacteristic = NetworkCharacteristic;
            Preferences.InitialSecurityLevel.OtherDbConnections = OtherDBConnections;
            Preferences.InitialSecurityLevel.PersonalDataActionCharacteristics = PersonalDataActionCharacteristics;
            Preferences.InitialSecurityLevel.PersonalDataPermissionSplit = PersonalDataPermissionSplit;
            Preferences.InitialSecurityLevel.PersonalDataSharingLevel = PersonalDataSharingLevel;
            Preferences.InitialSecurityLevel.TerritorialLocation = LocationCharacteristic;
            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("SecondStep", new FirstStepModel(Preferences));
        }

        public IActionResult FirstStepBack(List<RiskProbabilities> Risks)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            for (var i = 0; i < Preferences.Items.Count; i++)
            {
                Preferences.Items[i].RiskProbabilities = Risks[i];
            }
            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("SecondStep", new FirstStepModel(Preferences));
        }

        public IActionResult SecondStep(List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava, DangerLevel Int)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            Preferences.InitialSecurityLevel.PrivacyViolationDanger = Pri;
            Preferences.InitialSecurityLevel.IntegrityViolationDanger = Int;
            Preferences.InitialSecurityLevel.AvailabilityViolationDanger = Ava;
            Preferences.Source = Preferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            Preferences.Targets = Preferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            var temp = Preferences.AllItems
                .Where(x => x.Source
                    .Any(y => Preferences.Source
                        .Where(x2 => x2.Item2)
                        .Select(x1 => x1.Item1)
                        .Contains(y)))
                .Where(x =>
                    x.ExposureSubject
                        .Any(y => Preferences.Targets
                            .Where(x2 => x2.Item2)
                            .Select(k => k.Item1)
                            .Contains(y)))
                .OrderBy(x => x.Id)
                .ToList();
            foreach (var threatModel in temp)
            {
                var curr = Preferences.Items.FirstOrDefault(x => threatModel.Id == x.Id);
                if (curr != null)
                {
                    threatModel.RiskProbabilities = curr.RiskProbabilities;
                }
            }

            Preferences.Items = temp;

            foreach (var line in Preferences.AllDangers)
            {
                if (line.Properties.Contains("конф"))
                {
                    line.DangerLevel = Pri;
                }

                if (line.Properties.Contains("дост") && line.DangerLevel < Ava)
                {
                    line.DangerLevel = Ava;
                }

                if (line.Properties.Contains("цело") && line.DangerLevel < Int)
                {
                    line.DangerLevel = Int;
                }
            }

            var projectPath = Path.GetDirectoryName(Directory.GetCurrentDirectory());
            MLContext mlContext = new MLContext();
            ITransformer trainedModel = mlContext.Model.Load($"{projectPath}\\model.zip", out var modelSchema);
            

            foreach (var preferencesItem in Preferences.Items)
            {
                var modelPart = Resolver.GetMlModel(Preferences);
                modelPart.ThreatName = Preferences.AllItems
                    .OrderBy(x => x.Name)
                    .Select((x, i) => (x, i))
                    .FirstOrDefault(x => x.x.Name == preferencesItem.Name).i;

                var predictionEngine = mlContext.Model.CreatePredictionEngine<PossibilityMLModel, RiskPrediction>(trainedModel);
                var prediction = predictionEngine.Predict(modelPart);

                RiskProbabilities risk;
                if (prediction.Risk < 0) risk = RiskProbabilities.Unlikely;
                else
                if (prediction.Risk >= 4) risk = RiskProbabilities.High;
                else
                {
                    risk = (RiskProbabilities) ((int) Math.Round(Convert.ToDouble(prediction.Risk)));
                }

                preferencesItem.RiskProbabilities = risk;
            }

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("ThirdStep", new SecondStepModel(Preferences));
        }

        public IActionResult SecondStepBack(List<DangerLevel> Dangers)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            for (var i = 0; i < Preferences.Dangers.Count; i++)
            {
                Preferences.Dangers[i].DangerLevel = Dangers[i];
            }
            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("ThirdStep", new SecondStepModel(Preferences));
        }

        public IActionResult ThirdStep(List<RiskProbabilities> Risks)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            for (var i = 0; i < Preferences.Items.Count; i++)
            {
                Preferences.Items[i].RiskProbabilities = Risks[i];
            }
            Preferences.Dangers = Preferences.AllDangers
                .Where(x =>
                    Preferences.Source
                        .Where(y => y.Item2)
                        .Select(y => y.Item1)
                        .Contains(x.Source) &&
                    Preferences.Items
                        .Select(y => y.Name)
                        .Contains(x.ThreatName))
                .OrderBy(x => x.ThreatName)
                .ToList();

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("ForthStep", new ThirdStepModel(Preferences));
        }

        public IActionResult ThirdStepBack()
        {
            var json = TempData["qw"] as string ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            return View("ForthStep", new ThirdStepModel(Preferences));
        }

        public IActionResult ForthStep()
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            return View("FirstStep", new ForthStepModel(Preferences));
        }

        public IActionResult ForthStepBack(List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava, DangerLevel Int)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            Preferences.InitialSecurityLevel.PrivacyViolationDanger = Pri;
            Preferences.InitialSecurityLevel.IntegrityViolationDanger = Int;
            Preferences.InitialSecurityLevel.AvailabilityViolationDanger = Ava;
            Preferences.Source = Preferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            Preferences.Targets = Preferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            var temp = Preferences.AllItems
                .Where(x => x.Source
                    .Any(y => Preferences.Source
                        .Where(x2 => x2.Item2)
                        .Select(x1 => x1.Item1)
                        .Contains(y)))
                .Where(x =>
                    x.ExposureSubject
                        .Any(y => Preferences.Targets
                            .Where(x2 => x2.Item2)
                            .Select(k => k.Item1)
                            .Contains(y)))
                .OrderBy(x => x.Id)
                .ToList();
            foreach (var threatModel in temp)
            {
                var curr = Preferences.Items.FirstOrDefault(x => threatModel.Id == x.Id);
                if (curr != null)
                {
                    threatModel.RiskProbabilities = curr.RiskProbabilities;
                }
            }

            Preferences.Items = temp;

            foreach (var line in Preferences.AllDangers)
            {
                if (line.Properties.Contains("конф"))
                {
                    line.DangerLevel = Pri;
                }

                if (line.Properties.Contains("дост") && line.DangerLevel < Ava)
                {
                    line.DangerLevel = Ava;
                }

                if (line.Properties.Contains("цело") && line.DangerLevel < Int)
                {
                    line.DangerLevel = Int;
                }
            }

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("FirstStep", new ForthStepModel(Preferences));
        }

        public IActionResult Preview(List<DangerLevel> Dangers)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            for (var i = 0; i < Preferences.Dangers.Count; i++)
            {
                Preferences.Dangers[i].DangerLevel = Dangers[i];
            }
            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("Preview", new PreviewModel(Preferences));
        }

        public IActionResult Download(string Name)
        {
            if (string.IsNullOrEmpty(Name)) Name = "No Name";
            var json = TempData["qw"] as string ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            var model = ModelGeneration.GenerateModelForPreview(Preferences)
                .OrderBy(x => x.ThreatName)
                .ThenBy(x => x.Target)
                .ThenBy(x => x.Source)
                .Select((x, y) => new ModelLine(y + 1, x))
                .ToList();
            var bytes = FileSaver.SaveModel(model);
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{Name}.docx");
        }

        public IActionResult SaveModel(string Name)
        {
            if (string.IsNullOrEmpty(Name)) Name = "No Name"; 
            var json = TempData["qw"] as string ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            var model = ModelGeneration.GenerateModelForPreview(Preferences)
                .OrderBy(x => x.ThreatName)
                .ThenBy(x => x.Target)
                .ThenBy(x => x.Source)
                .Select((x, y) => new ModelLine(y + 1, x))
                .ToList();
            var userId = _context.User.FirstOrDefault(x => x.Login == User.Identity.Name).Id;
            Creator.SaveModel(_context, Preferences, model, userId, Name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult PreviewExist(Guid id)
        {
            Preferences = Creator.Initialize(_context, id);
            var json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("FirstStep", new ForthStepModel(Preferences));
        }

        public IActionResult DeleteExist(Guid id)
        {
            Creator.DeleteModel(_context, id);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult DownloadExist(Guid id)
        {
            Preferences = Creator.Initialize(_context, id);
            var model = ModelGeneration.GenerateModelForPreview(Preferences)
                .OrderBy(x => x.ThreatName)
                .ThenBy(x => x.Target)
                .ThenBy(x => x.Source)
                .Select((x, y) => new ModelLine(y + 1, x))
                .ToList();
            var bytes = FileSaver.SaveModel(model);
            var Name = _context.Model.FirstOrDefault(x => x.Id == id).Name;
            return File(bytes,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                $"{Name}.docx");
        }

    }
}