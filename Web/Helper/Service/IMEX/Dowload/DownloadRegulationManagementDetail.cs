using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service
{
    public class DownloadRegulationManagementDetail : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel()
        {
            //Create new Excel Sheet
            sheet = CreateSheet();

            //Create a header row
            CreateHeaderRow(sheet);

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            var tbl = App.Service.Imex.RegulationManagement.GetList();

            tbl = tbl.OrderBy(o => o.NoPermitCategory).ThenBy(o => o.CodePermitCategory).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.NoPermitCategory);
                row.CreateCell(1).SetCellValue(data.CodePermitCategory);
                row.CreateCell(2).SetCellValue(data.PermitCategoryName + " " + data.PermitCategoryName);
                row.CreateCell(3).SetCellValue(data.HSCode);
                row.CreateCell(4).SetCellValue(data.Lartas);
                row.CreateCell(5).SetCellValue(data.Permit);
                row.CreateCell(6).SetCellValue(data.Regulation);
                row.CreateCell(7).SetCellValue(data.Date != null ? data.Date.Value.ToString("MM/dd/yyyy") : "");
                row.CreateCell(8).SetCellValue(data.Description);
                row.CreateCell(9).SetCellValue(data.OMCode);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "RegulationManagementDeatil.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//No Permit Category
            sheet.SetColumnWidth(1, 20 * 256);//Code Permit Category
            sheet.SetColumnWidth(2, 20 * 256);//Permit Category
            sheet.SetColumnWidth(3, 20 * 256);//HS Code
            sheet.SetColumnWidth(4, 20 * 256);//Lartas
            sheet.SetColumnWidth(5, 40 * 256);//Permit
            sheet.SetColumnWidth(6, 50 * 256);//Regulation
            sheet.SetColumnWidth(7, 10 * 256);//Date
            sheet.SetColumnWidth(8, 40 * 256);//Description
            sheet.SetColumnWidth(9, 10 * 256);//Order Method

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("Code Permit Category");
            headerRow.CreateCell(2).SetCellValue("Permit Category");
            headerRow.CreateCell(3).SetCellValue("HS Code");
            headerRow.CreateCell(4).SetCellValue("Lartas");
            headerRow.CreateCell(5).SetCellValue("Permit");
            headerRow.CreateCell(6).SetCellValue("Regulation");
            headerRow.CreateCell(7).SetCellValue("Date");
            headerRow.CreateCell(8).SetCellValue("Description");
            headerRow.CreateCell(9).SetCellValue("Order Method");

            return headerRow;
        }
    }
}