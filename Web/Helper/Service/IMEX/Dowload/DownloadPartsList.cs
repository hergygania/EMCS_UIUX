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
    public class DownloadPartsList : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel()
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);


                var count = App.Service.Master.PartsLists.SP_GetCountPerPage(0, 5, "", "").FirstOrDefault();
                var tbl = App.Service.Master.PartsLists.SP_GetListPerPage(0, count, "", "");

                tbl = tbl.OrderByDescending(o => o.ModifiedDate).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.PartsNumber);
                    row.CreateCell(1).SetCellValue(data.PartsName);
                    row.CreateCell(2).SetCellValue(data.Description);
                    //row.CreateCell(3).SetCellValue(data.OMCode);    
                    row.CreateCell(3).SetCellValue(data.Status == 1 ? "Active" : "Deactive");

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "PartsList.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "PartsList.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Parts Number
            sheet.SetColumnWidth(1, 20 * 256);//Parts Name
            sheet.SetColumnWidth(2, 20 * 256);//Description
            //sheet.SetColumnWidth(3, 20 * 256);//Order Method
            sheet.SetColumnWidth(3, 20 * 256);//Status   

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Parts Number");
            headerRow.CreateCell(1).SetCellValue("Parts Name");
            headerRow.CreateCell(2).SetCellValue("Description");
            //headerRow.CreateCell(3).SetCellValue("Order Method");
            headerRow.CreateCell(3).SetCellValue("Status");

            return headerRow;
        }
    }
}