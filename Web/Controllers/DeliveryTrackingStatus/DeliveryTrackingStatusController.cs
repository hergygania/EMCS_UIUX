using App.Domain;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Service.DeliveryTrackingStatus;
using dts = App.Service.DeliveryTrackingStatus.PageDTS;
using MSTdts = App.Service.DeliveryTrackingStatus.MasterDTS;
using saveToExcel = App.Service.Master.saveFileExcel;
using System.Web.Script.Serialization;
using App.Service.FreightCost;
using App.Web.App_Start;

namespace App.Web.Controllers.DeliveryTrakingStatus
{
    public class DeliveryTrackingStatusController : App.Framework.Mvc.BaseController
    {
        // GET: DeliveryTrackingStatus
        #region Import Delivery Tracking Status
        public ActionResult ImportDeliveryTracking()
        {
            return View();
        }

        #region grid
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "dts")]
        public ActionResult IndexPageLogImport()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetLogImportDTS();
        }

        public ActionResult GetLogImportDTS()
        {
            Func<MasterSearchForm, IList<Data.Domain.Extensions.DocumentUpload>> func = delegate(MasterSearchForm crit)
            {
                //App.Service.DeliveryTrackingStatus.ImportDTS ipp = new App.Service.DeliveryTrackingStatus.ImportDTS();
                //var list = ipp.GetListDocumentUpload();
                var doc = Service.Master.DocumentUpload.getListDocumentUpload("Delivery Tracking Status");

                return doc.ToList();

            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult ImportDTS(HttpPostedFileBase upload)  //Logic Import
        {
            List<Data.Domain.DeliveryTrackingStatus> listDTS = new List<Data.Domain.DeliveryTrackingStatus>();
            ReadDataDTSFromExcel readExcel = new ReadDataDTSFromExcel();

            string msg = "", filePathName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "Delivery Tracking Status", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            try
            {
                listDTS = readExcel.getDataDTS(filePathName, System.IO.Path.GetFileName(filePathName));
                foreach (var item in listDTS)
                {
                    Service.DeliveryTrackingStatus.ImportDTS.crud(item, "I");
                }

                _file.Status = 1;
                Service.Master.DocumentUpload.crud(_file, "U");

                return Json(new { Status = 0, Msg = "Upload succesful" });

            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(filePathName),
                           "Delivery Tracking Status", "");

                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        #endregion

        #region Searching Page
        public ActionResult Dts()
        {
            var model = new ReportFilterDTS
            {
                Origin = Service.DeliveryTrackingStatus.MasterDTS.GetMasterCItys(),
                Destination = Service.DeliveryTrackingStatus.MasterDTS.GetMasterCItys(),
                EstArrival = new DateTime(DateTime.Now.Year, 1, 1),
                EstDepature = new DateTime(DateTime.Now.Year, 1, 1)
            };
            return View(model);
        }

        public ActionResult IndexPageDts()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetDts();
        }

        public ActionResult GetDts()
        {
            Func<Data.Domain.V_filterDTS, IList<Data.Domain.V_DTS>> func = delegate(Data.Domain.V_filterDTS model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_filterDTS>(param);
                }

                var list = dts.GetDataViewDTS(model);
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateDTS(int id) //UI.Modal
        {
            ViewBag.Message = TempData["Message"] + "";
            this.PaginatorBoot.Remove("SessionTRN");

            var model = Service.DeliveryTrackingStatus.ImportDTS.GetDTSByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return PartialView("DTSuid", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDTS(App.Data.Domain.DeliveryTrackingStatus model) //UI.Modal
        {
            ViewBag.crudMode = "U";
            try
            {
                dts.Update(model, ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Msg = ex.Message });
            }
        }

        #region Get master Drop Down List
        [HttpGet]
        public JsonResult GetOrigin()
        {
            List<Select2Result> result = new List<Select2Result>();
            result = MSTdts.GetMasterCItys();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDestination()
        {
            List<Select2Result> result = new List<Select2Result>();
            result = MSTdts.GetMasterCItys();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetModa()
        {
            List<Select2Result> result = new List<Select2Result>();
            result = MSTdts.GetMasterModa();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStatus()
        {
            List<Select2Result> result = new List<Select2Result>();
            result = MSTdts.GetMasterStatus();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Milestone DTS
        [HttpGet]
        public ActionResult MilestoneDTS(string NODI, string SalesOrderNumber)
        {
            ViewBag.Message = TempData["Message"] + "";
            ViewBag.NODI = NODI;
            ViewBag.SalesOrderNumber = SalesOrderNumber;
            this.PaginatorBoot.Remove("SessionTRN");

            if (string.IsNullOrWhiteSpace(NODI) && string.IsNullOrWhiteSpace(SalesOrderNumber))
            {
                return HttpNotFound();
            }

            return PartialView("PopupMilestone");
        }

        [HttpPost]
        public ActionResult IndexPageMilestoneDTS()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetMilstoneDTS();
        }

        [HttpGet]
        public ActionResult GetMilstoneDTS() //UI.Modal
        {
            Func<Data.Domain.V_filterDTS, IList<Data.Domain.MilestoneDTS>> func = delegate(Data.Domain.V_filterDTS model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_filterDTS>(param);
                }

                var list = dts.GetMilestoneDTS(model.OutBoundDelivery, model.SalesOrderNumber);
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Detail DTS
        [HttpGet]
        public ActionResult infoDTS(App.Data.Domain.V_DTS model)
        {
            ViewBag.Message = TempData["Message"] + "";
            this.PaginatorBoot.Remove("SessionTRN");

            if (model == null)
            {
                return HttpNotFound();
            }

            return PartialView("DTSDetail", model);
        }

        public ActionResult IndexPageDetailDts()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetDetailDts();
        }

        public ActionResult GetDetailDts()
        {
            Func<Data.Domain.V_filterDTS, IList<Data.Domain.V_DetailDTS>> func = delegate(Data.Domain.V_filterDTS model)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    model = ser.Deserialize<Data.Domain.V_filterDTS>(param);
                }

                var list = dts.GetDataViewDetailDTS(model.NoDeliveryAdvice, model.OutBoundDelivery);
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Report DIFOT
        public ActionResult DIFOT()
        {
            return View();
        }

        public ActionResult ReportDIFOT()  //Logic Report
        {
            return View();
        }
        #endregion
    }
}