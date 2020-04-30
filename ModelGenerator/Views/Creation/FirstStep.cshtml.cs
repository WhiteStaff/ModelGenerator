using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelGenerator.DataBase.Models;
using ThreatsParser.Entities;

namespace ModelGenerator.Views.Creation
{
    public class FirstStepModel : PageModel
    {
        private GlobalPreferences _preferences;
        [BindProperty]
        public string Values { get; set; }
        public GlobalPreferences Preferences { get; set; }

        public FirstStepModel(GlobalPreferences preferences)
        {
            _preferences = Preferences = preferences;
        }

        public void OnGet()
        {
            Preferences = _preferences;
        }

        public void OnPost()
        {
            Preferences = _preferences;
        }
    }
}
