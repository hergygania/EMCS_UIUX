
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace App.Web.Controllers
{

	[myAuthorize(Roles = "Master")]
	public partial class MasterController : App.Framework.Mvc.BaseController
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

        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
    }
}