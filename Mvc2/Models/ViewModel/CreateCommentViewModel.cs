using Mvc2.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class CreateCommentViewModel
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public List<TicketComment> Comments { get; set; }
    }
}