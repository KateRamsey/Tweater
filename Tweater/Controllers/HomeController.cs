using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Tweater.Models;

namespace Tweater.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [AllowAnonymous]
        public ActionResult Index(int? pagenumber)
        {
            var currentUser = User.Identity.GetUserId();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userFromDB = db.Users.Find(currentUser);

            if (userFromDB.Following.Count == 0)
            {
                return RedirectToAction("Users");
            }

            var pn = pagenumber.GetValueOrDefault();
            var model = GetTimeLine(currentUser, pn);
            
            while (!model.Any())
            {
                pn--;
                model = GetTimeLine(currentUser, pn);
            }
            ViewBag.PageNumber = pn;

            return View(model);
        }

        public List<TweatVM> GetTimeLine(string userId, int pageNum)
        {
            var currentUser = db.Users.Find(userId);
            if (currentUser == null)
            {
                return null;
            }

            var Ids = currentUser.Following.Select(f => f.Id).ToList();
            Ids.Add(currentUser.Id);

            var timeLineTweats = new List<TweatVM>();

            var tweatsInOrder = db.Tweats.OrderByDescending(v => v.CreateDate).Where(x => Ids.Contains(x.Author.Id)).Skip(20*pageNum).Take(20);

            foreach (var t in tweatsInOrder)
            {
                var n = new TweatVM()
                {
                    AuthorHandle = t.Author.UserHandle,
                    Body = t.Body,
                    CreateDate = t.CreateDate,
                    Id = t.Id
                };
                timeLineTweats.Add(n);
            }

            return timeLineTweats;
        }

        [HttpGet]
        public ActionResult NewTweat()
        {
            return View();
        }


        [HttpPost]  
        public ActionResult NewTweat(CreateTweatVM tweat)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NewTweat", tweat);
            }
            if (tweat != null)
            {
                var newTweat = new Tweat()
                {
                    Author = db.Users.Find(User.Identity.GetUserId()),
                    Body = tweat.Body,
                    CreateDate = DateTime.Now
                };
                db.Tweats.Add(newTweat);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Users()
        {
            var model = db.Users.Select(x => new TweaterUserVM() {Id = x.Id, UserHandle = x.UserHandle}).ToList();
            model.Remove(model.FirstOrDefault(u => u.Id == User.Identity.GetUserId()));
            return View(model);
        }

        [HttpGet]
        public ActionResult UserProfile(string Id)
        {
            var user = db.Users.FirstOrDefault(x => x.UserHandle == Id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            TweaterUserProfileVM profile = new TweaterUserProfileVM()
            {
                Id = user.Id,
                Followers = user.Followers.Count,
                Following = user.Following.Count,
                TweatCount = user.Tweats.Count,
                UserHandle = user.UserHandle,
                Tweats = user.Tweats.OrderByDescending(x=>x.CreateDate).Select(t => new ProfileTweatVM()
                {
                    CreateDate = t.CreateDate,
                    Body = t.Body,
                    Id = t.Id
                }).ToList()
            };


            return View(profile);
        }

        [HttpGet]
        public ActionResult MyProfile()
        {
            var model = new MyProfileVM();
            //TODO: Map to MyProfileVM
            return View();
        }

        [HttpPost]
        public ActionResult Follow(string Id)
        {
            var toFollowUser = db.Users.Find(Id);
            var user = db.Users.Find(User.Identity.GetUserId());
            if (toFollowUser == null || user == null)
            {
                return HttpNotFound();
            }


            user.Following.Add(toFollowUser);
            toFollowUser.Followers.Add(user);
            db.SaveChanges();
            return Content("ok");
        }

        [HttpPost]
        public ActionResult UnFollow(string Id)
        {
            var toUnFollowUser = db.Users.Find(Id);
            var user = db.Users.Find(User.Identity.GetUserId());
            if (toUnFollowUser == null || user == null)
            {
                return HttpNotFound();
            }


            user.Following.Remove(toUnFollowUser);
            toUnFollowUser.Followers.Remove(user);
            db.SaveChanges();
            return Content("ok");
        }
    }


}