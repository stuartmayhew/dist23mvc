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
    public class CommLinksController : Controller
    {
        private Dist23Data db = new Dist23Data();

        // GET: CommLinks/Create
        public ActionResult CommLinkCreate(int commKey)
        {
            CommLinks newCommLink = new CommLinks();
            newCommLink.CommKey = commKey;
            newCommLink.DistKey = GlobalVariables.DistKey;
            return View(newCommLink);
        }

        // POST: CommLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommLinkCreate([Bind(Include = "pKey,DistKey,CommKey,LinkTitle,LinkHeader,LinkText,LinkURL")] CommLinks commLinks)
        {
            commLinks.DistKey = GlobalVariables.DistKey;
            if (ModelState.IsValid)
            {
                db.CommLinks.Add(commLinks);
                db.SaveChanges();
                return RedirectToAction("CommLinks", "CommHeaders", new { id = commLinks.CommKey });
            }

            return View(commLinks);
        }

        // GET: CommLinks/Edit/5
        public ActionResult CommLinkEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommLinks commLinks = db.CommLinks.Find(id);
            if (commLinks == null)
            {
                return HttpNotFound();
            }
            return View(commLinks);
        }

        // POST: CommLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommLinkEdit([Bind(Include = "pKey,DistKey,CommKey,LinkTitle,LinkHeader,LinkText,LinkURL")] CommLinks commLinks)
        {
            if (ModelState.IsValid)
            {
                commLinks.DistKey = GlobalVariables.DistKey;
                db.Entry(commLinks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CommLinks","CommHeaders",new { id = commLinks.CommKey });
            }
            return View(commLinks);
        }

        // GET: CommLinks/Delete/5
        public ActionResult CommLinkDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommLinks commLinks = db.CommLinks.Find(id);
            if (commLinks == null)
            {
                return HttpNotFound();
            }
            return View(commLinks);
        }

        // POST: CommLinks/Delete/5
        [HttpPost, ActionName("CommLinkDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CommLinks commLinks = db.CommLinks.Find(id);
            db.CommLinks.Remove(commLinks);
            db.SaveChanges();
            return RedirectToAction("CommIndex");
        }

        public ActionResult Volunteer(FormCollection fData)
        {
            string emailFrom = fData["reqEmail"].ToString();
            string nameFrom = fData["reqName"].ToString();
            string reqPhone = fData["reqPhone"].ToString();
            string commTitle = fData[5].ToString();
            string commKey = fData[4].ToString();
            
            string mailBody = "Volunteer for " + commTitle + " from " + nameFrom + " email:" + emailFrom + " phone:" + reqPhone;
            if (Helpers.MailHelper.SendEmail(mailBody, nameFrom, emailFrom, commTitle,"Volunteer"))
            {
                ViewBag.LoginReq = "Request sent. You'll here from us";
            }
            else
            {
                ViewBag.LoginReq = "Request failed, try again later.";
            }
            return RedirectToAction("CommLinks", "CommHeaders",new { id = Int32.Parse(commKey) });
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
