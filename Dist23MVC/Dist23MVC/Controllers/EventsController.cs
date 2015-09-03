using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using System.IO;


namespace Dist23MVC.Controllers
{
    public class EventsController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: Events
        public ActionResult EventsIndex()
        {
            return View(db.Events.ToList());
        }

        // GET: Events/Create
        public ActionResult EventsCreate()
        {
            if (!Helpers.LoginHelpers.isLoggedIn())
                return RedirectToAction("Login", "Login");            
            ViewBag.EventCatList = BuildEventCatList();
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventsCreate([Bind(Include = "pKey,EventCat,EventName,Eventlink,EventLinkText")] Events events)
        {
            if (ModelState.IsValid)
            {
                if (Session["currFile"] != null)
                {
                    events.Eventlink = Session["currFile"].ToString();
                }
                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("EventsIndex");
            }

            return View(events);
        }

        // GET: Events/Edit/5
        public ActionResult EventsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["currEventKey"] = id;
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            if (Session["currFile"] != null)
            {
                events.Eventlink = Session["currFile"].ToString();
            }
            ViewBag.EventCatList = BuildEventCatList();
            return View(events);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EventsEdit([Bind(Include = "pKey,EventCat,EventName,Eventlink,EventLinkText")] Events events)
        {
            if (ModelState.IsValid)
            {
                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("EventsIndex");
            }
            return View(events);
        }

        // GET: Events/Delete/5
        public ActionResult EventsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Events events = db.Events.Find(id);
            if (events == null)
            {
                return HttpNotFound();
            }
            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("EventsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Events events = db.Events.Find(id);
            db.Events.Remove(events);
            db.SaveChanges();
            return RedirectToAction("EventsIndex");
        }

        [HttpPost]
        public ActionResult UploadEdit()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/upload/"), fileName);
                    file.SaveAs(path);
                    Session["currFile"] = "http://www.easternshoreaa.org/upload/" + fileName;
                }
            }

            return RedirectToAction("EventsEdit" + "/" + Session["currEventKey"].ToString());
        }

        [HttpPost]
        public ActionResult UploadCreate()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/upload/"), fileName);
                    file.SaveAs(path);
                    Session["currFile"] = "http://www.easternshoreaa.org/upload/" + fileName;
                }
            }

            return RedirectToAction("EventsCreate");
        }

        List<SelectListItem> BuildEventCatList()
        {

            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "District 23", Value = "D23" });

            items.Add(new SelectListItem { Text = "District 12", Value = "D12" });

            items.Add(new SelectListItem { Text = "District 19", Value = "D19", Selected = true });

            items.Add(new SelectListItem { Text = "Area 1", Value = "AREA1" });

            items.Add(new SelectListItem { Text = "ALCYPAA", Value = "ALC" });

            items.Add(new SelectListItem { Text = "SWACO", Value = "SWACO" });
            
            items.Add(new SelectListItem { Text = "National", Value = "NATL" });

            return items;
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
