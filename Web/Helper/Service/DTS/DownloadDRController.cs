using App.Data.Domain;
using NPOI.HSSF.UserModel;
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
    public class DownloadDRController : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        #region Delivery Resuisition
        /// <summary>
        /// download data Inbound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcel(App.Data.Domain.DTS.DeliveryRequisitionFilter filter)
        {
            try
            {
                var tbl = App.Service.DTS.DeliveryRequisition.GetListFilter(filter, true);

                tbl = tbl.OrderByDescending(a => a.ID).ToList();
                sheet = CreateSheet();
                CreateHeaderRow(sheet);
                CreateSheetData(tbl);
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "DeliveryRequisition.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "DeliveryRequisition.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetData(List<App.Data.Domain.DeliveryRequisition> tbl)
        {

            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);
                row.CreateCell(0).SetCellValue(data.KeyCustom);
                string status = data.Status;
                switch (data.Status)
                {
                    case "approve":
                        status = "IN PROGRESS";
                        break;
                    case "reject":
                        status = "REJECTED";
                        break;
                    case "revise":
                        status = "NEED REVISION";
                        break;
                    case "revised":
                        status = "REVISED";
                        break;
                    case "submit":
                        status = "SUBMITED";
                        break;
                    case "complete":
                        status = "COMPLETED";
                        break;
                    case "booked":
                        status = "BOOKED";
                        break;
                    default:
                        status = "DRAFT";
                        break;
                }
                row.CreateCell(1).SetCellValue(status);
                row.CreateCell(2).SetCellValue(data.ReqName);
                row.CreateCell(3).SetCellValue(data.ReqHp);
                row.CreateCell(4).SetCellValue(data.Unit);

                row.CreateCell(5).SetCellValue(data.Model);
                row.CreateCell(6).SetCellValue(data.SerialNumber);
                row.CreateCell(7).SetCellValue(data.Batch);

                row.CreateCell(8).SetCellValue(data.Origin);
                row.CreateCell(9).SetCellValue(data.CustName);
                row.CreateCell(10).SetCellValue(data.Kabupaten);
                row.CreateCell(11).SetCellValue(data.PicName);
                row.CreateCell(12).SetCellValue(data.PicHP);
                row.CreateCell(13).SetCellValue(data.ExpectedTimeLoading.ToString());

            }
        }

        //static void SetValueAndFormat(IWorkbook workbook, ICell cell, string value)
        //{
        //    IDataFormat format = workbook.CreateDataFormat();
        //    short formatId = format.GetFormat("dd MMM yyyy");
        //    //set value for the cell
        //    if (!string.IsNullOrEmpty(value))
        //        cell.SetCellValue(Convert.ToDateTime(value));

        //    ICellStyle cellStyle = workbook.CreateCellStyle();
        //    cellStyle.DataFormat = formatId;
        //    cell.CellStyle = cellStyle;
        //}


        /// <summary>
        /// Initialize  Sheet Excel
        /// </summary>
        /// <returns></returns>
        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//DR NO
            sheet.SetColumnWidth(1, 20 * 200);//STATUS
            sheet.SetColumnWidth(2, 20 * 200);//Requestor Name
            sheet.SetColumnWidth(3, 20 * 256);//Requestor Hp
            sheet.SetColumnWidth(4, 20 * 256);//Unit
            sheet.SetColumnWidth(5, 20 * 256);//Model
            sheet.SetColumnWidth(6, 20 * 256);//SerialNumber
            sheet.SetColumnWidth(7, 20 * 256);//Batch
            sheet.SetColumnWidth(8, 20 * 200);//Origin
            sheet.SetColumnWidth(9, 20 * 200);//Cust Name
            sheet.SetColumnWidth(10, 20 * 150);//District / Kabupaten
            sheet.SetColumnWidth(11, 20 * 150);//PIC Name
            sheet.SetColumnWidth(12, 20 * 150);//PIC HP
            sheet.SetColumnWidth(13, 20 * 200);//ExpectedTimeLoading
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
            headerRow.CreateCell(0).SetCellValue("DR NO");
            headerRow.CreateCell(1).SetCellValue("STATUS");
            headerRow.CreateCell(2).SetCellValue("REQUESTER NAME");
            headerRow.CreateCell(3).SetCellValue("REQUESTER HP");
            headerRow.CreateCell(4).SetCellValue("UNIT");

            headerRow.CreateCell(5).SetCellValue("MODEL");
            headerRow.CreateCell(6).SetCellValue("SN");
            headerRow.CreateCell(7).SetCellValue("BATCH");

            headerRow.CreateCell(8).SetCellValue("ORIGIN");
            headerRow.CreateCell(9).SetCellValue("CUSTOMER NAME");
            headerRow.CreateCell(10).SetCellValue("DISTRICT");
            headerRow.CreateCell(11).SetCellValue("PIC NAME");
            headerRow.CreateCell(12).SetCellValue("PIC CONTACT");
            headerRow.CreateCell(13).SetCellValue("ETD");
            return headerRow;
        }
        #endregion

        #region ReportDelivery Resuisition
        private List<Tuple<string, string>> reportColumns = new List<Tuple<string, string>> {
            new Tuple<string, string>("DR NO","KeyCustom"),
            new Tuple<string, string>("CREATE BY","CreateBy"),
            new Tuple<string, string>("CREATE DATE","CreateDate"),
            new Tuple<string, string>("STATUS","Status_DR"),
            new Tuple<string, string>("ACTIVITY","Activity_DR"),
            new Tuple<string, string>("POSITION","Position"),
            new Tuple<string, string>("REQUESTER NAME","ReqName"),
            new Tuple<string, string>("REQUESTER HP","ReqHp"),
            new Tuple<string, string>("SALES NAME 1","Sales1Name"),
            new Tuple<string, string>("SALES HP 1","Sales1Hp"),
            new Tuple<string, string>("SALES NAME 1","Sales2Name"),
            new Tuple<string, string>("SALES HP 2","Sales2Hp"),
            new Tuple<string, string>("UNIT","Unit"),
            new Tuple<string, string>("MODEL","Model"),
            new Tuple<string, string>("SN","SerialNumber"),
            new Tuple<string, string>("BATCH","Batch"),
            new Tuple<string, string>("ORIGIN","Origin"),
            new Tuple<string, string>("CUSTOMER NAME","CustName"),
            new Tuple<string, string>("PIC NAME","PicName"),
            new Tuple<string, string>("PIC CONTACT","PicHP"),
            new Tuple<string, string>("ADDRESS","CustAddress"),
            new Tuple<string, string>("SUB-DISTRICT","Kecamatan"),
            new Tuple<string, string>("DISTRICT","Kabupaten"),
            new Tuple<string, string>("PROVINCE","Province"),
            new Tuple<string, string>("TERM OF DELIVERY","TermOfDelivery"),
            new Tuple<string, string>("SUPPORTING DOCUMENT","SupportingOfDelivery"),
            new Tuple<string, string>("INCOTERM","Incoterm"),
            new Tuple<string, string>("TRANSPORTATION","Transportation"),
            new Tuple<string, string>("MODA TRANSPORT","ModaTransport"),
            new Tuple<string, string>("VESSEL NAME","VeselName"),
            new Tuple<string, string>("VESSEL PIC NAME","Unit_PICName"),
            new Tuple<string, string>("VESSEL PIC HP","Unit_PICHp"),
            new Tuple<string, string>("VESSEL NO POLICE","VeselNoPolice"),
            new Tuple<string, string>("DRIVER NAME","DriverName"),
            new Tuple<string, string>("DRIVER HP","DriverHp"),
            new Tuple<string, string>("SO NO","SoNo"),
            new Tuple<string, string>("SO DATE","SoDate"),
            new Tuple<string, string>("STR NO","STRNo"),
            new Tuple<string, string>("STR DATE","STRDate"),
            new Tuple<string, string>("DI NO","DINo"),
            new Tuple<string, string>("DI DATE","DIDate"),
            new Tuple<string, string>("ETD","ExpectedTimeLoading"),
            new Tuple<string, string>("ETA","ExpectedTimeArrival"),
            new Tuple<string, string>("ATD","ActTimeDeparture"),
            new Tuple<string, string>("ATA","ActTimeArrival"),
        };
        public FileResult DownloadReportToExcel(App.Data.Domain.DTS.ReportDeliveryRequisitionFilter filter)
        {
            try
            {
                var tbl = App.Service.DTS.DeliveryRequisition.GetReportListFilter(filter);
                // tbl = tbl.OrderBy(a => a.ID).ToList();
                sheet = CreateSheetReport();
                CreateHeaderRowReport(sheet);
                //(Optional) freeze the header row so it is not scrolled
                //sheet.CreateFreezePane(0, 1, 0, 1);
                //Populate the sheet with values from the grid data
                CreateSheetDataReport(tbl);
                MemoryStream output = new MemoryStream();
                workbook.Write(output);
                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportDeliveryRequisition.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "ReportDeliveryRequisition.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        private IRow CreateHeaderRowReport(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            int colNumber = 0;
            foreach (var col in reportColumns)
            {
                headerRow.CreateCell(colNumber).SetCellValue(col.Item1);
                colNumber++;
            }

            return headerRow;
        }

        private ISheet CreateSheetReport()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//DR NO
            sheet.SetColumnWidth(1, 20 * 200);//STATUS
            sheet.SetColumnWidth(2, 20 * 200);//Requestor Name
            sheet.SetColumnWidth(3, 20 * 256);//Requestor Hp
            sheet.SetColumnWidth(4, 20 * 256);//Unit
            sheet.SetColumnWidth(5, 20 * 200);//Origin
            sheet.SetColumnWidth(6, 20 * 200);//Cust Name
            sheet.SetColumnWidth(7, 20 * 150);//District / Kabupaten
            sheet.SetColumnWidth(8, 20 * 150);//PIC Name
            sheet.SetColumnWidth(9, 20 * 150);//PIC HP
            sheet.SetColumnWidth(10, 20 * 200);//ExpectedTimeLoading
            return sheet;
        }

        private void CreateSheetDataReport(List<App.Data.Domain.ReportDeliveryRequisition> tbl)
        {
            //string[] fieldDate = new string[] { "SoDate", "STRDate", "DIDate", "ExpectedTimeLoading", "ExpectedTimeArrival"};
            //string[] fieldOtherValue = new string[] { "TermOfDelivery", "SupportingOfDelivery", "Incoterm", "Transportation"};
            int rowNumber = 1;
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                int colNumber = 0;
                foreach (var col in reportColumns)
                {
                    var propertyInfo = data.GetType().GetProperty(col.Item2);
                    if (propertyInfo != null)
                    {
                        
                        try
                        {
                            row.CreateCell(colNumber).SetCellValue(propertyInfo.GetValue(data, null).ToString());
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("error col {0}, {1}", col.Item2, ex.ToString());
                            row.CreateCell(colNumber).SetCellValue("");
                        }
                    }
                    colNumber++;
                }
            }
        }

        #endregion

        public FileResult DownloadTemplateUpload(string path, List<DeliveryRequisitionRef> tbl)
        {
            try
            {
                XSSFWorkbook workbook;
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(file);
                    file.Close();
                }

                ISheet sheetDR = workbook.GetSheetAt(1);
                ISheet sheetSN = workbook.GetSheetAt(2);
                //IRow rowDr = sheet.GetRow(0);
                
                if (tbl.Count() > 0)
                {
                    Int64 lastID = 0;
                    
                    int rowDRNumber = 0;
                    int rowSNNumber = 0;
                    int colSNNumber = -1;
                    foreach (var item in tbl)
                    {
                        IRow rowSN;
                        if (lastID != item.ID)
                        {
                            sheetDR.CreateRow(rowDRNumber++).CreateCell(0).SetCellValue(item.KeyCustom);
                            lastID = item.ID;

                            rowSNNumber = 0;
                            colSNNumber++;
                            rowSN = sheetSN.GetRow(rowSNNumber);
                            if (rowSN != null)
                            {
                                rowSN.CreateCell(colSNNumber).SetCellValue(item.KeyCustom);
                            }
                            else
                            {
                                sheetSN.CreateRow(rowSNNumber).CreateCell(colSNNumber).SetCellValue(item.KeyCustom);
                            }
                            rowSNNumber++;
                        }

                        rowSN = sheetSN.GetRow(rowSNNumber);
                        if (rowSN != null)
                        {
                            rowSN.CreateCell(colSNNumber).SetCellValue(item.SerialNumber);
                        }
                        else
                        {
                            sheetSN.CreateRow(rowSNNumber).CreateCell(colSNNumber).SetCellValue(item.SerialNumber);
                        }
                        rowSNNumber++;
                    }
                }
                

                MemoryStream output = new MemoryStream();
                workbook.Write(output);
                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Template Mass Upload DR.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "Template Mass Upload DR.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        public FileResult DownloadTemplateUpload2(string path, List<DeliveryRequisitionRef> tbl)
        {
            try
            {
                XSSFWorkbook workbook;
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(file);
                    file.Close();
                }

                ISheet sheetDR = workbook.GetSheetAt(0);

                if (tbl.Count() > 0)
                {
                    int rowDRNumber = 1;
                    foreach (var item in tbl)
                    {
                        var drRow = sheetDR.CreateRow(rowDRNumber++);
                        drRow.CreateCell(0).SetCellValue(item.KeyCustom);
                        drRow.CreateCell(1).SetCellValue(item.Model);
                        drRow.CreateCell(2).SetCellValue(item.SerialNumber);
                        drRow.CreateCell(3).SetCellValue(item.Batch);
                        drRow.CreateCell(4).SetCellValue(item.ItemId);
                    }
                }

                MemoryStream output = new MemoryStream();
                workbook.Write(output);
                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Template Mass Upload DR.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
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
                 "Template Mass Upload DR.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }
    }
}