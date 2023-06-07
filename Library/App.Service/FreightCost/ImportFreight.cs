using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.IO;
using System.Data;

namespace App.Service.FreightCost
{
    public class ImportFreight
    {
        public ALLsheet getAllSheet(string FilePath)
        {
            string extension = Path.GetExtension(FilePath);
            XSSFWorkbook xssfwb;
            ALLsheet sheets = new ALLsheet();

            try
            {
                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheets.sheetRate = xssfwb.GetSheet("Rate");
                        sheets.sheetInboundRate = xssfwb.GetSheet("Inbound Rate");
                        sheets.sheetSurcharge = xssfwb.GetSheet("Surcharge");
                        sheets.sheetTruckRate = xssfwb.GetSheet("Trucking Rate");
                    }
                    else
                        throw new Exception("File extension is not valid.");
                }

                if (sheets.sheetRate == null)
                    throw new Exception("No Data Rate.");
                else if (sheets.sheetInboundRate == null)
                    throw new Exception("No Data Inbound Rate.");
                else if (sheets.sheetSurcharge == null)
                    throw new Exception("No Data Surcharge.");
                else if (sheets.sheetTruckRate == null)
                    throw new Exception("No Data Trucking Rate.");

            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read Sheet. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read Sheet. Error Message: " + ex.InnerException.Message);
            }

