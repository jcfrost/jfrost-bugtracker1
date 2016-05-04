namespace jfrost_bugtracker.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<jfrost_bugtracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(jfrost_bugtracker.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Administrator"))
            {
                roleManager.Create(new IdentityRole { Name = "Administrator" });
            }

            var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "jcfrosty64@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jcfrosty64@gmail.com",
                    Email = "jcfrosty64@gmail.com",
                    FirstName = "James",
                    LastName = "Frost",
                    PhoneNumber = "(336) 210-1008",
                }, "jcf-Bugtracker2016");
            }
            var userId = userManager.FindByEmail("jcfrosty64@gmail.com").Id;
            userManager.AddToRole(userId, "Administrator");

            if (!context.Roles.Any(r => r.Name == "Project Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "projectmanager@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectmanager@coderfoundry.com",
                    Email = "projectmanager@coderfoundry.com",
                    FirstName = "projectmanager",
                    LastName = " ",
                    PhoneNumber = "(XXX) XXX-XXXX",
                }, "pm-Password1");
            }
            userId = userManager.FindByEmail("projectmanager@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Project Manager");

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "developer@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "developer@coderfoundry.com",
                    Email = "developer@coderfoundry.com",
                    FirstName = "developer",
                    LastName = " ",
                    PhoneNumber = "(XXX) XXX-XXXX",
                }, "developer-Password1");
            }
            userId = userManager.FindByEmail("developer@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Developer");

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "submitter@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "submitter@coderfoundry.com",
                    Email = "submitter@coderfoundry.com",
                    FirstName = "submitter",
                    LastName = " ",
                    PhoneNumber = "(XXX) XXX-XXXX",
                }, "submitter-Password1");
            }
            userId = userManager.FindByEmail("submitter@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Submitter");

            if (!context.Roles.Any(r => r.Name == "Guest"))
            {
                roleManager.Create(new IdentityRole { Name = "Guest" });
            }

            userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "guest@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "guest@coderfoundry.com",
                    Email = "guest@coderfoundry.com",
                    FirstName = "guest",
                    LastName = " ",
                    PhoneNumber = "(XXX) XXX-XXXX",
                }, "guest-Password1");
            }
            userId = userManager.FindByEmail("guest@coderfoundry.com").Id;
            userManager.AddToRole(userId, "Guest");

        }
    }
}
