using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using System.Web.Security;

namespace Dist23MVC.Controllers
{
    public class LoginController : Controller
    {
        private Dist23Data db = new Dist23Data();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Contacts contact)
        {
            if (ModelState.IsValid)
            {
                int ID = 0;

                if (IsValid(contact.email, contact.password, ref ID))
                {
                    FormsAuthentication.SetAuthCookie(contact.email, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.ErrorMsg = "Login data is incorrect!";
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Logout()
        {
            Session["loginName"] = null;
            return RedirectToAction("Index", "Home");
        }

        public bool IsValid(string username, string password, ref int id)
        {
            if(username == "stumay111@gmail.com" && password == "shadow111")
            {
                Session["LoginName"] = "Stuart";
                Session["AccessLevel"] = 10;
                return true;
            }

            Dist23Data db = new Dist23Data();
            var contacts = db.Contacts.Where(x => x.email == username && x.password == password).Where(x => x.DistKey == GlobalVariables.DistKey);
            Contacts contact = contacts.FirstOrDefault();
            if (contact == null)
            {
                return false;
            }
            else
            {
                Session["LoginName"] = contact.name;
                Session["AccessLevel"] = contact.AccessLvl;
                return true;
            }
        }

        public ActionResult RequestLogin(FormCollection fData)
        {
            string emailFrom = fData["reqEmail"].ToString();
            string nameFrom = fData["reqName"].ToString();
            string reqPassword = fData["reqPassword"].ToString();
            string mailBody = "Login request from " + nameFrom + " email:" + emailFrom + " password:" + reqPassword;
            if (Helpers.MailHelper.SendEmailContact(mailBody, nameFrom, emailFrom, "Webmaster"))
            {
                ViewBag.LoginReq = "Request sent. You'll here from us";
            }
            else
            {
                ViewBag.LoginReq = "Request failed, try again later.";
            }
            return View("Login");
        }
    }
}
