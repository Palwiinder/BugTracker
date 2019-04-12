using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.Domain
{
    public class Project
    {
        public int Projectid { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public virtual List<Ticket> Tickets { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateCreated { get; set; }
        public Project()
        {
            DateCreated = DateTime.Now;
            Users = new List<ApplicationUser>();
        }
        public DateTime? DateUpdated { get; set; }
    }
}