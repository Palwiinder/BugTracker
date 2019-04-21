using Mvc2.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc2.Models.ViewModel
{
    public class CreateTicketViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int TypeId { get; set; }
        public List<SelectListItem> TicketType { get; set; }
        public int PriorityId { get; set; }
        public List<SelectListItem> TicketPriority { get; set; }
        public int StatusId { get; set; }
        public List<SelectListItem> TicketStatus { get; set; }
        public int ProjectId { get; set; }
        public string UserName { get; set; }
        
        public int Id { get; set; }
        public string TicketDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public HttpPostedFileBase Media { get; set; }
        public string MediaUrl { get; set; }

        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

    }
}