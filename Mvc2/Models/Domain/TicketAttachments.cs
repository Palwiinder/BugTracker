using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.Domain
{
    public class TicketAttachments
    {
        public TicketAttachments()
        {
            DateCreated = DateTime.Now;
        }

        public int Id { get; set; }
        public string FilePath { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public string MediaUrl { get; set; }

        public virtual Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}