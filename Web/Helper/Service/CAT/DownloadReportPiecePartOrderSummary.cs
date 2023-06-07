using App.Web.Models.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadReportPiecePartOrderSummary : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(RptPiecePartOrderSummaryFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.Report.RptPiecePartOrderSummaryList.GetList(filter.ref_part_no, filter.model, filter.prefix, filter.smcs, filter.component, filter.mod, filter.DateFilter);

                tbl = tbl.OrderByDescending(o => o.RefPartNo).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.RefPartNo);
                    row.CreateCell(1).SetCellValue(data.Model);
                    row.CreateCell(2).SetCellValue(data.Prefix);
                    row.CreateCell(3).SetCellValue(data.SMCS);
                    row.CreateCell(4).SetCellValue(data.Component);
                    row.CreateCell(5).SetCellValue(data.MOD);
                    row.CreateCell(6).SetCellValue(data.Last12mTrans);
                    row.CreateCell(7).SetCellValue(data.NeedToRebuid);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportPiecePartOrdersSummary.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportPiecePartOrdersSummary.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Ref Parts Number
            sheet.SetColumnWidth(1, 20 * 256);//Model
            sheet.SetColumnWidth(2, 20 * 256);//Perfix
            sheet.SetColumnWidth(3, 20 * 256);//SCMS   
            sheet.SetColumnWidth(4, 20 * 256);//Component   
            sheet.SetColumnWidth(5, 20 * 256);//MOD   
            sheet.SetColumnWidth(6, 20 * 256);//Last 12m Trasaction   
            sheet.SetColumnWidth(7, 20 * 256);//Need To Rebuild   

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Ref. Parts Number");
            headerRow.CreateCell(1).SetCellValue("Model");
            headerRow.CreateCell(2).SetCellValue("Prefix");
            headerRow.CreateCell(3).SetCellValue("SMCS");
            headerRow.CreateCell(4).SetCellValue("Component");
            headerRow.CreateCell(5).SetCellValue("MOD");
            headerRow.CreateCell(6).SetCellValue("last 12m Trasaction");
            headerRow.CreateCell(7).SetCellValue("Need To Rebuild");

            return headerRow;
        }
    }
}