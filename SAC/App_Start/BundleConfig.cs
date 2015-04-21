// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BundleConfig.cs" company="">
//   Copyright © 2015 
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace App.SAC
{
    using System.Web;
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/content/css/app").Include(
                "~/scripts/vendor/winjs/css/ui-dark.css",
                "~/content/winjs.css",
                "~/content/app.css"));

            bundles.Add(new ScriptBundle("~/js/jquery").Include("~/scripts/vendor/jquery/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/app_vendor").Include(
                "~/scripts/vendor/winjs/js/WinJS.min.js",
                "~/scripts/vendor/winjs/angular-winjs.js",
                "~/scripts/vendor/bootstrap/ui-bootstrap-tpls-0.12.1.min.js",
                "~/scripts/vendor/angular/angular-ui-router.js",
                "~/scripts/vendor/angular/angular-animate.min.js",
                "~/scripts/vendor/angular/angular-sanitize.min.js"));

            bundles.Add(new ScriptBundle("~/js/app").IncludeDirectory("~/scripts/SAC", "*.js", true));
        }
    }
}
