namespace Mvc2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Mvc2.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;

    internal sealed class Configuration : DbMigrationsConfiguration<Mvc2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Mvc2.Models.ApplicationDbContext";
        }

        protected override void Seed(Mvc2.Models.ApplicationDbContext context)
        {
            var userManager =
               new UserManager<ApplicationUser>(
                       new UserStore<ApplicationUser>(context));

            var roleManager =
               new RoleManager<IdentityRole>(
                   new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            if (!context.Roles.Any(p => p.Name == "ProjectManager"))
            {
                var projectManagerRole = new IdentityRole("ProjectManager");
                roleManager.Create(projectManagerRole);
            }

            if (!context.Roles.Any(p => p.Name == "Developer"))
            {
                var developerRole = new IdentityRole("Developer");
                roleManager.Create(developerRole);
            }

            if (!context.Roles.Any(p => p.Name == "Submitter"))
            {
                var submitterRole = new IdentityRole("Submitter");
                roleManager.Create(submitterRole);
            }

            ApplicationUser adminUser;

            if (!context.Users.Any(p => p.UserName == "admin@mybugtracker.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@mybugtracker.com";
                adminUser.Email = "admin@mybugtracker.com";
                adminUser.EmailConfirmed = true; //To Test Email if Confirmed or not.
                adminUser.DisplayName = "adminUser";
                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context.Users.First(p => p.UserName == "admin@mybugtracker.com");
            }

            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
        }
    }
}
