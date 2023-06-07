using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
//using Spire.Xls;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRTaxAudit
    {
        public const string CacheName = "App.EMCS.SvcRTaxAudit";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        public static ISheet sheet;
        public static XSSFWorkbook workbook = new XSSFWorkbook();

        public static List<SpRTaxAudit> TaxAuditList(DateTime startDate, DateTime endDate)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@StartDate", startDate));
                parameterList.Add(new SqlParameter("@EndDate", endDate));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRTaxAudit>(@"[dbo].[SP_RTaxAudit] @StartDate, @EndDate", parameters).ToList();
                return data;
            }
        }

        public static List<SpRTaxAudit> GetTaxAuditList(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartDate", startDate));
                    parameterList.Add(new SqlParameter("@EndDate", endDate));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRTaxAudit>("exec [dbo].[SP_RTaxAudit] @StartDate, @EndDate", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetTaxAuditStream(List<SpRTaxAudit> data)
        {

            workbook = new XSSFWorkbook();

            //Create new Excel Sheet CIPLItem
            sheet = CreateSheetTaxAudit();
            ////Create a header row
            CreateHeaderRowTaxAudit(sheet);
            //Populate the sheet with values from the grid data
            CreateSheetDataTaxAudit(data);

            workbook.SetSheetName(0, "TaxAudit");

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;

        }

        public static ISheet CreateSheetTaxAudit()
        {

            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//No           
            sheet.SetColumnWidth(1, 20 * 200);//Name
            sheet.SetColumnWidth(2, 20 * 200);//Address
            sheet.SetColumnWidth(3, 20 * 200);//PebNo
            sheet.SetColumnWidth(4, 20 * 200);//PebDate  
            sheet.SetColumnWidth(5, 20 * 200);//CurrInvoice
            sheet.SetColumnWidth(6, 20 * 150);//CurrValue
            sheet.SetColumnWidth(7, 20 * 150);//Rate
            sheet.SetColumnWidth(8, 20 * 150);//PpjkName
            sheet.SetColumnWidth(9, 20 * 200);//DppExport
            sheet.SetColumnWidth(10, 20 * 200);//DaNo
            sheet.SetColumnWidth(11, 20 * 200);//DoNo
            sheet.SetColumnWidth(12, 20 * 200);//DoDate
            sheet.SetColumnWidth(13, 20 * 200);//WarehouseLoc
            sheet.SetColumnWidth(14, 20 * 200);//LoadingPort
            sheet.SetColumnWidth(15, 20 * 200);//NpeNo
            sheet.SetColumnWidth(16, 20 * 200);//NpeDate
            sheet.SetColumnWidth(17, 20 * 200);//InvoiceNo    
            sheet.SetColumnWidth(18, 20 * 200);//InvoiceDate     
            sheet.SetColumnWidth(19, 20 * 200);//Publisher
            sheet.SetColumnWidth(20, 20 * 200);//BlAwbNo
            sheet.SetColumnWidth(21, 20 * 200);//BlAwbDate
            sheet.SetColumnWidth(22, 20 * 200);//DestinationPort
            sheet.SetColumnWidth(23, 20 * 200);
            sheet.SetColumnWidth(24, 20 * 200);//Remarks
            sheet.SetColumnWidth(25, 20 * 200);//Remarks
            return sheet;
        }

        public static IRow CreateHeaderRowTaxAudit(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("NO");
            headerRow.CreateCell(1).SetCellValue("BUYER'S NAME");
            headerRow.CreateCell(2).SetCellValue("ADDRESS");
            headerRow.CreateCell(3).SetCellValue("PEB NUMBER");
            headerRow.CreateCell(4).SetCellValue("PEB DATE");
            headerRow.CreateCell(5).SetCellValue("CURRENCY INVOICE");
            headerRow.CreateCell(6).SetCellValue("INVOICE VALUE");
            headerRow.CreateCell(7).SetCellValue("TAX RATE");
            headerRow.CreateCell(8).SetCellValue("DPP EXPORT");
            headerRow.CreateCell(9).SetCellValue("PPJK NAME");
            headerRow.CreateCell(10).SetCellValue("PPJK ADDRESS");
            headerRow.CreateCell(11).SetCellValue("DA NUMBER");
            headerRow.CreateCell(12).SetCellValue("DO NUMBER");
            headerRow.CreateCell(13).SetCellValue("DATE (DO)");
            headerRow.CreateCell(14).SetCellValue("PILE PLACE");
            headerRow.CreateCell(15).SetCellValue("RECEIPT");
            headerRow.CreateCell(16).SetCellValue("LOADING PORT");
            headerRow.CreateCell(17).SetCellValue("EXPORT APPROVAL");
            headerRow.CreateCell(18).SetCellValue("DATE (Export Approval)");
            headerRow.CreateCell(19).SetCellValue("INVOICE NUMBER");
            headerRow.CreateCell(20).SetCellValue("DATE (Invoice)");
            headerRow.CreateCell(21).SetCellValue("PUBLISHER");
            headerRow.CreateCell(22).SetCellValue("NUMBER (Publisher)");
            headerRow.CreateCell(23).SetCellValue("DATE (Publisher)");
            headerRow.CreateCell(24).SetCellValue("PORT OF LOADING");
            headerRow.CreateCell(25).SetCellValue("REMARKS");
            return headerRow;
        }

        public static void CreateSheetDataTaxAudit(List<SpRTaxAudit> tbl)
        {

            int rowNumber = 1;

            foreach (var data in tbl)
            {

                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.no);
                row.CreateCell(1).SetCellValue(data.Name);
                row.CreateCell(2).SetCellValue(data.Address);
                row.CreateCell(3).SetCellValue(data.PebNo);
                row.CreateCell(4).SetCellValue(data.PebDate);
                row.CreateCell(5).SetCellValue(data.CurrInvoice);
                row.CreateCell(6).SetCellValue(data.CurrValue.ToString());
                row.CreateCell(7).SetCellValue(data.Rate.ToString());
                row.CreateCell(8).SetCellValue(data.DppExport.ToString());
                row.CreateCell(9).SetCellValue(data.PpjkName);
                row.CreateCell(10).SetCellValue("-");
                row.CreateCell(11).SetCellValue("-");
                row.CreateCell(12).SetCellValue(data.DoNo);
                row.CreateCell(13).SetCellValue(data.DoDate);
                row.CreateCell(14).SetCellValue(data.WarehouseLoc);
                row.CreateCell(15).SetCellValue(data.NpeNo);
                row.CreateCell(16).SetCellValue(data.LoadingPort);
                row.CreateCell(17).SetCellValue(data.NpeNo);
                row.CreateCell(18).SetCellValue(data.NpeDate);
                row.CreateCell(19).SetCellValue(data.InvoiceNo);
                row.CreateCell(20).SetCellValue(data.InvoiceDate);
                row.CreateCell(21).SetCellValue(data.Publisher);
                row.CreateCell(22).SetCellValue(data.BlAwbNo);
                row.CreateCell(23).SetCellValue(data.BlAwbDate);
                row.CreateCell(24).SetCellValue(data.DestinationPort);
                row.CreateCell(25).SetCellValue(data.Remarks);
            }
        }

        //public static MemoryStream GetTaxAuditStream(List<SpRTaxAudit> data, string fileExcel)
        //{
        //    try
        //    {
        //        Workbook workbook = new Workbook();
        //        workbook.LoadFromFile(fileExcel);
        //        workbook.ConverterSetting.SheetFitToPage = true;

        //        MemoryStream output = new MemoryStream();

        //        #region tabularSheet
        //        Worksheet tabularSheet = workbook.Worksheets[0];
        //        int startRow = 4;
        //        for (int i = 0; i < data.Count; i++)
        //        {
        //            tabularSheet.InsertRow(startRow + i);

        //            tabularSheet.SetCellValue(startRow + i, 1, data[i].Name);
        //            tabularSheet.SetCellValue(startRow + i, 2, data[i].Address);
        //            tabularSheet.SetCellValue(startRow + i, 3, data[i].PebNo);
        //            tabularSheet.SetCellValue(startRow + i, 4, data[i].NpeDate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 5, data[i].CurrInvoice);
        //            tabularSheet.SetCellValue(startRow + i, 6, data[i].CurrValue.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 7, data[i].Rate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 8, data[i].PpjkName);
        //            tabularSheet.SetCellValue(startRow + i, 9, data[i].DppExport.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 10, data[i].DoNo);
        //            tabularSheet.SetCellValue(startRow + i, 11, data[i].DaNo);
        //            tabularSheet.SetCellValue(startRow + i, 12, data[i].DoDate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 13, data[i].WarehouseLoc);
        //            tabularSheet.SetCellValue(startRow + i, 14, data[i].LoadingPort);
        //            tabularSheet.SetCellValue(startRow + i, 15, data[i].NpeNo);
        //            tabularSheet.SetCellValue(startRow + i, 16, data[i].NpeDate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 17, data[i].InvoiceNo);
        //            tabularSheet.SetCellValue(startRow + i, 18, data[i].InvoiceDate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 19, data[i].Publisher);
        //            tabularSheet.SetCellValue(startRow + i, 20, data[i].BlAwbNo);
        //            tabularSheet.SetCellValue(startRow + i, 21, data[i].BlAwbDate.ToString());
        //            tabularSheet.SetCellValue(startRow + i, 22, data[i].DestinationPort);
        //            tabularSheet.SetCellValue(startRow + i, 23, data[i].Remarks);
        //        }
        //        #endregion

        //        workbook.SaveToStream(output, FileFormat.Version97to2003);

        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}
    }
}
