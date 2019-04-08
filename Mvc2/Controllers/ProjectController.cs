using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc2.Models;
using Mvc2.Models.Domain;
using Mvc2.Models.Helpers;
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

        private ApplicationDbContext DbContext;
        private ProjectHelper ProjectHelper;
        public ProjectController()
        {
            DbContext = new ApplicationDbContext();
            ProjectHelper = new ProjectHelper(DbContext);
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = ProjectHelper.GetUsersProjects(userId)
               .Select(p => new IndexProjectViewModel
               {
                   ProjectId = p.Projectid,
                   ProjectName = p.ProjectName,
                   DateCreated = p.DateCreated,
                   DateUpdated = p.DateUpdated,
                   Users = p.Users ?? new List<ApplicationUser>(),
               }).ToList();

            return View(model);
        }

        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult AllProjects()
        {
            var model = ProjectHelper.GetAllProjects()
               .Select(p => new IndexProjectViewModel
               {
                   ProjectId = p.Projectid,
                   ProjectName = p.ProjectName,
                   DateCreated = p.DateCreated,
                   DateUpdated = p.DateUpdated,
                   Users = p.Users ?? new List<ApplicationUser>(),
               }).ToList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult CreateProject()
        {
            //PopulateViewBag();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult CreateProject(CreateProjectViewModel formData)
        {
            return SaveProject(null, formData);
        }

        private ActionResult SaveProject(int? id, CreateProjectViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Project project;
            var userId = User.Identity.GetUserId();
            if (!id.HasValue)
            {
                project = new Project();
                var applicationUser = DbContext.Users.FirstOrDefault(user => user.Id == userId);
                if (applicationUser == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
                project.Users.Add(applicationUser);
                DbContext.ProjectDatabase.Add(project);
            }
            else
            {
                project = DbContext.ProjectDatabase.FirstOrDefault(p => p.Projectid == id);

                if (project == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            project.ProjectName = formData.ProjectName;
            project.DateUpdated = DateTime.Now;
            DbContext.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var project = DbContext.ProjectDatabase.FirstOrDefault(p => p.Projectid == id);

            if (project == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var model = new CreateProjectViewModel();
            model.ProjectName = project.ProjectName;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Edit(int id, CreateProjectViewModel formData)
        {
            return SaveProject(id, formData);

        }
        [HttpGet]
        public ActionResult FullDetail(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var project = DbContext.ProjectDatabase.FirstOrDefault(p => p.Projectid == id);
            if (project == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var model = new ShowProjectViewModel()
            {
                ProjectName = project.ProjectName,
                DateCreated = project.DateCreated,

            };
            return View(model);
        }



        public ActionResult AssignProjects(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }
            var model = new AssignProjectViewModel();
            var project = DbContext.ProjectDatabase.FirstOrDefault(p => p.Projectid == id);
            var users = DbContext.Users.ToList();
            var UsersAssigned = project.Users.ToList();

            var usersToAdd = new HashSet<ApplicationUser>(UsersAssigned);

            foreach (var user in UsersAssigned)
            {
                if (project.Users.Contains(user))
                {
                    usersToAdd.Remove(user);
                }
            }
            model.AddUsers = new MultiSelectList(users, "Id", "Email", UsersAssigned);

            return View(model);
        }
        [HttpPost]

        public ActionResult AssignProjects(AssignProjectViewModel model)
        {
            var project = DbContext.ProjectDatabase.FirstOrDefault(p => p.Projectid == model.ProjectId);
            if (project == null)
            {
                return RedirectToAction(nameof(UserController.Index));
            }

            if (model.SelectedRemoveUsers != null)
            {
                foreach (var userId in model.SelectedRemoveUsers)
                {
                    var user = DbContext.Users.FirstOrDefault(p => p.Id == userId);
                    project.Users.Remove(user);
                }
            }

            if (model.SelectedAddUsers != null)
            {
                foreach (var userId in model.SelectedAddUsers)
                {
                    var user = DbContext.Users.FirstOrDefault(p => p.Id == userId);
                    project.Users.Add(user);
                }
            }

            DbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}