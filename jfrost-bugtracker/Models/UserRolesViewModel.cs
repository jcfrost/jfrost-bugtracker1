﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jfrost_bugtracker.Models
{
    public class UserRolesViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public MultiSelectList Roles { get; set; }
        public string[] Selected { get; set; }

    }
}