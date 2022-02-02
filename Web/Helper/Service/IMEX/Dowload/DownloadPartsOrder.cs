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
    public class DownloadPartsOrder : Controller
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

            var partList = App.Service.Vetting.PartsOrder.GetList(freightId, null, vettingRoute, "", "", null, null, "", "", "", "");
            var tbl =
            (
                from c in partList
                from si in App.Service.Master.ShippingInstruction.GetList().Where(w => w.ShippingInstructionID == c.ShippingInstructionID).DefaultIfEmpty()
                select new { c, shipnm = si == null ? "" : si.Description }
            )
            .GroupBy(g => new { g.c.InvoiceNo, g.c.InvoiceDate, g.c.JCode })
            .Select(g => new Data.Domain.PartsOrder
            {
                InvoiceNo = g.Key.InvoiceNo,
                InvoiceDate = g.Key.InvoiceDate,
                JCode = g.Key.JCode,
                AgreementType = g.Max(m => m.c.AgreementType),
                StoreNumber = g.Max(m => m.c.StoreNumber),
                ShippingIDASN = g.Max(m => m.c.ShippingIDASN),
                ShippingInstruction = g.Max(m => m.shipnm),
                DA = g.Max(m => m.c.DA),
                TotalAmount = g.Max(m => m.c.TotalAmount),
                PartsOrderID = g.Max(m => m.c.PartsOrderID),
                ModifiedBy = g.Max(m => m.c.ModifiedBy),
                ModifiedDate = g.Max(m => m.c.ModifiedDate)
            }).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(rowNumber - 1);
                row.CreateCell(1).SetCellValue(data.InvoiceNo);
                row.CreateCell(2).SetCellValue(data.InvoiceDate.ToString("MM/dd/yyyy"));
                row.CreateCell(3).SetCellValue(data.AgreementType);
                row.CreateCell(4).SetCellValue(data.JCode);
                row.CreateCell(5).SetCellValue(data.StoreNumber);
                row.CreateCell(6).SetCellValue(data.DA);
            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             freight + "-freight-PartsOrder-" + route + ".xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//No
            sheet.SetColumnWidth(1, 20 * 256);//Invoice No
            sheet.SetColumnWidth(2, 20 * 256);//Invoice Date
            sheet.SetColumnWidth(3, 20 * 256);//Agreement Type
            sheet.SetColumnWidth(4, 20 * 256);//J-Code
            sheet.SetColumnWidth(5, 20 * 256);//Store Number
            sheet.SetColumnWidth(6, 20 * 256);//DA Number

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("Invoice No");
            headerRow.CreateCell(2).SetCellValue("Invoice Date");
            headerRow.CreateCell(3).SetCellValue("Agreement Type");
            headerRow.CreateCell(4).SetCellValue("J-Code");
            headerRow.CreateCell(5).SetCellValue("Store Number");
            headerRow.CreateCell(6).SetCellValue("DA Number");

            return headerRow;
        }
    }
}