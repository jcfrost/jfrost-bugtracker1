using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jfrost_bugtracker.Models
{
    public class ProjUsersViewModel
    {
        //private int projectId;
        //private MultiSelectList selectList;

        //public ProjUsersViewModel(int projectId, MultiSelectList selectList)
        //{
        //    this.projectId = projectId;
        //    this.selectList = selectList;
        //}

        public Projects Project { get; set; }

        public MultiSelectList Users { get; set; }
        public string[] Selected { get; set; }
    }
}