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
    public class PaymentSetupsController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: PaymentSetups
        public ActionResult PaymentSetupsIndex()
        {
            return View(db.PaymentSetups.ToList());
        }

        // GET: PaymentSetups/Details/5
        public ActionResult PaymentSetupsDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSetup paymentSetup = db.PaymentSetups.Find(id);
            if (paymentSetup == null)
            {
                return HttpNotFound();
            }
            return View(paymentSetup);
        }

        // GET: PaymentSetups/Create
        public ActionResult PaymentSetupsCreate()
        {
            return View();
        }

        // POST: PaymentSetups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSetupsCreate([Bind(Include = "pKey,DistKey,EventKey,Amount,hasSpecial,SpecialLable")] PaymentSetup paymentSetup)
        {
            if (ModelState.IsValid)
            {
                db.PaymentSetups.Add(paymentSetup);
                db.SaveChanges();
                if (paymentSetup.hasSpecial)
                    return RedirectToAction("PaymentSpecCreate",new { id = paymentSetup.pKey });
                return RedirectToAction("PaymentSetupsIndex");
            }

            return View(paymentSetup);
        }

        public ActionResult PaymentSpecCreate(int id)
        {
            PaymentSpecValues specVal = new PaymentSpecValues();
            specVal.SpecialKey = id;
            PaymentSetup paymentSetup = db.PaymentSetups.Find(id);
            paymentSetup.paymentSpecValues.Add(specVal);
            return View("PaymentSetupsEdit", paymentSetup);
        }

        // GET: PaymentSetups/Edit/5
        public ActionResult PaymentSetupsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSetup paymentSetup = db.PaymentSetups.Find(id);
            if (paymentSetup == null)
            {
                return HttpNotFound();
            }
            return View(paymentSetup);
        }

        // POST: PaymentSetups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentSetupsEdit([Bind(Include = "pKey,DistKey,EventKey,Amount,hasSpecial,SpecialLable")] PaymentSetup paymentSetup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentSetup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PaymentSetupsIndex");
            }
            return View(paymentSetup);
        }

        // GET: PaymentSetups/Delete/5
        public ActionResult PaymentSetupsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentSetup paymentSetup = db.PaymentSetups.Find(id);
            if (paymentSetup == null)
            {
                return HttpNotFound();
            }
            return View(paymentSetup);
        }

        // POST: PaymentSetups/Delete/5
        [HttpPost, ActionName("PaymentSetupsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentSetup paymentSetup = db.PaymentSetups.Find(id);
            db.PaymentSetups.Remove(paymentSetup);
            db.SaveChanges();
            return RedirectToAction("PaymentSetupsIndex");
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
