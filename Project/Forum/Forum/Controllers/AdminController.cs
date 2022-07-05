using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum.ViewModels;
using Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Html;

namespace Forum.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public AdminController(ApplicationContext context, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Accept(string id)
        {

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (ViewBag.User.Role != "Admin")
                return RedirectToAction("Login");

            Report report = _context.Reports.FirstOrDefault(x => x.Id == id);
            _context.Articles.Remove(_context.Articles.FirstOrDefault(x => x.Id == report.ArticleId));
            _context.Reports.Remove(report);
            _context.Estimates.RemoveRange(_context.Estimates.Where(x => x.ArticleId == report.ArticleId));
            _context.Comments.RemoveRange(_context.Comments.Where(x => x.ArticleId == report.ArticleId));
            _context.SaveChanges();
            TempData["message"] = $"Статья удаленна!";
            return RedirectToAction("Report");
        }

        [HttpPost]
        public IActionResult Reject(string id)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (ViewBag.User.Role != "Admin")
                return RedirectToAction("Login");

            Report report = _context.Reports.FirstOrDefault(x => x.Id == id);
            _context.Reports.Remove(report);
            _context.SaveChanges();
            TempData["message"] = $"Жалоба отклоненна!";
            return RedirectToAction("Report");
        }

        [HttpGet]
        public IActionResult Report()
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (ViewBag.User.Role != "Admin")
                return RedirectToAction("Login");

            return View(_context.Reports.ToList());
        }
        public IActionResult Panel()
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if(ViewBag.User.Role != "Admin")
                return RedirectToAction("Login");

            return View(_context.Users.ToList());
        }

        public IActionResult ViewPage(string id)
        {
            return RedirectToAction("Index", "Profile", new { id = id });
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (ViewBag.User.Role != "Admin")
                return Content("");

            User user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                TempData["message"] = $"{user.UserName} удалён!";
            }
            else
                TempData["message"] = $"Пользователь не найден!";

            return RedirectToAction("Panel");
        }

        [HttpPost]
        public IActionResult EditRole(string id, string role)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (ViewBag.User.Role != "Admin")
                return Content("");

            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            user.Role = role;

            _context.Update(user);
            _context.SaveChanges();
            TempData["message"] = $"{user.UserName} успешно стал {user.Role}!";

            return RedirectToAction("Panel");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            return View(new AdminLoginViewModel());
        }

        [HttpPost]
        public IActionResult Login(AdminLoginViewModel reg)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);
            else
                return Content("");

            if (reg != null)
            {
                if (reg.Login == "321" && reg.Password == "321")
                {
                    User user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                    user.Role = "Admin";

                    _context.Update(user);
                    _context.SaveChanges();
                    TempData["message"] = $"Вы успешно стали {user.Role}!";
                }
            }

            return RedirectToAction("Panel");
        }
    }
}
