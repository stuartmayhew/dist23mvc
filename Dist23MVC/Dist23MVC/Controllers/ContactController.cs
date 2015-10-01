using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using Dist23MVC.Helpers;

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
            if (!MailHelper.SendEmail(body, nameFrom, emailFrom, destination))
            {
                ViewBag.Status = "Sorry, we couldn't send your note. Call the hotline at (251)301-6773 if you need help now!";
            }
            else
            {
                ViewBag.Status = "Mail successfully sent.";
            }
            return View("Contact");
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