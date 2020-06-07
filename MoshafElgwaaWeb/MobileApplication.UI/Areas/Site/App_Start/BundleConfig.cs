using System.Web;
using System.Web.Optimization;

namespace MobileApplication.UI.Areas.Site
{
    public partial class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/site/css").Include(

                   
                      ));

            bundles.Add(new ScriptBundle("~/site/js").Include(

                        
                        ));
            //BundleTable.EnableOptimizations = true;
        }
    }
}