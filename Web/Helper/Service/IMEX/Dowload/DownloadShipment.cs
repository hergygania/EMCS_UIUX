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
    public class DownloadShipment : Controller
    {
        private HSSFWorkbook workbook = new HSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(string freight, int vettingRoute)
        {
            int freightId = 2;
            bool isSea = false;
            string route = "Mix";
            if (vettingRoute == 1) route = "Normal";
            if (vettingRoute == 2) route = "Survey";

            if (freight.ToLower() == "sea")
            {
                isSea = true;
                freightId = 1;
            }

            //Create new Excel Sheet
            sheet = CreateSheet();

            //Create a header row
            CreateHeaderRow(sheet);

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            var tbl = App.Service.Vetting.Shipment.GetList(freightId, vettingRoute, "", "", "", "", "", null, null);

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(rowNumber - 1);
                row.CreateCell(1).SetCellValue(data.BLAWB);
                row.CreateCell(2).SetCellValue(data.Vessel);
                row.CreateCell(3).SetCellValue(data.LoadingPortDesc);
                row.CreateCell(4).SetCellValue(data.DestinationPortDesc);
                row.CreateCell(5).SetCellValue(data.ETD != null ? data.ETD.ToString("MM/dd/yyyy") : "");
                row.CreateCell(6).SetCellValue(data.ETA != null ? data.ETA.ToString("MM/dd/yyyy") : "");
                row.CreateCell(7).SetCellValue(data.totManifest);
                row.CreateCell(8).SetCellValue(data.totPackage);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             freight + "-freight-Shipment-" + route + ".xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//No
            sheet.SetColumnWidth(1, 20 * 256);//BL/AWB
            sheet.SetColumnWidth(2, 20 * 256);//Vessel/Voyage
            sheet.SetColumnWidth(3, 20 * 256);//Loading Port
            sheet.SetColumnWidth(4, 20 * 256);//Destination Posrt
            sheet.SetColumnWidth(5, 20 * 256);//ETD
            sheet.SetColumnWidth(6, 20 * 256);//ETA
            sheet.SetColumnWidth(7, 20 * 256);//Manifest
            sheet.SetColumnWidth(8, 20 * 256);//Package

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("BL/AWB");
            headerRow.CreateCell(2).SetCellValue("Vessel/Voyage");
            headerRow.CreateCell(3).SetCellValue("Loading Port");
            headerRow.CreateCell(4).SetCellValue("Destination Port");
            headerRow.CreateCell(5).SetCellValue("ETD");
            headerRow.CreateCell(6).SetCellValue("ETA");
            headerRow.CreateCell(7).SetCellValue("Manifest");
            headerRow.CreateCell(8).SetCellValue("Package");

            return headerRow;
        }
    }
}