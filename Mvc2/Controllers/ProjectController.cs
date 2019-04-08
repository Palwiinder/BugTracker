using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc2.Models;
using Mvc2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Controllers
{
    public class ProjectController : Controller
    {
        // GET: Project

        //Try to Assign Project to Users In The Same Way As I Assign Roles To Users But Not Accomplish SuccessFully At This Point.
        private ApplicationDbContext data = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(data.Users.ToList());
        }

        public ActionResult AssignProjects(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var model = new AssignProjectViewModel();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(data));
            var project = data.ProjectDatabase.FirstOrDefault(p => p.Projectid == id);
            var users = data.Users.ToList();
            var UsersAssigned = project.Users.Select(p => p.Id).ToList();

            var a = new HashSet<string>(UsersAssigned);

            foreach (var user in UsersAssigned)
            {
                if (project.ProjectName.Contains(user))
                {
                    a.Remove(user);
                }
            }
            model.AddProjects = new MultiSelectList(users, "ProjectId", UsersAssigned);

            return View(model);
        }
        [HttpPost]

        public ActionResult AssignProjects(AssignProjectViewModel model)
        {
            var project = data.ProjectDatabase.FirstOrDefault(p => p.Projectid == model.ProjectId);
            var projects = project.Users.ToList();
            foreach(var user in projects)
            {
                project.Users.Remove(user);
        }
        if(model.SelectedAddProjects != null)
            {
                foreach(var userId in model.SelectedAddProjects)
                {
                    var user = data.Users.FirstOrDefault(p => p.Id == userId);
                    project.Users.Add(user);
                }
            }
            data.SaveChanges();
            return RedirectToAction("Index");
            }
    }
}