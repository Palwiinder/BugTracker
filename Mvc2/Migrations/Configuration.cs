namespace Mvc2.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Mvc2.Models;
    using Mvc2.Models.Domain;
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

            ApplicationUser projectManagerUser;

            if (!context.Users.Any(p => p.UserName == "projectManager@mybugtracker.com"))
            {
                projectManagerUser = new ApplicationUser();
                projectManagerUser.UserName = "projectManager@mybugtracker.com";
                projectManagerUser.Email = "projectManager@mybugtracker.com";
                projectManagerUser.EmailConfirmed = true; //To Test Email if Confirmed or not.
                projectManagerUser.DisplayName = "projectManagerUser";
                userManager.Create(projectManagerUser, "Password-1");
            }
            else
            {
                projectManagerUser = context.Users.First(p => p.UserName == "projectManager@mybugtracker.com");
            }

            if (!userManager.IsInRole(projectManagerUser.Id, "ProjectManager"))
            {
                userManager.AddToRole(projectManagerUser.Id, "ProjectManager");
            }

            ApplicationUser developerUser;

            if (!context.Users.Any(p => p.UserName == "developer@mybugtracker.com"))
            {
                developerUser = new ApplicationUser();
                developerUser.UserName = "developer@mybugtracker.com";
                developerUser.Email = "developer@mybugtracker.com";
                developerUser.EmailConfirmed = true; //To Test Email if Confirmed or not.
                developerUser.DisplayName = "developerUser";
                userManager.Create(developerUser, "Password-1");
            }
            else
            {
                developerUser = context.Users.First(p => p.UserName == "developer@mybugtracker.com");
            }

            if (!userManager.IsInRole(developerUser.Id, "Developer"))
            {
                userManager.AddToRole(developerUser.Id, "Developer");
            }

            ApplicationUser submitterUser;

            if (!context.Users.Any(p => p.UserName == "submitter@mybugtracker.com"))
            {
                submitterUser = new ApplicationUser();
                submitterUser.UserName = "submitter@mybugtracker.com";
                submitterUser.Email = "submitter@mybugtracker.com";
                submitterUser.EmailConfirmed = true; //To Test Email if Confirmed or not.
                submitterUser.DisplayName = "submitterUser";
                userManager.Create(submitterUser, "Password-1");
            }
            else
            {
                submitterUser = context.Users.First(p => p.UserName == "submitter@mybugtracker.com");
            }

            if (!userManager.IsInRole(submitterUser.Id, "Submitter"))
            {
                userManager.AddToRole(submitterUser.Id, "Submitter");
            }

            if (!context.TicketsTypeDatabase.Any(p => p.Name == "Bug"))
            {
                var bug = new TicketType();
                bug.Name = "Bug";
                context.TicketsTypeDatabase.Add(bug);
            }

            if (!context.TicketsTypeDatabase.Any(p => p.Name == "Feature"))
            {
                var feature = new TicketType();
                feature.Name = "Feature";
                context.TicketsTypeDatabase.Add(feature);
            }

            if (!context.TicketsTypeDatabase.Any(p => p.Name == "Database"))
            {
                var database = new TicketType();
                database.Name = "Database";
                context.TicketsTypeDatabase.Add(database);
            }

            if (!context.TicketsTypeDatabase.Any(p => p.Name == "Support"))
            {
                var support = new TicketType();
                support.Name = "Support";
                context.TicketsTypeDatabase.Add(support);
            }

            if (!context.TicketsPriorityDatabase.Any(p => p.Name == "Low"))
            {
                var low = new TicketPriority();
                low.Name = "Low";
                context.TicketsPriorityDatabase.Add(low);
            }

            if (!context.TicketsPriorityDatabase.Any(p => p.Name == "Medium"))
            {
                var medium = new TicketPriority();
                medium.Name = "Medium";
                context.TicketsPriorityDatabase.Add(medium);
            }

            if (!context.TicketsPriorityDatabase.Any(p => p.Name == "High"))
            {
                var high = new TicketPriority();
                high.Name = "High";
                context.TicketsPriorityDatabase.Add(high);
            }

            if (!context.TicketsStatusDatabase.Any(p => p.Name == "Open"))
            {
                var open = new TicketStatus();
                open.Name = "Open";
                context.TicketsStatusDatabase.Add(open);
            }

            if (!context.TicketsStatusDatabase.Any(p => p.Name == "Resolved"))
            {
                var resolved = new TicketStatus();
                resolved.Name = "Resolved";
                context.TicketsStatusDatabase.Add(resolved);
            }

            if (!context.TicketsStatusDatabase.Any(p => p.Name == "Rejected"))
            {
                var rejected = new TicketStatus();
                rejected.Name = "Rejected";
                context.TicketsStatusDatabase.Add(rejected);
            }

            context.SaveChanges();
        }
    }
}
