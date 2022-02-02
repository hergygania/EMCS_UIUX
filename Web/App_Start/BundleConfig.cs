using System.Web;
using System.Web.Optimization;

namespace App.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                    "~/Scripts/jquery.js",
                                    "~/Scripts/jquery.cookie.js",
                                    "~/Scripts/TreeMenu.js", 
                                    "~/Scripts/jquery.treegrid.bootstrap3.js",
                                    "~/Scripts/sweetalert-dev.js"
                                    )); //"~/Scripts/jquery-{version}.js"));         

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                    "~/Scripts/jquery.unobtrusive*",
                                    "~/Scripts/jquery.validate*",
                                    "~/Scripts/jquery.unobtrusive-ajax.min.js"
                                    ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                "~/Scripts/bootstrap.min.js",
                                "~/Scripts/bootstrap-table-custom.js",
                                "~/Scripts/bootstrap-table-fixed-columns.js",
                                "~/Scripts/extension/filter/bootstrap-table-filter.min.js",
                                "~/Scripts/extension/filter/ext/bs-table.js",
                                "~/Scripts/bootstrap-datepicker.js",
                                "~/Scripts/daterangepicker.js",
                                "~/Scripts/bootstrap-timepicker.js"
                                //"~/Content/POST/SweetAlert/sweetalert2.min.js"
                                 //"~/Content/Dropzone/js/dropzone.js"
                                ));

            bundles.Add(new ScriptBundle("~/bundles/daterangepicker").Include(
                                "~/Scripts/daterangepicker-2/moment.min.js",
                                "~/Scripts/daterangepicker-2/daterangepicker.js"
                                ));

            bundles.Add(new ScriptBundle("~/bundles/extention").Include(
                                "~/Scripts/prettify.min.js",
                                "~/Scripts/jquery-ui-1.11.4.js",
                                "~/Scripts/select2.full.js",
                                "~/Scripts/validator.min.js",
                                "~/Scripts/sweetalert-dev.js",
                                "~/Scripts/jquery.inputmask.js",
                                "~/Scripts/jquery.inputmask.date.extensions.js",
                                "~/Scripts/jquery.inputmask.extensions.js",
                                "~/Scripts/menu-kiri.js",
                                "~/Scripts/script.js",
                                "~/Scripts/app/pis-bootstrap-table.js"
                                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                                "~/Content/font-awesome.css",
                                "~/Content/bootstrap.min.css",
                                "~/Content/bootstrap-table.css",
                                "~/Content/bootstrap-table-fixed-columns.css",
                                "~/Scripts/extension/filter/bootstrap-table-filter.css",
                                "~/Content/bootstrap-datepicker.css",
                                "~/Content/ionicons.css",
                                "~/Content/prettify.css",
                                "~/Content/select2.min.css",
                                "~/Content/sweetalert.css",
                                "~/Content/toogleSwitch.css",
                                "~/Content/bootstrap-timepicker.css",
                                "~/Content/daterangepicker-bs3.css",
                                "~/Content/AdminLTE.css",
                                "~/Content/site.css",
                           
                                   //"~/Content/Dropzone/css/dropzone.css",
                                "~/Content/custom.css","~/Content/Collapse.css"
                                ));
            bundles.Add(new ScriptBundle("~/bundles/AppScript").Include(
                "~/Scripts/jquery.easyui.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/cssDaterangepicker").Include(
                "~/Scripts/daterangepicker-2/daterangepicker.css"
                ));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                                
                "~/Content/SoVetting/scripts/libs.js",
                "~/Content/SoVetting/scripts/dataTables/jszip.min.js",
                "~/Content/SoVetting/scripts/dataTables/dataTables.buttons.min.js",
                "~/Content/SoVetting/scripts/dataTables/buttons.html5.min.js",
                "~/Content/SoVetting/scripts/script.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/extentionv2").Include(
                                "~/Scripts/prettify.min.js",
                                "~/Scripts/jquery-ui-1.11.4.js",
                                "~/Scripts/select2.full.js",
                                "~/Scripts/validator.min.js",
                                "~/Scripts/sweetalert-dev.js",
                                "~/Scripts/jquery.inputmask.js",
                                "~/Scripts/jquery.inputmask.date.extensions.js",
                                "~/Scripts/jquery.inputmask.extensions.js",
                                "~/Scripts/menu-kiri.js",
                                "~/Scripts/script.js",
                                "~/Scripts/app/pis-bootstrap-table-v2.js"
                                ));

            bundles.Add(new StyleBundle("~/Content/css2").Include(
                "~/Content/SoVetting/css/style.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapv2").Include(
                                "~/Scripts/bootstrap.min.js",
                                "~/Scripts/bootstrap3-editable/js/bootstrap-editable.js",
                                "~/Scripts/bt/dist/bootstrap-table.js",
                                "~/Scripts/bt/dist/extensions/fixed-columns/bootstrap-table-fixed-columns.js",
                                "~/Scripts/bt/dist/extensions/filter-control/bootstrap-table-filter-control.min.js",
                                "~/Scripts/bt/dist/extensions/editable/bootstrap-table-editable.js",
                                "~/Scripts/bootstrap-datepicker.js",
                                "~/Scripts/daterangepicker.js",
                                "~/Scripts/bootstrap-timepicker.js"
                                ));

            bundles.Add(new StyleBundle("~/Content/cssv2").Include(
                                "~/Content/fontawesome/css/all.css",
                                "~/Content/bootstrap.min.css",
                            
                                "~/Scripts/bootstrap3-editable/css/bootstrap-editable.css",
                                "~/Scripts/bt/dist/bootstrap-table.min.css",
                                "~/Scripts/bt/dist/extensions/filter-columns/bootstrap-table-fixed-columns.min.min",
                                "~/Scripts/bt/dist/extensions/filter-control/bootstrap-table-filter-control.min",

                                "~/Content/bootstrap-datepicker.css",
                                "~/Content/ionicons.css",
                                "~/Content/prettify.css",
                                "~/Content/select2.min.css",
                                "~/Content/sweetalert.css",
                                "~/Content/toogleSwitch.css",
                                "~/Content/bootstrap-timepicker.css",
                                "~/Content/daterangepicker-bs3.css",
                                "~/Content/AdminLTE.css",
                                "~/Content/sitev2.css",
                                "~/Content/custom.css", "~/Content/Collapse.css"
                                ));

            bundles.Add(new StyleBundle("~/Content/postbundle").Include(
                                "~/Content/font-awesome.css",
                                "~/Content/EMCS/fontawesome/css/all.css",
                                "~/Content/POST/Bootstrap/css/bootstrap.min.css",
                                "~/Content/POST/Bootstrap/css/bootstrap.css",                                
                                "~/Scripts/bootstrap3-editable/css/bootstrap-editable.css",
                                "~/Content/post/BootstrapTable/bootstrap-table.min.css",                                
                                "~/Content/post/BootstrapTable/bootstrap-table-group-by.css",
                                "~/Scripts/bt/dist/extensions/filter-columns/bootstrap-table-fixed-columns.min.min",
                                "~/Scripts/bt/dist/extensions/filter-control/bootstrap-table-filter-control.min",

                                "~/Content/bootstrap-datepicker.css",
                                "~/Content/ionicons.css",
                                "~/Content/prettify.css",
                                "~/Content/select2.min.css",
                                "~/Content/sweetalert.css",
                                "~/Content/toogleSwitch.css",
                                "~/Content/bootstrap-timepicker.css",
                                "~/Content/daterangepicker-bs3.css",
                                "~/Content/AdminLTE.css",
                                //"~/Content/sitev2.css",
                                "~/Content/site.css",
                                "~/Content/custom.css", "~/Content/Collapse.css"
                                ));


#if (DEBUG)
            BundleTable.EnableOptimizations = false;
#else
			BundleTable.EnableOptimizations = true;
#endif
        }
    }
}
