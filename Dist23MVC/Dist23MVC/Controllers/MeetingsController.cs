using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dist23MVC.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web.UI;
using System.Configuration;
using System.Data.SqlClient;



namespace Dist23MVC.Controllers
{
    public class MeetingsController : Controller
    {
        private Dist23Data db = new Dist23Data();
        static clsDataGetter dg;

        public ActionResult Meetings_Read([DataSourceRequest]DataSourceRequest request)
        {
            return Json(GetMeetings().ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<MeetingsList> GetMeetings()
        {
            var idParam = new SqlParameter {
                ParameterName = "DistKey",
                Value = int.Parse(Session["currDist"].ToString())
            };

            IEnumerable<MeetingsList> results = db.Database.SqlQuery<MeetingsList>(
                "sp_GetMeetings @DistKey",idParam).ToList();

            return results;
        }

        // GET: Meetings
        public ActionResult MeetingsIndex()
        {
            IEnumerable<MeetingsList> results = db.Database.SqlQuery<MeetingsList>(
                "sp_GetMeetings  @DistKey", Session["currDist"].ToString());
                //new SqlParameter("@DistKey", Session["currDist"].ToString())
                //).ToList();
            return View(results);
        }

        public ActionResult MeetingsEdit() 
        {
            if (!Dist23MVC.Helpers.LoginHelpers.isLoggedIn())
            {
                return View("../Login/Login");
            }
            IEnumerable<Meetings> results = db.Meetings.ToList();
            return View(results);
        }

        // GET: Meetings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            return View(meetings);
        }

        // GET: Meetings/Create
        public ActionResult MeetingCreate()
        {
            var aaGroup = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.aaGroup,
                Text = x.aaGroup,
            }).Distinct();
            ViewBag.aaGroup = aaGroup;

