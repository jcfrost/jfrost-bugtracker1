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
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace jfrost_bugtracker.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            //var user = db.Users.Find(User.Identity.GetUserId());

            //var tickets = user.Projects.SelectMany(p => p.Tickets).ToList(); //gets all tickets from projects that user is assigned to

            ////var tickets = db.Tickets.Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            //return View(tickets);
            return View(db.Tickets.ToList());
        }

        // GET: TicketAttachments
        public static class ImageUploadValidator
        {
            public static bool IsWebFriendlyImage(HttpPostedFileBase file)
            {
                // check for actual object
                if (file == null)
                    return false;

                //check size - file must be less than 2 MB and greater than 1 KB
                if (file.ContentLength > 2 * 1024 * 1024 || file.ContentLength < 1024)
                    return false;

                try
                {
                    using (var img = Image.FromStream(file.InputStream))
                    {
                        return ImageFormat.Jpeg.Equals(img.RawFormat) ||
                               ImageFormat.Png.Equals(img.RawFormat) ||
                               ImageFormat.Gif.Equals(img.RawFormat);
                    }
                }


                catch
                {
                    return false;
                }
            }
        }

        // GET: Tickets/AddTicketAttachments
        [Authorize]
        public ActionResult AddTicketAttachments(int? ticketId)
        {
            if (ticketId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(ticketId);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            var model = new TicketAttachments();
            model.TicketId = tickets.Id;
            return View(model);
        }

        // POST: Tickets/AddTicketAttachments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult AddTicketAttachments([Bind(Include = "Id,Description,TicketId,MediaURL")] TicketAttachments ticketAttachment, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                //restricting the valid file formats to images only
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/img_uploads/"), fileName));
                    ticketAttachment.FileUrl = "~/img_uploads/" + fileName;
                }
                
                ticketAttachment.Created = System.DateTimeOffset.Now;
                db.TicketAttachment.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        // GET: Tickets On My Projects
        //[Authorize]
        [Authorize(Roles = "Administrator, Developer, Project Manager")]
        public ActionResult TicketsonMyProjects()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var tickets = user.Projects.SelectMany(p => p.Tickets).ToList(); //gets all tickets from projects that user is assigned to

            return View(tickets);
        }

        //GET: TicketsOwned **Only for Submitters**
        //[Authorize]
        [Authorize(Roles = "Submitter")]

        public ActionResult TicketsOwned()
        {
            var userId = User.Identity.GetUserId();

            var tickets = db.Tickets.Where(u => u.OwnerUserId == userId).ToList(); //gets all tickets where userid = OwnerUserId

            //var tickets = db.Tickets.Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets);
        }

        //GET: TicketsAssigned **Only for Developers**
        //[Authorize]
        [Authorize(Roles = "Developer")]

        public ActionResult TicketsAssigned()
        {
            var userId = User.Identity.GetUserId();

            var tickets = db.Tickets.Where(u => u.AssignedToUserId == userId).ToList(); //gets all tickets where userid = OwnerUserId

            //var tickets = db.Tickets.Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize]
        public ActionResult Create(int? id)  //add "(string? name)??
        {
            //model.OwnerUserId = 
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name");
            var ticket = new Tickets();
            //ticket.ProjectId = (int)id;
            //ticket.Project.Name = (string)name;
            return View(ticket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Created = System.DateTimeOffset.Now;
                tickets.OwnerUserId = User.Identity.GetUserId();
                db.Tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/CreateTicketComments
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateTicketComment(TicketComments ticketComment)
        {
            if (ModelState.IsValid)
            {
                ticketComment.UserId = User.Identity.GetUserId();
                ticketComment.Created = System.DateTimeOffset.Now;
                //comment.Updated = System.DateTimeOffset.Now;
                db.TicketComment.Add(ticketComment);
                db.SaveChanges();

            }
            //var blog = db.Tickets.Find(ticketComment.TicketId);
            return RedirectToAction("Index", "Tickets");
        }

        
        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);
            ViewBag.AssignedToUserId = new SelectList(db.Users.Where(x=> x.Roles.Select(y=>y.RoleId).Contains (db.Roles.FirstOrDefault(z=>z.Name =="Developer").Id)).ToList(), "Id", "DisplayName");
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,Updated,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                tickets.Updated = System.DateTimeOffset.Now;
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", tickets.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriority, "Id", "Name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketType, "Id", "Name", tickets.TicketTypeId);
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "DisplayName");
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.Tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.Tickets.Find(id);
            db.Tickets.Remove(tickets);
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
