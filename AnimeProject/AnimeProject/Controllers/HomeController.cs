using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeProject.Models;

namespace AnimeProject.Controllers
{
    public class HomeController : Controller
    {
        // GET: HomeController
        public IActionResult Anime()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
    }
}
