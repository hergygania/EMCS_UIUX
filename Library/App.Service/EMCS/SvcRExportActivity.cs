using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using App.Data.Domain.EMCS;
using Spire.Xls;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRExportActivity
    {
        public const string CacheName = "App.EMCS.SvcRExportActivity";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRTrendExport> TrendExportList(int startYear, int endYear, string filter)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var sql = "EXEC SP_ActivityReport_TrendExport @startYear = " + startYear + ", @endYear = " + endYear + ", @filter = '" + filter + "'";
                    List<SpRTrendExport> data = db.Database.SqlQuery<SpRTrendExport>(sql).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRExportByCategory> ExportByCategoryList(int startYear, int endYear)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var sql = "EXEC SP_ActivityReport_ExportByCategory @startYear = " + startYear + ", @endYear = " + endYear;
                    List<SpRExportByCategory> data = db.Database.SqlQuery<SpRExportByCategory>(sql).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRSalesVsNonSales> SalesVsNonSalesList(int startYear, int endYear)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var sql = "EXEC SP_ActivityReport_SalesVSNonSales @startYear = " + startYear + ", @endYear = " + endYear;
                    List<SpRSalesVsNonSales> data = db.Database.SqlQuery<SpRSalesVsNonSales>(sql).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRTotalExport> TotalExportList(int year)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var sql = "EXEC SP_ActivityReport_TotalExport @year = " + year;
                    List<SpRTotalExport> data = db.Database.SqlQuery<SpRTotalExport>(sql).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<DashboardExportToday> TotalBig5CommoditiesList(Domain.MasterSearchForm crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    int year = DateTime.Now.Year;
                    var searchId = crit.searchId ?? year;
                    var searchName = crit.searchName ?? "";
                    //if (searchName != null)
                    //{
                    //    searchName = Regex.Replace(searchName, @"[^0-9a-zA-Z]+", "");
                    //}
                    

                    var sql = "EXEC SP_RBigestCommodities @date1 = '" + searchId + "', @ExportType = '" + searchName + "'";
                    List<DashboardExportToday> data = db.Database.SqlQuery<DashboardExportToday>(sql).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Export

        public static MemoryStream GetTrendExportStream(List<SpRTrendExport> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<TrendExportChartCellRange> cellrange = GetTrendExportReplacedCellsChart(chartSheet);

                if (WriteTrendExportReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
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
        public static List<TrendExportChartCellRange> GetTrendExportReplacedCellsChart(Worksheet worksheet)
        {
            List<TrendExportChartCellRange> cellrange = new List<TrendExportChartCellRange>();

            try
            {
                for (int i = 1; i <= 50; i++)
                {
                    cellrange.Add(
                        new TrendExportChartCellRange
                        {
                            Year = worksheet.FindString("[Year" + i + "]", false, false),
                            Export = worksheet.FindString("[Export" + i + "]", false, false),
                            Peb = worksheet.FindString("[PEB" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteTrendExportReplacedCellsChart(List<SpRTrendExport> data, Worksheet worksheet, List<TrendExportChartCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].Year.Value, data[i].Year);
                        worksheet.Replace(range[i].Export.Value, data[i].TotalExport.ToString(CultureInfo.InvariantCulture));
                        worksheet.Replace(range[i].Peb.Value, data[i].TotalPeb);
                    }
                    else
                    {
                        worksheet.Replace(range[i].Year.Value, string.Empty);
                        worksheet.Replace(range[i].Export.Value, string.Empty);
                        worksheet.Replace(range[i].Peb.Value, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static MemoryStream GetExportByCategoryStream(List<SpRExportByCategory> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<ExportByCategoryChartCellRange> cellrange = GetExportByCategoryReplacedCellsChart(chartSheet);

                if (WriteExportByCategoryReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
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
        public static List<ExportByCategoryChartCellRange> GetExportByCategoryReplacedCellsChart(Worksheet worksheet)
        {
            List<ExportByCategoryChartCellRange> cellrange = new List<ExportByCategoryChartCellRange>();

            try
            {
                for (int i = 1; i <= 50; i++)
                {
                    cellrange.Add(
                        new ExportByCategoryChartCellRange
                        {
                            Category = worksheet.FindString("[Category" + i + "]", false, false),
                            Percentage = worksheet.FindString("[Percentage" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteExportByCategoryReplacedCellsChart(List<SpRExportByCategory> data, Worksheet worksheet, List<ExportByCategoryChartCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].Category.Value, data[i].Category);

                        range[i].Percentage.Value = data[i].TotalPercentage.ToString(CultureInfo.InvariantCulture);
                        range[i].Percentage.NumberFormat = "0%";
                    }
                    else
                    {
                        worksheet.Replace(range[i].Category.Value, string.Empty);
                        worksheet.Replace(range[i].Percentage.Value, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static MemoryStream GetSalesVsNonSalesStream(List<SpRSalesVsNonSales> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<SalesVsNonSalesChartCellRange> cellrange = GetSalesVsNonSalesReplacedCellsChart(chartSheet);

                if (WriteSalesVsNonSalesReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
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
        public static List<SalesVsNonSalesChartCellRange> GetSalesVsNonSalesReplacedCellsChart(Worksheet worksheet)
        {
            List<SalesVsNonSalesChartCellRange> cellrange = new List<SalesVsNonSalesChartCellRange>();

            try
            {
                for (int i = 1; i <= 50; i++)
                {
                    cellrange.Add(
                        new SalesVsNonSalesChartCellRange
                        {
                            Year = worksheet.FindString("[Year" + i + "]", false, false),
                            Sales = worksheet.FindString("[Sales" + i + "]", false, false),
                            NonSales = worksheet.FindString("[NonSales" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteSalesVsNonSalesReplacedCellsChart(List<SpRSalesVsNonSales> data, Worksheet worksheet, List<SalesVsNonSalesChartCellRange> range)
        {
            string msg = string.Empty;

            try
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].Year.Value, data[i].Year);

                        range[i].Sales.Value = data[i].Sales.ToString(CultureInfo.InvariantCulture);
                        range[i].Sales.NumberFormat = "0.00";

                        range[i].NonSales.Value = data[i].NonSales.ToString(CultureInfo.InvariantCulture);
                        range[i].NonSales.NumberFormat = "0.00";

                        //worksheet.Replace(range[i].Sales.Value, data[i].Sales.ToString());
                        //worksheet.Replace(range[i].NonSales.Value, data[i].NonSales.ToString());
                    }
                    else
                    {
                        worksheet.Replace(range[i].Year.Value, string.Empty);
                        worksheet.Replace(range[i].Sales.Value, string.Empty);
                        worksheet.Replace(range[i].NonSales.Value, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static MemoryStream GetTotalExportStream(List<SpRTotalExport> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<TotalExportChartCellRange> cellrange = GetTotalExportReplacedCellsChart(chartSheet);

                if (WriteTotalExportReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
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
        public static List<TotalExportChartCellRange> GetTotalExportReplacedCellsChart(Worksheet worksheet)
        {
            List<TotalExportChartCellRange> cellrange = new List<TotalExportChartCellRange>();

            try
            {
                for (int i = 1; i <= 50; i++)
                {
                    cellrange.Add(
                        new TotalExportChartCellRange
                        {
                            TotalInvoice = worksheet.FindString("[TotalInvoice" + i + "]", false, false),
                            TotalPeb = worksheet.FindString("[TotalPEB" + i + "]", false, false),
                            Outstanding = worksheet.FindString("[Outstanding" + i + "]", false, false),
                        });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return cellrange;
        }
        public static string WriteTotalExportReplacedCellsChart(List<SpRTotalExport> data, Worksheet worksheet, List<TotalExportChartCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].TotalInvoice.Value, data[i].Invoice);
                        worksheet.Replace(range[i].TotalPeb.Value, data[i].Peb);
                        worksheet.Replace(range[i].Outstanding.Value, data[i].Outstanding);
                    }
                    else
                    {
                        worksheet.Replace(range[i].TotalInvoice.Value, string.Empty);
                        worksheet.Replace(range[i].TotalPeb.Value, string.Empty);
                        worksheet.Replace(range[i].Outstanding.Value, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public static MemoryStream GetTotalBig5CommoditiesStream(List<DashboardExportToday> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                Worksheet chartSheet = workbook.Worksheets[0];
                List<TotalBig5CommoditiesCellRange> cellrange = GetTotalBig5CommoditiesReplacedCellsChart(chartSheet);

                if (WriteTotalBig5CommoditiesReplacedCellsChart(data, chartSheet, cellrange) == string.Empty)
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
        public static List<TotalBig5CommoditiesCellRange> GetTotalBig5CommoditiesReplacedCellsChart(Worksheet worksheet)
        {
            List<TotalBig5CommoditiesCellRange> cellrange = new List<TotalBig5CommoditiesCellRange>();

            try
            {
                for (int i = 1; i <= 5; i++)
                {
                    cellrange.Add(
                        new TotalBig5CommoditiesCellRange
                        {
                            Total = worksheet.FindString("[TotalInvoice" + i + "]", false, false),
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
        public static string WriteTotalBig5CommoditiesReplacedCellsChart(List<DashboardExportToday> data, Worksheet worksheet, List<TotalBig5CommoditiesCellRange> range)
        {
            string msg = string.Empty;
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].Total.Value, data[i].Total);
                    }
                    else
                    {
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

        #endregion
    }

    public class TrendExportChartCellRange
    {
        public CellRange Year { get; set; }
        public CellRange Export { get; set; }
        public CellRange Peb { get; set; }
    }
    public class ExportByCategoryChartCellRange
    {
        public CellRange Category { get; set; }
        public CellRange Percentage { get; set; }
    }
    public class SalesVsNonSalesChartCellRange
    {
        public CellRange Year { get; set; }
        public CellRange Sales { get; set; }
        public CellRange NonSales { get; set; }
    }
    public class TotalExportChartCellRange
    {
        public CellRange TotalInvoice { get; set; }
        public CellRange TotalPeb { get; set; }
        public CellRange Outstanding { get; set; }
    }
    public class TotalBig5CommoditiesCellRange
    {
        public CellRange Total { get; set; }
    }
}
