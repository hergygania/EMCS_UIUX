using App.Web.Models.CAT;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Helper.Service.CAT
{
    public class DownloadInventory : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;
        private string PONo_1, OriSchedule_1, UnitNo_1, SerialNo_1, Store_1, Customer_1;
        private string PONo_2, OriSchedule_2, UnitNo_2, SerialNo_2, Store_2, Customer_2;
        private string PONo_3, OriSchedule_3, UnitNo_3, SerialNo_3, Store_3, Customer_3;
        private string PONo_4, OriSchedule_4, UnitNo_4, SerialNo_4, Store_4, Customer_4;
        private string PONo_5, OriSchedule_5, UnitNo_5, SerialNo_5, Store_5, Customer_5;

        /// <summary>
        /// download data Inventory
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcel(App.Data.Domain.CAT.InventoryFilter filter)
        {
            try
            {
                var tbl = App.Service.CAT.InventoryList
                    .SP_GetListForDownload(filter);

                tbl = tbl.OrderBy(o => o.AlternetPartNumber).ToList();

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
                 "Inventory.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "Inventory.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// download data Inventory
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcelForEdit()
        {
            try
            {
                var tbl = App.Service.CAT.InventoryList
                    .SP_GetListEditForDownload();

                tbl = tbl.OrderBy(o => o.AlternetPartNumber).ToList();

                //Create new Excel Sheet
                sheet = workbook.CreateSheet("Sheet1");//CreateSheet();

                //Create a header row
                CreateHeaderRowEdit(sheet);

                //(Optional) freeze the header row so it is not scrolled
                //sheet.CreateFreezePane(0, 1, 0, 1);

                //Populate the sheet with values from the grid data
                CreateSheetDataEdit(tbl);

                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "InventoryForEdit.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "InventoryForEdit.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetData(List<App.Data.Domain.SP_DataInventoryForDownload> tbl)
        {

            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.RefPartNo); row.CreateCell(1).SetCellValue(data.AlternetPartNumber); row.CreateCell(2).SetCellValue(data.ApplicableModel);
                row.CreateCell(3).SetCellValue(data.Prefix); row.CreateCell(4).SetCellValue(data.SMCSCode); row.CreateCell(5).SetCellValue(data.Component);
                row.CreateCell(6).SetCellValue(data.MOD); row.CreateCell(7).SetCellValue(data.LastStatus); row.CreateCell(8).SetCellValue(data.StoreNumber);
                row.CreateCell(9).SetCellValue(data.SOS); row.CreateCell(10).SetCellValue(data.KAL); row.CreateCell(11).SetCellValue(data.CoreModel);
                row.CreateCell(12).SetCellValue(data.Family); row.CreateCell(13).SetCellValue(data.CRCTAT); row.CreateCell(14).SetCellValue(data.Section);
                row.CreateCell(15).SetCellValue(data.Surplus); row.CreateCell(16).SetCellValue(data.Note);

                //Cycle 1
                //SetCycle1(data.KAL);
                //row.CreateCell(17).SetCellValue(PONo_1); row.CreateCell(18).SetCellValue(OriSchedule_1); row.CreateCell(19).SetCellValue(UnitNo_1);
                //row.CreateCell(20).SetCellValue(SerialNo_1); row.CreateCell(21).SetCellValue(Store_1); row.CreateCell(22).SetCellValue(Customer_1);

                row.CreateCell(17).SetCellValue(data.PONo1); SetValueAndFormat(workbook, row.CreateCell(18), data.OriSchedule_1); row.CreateCell(19).SetCellValue(data.UnitNo_1);
                row.CreateCell(20).SetCellValue(data.SerialNo_1); row.CreateCell(21).SetCellValue(data.Store_1); row.CreateCell(22).SetCellValue(data.Customer_1);

                //Cycle 2
                //SetCycle2(data.KAL);
                //row.CreateCell(23).SetCellValue(PONo_2); row.CreateCell(24).SetCellValue(OriSchedule_2); row.CreateCell(25).SetCellValue(UnitNo_2);
                //row.CreateCell(26).SetCellValue(SerialNo_2); row.CreateCell(27).SetCellValue(Store_2); row.CreateCell(28).SetCellValue(Customer_2);

                row.CreateCell(23).SetCellValue(data.PONo2); SetValueAndFormat(workbook, row.CreateCell(24), data.OriSchedule_2); row.CreateCell(25).SetCellValue(data.UnitNo_2);
                row.CreateCell(26).SetCellValue(data.SerialNo_2); row.CreateCell(27).SetCellValue(data.Store_2); row.CreateCell(28).SetCellValue(data.Customer_2);

                //Cycle 3
                //SetCycle3(data.KAL);
                //row.CreateCell(29).SetCellValue(PONo_3); row.CreateCell(30).SetCellValue(OriSchedule_3); row.CreateCell(31).SetCellValue(UnitNo_3);
                //row.CreateCell(32).SetCellValue(SerialNo_3); row.CreateCell(33).SetCellValue(Store_3); row.CreateCell(34).SetCellValue(Customer_3);

                row.CreateCell(29).SetCellValue(data.PONo3); SetValueAndFormat(workbook, row.CreateCell(30), data.OriSchedule_3); row.CreateCell(31).SetCellValue(data.UnitNo_3);
                row.CreateCell(32).SetCellValue(data.SerialNo_3); row.CreateCell(33).SetCellValue(data.Store_3); row.CreateCell(34).SetCellValue(data.Customer_3);

                //Cycle 4
                //SetCycle4(data.KAL);
                //row.CreateCell(35).SetCellValue(PONo_4); row.CreateCell(36).SetCellValue(OriSchedule_4); row.CreateCell(37).SetCellValue(UnitNo_4);
                //row.CreateCell(38).SetCellValue(SerialNo_4); row.CreateCell(39).SetCellValue(Store_4); row.CreateCell(40).SetCellValue(Customer_4);

                row.CreateCell(35).SetCellValue(data.PONo4); SetValueAndFormat(workbook, row.CreateCell(36), data.OriSchedule_4); row.CreateCell(37).SetCellValue(data.UnitNo_4);
                row.CreateCell(38).SetCellValue(data.SerialNo_4); row.CreateCell(39).SetCellValue(data.Store_4); row.CreateCell(40).SetCellValue(data.Customer_4);

                //Cycle 5
                //SetCycle5(data.KAL);
                //row.CreateCell(41).SetCellValue(PONo_5); row.CreateCell(42).SetCellValue(OriSchedule_5); row.CreateCell(43).SetCellValue(UnitNo_5);
                //row.CreateCell(44).SetCellValue(SerialNo_5); row.CreateCell(45).SetCellValue(Store_5); row.CreateCell(46).SetCellValue(Customer_5);

                row.CreateCell(41).SetCellValue(data.PONo5); SetValueAndFormat(workbook, row.CreateCell(42), data.OriSchedule_5); row.CreateCell(43).SetCellValue(data.UnitNo_5);
                row.CreateCell(44).SetCellValue(data.SerialNo_5); row.CreateCell(45).SetCellValue(data.Store_5); row.CreateCell(46).SetCellValue(data.Customer_5);

                row.CreateCell(47).SetCellValue(data.UsedSN); row.CreateCell(48).SetCellValue(data.EquipmentNumber); row.CreateCell(49).SetCellValue(data.Customer);

                SetValueAndFormat(workbook, row.CreateCell(50), data.SuppliedDate);

                row.CreateCell(51).SetCellValue(data.ElapsedDays); SetValueAndFormat(workbook, row.CreateCell(52), data.StockTransferDate);
                row.CreateCell(53).SetCellValue(data.RefDocTransfer); row.CreateCell(54).SetCellValue(data.NewWO6F); row.CreateCell(55).SetCellValue(data.WO1K);
                row.CreateCell(56).SetCellValue(data.OldWO6F); row.CreateCell(57).SetCellValue(data.MO); row.CreateCell(58).SetCellValue(data.DocSales);
                row.CreateCell(59).SetCellValue(data.DocReturn); row.CreateCell(60).SetCellValue(data.DocWCSL); row.CreateCell(61).SetCellValue(data.RebuildStatus);
                SetValueAndFormat(workbook, row.CreateCell(62), data.CRC_PCD); row.CreateCell(63).SetCellValue(data.CRC_COMPLETION); row.CreateCell(64).SetCellValue(data.JOB_LOC);
                row.CreateCell(65).SetCellValue(data.PEX); row.CreateCell(66).SetCellValue(data.ReturnAsZero); row.CreateCell(67).SetCellValue(data.TUID);
                SetValueAndFormat(workbook, row.CreateCell(68), data.DateReceive); row.CreateCell(69).SetCellValue(data.DANumber); row.CreateCell(70).SetCellValue(data.Remarks);
                row.CreateCell(71).SetCellValue(data.wo_cms);
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
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetDataEdit(List<App.Data.Domain.SP_DataInventoryEditForDownload> tbl)
        {
            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.KAL);
                row.CreateCell(1).SetCellValue(data.AlternetPartNumber);
                row.CreateCell(2).SetCellValue(data.LastStatus);
                row.CreateCell(3).SetCellValue(data.StoreNumber);
                row.CreateCell(4).SetCellValue(data.SOS);
                row.CreateCell(5).SetCellValue(data.Surplus);
                row.CreateCell(6).SetCellValue(data.UnitNumber);
                row.CreateCell(7).SetCellValue(data.EquipmentNumber);
                row.CreateCell(8).SetCellValue(data.CUSTOMER_ID);
                row.CreateCell(9).SetCellValue(data.DocDate);
                row.CreateCell(10).SetCellValue(data.DocDateTransfer);
                row.CreateCell(11).SetCellValue(data.DocTransfer);
                row.CreateCell(12).SetCellValue(data.NewWO6F);
                row.CreateCell(13).SetCellValue(data.MO);
                row.CreateCell(14).SetCellValue(data.DocSales);
                row.CreateCell(15).SetCellValue(data.DocReturn);
                row.CreateCell(16).SetCellValue(data.DocWCSL);
            }
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
            sheet.SetColumnWidth(39, 20 * 300);//Location
            sheet.SetColumnWidth(40, 20 * 300);//Customer

            //Cycle 1 
            sheet.SetColumnWidth(41, 20 * 300);//PO Number
            sheet.SetColumnWidth(42, 20 * 300);//Schedule
            sheet.SetColumnWidth(43, 20 * 300);//Unit No
            sheet.SetColumnWidth(44, 20 * 300);//Serial Number
            sheet.SetColumnWidth(45, 20 * 300);//Location
            sheet.SetColumnWidth(46, 20 * 300);//Customer

            sheet.SetColumnWidth(47, 20 * 200);//Used SN
            sheet.SetColumnWidth(48, 20 * 200);//Equipment No
            sheet.SetColumnWidth(49, 20 * 200);//Customer
            sheet.SetColumnWidth(50, 20 * 200);//Supplied Date
            sheet.SetColumnWidth(51, 20 * 200);//Elapsed Days
            sheet.SetColumnWidth(52, 20 * 200);//Stock Transfer Date
            sheet.SetColumnWidth(53, 20 * 200);//Ref Doc Transfer
            sheet.SetColumnWidth(54, 20 * 200);//New WO 6F
            sheet.SetColumnWidth(55, 20 * 200);//WO1K
            sheet.SetColumnWidth(56, 20 * 200);//Old WO
            sheet.SetColumnWidth(57, 20 * 200);//Manual Order
            sheet.SetColumnWidth(58, 20 * 200);//DOC C
            sheet.SetColumnWidth(59, 20 * 200);//DOC R
            sheet.SetColumnWidth(60, 20 * 200);//WCSL
            sheet.SetColumnWidth(61, 20 * 200);//Rebuild Status
            sheet.SetColumnWidth(62, 20 * 356);//CRC Promised Completion Date
            sheet.SetColumnWidth(63, 20 * 200);//CRC Completion
            sheet.SetColumnWidth(64, 20 * 200);//Job Loc
            sheet.SetColumnWidth(65, 20 * 200);//PEX
            sheet.SetColumnWidth(66, 20 * 200);//Return As Zero
            sheet.SetColumnWidth(67, 20 * 200);//TUID
            sheet.SetColumnWidth(68, 20 * 200);//Date Receive
            sheet.SetColumnWidth(69, 20 * 200);//DA Number
            sheet.SetColumnWidth(70, 20 * 300);//Remarks
            sheet.SetColumnWidth(71, 20 * 300);//WO CMS

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
            headerRow.CreateCell(0).SetCellValue("Ref Part Number");
            headerRow.CreateCell(1).SetCellValue("Alt Part Number");
            headerRow.CreateCell(2).SetCellValue("Model");
            headerRow.CreateCell(3).SetCellValue("Prefix");
            headerRow.CreateCell(4).SetCellValue("SMCS");
            headerRow.CreateCell(5).SetCellValue("Component");
            headerRow.CreateCell(6).SetCellValue("MOD");
            headerRow.CreateCell(7).SetCellValue("Status");
            headerRow.CreateCell(8).SetCellValue("Store");
            headerRow.CreateCell(9).SetCellValue("SOS");
            headerRow.CreateCell(10).SetCellValue("KAL");
            headerRow.CreateCell(11).SetCellValue("Core Model");
            headerRow.CreateCell(12).SetCellValue("Family");
            headerRow.CreateCell(13).SetCellValue("CRC TAT");
            headerRow.CreateCell(14).SetCellValue("Section");
            headerRow.CreateCell(15).SetCellValue("Surplus");
            headerRow.CreateCell(16).SetCellValue("Note");

            //Cycle 1
            headerRow.CreateCell(17).SetCellValue("PO Number (Cycle 1)");
            headerRow.CreateCell(18).SetCellValue("Schedule (Cycle 1)");
            headerRow.CreateCell(19).SetCellValue("Unit No. (Cycle 1)");
            headerRow.CreateCell(20).SetCellValue("Serial No. (Cycle 1)");
            headerRow.CreateCell(21).SetCellValue("Location (Cycle 1)");
            headerRow.CreateCell(22).SetCellValue("Customer (Cycle 1)");

            //Cycle 2
            headerRow.CreateCell(23).SetCellValue("PO Number (Cycle 2)");
            headerRow.CreateCell(24).SetCellValue("Schedule (Cycle 2)");
            headerRow.CreateCell(25).SetCellValue("Unit No. (Cycle 2)");
            headerRow.CreateCell(26).SetCellValue("Serial No. (Cycle 2)");
            headerRow.CreateCell(27).SetCellValue("Location (Cycle 2)");
            headerRow.CreateCell(28).SetCellValue("Customer (Cycle 2)");

            //Cycle 1
            headerRow.CreateCell(29).SetCellValue("PO Number (Cycle 3)");
            headerRow.CreateCell(30).SetCellValue("Schedule (Cycle 3)");
            headerRow.CreateCell(31).SetCellValue("Unit No. (Cycle 3)");
            headerRow.CreateCell(32).SetCellValue("Serial No. (Cycle 3)");
            headerRow.CreateCell(33).SetCellValue("Location (Cycle 3)");
            headerRow.CreateCell(34).SetCellValue("Customer (Cycle 3)");

            //Cycle 1
            headerRow.CreateCell(35).SetCellValue("PO Number (Cycle 4)");
            headerRow.CreateCell(36).SetCellValue("Schedule (Cycle 4)");
            headerRow.CreateCell(37).SetCellValue("Unit No. (Cycle 4)");
            headerRow.CreateCell(38).SetCellValue("Serial No. (Cycle 4)");
            headerRow.CreateCell(39).SetCellValue("Location (Cycle 4)");
            headerRow.CreateCell(40).SetCellValue("Customer (Cycle 4)");

            //Cycle 1
            headerRow.CreateCell(41).SetCellValue("PO Number (Cycle 5)");
            headerRow.CreateCell(42).SetCellValue("Schedule (Cycle 5)");
            headerRow.CreateCell(43).SetCellValue("Unit No. (Cycle 5)");
            headerRow.CreateCell(44).SetCellValue("Serial No. (Cycle 5)");
            headerRow.CreateCell(45).SetCellValue("Location (Cycle 5)");
            headerRow.CreateCell(46).SetCellValue("Customer (Cycle 5)");

            headerRow.CreateCell(47).SetCellValue("Used SN");
            headerRow.CreateCell(48).SetCellValue("Equipment No");
            headerRow.CreateCell(49).SetCellValue("Customer");
            headerRow.CreateCell(50).SetCellValue("Supplied Date");
            headerRow.CreateCell(51).SetCellValue("Elapsed Days");
            headerRow.CreateCell(52).SetCellValue("Stock Transfer Date");
            headerRow.CreateCell(53).SetCellValue("Ref Doc Transfer");
            headerRow.CreateCell(54).SetCellValue("New WO 6F");
            headerRow.CreateCell(55).SetCellValue("WO1K");
            headerRow.CreateCell(56).SetCellValue("Old WO");
            headerRow.CreateCell(57).SetCellValue("Manual Order");
            headerRow.CreateCell(58).SetCellValue("DOC C");
            headerRow.CreateCell(59).SetCellValue("DOC R");
            headerRow.CreateCell(60).SetCellValue("WCSL");
            headerRow.CreateCell(61).SetCellValue("Rebuild Status");
            headerRow.CreateCell(62).SetCellValue("CRC Promised Completion Date");
            headerRow.CreateCell(63).SetCellValue("CRC Completion");
            headerRow.CreateCell(64).SetCellValue("Job Loc");
            headerRow.CreateCell(65).SetCellValue("Job Code");//PEX
            headerRow.CreateCell(66).SetCellValue("Return As Zero");
            headerRow.CreateCell(67).SetCellValue("TUID");
            headerRow.CreateCell(68).SetCellValue("Date Receive");
            headerRow.CreateCell(69).SetCellValue("DA Number");
            headerRow.CreateCell(70).SetCellValue("Remarks");
            headerRow.CreateCell(71).SetCellValue("WO CMS");
            return headerRow;
        }

        /// <summary>
        /// Initialize row Header
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private IRow CreateHeaderRowEdit(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("KAL");
            headerRow.CreateCell(1).SetCellValue("Alt Part Number");
            headerRow.CreateCell(2).SetCellValue("Status");
            headerRow.CreateCell(3).SetCellValue("Store");
            headerRow.CreateCell(4).SetCellValue("SOS");
            headerRow.CreateCell(5).SetCellValue("Surplus");
            headerRow.CreateCell(6).SetCellValue("Used SN");
            headerRow.CreateCell(7).SetCellValue("Equipment No");
            headerRow.CreateCell(8).SetCellValue("Customer");
            headerRow.CreateCell(9).SetCellValue("Store Supply Date");
            headerRow.CreateCell(10).SetCellValue("Stock Transfer Date");
            headerRow.CreateCell(11).SetCellValue("Ref Doc Transfer");
            headerRow.CreateCell(12).SetCellValue("New WO 6F");
            headerRow.CreateCell(13).SetCellValue("M Order");
            headerRow.CreateCell(14).SetCellValue("Sale Doc");
            headerRow.CreateCell(15).SetCellValue("Return Doc");
            headerRow.CreateCell(16).SetCellValue("WCSL");
            return headerRow;
        }

        /// <summary>
        /// Set Cycle 1 Allocation By KAL
        /// </summary>
        /// <param name="KAL"></param>
        private void SetCycle1(string KAL)
        {
            PONo_1 = ""; OriSchedule_1 = ""; UnitNo_1 = ""; SerialNo_1 = ""; Store_1 = ""; Customer_1 = "";

            var data = App.Service.CAT.InventoryAllocation.GetByKAL(KAL, 1);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.CUSTOMER_ID))
                {
                    var cust = App.Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(data.CUSTOMER_ID));
                    Customer_1 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
                }
                else
                    Customer_1 = "Customer Not Found";

                PONo_1 = data.PONumber;
                if (data.OriginalSchedule.ToString() == "1/1/1900 00:00:00")
                    OriSchedule_1 = "Date Not Found";
                else
                    OriSchedule_1 = data.OriginalSchedule.ToString("dd MMM yyyy");
                UnitNo_1 = data.UnitNo;
                SerialNo_1 = data.SerialNo;
                Store_1 = GetStroeName(data.StoreID);

            }
        }

        /// <summary>
        /// Set Cycle 2 Allocation By KAL
        /// </summary>
        /// <param name="KAL"></param>
        private void SetCycle2(string KAL)
        {
            PONo_2 = ""; OriSchedule_2 = ""; UnitNo_2 = ""; SerialNo_2 = ""; Store_2 = ""; Customer_2 = "";

            var data = App.Service.CAT.InventoryAllocation.GetByKAL(KAL, 2);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.CUSTOMER_ID))
                {
                    var cust = App.Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(data.CUSTOMER_ID));
                    Customer_2 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
                }
                else
                    Customer_2 = "Customer Not Found";

                PONo_2 = data.PONumber;
                if (data.OriginalSchedule.ToString() == "1/1/1900 00:00:00")
                    OriSchedule_2 = "Date Not Found";
                else
                    OriSchedule_2 = data.OriginalSchedule.ToString("dd MMM yyyy");
                UnitNo_2 = data.UnitNo;
                SerialNo_2 = data.SerialNo;
                Store_2 = GetStroeName(data.StoreID);
                //Customer_2 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
            }
        }

        /// <summary>
        /// Set Cycle 3 Allocation By KAL
        /// </summary>
        /// <param name="KAL"></param>
        private void SetCycle3(string KAL)
        {
            PONo_3 = ""; OriSchedule_3 = ""; UnitNo_3 = ""; SerialNo_3 = ""; Store_3 = ""; Customer_3 = "";

            var data = App.Service.CAT.InventoryAllocation.GetByKAL(KAL, 3);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.CUSTOMER_ID))
                {
                    var cust = App.Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(data.CUSTOMER_ID));
                    Customer_3 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
                }
                else
                    Customer_3 = "Customer Not Found";

                PONo_3 = data.PONumber;
                if (data.OriginalSchedule.ToString() == "1/1/1900 00:00:00")
                    OriSchedule_3 = "Date Not Found";
                else
                    OriSchedule_3 = data.OriginalSchedule.ToString("dd MMM yyyy");
                UnitNo_3 = data.UnitNo;
                SerialNo_3 = data.SerialNo;
                Store_3 = GetStroeName(data.StoreID);
                //Customer_3 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
            }
        }

        /// <summary>
        /// Set Cycle 4 Allocation By KAL
        /// </summary>
        /// <param name="KAL"></param>
        private void SetCycle4(string KAL)
        {
            PONo_4 = ""; OriSchedule_4 = ""; UnitNo_4 = ""; SerialNo_4 = ""; Store_4 = ""; Customer_4 = "";

            var data = App.Service.CAT.InventoryAllocation.GetByKAL(KAL, 4);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.CUSTOMER_ID))
                {
                    var cust = App.Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(data.CUSTOMER_ID));
                    Customer_4 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
                }
                else
                    Customer_4 = "Customer Not Found";

                PONo_4 = data.PONumber;
                if (data.OriginalSchedule.ToString() == "1/1/1900 00:00:00")
                    OriSchedule_4 = "Date Not Found";
                else
                    OriSchedule_4 = data.OriginalSchedule.ToString("dd MMM yyyy");
                UnitNo_4 = data.UnitNo;
                SerialNo_4 = data.SerialNo;
                Store_4 = GetStroeName(data.StoreID);
                //Customer_4 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
            }
        }

        /// <summary>
        /// Set Cycle 5 Allocation By KAL
        /// </summary>
        /// <param name="KAL"></param>
        private void SetCycle5(string KAL)
        {
            PONo_5 = ""; OriSchedule_5 = ""; UnitNo_5 = ""; SerialNo_5 = ""; Store_5 = ""; Customer_5 = "";

            var data = App.Service.CAT.InventoryAllocation.GetByKAL(KAL, 5);
            if (data != null)
            {
                if (!string.IsNullOrEmpty(data.CUSTOMER_ID))
                {
                    var cust = App.Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(data.CUSTOMER_ID));
                    Customer_5 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
                }
                else
                    Customer_5 = "Customer Not Found";

                PONo_5 = data.PONumber;
                if (data.OriginalSchedule.ToString() == "1/1/1900 00:00:00")
                    OriSchedule_5 = "Date Not Found";
                else
                    OriSchedule_5 = data.OriginalSchedule.ToString("dd MMM yyyy");
                UnitNo_5 = data.UnitNo;
                SerialNo_5 = data.SerialNo;
                Store_5 = GetStroeName(data.StoreID);
                //Customer_5 = data.CUSTOMER_ID + " - " + cust.CUSTOMERNAME;
            }
        }

        /// <summary>
        /// Pemngambilan value Storename di table Store By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        private string GetStroeName(int ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var data = db.Stores.Where(w => w.StoreID == ID).FirstOrDefault();
                if (data != null) return data.StoreNo + " - " + data.Name;

                return "";
            }
        }
    }
}