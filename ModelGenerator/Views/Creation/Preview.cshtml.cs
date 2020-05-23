using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelGenerator.Models.Core;
using ModelGenerator.Models.Core.Entities;

namespace ModelGenerator.Views.Creation
{
    public class PreviewModel : PageModel
    {
        public List<ModelLine> Model { get; set; }

        public PreviewModel(GlobalPreferences preferences)
        {
            Model = ModelGeneration.GenerateModelForPreview(preferences)
                .OrderBy(x => x.ThreatName)
                .ThenBy(x => x.Target)
                .ThenBy(x => x.Source)
                .Select((x, y) => new ModelLine(y + 1, x))
                .ToList();
        }

        public void OnGet()
        {
        }
    }
}
