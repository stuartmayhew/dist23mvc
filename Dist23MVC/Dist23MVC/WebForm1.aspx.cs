using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dist23MVC.Helpers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using System.Threading;
using Google.Apis.Services;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Util.Store; // for FileDataStore
using System.Reflection; // for accessing the embedded resource of the json file

namespace Dist23MVC
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        UserCredential credentials;
        CalendarService calendarService;
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //Here is your client ID
        //297371918805-v6qn4shauivr4d3liunnsrmqrd2smc2k.apps.googleusercontent.com
        //Here is your client secret
        //EAYmjTmjl5wYhpU5ruaRWlP4
        protected void Button1_Click(object sender, EventArgs e)
        {
            GetCredentials();

            // Create the service.
            var service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credentials,
                ApplicationName = "Calendar API Sample",
            });

            var myEvent = new Event
            {
                Summary = "Google Calendar Api Sample Code by Mukesh Salaria",
                Location = "Gurdaspur, Punjab, India",
                Start = new EventDateTime
                {
                    DateTime = new DateTime(2015, 3, 2, 6, 0, 0),
                },
                End = new EventDateTime
                {
                    DateTime = new DateTime(2015, 3, 2, 7, 30, 0),
                },
                Recurrence = new String[] { "RRULE:FREQ=WEEKLY;BYDAY=MO" },
            };

            var recurringEvent = service.Events.Insert(myEvent, "primary");
            recurringEvent.SendNotifications = true;
            recurringEvent.Execute();

        }

        private async void GetCredentials()
        {
            try
            {
                GoogleWebAuthorizationBroker.Folder = "Tasks.Auth.Store";
                credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                     GoogleClientSecrets.Load(new System.IO.FileStream("D:\\Downloads\\client_secret.json", System.IO.FileMode.Open)).Secrets,
          new[] {
          CalendarService.Scope.Calendar,
          Google.Apis.Plus.v1.PlusService.Scope.UserinfoEmail },
          "user", CancellationToken.None,
          new FileDataStore("AppDataFolderName"));
            }
            catch (Exception e)
            {

            }
        }
    }
}