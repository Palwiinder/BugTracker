using Mvc2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class ViewTicketViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }

        public  virtual ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }

        public List<TicketComment> Comments { get; set; }
        public List<TicketAttachments> Attachments { get; set; }
    }
}