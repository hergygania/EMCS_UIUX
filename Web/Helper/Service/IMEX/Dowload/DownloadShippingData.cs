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
    public class DownloadShippingData : Controller
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

            var tbl = App.Service.Imex.ShippingData.GetList("", "", "", "", "", null, null, null, null, null, null);

            tbl = tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.BLAWB).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.BLAWB);
                row.CreateCell(1).SetCellValue(data.Vessel);
                row.CreateCell(2).SetCellValue(data.LoadingPortDesc);
                row.CreateCell(3).SetCellValue(data.DestinationPortDesc);
                row.CreateCell(4).SetCellValue(data.ETD != null ? data.ETD.ToString("MM/dd/yyyy") : "");
                row.CreateCell(5).SetCellValue(data.ETA != null ? data.ETA.ToString("MM/dd/yyyy") : "");
                row.CreateCell(6).SetCellValue(data.totManifest);
                row.CreateCell(7).SetCellValue(data.totPackage);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "ShippingData.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//BL/AWB
            sheet.SetColumnWidth(1, 20 * 256);//Vessel/Voyage
            sheet.SetColumnWidth(2, 20 * 256);//Loading Port
            sheet.SetColumnWidth(3, 20 * 256);//Destination Posrt
            sheet.SetColumnWidth(4, 20 * 256);//ETD
            sheet.SetColumnWidth(5, 20 * 256);//ETA
            sheet.SetColumnWidth(6, 20 * 256);//Manifest
            sheet.SetColumnWidth(7, 20 * 256);//Package

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("BL/AWB");
            headerRow.CreateCell(1).SetCellValue("Vessel/Voyage");
            headerRow.CreateCell(2).SetCellValue("Loading Port");
            headerRow.CreateCell(3).SetCellValue("Destination Port");
            headerRow.CreateCell(4).SetCellValue("ETD");
            headerRow.CreateCell(5).SetCellValue("ETA");
            headerRow.CreateCell(6).SetCellValue("Manifest");
            headerRow.CreateCell(7).SetCellValue("Package");

            return headerRow;
        }
    }
}