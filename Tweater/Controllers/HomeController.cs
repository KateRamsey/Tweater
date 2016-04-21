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
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserId();
            var user = db.Users.Find(currentUser);
            var model = user.UserHandle;
            return View();
        }

    }
}