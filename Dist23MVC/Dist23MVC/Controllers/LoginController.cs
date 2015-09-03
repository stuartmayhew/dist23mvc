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
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                int ID = 0;

                if (IsValid(login.username,login.password, ref ID))
                {
                    FormsAuthentication.SetAuthCookie(login.username, false);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
            ModelState.AddModelError("", "Login data is incorrect!");
        }

        public bool IsValid(string username, string password, ref int id)
        {
            Dist23Data db = new Dist23Data();
            var logins = db.Login.Where(x => x.username == username && x.password == password);
            Login login = logins.FirstOrDefault();
            if (login == null)
            {
                return false;
            }
            else
            {
                Session["LoginName"] = login.Name;
                return true;
            }
        }
    }
}
