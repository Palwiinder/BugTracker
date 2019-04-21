using Mvc2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class CreateAttachmetsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public HttpPostedFileBase Media { get; set; }
        public string MediaUrl { get; set; }

        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}