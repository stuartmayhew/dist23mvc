using System.Web;
using System.Web.Optimization;

namespace Dist23MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundles(bundles);
            RegisterScriptBundles(bundles);

            BundleTable.EnableOptimizations = false;
        }


        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/css/kendo")
                .Include("~/Content/kendo/kendo.common-bootstrap.min.css")
                .Include("~/Content/kendo/kendo.bootstrap.min.css")
                .Include("~/Content/kendo/kendo.dataviz.min.css")
                .Include("~/Content/kendo/kendo.dataviz.bootstrap.min.css"));
        }

        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery")
                .Include("~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/scripts")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/jquery-ui-1.10.4.custom.min.js"));

            bundles.Add(new ScriptBundle("~/js/kendo")
                .Include("~/Scripts/kendo.all.min.js")
                .Include("~/Scripts/kendo.aspnetmvc.min.js")
                .Include("~/Scripts/jszip.js"));

            //bundles.Add(new ScriptBundle("~/bundles/demo").Include(
            //            "~/Scripts/console.js",
            //            "~/Scripts/prettify.js"));

            bundles.Add(new ScriptBundle("~/js/forms")
                .Include("~/scripts/jquery.placeholder.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/additional-methods.js"));

            bundles.Add(new ScriptBundle("~/js/trawick")
                .Include("~/Scripts/main.js"));
        }
    }
}

        //private static void RegisterScriptBundles(BundleCollection bundles)
        //{
        //    bundles.Add(new ScriptBundle("~/js/jquery")
        //        .Include("~/scripts/jquery-{version}.js"));

        //    bundles.Add(new ScriptBundle("~/js/scripts")
        //        .Include("~/scripts/bootstrap.js")
        //        .Include("~/scripts/jquery-ui-1.10.4.custom.min.js"));

        //    bundles.Add(new ScriptBundle("~/js/kendo")
        //        .Include("~/Scripts/kendo.all.min.js")
        //        .Include("~/Scripts/kendo.aspnetmvc.min.js")
        //        .Include("~/Scripts/jszip.js")
        //        .Include("~/Scripts/main.js"));

        //    bundles.Add(new ScriptBundle("~/js/forms")
        //        .Include("~/Scripts/jquery.placeholder.js")
        //        .Include("~/Scripts/jquery.validate.js")


        //}
//    }
//}
