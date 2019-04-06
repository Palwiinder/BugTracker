using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class ShowProjectViewModel
    {
        public string ProjectName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}