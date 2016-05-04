using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jfrost_bugtracker.Models
{
    public class Projects
    {
        public Projects()
        {
            this.Tickets = new HashSet<Tickets>();
            this.Users = new HashSet<ApplicationUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //public DateTimeOffset Created { get; set; }
        //public DateTimeOffset? Updated { get; set; }
        //[Required]

        [Required]
        [AllowHtml]
        public string ProjectStatus { get; set; }

        public virtual ICollection<Tickets> Tickets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        //public DateTimeOffset Edited { get; internal set; }
    }
}