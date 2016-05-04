using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jfrost_bugtracker.Models
{
    public class TicketNotifications
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public string UserId { get; set; }
    }
}