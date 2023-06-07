using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Framework.Mvc;
using App.Web.Helper;
using App.Web.Models;
using System.Data.Linq.SqlClient;
using App.Web.App_Start;

namespace App.Web.Controllers.Imex
{
    public partial class ImexController
    {
        #region Initilize
        #endregion

        //[HttpGet]
        //public ActionResult RegulationManagementDetail(int NoPermitCategory)
        //{
        //    ViewBag.Message = TempData["Message"] + "";
        //    this.PaginatorBoot.Remove("SessionTRN");

        //    var model = Service.Imex.RegulationManagementDetail.GetByNoPermitCategory(NoPermitCategory);
        //    if (model == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.NoPermitCategory = NoPermitCategory;

        //    return PartialView("regulation-management.Detail", model);
        //}

        [HttpGet]
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [Route("RegulationManagementDetail/{NoPermitCategory:int}")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public ActionResult RegulationManagementDetail(int NoPermitCategory)
        {
            ViewBag.Message = TempData["Message"] + "";
            this.PaginatorBoot.Remove("SessionTRN");

            var model = Service.Imex.RegulationManagement.GetByNoPermitCategory(NoPermitCategory);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.NoPermitCategory = NoPermitCategory;

            return PartialView("regulation-management.Detail", model);
        }

        public ActionResult RegulationManagDetailPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RegulationManagDetailPageXt();
        }

        public ActionResult RegulationManagDetailPageXt()
        {
            Func<RegulationManagementView, List<Data.Domain.RegulationManagement>> func = delegate (RegulationManagementView crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<RegulationManagementView>(param);
                }

                var tbl = Service.Imex.RegulationManagement.GetList(crit.Regulation, crit.ListCodePermitCategory, crit.ListHSCode, crit.ListOM);
                
                return tbl;
            };

