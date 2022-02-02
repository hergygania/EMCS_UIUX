using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service.DTS
{
    public class DownloadOutboundController : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        /// <summary>
        /// download data Inbound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcel(App.Data.Domain.DTS.OutboundFilter filter)
        {
            try
            {
                var tbl = App.Service.DTS.ShipmentOutbound.GetListFilter(filter);

                tbl = tbl.OrderBy(a => a.DI).ToList();

                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                //sheet.CreateFreezePane(0, 1, 0, 1);

                //Populate the sheet with values from the grid data
                CreateSheetData(tbl);


                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Outbound.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                if (ex.InnerException != null)
                    Debug.WriteLine(ex.InnerException.Message);
                else
                    Debug.WriteLine(ex.Message);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Outbound.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetData(List<App.Data.Domain.ShipmentOutboundListData> tbl)
        {

            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.DA); row.CreateCell(1).SetCellValue(data.DI); row.CreateCell(2).SetCellValue(data.Origin);
                row.CreateCell(3).SetCellValue(data.Destination); row.CreateCell(4).SetCellValue(data.Moda); row.CreateCell(5).SetCellValue(data.UnitModa);
                row.CreateCell(6).SetCellValue(data.UnitType); row.CreateCell(7).SetCellValue(data.Model); row.CreateCell(8).SetCellValue(data.ETD.ToString());
                row.CreateCell(9).SetCellValue(data.ATD.ToString()); row.CreateCell(10).SetCellValue(data.ETA.ToString()); row.CreateCell(11).SetCellValue(data.ATA.ToString());
                row.CreateCell(12).SetCellValue(data.Position); row.CreateCell(13).SetCellValue(data.Status); row.CreateCell(14).SetCellValue(data.SerialNumber);
                row.CreateCell(15).SetCellValue(data.Remarks);

            }
        }

        static void SetValueAndFormat(IWorkbook workbook, ICell cell, string value)
        {
            IDataFormat format = workbook.CreateDataFormat();
            short formatId = format.GetFormat("dd MMM yyyy");
            //set value for the cell
            if (!string.IsNullOrEmpty(value))
                cell.SetCellValue(Convert.ToDateTime(value));

            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }


        /// <summary>
        /// Initialize  Sheet Excel
        /// </summary>
        /// <returns></returns>
        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//DA
            sheet.SetColumnWidth(1, 20 * 200);//DI
            sheet.SetColumnWidth(2, 20 * 200);//Origin
            sheet.SetColumnWidth(3, 20 * 256);//Destination
            sheet.SetColumnWidth(4, 20 * 256);//Moda
            sheet.SetColumnWidth(5, 20 * 200);//Unit Moda
            sheet.SetColumnWidth(6, 20 * 200);//Unit Type
            sheet.SetColumnWidth(7, 20 * 150);//Model
            sheet.SetColumnWidth(8, 20 * 150);//ETD
            sheet.SetColumnWidth(9, 20 * 150);//ATD
            sheet.SetColumnWidth(10, 20 * 200);//ETA
            sheet.SetColumnWidth(11, 20 * 200);//ATA
            sheet.SetColumnWidth(12, 20 * 200);//Position
            sheet.SetColumnWidth(13, 20 * 200);//Status
            sheet.SetColumnWidth(14, 20 * 200);//Serial Number
            sheet.SetColumnWidth(15, 20 * 200);//Remarks



            return sheet;
        }

        /// <summary>
        /// Initialize row Header
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("DA");
            headerRow.CreateCell(1).SetCellValue("DI");
            headerRow.CreateCell(2).SetCellValue("Origin");
            headerRow.CreateCell(3).SetCellValue("Destination");
            headerRow.CreateCell(4).SetCellValue("Moda");
            headerRow.CreateCell(5).SetCellValue("Unit Moda");
            headerRow.CreateCell(6).SetCellValue("Unit Type");
            headerRow.CreateCell(7).SetCellValue("Model");
            headerRow.CreateCell(8).SetCellValue("ETD");
            headerRow.CreateCell(9).SetCellValue("ATD");
            headerRow.CreateCell(10).SetCellValue("ETA");
            headerRow.CreateCell(11).SetCellValue("ATA");
            headerRow.CreateCell(12).SetCellValue("Position");
            headerRow.CreateCell(13).SetCellValue("Status");
            headerRow.CreateCell(14).SetCellValue("Serial Number");
            headerRow.CreateCell(15).SetCellValue("Remarks");

            return headerRow;
        }



    }
}