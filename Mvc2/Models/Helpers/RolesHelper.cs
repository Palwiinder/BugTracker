using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace Mvc2.Models.Helpers
{
    public class RolesHelper
    {
        private ApplicationDbContext context;

        private UserManager<ApplicationUser> UserManager;

        private RoleManager<IdentityRole> RoleManager;

        public RolesHelper()
        {
            context = new ApplicationDbContext();
            UserManager = new
            UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            RoleManager = new
            RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
        }

        public List<IdentityRole> ListUserRoles()
        {
            return RoleManager.Roles.ToList();
        }

        public List<string> UsersInRole(string id)
        {
            return UserManager.GetRoles(id).ToList();
        }
    }
}