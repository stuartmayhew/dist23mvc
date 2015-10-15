using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dist23MVC.Models
{
    public class Meetings
    {
        [Key]
        public int pKey { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string type { get; set; }
        public string topic { get; set; }
        public string aaGroup { get; set; }
        public string location { get; set; }
        public string city { get; set; }
        public int DistKey { get; set; }

    }
}