            return sheets;
        }

        public static string CheckHeaderExcel(ISheet sheet, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            if (Modul == "Rate")
                sb = SheetHeaderRate(sheet, Modul);
            else if (Modul == "Inbound Rate")
                sb = SheetHeaderInboundRate(sheet, Modul);
            else if (Modul == "Surcharge")
                sb = SheetHeaderSurcharge(sheet, Modul);
            else if (Modul == "Trucking Rate")
                sb = SheetHeaderTruckRate(sheet, Modul);


            return sb.ToString();
        }

        public static Decimal getValue(ISheet sheet, int row, int collumn)
        {
            decimal _Value = 0;
            try
            {
                if (sheet.GetRow(row).GetCell(collumn) != null)
                {
                    if (sheet.GetRow(row).GetCell(collumn).CellType == CellType.Numeric)
                        _Value = Convert.ToDecimal(sheet.GetRow(row).GetCell(collumn).NumericCellValue);
                    else
                        _Value = Convert.ToDecimal(sheet.GetRow(row).GetCell(collumn).StringCellValue);
                }
                else
                    throw new Exception("Value at row " + (row + 1) + " cannot empty");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data Value at row " + (row + 1) + ". Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data Value at row " + (row + 1) + ". Error Message: " + ex.InnerException.Message);
            }
            return _Value;
        }

        private static StringBuilder SheetHeaderRate(ISheet sheet, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            if (sheet.GetRow(0) == null)
                sb.AppendLine(string.Format("Row Heade(1) sheet Rate cannot empty ", sheet.GetRow(0)));
            else
            {
                if (sheet.GetRow(0).GetCell(0).StringCellValue != "SERVICE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(0).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(1).StringCellValue != "ORIGIN CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(1).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(1).StringCellValue));
                if (sheet.GetRow(0).GetCell(2).StringCellValue != "ORIGIN DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(2).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(2).StringCellValue));
                if (sheet.GetRow(0).GetCell(3).StringCellValue != "DESTINATION CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(3).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(3).StringCellValue));
                if (sheet.GetRow(0).GetCell(4).StringCellValue != "DESTINATION DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(4).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(4).StringCellValue));
                if (sheet.GetRow(0).GetCell(5).StringCellValue != "CURRENCY")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(5).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(5).StringCellValue));
                if (sheet.GetRow(0).GetCell(6).StringCellValue != "LEAD TIME")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(6).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(6).StringCellValue));
                if (sheet.GetRow(0).GetCell(7).StringCellValue != "MIN RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(7).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(7).StringCellValue));
                if (sheet.GetRow(0).GetCell(8).StringCellValue != "0-1000")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(8).GetCell(8).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(9).StringCellValue != "1001-3999")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(9).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(9).StringCellValue));
                if (sheet.GetRow(0).GetCell(10).StringCellValue != "4000-99999")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(10).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(10).StringCellValue));
                if (sheet.GetRow(0).GetCell(11).StringCellValue != "VIA")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(11).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(11).StringCellValue));
                if (sheet.GetRow(0).GetCell(12).StringCellValue != "RAGULATED AGENT")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(12).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(12).StringCellValue));
                if (sheet.GetRow(0).GetCell(13).StringCellValue != "COST")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(13).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(13).StringCellValue));
                if (sheet.GetRow(0).GetCell(14).StringCellValue != "IR")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(14).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(14).StringCellValue));
                if (sheet.GetRow(0).GetCell(15).StringCellValue != "VALID ON MOUNTH")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(15).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(15).StringCellValue));
                if (sheet.GetRow(0).GetCell(16).StringCellValue != "VALID ON YEARS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(16).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(16).StringCellValue));
                if (sheet.GetRow(0).GetCell(17).StringCellValue != "REMARKS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(17).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(17).StringCellValue));
            }

            return sb;
        }

        private static StringBuilder SheetHeaderInboundRate(ISheet sheet, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            if (sheet.GetRow(0) == null)
                sb.AppendLine(string.Format("Row Heade(1) sheet Inbound Rate cannot empty ", sheet.GetRow(0)));
            else
            {
                if (sheet.GetRow(0).GetCell(0).StringCellValue != "SERVICE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(0).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(1).StringCellValue != "ORIGIN CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(1).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(1).StringCellValue));
                if (sheet.GetRow(0).GetCell(2).StringCellValue != "ORIGIN DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(2).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(2).StringCellValue));
                if (sheet.GetRow(0).GetCell(3).StringCellValue != "DESTINATION CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(3).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(3).StringCellValue));
                if (sheet.GetRow(0).GetCell(4).StringCellValue != "DESTINATION DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(4).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(4).StringCellValue));
                if (sheet.GetRow(0).GetCell(5).StringCellValue != "CURRENCY")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(5).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(5).StringCellValue));
                if (sheet.GetRow(0).GetCell(6).StringCellValue != "LEAD TIME")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(6).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(6).StringCellValue));
                if (sheet.GetRow(0).GetCell(7).StringCellValue != "PORT HUB")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(7).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(7).StringCellValue));
                if (sheet.GetRow(0).GetCell(8).StringCellValue != "SOURCE - SIN RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(8).GetCell(8).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(9).StringCellValue != "HANDLING AT SIN RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(9).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(9).StringCellValue));
                if (sheet.GetRow(0).GetCell(10).StringCellValue != "SIN-ID PORT RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(10).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(10).StringCellValue));
                if (sheet.GetRow(0).GetCell(11).StringCellValue != "CUSTOM CLEARANCE RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(11).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(11).StringCellValue));
                if (sheet.GetRow(0).GetCell(12).StringCellValue != "DEBIT NOTE RATE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(12).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(12).StringCellValue));
                if (sheet.GetRow(0).GetCell(13).StringCellValue != "RATE INBOUND")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(13).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(13).StringCellValue));
                if (sheet.GetRow(0).GetCell(14).StringCellValue != "VALID ON MOUNTH")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(14).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(14).StringCellValue));
                if (sheet.GetRow(0).GetCell(15).StringCellValue != "VALID ON YEARS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(15).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(15).StringCellValue));
                if (sheet.GetRow(0).GetCell(16).StringCellValue != "REMARKS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(16).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(16).StringCellValue));
            }

            return sb;
        }

        private static StringBuilder SheetHeaderSurcharge(ISheet sheet, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            if (sheet.GetRow(0) == null)
                sb.AppendLine(string.Format("Row Heade(1) sheet Surcharge cannot empty ", sheet.GetRow(0)));
            else
            {
                if (sheet.GetRow(0).GetCell(0).StringCellValue != "SERVICE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(0).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(1).StringCellValue != "ORIGIN CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(1).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(1).StringCellValue));
                if (sheet.GetRow(0).GetCell(2).StringCellValue != "ORIGIN DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(2).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(2).StringCellValue));
                if (sheet.GetRow(0).GetCell(3).StringCellValue != "DESTINATION CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(3).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(3).StringCellValue));
                if (sheet.GetRow(0).GetCell(4).StringCellValue != "DESTINATION DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(4).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(4).StringCellValue));
                if (sheet.GetRow(0).GetCell(5).StringCellValue != "MODA")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(5).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(5).StringCellValue));
                if (sheet.GetRow(0).GetCell(6).StringCellValue != "SURCHARGE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(6).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(6).StringCellValue));
                if (sheet.GetRow(0).GetCell(7).StringCellValue != "SURCHARGE 50%")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(7).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(7).StringCellValue));
                if (sheet.GetRow(0).GetCell(8).StringCellValue != "SURCHARGE 100%")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(8).GetCell(8).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(9).StringCellValue != "SURCHARGE 200%")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(9).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(9).StringCellValue));
                if (sheet.GetRow(0).GetCell(10).StringCellValue != "VALID ON MOUNTH")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(10).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(10).StringCellValue));
                if (sheet.GetRow(0).GetCell(11).StringCellValue != "VALID ON YEARS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(11).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(11).StringCellValue));
                if (sheet.GetRow(0).GetCell(12).StringCellValue != "REMARKS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(12).StringCellValue + " in sheet " +
                    "" + Modul + " your document template.", sheet.GetRow(0).GetCell(12).StringCellValue));
            }

            return sb;
        }

        private static StringBuilder SheetHeaderTruckRate(ISheet sheet, string Modul)
        {
            StringBuilder sb = new StringBuilder();
            if (sheet.GetRow(0) == null)
                sb.AppendLine(string.Format("Row Heade(1) sheet Trucking Rate cannot empty ", sheet.GetRow(0)));
            else
            {
                if (sheet.GetRow(0).GetCell(0).StringCellValue != "ORIGIN CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(0).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(0).StringCellValue));
                if (sheet.GetRow(0).GetCell(1).StringCellValue != "ORIGIN DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(1).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(1).StringCellValue));
                if (sheet.GetRow(0).GetCell(2).StringCellValue != "DESTINATION CITY CODE")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(2).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(2).StringCellValue));
                if (sheet.GetRow(0).GetCell(3).StringCellValue != "DESTINATION DETAIL")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(3).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(3).StringCellValue));
                if (sheet.GetRow(0).GetCell(4).StringCellValue != "CONDITION OF MODA")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(4).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(4).StringCellValue));
                if (sheet.GetRow(0).GetCell(5).StringCellValue != "RATE PER TRIP (IDR)")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(5).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(5).StringCellValue));
                if (sheet.GetRow(0).GetCell(6).StringCellValue != "REMARKS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(6).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(6).StringCellValue));
                if (sheet.GetRow(0).GetCell(7).StringCellValue != "VALID ON MOUNTH")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(7).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(7).StringCellValue));
                if (sheet.GetRow(0).GetCell(8).StringCellValue != "VALID ON YEARS")
                    sb.AppendLine(string.Format("There is no column " + sheet.GetRow(0).GetCell(8).StringCellValue + " in sheet " +
                        "" + Modul + " your document template.", sheet.GetRow(0).GetCell(8).StringCellValue));
            }

            return sb;
        }

        public static void setException(Data.Domain.LogImport _logImport, string _message,
            string _filName, string _modul, string _sheet)
        {
            _logImport.Message = _message;
            _logImport.FileName = _filName;
            _logImport.Modul = _modul;
            _logImport.Sheet = _sheet;
            Service.Master.LogImport.crud(_logImport, "I");
        }


        public class ALLsheet
        {
            public ISheet sheetRate { get; set; }
            public ISheet sheetInboundRate { get; set; }
            public ISheet sheetSurcharge { get; set; }
            public ISheet sheetTruckRate { get; set; }
        }

        public static void TruncateTbale(string _tableName)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.ExecuteSqlCommand("truncate table [dbo].[" + _tableName + "]");
            }
        }



        //public List<Data.Domain.ModaOfCondition> getListTruckRate(string FilePath)
        //{
        //    try
        //    {
        //        //List<RequestProgramProposal> result = new List<RequestProgramProposal>();


        //        DataTable dt = new DataTable();


        //        //getDataHeader(sheet);
        //        //result = getDataDetail(sheet);
        //        //result.ForEach(a =>
        //        //{
        //        //    a.PPTypeID = PPTypeID;
        //        //    a.AreaID = AreaID;
        //        //    a.ChannelID = ChannelID;
        //        //    a.AccountID = AccountID;
        //        //    a.ProgramName = ProgramName;
        //        //    a.DistributorID = DistributorID;
        //        //    a.BusinessID = BusinessID;
        //        //    a.DivisionID = DivisionID;
        //        //    a.DateFrom = DateFrom;
        //        //    a.DateTo = DateTo;
        //        //    a.Mechanism = Mechanism;
        //        //    a.Detail = Detail;
        //        //    a.IsActive = true;
        //        //    a.CreatedBy = "1";
        //        //    a.CreatedDate = DateTime.Now;
        //        //    a.UpdatedBy = "1";
        //        //    a.UpdatedDate = DateTime.Now;
        //        //});

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //        return null;
        //    }
        //}

        //public static string CheckTemplateSheet(DataTable sourceTable, string Modul)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    List<string> mandatoryColumns = new List<string>();

        //    //if (Modul == "Rate")
        //    //    mandatoryColumns = GetMandatoryColumnsTemplateRate();
        //    //if (Modul == "Inbound Rate")
        //    //    mandatoryColumns = GetMandatoryColumnsTemplateInboundRate();
        //    //if (Modul == "Surcharge")
        //    //    mandatoryColumns = GetMandatoryColumnsTemplateSurcharge();
        //    if (Modul == "Trucking Rate")
        //        mandatoryColumns = GetMandatoryColumnsTemplateTrucking();

        //    DataColumnCollection columns = sourceTable.Columns;

        //    foreach (string s in mandatoryColumns)
        //    {
        //        if (!columns.Contains(s))
        //        {
        //            sb.AppendLine(string.Format("There is no column {0} in sheet " + Modul + " your document template. || ", s));

        //        }
        //    }

        //    return sb.ToString();
        //}

        //static List<string> GetMandatoryColumnsTemplateTrucking()
        //{
        //    List<string> columns = new List<string>();
        //    columns.Add("ORIGIN CITY CODE");
        //    columns.Add("DESTINATION CITY CODE");
        //    columns.Add("CONDITION OF MODA");
        //    columns.Add("RATE PER TRIP (IDR)");
        //    columns.Add("REMARKS");
        //    columns.Add("Valid on Mounth");
        //    columns.Add("Valid on Years");

        //    return columns;
        //}



    }
}
