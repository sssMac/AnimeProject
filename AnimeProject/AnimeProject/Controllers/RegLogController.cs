using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AnimeProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AnimeProject.Controllers
{
    public class RegLogController : Controller
    {
        ApplicationContext dataBase;

        public RegLogController(ApplicationContext context)
        {
            dataBase = context;
        }
        
        public IActionResult Privacy()
        {
            var model = new User();
            if (User.Identity.IsAuthenticated)
            {
                var id = User.Claims
                    .First(x => x.Type.Equals(ClaimTypes.NameIdentifier))
                    .Value;

                model = dataBase.Users.FirstOrDefault<User>(x => x.Id.ToString() == id);
            }
            return View(model);
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
            if (ModelState.IsValid && (model.Password == model.ConfirmPassword))
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
                        ConfirmPassword = model.ConfirmPassword
                    };
                    dataBase.Users.Add(currentUser);
                    await dataBase.SaveChangesAsync();

                    await Authenticate(currentUser);
                    
                    return RedirectToAction("Anime", "Home");
                }
                else
                    ModelState.AddModelError("", "User already exists");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await dataBase.Users.FirstOrDefaultAsync(x => (x.Email == model.Email)
                                                                          && (x.Password == model.Password));
                if (user != null)
                {
                    await Authenticate(user);
                    ViewBag.User = user;
                    return RedirectToAction("Privacy", "RegLog");
                }
                ModelState.AddModelError("", "Invalid login or(and) password!");
            }

            return View(model);
        }

        private async Task Authenticate(User model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Anime", "Home");
        }
    }
}
