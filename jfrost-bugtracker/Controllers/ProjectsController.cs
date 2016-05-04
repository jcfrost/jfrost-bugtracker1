using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jfrost_bugtracker.Models;
using Microsoft.AspNet.Identity;

namespace jfrost_bugtracker.Controllers
{
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //need to display list of users
        public ActionResult Users()
        {
            var user = db.Users.ToList();
            return View(user); //return View(db.Users.ToList());
        }

        //GET: Projects/AssignUsersToProjects
        [Authorize(Roles = "Administrator, Project Manager")]
        public ActionResult AssignUsersToProj (int id)
        {
            var project = db.Projects.Find(id);//change user to project
            ProjectHelper helper = new ProjectHelper(db);
            var model = new ProjUsersViewModel();
            ViewBag.currentUsers = helper.UsersOnProj(id);
            model.Project = project;

            model.Selected = helper.UsersOnProj(id).Select(n => n.Id).ToArray();
            model.Users = new MultiSelectList(db.Users, "Id", "DisplayName", model.Selected);

            return View(model);
        }

        //POST: Projects/AssignUsersToProjects
        [Authorize(Roles = "Administrator, Project Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignUsersToProj(ProjUsersViewModel model)
        {
            //if (ModelState.IsValid)


            var project = db.Projects.Find(model.Project.Id);
            ProjectHelper helper = new ProjectHelper(db);
            foreach (var users in db.Users.Select(u => u.Id).ToList()) //"users" or "project"??
            {
                helper.RemoveUserFromProj(users, project.Id);
            }
            if (model.Selected != null)
            {
                foreach (var users in model.Selected) //"users" or "project"??
                {
                    helper.AddUserToProj(users, project.Id);
                }
            }
            return RedirectToAction("Index", "Projects");


            //return View();

        }


        // GET: Projects
        //[Authorize]
        [Authorize(Roles = "Administrator, Developer, Project Manager")]
        public ActionResult Index()
        {
            //var user = db.Users.Find(User.Identity.GetUserId());

            //var projects = user.Projects.Select(p => p.Name).ToList(); //gets all projects that user is assigned to

            //return View(projects);

            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        [Authorize]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Name, ProjectStatus")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                projects.ProjectStatus = "OPEN";
                db.Projects.Add(projects);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projects);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ProjectStatus")] Projects projects)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projects).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projects);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Projects projects = db.Projects.Find(id);
            if (projects == null)
            {
                return HttpNotFound();
            }
            return View(projects);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Projects projects = db.Projects.Find(id);
            db.Projects.Remove(projects);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
