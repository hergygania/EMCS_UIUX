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
    public class DownloadInboundController : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        /// <summary>
        /// download data Inbound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcel(App.Data.Domain.DTS.InboundFilter filter)
        {
            try
            {
                var tbl = App.Service.DTS.ShipmentInbound.GetListFilterforExcel(filter);

                //tbl = tbl.OrderBy(o => o.UpdateDate).ToList();

                //Create new Excel Sheet
                sheet = CreateSheet();

                //Create a header row
                CreateHeaderRow(sheet);

                //(Optional) freeze the header row so it is not scrolled
                sheet.CreateFreezePane(0, 1, 0, 1);

                //Populate the sheet with values from the grid data
                CreateSheetData(tbl);


                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Inbound.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "Inbound.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetData(List<App.Data.Domain.ShipmentInboundListDownload> tbl)
        {

            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.PONo);
                row.CreateCell(1).SetCellValue(data.PODate.ToString());
                row.CreateCell(2).SetCellValue(data.AjuNo);
                row.CreateCell(3).SetCellValue(data.MSONo);
                row.CreateCell(4).SetCellValue(data.LoadingPort);
                row.CreateCell(5).SetCellValue(data.DischargePort);
                row.CreateCell(6).SetCellValue(data.Model);
                row.CreateCell(7).SetCellValue(data.ModelDescription);
                row.CreateCell(8).SetCellValue(data.Status);
                //row.CreateCell(8).SetCellValue(data.ETACakung.ToString());
                //row.CreateCell(9).SetCellValue(data.ATACakung.ToString());
                //row.CreateCell(10).SetCellValue(data.ETAPort.ToString());
                //row.CreateCell(11).SetCellValue(data.ATAPort.ToString());
                row.CreateCell(9).SetCellValue(data.SerialNumber);
                row.CreateCell(10).SetCellValue(data.BatchNumber);
                row.CreateCell(11).SetCellValue(data.Remark);
                row.CreateCell(12).SetCellValue(data.Notes);
                row.CreateCell(13).SetCellValue(data.Position);
                //detail
                row.CreateCell(14).SetCellValue(data.RTSPlan.ToString());
                row.CreateCell(15).SetCellValue(data.RTSActual.ToString());
                row.CreateCell(16).SetCellValue(data.OnBoardVesselPlan.ToString());
                row.CreateCell(17).SetCellValue(data.OnBoardVesselActual.ToString());
                row.CreateCell(18).SetCellValue(data.PortInPlan.ToString());
                row.CreateCell(19).SetCellValue(data.PortInActual.ToString());
                row.CreateCell(20).SetCellValue(data.PortOutPlan.ToString());
                row.CreateCell(21).SetCellValue(data.PortOutActual.ToString());
                row.CreateCell(22).SetCellValue(data.PLBInPlan.ToString());
                row.CreateCell(23).SetCellValue(data.PLBInActual.ToString());
                row.CreateCell(24).SetCellValue(data.PLBOutPlan.ToString());
                row.CreateCell(25).SetCellValue(data.PLBOutActual.ToString());
                row.CreateCell(26).SetCellValue(data.YardInPlan.ToString());
                row.CreateCell(27).SetCellValue(data.YardInActual.ToString());
                row.CreateCell(28).SetCellValue(data.YardOutPlan.ToString());
                row.CreateCell(29).SetCellValue(data.YardOutActual.ToString());
                row.CreateCell(30).SetCellValue(data.Plant);

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
            sheet.SetColumnWidth(0, 20 * 200);//Ref Part Number
            sheet.SetColumnWidth(1, 20 * 200);//Alt Part Number
            sheet.SetColumnWidth(2, 20 * 200);//Model
            sheet.SetColumnWidth(3, 20 * 256);//Prefix
            sheet.SetColumnWidth(4, 20 * 256);//SMCS
            sheet.SetColumnWidth(5, 20 * 200);//Component
            sheet.SetColumnWidth(6, 20 * 200);//MOD
            sheet.SetColumnWidth(7, 20 * 150);//Status   
            sheet.SetColumnWidth(8, 20 * 150);//Store
            sheet.SetColumnWidth(9, 20 * 150);//SOS
            sheet.SetColumnWidth(10, 20 * 200);//KAL
            sheet.SetColumnWidth(11, 20 * 200);//Core Model
            sheet.SetColumnWidth(12, 20 * 200);//Family
            sheet.SetColumnWidth(13, 20 * 200);//CRC TAT
            sheet.SetColumnWidth(14, 20 * 200);//Section
            sheet.SetColumnWidth(15, 20 * 200);//Surplus 
            sheet.SetColumnWidth(16, 20 * 200);//Note 

            //Cycle 1 
            sheet.SetColumnWidth(17, 20 * 300);//PO Number
            sheet.SetColumnWidth(18, 20 * 300);//Schedule
            sheet.SetColumnWidth(29, 20 * 300);//Unit No
            sheet.SetColumnWidth(20, 20 * 300);//Serial Number
            sheet.SetColumnWidth(21, 20 * 300);//Location
            sheet.SetColumnWidth(22, 20 * 300);//Customer

            //Cycle 2 
            sheet.SetColumnWidth(23, 20 * 300);//PO Number
            sheet.SetColumnWidth(24, 20 * 300);//Schedule
            sheet.SetColumnWidth(25, 20 * 300);//Unit No
            sheet.SetColumnWidth(26, 20 * 300);//Serial Number
            sheet.SetColumnWidth(27, 20 * 300);//Location
            sheet.SetColumnWidth(28, 20 * 300);//Customer

            //Cycle 1 
            sheet.SetColumnWidth(29, 20 * 300);//PO Number
            sheet.SetColumnWidth(30, 20 * 300);//Schedule
            sheet.SetColumnWidth(31, 20 * 300);//Unit No
            sheet.SetColumnWidth(32, 20 * 300);//Serial Number
            sheet.SetColumnWidth(33, 20 * 300);//Location
            sheet.SetColumnWidth(34, 20 * 300);//Customer

            //Cycle 1 
            sheet.SetColumnWidth(35, 20 * 300);//PO Number
            sheet.SetColumnWidth(36, 20 * 300);//Schedule
            sheet.SetColumnWidth(37, 20 * 300);//Unit No
            sheet.SetColumnWidth(38, 20 * 300);//Serial Number
            sheet.SetColumnWidth(39, 20 * 300);//Batch Number
            sheet.SetColumnWidth(40, 20 * 300);//Location
            sheet.SetColumnWidth(41, 20 * 300);//Customer

            //Cycle 1 
            sheet.SetColumnWidth(42, 20 * 300);//PO Number
            sheet.SetColumnWidth(43, 20 * 300);//Schedule
            sheet.SetColumnWidth(44, 20 * 300);//Unit No
            sheet.SetColumnWidth(45, 20 * 300);//Serial Number
            sheet.SetColumnWidth(46, 20 * 300);//Location
            sheet.SetColumnWidth(47, 20 * 300);//Customer

            sheet.SetColumnWidth(48, 20 * 200);//Used SN
            sheet.SetColumnWidth(49, 20 * 200);//Equipment No
            sheet.SetColumnWidth(50, 20 * 200);//Customer
            sheet.SetColumnWidth(51, 20 * 200);//Supplied Date
            sheet.SetColumnWidth(52, 20 * 200);//Elapsed Days
            sheet.SetColumnWidth(53, 20 * 200);//Stock Transfer Date
            sheet.SetColumnWidth(54, 20 * 200);//Ref Doc Transfer
            sheet.SetColumnWidth(55, 20 * 200);//New WO 6F
            sheet.SetColumnWidth(56, 20 * 200);//WO1K
            sheet.SetColumnWidth(57, 20 * 200);//Old WO
            sheet.SetColumnWidth(58, 20 * 200);//Manual Order
            sheet.SetColumnWidth(59, 20 * 200);//DOC C
            sheet.SetColumnWidth(60, 20 * 200);//DOC R
            sheet.SetColumnWidth(61, 20 * 200);//WCSL
            sheet.SetColumnWidth(62, 20 * 200);//Rebuild Status
            sheet.SetColumnWidth(63, 20 * 356);//CRC Promised Completion Date
            sheet.SetColumnWidth(64, 20 * 200);//CRC Completion
            sheet.SetColumnWidth(65, 20 * 200);//Job Loc
            sheet.SetColumnWidth(66, 20 * 200);//PEX
            sheet.SetColumnWidth(67, 20 * 200);//Return As Zero
            sheet.SetColumnWidth(68, 20 * 200);//TUID
            sheet.SetColumnWidth(69, 20 * 200);//Date Receive
            sheet.SetColumnWidth(70, 20 * 200);//DA Number
            sheet.SetColumnWidth(71, 20 * 300);//Remarks
            sheet.SetColumnWidth(72, 20 * 300);//WO CMS
            sheet.SetColumnWidth(73, 20 * 300);//WO CMS

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
            headerRow.CreateCell(0).SetCellValue("Po Number");
            headerRow.CreateCell(1).SetCellValue("Po Date");
            headerRow.CreateCell(2).SetCellValue("Aju Number");
            headerRow.CreateCell(3).SetCellValue("MSO Number");
            headerRow.CreateCell(4).SetCellValue("Loading Port");
            headerRow.CreateCell(5).SetCellValue("Discharge Port");
            headerRow.CreateCell(6).SetCellValue("Model");
            headerRow.CreateCell(7).SetCellValue("Model Description");
            headerRow.CreateCell(8).SetCellValue("Status");
            headerRow.CreateCell(9).SetCellValue("Serial Number");
            headerRow.CreateCell(10).SetCellValue("Batch Number");
            headerRow.CreateCell(11).SetCellValue("Remark");
            headerRow.CreateCell(12).SetCellValue("Notes");
            headerRow.CreateCell(13).SetCellValue("Position");
            headerRow.CreateCell(14).SetCellValue("RTS Plan");
            headerRow.CreateCell(15).SetCellValue("RTS Actual");
            headerRow.CreateCell(16).SetCellValue("On Board Vessel Plan");
            headerRow.CreateCell(17).SetCellValue("On Board Vessel Actual");
            headerRow.CreateCell(18).SetCellValue("Port In Plan");
            headerRow.CreateCell(19).SetCellValue("Port In Actual");
            headerRow.CreateCell(20).SetCellValue("Port Out Plan");
            headerRow.CreateCell(21).SetCellValue("Port Out Actual");
            headerRow.CreateCell(22).SetCellValue("PLB In Plan");
            headerRow.CreateCell(23).SetCellValue("PLB In Actual");
            headerRow.CreateCell(24).SetCellValue("PLB Out Plan");
            headerRow.CreateCell(25).SetCellValue("PLB Out Actual");
            headerRow.CreateCell(26).SetCellValue("Yard In Plan");
            headerRow.CreateCell(27).SetCellValue("Yard In Actual");
            headerRow.CreateCell(28).SetCellValue("Yard Out Plan");
            headerRow.CreateCell(29).SetCellValue("Yard Out Actual");
            headerRow.CreateCell(30).SetCellValue("Plant");
            //headerRow.CreateCell(30).SetCellValue("ETA Cakung");
            //headerRow.CreateCell(31).SetCellValue("ATA Cakung");
            //headerRow.CreateCell(32).SetCellValue("ETA Port");

            return headerRow;
        }
    }
}