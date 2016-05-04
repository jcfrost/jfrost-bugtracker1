using jfrost_bugtracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jfrost_bugtracker.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //need to display list of users
        public ActionResult Users()
        {
            var user = db.Users.ToList();
            return View(user); //return View(db.Users.ToList());

        }

        public ActionResult EditUser(string Id)
        {
            var user = db.Users.Find(Id);
            UserRolesHelper helper = new UserRolesHelper(db);
            var model = new UserRolesViewModel();
            model.Name = user.DisplayName;
            model.Id = user.Id;
            model.Selected = helper.ListUserRoles(Id).ToArray();
            model.Roles = new MultiSelectList(db.Roles, "Name", "Name", model.Selected);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditUser(UserRolesViewModel model)
        {
            UserRolesHelper helper = new UserRolesHelper(db);
            foreach (var role in db.Roles.Select(r => r.Name).ToList())
            {
                helper.RemoveUserFromRole(model.Id, role);
            }
            foreach (var role in model.Selected)
            {
                helper.AddUserToRole(model.Id, role);
            }

            return RedirectToAction("Users", "Admin");
        }
    }
}