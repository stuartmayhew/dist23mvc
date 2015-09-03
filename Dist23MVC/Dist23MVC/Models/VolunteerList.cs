using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dist23MVC.Models
{
    public class VolunteerList
    {
        [Key]
        public int pKey { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "Home Group")]
        public string HomeGroup { get; set; }
        public Nullable<DateTime> LastServed { get; set; }
        public Nullable<DateTime> SobDate { get; set; }
        [Display(Name = "Receive Speaker Emails ")]
        public bool Speaker { get; set; }
        [Display(Name = "Attend Jail Meetings")]
        public bool Jail { get; set; }
        [Display(Name = "Go on 12-Step Calls")]
        public bool TwStep { get; set; }
        [Display(Name = "Give someone a ride to a meeting")]
        public bool Ride { get; set; }
        [Display(Name = "Be on phone list or hotline")]
        public bool PhoneList { get; set; }
        public bool AttendWorkshop { get; set; }
        [Display(Name = "Attent Treatment center meetings")]
        public bool Treatment { get; set; }
        [Display(Name = "Help with special events")]
        public bool SpecialEvents { get; set; }
        public bool GoogleVoice { get; set; }
        public bool onHold { get; set; }
    }
}