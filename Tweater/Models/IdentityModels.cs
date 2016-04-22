using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Tweater.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class TweaterUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<TweaterUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
 
        public string UserHandle { get; set; }

        //[ForeignKey("Following")]
        public virtual ICollection<TweaterUser> Following { get; set; } = new List<TweaterUser>();
        //[ForeignKey("Followers")]
        public virtual ICollection<TweaterUser> Followers { get; set;} = new List<TweaterUser>();

        public virtual ICollection<Tweat> Tweats { get; set; } = new List<Tweat>();
    }

    public class TweaterUserVM
    {
        [StringLength(20, ErrorMessage = "A Username can be at most 20 characters long.")]
        public string UserHandle { get; set; }
    }


    public class TweaterUserProfileVM
    {
        public string UserHandle { get; set; }
        public int Followers { get; set; }
        public int Following { get; set; }
        public int TweatCount { get; set; }

        public List<ProfileTweatVM> Tweats { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<TweaterUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Tweater.Models.Tweat> Tweats { get; set; }
    }
}