using App.Web.Models.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadReportOutstandingOldCoreSummary : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(RptOutstandingOldCoreSummaryFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.Report.RptOutstandingOldCoreSummaryList.GetList(filter.storeid ?? "", filter.DateFilter);

                tbl = tbl.OrderByDescending(o => o.Store).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.Store);
                    row.CreateCell(1).SetCellValue(data.OutOldCore0day);
                    row.CreateCell(2).SetCellValue(data.OutOldCore11day);
                    row.CreateCell(3).SetCellValue(data.OutOldCore21day);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportOutstandingOldCoreSummary.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportOutstandingOldCoreSummary.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Store
            sheet.SetColumnWidth(1, 20 * 256);//Out Standing Old Core Elapsed Day 0 - 10 Days
            sheet.SetColumnWidth(2, 20 * 256);//Out Standing Old Core Elapsed Day 11 - 20 Days
            sheet.SetColumnWidth(3, 20 * 256);//Out Standing Old Core Elapsed Day >= 21 Days

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Store");
            headerRow.CreateCell(1).SetCellValue("Out Standing Old Core Elapsed Day 0 - 10 Days");
            headerRow.CreateCell(2).SetCellValue("Out Standing Old Core Elapsed Day 11 - 20 Days");
            headerRow.CreateCell(3).SetCellValue("Out Standing Old Core Elapsed Day >= 21 Days");

            return headerRow;
        }
    }
}