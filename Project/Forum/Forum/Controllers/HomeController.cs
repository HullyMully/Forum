using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<User> _userManager;

        public HomeController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Main()
        {
            if (User.Identity.IsAuthenticated && _userManager.Users.Count() > 0)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            ViewBag.Articles = _context.Articles.ToList();
            ViewBag.CountArticles = _context.Articles.Count() + 0;

            ViewBag.UserManager = _userManager;

            return View();
        }

        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }

    }
}
