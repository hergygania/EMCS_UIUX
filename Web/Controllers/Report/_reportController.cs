using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using App.Framework.Mvc;
using App.Service.Report;
using Microsoft.Ajax.Utilities;
using App.Web.App_Start;

namespace App.Web.Controllers
{
    //[myAuthorize(Roles = "Report")]
    //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
    public partial class ReportController : BaseController
    {
        public void ExportToExcel(DataTable dt, string filenName)
        {
            string attachment = String.Format("attachment; filename={0}.xls", filenName);
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + Regex.Replace(dc.ColumnName, "(\\B[A-Z])", " $1"));
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i]);
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        public IEnumerable<SelectListItem> GetCustomers()
        {
            return DocumentAwaitingAcks.GetListCustomers().Select(a =>
                new SelectListItem
                {
                    Text = a.rack_CustName,
                    Value = a.rack_CustID
                }
                );
        }

        public string FormatStringNumber(string value)
        {
            var arrayNumber = value.Split('.');
            if (arrayNumber.Length > 1)
            {
                if (int.Parse(arrayNumber[arrayNumber.Length - 1]) > 0)
                    return GetStringNumber(arrayNumber[0]) + "," + arrayNumber[1];
                return GetStringNumber(arrayNumber[0]);
            }
            return GetStringNumber(arrayNumber[0]);
        }

        public string GetStringNumber(string value)
        {
            var loop = (int)Math.Ceiling((double)value.Length / 3);
            string[] arrayString = new string[loop];
            for (int i = 1; i <= loop; i++)
            {
                var sisa = value.Length - 3 * (i - 1);
                if (sisa > 3)
                {
                    var substring = value.Substring(value.Length - (3 * i), 3);
                    if (loop - i != 0)
                        arrayString[loop - i] = string.Format("{0}{1}", ".", substring);
                    else
                        arrayString[loop - i] = substring;
                }
                else
                {
                    arrayString[loop - i] = value.Substring(0, sisa);
                    ;
                }
            }
            return string.Join("", arrayString);
        }


        public List<Select2Result> GetListStoreByUserName(string userName)
        {
            return Service.Master.Stores.GetListByUser(userName);

        }

        public List<Select2Result> GetListAreaByUserName(string userName)
        {
            return Service.Master.Area.GetListByUser(userName);


        }

        public List<Select2Result> GetListHubByUserName(string userName)
        {
            return Service.Master.Hub.GetListByUser(userName);

        }

        public JsonResult GetFilterBy(int index)
        {

            if (index == 1)
            {
                var filterData = GetListHubByUserName(Domain.SiteConfiguration.UserName);
                return Json(filterData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var filterData = GetListAreaByUserName(Domain.SiteConfiguration.UserName);
                return Json(filterData, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetStore(string filter_type, int? id)
        {

            List<Select2Result> filterData = new List<Select2Result>();
            if (!filter_type.IsNullOrWhiteSpace() && id.HasValue)
            {
                filterData = Service.Master.Stores.GetSelectList(filter_type, id);
            }
            else
            {
                filterData = GetListStoreByUserName(SiteConfiguration.UserName);

            }
            return Json(filterData, JsonRequestBehavior.AllowGet);
        }


    }
}


