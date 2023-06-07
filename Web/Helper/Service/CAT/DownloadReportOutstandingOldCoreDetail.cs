using App.Data.Domain.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadReportOutstandingOldCoreDetail : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

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

        public FileResult DownloadToExcel(RptOutstandingOldCoreDetailFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.Report.RptOutstandingOldCoreDetailList.SP_GetList(filter);

                tbl = tbl.OrderByDescending(o => o.Store).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.Store);
                    row.CreateCell(1).SetCellValue(data.SOS);
                    row.CreateCell(2).SetCellValue(data.PartNo);
                    row.CreateCell(3).SetCellValue(data.KAL);
                    row.CreateCell(4).SetCellValue(data.Model);
                    row.CreateCell(5).SetCellValue(data.Prefix);
                    row.CreateCell(6).SetCellValue(data.Component);
                    row.CreateCell(7).SetCellValue(data.UsedSN);
                    row.CreateCell(8).SetCellValue(data.EquipmentNo);
                    row.CreateCell(9).SetCellValue(data.Customer_Spuly);
                    SetValueAndFormat(workbook, row.CreateCell(10), data.StoreSuppliedDate.ToString());
                    //row.CreateCell(10).SetCellValue(data.StoreSuppliedDate != null ? data.StoreSuppliedDate.ToString() : "");
                    row.CreateCell(11).SetCellValue(data.ReconditionedWO);
                    row.CreateCell(12).SetCellValue(data.SaleDoc);
                    row.CreateCell(13).SetCellValue(data.ReturnDoc);
                    row.CreateCell(14).SetCellValue(data.WCSL);
                    row.CreateCell(15).SetCellValue(data.PONumber);
                    SetValueAndFormat(workbook, row.CreateCell(16), data.Schedule.ToString());
                    //row.CreateCell(16).SetCellValue(data.Schedule != null ? data.Schedule.ToString() : "");
                    row.CreateCell(17).SetCellValue(data.UnitNoSN);
                    row.CreateCell(18).SetCellValue(data.SerialNo);
                    row.CreateCell(19).SetCellValue(data.Location);
                    row.CreateCell(20).SetCellValue(data.Customer);
                    row.CreateCell(21).SetCellValue(data.Section);
                    row.CreateCell(22).SetCellValue(data.RebuildOption);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportOutstandingOldCoreDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportOutstandingOldCoreDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Store
            sheet.SetColumnWidth(1, 20 * 256);//SOS
            sheet.SetColumnWidth(2, 20 * 256);//Part Number
            sheet.SetColumnWidth(3, 20 * 256);//KAL
            sheet.SetColumnWidth(4, 20 * 256);//Model
            sheet.SetColumnWidth(5, 20 * 256);//Prefix 
            sheet.SetColumnWidth(6, 20 * 256);//Component
            sheet.SetColumnWidth(7, 20 * 256);//Used SN
            sheet.SetColumnWidth(8, 20 * 256);//Equipment No
            sheet.SetColumnWidth(9, 20 * 256);//Customer
            sheet.SetColumnWidth(10, 20 * 256);//Store Suplied Date
            sheet.SetColumnWidth(11, 20 * 256);//Recondition WO
            sheet.SetColumnWidth(12, 20 * 256);//Sales Doc
            sheet.SetColumnWidth(13, 20 * 256);//Return Doc
            sheet.SetColumnWidth(14, 20 * 256);//WCSL
            sheet.SetColumnWidth(15, 20 * 256);//PO Number
            sheet.SetColumnWidth(16, 20 * 256);//Schedule
            sheet.SetColumnWidth(17, 20 * 256);//Unit No
            sheet.SetColumnWidth(18, 20 * 256);//Serial Number
            sheet.SetColumnWidth(19, 20 * 256);//Location
            sheet.SetColumnWidth(20, 20 * 256);//Customer
            sheet.SetColumnWidth(21, 20 * 256);//Section
            sheet.SetColumnWidth(22, 20 * 256);//Rebuild Option

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Store");
            headerRow.CreateCell(1).SetCellValue("SOS");
            headerRow.CreateCell(2).SetCellValue("Part Number");
            headerRow.CreateCell(3).SetCellValue("KAL");
            headerRow.CreateCell(4).SetCellValue("Model");
            headerRow.CreateCell(5).SetCellValue("Prefix");
            headerRow.CreateCell(6).SetCellValue("Component");
            headerRow.CreateCell(7).SetCellValue("Used SN");
            headerRow.CreateCell(8).SetCellValue("Equipment No");
            headerRow.CreateCell(9).SetCellValue("Customer");
            headerRow.CreateCell(10).SetCellValue("Store Suplied Date");
            headerRow.CreateCell(11).SetCellValue("Recondition WO");
            headerRow.CreateCell(12).SetCellValue("Sales Doc");
            headerRow.CreateCell(13).SetCellValue("Return Doc");
            headerRow.CreateCell(14).SetCellValue("WCSL");
            headerRow.CreateCell(15).SetCellValue("PO Number");
            headerRow.CreateCell(16).SetCellValue("Schedule");
            headerRow.CreateCell(17).SetCellValue("Unit No");
            headerRow.CreateCell(18).SetCellValue("Serial Number");
            headerRow.CreateCell(19).SetCellValue("Location");
            headerRow.CreateCell(20).SetCellValue("Customer");
            headerRow.CreateCell(21).SetCellValue("Section");
            headerRow.CreateCell(22).SetCellValue("Rebuild Option");

            return headerRow;
        }
    }
}