using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service
{
    public class DownloadHSRegulationManagement : Controller
    {
        private HSSFWorkbook workbook = new HSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel()
        {
            //Create new Excel Sheet
            sheet = CreateSheet();

            //Create a header row
            CreateHeaderRow(sheet);

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            var count = App.Service.Imex.HsRegulation.SPCount_HSRegulation(0, 5, null, "", null, null, "");
            var tbl = App.Service.Imex.HsRegulation.SP_HSRegulation(0, count, null, "", null, null, "");

            tbl = tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.HSCode).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.HSCode);
                row.CreateCell(1).SetCellValue(data.HSDescription);
                row.CreateCell(2).SetCellValue(data.BeaMasuk != null ? data.BeaMasuk.Value.ToString() : "0");
                row.CreateCell(3).SetCellValue(data.OMCode);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "HSRegulationManagement.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 10 * 256);//HS Code
            sheet.SetColumnWidth(1, 50 * 256);//HS Description
            sheet.SetColumnWidth(2, 10 * 256);//Bea Masuk Duty
            sheet.SetColumnWidth(3, 10 * 256);//Order Mehtod

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("HS Code");
            headerRow.CreateCell(1).SetCellValue("HS Description");
            headerRow.CreateCell(2).SetCellValue("Bea Masuk (Duty)");
            headerRow.CreateCell(3).SetCellValue("Order Method");

            return headerRow;
        }
    }
}