            var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.RegulationManagement>("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public JsonResult GetDataFilterSelect2()
        {
            var dataOM = Service.Master.OrderMethods.GetList().Select(p => new { id = p.OMID, text = p.OMCode }).ToList();
            var dataPermitCategory = Service.Imex.RegulationManagement.GetListHeader().Select(p => new { id = p.CodePermitCategory, text = p.CodePermitCategory + " - " + p.PermitCategoryName }).ToList();
            //var dataHS = Service.Master.HSCodeLists.GetList().Select(p => new { id = p.HSCode, text = p.HSCode }).ToList();

            return Json(new { dataOM, dataPermitCategory }, JsonRequestBehavior.AllowGet);
            //return Json(new { dataHS, dataOM, dataPermitCategory }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult RegulationManagDetailPageXt()
        //{
        //    Func<RegulationManagementView, List<Data.Domain.RegulationManagementDetail>> func = delegate(RegulationManagementView crit)
        //    {
        //        var param = Request["params"];
        //        if(!string.IsNullOrEmpty(param))
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            crit = ser.Deserialize<RegulationManagementView>(param);
        //        }


        //        var tbl = Service.Imex.RegulationManagementDetail.GetList().Where(w => w.RegulationManagementID == crit.RegulationManagementID);

        //        if(crit.Status.HasValue)
        //            tbl = tbl.Where(w => w.Status == crit.Status.Value).ToList();

        //        var list = from c in tbl
        //                             from g in Service.Imex.PartsMapping.GetList().Where(w => w.HSId == c.HSID)
        //                             .GroupBy(g => g.HSId).Select(x => new { HsId = x.Key, NoPart = x.Count() }).DefaultIfEmpty()
        //                             select new Data.Domain.RegulationManagementDetail()
        //                             {
        //                                 RegulationManagementID = c.RegulationManagementID,
        //                                 Regulation = c.Regulation,
        //                                 DetailID = c.DetailID,
        //                                 HSID = c.HSID,
        //                                 Status = c.Status,
        //                                 QtyOfParts = (g ==null ? 0 : g.NoPart), //c.QtyOfParts,
        //                                 EntryDate = c.EntryDate,
        //                                 ModifiedDate = c.ModifiedDate,
        //                                 EntryBy = c.EntryBy,
        //                                 ModifiedBy = c.ModifiedBy,
        //                                 HSCode = c.HSCode,
        //                                 HSDescription = c.HSDescription,
        //                                 OMCode = c.OMCode,
        //                                 LicenseNumber = c.LicenseNumber,
        //                                 LartasId = c.LartasId,
        //                                 LartasDesc = c.LartasDesc
        //                             };

        //        return list.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.HSCode).ToList();
        //    };

        //    var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.RegulationManagementDetail>("SessionTRN", func).Pagination.ToJsonResult();
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}


        //#region crud
        [HttpGet]
        public ActionResult RegulationManagDetailAdd(int regid)
        {
            var reg = Service.Imex.RegulationManagement.GetId(regid);
            var model = new Data.Domain.RegulationManagementDetail();
            model.Status = 1;
            model.EntryDate = DateTime.Now;
            model.RegulationManagementID = regid;
            model.Regulation = reg.Regulation;
            model.OMCode = reg.OMCode;
            model.HSCode = reg.HSCode;
            model.RegulationCode = reg.CodePermitCategory;
            ViewBag.crudMode = "I";

            return PartialView("regulation-management.detail.iud", model);
        }

        [HttpPost]
        public ActionResult RegulationManagDetailAdd(Data.Domain.RegulationManagementDetail item)
        {
            ViewBag.crudMode = "I";
            if (string.IsNullOrWhiteSpace(item.RegulationCode))
            {
                return Json(new JsonObject { Status = 1, Msg = "Regulation required ..!" });
            }
            if (item.HSID == 0)
            {
                return Json(new JsonObject { Status = 1, Msg = "HS required ..!" });
            }
            //if (item.LartasId == 0) 
            //{
            //    return Json(new JsonObject { Status = 1, Msg = "Lartas required ..!" });
            //}

            if (ModelState.IsValid)
            {
                //if (Service.Imex.RegulationManagementDetail.GetList()
                //    .Where(w => w.RegulationManagementID == item.RegulationManagementID
                //    && w.HSID == item.HSID && w.Status == item.Status).Count() > 0)
                //{
                //    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Data already exists" });
                //}

                if (Service.Imex.RegulationManagementDetail.IfExist(item.HSID, item.RegulationCode))
                {
                    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Data already exists" });
                }

                App.Service.Imex.RegulationManagementDetail.Update(item, "I");
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var nsg = Web.Helper.Error.ModelStateErrors(ModelState);
                return Json(new { success = false, Msg = nsg });
                //return Json(new { success = false });
            }
        }

        //[HttpGet]
        //public ActionResult RegulationManagDetailEdit(int id)
        //{
        //    ViewBag.crudMode = "U";
        //    try
        //    {
        //        var model = Service.Imex.RegulationManagementDetail.GetId(id);
        //        if(model == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return PartialView("regulation-management.detail.iud", model);
        //    }
        //    catch(Exception e)
        //    {
        //        return PartialView("Error.partial", e.InnerException.Message);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegulationManagDetailEdit(Data.Domain.RegulationManagementDetail item)
        //{
        //    if(item.RegulationManagementID == 0)
        //    {
        //        return Json(new JsonObject { Status = 1, Msg = "Regulation required ..!" });
        //    }
        //    if(item.HSID == 0)
        //    {
        //        return Json(new JsonObject { Status = 1, Msg = "HS required ..!" });
        //    }
        //    if(item.LartasId == 0)
        //    {
        //        return Json(new JsonObject { Status = 1, Msg = "Lartas required ..!" });
        //    }

        //    ViewBag.crudMode = "U";
        //    if(ModelState.IsValid)
        //    {
        //        if(Service.Imex.RegulationManagementDetail.GetList()
        //            .Where(w => w.DetailID != item.DetailID
        //            && w.RegulationManagementID == item.RegulationManagementID && w.HSID == item.HSID && w.Status == item.Status).Count() > 0)
        //        {
        //            return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Data already exists" });
        //        }

        //        App.Service.Imex.RegulationManagementDetail.Update(item, "U");
        //        return JsonCRUDMessage(ViewBag.crudMode);
        //    }
        //    else
        //    {
        //        var nsg = Helper.Error.ModelStateErrors(ModelState);
        //        return Json(new { success = false, Msg = nsg });
        //    }
        //}

        //[HttpPost]
        //public ActionResult RegulationManagDetailDeleteId(int id)
        //{
        //    var item = Service.Imex.RegulationManagementDetail.GetId(id);
        //    App.Service.Imex.RegulationManagementDetail.Update(item, "D");
        //    return JsonCRUDMessage("D");
        //}
        //#endregion
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public JsonResult DownloadRegulationDeatilToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadRegulationManagementDetail data = new Helper.Service.DownloadRegulationManagementDetail();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}