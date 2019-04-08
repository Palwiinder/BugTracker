using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class IndexProjectViewModel
    {
        public string ProjectName { get; set; }
        public string Tickets { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual List<ApplicationUser> Users { get; set; }
        public int ProjectId { get; set; }

    }
}