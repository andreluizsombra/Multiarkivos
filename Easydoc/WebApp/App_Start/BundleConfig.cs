using System.Web;
using System.Web.Optimization;

namespace MK.Easydoc.WebApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/app-scripts").Include(
                        "~/Assets/js/app-scripts/app-global.js", "~/Assets/js/app-scripts/app-utils.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Assets/plugins/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Assets/plugins/jquery/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Assets/plugins/jquery/jquery.unobtrusive*",
                        //"~/Assets/plugins/jquery/jquery.validate.js",
                        "~/Assets/plugins/jquery/jquery.mask.js",
                        //"~/Assets/plugins/jquery/jquery.validate.unobtrusive.js",
                        "~/Assets/plugins/jquery/jquery.blockui.js",
                        "~/Assets/plugins/bootstrap-toggle-master/js/bootstrap-toggle.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Assets/plugins/diversos/modernizr-*"));



            bundles.Add(new ScriptBundle("~/plugin/jquerytimer").Include(
            "~/Assets/plugins/jquery/jquery.timer.js"));

            bundles.Add(new ScriptBundle("~/plugin/pdfSlider").Include(
                "~/Assets/plugins/pdfSlider/pdfSlider.js"));


            bundles.Add(new ScriptBundle("~/plugin/js/iviewer").Include(
            "~/Assets/plugins/iviewer-master/jquery.mousewheel.js",
            "~/Assets/plugins/iviewer-master/jquery.iviewer.js"));


            bundles.Add(new ScriptBundle("~/Assets/js/core-scripts/").Include("~/Assets/js/core-scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/Assets/js/core-scripts/bootstrap").Include("~/Assets/js/core-scripts/bootstrap.min.js"));
            
            bundles.Add(new StyleBundle("~/Assets/css/core-style/bootstrap").Include("~/Assets/css/core-style/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Assets/css/core-style/AdminLTE").Include("~/Assets/css/core-style/AdminLTE.min.css", "~/Assets/css/core-style/skins/skin-green.min.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Assets/plugins/iviewer-master/jquery.iviewer.css"));

            bundles.Add(new ScriptBundle("~/plugin/js/fileuploader").Include("~/Assets/plugins/fileuploader/fileuploader.js"));

            bundles.Add(new StyleBundle("~/plugin/css/fileuploader").Include("~/Assets/plugins/fileuploader/fileuploader.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Assets/plugins/themes/base/jquery.ui.core.css",
                        "~/Assets/plugins/base/jquery.ui.resizable.css",
                        "~/Assets/plugins/base/jquery.ui.selectable.css",
                        "~/Assets/plugins/base/jquery.ui.accordion.css",
                        "~/Assets/plugins/base/jquery.ui.autocomplete.css",
                        "~/Assets/plugins/base/jquery.ui.button.css",
                        "~/Assets/plugins/base/jquery.ui.dialog.css",
                        "~/Assets/plugins/base/jquery.ui.slider.css",
                        "~/Assets/plugins/base/jquery.ui.tabs.css",
                        "~/Assets/plugins/base/jquery.ui.datepicker.css",
                        "~/Assets/plugins/base/jquery.ui.progressbar.css",
                        "~/Assets/plugins/base/jquery.ui.theme.css"));
        }
    }
}