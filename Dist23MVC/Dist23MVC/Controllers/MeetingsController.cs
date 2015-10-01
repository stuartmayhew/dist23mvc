using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;


namespace Dist23MVC.Controllers
{
    public class MeetingsController : Controller
    {
        private Dist23Data db = new Dist23Data();

        public ActionResult Meetings_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetMeetings().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<MeetingsList> GetMeetings()
        {
            IEnumerable<MeetingsList> results = db.Database.SqlQuery<MeetingsList>("sp_GetMeetings").ToList();
            return results;
        }

        // GET: Meetings
        public ActionResult MeetingsIndex()
        {
            IEnumerable<MeetingsList> results = db.Database.SqlQuery<MeetingsList>("sp_GetMeetings").ToList();
            return View(results);
        }

        public ActionResult MeetingsEdit() 
        {
            if (!Dist23MVC.Helpers.LoginHelpers.isLoggedIn())
            {
                return View("../Login/Login");
            }
            IEnumerable<Meetings> results = db.Meetings.ToList();
            return View(results);
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            return View(meetings);
        }

        // GET: Meetings/Create
        public ActionResult MeetingCreate()
        {
            var aaGroup = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.aaGroup,
                Text = x.aaGroup,
            }).Distinct();
            ViewBag.aaGroup = aaGroup;

            var location = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.location,
                Text = x.location,
            }).Distinct();
            ViewBag.location = location;
            
            var Day = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.Day,
                Text = x.Day,
            }).Distinct();
            ViewBag.Day = Day;

            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeetingCreate([Bind(Include = "pKey,Day,Time,type,topic,aaGroup,location,city")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Meetings.Add(meetings);
                db.SaveChanges();
                return RedirectToAction("MeetingsEdit");
            }

            return View(meetings);
        }

        // GET: Meetings/Edit/5
        public ActionResult MeetingEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            return View(meetings);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeetingEdit([Bind(Include = "pKey,Day,Time,type,topic,aaGroup,location,city")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meetings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MeetingsEdit");
            }
            return View(meetings);
        }

        // GET: Meetings/Delete/5
        public ActionResult MeetingDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            db.Meetings.Remove(meetings);
            db.SaveChanges();
            return RedirectToAction("MeetingsEdit");
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
