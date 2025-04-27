using System.Web.Optimization;

namespace LibraryManagementSystem
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Register only bundles for files that exist in your project
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            // CSS bundle
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
        }
    }
}