            var location = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.location,
                Text = x.location,
            }).Distinct();
            ViewBag.location = location;
            
            var Day = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.Day,
                Text = x.Day,
            }).Distinct();
            ViewBag.Day = Day;

            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeetingCreate([Bind(Include = "pKey,Day,Time,type,topic,aaGroup,location,city")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Meetings.Add(meetings);
                db.SaveChanges();
                return RedirectToAction("MeetingsEdit");
            }

            return View(meetings);
        }

        // GET: Meetings/Edit/5
        public ActionResult MeetingEdit(int? id)
        {
            var aaGroup = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.aaGroup,
                Text = x.aaGroup,
            }).Distinct();
            ViewBag.aaGroup = aaGroup;

            var location = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.location,
                Text = x.location,
            }).Distinct();
            ViewBag.location = location;

            var Day = db.Meetings.Select(x => new SelectListItem
            {
                Value = x.Day,
                Text = x.Day,
            }).Distinct();
            ViewBag.Day = Day;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            return View(meetings);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MeetingEdit([Bind(Include = "pKey,Day,Time,type,topic,aaGroup,location,city")] Meetings meetings)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meetings).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MeetingsEdit");
            }
            return View(meetings);
        }

        // GET: Meetings/Delete/5
        public ActionResult MeetingDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Meetings meetings = db.Meetings.Find(id);
            if (meetings == null)
            {
                return HttpNotFound();
            }
            db.Meetings.Remove(meetings);
            db.SaveChanges();
            return RedirectToAction("MeetingsEdit");
        }

        private void MakeMeetingPDF()
        {
            dg = new clsDataGetter(db.Database.Connection.ConnectionString);
            SqlDataReader dr = dg.GetDataReader("EXEC sp_GetMeetingsPrint");
            string mtgList = "";
            //while (dr.Read())
            //{
            //    string mtgLine = FormatLine(dr);
            //    //for (int i = 0; i < dr.FieldCount; i++)
            //    //{
            //    //    mtgLine += dr[i].ToString() + " - ";
            //    //}
            //    mtgList += mtgLine;
            //}
            //dg.KillReader(dr);
            FillPDFForm(Server.MapPath("~\\upload") + "\\district_23.pdf", Server.MapPath("~\\upload")+ "\\district_23101.pdf", dr);
            DisplayDocumentOrErrorPage(Server.MapPath("~\\upload") + "\\district_23101.pdf");
        }

        public static void FillPDFForm(string spath, string destpath, SqlDataReader dr)
        {
            using (var fs = new FileStream(destpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                var reader = new iTextSharp.text.pdf.PdfReader(new RandomAccessFileOrArray(spath), null);
                replaceFields(reader, fs, dr);
            }
        }

        private static void replaceFields(iTextSharp.text.pdf.PdfReader reader, FileStream fs, SqlDataReader dr)
        {
            using (var pdfStamper = new PdfStamper(reader, fs))
            {
                int i = 0;
                while (dr.Read())
                {
                    i++;

                    if (i > 1)
                        break;
                    TextField field = new TextField(pdfStamper.Writer, new iTextSharp.text.Rectangle(40, 500, 360, 530), "Day" + i.ToString());
                    pdfStamper.AddAnnotation(field.GetTextField(), 1);
                    pdfStamper.AcroFields.SetField("Day" + i.ToString(), FormatField(dr["Day"].ToString(), "Day"));




                    //pdfStamper.AcroFields.SetField("Time" + i.ToString(), FormatField(dr["Time"].ToString(), "Time"));
                    //pdfStamper.AcroFields.SetField("Type" + i.ToString(), FormatField(dr["Type"].ToString(), "Type"));
                    //pdfStamper.AcroFields.SetField("Topic" + i.ToString(), FormatField(dr["Topic"].ToString(), "Topic"));
                    //pdfStamper.AcroFields.SetField("Group" + i.ToString(), FormatField(dr["aaGroup"].ToString(), "aaGroup"));
                    //pdfStamper.AcroFields.SetField("Location" + i.ToString(), FormatField(dr["Location"].ToString(), "Location"));
                }
                pdfStamper.FormFlattening = true;
                pdfStamper.Close();
                reader.Close();
            }

        }

        public static PdfStamper AddField(string source, string dest)
        {
            PdfReader reader = new PdfReader(source);

            FileStream output = new FileStream(dest, FileMode.Create, FileAccess.Write);

            PdfStamper stamp = new PdfStamper(reader, output);

            return stamp;


        }

        protected void DisplayDocumentOrErrorPage(string FilePath)
        {
            if (String.IsNullOrWhiteSpace(FilePath))
            {
                //RedirectToErrorPage();
            }
            else
            {
                try
                {
                    byte[] pdfBytes = System.IO.File.ReadAllBytes(FilePath);
                    if (pdfBytes.GetLength(0) == 0)
                    {
                        throw new Exception();
                    }
                    Response.ClearContent();
                    Response.ClearHeaders();

                    Response.AddHeader("Content-disposition", "attachment; filename=" +
                                       FilePath);
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(pdfBytes);
                    Response.End();
                }
                catch (Exception ex)
                {
                    //RedirectToErrorPage();
                }
            }
        }
        public void DownloadMeetings()
        {
            MakeMeetingPDF();
        }

        private string FormatLine(SqlDataReader dr)
        {
            string mtgLine = "";
            for (int i = 0; i < dr.FieldCount; i++)
            {
                mtgLine += FormatField(dr[i].ToString(), dr.GetName(i));
            }

            return mtgLine + "\n\r";

        }
        private static string FormatField(string fieldData,string fieldName)
        {
            return fieldData;
            //bool mustSplit = false;
            //int maxLength = dg.GetScalarInteger("SELECT MAX(LEN(" + fieldName + ")) FROM meetings");
            //if (maxLength > 15 && fieldData.Contains(" "))
            //    mustSplit = true;

            //string newStr = "";
            //if (mustSplit)
            //{
            //    string[] strs = fieldData.Split(' ');
            //    int wordCount = strs.Length;
            //    decimal splitDec = wordCount / 2;
            //    int splitSpot = ((int)Math.Floor(splitDec) - 1);
            //    strs[splitSpot] += "\n\r";
            //    for (int x = 0;x < wordCount; x++)
            //    {
            //        newStr += strs[x];
            //    }
            
            //}
            //else
            //    newStr = fieldData.PadRight(maxLength);
            //return newStr;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
