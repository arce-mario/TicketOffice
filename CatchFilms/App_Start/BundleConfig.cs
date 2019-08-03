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
                "~/Content/css/style-home.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/js")
                .Include(
                "~/Scripts/plugins.js",
                "~/Scripts/app.js"));

        }
    }
}