using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Tweater.Models;

namespace Tweater.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var currentUser = User.Identity.GetUserId();
            var model = GetTimeLine(currentUser);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public List<TweatVM> GetTimeLine(string userId)
        {
            var currentUser = db.Users.Find(userId);
            if (currentUser == null)
            {
                return null;
            }


            var Ids = (List<string>) currentUser.Following.Select(f => f.Id).ToList();
            Ids.Add(currentUser.Id);

            var timeLineTweats = new List<TweatVM>();
            foreach (var t in db.Tweats)
            {
                foreach (var i in Ids)
                {
                    if (t.Author.Id == i)
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
                }
            }

            return timeLineTweats;
        }

        [HttpGet]
        public ActionResult NewTweat()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
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
        public ActionResult UserProfile(string handle)
        {
            var user = db.Users.FirstOrDefault(x => x.UserHandle == handle);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            TweaterUserProfileVM profile = new TweaterUserProfileVM()
            {
                Followers = user.Followers.Count,
                Following = user.Following.Count,
                TweatCount = user.Tweats.Count,
                UserHandle = user.UserHandle,
                Tweats = user.Tweats.Select(t => new ProfileTweatVM()
                {
                    CreateDate = t.CreateDate,
                    Body = t.Body,
                    Id = t.Id
                }).ToList()
        };
            return Json(profile, JsonRequestBehavior.AllowGet);
        }
    }


}