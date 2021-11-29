using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnimeProject.Controllers
{
    public class AnimeController : Controller
    {
        // GET: AnimeController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index(string a)
        {
            return Ok(a);
        }
    }
}
