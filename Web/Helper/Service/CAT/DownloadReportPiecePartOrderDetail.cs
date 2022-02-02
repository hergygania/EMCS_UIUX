using App.Web.Models.CAT;
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
    public class DownloadReportPiecePartOrderDetail : Controller
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

        public FileResult DownloadToExcel(RptPiecePartOrderSummaryFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.Report.RptPiecePartOrderDetailList.SP_GetList(filter.ref_part_no, filter.model, filter.prefix, filter.smcs, filter.component, filter.mod, filter.DateFilter);

                tbl = tbl.OrderByDescending(o => o.RefPartNo).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.RefPartNo);
                    row.CreateCell(1).SetCellValue(data.PartNo);
                    row.CreateCell(2).SetCellValue(data.Model);
                    row.CreateCell(3).SetCellValue(data.Prefix);
                    row.CreateCell(4).SetCellValue(data.SMCS);
                    row.CreateCell(5).SetCellValue(data.Component);
                    row.CreateCell(6).SetCellValue(data.MOD);
                    row.CreateCell(7).SetCellValue(data.Status);
                    row.CreateCell(8).SetCellValue(data.Store);
                    row.CreateCell(9).SetCellValue(data.SOS);
                    row.CreateCell(10).SetCellValue(data.KAL);
                    row.CreateCell(11).SetCellValue(data.UsedSN);
                    row.CreateCell(12).SetCellValue(data.EquipmentNo);
                    row.CreateCell(13).SetCellValue(data.Customer_Spuly);
                    SetValueAndFormat(workbook, row.CreateCell(14), data.StoreSuppliedDate.ToString());
                    //row.CreateCell(14).SetCellValue(data.StoreSuppliedDate != null ? data.StoreSuppliedDate.ToString() : "");
                    row.CreateCell(15).SetCellValue(data.ReconditionedWO);
                    row.CreateCell(16).SetCellValue(data.SaleDoc);
                    row.CreateCell(17).SetCellValue(data.ReturnDoc);
                    row.CreateCell(18).SetCellValue(data.WCSL);
                    row.CreateCell(19).SetCellValue(data.PONumber);
                    SetValueAndFormat(workbook, row.CreateCell(20), data.Schedule.ToString());
                    //row.CreateCell(20).SetCellValue(data.Schedule != null ? data.Schedule.ToString() : "");
                    row.CreateCell(21).SetCellValue(data.UnitNoSN);
                    row.CreateCell(22).SetCellValue(data.SerialNo);
                    row.CreateCell(23).SetCellValue(data.Location);
                    row.CreateCell(24).SetCellValue(data.Customer);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportPiecePartOrderDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportPiecePartOrderDetail.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Ref Parts Number
            sheet.SetColumnWidth(1, 20 * 256);//Part Number
            sheet.SetColumnWidth(2, 20 * 256);//Model
            sheet.SetColumnWidth(3, 20 * 256);//Perfic
            sheet.SetColumnWidth(4, 20 * 256);//SCMS
            sheet.SetColumnWidth(5, 20 * 256);//Component  
            sheet.SetColumnWidth(6, 20 * 256);//MOD
            sheet.SetColumnWidth(7, 20 * 256);//Status
            sheet.SetColumnWidth(8, 20 * 256);//Store
            sheet.SetColumnWidth(9, 20 * 256);//SOS
            sheet.SetColumnWidth(10, 20 * 256);//KAL
            sheet.SetColumnWidth(11, 20 * 256);//Used SN
            sheet.SetColumnWidth(12, 20 * 256);//Equipment No
            sheet.SetColumnWidth(13, 20 * 256);//Customer
            sheet.SetColumnWidth(14, 20 * 256);//Store Suplied Date
            sheet.SetColumnWidth(15, 20 * 256);//Recondition WO
            sheet.SetColumnWidth(16, 20 * 256);//Sales Doc
            sheet.SetColumnWidth(17, 20 * 256);//Return Doc
            sheet.SetColumnWidth(18, 20 * 256);//WCSL
            sheet.SetColumnWidth(19, 20 * 256);//PO Number
            sheet.SetColumnWidth(20, 20 * 256);//Schedule
            sheet.SetColumnWidth(21, 20 * 256);//Unit No
            sheet.SetColumnWidth(22, 20 * 256);//Serial Number
            sheet.SetColumnWidth(23, 20 * 256);//Location
            sheet.SetColumnWidth(24, 20 * 256);//Customer

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Ref. Parts Number");
            headerRow.CreateCell(1).SetCellValue("Part Number");
            headerRow.CreateCell(2).SetCellValue("Model");
            headerRow.CreateCell(3).SetCellValue("Prefix");
            headerRow.CreateCell(4).SetCellValue("SMCS");
            headerRow.CreateCell(5).SetCellValue("Component");
            headerRow.CreateCell(6).SetCellValue("MOD");
            headerRow.CreateCell(7).SetCellValue("Status");
            headerRow.CreateCell(8).SetCellValue("Store");
            headerRow.CreateCell(9).SetCellValue("SOS");
            headerRow.CreateCell(10).SetCellValue("KAL");
            headerRow.CreateCell(11).SetCellValue("Used SN");
            headerRow.CreateCell(12).SetCellValue("Equipment No");
            headerRow.CreateCell(14).SetCellValue("Customer");
            headerRow.CreateCell(14).SetCellValue("Store Suplied Date");
            headerRow.CreateCell(15).SetCellValue("Recondition WO");
            headerRow.CreateCell(16).SetCellValue("Sales Doc");
            headerRow.CreateCell(17).SetCellValue("Return Doc");
            headerRow.CreateCell(18).SetCellValue("WCSL");
            headerRow.CreateCell(19).SetCellValue("PO Number");
            headerRow.CreateCell(20).SetCellValue("Schedule");
            headerRow.CreateCell(21).SetCellValue("Unit No");
            headerRow.CreateCell(22).SetCellValue("Serial Number");
            headerRow.CreateCell(23).SetCellValue("Location");
            headerRow.CreateCell(24).SetCellValue("Customer");

            return headerRow;
        }
    }
}