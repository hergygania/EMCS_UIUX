using System;
using System.IO;
using System.Web.Mvc;
using App.Data.Domain.POST;
using App.Service.POST;
using App.Web.App_Start;


namespace App.Web.Controllers.POST
{
    public partial class PostController
    {

        public ActionResult QuickSearch()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 1;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult AdvanceSearch()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 2;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }


        #region Table
        public JsonResult GetListAdvanceSearch(SearchAdvance model)
        {
            try
            {
                var data = Service.POST.AdvanceSearh.GetListAdvanceSearch(model);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetQuickSearchPOList(string search, string filterBy)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                int countData = 0;
                var datajson = new JsonResult();
                if (filterBy == "Vendor Name")
                {
                    var data = Service.POST.QuickSearch.GetPOByVendor(search, userLogin);
                    countData = data.Count;
                    datajson = Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else if (filterBy == "Goods Name")
                {
                    var data = Service.POST.QuickSearch.GetPOByGoods(search, userLogin);
                    countData = data.Count;
                    datajson = Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else if (filterBy == "PR Number")
                {
                    var data = Service.POST.QuickSearch.GetPOByPRNoMultiple(search, userLogin);
                    countData = data.Count;
                    datajson = Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = Service.POST.QuickSearch.GetPOByPODate(search, userLogin);
                    countData = data.Count;
                    datajson = Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }

                if (countData == 0)
                {
                    return Json(new { status = "FAILED", result = "" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    return datajson;
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPOByPRNo(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.QuickSearch.GetPOByPRNo(search, userLogin);

                if (data != null)
                {
                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "FAILED", result = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPOByGoods(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.QuickSearch.GetPOByGoods(search, userLogin);

                if (data != null)
                {
                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "FAILED", result = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetPOByPONo(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.QuickSearch.GetPOByPONo(search, userLogin);

                if (data != null)
                {
                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "FAILED", result = data }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Get Select2 List
        public JsonResult GetSelectDestination(string search)
        {
            try
            {
                var data = Global.GetSelectBranch(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectDeliveryStatus(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectDeliveryStatus(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectStatusPO(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectStatusPO(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectSupplier(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.Global.GetSelectSupplier(search, userLogin);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectUserPic(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectUserPic(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region DownloadExcel
        public ActionResult DownloadReportExcelSla(SearchReport model)
        {
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadToExcelSla(model);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadResultReportExcelSla(string guid)
        {
            return Session[guid] as FileResult;
        }


        private FileResult DownloadToExcelSla(SearchReport model)
        {
            try
            {
                var user = HttpContext.User.Identity.Name;
                var output = Service.POST.AdvanceSearh.DownloadToExcelSla(model, user);
                return File(output.ToArray(),
                 "application/vnd.ms-excel",
                 "ReportSLA.xlsx");
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportSLA.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        #endregion
    }
}