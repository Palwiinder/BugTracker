using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mvc2.Models.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Ticket()
        {
            DateCreated = DateTime.Now;
            Comments = new List<TicketComment>();
            Attachments = new List<TicketAttachments>();
            TicketHistory = new List<TicketHistory>();
        }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }
        public virtual TicketType TicketType { get; set; }
        public int TicketTypeId { get; set; }
        public string MediaUrl { get; set; }

        public virtual TicketPriority TicketPriority { get; set; }
        public int TicketPriorityId { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }
        public int TicketStatusId { get; set; }
        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public virtual List<ApplicationUser> SendNotification { get; set; }

        public virtual ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }

        public virtual List<TicketComment> Comments { get; set; }
        public virtual List<TicketAttachments> Attachments { get; set; }
        public virtual List<TicketHistory> TicketHistory { get; set; }
    }
}