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
using NPOI.SS.UserModel;
using App.Data.Domain.EMCS;
using NPOI.HSSF.UserModel;
using NPOI.SS.Util;
using System.Text.RegularExpressions;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ExportProblem()
        {
            ViewBag.AppTitle = "Export Monitoring & Controlling System";
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult RExportProblemPage(string startDate, string endDate)
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RExportProblemPageXt(startDate, endDate);
        }

        public ActionResult RExportProblemPageXt(string startDate, string endDate)
        {
            Func<CiplListFilter, List<SpRProblemHistory>> func = delegate (CiplListFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<CiplListFilter>(param);
                }
                var list = Service.EMCS.SvcRExportProblem.ExportProblemList(filter, startDate, endDate);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportProblem")]
        public JsonResult GetExportProblemList(string startDate, string endDate)
        {
            var data = Service.EMCS.SvcRExportProblem.GetReportProblemHistory(startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadProblemHistory(string startDate, string endDate)
        {
            var data = Service.EMCS.SvcRExportProblem.GetProblemHistoryListExport(startDate, endDate);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\Template_ProblemHistory.xls");
            MemoryStream output = Service.EMCS.SvcRExportProblem.GetProblemHistoryStream(data, fileExcel);

            return File(output.ToArray(), "application/vnd.ms-excel", "ProblemHistory_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ExportProblem")]
        public FileResult DownloadProblemAnalysis(string startDate, string endDate)
        {
            MemoryStream output = new MemoryStream();
            var path = Server.MapPath(@"~\Content\EMCS\Templates\New_Template_Problem_History.xls");

            IWorkbook workbook;
            HSSFChart chart_0;// chart 1
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(file);
                file.Close();
            }

            HSSFSheet sheet = (HSSFSheet)workbook.GetSheet(workbook.GetSheetAt(1).SheetName);
            var charts = HSSFChart.GetSheetCharts(sheet);
            chart_0 = charts[0];

            ISheet sheet1 = workbook.GetSheet("Problem List");
            ISheet sheet2 = workbook.GetSheet("Chart");

            var header = sheet1.CreateRow(0);
            header.CreateCell(0).SetCellValue("");
            header.CreateCell(1).SetCellValue("");

            //RproblemCategory data = Service.EMCS.SvcRExportProblem.GetReportProblemHistory(startDate, endDate);
            //var problemList = data.Rows;
            //int startRow = 2;
            //int startRowChart = 3;

            //foreach (var itemProblemCategory in problemList)
            //{
            //    var problemChartRow = sheet2.CreateRow(startRowChart);
            //    problemChartRow.CreateCell(0).SetCellValue(itemProblemCategory.Name);
            //    problemChartRow.CreateCell(1).SetCellValue(itemProblemCategory.TotalCategory);
            //    problemChartRow.CreateCell(2).SetCellValue(Convert.ToDouble(itemProblemCategory.TotalCategoryPercentage) / 100);

            //    var problemCategoryRow = sheet1.CreateRow(startRow);
            //    problemCategoryRow.CreateCell(0).SetCellValue(itemProblemCategory.Id);
            //    problemCategoryRow.CreateCell(1).SetCellValue(itemProblemCategory.Name);
            //    problemCategoryRow.CreateCell(7).SetCellValue(Convert.ToDouble(itemProblemCategory.TotalCategory));
            //    problemCategoryRow.CreateCell(8).SetCellValue(Convert.ToDouble(itemProblemCategory.TotalCategoryPercentage));

            //    foreach (var itemProblemCase in itemProblemCategory.Children)
            //    {
            //        startRow = startRow + 1;
            //        var problemCaseRow = sheet1.CreateRow(startRow);
            //        problemCaseRow.CreateCell(2).SetCellValue(itemProblemCase.Name);
            //        problemCaseRow.CreateCell(6).SetCellValue(itemProblemCase.TotalCases);

            //        foreach (var itemProblemReason in itemProblemCase.Children)
            //        {
            //            startRow = startRow + 1;
            //            var problemCaseReason = sheet1.CreateRow(startRow);
            //            problemCaseReason.CreateCell(3).SetCellValue(itemProblemReason.Name);
            //            problemCaseReason.CreateCell(4).SetCellValue(itemProblemReason.Impact);
            //            problemCaseReason.CreateCell(5).SetCellValue(itemProblemReason.TotalReason);
            //        }
            //    }

            //    startRow++;
            //    startRowChart++;
            //}

            //startRow = startRow + 1;
            //var totalRow0 = sheet1.CreateRow(startRow);
            //totalRow0.CreateCell(0).SetCellValue(data.Rows[0].Name);
            //totalRow0.CreateCell(8).SetCellValue(Convert.ToDouble(data.Rows[0].TotalCategory));

            //startRow = startRow + 1;
            //var totalRow1 = sheet1.CreateRow(startRow);
            //totalRow1.CreateCell(0).SetCellValue("RESULT");

            //string resultString = "POOR";
            //int totalCategory = Convert.ToInt32(data.Rows[1].TotalCategory);

            //if (totalCategory <= 10 && totalCategory > 0)
            //{
            //    resultString = "GOOD";
            //}
            //else if (totalCategory <= 0)
            //{
            //    resultString = "EXCELENT";
            //}

            //totalRow0.CreateCell(8).SetCellValue(resultString);
            //CellRangeAddressBase chartRangeG = new CellRangeAddress(3, 6, 2, 2);
            //chart_0.Series[0].SetValuesCellRange(chartRangeG);


            //workbook.Write(output);
            return File(output.ToArray(), "application/vnd.ms-excel", "Problem_Analysis_" + DateTime.Now + ".xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        public JsonResult GetProblemByCategory(string startDate, string endDate)
        {
            var data = Service.EMCS.SvcRExportProblem.GetProblemHistoryList(startDate, endDate).Where(i => i.ParentId == 0).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}