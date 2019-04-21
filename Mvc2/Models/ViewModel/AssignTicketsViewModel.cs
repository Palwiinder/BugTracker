using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Models.ViewModel
{
    public class AssignTicketsViewModel
    {
        public int TicketId { get; set; }
        public SelectList AddUsers { get; set; }
        public SelectList RemoveUsers { get; set; }
        public string SelectedAddUsers { get; set; } 
        public string SelectedRemoveUsers { get; set; } 
    }
}