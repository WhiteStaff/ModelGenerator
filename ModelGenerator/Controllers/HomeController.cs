using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelGenerator.DataBase;
using ModelGenerator.DataBase.Models;
using ModelGenerator.Models;
using ThreatsParser.Entities;
using ThreatsParser.FileActions;

namespace ModelGenerator.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ThreatsDbContext _context;

        public HomeController(ILogger<HomeController> logger, ThreatsDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        
        public IActionResult Index()
        {
            //FileCreator.SetParsedData(_context);
            return View();
        }

        public IActionResult Create()
        {
            return RedirectToAction("Start", "Creation");
        }

        [HttpGet, Route("createDb")]
        public IActionResult CreateTable()
        {
            FileCreator.SetParsedData(_context);

            return Ok();
        }

        [AllowAnonymous]
        public IActionResult Creation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateUser(string Name, string Login, string Password)
        {
            var sha1 = new SHA1CryptoServiceProvider();
            var data = Encoding.ASCII.GetBytes(Password);

            _context.Add(new User
            {
                Id = Guid.NewGuid(),
                Login = Login,
                Name = Name,
                Password = Encoding.ASCII.GetString( sha1.ComputeHash(data)),
            });

            _context.SaveChanges();

            return RedirectToAction("Login", "Account");
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
