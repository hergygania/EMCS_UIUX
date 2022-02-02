using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace App.Web.Controllers
{
	public partial class ReportController
	{

		public ActionResult zTableInfo(string id)
		{
		 string ret="";
			if(!string.IsNullOrEmpty(id))
			{
				DataTable dt1 = Service.Report.zIndex.TableDef(id);
				ret = ret + ExportToScreen(dt1, "Table-Definition");
				ret = ret + "<hr>";
			}

			DataTable dt = Service.Report.zIndex.TableIndex(id);
			ret = ret + ExportToScreen(dt, "Index-Table");

			return Content(ret);
		}

		public string ExportToScreen(DataTable dt, string tile)
		{
			string tab = "";
			var sb = new System.Text.StringBuilder();
			sb.Append("<h4>" + tile + "</h4>");
			sb.Append("<table cellspacing='2' border='1' style='width:100%;	border-collapse: collapse' class='table-bordered table2excel'>");
			sb.Append("<tr>");
			foreach(DataColumn dc in dt.Columns)
			{
				sb.Append("<th>" + dc.ColumnName + "</th>");
			}
			sb.Append("</tr>");
			
			int i;
			foreach(DataRow dr in dt.Rows)
			{
				sb.Append("<tr>");
				for(i = 0; i < dt.Columns.Count; i++)
				{
					sb.Append("<td>" + tab + dr[i] + "</td>");
				}
				sb.Append("</tr>");
			}
			sb.Append("</table>");

			return sb.ToString();
		}
		
	}
}