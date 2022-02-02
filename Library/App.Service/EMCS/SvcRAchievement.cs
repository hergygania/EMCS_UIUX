using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using App.Data.Domain.EMCS;
using Spire.Xls;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRAchievement
    {
        public const string CacheName = "App.EMCS.SvcRAchievement";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRAchievement> AchievementList(CiplListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                var data = db.DbContext.Database.SqlQuery<SpRAchievement>(@"[dbo].[SP_RAchievement]").ToList();
                return data;
            }
        }

        public static List<SpRAchievement> AchievementChart(String startDate, String endDate, String cycle)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                if (cycle == "undefined" || cycle == null) cycle = "";
                if (cycle != null)
                {
                    cycle = Regex.Replace(cycle, @"[^0-9a-zA-Z]+", "");
                }
                

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@StartDate", startDate));
                parameterList.Add(new SqlParameter("@EndDate", endDate));
                parameterList.Add(new SqlParameter("@Cycle", cycle ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRAchievement>(@"[dbo].[SP_RAchievement_Chart] @StartDate, @EndDate, @Cycle", parameters).ToList();
                return data;
            }
        }

        public static List<SpRAchievement> GetAchievementListExport(string startDate, string endDate)
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
                    var data = db.DbContext.Database.SqlQuery<SpRAchievement>("exec [dbo].[SP_RAchievement] @StartDate, @EndDate", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetAchievementStream(List<SpRAchievement> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();
                ChartDataRAchievement cycle = new ChartDataRAchievement();

                #region tabularSheet
                Worksheet tabularSheet = workbook.Worksheets[0];
                int startRow = 3;
                int tmpAchievement = 0;
                decimal totAchievement;
                decimal totUnachieved;

                for (int i = 0; i < data.Count; i++)
                {
                    tabularSheet.InsertRow(startRow + i);

                    tabularSheet.SetCellValue(startRow + i, 1, data[i].Cycle);
                    tabularSheet.SetCellValue(startRow + i, 2, data[i].Target);
                    tabularSheet.SetCellValue(startRow + i, 3, data[i].Actual);
                    tabularSheet.SetCellValue(startRow + i, 4, data[i].Achievement);

                    tmpAchievement += int.Parse(data[i].Achievement);
                }
                var dcmAchievement = Convert.ToDecimal(tmpAchievement);
                #endregion

                totAchievement = dcmAchievement / (data.Count * 100);
                totUnachieved = (100 - (dcmAchievement / data.Count)) / 100;

                cycle.PercentageAchieved = totAchievement.ToString(CultureInfo.InvariantCulture);
                cycle.PercentageUnachieved = totUnachieved.ToString(CultureInfo.InvariantCulture);

                Worksheet chartSheet = workbook.Worksheets[0];
                ChartCellRangeRAchievement cellrange = GetReplacedCellsChart(chartSheet);

                if (WriteReplacedCellsChart(cycle, chartSheet, cellrange) == string.Empty)
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

        public static ChartCellRangeRAchievement GetReplacedCellsChart(Worksheet worksheet)
        {
            ChartCellRangeRAchievement cellrange = new ChartCellRangeRAchievement();

            try
            {
                cellrange.PercentageAchieved = worksheet.FindString("[PercentageAchieved]", false, false);
                cellrange.PercentageUnachieved = worksheet.FindString("[PercentageUnachieved]", false, false);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }

        public static string WriteReplacedCellsChart(ChartDataRAchievement data, Worksheet worksheet, ChartCellRangeRAchievement range)
        {
            string msg = string.Empty;
            try
            {
                range.PercentageAchieved.Value = data.PercentageAchieved;
                range.PercentageAchieved.NumberFormat = "0%";
                range.PercentageUnachieved.Value = data.PercentageUnachieved;
                range.PercentageUnachieved.NumberFormat = "0%";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }
    }

    public class ChartDataRAchievement
    {
        public string PercentageAchieved { get; set; }
        public string PercentageUnachieved { get; set; }
    }

    public class ChartCellRangeRAchievement
    {
        public CellRange PercentageAchieved { get; set; }
        public CellRange PercentageUnachieved { get; set; }
    }
}
