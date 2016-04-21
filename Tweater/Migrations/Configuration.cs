using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Tweater.Models;

namespace Tweater.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Tweater.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Tweater.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var userStore = new UserStore<TweaterUser>(context);
            var userManager = new UserManager<TweaterUser>(userStore);
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            if (context.Tweats.Any())
            {
                return;
            }
            var kate = new TweaterUser() { UserHandle = "kateramsey",
                Email = "kateramsey@live.com",
                UserName = "kateramsey@live.com"};
            userManager.Create(kate, "MyP@ssw0rd!");

            var bruce = new TweaterUser() { UserHandle = "brucewills",
                Email = "kateramseytest@live.com",
                UserName = "kateramseytest@live.com" };
            userManager.Create(bruce, "MyP@ssw0rd!");

            kate.Following.Add(bruce);
             

            kate.Tweats.Add(new Tweat() {CreateDate = DateTime.Now.AddDays(-3), Author = kate, Body="This is my first tweat! Yay!"});
            kate.Tweats.Add(new Tweat() {CreateDate = DateTime.Now.AddDays(-2), Author = kate, Body = "More tweating, yay!" });
            kate.Tweats.Add(new Tweat() {CreateDate = DateTime.Now.AddDays(-1), Author = kate, Body = "Ok, maybe a little over it" });
            bruce.Tweats.Add(new Tweat() {CreateDate = DateTime.Now, Author = bruce, Body = "Kate made me get this, but I'll never use it" });

            context.Tweats.AddRange(kate.Tweats);
            context.Tweats.AddRange(bruce.Tweats);
        }
    }
}
