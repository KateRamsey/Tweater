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
            var model = new TweaterUserVM()
            {
                UserHandle = db.Users.Find(currentUser).UserHandle
            }; 

            return Json(model, JsonRequestBehavior.AllowGet);
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
    }
}