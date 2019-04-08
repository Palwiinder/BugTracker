using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Models.ViewModel
{
    public class RoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Tickets { get; set; }
        public MultiSelectList AddRoles { get; set; }
        public MultiSelectList RemoveRoles { get; set; }
        public string[] SelectedAddRoles { get; set; } = new string[] { };
        public string[] SelectedRemoveRoles { get; set; } = new string[] { };
    }
}