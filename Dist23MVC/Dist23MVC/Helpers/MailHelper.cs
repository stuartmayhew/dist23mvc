﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using Dist23MVC.Models;

namespace Dist23MVC.Helpers
{
    public static class MailHelper
    {
        const String HOST = "email-smtp.us-west-2.amazonaws.com";
        static string fromAddress = "noreply@" + GlobalVariables.DomainName;

        // Port we will connect to on the Amazon SES SMTP endpoint. We are choosing port 587 because we will use
        // STARTTLS to encrypt the connection.
        const int PORT = 587;

        const String SMTP_USERNAME = "AKIAJNZBFYLLZHC3NPIQ";  // Replace with your SMTP username. 
        const String SMTP_PASSWORD = "AqZ2UqLaCgyLYWM+bLngTJlrluc8NH3J5+mwShqwYTKP";  // Replace with your SMTP password.

        private static SmtpClient client = new SmtpClient(HOST, PORT);
        public static bool SendEmail(string textBody, string nameFrom, string emailFrom, string destination,bool Lookup = false)
        {
            client.Credentials = new System.Net.NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
            client.EnableSsl = true;
            string body = nameFrom + " at " + emailFrom + " wrote <br/>";
            body += textBody;
            string emailTo = "";
            if (Lookup)
            {
                emailTo = GetDestinationEmail(destination);

            }
            else
            {
                emailTo = GetDestinationEmail(destination);
            }

            MailMessage mail = new MailMessage(fromAddress, emailTo);
            mail.Subject = "Email from website from " + nameFrom;
            mail.IsBodyHtml = true;
            mail.Body = body;
            try
            {
                client.Send(mail);
                SendConfirm(emailFrom);
                return true;
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return false;
            }
        }
        private static string GetDestinationEmail(string emailTo)
        {
            switch (emailTo)
            {
                case "I need help":
                    return LookupEmail("Webmaster", true);
                case "Webmaster":
                    return LookupEmail("Webmaster", true);
                case "DCM (District Committee Member, true)":
                    return LookupEmail("DCM", true);
                case "Public Information":
                    return LookupEmail("Public Information", true);
                case "Treatment Chair":
                    return LookupEmail("Treatment", true);
                case "Corrections Chair":
                case "Corrections Committee":
                    return LookupEmail("Corrections", true);
                case "I am a professional needing information":
                    return LookupEmail("CPC", true);
            }
            return "";
        }

        private static string LookupEmail(string type,bool isDistrict)
        {
            
            using (Dist23Data db = new Dist23Data())
            {
                IQueryable<Contacts> emailAddVar =
                    from contact in db.Contacts
                    join contactPosition in db.ContactPosition on contact.pKey equals contactPosition.ContactID
                    join positions in db.Positions on contactPosition.PositionID equals positions.pKey
                    join groups in db.Groups on contactPosition.DistKey equals groups.DistKey
                    where positions.PositionName == type
                    where contact.DistKey == GlobalVariables.DistKey
                    where groups.isDistrict == true
                    select contact;
                return emailAddVar.FirstOrDefault().email;
                   
                

            }
        }

        private static void SendConfirm(string to)
        {
            MailAddress toAddr = new MailAddress(to);
            MailMessage mail = new MailMessage(new MailAddress(fromAddress), toAddr);
            mail.Subject = "Thanks for contacting AA District " + GlobalVariables.DistNumber;
            mail.Body = BuildBody("confirm");
            mail.IsBodyHtml = true;
            client.Send(mail);
        }

        private static string BuildBody(string type)
        {
            switch (type)
            {
                case "confirm":
                     string emailStr = "";
                     emailStr += "<p>Thanks for contacting District " + GlobalVariables.DistNumber + " AA.</p>";
                     emailStr += "<p>Someone will get back to you as soon as possible</p><br/>";
                     emailStr += "<p>Thanks,<br>";
                     emailStr += "District " + GlobalVariables.DistNumber + " AA";
                     return emailStr;
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