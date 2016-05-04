using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Collections;

namespace jfrost_bugtracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ApplicationUser()
        {
            this.TicketComment = new HashSet<TicketComments>();
            this.Projects = new HashSet<Projects>();
        }

        public virtual ICollection<TicketComments> TicketComment { get; set; }
        public virtual ICollection<Projects> Projects { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<TicketComments> TicketComment { get; set; }
        public DbSet<TicketAttachments> TicketAttachment { get; set; }
        public DbSet<TicketHistories> TicketHistory { get; set; }
        public DbSet<TicketNotifications> TicketNotification { get; set; }
        public DbSet<TicketPriorities> TicketPriority { get; set; }
        public DbSet<TicketStatuses> TicketStatus { get; set; }
        public DbSet<TicketTypes> TicketType { get; set; }

        public DbSet<Projects> Projects { get; set; }
    }
}