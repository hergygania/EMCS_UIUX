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
    public class DownloadCommodityMaping : Controller
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

            var tbl = App.Service.Imex.CommodityMapping.GetList();

            tbl = tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.CommodityCode).ToList();
                                                                                
            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(rowNumber - 1);
                row.CreateCell(1).SetCellValue(data.CommodityCode);
                row.CreateCell(2).SetCellValue(data.CommodityName);
                row.CreateCell(3).SetCellValue(data.HSCode);
                row.CreateCell(4).SetCellValue(data.HSDescription);
                row.CreateCell(5).SetCellValue(data.Status == 1 ? "Active" : "Deactive");

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "CommodityMaping.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//No
            sheet.SetColumnWidth(1, 20 * 256);//Commodity Code
            sheet.SetColumnWidth(2, 20 * 256);//Commodity Descritpion
            sheet.SetColumnWidth(3, 20 * 256);//HS Code
            sheet.SetColumnWidth(4, 50 * 256);//HS Descritpion
            sheet.SetColumnWidth(5, 20 * 256);//Status

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("Commodity Code");
            headerRow.CreateCell(2).SetCellValue("Commodity Description");
            headerRow.CreateCell(3).SetCellValue("HS Code");
            headerRow.CreateCell(4).SetCellValue("HS Description");
            headerRow.CreateCell(5).SetCellValue("Status");

            return headerRow;
        }
    }
}