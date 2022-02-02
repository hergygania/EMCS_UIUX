using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DheBankIndonesia()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DHEBankIndonesia")]
        //public ActionResult RDheBiListPage(DateTime startDate, DateTime endDate, string category, string exportType)
        //{
        //    this.PaginatorBoot.Remove("SessionTRN");
        //    return RDheBiPageXt(startDate, endDate, category, exportType);
        //}

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DHEBankIndonesia")]
        //public ActionResult RDheBiPageXt(DateTime startDate, DateTime endDate, string category, string exportType)
        //{
        //    Func<Data.Domain.EMCS.SpRDheBi, IList<Data.Domain.EMCS.SpRDheBi>> func = delegate
        //    {
        //        var list = Service.EMCS.SvcRDheBi.GetDhebiList(startDate, endDate, category, exportType);
        //        return list.ToList();
        //    };

        //    var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DHEBankIndonesia")]
        public ActionResult RDheBiListPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RDheBiPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DHEBankIndonesia")]
        public ActionResult RDheBiPageXt()
        {
            Func<Data.Domain.EMCS.RDheBIListFilter, List<Data.Domain.EMCS.SpRDheBi>> func = delegate (App.Data.Domain.EMCS.RDheBIListFilter filter)
            {
                var param = Request["params"];

                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.EMCS.RDheBIListFilter>(param);
                }
                var list = Service.EMCS.SvcRDheBi.GetDhebiList_old(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDhebiList(DateTime startDate, DateTime endDate, string category, string exportType)
        {
            var data = Service.EMCS.SvcRDheBi.GetDhebiList(startDate, endDate, category, exportType);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DHEBankIndonesia")]
        public ActionResult DownloadDhebi(DateTime startDate,DateTime endDate, string category, string exportType)
        {
            var data = Service.EMCS.SvcRDheBi.GetDhebiList(startDate, endDate, category, exportType);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_DHE_BI.xls");

            MemoryStream output = Service.EMCS.SvcRDheBi.GetDhebiStream(data, fileExcel);

            return File(output.ToArray(), "application/vnd.ms-excel", "Template_DHE_BI_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
    }
}