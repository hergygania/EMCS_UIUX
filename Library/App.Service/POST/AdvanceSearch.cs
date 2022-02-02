using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Web;

namespace App.Service.POST
{
    public static class AdvanceSearh
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        public readonly static XSSFWorkbook workbookSLA = new XSSFWorkbook();
<<<<<<< HEAD
        public readonly static XSSFWorkbook workbookGR = new XSSFWorkbook();
=======
>>>>>>> 639d8d0 (Intial commit)

        public static List<AdvanceSearchModel> GetListAdvanceSearch(SearchAdvance param)
        {
            if (param.startDeliveryDate != null) param.startDeliveryDate = DateTime.ParseExact(param.startDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.endDeliveryDate != null) param.endDeliveryDate = DateTime.ParseExact(param.endDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.startPODate != null) param.startPODate = DateTime.ParseExact(param.startPODate, Global.dateformatParam, null).ToString();
            if (param.endPODate != null) param.endPODate = DateTime.ParseExact(param.endPODate, Global.dateformatParam, null).ToString();


            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@statusPO", param.statusPO ?? ""));
                parameterList.Add(new SqlParameter("@startPODate", param.startPODate ?? ""));
                parameterList.Add(new SqlParameter("@endPODate", param.endPODate ?? ""));
                parameterList.Add(new SqlParameter("@branch", param.branch ?? ""));
                parameterList.Add(new SqlParameter("@supplier", param.supplier ?? ""));
                parameterList.Add(new SqlParameter("@userPIC", param.userPIC ?? ""));
                parameterList.Add(new SqlParameter("@startDeliveryDate", param.startDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@endDeliveryDate", param.endDeliveryDate ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<AdvanceSearchModel>(@"exec [dbo].[SP_AdvanceSearch_LIST]
	                @statusPO		   
                  , @startPODate	   
                  , @endPODate		   
                  , @branch			   
                  , @supplier		   
                  , @userPIC		   
                  , @startDeliveryDate 
                  , @endDeliveryDate                   
                ", parameters).ToList();
                return data;
            }
        }

        public static IRow CreateHeaderRowSla(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("NPWP");
            headerRow.CreateCell(1).SetCellValue("MAP");
            headerRow.CreateCell(2).SetCellValue("KJS");
            headerRow.CreateCell(3).SetCellValue("MasaPajak");
            headerRow.CreateCell(4).SetCellValue("MasaPajak2");
            headerRow.CreateCell(4).SetCellValue("TahunPajak");
            headerRow.CreateCell(4).SetCellValue("Amount");

            return headerRow;
        }

        public static ICell SetCellStyleSla(ICell Cell, string Value, ICellStyle CellStyle)
        {
            CellStyle.BorderLeft = BorderStyle.Medium;
            CellStyle.BorderTop = BorderStyle.Medium;
            CellStyle.BorderRight = BorderStyle.Medium;
            CellStyle.BorderBottom = BorderStyle.Medium;
            Cell.SetCellValue(Value);
            Cell.CellStyle = CellStyle;
            return Cell;
        }

        public static MemoryStream DownloadToExcelSla(SearchReport model, string user)
        {
            try
            {
                //Create new Excel Sheet
                XSSFWorkbook workbookSLA = new XSSFWorkbook();
                ISheet sheetSLA = workbookSLA.CreateSheet();

                //Create a header row
                var font = workbookSLA.CreateFont();
                var headerRow = sheetSLA.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("PO No");
<<<<<<< HEAD
                headerRow.CreateCell(1).SetCellValue("PO Date");               
                headerRow.CreateCell(2).SetCellValue("PO Line Item");
                headerRow.CreateCell(3).SetCellValue("Item Desc");
                headerRow.CreateCell(4).SetCellValue("Plant");
                headerRow.CreateCell(5).SetCellValue("Vendor");
                headerRow.CreateCell(6).SetCellValue("PO Confirm Date");
                headerRow.CreateCell(7).SetCellValue("PR Number");
                headerRow.CreateCell(8).SetCellValue("PR line item");
                headerRow.CreateCell(9).SetCellValue("PR Date");
                headerRow.CreateCell(10).SetCellValue("PR Creator");
                headerRow.CreateCell(11).SetCellValue("Ordering By");
                headerRow.CreateCell(12).SetCellValue("Request Date");
                headerRow.CreateCell(13).SetCellValue("Promise Date");
                headerRow.CreateCell(14).SetCellValue("Aging");
                headerRow.CreateCell(15).SetCellValue("ETD");
                headerRow.CreateCell(16).SetCellValue("ATD");
                headerRow.CreateCell(17).SetCellValue("ETA");
                headerRow.CreateCell(18).SetCellValue("ATA");
                headerRow.CreateCell(19).SetCellValue("Plan Start Date");
                headerRow.CreateCell(20).SetCellValue("Actual Complete Date");
                headerRow.CreateCell(21).SetCellValue("Plan Complete Date");
                headerRow.CreateCell(22).SetCellValue("Actual Finish Date");
                headerRow.CreateCell(23).SetCellValue("M-L");
                headerRow.CreateCell(24).SetCellValue("P-O");
                headerRow.CreateCell(25).SetCellValue("GR / SA Number");
                headerRow.CreateCell(26).SetCellValue("GR / SA Posting Date");
                headerRow.CreateCell(27).SetCellValue("GR / SA Document Date");
                headerRow.CreateCell(28).SetCellValue("GR / SA Amount");             
                headerRow.CreateCell(29).SetCellValue("Invoice Number");
                headerRow.CreateCell(30).SetCellValue("Invoice Posting Date");
                headerRow.CreateCell(31).SetCellValue("Invoice Date");
                headerRow.CreateCell(32).SetCellValue("PO Status");
            
=======
                headerRow.CreateCell(1).SetCellValue("PO Date");
                headerRow.CreateCell(2).SetCellValue("Item Category");
                headerRow.CreateCell(3).SetCellValue("PO Line Item");
                headerRow.CreateCell(4).SetCellValue("Item Desc");
                headerRow.CreateCell(5).SetCellValue("Plant");
                headerRow.CreateCell(6).SetCellValue("Vendor");
                headerRow.CreateCell(7).SetCellValue("PO Confirm Date");
                headerRow.CreateCell(8).SetCellValue("PR Number");
                headerRow.CreateCell(9).SetCellValue("PR line item");
                headerRow.CreateCell(10).SetCellValue("PR Date");
                headerRow.CreateCell(11).SetCellValue("PR Creator");
                headerRow.CreateCell(12).SetCellValue("Ordering By");
                headerRow.CreateCell(13).SetCellValue("Request Date");
                headerRow.CreateCell(14).SetCellValue("Promise Date");
                headerRow.CreateCell(15).SetCellValue("Aging");
                headerRow.CreateCell(16).SetCellValue("ETD");
                headerRow.CreateCell(17).SetCellValue("ATD");
                headerRow.CreateCell(18).SetCellValue("ETA");
                headerRow.CreateCell(19).SetCellValue("ATA");
                headerRow.CreateCell(20).SetCellValue("Plan Start Date");
                headerRow.CreateCell(21).SetCellValue("Actual Complete Date");
                headerRow.CreateCell(22).SetCellValue("Plan Complete Date");
                headerRow.CreateCell(23).SetCellValue("Actual Finish Date");
                headerRow.CreateCell(24).SetCellValue("M-L");
                headerRow.CreateCell(25).SetCellValue("P-O");
                headerRow.CreateCell(26).SetCellValue("GR / SA Number");
                headerRow.CreateCell(27).SetCellValue("GR / SA Posting Date");
                headerRow.CreateCell(28).SetCellValue("GR / SA Document Date");
                headerRow.CreateCell(29).SetCellValue("Cost Center");
                headerRow.CreateCell(30).SetCellValue("Invoice Number");
                headerRow.CreateCell(31).SetCellValue("Invoice Posting Date");
                headerRow.CreateCell(32).SetCellValue("Invoice Date");
                headerRow.CreateCell(33).SetCellValue("PO Status");

>>>>>>> 639d8d0 (Intial commit)
                int rowNumber = 1;
                model.isExport = true;
                var data = Service.POST.Report.GetListReportSla(user, model);

                foreach (var item in data)
                {
                    var rowQuestion = sheetSLA.CreateRow(rowNumber);
                    rowQuestion.CreateCell(0).SetCellValue(item.PO_Number);

                    if (item.PO_Date.HasValue)
                        rowQuestion.CreateCell(1).SetCellValue(item.PO_Date.ToString());
<<<<<<< HEAD
                    rowQuestion.CreateCell(2).SetCellValue(item.PO_lineitem);
                    rowQuestion.CreateCell(3).SetCellValue(item.ItemDescription);
                    rowQuestion.CreateCell(4).SetCellValue(item.Plant);
                    rowQuestion.CreateCell(5).SetCellValue(item.VendorName);
                    if (item.PO_ConfirmDate.HasValue)
                        rowQuestion.CreateCell(6).SetCellValue(item.PO_ConfirmDate.ToString());

                    rowQuestion.CreateCell(7).SetCellValue(item.PR_Number);
                    rowQuestion.CreateCell(8).SetCellValue(item.PR_lineitem);

                    if (item.PR_Date.HasValue)
                        rowQuestion.CreateCell(9).SetCellValue(item.PR_Date.ToString());

                    rowQuestion.CreateCell(10).SetCellValue(item.PR_Creator);
                    rowQuestion.CreateCell(11).SetCellValue(item.OrderingBy);

                    if (item.RequestDate.HasValue)
                        rowQuestion.CreateCell(12).SetCellValue(item.RequestDate.ToString());

                    rowQuestion.CreateCell(13).SetCellValue(item.PromiseDate);
                    rowQuestion.CreateCell(14).SetCellValue(item.Aging);
                    rowQuestion.CreateCell(15).SetCellValue(item.ETD);
                    rowQuestion.CreateCell(16).SetCellValue(item.ATD);
                    rowQuestion.CreateCell(17).SetCellValue(item.ETA);
                    rowQuestion.CreateCell(18).SetCellValue(item.ATA);
                    rowQuestion.CreateCell(19).SetCellValue(item.PlanStartDate);
                    rowQuestion.CreateCell(20).SetCellValue(item.ActualCompleteDate);
                    rowQuestion.CreateCell(21).SetCellValue(item.PlanCompleteDate);
                    rowQuestion.CreateCell(22).SetCellValue(item.ActualFinishDate);
                    rowQuestion.CreateCell(23).SetCellValue(item.M_L);
                    rowQuestion.CreateCell(24).SetCellValue(item.P_O);
                    rowQuestion.CreateCell(25).SetCellValue(item.SA_Number);
                    rowQuestion.CreateCell(26).SetCellValue(item.SA_PostingDate);
                    rowQuestion.CreateCell(27).SetCellValue(item.SA_DocumentDate);
                    rowQuestion.CreateCell(28).SetCellValue(item.SA_Amount);                  
                    rowQuestion.CreateCell(29).SetCellValue(item.Invoice_Number);
                    rowQuestion.CreateCell(30).SetCellValue(item.Invoice_PostingDate.ToString());
                    rowQuestion.CreateCell(31).SetCellValue(item.Invoice_Date.ToString());
                    rowQuestion.CreateCell(32).SetCellValue(item.POStatus);
                
=======

                    //rowQuestion.CreateCell(2).SetCellValue(item.Item_Category);
                    rowQuestion.CreateCell(3).SetCellValue(item.PO_lineitem);
                    rowQuestion.CreateCell(4).SetCellValue(item.ItemDescription);
                    rowQuestion.CreateCell(5).SetCellValue(item.Plant);
                    rowQuestion.CreateCell(6).SetCellValue(item.VendorName);
                    if (item.PO_ConfirmDate.HasValue)
                        rowQuestion.CreateCell(7).SetCellValue(item.PO_ConfirmDate.ToString());

                    rowQuestion.CreateCell(8).SetCellValue(item.PR_Number);
                    rowQuestion.CreateCell(9).SetCellValue(item.PR_lineitem);

                    if (item.PR_Date.HasValue)
                        rowQuestion.CreateCell(10).SetCellValue(item.PR_Date.ToString());

                    rowQuestion.CreateCell(11).SetCellValue(item.PR_Creator);
                    rowQuestion.CreateCell(12).SetCellValue(item.OrderingBy);

                    if (item.RequestDate.HasValue)
                        rowQuestion.CreateCell(13).SetCellValue(item.RequestDate.ToString());

                    rowQuestion.CreateCell(14).SetCellValue(item.PromiseDate);
                    rowQuestion.CreateCell(15).SetCellValue(item.Aging);
                    rowQuestion.CreateCell(16).SetCellValue(item.ETD);
                    rowQuestion.CreateCell(17).SetCellValue(item.ATD);
                    rowQuestion.CreateCell(18).SetCellValue(item.ETA);
                    rowQuestion.CreateCell(19).SetCellValue(item.ATA);
                    rowQuestion.CreateCell(20).SetCellValue(item.PlanStartDate);
                    rowQuestion.CreateCell(21).SetCellValue(item.ActualCompleteDate);
                    rowQuestion.CreateCell(22).SetCellValue(item.PlanCompleteDate);
                    rowQuestion.CreateCell(23).SetCellValue(item.ActualFinishDate);
                    rowQuestion.CreateCell(24).SetCellValue(item.M_L);
                    rowQuestion.CreateCell(25).SetCellValue(item.P_O);
                    rowQuestion.CreateCell(26).SetCellValue(item.SA_Number);
                    rowQuestion.CreateCell(27).SetCellValue(item.SA_PostingDate);
                    rowQuestion.CreateCell(28).SetCellValue(item.SA_DocumentDate);
                    //rowQuestion.CreateCell(29).SetCellValue(item.CostCenter);
                    rowQuestion.CreateCell(30).SetCellValue(item.Invoice_Number);
                    rowQuestion.CreateCell(31).SetCellValue(item.Invoice_PostingDate.ToString());
                    rowQuestion.CreateCell(32).SetCellValue(item.Invoice_Date.ToString());
                    rowQuestion.CreateCell(33).SetCellValue(item.POStatus);

>>>>>>> 639d8d0 (Intial commit)
                    rowNumber = rowNumber + 1;
                }
                MemoryStream output = new MemoryStream();
                workbookSLA.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MemoryStream output = new MemoryStream();
                workbookSLA.Write(output);
                return output;
            }
        }
<<<<<<< HEAD

        public static MemoryStream DownloadToExcelGR(string PoNo,string ItemId)
        {
            try
            {
                //Create new Excel Sheet
                XSSFWorkbook workbookGR = new XSSFWorkbook();
                ISheet sheetGR = workbookGR.CreateSheet();

                //Create a header row
                var font = workbookGR.CreateFont();
                var headerRow = sheetGR.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("PO Number");
                headerRow.CreateCell(1).SetCellValue("PO Item");
                headerRow.CreateCell(2).SetCellValue("Item Description");
                headerRow.CreateCell(3).SetCellValue("GR Number");
                headerRow.CreateCell(4).SetCellValue("GR Date");
                headerRow.CreateCell(5).SetCellValue("GR Posting Date");
                headerRow.CreateCell(6).SetCellValue("GR Amount");
               

                int rowNumber = 1;
              
                var data =Service.POST.Transaction.GetDataGRByPO(PoNo, "");

                foreach (var item in data)
                {
                    var rowQuestion = sheetGR.CreateRow(rowNumber);
                    rowQuestion.CreateCell(0).SetCellValue(item.PONumber);
                    rowQuestion.CreateCell(1).SetCellValue(item.POItem);
                    rowQuestion.CreateCell(2).SetCellValue(item.ItemDescription);
                    rowQuestion.CreateCell(3).SetCellValue(item.GRNo);
                    rowQuestion.CreateCell(4).SetCellValue(item.GRDate.ToString());
                    rowQuestion.CreateCell(5).SetCellValue(item.GRPostingDate.ToString());
                    rowQuestion.CreateCell(6).SetCellValue(item.GRValue.ToString());
                  
                   
                    rowNumber = rowNumber + 1;
                }
                MemoryStream output = new MemoryStream();
                workbookGR.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MemoryStream output = new MemoryStream();
                workbookGR.Write(output);
                return output;
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)
    }
}
