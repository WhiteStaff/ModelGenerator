using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Models.Core;
using ModelGenerator.Models.Core.Entities;
using ModelGenerator.Models.Core.FileActions;
using ModelGenerator.Models.Core.Helpers;
using ModelGenerator.Views.Creation;
using Newtonsoft.Json;

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

        public IActionResult Cancel()
        {
            return RedirectToAction("Index", "Home");
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

            Preferences.SecondStep(LocationCharacteristic, NetworkCharacteristic, PersonalDataActionCharacteristics,
                PersonalDataPermissionSplit, OtherDBConnections, AnonymityLevel, PersonalDataSharingLevel);

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("SecondStep", new FirstStepModel(Preferences));
        }

        public IActionResult FirstStepBack(List<RiskProbabilities> Risks)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);

            Preferences.SecondStepBack(Risks);

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("SecondStep", new FirstStepModel(Preferences));
        }

        public IActionResult SecondStep(List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava,
            DangerLevel Int)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);

            Preferences.ThirdStep(values, targets, Pri, Ava, Int);

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("ThirdStep", new SecondStepModel(Preferences));
        }

        public IActionResult SecondStepBack(List<DangerLevel> Dangers)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);

            Preferences.ThirdStepBack(Dangers);

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("ThirdStep", new SecondStepModel(Preferences));
        }

        public IActionResult ThirdStep(List<RiskProbabilities> Risks)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);

            Preferences.ForthStep(Risks);

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

        public IActionResult ForthStepBack(List<string> values, List<string> targets, DangerLevel Pri, DangerLevel Ava,
            DangerLevel Int)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
           Preferences.FirstStepBack(values, targets, Pri, Ava, Int);

            json = JsonConvert.SerializeObject(Preferences);
            TempData["qw"] = json;
            HttpContext.Session.SetString("pref", json);
            return View("FirstStep", new ForthStepModel(Preferences));
        }

        public IActionResult Preview(List<DangerLevel> Dangers)
        {
            var json = (TempData["qw"] as string) ?? HttpContext.Session.GetString("pref");
            Preferences = JsonConvert.DeserializeObject<GlobalPreferences>(json);
            Preferences.Preview(Dangers);

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