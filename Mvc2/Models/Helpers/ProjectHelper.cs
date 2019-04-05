using Mvc2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.Helpers
{
    public class ProjectHelper
    {
        private ApplicationDbContext DbContext;

        public ProjectHelper(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Project GetProjectById(int id)
        {
            return DbContext.ProjectDatabase.FirstOrDefault(
                p => p.Projectid == id);
        }
        public List<Project> GetUsersProjects(string userId)
        {
            return DbContext.ProjectDatabase
                .Where(project => project.Users.Any(user => user.Id == userId))
                .ToList();
        }
        public List<Project> GetAllProjects() => DbContext.ProjectDatabase.ToList();
    }
}