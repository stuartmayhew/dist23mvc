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
    public class DocumentsController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: Documents
        public ActionResult DocIndex()
        {
            return View(db.Documents.Where(x=>x.DistKey == GlobalVariables.DistKey).ToList());
        }


        // GET: Documents/Create
        public ActionResult DocCreate()
        {
            Documents doc = new Documents();
            doc.DistKey = GlobalVariables.DistKey;
            return View(doc);
        }

        // POST: Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocCreate(Documents documents)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(documents);
                db.SaveChanges();
                Session["currDocKey"] = documents.pKey;
                return RedirectToAction("DocEdit/" + Session["currDocKey"].ToString());
            }

            return View(documents);
        }

        // GET: Documents/Edit/5
        public ActionResult DocEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Documents documents = db.Documents.Find(id);
            if (documents == null)
            {
                return HttpNotFound();
            }
            if (Session["currFile"] != null)
            {
                documents.DocLink = Session["currFile"].ToString();
            }
            return View(documents);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocEdit(Documents documents)
        {
            if (Session["currFile"] != null)
            {
                documents.DocLink = Session["currFile"].ToString();
            }
            documents.DistKey = GlobalVariables.DistKey;
            if (ModelState.IsValid)
            {
                db.Entry(documents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DocIndex");
            }
            return View(documents);
        }

        // GET: Documents/Delete/5
        public ActionResult DocsDelete(int? id)
        {
            Documents documents = db.Documents.Find(id);
            db.Documents.Remove(documents);
            db.SaveChanges();
            return RedirectToAction("DocIndex");
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
