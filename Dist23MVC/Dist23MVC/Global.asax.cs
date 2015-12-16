using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Configuration;
using System.Data.SqlClient;

namespace Dist23MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Dist23MVC.Models.clsDataGetter dg = new Models.clsDataGetter(ConfigurationManager.ConnectionStrings["Dist23Data"].ConnectionString);
        int DistKey = 0;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        void Application_BeginRequest(Object source, EventArgs e)
        {
            int DistKey;
            if (GlobalVariables.DistNumber != null)
                return;
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;

            string host = FirstRequestInitialisation.Initialise(context);
            //dg.RunCommand("INSERT INTO hosts(HostURL) values('" + host + "')");
            try
            {
                DistKey = dg.GetScalarInteger("SELECT DistKey FROM SiteConfig WHERE DistURL='" + host + "'");
            }
            catch
            {
                DistKey = 23;
            }
            SqlDataReader dr = dg.GetDataReader("SELECT * FROM SiteConfig WHERE DistKey=" + DistKey.ToString());
            while (dr.Read())
            {
                GlobalVariables.DistNumber = DistKey.ToString();
                GlobalVariables.DistKey = int.Parse(GlobalVariables.DistNumber);
                GlobalVariables.BannerText = dr["BannerTitle"].ToString();
                GlobalVariables.BannerSubText = dr["BannerSubTitle"].ToString();
                GlobalVariables.StyleSheet = dr["DistStyle"].ToString();
                GlobalVariables.SiteName = "AA District " + DistKey.ToString();
                GlobalVariables.DomainName = dr["DomainName"].ToString();
            }
            dg.KillReader(dr);


        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null)
            {
                context.Session["currDist"] = GlobalVariables.DistNumber;
                BundleConfig.RegisterBundles(BundleTable.Bundles);
            }
        }
        class FirstRequestInitialisation
        {
            private static string host = null;

            private static Object s_lock = new Object();

            // Initialise only on the first request
            public static string Initialise(HttpContext context)
            {
                if (string.IsNullOrEmpty(host))
                {
                    lock (s_lock)
                    {
                        if (string.IsNullOrEmpty(host))
                        {
                            Uri uri = HttpContext.Current.Request.Url;
                            host = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                        }
                    }
                }

                return host;
            }
        }
    }
    public static class GlobalVariables
    {
        public static string DistNumber { get; set; }
        public static int DistKey { get; set; }
        public static string BannerText { get; set; }
        public static string BannerSubText { get; set; }
        public static string StyleSheet { get; set; }
        public static string SiteName { get; set; }
        public static string DomainName { get; set; }
    }
}
