using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRDetailsTracking
    {
        public const string CacheName = "App.EMCS.SvcRDetailsTracking";
        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        public static ISheet sheet;
        public static XSSFWorkbook workbook = new XSSFWorkbook();
        public static List<SpRDetailsTracking> DetailsTrackingList_old(DetailTrackingListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@StartMonth", filter.startMonth ?? ""));
                parameterList.Add(new SqlParameter("@EndMonth", filter.endMonth ?? ""));
                parameterList.Add(new SqlParameter("@ParamName", filter.paramName ?? ""));
                parameterList.Add(new SqlParameter("@ParamValue", filter.paramValue ?? ""));
                parameterList.Add(new SqlParameter("@KeyNum", filter.keynum ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<SpRDetailsTracking>(@"[dbo].[SP_RDetailsTracking] @StartMonth, @EndMonth,  @ParamName, @ParamValue, @KeyNum", parameters).ToList();
                return data;
            }
        }

        public static List<SpRDetailsTracking> DetailsTrackingList(string startMonth, string endMonth, string paramName, string paramValue, string keyNum)
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

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRDetailsTracking>("exec [dbo].[SP_RDetailsTracking]  @StartMonth, @EndMonth,  @ParamName, @ParamValue, @KeyNum", parameters).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Detail Tracking        
        public static MemoryStream GetDetailsTrackingStream(List<SpRDetailsTracking> data)
        {
            
            workbook = new XSSFWorkbook();
            
            //Create new Excel Sheet CIPLItem
            sheet = CreateSheetDetailTracking();
            ////Create a header row
            CreateHeaderRowDetailTracking(sheet);
            //Populate the sheet with values from the grid data
            CreateSheetDataDetailTracking(data);

            workbook.SetSheetName(0, "DetailTracking");

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);
            return output;

        }
     
        public static ISheet CreateSheetDetailTracking()
        {
            
            var sheet = workbook.CreateSheet();          

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//PebMonth           
            sheet.SetColumnWidth(1, 20 * 200);//ReferenceNo
            sheet.SetColumnWidth(2, 20 * 200);//CiplNo
            sheet.SetColumnWidth(3, 20 * 200);//CiplCreateDate
            sheet.SetColumnWidth(4, 20 * 200);//CiplApprovalDate  
            sheet.SetColumnWidth(5, 20 * 200);//DescGoods
            sheet.SetColumnWidth(6, 20 * 150);//Category
            sheet.SetColumnWidth(7, 20 * 150);//SubCategory
            sheet.SetColumnWidth(8, 20 * 150);//PermanentTemporary
            sheet.SetColumnWidth(9, 20 * 200);//SalesNonSales
            sheet.SetColumnWidth(10, 20 * 200);//Type
            sheet.SetColumnWidth(11, 20 * 200);//Remarks
            sheet.SetColumnWidth(12, 20 * 200);//PicName
            sheet.SetColumnWidth(13, 20 * 200);//PicApproverName
            sheet.SetColumnWidth(14, 20 * 200);
            sheet.SetColumnWidth(15, 20 * 200);
            sheet.SetColumnWidth(16, 20 * 200);//EdiNo
            sheet.SetColumnWidth(17, 20 * 200);//EdiDate    
            sheet.SetColumnWidth(18, 20 * 200);//RgNo     
            sheet.SetColumnWidth(19, 20 * 200);//RgDate
            sheet.SetColumnWidth(20, 20 * 200);//RgApproverName
            sheet.SetColumnWidth(21, 20 * 200);//RgApprovalDate
            sheet.SetColumnWidth(22, 20 * 200);//ClNo
            sheet.SetColumnWidth(23, 20 * 200);//ClDate
            sheet.SetColumnWidth(24, 20 * 200);//ClApproverName     
            sheet.SetColumnWidth(25, 20 * 200);//ClApprovalDate
            sheet.SetColumnWidth(26, 20 * 150);//SsNo
            sheet.SetColumnWidth(27, 20 * 150);//SsDate
            sheet.SetColumnWidth(28, 20 * 150);//SiNo
            sheet.SetColumnWidth(29, 20 * 200);//SiDate
            sheet.SetColumnWidth(30, 20 * 200);//OtherName
            sheet.SetColumnWidth(31, 20 * 200);//OtherNumber
            sheet.SetColumnWidth(32, 20 * 200);//OtherDate
            sheet.SetColumnWidth(33, 20 * 200);//Etd
            sheet.SetColumnWidth(34, 20 * 200);//Eta
            sheet.SetColumnWidth(35, 20 * 200);//AjuNumber     
            sheet.SetColumnWidth(36, 20 * 200);//AjuDate
            sheet.SetColumnWidth(37, 20 * 200);//NpeNumber      
            sheet.SetColumnWidth(38, 20 * 200);//Nopen    
            sheet.SetColumnWidth(39, 20 * 200);//NopenDate
            sheet.SetColumnWidth(40, 20 * 200);//PebApproverName 
            sheet.SetColumnWidth(41, 20 * 200);//PebApprovalDate
            sheet.SetColumnWidth(42, 20 * 200);//MasterBlAwbNumber
            sheet.SetColumnWidth(43, 20 * 200);//MasterBlAwbDate
            sheet.SetColumnWidth(44, 20 * 200);//HouseBlAwbNumber
            sheet.SetColumnWidth(45, 20 * 200);//HouseBlAwbDate
            sheet.SetColumnWidth(46, 20 * 200);//Liner 
            sheet.SetColumnWidth(47, 20 * 200);//VesselName               
            sheet.SetColumnWidth(48, 20 * 200);//VesselVoyNo
            sheet.SetColumnWidth(49, 20 * 200);//Liner  
            sheet.SetColumnWidth(50, 20 * 200);//FlightName
            sheet.SetColumnWidth(51, 20 * 200);//FlightVoyNo 
            sheet.SetColumnWidth(52, 20 * 200);//ConsigneeName           
            sheet.SetColumnWidth(53, 20 * 200);//ConsigneeAddress
            sheet.SetColumnWidth(54, 20 * 200);//ConsigneeCountry
            sheet.SetColumnWidth(55, 20 * 200);//ConsigneePic
            sheet.SetColumnWidth(56, 20 * 200);//ConsigneeEmail     
            sheet.SetColumnWidth(57, 20 * 200);//ConsigneeTelephone
            sheet.SetColumnWidth(58, 20 * 150);//ConsigneeFax
            sheet.SetColumnWidth(59, 20 * 150);//NotifyName
            sheet.SetColumnWidth(60, 20 * 150);//NotifyAddress
            sheet.SetColumnWidth(61, 20 * 200);//NotifyCountry
            sheet.SetColumnWidth(62, 20 * 200);//NotifyPic
            sheet.SetColumnWidth(63, 20 * 200);//NotifyEmail
            sheet.SetColumnWidth(64, 20 * 200);//NotifyTelephone
            sheet.SetColumnWidth(65, 20 * 200);//NotifyFax
            sheet.SetColumnWidth(66, 20 * 200);//SoldToName
            sheet.SetColumnWidth(67, 20 * 200);//SoldToAddress    
            sheet.SetColumnWidth(68, 20 * 200);//SoldToCountry
            sheet.SetColumnWidth(69, 20 * 200);//SoldToPic     
            sheet.SetColumnWidth(70, 20 * 200);//SoldToEmail    
            sheet.SetColumnWidth(71, 20 * 200);//SoldToTelephone
            sheet.SetColumnWidth(72, 20 * 200);//SoldToFax 
            sheet.SetColumnWidth(73, 20 * 200);//PortOfDestination
            sheet.SetColumnWidth(74, 20 * 200);//ShippingMethod
            sheet.SetColumnWidth(75, 20 * 200);//IncoTerms
            sheet.SetColumnWidth(76, 20 * 200);//CargoType     
            sheet.SetColumnWidth(77, 20 * 200);//""
            sheet.SetColumnWidth(78, 20 * 150);//Seal
            sheet.SetColumnWidth(79, 20 * 150);//Type
            sheet.SetColumnWidth(80, 20 * 150);//""
            sheet.SetColumnWidth(81, 20 * 200);//QuantityUom
            sheet.SetColumnWidth(82, 20 * 200);//""
            sheet.SetColumnWidth(83, 20 * 200);//""
            sheet.SetColumnWidth(84, 20 * 200);//""
            sheet.SetColumnWidth(85, 20 * 200);//""
            sheet.SetColumnWidth(86, 20 * 200);//"
            sheet.SetColumnWidth(87, 20 * 200);//PebFob     
            sheet.SetColumnWidth(88, 20 * 200);//FreightPyment
            sheet.SetColumnWidth(89, 20 * 200);//InsuranceAmount     
            sheet.SetColumnWidth(90, 20 * 200);//""     
            sheet.SetColumnWidth(91, 20 * 200);//TotalPeb
            sheet.SetColumnWidth(92, 20 * 200);//InvoiceNoServiceCharges 
            sheet.SetColumnWidth(93, 20 * 200);//CurrencyServiceCharges
            sheet.SetColumnWidth(94, 20 * 200);//TotalServiceCharges
            sheet.SetColumnWidth(95, 20 * 200);//InvoiceNoConsignee
            sheet.SetColumnWidth(96, 20 * 200);//CurrencyValueConsignee
            sheet.SetColumnWidth(97, 20 * 200);//TotalValueConsignee    
            sheet.SetColumnWidth(98, 20 * 200);//ValueBalanceConsignee  
            sheet.SetColumnWidth(99, 20 * 200);//Status               
            sheet.SetColumnWidth(100, 20 * 200);//ValueBalanceConsignee  
            sheet.SetColumnWidth(101, 20 * 200);//Status  
            return sheet;
        }
        public static IRow CreateHeaderRowDetailTracking(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("MONTH");            
            headerRow.CreateCell(1).SetCellValue("REFERENCE NO.");
            headerRow.CreateCell(2).SetCellValue("CIPL NO");
            headerRow.CreateCell(3).SetCellValue("CIPL DATE");
            headerRow.CreateCell(4).SetCellValue("CIPL APPROVE DATE");
            headerRow.CreateCell(5).SetCellValue("DESCRPTION OF GOODS");
            headerRow.CreateCell(6).SetCellValue("CATEGORY (EXPORT CATEGORY)");
            headerRow.CreateCell(7).SetCellValue("SUB (EXPORT CATEGORY)");
            headerRow.CreateCell(8).SetCellValue("PERMANENT/TEMPORARY (EXPORT TYPE)");
            headerRow.CreateCell(9).SetCellValue("SALES/NON SALES (EXPORT TYPE)");            
            headerRow.CreateCell(10).SetCellValue("PE/RR/R/E/- (EXPORT TYPE)");
            headerRow.CreateCell(11).SetCellValue("REMARKS");
            headerRow.CreateCell(12).SetCellValue("PIC NAME");
            headerRow.CreateCell(13).SetCellValue("PIC APPROVER NAME");
            headerRow.CreateCell(14).SetCellValue("PIC DEPARTMENT");
            headerRow.CreateCell(15).SetCellValue("PIC BRANCH");
            headerRow.CreateCell(16).SetCellValue("EDI NO");
            headerRow.CreateCell(17).SetCellValue("EDI DATE");
            headerRow.CreateCell(18).SetCellValue("RG NO");
            headerRow.CreateCell(19).SetCellValue("RG DATE");
            headerRow.CreateCell(20).SetCellValue("RG APPROVER NAME");
            headerRow.CreateCell(21).SetCellValue("RG APPROVAL DATE");
            headerRow.CreateCell(22).SetCellValue("CL NO");
            headerRow.CreateCell(23).SetCellValue("CL DATE");
            headerRow.CreateCell(24).SetCellValue("CL APPROVER NAME");
            headerRow.CreateCell(25).SetCellValue("CR APPROVAL DATE");
            headerRow.CreateCell(26).SetCellValue("SS NO");
            headerRow.CreateCell(27).SetCellValue("SS DATE");
            headerRow.CreateCell(28).SetCellValue("SI NO");
            headerRow.CreateCell(29).SetCellValue("SI DATE");
            headerRow.CreateCell(30).SetCellValue("OTHER NAME");
            headerRow.CreateCell(31).SetCellValue("OTHER NUMBER");
            headerRow.CreateCell(32).SetCellValue("OTHER DATE");            
            headerRow.CreateCell(33).SetCellValue("ESTIMATED ETD");
            headerRow.CreateCell(34).SetCellValue("ESTIMATED ETA");
            headerRow.CreateCell(35).SetCellValue("PEB AJU NO");
            headerRow.CreateCell(36).SetCellValue("PEB AJU DATE");
            headerRow.CreateCell(37).SetCellValue("PEB NPE NO");
            headerRow.CreateCell(38).SetCellValue("PEB NOPEN");
            headerRow.CreateCell(39).SetCellValue("PEB NOPEN DATE");
            headerRow.CreateCell(40).SetCellValue("PEB APPROVER NAME");
            headerRow.CreateCell(41).SetCellValue("PEB APPRVAL DATE");
            headerRow.CreateCell(42).SetCellValue("MASTER BL/AWB NO");
            headerRow.CreateCell(43).SetCellValue("MASTER BL/AWB DATE");
            headerRow.CreateCell(44).SetCellValue("HOUSE BL/AWB NO");
            headerRow.CreateCell(45).SetCellValue("HOUSER BL/AWB DATE");
            headerRow.CreateCell(46).SetCellValue("LINER OCEAN NAME ");
            headerRow.CreateCell(47).SetCellValue("LINER OCEAN VESSEL");
            headerRow.CreateCell(48).SetCellValue("LINER OCEAN SAILING/VOYAGE NUMBER");
            headerRow.CreateCell(49).SetCellValue("LINER AIR NAME");
            headerRow.CreateCell(50).SetCellValue("LINER AIR FLIGHT ");
            headerRow.CreateCell(51).SetCellValue("LINER AIR FLIGHT NUMBER");
            headerRow.CreateCell(52).SetCellValue("CONSIGNEE NAME");
            headerRow.CreateCell(53).SetCellValue("CONSIGNEE ADDRESS");
            headerRow.CreateCell(54).SetCellValue("CONSIGNEE COUNTRY ");
            headerRow.CreateCell(55).SetCellValue("CONSIGNEE PIC NAME");
            headerRow.CreateCell(56).SetCellValue("CONSIGNEE EMAIL");
            headerRow.CreateCell(57).SetCellValue("CONSIGNEE PHONE");
            headerRow.CreateCell(58).SetCellValue("CONSIGNEE FAX NUMBER");
            headerRow.CreateCell(59).SetCellValue("NOTIFY PARTY NAME");
            headerRow.CreateCell(60).SetCellValue("NOTIFY PARTY ADDRESS");
            headerRow.CreateCell(61).SetCellValue("NOTIFY PARTY COUNTRY");
            headerRow.CreateCell(62).SetCellValue("NOTIFY PARTY PIC NAME");
            headerRow.CreateCell(63).SetCellValue("NOTIFY PARTY EMAIL");
            headerRow.CreateCell(64).SetCellValue("NOTIFY PARTY PHONE NUMBER");
            headerRow.CreateCell(65).SetCellValue("NOTIFY PARTY FAX NUMBER");
            headerRow.CreateCell(66).SetCellValue("SHIP TO/DELIVERY TO NAME");
            headerRow.CreateCell(67).SetCellValue("SHIP TO/DELIVERY TO ADDRESS");
            headerRow.CreateCell(68).SetCellValue("SHIP TO/DELIVERY TO COUNTRY");
            headerRow.CreateCell(69).SetCellValue("SHIP TO/DELIVERY TO PIC NAME");
            headerRow.CreateCell(70).SetCellValue("SHIP TO/DELIVERY TO EMAIL");
            headerRow.CreateCell(71).SetCellValue("SHIP TO/DELIVERY TO PHONE NUMBER");
            headerRow.CreateCell(72).SetCellValue("SHIP TO/DELIVERY TO FAX NUMBER");
            headerRow.CreateCell(73).SetCellValue("PORT OF LOADING");
            headerRow.CreateCell(74).SetCellValue("PORT OF DISCHARGE");
            headerRow.CreateCell(75).SetCellValue("SHIPPING METHOD");
            headerRow.CreateCell(76).SetCellValue("SHIPPING TERMS");
            headerRow.CreateCell(77).SetCellValue("CARGO TYPE");
            headerRow.CreateCell(78).SetCellValue("FCL CNTR#");
            headerRow.CreateCell(79).SetCellValue("FCL SEAL#");
            headerRow.CreateCell(80).SetCellValue("FCL TYPE");
            headerRow.CreateCell(81).SetCellValue("QUANITY TOTAL");
            headerRow.CreateCell(82).SetCellValue("QUANITY UOM");
            headerRow.CreateCell(83).SetCellValue("COLLY");
            headerRow.CreateCell(84).SetCellValue("WEIGHT GROSS");
            headerRow.CreateCell(85).SetCellValue("WEIGHT NETT");
            headerRow.CreateCell(86).SetCellValue("TOTAL VOLUME IN M3");
            headerRow.CreateCell(87).SetCellValue("CURRENCY");
            headerRow.CreateCell(88).SetCellValue("EXCHANGE RATE");
            headerRow.CreateCell(89).SetCellValue("EXPORT VALUE");
            headerRow.CreateCell(90).SetCellValue("EXPORT FREIGHT COST");
            headerRow.CreateCell(91).SetCellValue("EXPORT INSURANCE");
            headerRow.CreateCell(92).SetCellValue("EXPORT TOTAL (IN USD) BASED ON INVOICE");
            headerRow.CreateCell(93).SetCellValue("EXPORT TOTAL (IN USD) BASED ON PEB");
            headerRow.CreateCell(94).SetCellValue("EXPORT SERVICE CHARGES - INVOICE NUMBER");
            headerRow.CreateCell(95).SetCellValue("EXPORT SERVICE CHARGES - CURRENCY");
            headerRow.CreateCell(96).SetCellValue("EXPORT SERVICE CHARGES - TOTAL SERVICE CHARGES");
            headerRow.CreateCell(97).SetCellValue("VALUE RECEIVED FROM CONSIGNEE - INVOICE NUMER");
            headerRow.CreateCell(98).SetCellValue("VALUE RECEIVED FROM CONSIGNEE - CURRENCY");
            headerRow.CreateCell(99).SetCellValue("VALUE RECEIVED FROM CONSIGNEE - TOTAL VALUE");
            headerRow.CreateCell(100).SetCellValue("VALUE RECEIVED FROM CONSIGNEE - BALANCE");
            headerRow.CreateCell(101).SetCellValue("STATUS");
            return headerRow;
        }
      
        public static void CreateSheetDataDetailTracking(List<SpRDetailsTracking> tbl)
        {

            int rowNumber = 1;
            
            foreach (var data in tbl)
            {
               
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.PebMonth);
                row.CreateCell(1).SetCellValue(data.ReferenceNo);
                row.CreateCell(2).SetCellValue(data.CiplNo);
                row.CreateCell(3).SetCellValue(data.CiplCreateDate);
                row.CreateCell(4).SetCellValue(data.CiplApprovalDate);
                row.CreateCell(5).SetCellValue(data.DescGoods);
                row.CreateCell(6).SetCellValue(data.Category);
                row.CreateCell(7).SetCellValue(data.SubCategory);
                row.CreateCell(8).SetCellValue(data.PermanentTemporary);
                row.CreateCell(9).SetCellValue(data.SalesNonSales);
                row.CreateCell(10).SetCellValue(data.Type);
                row.CreateCell(11).SetCellValue(data.Remarks);
                row.CreateCell(12).SetCellValue(data.PicName);
                row.CreateCell(13).SetCellValue(data.PicApproverName);
                row.CreateCell(14).SetCellValue("-");
                row.CreateCell(15).SetCellValue("-");
                row.CreateCell(16).SetCellValue(data.EdiNo);
                row.CreateCell(17).SetCellValue(data.EdiDate);
                row.CreateCell(18).SetCellValue(data.RgNo);
                row.CreateCell(19).SetCellValue(data.RgDate);
                row.CreateCell(20).SetCellValue(data.RgApproverName);
                row.CreateCell(21).SetCellValue(data.RgApprovalDate);
                row.CreateCell(22).SetCellValue(data.ClNo);
                row.CreateCell(23).SetCellValue(data.ClDate);
                row.CreateCell(24).SetCellValue(data.ClApproverName);
                row.CreateCell(25).SetCellValue(data.ClApprovalDate);
                row.CreateCell(26).SetCellValue(data.SsNo);
                row.CreateCell(27).SetCellValue(data.SsDate);
                row.CreateCell(28).SetCellValue(data.SiNo);
                row.CreateCell(29).SetCellValue(data.SiDate);
                row.CreateCell(30).SetCellValue(data.OtherName);
                row.CreateCell(31).SetCellValue(data.OtherNumber);
                row.CreateCell(32).SetCellValue(data.OtherDate);
                row.CreateCell(33).SetCellValue(data.Etd);
                row.CreateCell(34).SetCellValue(data.Eta);
                row.CreateCell(35).SetCellValue(data.AjuNumber);
                row.CreateCell(36).SetCellValue(data.AjuDate);
                row.CreateCell(37).SetCellValue(data.NpeNumber);
                row.CreateCell(38).SetCellValue(data.Nopen);
                row.CreateCell(39).SetCellValue(data.NopenDate);
                row.CreateCell(40).SetCellValue(data.PebApproverName);
                row.CreateCell(41).SetCellValue(data.PebApprovalDate);
                row.CreateCell(42).SetCellValue(data.MasterBlAwbNumber);
                row.CreateCell(43).SetCellValue(data.MasterBlAwbDate);
                row.CreateCell(44).SetCellValue(data.HouseBlAwbNumber);
                row.CreateCell(45).SetCellValue(data.HouseBlAwbDate);
                row.CreateCell(46).SetCellValue(data.Liner);
                row.CreateCell(47).SetCellValue(data.VesselName);
                row.CreateCell(48).SetCellValue(data.VesselVoyNo);
                row.CreateCell(49).SetCellValue(data.Liner);
                row.CreateCell(50).SetCellValue(data.FlightName);
                row.CreateCell(51).SetCellValue(data.FlightVoyNo);
                row.CreateCell(52).SetCellValue(data.ConsigneeName);
                row.CreateCell(53).SetCellValue(data.ConsigneeAddress);
                row.CreateCell(54).SetCellValue(data.ConsigneeCountry);
                row.CreateCell(55).SetCellValue(data.ConsigneePic);            
                row.CreateCell(56).SetCellValue(data.ConsigneeEmail);
                row.CreateCell(57).SetCellValue(data.ConsigneeTelephone);
                row.CreateCell(58).SetCellValue(data.ConsigneeFax);
                row.CreateCell(59).SetCellValue(data.NotifyName);
                row.CreateCell(60).SetCellValue(data.NotifyAddress);
                row.CreateCell(61).SetCellValue(data.NotifyCountry);
                row.CreateCell(62).SetCellValue(data.NotifyPic);
                row.CreateCell(63).SetCellValue(data.NotifyEmail);
                row.CreateCell(64).SetCellValue(data.NotifyTelephone);
                row.CreateCell(65).SetCellValue(data.NotifyFax);
                row.CreateCell(66).SetCellValue(data.SoldToName);
                row.CreateCell(67).SetCellValue(data.SoldToAddress);
                row.CreateCell(68).SetCellValue(data.SoldToCountry);
                row.CreateCell(69).SetCellValue(data.SoldToPic);
                row.CreateCell(70).SetCellValue(data.SoldToEmail);
                row.CreateCell(71).SetCellValue(data.SoldToTelephone);
                row.CreateCell(72).SetCellValue(data.SoldToFax);
                row.CreateCell(73).SetCellValue(data.PortOfLoading); 
                row.CreateCell(74).SetCellValue(data.PortOfDestination);
                row.CreateCell(75).SetCellValue(data.ShippingMethod);
                row.CreateCell(76).SetCellValue(data.IncoTerms);
                row.CreateCell(77).SetCellValue(data.CargoType);
                row.CreateCell(78).SetCellValue("");
                row.CreateCell(79).SetCellValue(data.Seal);
                row.CreateCell(80).SetCellValue(data.Type);
                row.CreateCell(81).SetCellValue("");
                row.CreateCell(82).SetCellValue(data.QuantityUom);
                row.CreateCell(83).SetCellValue("");
                row.CreateCell(84).SetCellValue("");
                row.CreateCell(85).SetCellValue("");
                row.CreateCell(86).SetCellValue("");
                row.CreateCell(87).SetCellValue("");
                row.CreateCell(88).SetCellValue("");
                row.CreateCell(89).SetCellValue(data.PebFob);
                row.CreateCell(90).SetCellValue(data.FreightPyment);
                row.CreateCell(91).SetCellValue(data.InsuranceAmount);
                row.CreateCell(92).SetCellValue("");
                row.CreateCell(93).SetCellValue(data.TotalPeb);
                row.CreateCell(94).SetCellValue(data.InvoiceNoServiceCharges);
                row.CreateCell(95).SetCellValue(data.CurrencyServiceCharges);
                row.CreateCell(96).SetCellValue(data.TotalServiceCharges);
                row.CreateCell(97).SetCellValue(data.InvoiceNoConsignee);
                row.CreateCell(98).SetCellValue(data.CurrencyValueConsignee);
                row.CreateCell(99).SetCellValue(data.TotalValueConsignee);
                row.CreateCell(100).SetCellValue(data.ValueBalanceConsignee);
                row.CreateCell(101).SetCellValue(data.Status);
            }
        }
        #endregion
    }
}
