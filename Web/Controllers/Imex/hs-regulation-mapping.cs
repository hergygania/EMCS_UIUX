using App.Data.Caching;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.Imex
{
    public partial class ImexController
    {

        [Route("hsregulationmapping")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult HsRegulationMapping()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.Message = TempData["Message"] + "";
            this.PaginatorBoot.Remove("SessionTRN");
            var model = new RegulationManagementView();
            return View("hsregulationmapping", model);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSRegulationMapping")]
        public ActionResult HsRegulationMappingPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return HsRegulationMappingPageXt();
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSRegulationMapping")]
        public ActionResult HsRegulationMappingPageXt()
        {
            var initPaging = HSCodeListService.InitializePaging(Request);
            int countList = 0;
            this.PaginatorBoot.Remove("SessionTRN");

            Func<RegulationManagementView, List<Data.Domain.SP_HSRegulation>> func = delegate(RegulationManagementView crit)
            {
                var param = Request["params"];

                const string cacheName = "App.imex.FilterHSRegulation";
                ICacheManager _cacheManager = new MemoryCacheManager();

                string key = string.Format(cacheName);


                if (!string.IsNullOrEmpty(param))
                {
                    if (!initPaging.IsPaging) _cacheManager.Remove(cacheName);

                    var filter = _cacheManager.Get(key, () =>
                    {
                        JavaScriptSerializer ser = new JavaScriptSerializer();
                        crit = ser.Deserialize<RegulationManagementView>(param);
                        return crit;
                    });

                    crit = filter;
                }

                countList = Service.Imex.HsRegulation.SPCount_HSRegulation(initPaging.StartNumber, initPaging.EndNumber, crit.selIssueBy, crit.HSDescription, crit.selHSCode, crit.selOrderMethods, initPaging.OrderBy);
                var tbl = Service.Imex.HsRegulation.SP_HSRegulation(initPaging.StartNumber, initPaging.EndNumber, crit.selIssueBy, crit.HSDescription, crit.selHSCode, crit.selOrderMethods, initPaging.OrderBy);

                return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.HSCode).ToList();
            };

            var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.SP_HSRegulation>("SessionTRN", func).Pagination.ToJsonResult();
            return Json(new { paging = paging, totalcount = countList }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult HsRegulationMappingPageXt()
        //{
        //    Func<RegulationManagementView, List<Data.Domain.SP_HSRegulation>> func = delegate (RegulationManagementView crit)
        //    {
        //        var param = Request["params"];
        //        if (!string.IsNullOrEmpty(param))
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            crit = ser.Deserialize<RegulationManagementView>(param);
        //        }


        //        var tbl = Service.Imex.HsRegulation.SP_HSRegulation(crit.selIssueBy, crit.HSDescription, crit.selHSCode, crit.selOrderMethods);

        //        //if (crit.Status.HasValue)
        //        //    tbl = tbl.Where(w => w.Status == crit.Status.Value).ToList();

        //        //if (crit.selRegulation != null)
        //        //    tbl = tbl.Where(w => crit.selRegulation.Any(a => a == w.RegulationManagementID.ToString())).ToList();

        //        if (!string.IsNullOrEmpty(crit.HSDescription))
        //            tbl = tbl.Where(w => (w.HSCode.ToString() + " | " + w.HSDescription).ToLower().Contains(crit.HSDescription.ToLower())).ToList();

        //        //if (crit.selIssueBy != null)
        //        //    tbl = tbl.Where(w => crit.selIssueBy.Any(a => a == w.IssuedBy)).ToList();

        //        //if (crit.IssuedDateSta.HasValue && crit.IssuedDateFin.HasValue)
        //        //    tbl = tbl.Where(w => w.IssuedDate >= crit.IssuedDateSta.Value && w.IssuedDate <= crit.IssuedDateFin.Value).ToList();

        //        if (crit.selHSCode != null)
        //            tbl = tbl.Where(w => crit.selHSCode.Any(a => a == w.HSID.ToString())).ToList();

        //        //if (crit.selLartas != null)
        //        //    tbl = tbl.Where(w => crit.selLartas.Any(a => a == w.LartasId.ToString())).ToList();

        //        if (crit.selOrderMethods != null)
        //            tbl = tbl.Where(w => crit.selOrderMethods.Any(a => a == w.OMCode)).ToList();

        //        return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.HSCode).ToList();
        //    };

        //    var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.SP_HSRegulation>("SessionTRN", func).Pagination.ToJsonResult();
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult RegulationListNew(string HSCode)
        {
            var list = Service.Imex.HsRegulation.GetRegulationByHSCode(HSCode).ToList();
            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RegulationList(int id, int hsid)
        {
            var list = Service.Imex.HsRegulation.GetRegulationHsList(hsid).ToList();
            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSRegulationMapping")]
        public JsonResult DownloadHSRegulationToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadHSRegulationManagement data = new Helper.Service.DownloadHSRegulationManagement();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}