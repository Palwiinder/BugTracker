using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc2.Models.ViewModel
{
    public class DashBoardViewModel
    {
        public int Project { get; set; }
        public int Tickets { get; set; }
        public int Open { get; set; }
        public int Resolved { get; set; }
        public int Rejected { get; set; }
    }
}