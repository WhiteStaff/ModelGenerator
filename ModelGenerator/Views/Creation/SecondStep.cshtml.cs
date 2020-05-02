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
        
        public GlobalPreferences Preferences { get; set; }

        public InitialSecurityLevel Level { get; set; }

        public List<List<bool>> Flags { get; set; } = new List<List<bool>>();

        public FirstStepModel(GlobalPreferences preferences)
        {
            _preferences = Preferences = preferences;
            Level = preferences.InitialSecurityLevel ?? new InitialSecurityLevel();
            for (int i = 0; i < 7; i++)
            {
                var list = Enumerable.Repeat(false, 4).ToList();
                Flags.Add(list);
            }

            Flags[0][(int)Level.PrivacyViolationDanger] = true;
            Flags[1][(int)Level.AvailabilityViolationDanger] = true;
            Flags[2][(int)Level.IntegrityViolationDanger] = true;
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
