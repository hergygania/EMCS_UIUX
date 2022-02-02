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
    public class DownloadOrderMethod : Controller
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

            //var countList = App.Service.Master.HSCodeLists.SP_GetCountPerPage(0, 5, "", "").FirstOrDefault();

            //var tbl = App.Service.Master.HSCodeLists.SP_GetListPerPage(0, countList, "", "");

            var tbl = App.Service.Master.OrderMethods.GetList();

            tbl = tbl.OrderBy(o => o.OMRank).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.OMCode);
                row.CreateCell(1).SetCellValue(data.Description);
                row.CreateCell(2).SetCellValue(data.OMRank);
                string route = "Mix";
                if(data.VettingRoute == 1) route = "Normal";
                if(data.VettingRoute == 2) route = "Survey"
                        ;
                row.CreateCell(3).SetCellValue(route);    
                row.CreateCell(4).SetCellValue(data.Status == 1 ? "Active" : "Deactive");

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "OrderMethod.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//HS Code
            sheet.SetColumnWidth(1, 20 * 256);//Bea Masuk
            sheet.SetColumnWidth(2, 40 * 256);//Description
            sheet.SetColumnWidth(3, 20 * 256);//OM Code
            sheet.SetColumnWidth(4, 20 * 256);//Status  

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("HS Code");
            headerRow.CreateCell(1).SetCellValue("Bea Masuk");
            headerRow.CreateCell(2).SetCellValue("Description");
            headerRow.CreateCell(3).SetCellValue("Order Method Code");
            headerRow.CreateCell(4).SetCellValue("Status");

            return headerRow;
        }
    }
}