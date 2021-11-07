using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnimeProject.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeProject.Controllers
{
    public class RegLogController : Controller
    {
        ApplicationContext dataBase;

        public RegLogController(ApplicationContext context)
        {
            dataBase = context;
        }
        
        // GET: RegLogController

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(UserViewModel model)
        {
            if (ModelState.IsValid && (model.ConfirmPassword == model.Password))
            {
                User user = await dataBase.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                if (user == null)
                {
                    var currentUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        Username = model.Email,
                        Password = model.Password,
                        // ConfirmPassword = model.ConfirmPassword
                    };
                    dataBase.Users.Add(currentUser);
                    await dataBase.SaveChangesAsync();
                    
                    return RedirectToAction("Anime", "Home");
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return View(model);
        }
    }
}
