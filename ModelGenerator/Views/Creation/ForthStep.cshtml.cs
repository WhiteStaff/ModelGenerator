using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModelGenerator.DataBase.Models.Enums;
using ModelGenerator.Models.Core.Entities;

namespace ModelGenerator.Views.Creation
{
    public class ThirdStepModel : PageModel
    {
        public GlobalPreferences Preferences { get; set; }
        public List<SelectListItem> Danger { set; get; } = new List<SelectListItem>{
            new SelectListItem { Value = "0", Text = "Низкая" },
            new SelectListItem { Value = "1", Text = "Средняя" },
            new SelectListItem { Value = "2", Text = "Высокая" },
        };

        public List<DangerLevel> Dangers { get; set; }

        public ThirdStepModel(GlobalPreferences preferences)
        {
            Preferences = preferences;
            Dangers = preferences.Dangers.Select(x => x.DangerLevel).ToList();
        }

        public void OnGet()
        {
        }
    }
}
