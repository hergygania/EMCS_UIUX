using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using Spire.Xls;
using System.IO;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRExportTransaction
    {
        public const string CacheName = "App.EMCS.SvcRExportTransaction";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic TotalExportMonthly(Domain.MasterSearchForm crit)
        {
            var thisYear = DateTime.Now.Year.ToString();
            string yearNow = crit.searchCode ?? thisYear;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", yearNow));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpRTotalExportMonthly>(@"exec [dbo].[Sp_Report_Total_Export_Monthly]@year", parameters).FirstOrDefault();
                return data;
            }
        }

        public static dynamic TotalExportPort(Domain.MasterSearchForm crit)
        {
            var thisYear = DateTime.Now.Year.ToString();
            string yearNow = crit.searchCode ?? thisYear;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", yearNow));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpRTotalExportPort>(@"exec [dbo].[Sp_Report_Total_Export_Port]@year", parameters).ToList();
                return data;
            }
        }

        public static dynamic TotalExportMonthlyDownload(Domain.MasterSearchForm crit)
        {
            var thisYear = DateTime.Now.Year.ToString();
            string yearNow = crit.searchCode ?? thisYear;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", yearNow));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpRTotalExportMonthly>(@"exec [dbo].[Sp_Report_Total_Export_Monthly]@year", parameters).ToList();
                return data;
            }
        }
        public static MemoryStream GetTotalExportTransactionMonthlyStream(List<Data.Domain.EMCS.SpRTotalExportMonthly> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<TotalExportTransactionMonthlyCellRange> cellrange = GetTotalExportTransactionMonthlyReplacedCellsChart(chartSheet);

                if (WriteTotalExportTransactionMonthlyReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
                {
                    workbook.SaveToStream(output, FileFormat.Version97to2003);
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<TotalExportTransactionMonthlyCellRange> GetTotalExportTransactionMonthlyReplacedCellsChart(Worksheet worksheet)
        {
            List<TotalExportTransactionMonthlyCellRange> cellrange = new List<TotalExportTransactionMonthlyCellRange>();

            try
            {
                for (int i = 1; i <= 12; i++)
                {
                    cellrange.Add(
                        new TotalExportTransactionMonthlyCellRange
                        {
                            Total = worksheet.FindString("[Export" + i + "]", false, false),
                            //Total = worksheet.FindString("[Total" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteTotalExportTransactionMonthlyReplacedCellsChart(List<Data.Domain.EMCS.SpRTotalExportMonthly> data, Worksheet worksheet, List<TotalExportTransactionMonthlyCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                worksheet.Replace(range[0].Total.Value, data[0].January);
                worksheet.Replace(range[1].Total.Value, data[0].February);
                worksheet.Replace(range[2].Total.Value, data[0].March);
                worksheet.Replace(range[3].Total.Value, data[0].April);
                worksheet.Replace(range[4].Total.Value, data[0].May);
                worksheet.Replace(range[5].Total.Value, data[0].June);
                worksheet.Replace(range[6].Total.Value, data[0].July);
                worksheet.Replace(range[7].Total.Value, data[0].August);
                worksheet.Replace(range[8].Total.Value, data[0].September);
                worksheet.Replace(range[9].Total.Value, data[0].October);
                worksheet.Replace(range[10].Total.Value, data[0].November);
                worksheet.Replace(range[11].Total.Value, data[0].December);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
        // ============================================================
        public static MemoryStream GetTotalExportTransactionPortStream(List<Data.Domain.EMCS.SpRTotalExportPort> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<TotalExportTransactionPortCellRange> cellrange = GetTotalExportTransactionPortReplacedCellsChart(chartSheet);

                if (WriteTotalExportTransactionPortReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
                {
                    workbook.SaveToStream(output, FileFormat.Version97to2003);
                }

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<TotalExportTransactionPortCellRange> GetTotalExportTransactionPortReplacedCellsChart(Worksheet worksheet)
        {
            List<TotalExportTransactionPortCellRange> cellrange = new List<TotalExportTransactionPortCellRange>();

            try
            {
                for (int i = 1; i <= 30; i++)
                {
                    cellrange.Add(
                        new TotalExportTransactionPortCellRange
                        {
                            PortOfDestination = worksheet.FindString("[Branch" + i + "]", false, false),
                            Total = worksheet.FindString("[TotalBranch" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteTotalExportTransactionPortReplacedCellsChart(List<Data.Domain.EMCS.SpRTotalExportPort> data, Worksheet worksheet, List<TotalExportTransactionPortCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                for (int i = 0; i < 30; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].PortOfDestination.Value, data[i].PortOfLoading);  
                        worksheet.Replace(range[i].Total.Value, data[i].Total); 
                    }
                    else
                    {
                        worksheet.Replace(range[i].PortOfDestination.Value, string.Empty);
                        worksheet.Replace(range[i].Total.Value, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }


    }

    public class TotalExportTransactionMonthlyCellRange
    {
        public CellRange Total { get; set; }
    }

    public class TotalExportTransactionPortCellRange
    {
        public CellRange PortOfDestination { get; set; }
        public CellRange Total { get; set; }
    }
}
