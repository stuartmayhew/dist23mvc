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
    public class PaymentsController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: Payments
        public ActionResult PaymentsIndex()
        {
            return View(db.Payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult PaymentsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // GET: Payments/Create
        public ActionResult PaymentsCreate(int? id)
        {
            Payments payment = new Payments();
            Events currEvent = db.Events.Find(id);
            ViewBag.EventName = currEvent.EventName;
            payment.DistKey = (int)Session["DistKey"];
            if (id != null)
                payment.EventKey = (int)id;
            payment.PaymentDate = DateTime.Now;
            payment.PaymentType = "event";
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentsCreate([Bind(Include = "pKey,DistKey,EventKey,PaymentType,Name,Address,Phone,Email,Special,PaypalButton,PaypalReturn")] Payments payments)
        {
            if (ModelState.IsValid)
            {

                db.Payments.Add(payments);
                db.SaveChanges();
                return RedirectToAction("PaymentsIndex");
            }

            return View(payments);
        }

        // GET: Payments/Delete/5
        public ActionResult PaymentsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payments payments = db.Payments.Find(id);
            if (payments == null)
            {
                return HttpNotFound();
            }
            return View(payments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("PaymentsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payments payments = db.Payments.Find(id);
            db.Payments.Remove(payments);
            db.SaveChanges();
            return RedirectToAction("PaymentsIndex");
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
