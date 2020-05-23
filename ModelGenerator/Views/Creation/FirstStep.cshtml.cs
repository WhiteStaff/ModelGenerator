using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelGenerator.Models.Core.Entities;

namespace ModelGenerator.Views.Creation
{
    public class ForthStepModel : PageModel
    {
        public GlobalPreferences Preferences { get; set; }
        public InitialSecurityLevel Level { get; set; }
        public List<List<bool>> Flags { get; set; } = new List<List<bool>>();

        public ForthStepModel(GlobalPreferences preferences)
        {
            Preferences = preferences;
            Level = preferences.InitialSecurityLevel ?? new InitialSecurityLevel();

            for (int i = 0; i < 7; i++)
            {
                var list = Enumerable.Repeat(false, 7).ToList();
                Flags.Add(list);
            }

            Flags[0][(int) Level.TerritorialLocation] = true;
            Flags[1][(int) Level.NetworkCharacteristic] = true;
            Flags[2][(int) Level.PersonalDataActionCharacteristics] = true;
            Flags[3][(int) Level.PersonalDataPermissionSplit] = true;
            Flags[4][(int) Level.OtherDbConnections] = true;
            Flags[5][(int) Level.AnonymityLevel] = true;
            Flags[6][(int) Level.PersonalDataSharingLevel] = true;

        }

        public void OnGet()
        {
        }
    }
}
