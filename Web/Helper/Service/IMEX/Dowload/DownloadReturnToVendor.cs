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
    public class DownloadReturnToVendor : Controller
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

            var partList = App.Service.Imex.ReturToVendor.GetList("", null, null, "", "", "", "");
            var tbl = partList
            .GroupBy(g => new { g.InvoiceNo, g.InvoiceDate, g.JCode })
            .Select(g => new Data.Domain.PartsOrder
            {
                InvoiceNo = g.Key.InvoiceNo,
                InvoiceDate = g.Key.InvoiceDate,
                JCode = g.Key.JCode,
                AgreementType = g.Max(m => m.AgreementType),
                StoreNumber = g.Max(m => m.StoreNumber),
                ShippingIDASN = g.Max(m => m.ShippingIDASN),
                DA = g.Max(m => m.DA),
                TotalAmount = g.Max(m => m.TotalAmount),
                PartsOrderID = g.Max(m => m.PartsOrderID),
                ModifiedBy = g.Max(m => m.ModifiedBy),
                ModifiedDate = g.Max(m => m.ModifiedDate)
            }).ToList();

            tbl = tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.InvoiceNo).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.InvoiceNo);
                row.CreateCell(1).SetCellValue(data.InvoiceDate.ToString("MM/dd/yyyy"));
                row.CreateCell(2).SetCellValue(data.AgreementType);
                row.CreateCell(3).SetCellValue(data.JCode);
                row.CreateCell(4).SetCellValue(data.StoreNumber);
                row.CreateCell(5).SetCellValue(data.DA);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "ReturnToVendor.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Invoice No
            sheet.SetColumnWidth(1, 20 * 256);//Invoice Date
            sheet.SetColumnWidth(2, 20 * 256);//Agreement Type
            sheet.SetColumnWidth(3, 20 * 256);//J-Code
            sheet.SetColumnWidth(4, 20 * 256);//Store Number
            sheet.SetColumnWidth(5, 20 * 256);//DA Number

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Invoice No");
            headerRow.CreateCell(1).SetCellValue("Invoice Date");
            headerRow.CreateCell(2).SetCellValue("Agreement Type");
            headerRow.CreateCell(3).SetCellValue("J-Code");
            headerRow.CreateCell(4).SetCellValue("Store Number");
            headerRow.CreateCell(5).SetCellValue("DA Number");

            return headerRow;
        }
    }
}