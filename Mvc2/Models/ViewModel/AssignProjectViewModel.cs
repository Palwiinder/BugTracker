using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Models.ViewModel
{
    public class AssignProjectViewModel
    {
        public int ProjectId { get; set; }
        public MultiSelectList AddUsers { get; set; }
        public MultiSelectList RemoveUsers { get; set; }
        public string[] SelectedAddUsers { get; set; } = new string[] { };
        public string[] SelectedRemoveUsers { get; set; } = new string[] { };
    }
}