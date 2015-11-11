using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;

namespace Dist23MVC.Controllers
{
    public class CommHeadersController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: CommHeaders
        public ActionResult CommIndex()
        {
            return View(db.CommHeaders.ToList());
        }

        // GET: CommHeaders/Details/5
        public ActionResult CommLinks(int id)
        {
            CommHeaders comm = db.CommHeaders.Where(h => h.pKey == id).FirstOrDefault();
            ViewBag.CommTitle = comm.CommName;
            ViewBag.CommDetail = comm.CommHeader;
            List<CommLinks> commLinks = db.CommLinks.Where(c => c.CommKey == id).ToList();
            if (commLinks == null)
            {
                return HttpNotFound();
            }
            return View("CommLinks",commLinks);
        }

        // GET: CommHeaders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommHeaders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pKey,DistKey,CommName,CommHeader")] CommHeaders commHeaders)
        {
            if (ModelState.IsValid)
            {
                db.CommHeaders.Add(commHeaders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(commHeaders);
        }

        // GET: CommHeaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommHeaders commHeaders = db.CommHeaders.Find(id);
            if (commHeaders == null)
            {
                return HttpNotFound();
            }
            return View(commHeaders);
        }

        // POST: CommHeaders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pKey,DistKey,CommName,CommHeader")] CommHeaders commHeaders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(commHeaders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(commHeaders);
        }

        // GET: CommHeaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommHeaders commHeaders = db.CommHeaders.Find(id);
            if (commHeaders == null)
            {
                return HttpNotFound();
            }
            return View(commHeaders);
        }

        // POST: CommHeaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommHeaders commHeaders = db.CommHeaders.Find(id);
            db.CommHeaders.Remove(commHeaders);
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
