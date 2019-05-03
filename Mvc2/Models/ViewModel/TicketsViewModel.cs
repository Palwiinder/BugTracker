﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class TicketsViewModel
    {
        public int? TicketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Project { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
        public string CreatedById { get; set; }

        public virtual ApplicationUser AssignedTo { get; set; }
        public string AssignedToId { get; set; }

    }
}