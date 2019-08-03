using System.Web.Optimization;

namespace CatchFilms
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css")
                .Include(
                "~/Content/fonts/font-awesome.min.css",
                "~/Content/style.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                "~/Content/js/jquery-1.11.1.min.js",
                "~/Content/js/plugins.js",
                "~/Content/js/app.js"));

        }
    }
}