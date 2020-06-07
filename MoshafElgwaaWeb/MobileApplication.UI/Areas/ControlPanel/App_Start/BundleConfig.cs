using System.Web;
using System.Web.Optimization;
using MobileApplication.DataService;

namespace MobileApplication.UI.Areas.ControlPanel
{
    public partial class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Areas/ControlPanel/Content/csss/").Include(
                       "~/Areas/ControlPanel/Content/css/styles.css",
                       "~/JSPlugins/QVConfirmationMessage/toastr.css",
                       "~/Areas/ControlPanel/Content/css/jquery.calendars.picker.css"

                      
                       ));

           Bundle Scripts   =new ScriptBundle("~/ControlPanel/js").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/enquire.js",
                        "~/Scripts/jquery.nicescroll.min.js",
                        "~/Scripts/jquery.cookie.js",
                        "~/Scripts/application.js",
                        "~/Scripts/Angular/angular.js",
                        "~/Scripts/Angular/customSelect.js",
                        "~/Scripts/Angular/angular-sanitize.js",
                        "~/Scripts/Angular/angular-resource.min.js",
                        "~/Scripts/Angular/AngularConfig.js",
                        "~/Scripts/jquery.validate.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/JSPlugins/QVCalender/jquery.calendars.js",
                        "~/JSPlugins/QVCalender/jquery.calendars.plus.js",
                        "~/JSPlugins/QVCalender/jquery.calendars.picker.js",
                        "~/JSPlugins/QVCalender/jquery.calendars.ummalqura.js",
                        "~/JSPlugins/QVCalender/jquery.calendars.dblDatePicker.js",
                        "~/Scripts/jquery-ui.min.js",
                        "~/Scripts/Angular/angular-ui.js",
                        "~/Scripts/Angular/angular-dragdrop.min.js",
                        "~/JSPlugins/QVConfirmationMessage/toastr.js",
                        "~/Scripts/QvLib.UI.js"
                       
                        );
          
            bundles.Add(Scripts);
         //   System.Web.Optimization.BundleTable.EnableOptimizations = true;
        }
    }
}
