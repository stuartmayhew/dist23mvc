﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using System.Data.Entity;
using System.Web.Script.Serialization;

namespace Dist23MVC.Controllers
{
    public class HomeController : Controller
    {
       public Dist23MVC.Models.Dist23Data db = new Dist23Data();

        public ActionResult Index()
        {
            var newsItems = db.News.OrderBy(n => n.ListOrder).ToList();
            return View(newsItems);
        }

        public ActionResult NewsEdit(int ID)
        {
            News news = db.News.Find(ID);
            return View(news);
        }

        [HttpPost]
        public ActionResult NewsEdit(News news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        public ActionResult NewsDelete(int ID)
        {
            News news = db.News.Find(ID);
            if (ModelState.IsValid)
            {
                db.Entry(news).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult NewsCreate()
        {
            News news = new News();
            var eventList = db.Events.Select(x => new SelectListItem
                                    {
                                        Value = x.pKey.ToString(),
                                        Text = x.EventName,
                                    });
            ViewBag.EventKey = eventList;
            return View(news);
        }

        public string GetEvent(string selection)
        {
            int EventKey = int.Parse(selection);
            var Qdata = db.Events.Where(x => x.pKey == EventKey);
            Events data = (Events)Qdata.FirstOrDefault();
            News news = new News();
            news.EventKey = EventKey;
            news.NewsText = data.EventName;
            news.NewsLink = data.Eventlink;
            news.LinkText = data.EventLinkText;
            var json = new JavaScriptSerializer().Serialize(news);
            return json;
        }

        [HttpPost]
        public ActionResult NewsCreate(News news)
        {

            if (ModelState.IsValid)
            {
                db.News.Add(news);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(news);
            }
        }
    }
}