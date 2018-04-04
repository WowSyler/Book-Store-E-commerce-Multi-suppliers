using System.Web;
using System.Web.Optimization;

namespace BookStore
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Geliştirme yapmak ve öğrenmek için Modernizr'ın geliştirme sürümünü kullanın. Daha sonra,
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/css/font-awesome.min.css"));
            bundles.Add(new StyleBundle("~/Content/css/admincss").Include(
                      "~/Content/css/admincss/bootstrap.min.css",
                      "~/Content/css/admincss/sb-admin.css",
                      "~/Content/css/admincss/fff.css"));

            bundles.Add(new ScriptBundle("~/Content/css/admincjs").Include(
             "~/Content/css/admincjs/jquery.js",
             "~/Content/css/admincjs/bootstrap.min.js",
             "~/Content/css/admincjs/MyCustom.js"
             ));
        }
    }
}
