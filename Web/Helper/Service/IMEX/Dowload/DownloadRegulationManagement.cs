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
    public class DownloadRegulationManagement : Controller
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

            var tbl = App.Service.Imex.RegulationManagement.GetListHeader();

            tbl = tbl.OrderBy(o => o.NoPermitCategory).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.NoPermitCategory);
                row.CreateCell(1).SetCellValue(data.CodePermitCategory + " " + data.PermitCategoryName);
                row.CreateCell(2).SetCellValue(data.QTY);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "RegulationManagement.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 5 * 256);//No
            sheet.SetColumnWidth(1, 40 * 256);//Permit Category
            sheet.SetColumnWidth(2, 10 * 256);//Quantity

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("Permit Category");
            headerRow.CreateCell(2).SetCellValue("Quantity");

            return headerRow;
        }
    }
}