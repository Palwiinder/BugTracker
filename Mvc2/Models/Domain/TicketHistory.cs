using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.Domain
{
    public class TicketHistory
    {
        public TicketHistory()
        {
            DateChanged = DateTime.Now;
        }

        public int Id { get; set; }
        public string Property { get; set; }

        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime DateChanged { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}