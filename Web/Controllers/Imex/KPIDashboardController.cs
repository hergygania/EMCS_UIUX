using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain.KPIDashboard;
using App.Service.Imex;

namespace App.Web.Controllers.Imex
{
    public class KPIDashboardController : Controller
    {
        KPIDashboardBLL KPIDashboardBLL = new KPIDashboardBLL();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetSCISvsCCOS()
        {
            DataTable data = KPIDashboardBLL.GetSCISvsCCOS();
            List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
            foreach (DataRow dr1 in data.Rows)
            {
                Dictionary<string, object> aSeries = new Dictionary<string, object>();
                aSeries["name"] = dr1["Description"];
                aSeries["y"] = dr1["Percentage"];
                //aSeries["data"] = new List<decimal>();
                //int N = dr1.ItemArray.Length;
                //for (int i = 0; i < N; i++)
                //{
                //    if (i == 0)
                //    {
                //        ((List<decimal>)aSeries["data"]).Add(Convert.ToDecimal(dr1[i]));
                //    }
                //    else
                //    {
                //        ((List<decimal>)aSeries["data"]).Add(Convert.ToDecimal(dr1[i]));
                //    }
                //}
                allSeries.Add(aSeries);
            }
            string jsonSeries = Newtonsoft.Json.JsonConvert.SerializeObject(allSeries);
            return Json(jsonSeries, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogChart()
        {
            DataTable data = KPIDashboardBLL.GetChangeLogChart();
            List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
            foreach (DataRow dr1 in data.Rows)
            {
                Dictionary<string, object> aSeries = new Dictionary<string, object>();
                aSeries["name"] = dr1["Date"];
                aSeries["y"] = dr1["Total"];
                //aSeries["x"] = dr1["Date"];
                //int N = dr1.ItemArray.Length;
                //for (int i = 1; i < N; i++)
                //{
                //    ((List<int>)aSeries["data"]).Add((int)dr1[i]);
                //}
                allSeries.Add(aSeries);
            }
            string jsonSeries = Newtonsoft.Json.JsonConvert.SerializeObject(allSeries);
            return Json(jsonSeries, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetChangeLogChartNewMapping()
        {
            DataTable data = KPIDashboardBLL.GetChangeLogChartNewMapping();
            List<Dictionary<string, object>> allSeries = new List<Dictionary<string, object>>();
            foreach (DataRow dr1 in data.Rows)
            {
                Dictionary<string, object> aSeries = new Dictionary<string, object>();
                aSeries["name"] = dr1["Date"];
                aSeries["y"] = dr1["Total"];
                //aSeries["x"] = dr1["Date"];
                //int N = dr1.ItemArray.Length;
                //for (int i = 1; i < N; i++)
                //{
                //    ((List<int>)aSeries["data"]).Add((int)dr1[i]);
                //}
                allSeries.Add(aSeries);
            }
            string jsonSeries = Newtonsoft.Json.JsonConvert.SerializeObject(allSeries);
            return Json(jsonSeries, JsonRequestBehavior.AllowGet);
        }
    }
}