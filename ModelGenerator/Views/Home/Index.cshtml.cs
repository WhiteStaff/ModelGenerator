using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModelGenerator.DataBase.Models;

namespace ModelGenerator.Views.Home
{
    public class IndexModel : PageModel
    {
        public List<Model> Models { get; set; }

        public IndexModel(List<Model> models)
        {
            Models = models;
        }

        public void OnGet()
        {
        }
    }
}
