using App.Data.Domain.DTS;
using App.Web.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        // GET: Freight
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "Freight")]
        public ActionResult Freight()
        {
            return View();
        }

        public ActionResult FreightCalculatorPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return FreightCalculatorPageXt();
        }

        public JsonResult GetUnitModel()
        {
            var unitModel = Service.DTS.FreightCalculator.GetListModel();
            var data = unitModel.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFreightRouteSalesOption(string type,string key)
        {
            //var key = Request.Form["key"] ?? "";
            //var type = Request.Form["type"] ?? "";
            var listData = Service.DTS.FreightCalculator.GetListFreightRouteOption(key, type);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFreightRouteSalesData(string Origin, string Destination, string Model)
        {
           
            var listData = Service.DTS.FreightCalculator.GetDataFreightRouteSales(Origin, Destination, Model);
            FreightOptions data = new FreightOptions();
            data = listData;
            return Json(new { result = data, message ="success" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFreightOption()
        {
            var key = Request.Form["key"] ?? "";
            var type = Request.Form["type"] ?? "";
            var listData = Service.DTS.FreightCalculator.GetListFreightOption(type, key);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFreightRouteOption()
        {
            var type = Request.Form["type"] ?? "";
            var key = Request.Form["key"] ?? "";
            var listData = Service.DTS.FreightCalculator.GetListFreightRouteOption(key,type);
            var data = listData.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "Freight")]
        public ContentResult FreightCalculatorPageXt()
        {
            var sort = Request.QueryString["sort"];
            var limit = Convert.ToInt32(Request.QueryString["limit"]);
            var offset = Convert.ToInt32(Request.QueryString["offset"]);
            var unit = Request.QueryString["unit"];
            var destination = Request.QueryString["destination"];
             destination = Regex.Replace(destination, @"[^0-9a-zA-Z]+", "");
            var origin = Request.QueryString["origin"];
            origin = Regex.Replace(origin, @"[^0-9a-zA-Z]+", "");
            var dataTable = Service.DTS.FreightCalculator.GetListFilter(sort, limit, offset, destination, origin);
            int dataTotal = Service.DTS.FreightCalculator.GetTotal(destination, origin);

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dataTable.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName.ToLower(), dr[col]);
                }
                rows.Add(row);
            }

            dynamic foo = new ExpandoObject();
            foo.total = dataTotal;
            foo.rows = rows;

            string jsonData = JsonConvert.SerializeObject(foo);
            //return Json(jsonData, JsonRequestBehavior.AllowGet);
            ContentResult results = Content(jsonData);
            return results;
        }


        public ActionResult FreighCostPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return FreighCostXt();
        }

        public ActionResult FreighCostXt()
        {
            Func<App.Data.Domain.DTS.FreightRouteOptions, List<Data.Domain.FreightCost>> func = delegate (App.Data.Domain.DTS.FreightRouteOptions filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.FreightRouteOptions>(param);
                }

                var list = Service.DTS.FreightCalculator.GetListFilter(filter);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFreight()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadFreightEstimateController data = new Helper.Service.DTS.DownloadFreightEstimateController();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadToExcelFreight(string guid)
        {
            return Session[guid] as FileResult;
        }

        [HttpPost]
        public JsonResult GetFreightDetail()
        {
            var origin = Request.Form["origin"] ?? "";
            var dest = Request.Form["destination"] ?? "";
            var etd = Request.Form["etd"] ?? "";
            var model = Request.Form["model"] ?? "";

            var data = Service.DTS.FreightCalculator.GetDetailFreight(origin, dest, model, etd);
            ViewBag.Model = model;
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}