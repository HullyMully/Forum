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
    public class ProfileController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        IWebHostEnvironment _appEnvironment;
            
        [HttpGet]
        public IActionResult SendReport(string articleId, string userId)
        {
            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = true;
            ViewBag.UserManager = _userManager;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            ReportViewModel report = new ReportViewModel() { ArticleId = articleId, UserId = userId };
            ViewBag.ArticleId = articleId;
            ViewBag.UserId = userId;
            return View(report);
        }    

        [HttpPost]
        public IActionResult SendReport(string content, string userId, string articleId)
        {
            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = true;
            ViewBag.UserManager = _userManager;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            if(content == null ||content.Length < 16 || content.Length > 250)
                return Content("У вас слишком мало(<16) или слишком много символов(>250)!");

            if (ModelState.IsValid)
            {
                Report report = new Report();
                if (_context.Reports.Count() == 0) report.Id = "1";
                else report.Id = Convert.ToString(Convert.ToInt32(_context.Reports.OrderByDescending(x => x.Id).Take(1).First().Id) + 1);
                report.Content = content;
                report.SenderUserId = User.Identity.GetUserId();
                report.HostUserId = userId;
                report.ArticleId = articleId;
                _context.Reports.Add(report);
                _context.SaveChanges();
                TempData["message"] = $"Жалоба отправлена!";
                return RedirectToAction("Main", "Home");
            }
            return Content("Упс.. что-то пошло не так");
        }

        [HttpPost]
        public IActionResult SendComment(string id, string content)
        {
            Article article = _context.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article == null) return Content("Ничего не найденно.");

            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = false;
            ViewBag.UserManager = _userManager;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            User user = _context.Users.First(x => x.UserName == User.Identity.Name);

            Comment comment = new Comment();
            if (_context.Comments.Count() == 0) comment.Id = "1";
            else comment.Id = Convert.ToString(Convert.ToInt32(_context.Comments.OrderByDescending(x => x.Id).Take(1).First().Id) + 1);

            comment.UserId = user.Id;
            comment.ArticleId = article.Id;
            comment.Content = content;


            _context.Comments.Add(comment);
            _context.SaveChanges();
            return RedirectToAction("Article", new { id = id });
        }

        [HttpGet]
        public IActionResult AddLike(string id)
        {
            if(!User.Identity.IsAuthenticated)
                return Content("Вы не зарегестрированны!");

            Article article = _context.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article == null) return Content("Ничего не найденно.");

            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = false;
            ViewBag.UserManager = _userManager;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            User user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (_context.Estimates.FirstOrDefault(x => x.UserId == user.Id && x.ArticleId == article.Id) != null)
            {
                Estimate estimate = _context.Estimates.FirstOrDefault(x => x.UserId == user.Id && x.ArticleId == article.Id);
                if (!estimate.Like)
                {
                    article.Like++;
                    estimate.Like = true;
                    if (estimate.Dislike)
                    {
                        estimate.Dislike = false;
                        article.Dislike--;
                    }

                    _context.Estimates.Update(estimate);
                    _context.SaveChanges();
                }
                else
                {                                       
                    article.Like--;
                    estimate.Like = false;

                    _context.Estimates.Update(estimate);
                    _context.SaveChanges();   
                }
            }
            else
            {
                Estimate estimate = new Estimate();
                if (_context.Estimates.Count() == 0) estimate.Id = "1";
                else estimate.Id = Convert.ToString(Convert.ToInt32(_context.Estimates.OrderByDescending(x => x.Id).Take(1).First().Id) + 1);

                estimate.Like = true;
                estimate.UserId = user.Id;
                estimate.ArticleId = article.Id;

                article.Like++;

                _context.Estimates.Add(estimate);
                _context.SaveChanges();
            }

            _context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("Article", new { id = id });
        }

        [HttpGet]
        public IActionResult AddDislike(string id)
        {
            if (!User.Identity.IsAuthenticated)
                return Content("Вы не зарегестрированны!");

            Article article = _context.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article == null) return Content("Ничего не найденно.");

            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = false;
            ViewBag.UserManager = _userManager;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);

            User user = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (_context.Estimates.FirstOrDefault(x => x.UserId == user.Id && x.ArticleId == article.Id) != null)
            {
                Estimate estimate = _context.Estimates.FirstOrDefault(x => x.UserId == user.Id && x.ArticleId == article.Id);
                if (!estimate.Dislike)
                {
                    article.Dislike++;
                    estimate.Dislike = true;
                    if (estimate.Like)
                    {
                        article.Like--;
                        estimate.Like = false;
                    }

                    _context.Estimates.Update(estimate);
                    _context.SaveChanges();
                }
                else
                {
                    article.Dislike--;
                    estimate.Dislike = false;

                    _context.Estimates.Update(estimate);
                    _context.SaveChanges();

                }
            }
            else
            {
                Estimate estimate = new Estimate();
                if (_context.Estimates.Count() == 0) estimate.Id = "1";
                else estimate.Id = Convert.ToString(Convert.ToInt32(_context.Estimates.OrderByDescending(x => x.Id).Take(1).First().Id) + 1);

                estimate.Like = true;
                estimate.UserId = user.Id;
                estimate.ArticleId = article.Id;

                article.Dislike++;

                _context.Estimates.Add(estimate);
                _context.SaveChanges();
            }


            _context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("Article", new { id = id });
        }
        public ProfileController(ApplicationContext context, Microsoft.AspNetCore.Identity.UserManager<User> userManager, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewBag.IsEdit = false;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            if (_userManager.FindByIdAsync(id).Result == null)
                return Content("Пользователь с таким Id не существует!");

            if (User.Identity.GetUserId() == id )
                ViewBag.IsMyProfile = true;
            else
            {
                ViewBag.IsMyProfile = false;
                ViewBag.User = _userManager.FindByIdAsync(id).Result;
            }

            ViewBag.Articles = _context.Articles.Where(x => x.UserId == id).ToList();
            ViewBag.CountArticles = _context.Articles.Where(x => x.UserId == id).Count() + 0;

            return View("Index", id);
        }

        [HttpPost]
        public IActionResult Index(bool isEdit)
        {
            ViewBag.Articles = _context.Articles.Where(x => x.UserId == User.Identity.GetUserId()).ToList();
            ViewBag.CountArticles = _context.Articles.Where(x => x.UserId == User.Identity.GetUserId()).Count() + 0;

            ViewBag.IsEdit = isEdit;

            ViewBag.IsMyProfile = true;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            return View();
        }

        [HttpPost]
        public IActionResult EditSocial(User user)
        {

            ViewBag.Articles = _context.Articles.Where(x => x.UserId == User.Identity.GetUserId()).ToList();
            ViewBag.CountArticles = _context.Articles.Where(x => x.UserId == User.Identity.GetUserId()).Count() + 0;

            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = true;
            ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            var me = _userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
            me.Website = user.Website;
            me.Github = user.Github;
            me.Twitter = user.Twitter;
            me.Discord = user.Discord;
            me.Facebook = user.Facebook;

            _userManager.UpdateAsync(me);
            _context.SaveChanges();

            return RedirectToAction($"Index", new { id = me.Id });
        }

        [HttpGet]
        public IActionResult NewArticle()
        {
            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = true;

            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            return View("NewArticle");
        }

        [HttpPost]
        public async Task<IActionResult> NewArticle(ArticleViewModel article)
        {
            if (User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            if (ModelState.IsValid)
            {
                User user = _userManager.FindByIdAsync(User.Identity.GetUserId()).Result;
                Article articelNew = new Article();

                if (_context.Articles.Count() == 0) articelNew.Id = "1";
                else articelNew.Id = Convert.ToString(Convert.ToInt32(_context.Articles.OrderByDescending(x => x.Id).Take(1).First().Id) + 1);
                articelNew.UserId = user.Id;
                articelNew.Name = article.Name;

                articelNew.Content = article.Content;

                _context.Articles.Add(articelNew);

                await _context.SaveChangesAsync();

                ViewBag.IsEdit = false;

                ViewBag.IsMyProfile = true;

                if (User.Identity.IsAuthenticated)
                    ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

                return RedirectToAction($"Index", "Profile", new { id = User.Identity.GetUserId() });
            }
            return View(article);
        }

        [HttpGet]
        public IActionResult Article(string id)
        {
            Article article = _context.Articles.Where(x => x.Id == id).FirstOrDefault();
            if (article == null) return Content("Ничего не найденно.");

            ViewBag.IsEdit = false;

            ViewBag.IsMyProfile = true;
            ViewBag.UserManager = _userManager;

            if(User.Identity.IsAuthenticated)
                ViewBag.User = _context.Users.First(x => x.UserName == User.Identity.Name);

            ViewBag.CountComments = _context.Comments.Where(x => x.ArticleId == id).Count();
            ViewBag.Comments = _context.Comments.Where(x => x.ArticleId == id).ToList();

            return View(article);
        }

        [HttpPost]
        public IActionResult RemoveArticle(string id)
        {
            Article article = _context.Articles.First(x => x.Id == id);
            if (article == null) return NotFound();
            if(User.Identity.GetUserId() != article.UserId || !User.Identity.IsAuthenticated)
                return Content("Ошибка: Недостаточно прав!");
            _context.Articles.Remove(article);
            _context.Estimates.RemoveRange(_context.Estimates.Where(x => x.ArticleId == article.Id));
            _context.Comments.RemoveRange(_context.Comments.Where(x => x.ArticleId == article.Id));
            _context.SaveChanges();
            return RedirectToAction($"Index", "Profile", new { id = User.Identity.GetUserId() });
        }

    }
}
