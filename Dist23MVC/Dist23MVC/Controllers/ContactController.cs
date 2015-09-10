using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Dist23MVC.Models;

namespace Dist23MVC.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMail(FormCollection fData)
        {
            string body = fData["msgBody"].ToString();
            string nameFrom = fData["userName"].ToString();
            string emailFrom = fData["userEmail"].ToString();
            string destination = fData["contactWho"].ToString();
            SendEmail(body, nameFrom, emailFrom, destination);
            return View();

        }

        private bool SendEmail(string textBody, string nameFrom, string emailFrom, string destination)
        {
            string body = nameFrom + " at " + emailFrom + " wrote <br/>";
            body += textBody;
            string emailTo = GetDestinationEmail(destination);
            //SmtpClient client = new SmtpClient("relay-hosting.secureserver.net");
            //MailMessage mail = new MailMessage();
            //mail.From = emailFrom;
            //mail.To = emailTo;
            //mail.Subject = "Email from website from " + nameFrom;
            //mail.IsBodyHtml = true;
            //mail.Body = body;
            //try
            //{
            //    client.Send(mail);
            //    return true;
            //}
            //catch
            //{
 return false;
            //}
        }
        private string GetDestinationEmail(string emailTo)
        {
            switch (emailTo)
            {
                case "I need help":
                    return LookupEmail("general");
                    break;
                case "Webmaster":
                    return LookupEmail("webmaster");
                    break;
                case "DCM (District Committee Member)":
                    return LookupEmail("DCM");
                    break;
                case "Public Information":
                    return LookupEmail("PI");
                    break;
                case "Treatment Chair":
                    return LookupEmail("Treatment");
                    break;
                case "Corrections Chair":
                    return LookupEmail("Corrections");
                    break;
                case "I am a professional needing information":
                    return LookupEmail("CPC");
                    break;
            }
            return "";
        }

        private string LookupEmail(string type)
        {
            using (Dist23Data db = new Dist23Data())
            {
                // string emailAdd = db.Database.SqlQuery("SELECT email FROM Committees WHERE committeeName='" + type + "'");
            }
            return "";
        }

    }
}

//     Dim SmtpServer As New SmtpClient("relay-hosting.secureserver.net")
//     Dim mail As New MailMessage()
//     mail = New MailMessage()
//     mail.From = New MailAddress("noreplay@district11aa.org")
//     mail.To.Add(GetEmail)
//     mail.Subject = "Email from " + tbName.Text + " from AA Website"
//     mail.Body = body






//Private Sub SendEMail()
//     Dim body As String = tbBody.Text + " " + tbName.Text + " " + tbFrom.Text
//     Dim SmtpServer As New SmtpClient("relay-hosting.secureserver.net")
//     Dim mail As New MailMessage()
//     mail = New MailMessage()
//     mail.From = New MailAddress("noreplay@district11aa.org")
//     mail.To.Add(GetEmail)
//     mail.Subject = "Email from " + tbName.Text + " from AA Website"
//     mail.Body = body
//     Try
//         SmtpServer.Send(mail)
//         Label1.Visible = True
//         Label1.Text = "Email sent!"
//         SendConfirm()
//     Catch ex As Exception
//         Label1.Visible = True
//         Label1.Text = "Email failed, contact webmaster <a href='mailto:tyvrymch@rocketmail.com'> here </a>"
//     End Try

// End Sub
// Private Function GetEmail() As String
//     Select Case cbTo.Text
//         Case "Webmaster"
//             Return "tyvrymch@rocketmail.com"
//         Case "DCM (District Committee Member)"
//             Return "tyvrymch@rocketmail.com"
//         Case "Public Information"
//             Return "tyvrymch@rocketmail.com"
//         Case "Treatment Chair"
//             Return "tyvrymch@rocketmail.com"
//         Case "Corrections Chair"
//             Return "tyvrymch@rocketmail.com"
//         Case "I am a professional needing information"
//             Return "tyvrymch@rocketmail.com"
//         Case "I need help"
//             Return "tyvrymch@rocketmail.com"
//     End Select
//     Return ""
// End Function

// Private Sub SendConfirm()
//     Dim body As String = BuildBody()
//     Dim SmtpServer As New SmtpClient("relay-hosting.secureserver.net")
//     Dim mail As New MailMessage()
//     mail = New MailMessage()
//     mail.From = New MailAddress("noreplay@district11aa.org")
//     mail.To.Add(tbFrom.Text)
//     mail.Subject = "Thanks for contacting AA District 11"
//     mail.Body = BuildBody()
//     mail.IsBodyHtml = True
//     SmtpServer.Send(mail)
// End Sub

// Private Function BuildBody() As String
//     Dim emailStr As String = ""
//     emailStr += "<p>Thanks for contacting District 11 AA.</p>"
//     emailStr += "<p>Someone will get back to you as soon as possible</p><br/>"
//     emailStr += "<p>Thanks,<br>"
//     emailStr += "District 11 AA"
//     Return emailStr
// End Function