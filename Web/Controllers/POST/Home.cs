using System;
using System.Web.Mvc;
using App.Web.App_Start;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        #region View

        public ActionResult Dashboard()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 4;

            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        public ActionResult DashboardMockUp()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            var data = Service.POST.Transaction.GetDataDashboard();
            ViewBag.po_incoming = data.po_incoming;
            ViewBag.po_progress = data.po_progress;
            ViewBag.po_done = data.po_done;
            ViewBag.po_complete = data.po_complete;
            ViewBag.po_invoice = data.po_invoice;
            ViewBag.po_bast = data.po_bast;
            ViewBag.CountPODNotBAST = data.CountPODNotBAST;
            ViewBag.CountBASTNotGR = data.CountBASTNotGR;
            ViewBag.CountGRNotInvoice = data.CountGRNotInvoice;
            ViewBag.CountInvoiceNotPayment = data.CountInvoiceNotPayment;
            ViewBag.Menu = 4;

            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        #endregion
        #region GetDate
        public JsonResult GetDataCount(int year, int month)
        {
            try
            {
               var data = Service.POST.Transaction.GetDataDashboard();

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetDashboardTopDelayVendor(int year , int month)
        {
            try
            {
                var data = Service.POST.Dashboard.GetDashboardTopDelayVendor(year,month);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardTopDelayPO(int year, int month)
        {
            try
            {
                var data = Service.POST.Dashboard.GetDashboardTopDelayPO(year,month);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardActiveVendor(int year, int month)
        {
            try
            {
                var data = Service.POST.Dashboard.GetDashboardActiveVendor(year, month);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardActiveVendorNewVerse(int year, int month, int yearfrom, int monthfrom, int yearto, int monthto, string type)
        {
            try
            {
                if(type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardActiveVendor(year, month);

                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardActiveVendorPeriod(yearfrom, monthfrom, yearto, monthto);

                    return Json(new { status = "SUCCESS", result = data2 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardActiveEmployee(int year, int month, int yearfrom, int monthfrom, int yearto, int monthto, string type)
        {
            try
            {
                if (type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardActiveEmployee(year, month, year, month, type);

                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardActiveEmployee(yearfrom, monthfrom, yearto, monthto, type);

                    return Json(new { status = "SUCCESS", result = data2 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardVendorHitrate(int year, int month, int yearfrom, int monthfrom, int yearto, int monthto, string type)
        {
            try
            {
                if (type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardVendorHitrate(year, month, year, month, type);

                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardVendorHitrate(yearfrom, monthfrom, yearto, monthto, type);

                    return Json(new { status = "SUCCESS", result = data2 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardVendorHitrateTbl(int year, int month, int yearFrom, int monthFrom, int yearTo, int monthTo, string type)
        {
            try
            {
                if (type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardVendorHitrateTbl(year, month, year, month, type);

                    //return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardVendorHitrateTbl(yearFrom, monthFrom, yearTo, monthTo, type);

                    //return Json(new { status = "SUCCESS", result = data2 }, JsonRequestBehavior.AllowGet);
                    return Json(data2, JsonRequestBehavior.AllowGet);
                }
                //var userLogin = HttpContext.User.Identity.Name;
                //var data = Service.POST.Dashboard.GetDashboardVendorHitrateTbl(yearFrom, monthFrom, yearTo, monthTo, type);

                //return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetDashboardEmployeeHitrate(int year, int month, int yearfrom, int monthfrom, int yearto, int monthto, string type)
        {
            try
            {
                if (type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardEmployeeHitrate(year, month, year, month, type);

                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardEmployeeHitrate(yearfrom, monthfrom, yearto, monthto, type);

                    return Json(new { status = "SUCCESS", result = data2 }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDashboardEmployeeHitrateTbl(int year, int month, int yearFrom, int monthFrom, int yearTo, int monthTo, string type)
        {
            try
            {
                if (type == "monthly")
                {
                    var data = Service.POST.Dashboard.GetDashboardEmployeeHitrateTbl(year, month, year, month, type);

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data2 = Service.POST.Dashboard.GetDashboardEmployeeHitrateTbl(yearFrom, monthFrom, yearTo, monthTo, type);

                    return Json(data2, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion
    }
}