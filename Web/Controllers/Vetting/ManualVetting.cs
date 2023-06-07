using App.Data.Caching;
using App.Service.FreightCost;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ReadExcel = App.Service.Vetting.ReadDataManualVetting;
using saveToExcel = App.Service.Master.saveFileExcel;

namespace App.Web.Controllers.Vetting
{
    public partial class VettingController
    {
        [Route("Manual-Vetting")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ManualVetting()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Message = TempData["Message"] + "";

            this.PaginatorBoot.Remove("SessionTRN");
            return View("ManualVetting");
        }

        public ActionResult ManualVettingPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return ManualVettingPageXt();
        }

        public ActionResult ManualVettingPageXt()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            int counttotal = 0;

            Func<ManualVettingModel, List<Data.Domain.SP_ManualVetting>> func = delegate (ManualVettingModel crit)
            {
                var param = Request["params"];
                #region Paging
                int startNum = 0, EndNum = 0;
                int limit = Request["limit"] != null ? Convert.ToInt32(Request["limit"]) : 0;
                int offset = Request["offset"] != null ? Convert.ToInt32(Request["offset"]) : 0;
                bool isPaging = Request["IsPaging"] != null ? Convert.ToBoolean(Request["IsPaging"]) : false;
                string orderBy = Request["sortName"] ?? "";

                if (limit > 0 && offset > 0)
                {
                    startNum = (offset * limit) - (limit - 1);
                    EndNum = (offset * limit);
                }

                const string cacheName = "App.imex.FilterManualVetting";
                ICacheManager _cacheManager = new MemoryCacheManager();

                string key = string.Format(cacheName);


                if (!string.IsNullOrEmpty(param))
                {
                    if (!isPaging) _cacheManager.Remove(cacheName);

                    var filter = _cacheManager.Get(key, () =>
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<ManualVettingModel>(param);
                        return crit;
                    });

                    crit = filter;
                }
                #endregion

                string partlistid = crit.selPartsList_Ids != null ? string.Join(",", crit.selPartsList_Ids.ToArray()) : "";
                string hscodeid = crit.selHSCodeList_Ids != null ? string.Join(",", crit.selHSCodeList_Ids.ToArray()) : "";
                string omlistid = crit.selOrderMethods != null ? string.Join(",", crit.selOrderMethods.ToArray()) : "";

                var tbl = Service.Vetting.ManualVetting.GetSPList_Paging(Domain.SiteConfiguration.UserName, startNum, EndNum, partlistid, hscodeid, omlistid, orderBy);

                counttotal = Service.Vetting.ManualVetting.GetSPCount_Paging(Domain.SiteConfiguration.UserName, startNum, EndNum, partlistid, hscodeid, omlistid, orderBy);

                return tbl.ToList();
            };
                                                                     
            var paging = PaginatorBoot.Manage<ManualVettingModel, Data.Domain.SP_ManualVetting>("SessionTRN", func).Pagination.ToJsonResult();

            return Json(new
            {
                paging = paging,
                totalcount = counttotal
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportManualVetting(HttpPostedFileBase upload)  //Logic 
        {
            List<Data.Domain.ManualVetting> data = new List<Data.Domain.ManualVetting>();

            string msg = "", filePathName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "Manual Vetting", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            try
            {
                data = ReadExcel.GetDataManualVetting(filePathName, System.IO.Path.GetFileName(filePathName));

                using (TransactionScope scop = new System.Transactions.TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { Timeout = new TimeSpan(5, 0, 0) }))
                {       
                    try
                    {
                        Service.Vetting.ManualVetting.UpdateBulkNew(data, "I");

                        Service.Vetting.ManualVetting.UpdateRemarks();

                        _file.Status = 1;
                        Service.Master.DocumentUpload.crud(_file, "U");

                        scop.Complete();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                            throw new Exception(ex.Message);
                        else
                            throw new Exception(ex.InnerException.Message);
                    }
                }

                return Json(new { Status = 0, Msg = "Upload succesful" });

            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(filePathName),
                           "Manual Vetting", "");

                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        public JsonResult GetDataFilterSelect2()
        {
            var dataOM = Service.Master.OrderMethods.GetListDataForSelect2();

            return Json(new { dataOM = dataOM }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DownloadManualVettingToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadmanualVetting data = new Helper.Service.DownloadmanualVetting();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}