using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Dist23MVC.Models;

namespace Dist23MVC.Helpers
{
    public static class MailHelper
    {
        private static SmtpClient client = new SmtpClient("relay-hosting.secureserver.net");
        public static bool SendEmail(string textBody, string nameFrom, string emailFrom, string destination)
        {
            string body = nameFrom + " at " + emailFrom + " wrote <br/>";
            body += textBody;
            string emailTo = GetDestinationEmail(destination);
            MailMessage mail = new MailMessage(emailFrom, emailTo);
            mail.Subject = "Email from website from " + nameFrom;
            mail.IsBodyHtml = true;
            mail.Body = body;
            try
            {
                client.Send(mail);
                SendConfirm(mail.From);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private static string GetDestinationEmail(string emailTo)
        {
            switch (emailTo)
            {
                case "I need help":
                    return LookupEmail("general","contact");
                case "Webmaster":
                    return LookupEmail("webmaster", "contact");
                case "DCM (District Committee Member)":
                    return LookupEmail("DCM", "contact");
                case "Public Information":
                    return LookupEmail("PI", "contact");
                case "Treatment Chair":
                    return LookupEmail("Treatment", "contact");
                case "Corrections Chair":
                    return LookupEmail("Corrections", "contact");
                case "I am a professional needing information":
                    return LookupEmail("CPC", "contact");
            }
            return "";
        }

        private static string LookupEmail(string type,string table)
        {
            var emailAddVar = "";
            using (Dist23Data db = new Dist23Data())
            {
                if (table == "contact")
                {
                    emailAddVar = db.Database.SqlQuery<string>("SELECT email FROM contacts WHERE committeeName='" + type + "'").FirstOrDefault();
                }
            }
            return emailAddVar;
        }

        private static void SendConfirm(MailAddress to)
        {
            MailMessage mail = new MailMessage(new MailAddress("no-reply@easternshoreaa.org"),to);
            mail.Subject = "Thanks for contacting AA District 11";
            mail.Body = BuildBody("confirm");
            client.Send(mail);
        }

        private static string BuildBody(string type)
        {
            switch (type)
            {
                case "confirm":
                     string emailStr = "";
                     emailStr += "<p>Thanks for contacting District 23 AA.</p>";
                     emailStr += "<p>Someone will get back to you as soon as possible</p><br/>";
                     emailStr += "<p>Thanks,<br>";
                     emailStr += "District 23 AA";
                     return emailStr;
                    break;
            }
            return "";
        }
    }
}

//ses-smtp-user.20151206-074104
//SMTP Username:
//AKIAJNZBFYLLZHC3NPIQ
//SMTP Password:
//AqZ2UqLaCgyLYWM+bLngTJlrluc8NH3J5+mwShqwYTKP