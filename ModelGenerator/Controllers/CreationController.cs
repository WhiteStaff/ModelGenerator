using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Views.Creation;
using ThreatsParser.Entities;
using ThreatsParser.FileActions;

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
            Preferences = FileCreator.Initialize(context, Guid.Empty);
        }


        public IActionResult FirstStep(List<string> values, List<string> targets)
        {
            return View("FirstStep", new FirstStepModel(Preferences));
        }

        public IActionResult FirstStepHelper(List<string> values, List<string> targets)
        {
            Preferences.Source = Preferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            Preferences.Targets = Preferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            return View("FirstStep", new FirstStepModel(Preferences));
        }

        public IActionResult SecondStep(List<string> values, List<string> targets)
        {
            Preferences.Source = Preferences.Source.Select(x => (x.Item1, values.Contains(x.Item1))).ToList();
            Preferences.Targets = Preferences.Targets.Select(x => (x.Item1, targets.Contains(x.Item1))).ToList();
            Preferences.Items = Preferences.AllItems
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
                .ToList();
            return View("SecondStep", new SecondStepModel(Preferences));
        }

        public IActionResult ThirdStep(List<string> Ids, List<RiskProbabilities> Risks)
        {
            return Ok();
        }

    }
}