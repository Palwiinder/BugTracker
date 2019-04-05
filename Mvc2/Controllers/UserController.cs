using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Mvc2.Models;
using Mvc2.Models.Helpers;
using Mvc2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View(context.Users.ToList());
        }

        public ActionResult AssignRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(nameof(UserController.Index));
            }
            //var id = User.Identity.GetUserId();
            var model = new RoleViewModel();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var user = userManager.FindById(id);
            model.UserId = id;
            model.UserName = User.Identity.Name;
            var roles = roleManager.Roles.Select(role => role.Name).ToList();
            var userRoles = userManager.GetRoles(id);

            var a = new HashSet<string>(roles);

            foreach (string role in roles)
            {
                if (userRoles.Contains(role))
                {
                    a.Remove(role);
                }
            }

            //var a = roles.Where(role => userRoles.Any(r => r != role)).ToList();

            model.AddRoles = new MultiSelectList(a.ToList(), userRoles);
            if (user.Roles.Any())
            {
                model.RemoveRoles = new MultiSelectList(userRoles, roles);
            }



            //var userRoleHelper = new RolesHelper();
            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}
            //var user = context.Users.FirstOrDefault(p => p.Id == id);
            if (user == null)
            {
                return RedirectToAction(nameof(UserController.Index));
            }
            //model.UserId = id;
            //model.UserName = user.UserName;
            //var roles = userRoleHelper.ListUserRoles();
            //var userRoles = userRoleHelper.UsersInRole(id);

            //model.Roles = new MultiSelectList( "DisplayName", userRoles);

            return View(model);
        }

        [HttpPost]
        public ActionResult AssignRole(RoleViewModel viewModel)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = userManager.FindById(viewModel.UserId);
            var userRoles = userManager.GetRoles(user.Id);

            foreach (var role in viewModel.SelectedRemoveRoles)
            {
                userManager.RemoveFromRole(user.Id, role);
            }

            foreach (var role in viewModel.SelectedAddRoles)
            {
                userManager.AddToRole(user.Id, role);
            }
            return RedirectToAction(nameof(UserController.Index));
        }

    }
}