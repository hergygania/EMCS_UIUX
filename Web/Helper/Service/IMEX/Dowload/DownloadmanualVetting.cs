using App.Web.Controllers.Vetting;
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
    public class DownloadmanualVetting : Controller
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

            var count = App.Service.Vetting.ManualVetting.GetSPCount_Paging(Domain.SiteConfiguration.UserName, 0, 5, "", "", "", "");
            var tbl = App.Service.Vetting.ManualVetting.GetSPList_Paging(Domain.SiteConfiguration.UserName, 0, count, "", "", "", "");

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.PRIMPSO);
                row.CreateCell(1).SetCellValue(data.PartNumber);
                row.CreateCell(2).SetCellValue(data.ManufacturingCode);
                row.CreateCell(3).SetCellValue(data.PartName);
                row.CreateCell(4).SetCellValue(data.CustomerRef);
                row.CreateCell(5).SetCellValue(data.CustomerCode);
                row.CreateCell(6).SetCellValue(data.Status);
                row.CreateCell(7).SetCellValue(data.OrderClassCode);
                row.CreateCell(8).SetCellValue(data.ProfileNumber ?? 0);
                row.CreateCell(9).SetCellValue(data.OMCode);
                row.CreateCell(10).SetCellValue(data.RemarksName);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "ManualVetting.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//PRIMPSO
            sheet.SetColumnWidth(1, 20 * 256);//Parts Number
            sheet.SetColumnWidth(2, 20 * 256);//Manufacturing Code
            sheet.SetColumnWidth(3, 20 * 256);//Parts Name
            sheet.SetColumnWidth(4, 20 * 256);//Customer Ref
            sheet.SetColumnWidth(5, 20 * 256);//Customer Code
            sheet.SetColumnWidth(6, 20 * 256);//Status
            sheet.SetColumnWidth(7, 20 * 256);//Order Class Code
            sheet.SetColumnWidth(8, 20 * 256);//Profile Number
            sheet.SetColumnWidth(9, 20 * 256);//Order Method
            sheet.SetColumnWidth(10, 20 * 256);//Remarks

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("PRIMPSO");
            headerRow.CreateCell(1).SetCellValue("Parts Number");
            headerRow.CreateCell(2).SetCellValue("Manufacturing Code");
            headerRow.CreateCell(3).SetCellValue("Parts Name");
            headerRow.CreateCell(4).SetCellValue("Customer Ref");
            headerRow.CreateCell(5).SetCellValue("Customer Code");
            headerRow.CreateCell(6).SetCellValue("Status");
            headerRow.CreateCell(7).SetCellValue("Order Class Code");
            headerRow.CreateCell(8).SetCellValue("Profile Number");
            headerRow.CreateCell(9).SetCellValue("Order Method");
            headerRow.CreateCell(10).SetCellValue("Remarks");

            return headerRow;
        }
    }
}