using System.Web;
using System.Web.Optimization;

namespace MvcMyhome
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/Layoutcss").Include(
                  "~/Content/layui/css/layui.css"
                , "~/Content/Layout/css/family.css"
                , "~/Content/Layout/css/animate.css"
                , "~/Content/Layout/css/icomoon.css"
                , "~/Content/Layout/css/bootstrap.css"
                , "~/Content/Layout/css/flexslider.css"
                , "~/Content/Layout/css/style.css"
                , "~/Content/layui/css/global.css"
               ));
            bundles.Add(new ScriptBundle("~/Content/jqueryjs").Include(
                  "~/Content/jquery-1.10.2.js", 
                  "~/Content/layui/layui.js"
               ));
            bundles.Add(new ScriptBundle("~/Content/Layoutjs").Include(
                  "~/Content/Layout/js/modernizr-2.6.2.min.js"
                , "~/Content/Layout/js/jquery.easing.1.3.js"
                , "~/Content/Layout/js/bootstrap.min.js"
                , "~/Content/Layout/js/jquery.waypoints.min.js"
                , "~/Content/Layout/js/jquery.flexslider-min.js"
                , "~/Content/Layout/js/main.js"
                ));
            bundles.Add(new ScriptBundle("~/Content/Albumjs").Include(
                   "~/Content/jqeryMasonry/dist/scripts/chromagallery.pkgd.min.js"
                  , "~/Content/jqeryMasonry/js/jquery.lazyload.js"
                ));
            bundles.Add(new StyleBundle("~/Content/Albumcss").Include(
                "~/Content/jqeryMasonry/dist/stylesheets/chromagallery.min.css"
               ));
        }
    }
}
