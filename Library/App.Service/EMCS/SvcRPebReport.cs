using App.Data.Domain.EMCS;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.EMCS
{
    public class SvcRPebReport
    {
        public static ISheet sheet;
        public static XSSFWorkbook workbook = new XSSFWorkbook();
        public static List<SpRPebReport> RPebReports_old(DetailTrackingListFilter filter)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    //if (filter.startMonth != null)
                    //{
                    //    var Start = Convert.ToDateTime(filter.startMonth);
                    //}
                    //if (filter.endMonth != null)
                    //{
                    //    filter.endMonth = Convert.ToString(filter.endMonth);  
                    //}
                    parameterList.Add(new SqlParameter("@StartMonth", filter.startMonth ?? ""));
                    parameterList.Add(new SqlParameter("@EndMonth", filter.endMonth ?? ""));
                    parameterList.Add(new SqlParameter("@ParamName", filter.paramName ?? ""));
                    parameterList.Add(new SqlParameter("@ParamValue", filter.paramValue ?? ""));
                    parameterList.Add(new SqlParameter("@KeyNum", filter.keynum ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<SpRPebReport>(@"[Sp_RPebReport] @StartMonth, @EndMonth,  @ParamName, @ParamValue, @KeyNum", parameters).ToList();
                    var Oldid = 0;
                    var AjuNumber = "";
                    var Nopen = "";
                    var NopenDate="";
                    var PPJK = "";
                    var ShippingMethod = "";
                    var CargoType = "";
                    var Container = "";
                    var Packages = "";
                    var Gross = "";
                    var GrossWeightUom = "";
                    var PortOfLoading = "";
                    var PortOfDestination = "";
                    var TYPEOFEXPORTNote = "";
                    var TYPEOFEXPORTType = "";
                    var PEBIncoTerms = "";
                    var PEBValuta = "";
                    var PebNpeRate = "";
                    var Eta = "";
                    var Etd = "";
                    var MasterBlAwbNumber = "";
                    var BlDate = "";
                    var Rate = "";
                    var FreightPayment = "";
                    var InsuranceAmount = "";
                    var TotalAmount = "";
                    var Balanced = "";
                    foreach (var item in data)
                    {
                        if (Oldid == item.IdCl)
                        {

                            #region All Conditions
                            if (AjuNumber == item.AjuNumber)
                                item.AjuNumber = null;
                            else
                                AjuNumber = item.AjuNumber;

                            if (Nopen == item.Nopen)
                                item.Nopen = null;
                            else
                                Nopen = item.Nopen;

                            if (TYPEOFEXPORTNote == item.TYPEOFEXPORTNote)
                                item.TYPEOFEXPORTNote = null;
                            else
                                TYPEOFEXPORTNote = item.TYPEOFEXPORTNote;

                            if (TYPEOFEXPORTType == item.TYPEOFEXPORTType)
                                item.TYPEOFEXPORTType = null;
                            else
                                TYPEOFEXPORTType = item.TYPEOFEXPORTType;

                            if (PEBIncoTerms == item.PEBIncoTerms)
                                item.PEBIncoTerms = null;
                            else
                                PEBIncoTerms = item.PEBIncoTerms;

                            if (PEBValuta == item.PEBValuta)
                                item.PEBValuta = null;
                            else
                                PEBValuta = item.PEBValuta;

                            if (Nopen == item.Nopen)
                                item.Nopen = null;
                            else
                                Nopen = item.Nopen;

                            if (NopenDate == Convert.ToString(item.NopenDate))
                                item.NopenDate = null;
                            else
                                NopenDate = Convert.ToString(item.NopenDate);

                            if (PPJK == item.PPJK)
                                item.PPJK = null;
                            else
                                PPJK = item.PPJK;

                            if (ShippingMethod == item.ShippingMethod)
                                item.ShippingMethod = null;
                            else
                                ShippingMethod = item.ShippingMethod;

                            if (CargoType == item.CargoType)
                                item.CargoType = null;
                            else
                                CargoType = item.CargoType;

                            if (Container == item.Container)
                                item.Container = null;
                            else
                                Container = item.Container;

                            if (Packages == item.Packages)
                                item.Packages = null;
                            else
                                Packages = item.Packages;

                            if (Gross == item.Gross)
                                item.Gross = null;
                            else
                                Gross = item.Gross;

                            if (GrossWeightUom == item.GrossWeightUom)
                                item.GrossWeightUom = null;
                            else
                                GrossWeightUom = item.GrossWeightUom;


                            if (PortOfLoading == item.PortOfLoading)
                                item.PortOfLoading = null;
                            else
                                PortOfLoading = item.PortOfLoading;

                            if (PortOfDestination == item.PortOfDestination)
                                item.PortOfDestination = null;
                            else
                                PortOfDestination = item.PortOfDestination;

                            if (Eta == item.Eta)
                                item.Eta = null;
                            else
                                Eta = item.Eta;

                            if (Etd == item.Etd)
                                item.Etd = null;
                            else
                                Etd = item.Etd;

                            if (MasterBlAwbNumber == item.MasterBlAwbNumber)
                                item.MasterBlAwbNumber = null;
                            else
                                MasterBlAwbNumber = item.MasterBlAwbNumber;

                            if (BlDate == item.BlDate)
                                item.BlDate = null;
                            else
                                BlDate = item.BlDate;

                            if (Rate == item.Rate)
                                item.Rate = null;
                            else
                                Rate = item.Rate;

                            if (FreightPayment == item.FreightPayment)
                                item.FreightPayment = null;
                            else
                                FreightPayment = item.FreightPayment;

                            if (InsuranceAmount == item.InsuranceAmount)
                                item.InsuranceAmount = null;
                            else
                                InsuranceAmount = item.InsuranceAmount;

                            if (TotalAmount == item.TotalAmount)
                                item.TotalAmount = null;
                            else
                                TotalAmount = item.TotalAmount;

                            if (Balanced == item.Balanced)
                                item.Balanced = null;
                            else
                                Balanced = item.Balanced;

                            if (PebNpeRate  == item.PebNpeRate)
                                item.PebNpeRate = null;
                            else
                                PebNpeRate = item.PebNpeRate;
                            #endregion
                            item.TOTALVALUEPERSHIPMENT = null;
                            item.TOTALEXPORTVALUEINIDR = null;
                        }
                        else
                        {
                            AjuNumber = item.AjuNumber;
                            Oldid = Convert.ToInt32(item.IdCl);
                            Nopen = item.Nopen;
                            TYPEOFEXPORTNote = item.TYPEOFEXPORTNote;
                            TYPEOFEXPORTType = item.TYPEOFEXPORTType;
                            PEBIncoTerms = item.PEBIncoTerms;
                            PEBValuta = item.PEBValuta;
                            NopenDate = Convert.ToString(item.NopenDate);
                            PPJK = item.PPJK;
                            ShippingMethod = item.ShippingMethod;
                            CargoType = item.CargoType;
                            Container = item.Container;
                            Packages = item.Packages;
                            Gross = item.Gross;
                            GrossWeightUom = item.GrossWeightUom;
                            PortOfLoading = item.PortOfLoading;
                            PortOfDestination = item.PortOfDestination;
                            Eta = item.Eta;
                            Etd = item.Etd;
                            MasterBlAwbNumber = item.MasterBlAwbNumber;
                            BlDate = item.BlDate;
                            Rate = item.Rate;
                            PebNpeRate = item.PebNpeRate;
                            FreightPayment = item.FreightPayment;
                            InsuranceAmount = item.InsuranceAmount;
                            TotalAmount = item.TotalAmount;
                            Balanced = item.Balanced;

                        }
                        


                    }
                    return data;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<SpRPebReport> RPebReportsList(string startMonth, string endMonth, string paramName, string paramValue, string keyNum, int istotal = 0)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartMonth", startMonth ?? ""));
                    parameterList.Add(new SqlParameter("@EndMonth", endMonth ?? ""));
                    parameterList.Add(new SqlParameter("@ParamName", paramName ?? ""));
                    parameterList.Add(new SqlParameter("@ParamValue", paramValue ?? ""));
                    parameterList.Add(new SqlParameter("@KeyNum", keyNum ?? ""));

                    //parameterList.Add(new SqlParameter("@offset", "0"));
                    //parameterList.Add(new SqlParameter("@limit", "10"));
                    //parameterList.Add(new SqlParameter("@istotal", istotal));

                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<SpRPebReport>(@"[Sp_RPebReport] @StartMonth, @EndMonth,  @ParamName, @ParamValue, @KeyNum", parameters).ToList();
                    var Oldid = 0;
                    var AjuNumber = "";
                    var Nopen = "";
                    var NopenDate = "";
                    var PPJK = "";
                    var ShippingMethod = "";
                    var CargoType = "";
                    var Container = "";
                    var Packages = "";
                    var Gross = "";
                    var GrossWeightUom = "";
                    var PortOfLoading = "";
                    var PortOfDestination = "";
                    var TYPEOFEXPORTNote = "";
                    var TYPEOFEXPORTType = "";
                    var PEBIncoTerms = "";
                    var PEBValuta = "";
                    var PebNpeRate = "";
                    var Eta = "";
                    var Etd = "";
                    var MasterBlAwbNumber = "";
                    var BlDate = "";
                    var Rate = "";
                    var FreightPayment = "";
                    var InsuranceAmount = "";
                    var TotalAmount = "";
                    var Balanced = "";
                    foreach (var item in data)
                    {
                        if (Oldid == item.IdCl)
                        {

                            #region All Conditions
                            if (AjuNumber == item.AjuNumber)
                                item.AjuNumber = null;
                            else
                                AjuNumber = item.AjuNumber;

                            if (Nopen == item.Nopen)
                                item.Nopen = null;
                            else
                                Nopen = item.Nopen;

                            if (TYPEOFEXPORTNote == item.TYPEOFEXPORTNote)
                                item.TYPEOFEXPORTNote = null;
                            else
                                TYPEOFEXPORTNote = item.TYPEOFEXPORTNote;

                            if (TYPEOFEXPORTType == item.TYPEOFEXPORTType)
                                item.TYPEOFEXPORTType = null;
                            else
                                TYPEOFEXPORTType = item.TYPEOFEXPORTType;

                            if (PEBIncoTerms == item.PEBIncoTerms)
                                item.PEBIncoTerms = null;
                            else
                                PEBIncoTerms = item.PEBIncoTerms;

                            if (PEBValuta == item.PEBValuta)
                                item.PEBValuta = null;
                            else
                                PEBValuta = item.PEBValuta;

                            if (Nopen == item.Nopen)
                                item.Nopen = null;
                            else
                                Nopen = item.Nopen;

                            if (NopenDate == Convert.ToString(item.NopenDate))
                                item.NopenDate = null;
                            else
                                NopenDate = Convert.ToString(item.NopenDate);

                            if (PPJK == item.PPJK)
                                item.PPJK = null;
                            else
                                PPJK = item.PPJK;

                            if (ShippingMethod == item.ShippingMethod)
                                item.ShippingMethod = null;
                            else
                                ShippingMethod = item.ShippingMethod;

                            if (CargoType == item.CargoType)
                                item.CargoType = null;
                            else
                                CargoType = item.CargoType;

                            if (Container == item.Container)
                                item.Container = null;
                            else
                                Container = item.Container;

                            if (Packages == item.Packages)
                                item.Packages = null;
                            else
                                Packages = item.Packages;

                            if (Gross == item.Gross)
                                item.Gross = null;
                            else
                                Gross = item.Gross;

                            if (GrossWeightUom == item.GrossWeightUom)
                                item.GrossWeightUom = null;
                            else
                                GrossWeightUom = item.GrossWeightUom;


                            if (PortOfLoading == item.PortOfLoading)
                                item.PortOfLoading = null;
                            else
                                PortOfLoading = item.PortOfLoading;

                            if (PortOfDestination == item.PortOfDestination)
                                item.PortOfDestination = null;
                            else
                                PortOfDestination = item.PortOfDestination;

                            if (Eta == item.Eta)
                                item.Eta = null;
                            else
                                Eta = item.Eta;

                            if (Etd == item.Etd)
                                item.Etd = null;
                            else
                                Etd = item.Etd;

                            if (MasterBlAwbNumber == item.MasterBlAwbNumber)
                                item.MasterBlAwbNumber = null;
                            else
                                MasterBlAwbNumber = item.MasterBlAwbNumber;

                            if (BlDate == item.BlDate)
                                item.BlDate = null;
                            else
                                BlDate = item.BlDate;

                            if (Rate == item.Rate)
                                item.Rate = null;
                            else
                                Rate = item.Rate;

                            if (FreightPayment == item.FreightPayment)
                                item.FreightPayment = null;
                            else
                                FreightPayment = item.FreightPayment;

                            if (InsuranceAmount == item.InsuranceAmount)
                                item.InsuranceAmount = null;
                            else
                                InsuranceAmount = item.InsuranceAmount;

                            if (TotalAmount == item.TotalAmount)
                                item.TotalAmount = null;
                            else
                                TotalAmount = item.TotalAmount;

                            if (Balanced == item.Balanced)
                                item.Balanced = null;
                            else
                                Balanced = item.Balanced;

                            if (PebNpeRate == item.PebNpeRate)
                                item.PebNpeRate = null;
                            else
                                PebNpeRate = item.PebNpeRate;
                            #endregion
                            item.TOTALVALUEPERSHIPMENT = null;
                            item.TOTALEXPORTVALUEINIDR = null;
                        }
                        else
                        {
                            AjuNumber = item.AjuNumber;
                            Oldid = Convert.ToInt32(item.IdCl);
                            Nopen = item.Nopen;
                            TYPEOFEXPORTNote = item.TYPEOFEXPORTNote;
                            TYPEOFEXPORTType = item.TYPEOFEXPORTType;
                            PEBIncoTerms = item.PEBIncoTerms;
                            PEBValuta = item.PEBValuta;
                            NopenDate = Convert.ToString(item.NopenDate);
                            PPJK = item.PPJK;
                            ShippingMethod = item.ShippingMethod;
                            CargoType = item.CargoType;
                            Container = item.Container;
                            Packages = item.Packages;
                            Gross = item.Gross;
                            GrossWeightUom = item.GrossWeightUom;
                            PortOfLoading = item.PortOfLoading;
                            PortOfDestination = item.PortOfDestination;
                            Eta = item.Eta;
                            Etd = item.Etd;
                            MasterBlAwbNumber = item.MasterBlAwbNumber;
                            BlDate = item.BlDate;
                            Rate = item.Rate;
                            PebNpeRate = item.PebNpeRate;
                            FreightPayment = item.FreightPayment;
                            InsuranceAmount = item.InsuranceAmount;
                            TotalAmount = item.TotalAmount;
                            Balanced = item.Balanced;

                        }



                    }
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static MemoryStream GetPebReportStream(List<SpRPebReport> data)
        {

            workbook = new XSSFWorkbook();

            //Create new Excel Sheet CIPLItem
            sheet = CreateSheetPebReport();
            ////Create a header row
            CreateHeaderRowPebReport(sheet);
            //Populate the sheet with values from the grid data
            CreateSheetDataPebReport(data);

            workbook.SetSheetName(0, "PebReport");

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;

        }

        public static ISheet CreateSheetPebReport()
        {

            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.Workbook.CreateSheet();
            sheet.SetColumnWidth(0 , 30 * 200); //MONTH                                                          MONTH   
            sheet.SetColumnWidth(1 , 30 * 200); //NO                                                             NO
            sheet.SetColumnWidth(2 , 30 * 200); //PEB                                                            PEB
            sheet.SetColumnWidth(3 , 30 * 200); //CUSTOMS BROKER(PPJK)                                           CUSTOMS BROKER(PPJK)
            sheet.SetColumnWidth(4 , 30 * 200); //SHIPMENT BY                                                    SHIPMENT BY
            sheet.SetColumnWidth(5 , 30 * 200); //LOADING TYPE                                                   LOADING TYPE
            sheet.SetColumnWidth(6 , 30 * 200); //CONTAINER                                                      CONTAINER
            sheet.SetColumnWidth(7 , 30 * 200); //PACKAGES                                                       PACKAGES
            sheet.SetColumnWidth(8 , 30 * 200); //GROSS WEIGHT                                                   GROSS WEIGHT
            sheet.SetColumnWidth(9 , 30 * 200); //TYPE OF EXPORT                                                 TYPE OF EXPORT
            sheet.SetColumnWidth(10, 30 * 200);//PORT                                                           PORT
            sheet.SetColumnWidth(11, 30 * 200);//ESTIMATED SCHEDULE                                             ESTIMATED SCHEDULE
            sheet.SetColumnWidth(12, 30 * 200);//BL/AWB                                                         BL/AWB
            sheet.SetColumnWidth(13, 30 * 200);//VALUE IN PEB                                                   VALUE IN PEB
            sheet.SetColumnWidth(14, 30 * 200);//TRAKINDO CIPL                                                  TRAKINDO CIPL
            sheet.SetColumnWidth(15, 30 * 200);//CONSIGNEE                                                      CONSIGNEE
            sheet.SetColumnWidth(16, 30 * 200);//CUSTOMER                                                       CUSTOMER
            sheet.SetColumnWidth(17, 30 * 200);//NOTE                                                           NOTE
            sheet.SetColumnWidth(18, 30 * 200);//DESCRIPTION OF GOODS                                           DESCRIPTION OF GOODS
            sheet.SetColumnWidth(19, 30 * 200);//INCOTERMS                                                      INCOTERMS
            sheet.SetColumnWidth(20, 30 * 200);//TOTAL PRICE                                                    TOTAL PRICE
            sheet.SetColumnWidth(21, 30 * 200);//FREIGHT
            sheet.SetColumnWidth(22, 30 * 200);//INSURANCE
            sheet.SetColumnWidth(23, 30 * 200);//TOTAL EXPORT VALUE                                             TOTAL EXPORT VALUE
            sheet.SetColumnWidth(24, 30 * 200);//TOTAL EXPORT VALUE IN USD                                      TOTAL EXPORT VALUE IN USD   
            sheet.SetColumnWidth(25, 30 * 200);//TOTAL VALUE PERSHIPMENT                                        TOTAL VALUE PERSHIPMENT
            sheet.SetColumnWidth(26, 30 * 200);//BALANCED                                                       BALANCED
            sheet.SetColumnWidth(27, 30 * 200);//TOTAL EXPORT VALUE IN IDR                                      TOTAL EXPORT VALUE IN IDR
            sheet.SetColumnWidth(28, 30 * 200);//SALES                                                          SALES
            sheet.SetColumnWidth(29, 30 * 200);//NON SALES                                                      NON SALES
            sheet.SetColumnWidth(30, 30 * 200);//Exporter                                                       Exporter
            sheet.SetColumnWidth(31, 30 * 200);//AJU NO.                                                        
            sheet.SetColumnWidth(32, 30 * 200);//NOPEN                                                          
            sheet.SetColumnWidth(33, 30 * 200);//NO PEN DATE                                                    
            sheet.SetColumnWidth(34, 30 * 200);//TOTAL                                                          
            sheet.SetColumnWidth(35, 30 * 200);//UOM                                                            
            sheet.SetColumnWidth(36, 30 * 200);//PERMANENT / TEMPORARY                                          
            sheet.SetColumnWidth(37, 30 * 200);//SALES / NON SALES                                              
            sheet.SetColumnWidth(38, 30 * 200);//LOADING                                                        
            sheet.SetColumnWidth(39, 30 * 200);//DESTINATION                                                    
            sheet.SetColumnWidth(40, 30 * 200);//ETD                                                            
            sheet.SetColumnWidth(41, 30 * 200);//ETA                                                            
            sheet.SetColumnWidth(42, 30 * 200);//NO.                                                            
            sheet.SetColumnWidth(43, 30 * 200);//DATE                                                           
            sheet.SetColumnWidth(44, 30 * 200);//N0. INVOICE CONSOLIDATION                                      
            sheet.SetColumnWidth(45, 30 * 200);//DATE                                                           
            sheet.SetColumnWidth(46, 30 * 200);//INCOTERMS                                                      
            sheet.SetColumnWidth(47, 30 * 200);//CURRENCY                                                       
            sheet.SetColumnWidth(48, 30 * 200);//AMMOUNT                                                        
            sheet.SetColumnWidth(49, 30 * 200);//FREIGHT                                                        
            sheet.SetColumnWidth(50, 30 * 200);//INSURANCE                                                      
            sheet.SetColumnWidth(51, 30 * 200);//TOTAL AMOUNT                                                   
            sheet.SetColumnWidth(52, 30 * 200);//NO                                                             
            sheet.SetColumnWidth(53, 30 * 200);//BRANCH                                                         
            sheet.SetColumnWidth(54, 30 * 200);//DATE                                                           
            sheet.SetColumnWidth(55, 30 * 200);//REMARKS                                                        
            sheet.SetColumnWidth(56, 30 * 200);//NAME                                                           
            sheet.SetColumnWidth(57, 30 * 200);//COUNTRY                                                        
            //sheet.SetColumnWidth(58, 30 * 200);//NAME                                                           
            //sheet.SetColumnWidth(58, 30 * 200);//NAME                                                           
            //sheet.SetColumnWidth(59, 20 * 200);//TYPES                                                          
            //sheet.SetColumnWidth(60, 20 * 200);//NAME                                                           
            //sheet.SetColumnWidth(61, 20 * 200);//COLLI                                                          
            //sheet.SetColumnWidth(62, 20 * 200);//QTY                                                            
            //sheet.SetColumnWidth(63, 20 * 200);//UOM                                                            
            //sheet.SetColumnWidth(64, 20 * 200);//WEIGHT                                                         
            //sheet.SetColumnWidth(65, 20 * 200);//UOM                                                            
            //sheet.SetColumnWidth(66, 20 * 200);//CURRENCY                                                       
            //sheet.SetColumnWidth(67, 20 * 200);//AMMOUNT                                                        
            //sheet.SetColumnWidth(68, 20 * 200);//IDR / USD                                                      



















            return sheet;
        }
        public static IRow CreateHeaderRowPebReport(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("MONTH");
            headerRow.CreateCell(1).SetCellValue("NO");
            headerRow.CreateCell(2).SetCellValue("Peb AJU NO.");
            headerRow.CreateCell(3).SetCellValue("PebNOPEN");
            headerRow.CreateCell(4).SetCellValue("Peb NO PEN DATE");
            headerRow.CreateCell(5).SetCellValue("CUSTOMS BROKER(PPJK)");
            headerRow.CreateCell(6).SetCellValue("SHIPMENT BY");
            headerRow.CreateCell(7).SetCellValue("LOADING TYPE");
            headerRow.CreateCell(8).SetCellValue("CONTAINER");
            headerRow.CreateCell(9).SetCellValue("PACKAGES");
            headerRow.CreateCell(10).SetCellValue("GROSS WEIGHT TOTAL");
            headerRow.CreateCell(11).SetCellValue("GROSS WEIGHT UOM");
            headerRow.CreateCell(12).SetCellValue("TYPE OF EXPORT PERMANENT / TEMPORARY");
            headerRow.CreateCell(13).SetCellValue("TYPE OF EXPORT SALES / NON SALES");
            headerRow.CreateCell(14).SetCellValue("PORT LOADING");
            headerRow.CreateCell(15).SetCellValue("PORT DESTINATION");
            headerRow.CreateCell(16).SetCellValue("ESTIMATED SCHEDULE ETD");
            headerRow.CreateCell(17).SetCellValue("ESTIMATED SCHEDULE ETA");
            headerRow.CreateCell(18).SetCellValue("BL/AWB NO.");
            headerRow.CreateCell(19).SetCellValue("BL/AWB DATE");
            headerRow.CreateCell(20).SetCellValue("VALUE IN PEB N0. INVOICE CONSOLIDATION");
            headerRow.CreateCell(21).SetCellValue("VALUE IN PEB DATE");
            headerRow.CreateCell(22).SetCellValue("VALUE IN PEB INCOTERMS");
            headerRow.CreateCell(23).SetCellValue("VALUE IN PEB CURRENCY");
            headerRow.CreateCell(24).SetCellValue("VALUE IN PEB AMMOUNT");
            headerRow.CreateCell(25).SetCellValue("VALUE IN PEB FREIGHT");
            headerRow.CreateCell(26).SetCellValue("VALUE IN PEB INSURANCE");
            headerRow.CreateCell(27).SetCellValue("VALUE IN PEB TOTAL AMOUNT");
            headerRow.CreateCell(28).SetCellValue("TRAKINDO CIPL NO");
            headerRow.CreateCell(29).SetCellValue("TRAKINDO CIPL BRANCH");
            headerRow.CreateCell(30).SetCellValue("TRAKINDO CIPL DATE");
            headerRow.CreateCell(31).SetCellValue("TRAKINDO CIPL REMARKS");
            headerRow.CreateCell(32).SetCellValue("CONSIGNEE NAME");
            headerRow.CreateCell(33).SetCellValue("CONSIGNEE COUNTRY");
            headerRow.CreateCell(34).SetCellValue("CUSTOMER NAME");
            headerRow.CreateCell(35).SetCellValue("CUSTOMER COUNTRY");
            headerRow.CreateCell(36).SetCellValue("NOTE");
            headerRow.CreateCell(37).SetCellValue("DESCRIPTION OF GOODS TYPES");
            headerRow.CreateCell(38).SetCellValue("DESCRIPTION OF GOODS NAME");
            headerRow.CreateCell(39).SetCellValue("DESCRIPTION OF GOODS COLLI");
            headerRow.CreateCell(40).SetCellValue("DESCRIPTION OF GOODS QTY");
            headerRow.CreateCell(41).SetCellValue("DESCRIPTION OF GOODS UOM");
            headerRow.CreateCell(42).SetCellValue("DESCRIPTION OF GOODS WEIGHT");
            headerRow.CreateCell(43).SetCellValue("DESCRIPTION OF GOODS UOM");
            headerRow.CreateCell(44).SetCellValue("INCOTERMS");
            headerRow.CreateCell(45).SetCellValue("TOTAL PRICE CURRENCY");
            headerRow.CreateCell(46).SetCellValue("TOTAL PRICE AMMOUNT");
            headerRow.CreateCell(47).SetCellValue("FREIGHT");
            headerRow.CreateCell(48).SetCellValue("Insurance");
            headerRow.CreateCell(49).SetCellValue("TOTAL EXPORT VALUE");
            headerRow.CreateCell(50).SetCellValue("EXCHANGE RATE IDR / USD");
            headerRow.CreateCell(51).SetCellValue("TOTAL EXPORT VALUE IN USD");
            headerRow.CreateCell(52).SetCellValue("TOTAL VALUE PERSHIPMENT");
            headerRow.CreateCell(53).SetCellValue("BALANCED");
            headerRow.CreateCell(54).SetCellValue("TOTAL EXPORT VALUE IN IDR");
            headerRow.CreateCell(55).SetCellValue("SALES");
            headerRow.CreateCell(56).SetCellValue("NON SALES");
            headerRow.CreateCell(57).SetCellValue("Exporter");
                                  









            return headerRow;
        }

        public static void CreateSheetDataPebReport(List<SpRPebReport> tbl)
        {

            int rowNumber = 1;

            foreach (var data in tbl)
            {

                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.PebMonth);                                                                          
                row.CreateCell(1).SetCellValue(data.RowNumber);                                                                         
                row.CreateCell(2).SetCellValue(data.AjuNumber);                                                                         
                row.CreateCell(3).SetCellValue(data.Nopen);                                                                             
                row.CreateCell(4).SetCellValue(Convert.ToString(data.NopenDate));                                                       
                row.CreateCell(5).SetCellValue(data.PPJK);                                                                              
                row.CreateCell(6).SetCellValue(data.ShippingMethod);                                                                    
                row.CreateCell(7).SetCellValue(data.CargoType);                                                                         
                row.CreateCell(8).SetCellValue(data.Container);                                                                         
                row.CreateCell(9).SetCellValue(data.Packages);                                                                         
                row.CreateCell(10).SetCellValue(data.Gross);                                                                            
                row.CreateCell(11).SetCellValue(data.GrossWeightUom);                                                                   
                row.CreateCell(12).SetCellValue(data.TYPEOFEXPORTType);                                                                             
                row.CreateCell(13).SetCellValue(data.TYPEOFEXPORTNote);                                                                             
                row.CreateCell(14).SetCellValue(data.PortOfLoading);                                                                    
                row.CreateCell(15).SetCellValue(data.PortOfDestination);                                                                
                row.CreateCell(16).SetCellValue(data.Etd);                                                                              
                row.CreateCell(17).SetCellValue(data.Eta);                                                                              
                row.CreateCell(18).SetCellValue(data.MasterBlAwbNumber);                                                                
                row.CreateCell(19).SetCellValue(data.BlDate);                                                                           
                row.CreateCell(20).SetCellValue("-");                                                                                   
                row.CreateCell(21).SetCellValue("-");                                                                                   
                row.CreateCell(22).SetCellValue(data.PEBIncoTerms);                                                                        
                row.CreateCell(23).SetCellValue(data.PEBValuta);                                                                           
                row.CreateCell(24).SetCellValue(data.Rate);                                                                             
                row.CreateCell(25).SetCellValue(data.FreightPayment);                                                                   
                row.CreateCell(26).SetCellValue(data.InsuranceAmount);                                                                  
                row.CreateCell(27).SetCellValue(data.TotalAmount);                                                                    
                row.CreateCell(28).SetCellValue(data.CiplNo);                                                                           
                row.CreateCell(29).SetCellValue(data.Branch);                                                                           
                row.CreateCell(30).SetCellValue(data.CiplDate);                                                                         
                row.CreateCell(31).SetCellValue(data.Remarks);                                                                          
                row.CreateCell(32).SetCellValue(data.ConsigneeName);                                                                    
                row.CreateCell(33).SetCellValue(data.ConsigneeCountry);                                                                 
                row.CreateCell(34).SetCellValue(data.ConsigneeName);                                                                    
                row.CreateCell(35).SetCellValue(data.ConsigneeCountry);                                                                 
                row.CreateCell(36).SetCellValue(data.Note);                                                                             
                row.CreateCell(37).SetCellValue(data.Type);                                                                                   
                row.CreateCell(38).SetCellValue(data.CategoryName);                                                                     
                row.CreateCell(39).SetCellValue(data.Colli);                                                                            
                row.CreateCell(40).SetCellValue(data.CiplQty);                                                                          
                row.CreateCell(41).SetCellValue(data.CiplUOM);                                                                          
                row.CreateCell(42).SetCellValue(data.CiplWeight);                                                                       
                row.CreateCell(43).SetCellValue(data.GoodsUom);                                                                                 
                row.CreateCell(44).SetCellValue(data.IncoTerms);                                                                        
                row.CreateCell(45).SetCellValue(data.Valuta);                                                                           
                row.CreateCell(46).SetCellValue(data.Ammount);                                                                           
                row.CreateCell(47).SetCellValue(data.FreightPayment);                                                                   
                row.CreateCell(48).SetCellValue(data.InsuranceAmount);                                                                  
                row.CreateCell(49).SetCellValue(data.TOTALEXPORTVALUE);                                                                 
                row.CreateCell(50).SetCellValue(data.PebNpeRate);                                                                       
                row.CreateCell(51).SetCellValue(data.TotalExportValueInUsd);                                                                          
                row.CreateCell(52).SetCellValue(data.TOTALVALUEPERSHIPMENT);                                                            
                row.CreateCell(53).SetCellValue(data.Balanced);                                                                         
                row.CreateCell(54).SetCellValue(data.TOTALEXPORTVALUEINIDR);                                                            
                row.CreateCell(55).SetCellValue(data.Sales);                                                                            
                row.CreateCell(56).SetCellValue(data.NonSales);                                                                         
                row.CreateCell(57).SetCellValue("PT. Trakindo Utama");                                                                  




            }
        }

    }
}
