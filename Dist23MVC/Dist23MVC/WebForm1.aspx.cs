using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dist23MVC.Helpers;
using Google.GData.Calendar;

namespace Dist23MVC
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GCalendar cal = new GCalendar("District Twentythree", "district23hotline@gmail.com", "billwilson12");
            Helpers.Calendar [] events = cal.GetEvents();

        }
    }
}