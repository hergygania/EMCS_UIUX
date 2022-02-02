using App.Web.Models.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadReportForcastAccuracyDetail : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(ForecastAccuracyFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.ForecastAccuracyDetailList.GetList(filter.customer_id, filter.month_id, filter.store_id, filter.year);

                tbl = tbl.OrderByDescending(o => o.Store).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.Store);
                    row.CreateCell(1).SetCellValue(data.RefPartNumber);
                    row.CreateCell(2).SetCellValue(data.Customer);
                    row.CreateCell(3).SetCellValue(data.Component);
                    row.CreateCell(4).SetCellValue(data.Model);
                    row.CreateCell(5).SetCellValue(data.Prefix);
                    row.CreateCell(6).SetCellValue(data.Forecast);
                    row.CreateCell(7).SetCellValue(data.Sales);
                    row.CreateCell(8).SetCellValue(data.ForecastedSales);
                    row.CreateCell(9).SetCellValue(data.UnForecastedSales);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportForecastAccuracyDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportForecastAccuracyDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Store
            sheet.SetColumnWidth(1, 20 * 256);//Part No.
            sheet.SetColumnWidth(2, 20 * 256);//Customer
            sheet.SetColumnWidth(3, 20 * 256);//Component
            sheet.SetColumnWidth(4, 20 * 256);//Model
            sheet.SetColumnWidth(5, 20 * 256);//Prefix
            sheet.SetColumnWidth(6, 20 * 256);//Forcast
            sheet.SetColumnWidth(7, 20 * 256);//Sales
            sheet.SetColumnWidth(8, 20 * 256);//Forcast Sales
            sheet.SetColumnWidth(9, 20 * 256);//Unforcasted Sales

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Store");
            headerRow.CreateCell(1).SetCellValue("Part No.");
            headerRow.CreateCell(2).SetCellValue("Customer");
            headerRow.CreateCell(3).SetCellValue("Component");
            headerRow.CreateCell(4).SetCellValue("Model");
            headerRow.CreateCell(5).SetCellValue("Prefix");
            headerRow.CreateCell(6).SetCellValue("Forcast");
            headerRow.CreateCell(7).SetCellValue("Sales");
            headerRow.CreateCell(8).SetCellValue("Forcast Sales");
            headerRow.CreateCell(9).SetCellValue("Unforcasted Sales");

            return headerRow;
        }
    }
}