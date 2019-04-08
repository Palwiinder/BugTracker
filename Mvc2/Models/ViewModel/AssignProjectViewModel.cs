using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Models.ViewModel
{
    // View Model For Assigning Projects To Users
    public class AssignProjectViewModel
    {
        public int ProjectId { get; set; }
        public string UserId { get; set; }
        public string Tickets { get; set; }
        public MultiSelectList AddProjects { get; set; }
        public MultiSelectList RemoveProjects { get; set; }
        public string[] SelectedAddProjects { get; set; } = new string[] { };
        public string[] SelectedRemoveProjects { get; set; } = new string[] { };
    }
}