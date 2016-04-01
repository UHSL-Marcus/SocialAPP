using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace SocialApp
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Scripts/Utils").Include(
                           "~/Scripts/Utils/DateDropdown.js",
                           "~/Scripts/Utils/FormValidation.js",
                           "~/Scripts/Utils/jspostcode.js",
                           "~/Scripts/Utils/jstelnumbers.js",
                           "~/Scripts/Utils/jquery.canvasjs.min.js",
                           "~/Scripts/Utils/jsonStringify.js",
                           "~/Scripts/Utils/headerHider.js",
                           "~/Scripts/Utils/Misc.js",
                           "~/Scripts/Pages/Master.js"
                           ));

            bundles.Add(new StyleBundle("~/bundles/Content/Utils").Include(
                            "~/Content/Utils/Flex.css",
                            "~/Content/Utils/Misc.css",
                            "~/Content/Utils/Positioning.css",
                            "~/Content/Utils/HideHeader.css",
                            "~/Content/Pages/Master.css"
                            ));

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.min.js",
                    DebugPath = "~/Scripts/respond.js",
                });
        }
    }
}