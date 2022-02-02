using App.Web.Models.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadReportSummaryPEX : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(RptSummaryPEXFilter filter)
        {
            try
            {
                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                var tbl = App.Service.CAT.Report.RptSummaryPEXList.GetList(filter.ref_part_no, filter.model, filter.component, filter.SOS);

                tbl = tbl.OrderByDescending(o => o.ref_part_no).ToList();

                int rowNumber = 1;

                //Populate the sheet with values from the grid data
                foreach (var data in tbl)
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(data.ref_part_no);
                    row.CreateCell(1).SetCellValue(data.model);
                    row.CreateCell(2).SetCellValue(data.component);
                    row.CreateCell(3).SetCellValue(data.prefix);
                    row.CreateCell(4).SetCellValue(data.scms);
                    row.CreateCell(5).SetCellValue(data.mod);
                    row.CreateCell(6).SetCellValue(data.total_availability);
                    row.CreateCell(7).SetCellValue(data.allocated_oh);
                    row.CreateCell(8).SetCellValue(data.allocated_st);
                    row.CreateCell(9).SetCellValue(data.allocated_woc);
                    row.CreateCell(10).SetCellValue(data.allocated_ttc);
                    row.CreateCell(11).SetCellValue(data.allocated_sq);
                    row.CreateCell(12).SetCellValue(data.allocated_wip);
                    row.CreateCell(13).SetCellValue(data.allocated_jc);
                    row.CreateCell(14).SetCellValue(data.free_allocation_oh);
                    row.CreateCell(15).SetCellValue(data.free_allocation_st);
                    row.CreateCell(16).SetCellValue(data.free_allocation_woc);
                    row.CreateCell(17).SetCellValue(data.free_allocation_ttc);
                    row.CreateCell(18).SetCellValue(data.free_allocation_sq);
                    row.CreateCell(19).SetCellValue(data.free_allocation_wip);
                    row.CreateCell(20).SetCellValue(data.free_allocation_jc);
                    row.CreateCell(21).SetCellValue(data.total_allocation);
                    row.CreateCell(22).SetCellValue(data.allocation_cycle1);
                    row.CreateCell(23).SetCellValue(data.allocation_cycle2);
                    row.CreateCell(24).SetCellValue(data.allocation_cycle3);
                    row.CreateCell(25).SetCellValue(data.allocation_cycle4);
                    row.CreateCell(26).SetCellValue(data.allocation_cycle5);

                }

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportSummaryPEX.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportSummaryPEX.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//Ref Part No.
            sheet.SetColumnWidth(1, 20 * 256);//Model
            sheet.SetColumnWidth(2, 20 * 256);//Component
            sheet.SetColumnWidth(3, 20 * 256);//Prefix
            sheet.SetColumnWidth(4, 20 * 256);//SCMS
            sheet.SetColumnWidth(5, 20 * 256);//MOD
            sheet.SetColumnWidth(6, 20 * 256);//Total Availability
            sheet.SetColumnWidth(7, 20 * 256);//Allocated OH
            sheet.SetColumnWidth(8, 20 * 256);//Allocated ST
            sheet.SetColumnWidth(9, 20 * 256);//Allocated WOC
            sheet.SetColumnWidth(10, 20 * 256);//Allocated TTC
            sheet.SetColumnWidth(11, 20 * 256);//Allocated SQ
            sheet.SetColumnWidth(12, 20 * 256);//Allocated WIP
            sheet.SetColumnWidth(13, 20 * 256);//Allocated JC
            sheet.SetColumnWidth(14, 20 * 256);//Free Allocated OH
            sheet.SetColumnWidth(15, 20 * 256);//Free Allocated ST
            sheet.SetColumnWidth(16, 20 * 256);//Free Allocated WOC
            sheet.SetColumnWidth(17, 20 * 256);//Free Allocated TTC
            sheet.SetColumnWidth(18, 20 * 256);//Free Allocated SQ
            sheet.SetColumnWidth(19, 20 * 256);//Free Allocated WIP
            sheet.SetColumnWidth(20, 20 * 256);//Free Allocated JC
            sheet.SetColumnWidth(21, 20 * 256);//Total Allocation
            sheet.SetColumnWidth(22, 20 * 256);//Cycle 1
            sheet.SetColumnWidth(23, 20 * 256);//Cycle 2
            sheet.SetColumnWidth(24, 20 * 256);//Cycle 3
            sheet.SetColumnWidth(25, 20 * 256);//Cycle 4
            sheet.SetColumnWidth(26, 20 * 256);//Cycle 5

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Ref. Parts No.");
            headerRow.CreateCell(1).SetCellValue("Model");
            headerRow.CreateCell(2).SetCellValue("Component");
            headerRow.CreateCell(3).SetCellValue("Prefix");
            headerRow.CreateCell(4).SetCellValue("SCMS");
            headerRow.CreateCell(5).SetCellValue("MOD");
            headerRow.CreateCell(6).SetCellValue("Total Availability");
            headerRow.CreateCell(7).SetCellValue("Allocated OH");
            headerRow.CreateCell(8).SetCellValue("Allocated ST");
            headerRow.CreateCell(9).SetCellValue("Allocated WOC");
            headerRow.CreateCell(10).SetCellValue("Allocated TTC");
            headerRow.CreateCell(11).SetCellValue("Allocated SQ");
            headerRow.CreateCell(12).SetCellValue("Allocated WIP");
            headerRow.CreateCell(13).SetCellValue("Allocated JC");
            headerRow.CreateCell(14).SetCellValue("Free Allocated OH");
            headerRow.CreateCell(15).SetCellValue("Free Allocated ST");
            headerRow.CreateCell(16).SetCellValue("Free Allocated WOC");
            headerRow.CreateCell(17).SetCellValue("Free Allocated TTC");
            headerRow.CreateCell(18).SetCellValue("Free Allocated SQ");
            headerRow.CreateCell(19).SetCellValue("Free Allocated WIP");
            headerRow.CreateCell(20).SetCellValue("Free Allocated JC");
            headerRow.CreateCell(21).SetCellValue("Total Allocation");
            headerRow.CreateCell(22).SetCellValue("Cycle 1");
            headerRow.CreateCell(23).SetCellValue("Cycle 2");
            headerRow.CreateCell(24).SetCellValue("Cycle 3");
            headerRow.CreateCell(25).SetCellValue("Cycle 4");
            headerRow.CreateCell(26).SetCellValue("Cycle 5");

            return headerRow;
        }
    }
}