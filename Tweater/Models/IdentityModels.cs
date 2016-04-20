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


        [StringLength(20, ErrorMessage = "A Username can be at most 20 characters long.")]
        public string UserHandle { get; set; }

        [ForeignKey("Following")]
        public ICollection<TweaterUser> Following { get; set; } = new List<TweaterUser>();
        [ForeignKey("Followers")]
        public ICollection<TweaterUser> Followers { get; set;} = new List<TweaterUser>();

        public ICollection<Tweat> Tweats { get; set; } = new List<Tweat>();
    }


    public class Tweat
    {
        public int Id { get; set; }
        public TweaterUser Author { get; set; }
        [StringLength(140, ErrorMessage = "The Tweat can be at most 140 characters long.")]
        public string Body { get; set; }
        public DateTime CreateDate { get; set; }

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
    }
}