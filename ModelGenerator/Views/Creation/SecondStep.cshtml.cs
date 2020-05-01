using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelGenerator.DataBase.Models.Enums;
using ThreatsParser.Entities;

namespace ModelGenerator.Views.Creation
{
    public class SecondStepModel : PageModel
    {
        public GlobalPreferences Preferences { get; set; }
        public List<SelectListItem> Risk { set; get; } = new List<SelectListItem>{
            new SelectListItem { Value = "0", Text = "Маловероятно" },
            new SelectListItem { Value = "2", Text = "Низкая" },
            new SelectListItem { Value = "5", Text = "Средняя" },
            new SelectListItem { Value = "10", Text = "Высокая" },
        };

        public List<RiskProbabilities> Risks { get; set; }
        public List<int> Ids { get; set; }

        public SecondStepModel(GlobalPreferences preferences)
        {
            Preferences = preferences;
            Risks = preferences.Items.Select(x => x.RiskProbabilities).ToList();
            Ids = preferences.Items.Select(x => x.Id).ToList();
        }

        public void OnGet()
        {
        }
    }
}
