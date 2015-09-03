using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Dist23MVC.Models
{
    public class Events
    {
        [Key]
        public int pKey { get; set; }
        public string EventCat { get; set; }
        public string EventName { get; set; }
        public string Eventlink  { get; set; }
        public string EventLinkText { get; set; }
        public string EventCatName { get; set; }
    }
}