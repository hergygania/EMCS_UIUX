using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Globalization;
using App.Data.Domain.EMCS;
using System.IO;
using Spire.Xls;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRpttuBranch
    {
        public const string CacheName = "App.EMCS.SvcRPTTUBranch";

        public static List<SpRpttuBranch> PttuBranchList(string startMonth, string endMonth, string type)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartMonth", startMonth ?? ""));
                    parameterList.Add(new SqlParameter("@EndMonth", endMonth ?? ""));
                    parameterList.Add(new SqlParameter("@Type", type ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRpttuBranch>("exec [dbo].[SP_RPTTUBranch]  @StartMonth, @EndMonth, @Type", parameters).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRpttuBranchAverage> PttuBranchAverage(string startMonth, string endMonth)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartMonth", startMonth ?? ""));
                    parameterList.Add(new SqlParameter("@EndMonth", endMonth ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRpttuBranchAverage>("exec [dbo].[SP_RPTTUBranch_Average]  @StartMonth, @EndMonth", parameters).ToList();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static SpRpttuBranchAverageYtd PttuBranchAverageYtd(string startMonth, string endMonth)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartMonth", startMonth ?? ""));
                    parameterList.Add(new SqlParameter("@EndMonth", endMonth ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRpttuBranchAverageYtd>("exec [dbo].[SP_RPTTUBranch_AverageYTD]  @StartMonth, @EndMonth", parameters).FirstOrDefault();

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetPttuBranchStream(string startDate, string endDate, string type, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();
                List<SpRpttuBranch> dataCategory = PttuBranchList(startDate, endDate, type).ToList();
                List<SpRpttuBranchAverage> dataAverage = PttuBranchAverage(startDate, endDate).ToList();
                SpRpttuBranchAverageYtd dataAverageYtd = PttuBranchAverageYtd(startDate, endDate);

                int totalPebJan = 0, totalPebFeb = 0, totalPebMar = 0, totalPebApr = 0, totalPebMay = 0, totalPebJun = 0, totalPebJul = 0, totalPebAug = 0, totalPebSep = 0, totalPebOct = 0, totalPebNov = 0, totalPebDec = 0, totalPeb = 0;

                Worksheet worksheet = workbook.Worksheets[0];

                int startRow = 3;

                CellRange categoryName = worksheet.FindString("[CategoryName]", false, false);
                worksheet.Replace(categoryName.Value, type == "Branch" ? "PPTU BRANCH" : "LOADING PORT");

                for (int i = 0; i < dataCategory.Count; i++)
                {
                    worksheet.InsertRow(startRow + i);

                    worksheet.SetCellValue(startRow + i, 1, dataCategory[i].RowNumber);
                    worksheet.SetCellValue(startRow + i, 2, dataCategory[i].Name);
                    worksheet.SetCellValue(startRow + i, 3, dataCategory[i].TotalPebJan.ToString());
                    worksheet.SetCellValue(startRow + i, 4, dataCategory[i].TotalPebFeb.ToString());
                    worksheet.SetCellValue(startRow + i, 5, dataCategory[i].TotalPebMar.ToString());
                    worksheet.SetCellValue(startRow + i, 6, dataCategory[i].TotalPebApr.ToString());
                    worksheet.SetCellValue(startRow + i, 7, dataCategory[i].TotalPebMay.ToString());
                    worksheet.SetCellValue(startRow + i, 8, dataCategory[i].TotalPebJun.ToString());
                    worksheet.SetCellValue(startRow + i, 9, dataCategory[i].TotalPebJul.ToString());
                    worksheet.SetCellValue(startRow + i, 10, dataCategory[i].TotalPebAug.ToString());
                    worksheet.SetCellValue(startRow + i, 11, dataCategory[i].TotalPebSep.ToString());
                    worksheet.SetCellValue(startRow + i, 12, dataCategory[i].TotalPebOct.ToString());
                    worksheet.SetCellValue(startRow + i, 13, dataCategory[i].TotalPebNov.ToString());
                    worksheet.SetCellValue(startRow + i, 14, dataCategory[i].TotalPebDec.ToString());
                    worksheet.SetCellValue(startRow + i, 15, dataCategory[i].TotalPeb.ToString());

                    totalPebJan += dataCategory[i].TotalPebJan;
                    totalPebFeb += dataCategory[i].TotalPebFeb;
                    totalPebMar += dataCategory[i].TotalPebMar;
                    totalPebApr += dataCategory[i].TotalPebApr;
                    totalPebMay += dataCategory[i].TotalPebMay;
                    totalPebJun += dataCategory[i].TotalPebJun;
                    totalPebJul += dataCategory[i].TotalPebJul;
                    totalPebAug += dataCategory[i].TotalPebAug;
                    totalPebSep += dataCategory[i].TotalPebSep;
                    totalPebOct += dataCategory[i].TotalPebOct;
                    totalPebNov += dataCategory[i].TotalPebNov;
                    totalPebDec += dataCategory[i].TotalPebDec;
                    totalPeb += dataCategory[i].TotalPeb;
                }

                worksheet.InsertRow(startRow + dataCategory.Count);

                CellRange cell = worksheet.Range[startRow + dataCategory.Count, 1, startRow + dataCategory.Count, 2];
                cell.Merge();
                cell.Style.HorizontalAlignment = HorizontalAlignType.Center;
                cell.Value = "Total";

                worksheet.SetCellValue(startRow + dataCategory.Count, 3, totalPebJan.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 4, totalPebFeb.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 5, totalPebMar.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 6, totalPebApr.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 7, totalPebMay.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 8, totalPebJun.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 9, totalPebJul.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 10, totalPebAug.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 11, totalPebSep.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 12, totalPebOct.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 13, totalPebNov.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 14, totalPebDec.ToString());
                worksheet.SetCellValue(startRow + dataCategory.Count, 15, totalPeb.ToString());

                #region Total Per Month
                startRow += dataCategory.Count + 1 + 4;
                worksheet.InsertRow(startRow);

                CellRange cellMonthlyTotal = worksheet.Range[startRow, 1, startRow, 2];
                cellMonthlyTotal.Merge();
                cellMonthlyTotal.Value = "Total Per Month";

                worksheet.SetCellValue(startRow, 3, totalPebJan.ToString());
                worksheet.SetCellValue(startRow, 4, totalPebFeb.ToString());
                worksheet.SetCellValue(startRow, 5, totalPebMar.ToString());
                worksheet.SetCellValue(startRow, 6, totalPebApr.ToString());
                worksheet.SetCellValue(startRow, 7, totalPebMay.ToString());
                worksheet.SetCellValue(startRow, 8, totalPebJun.ToString());
                worksheet.SetCellValue(startRow, 9, totalPebJul.ToString());
                worksheet.SetCellValue(startRow, 10, totalPebAug.ToString());
                worksheet.SetCellValue(startRow, 11, totalPebSep.ToString());
                worksheet.SetCellValue(startRow, 12, totalPebOct.ToString());
                worksheet.SetCellValue(startRow, 13, totalPebNov.ToString());
                worksheet.SetCellValue(startRow, 14, totalPebDec.ToString());
                #endregion

                #region Total Per Quarter
                startRow += 1;
                worksheet.InsertRow(startRow);

                CellRange cellQuarterlyTotal = worksheet.Range[startRow, 1, startRow, 2];
                cellQuarterlyTotal.Merge();
                cellQuarterlyTotal.Value = "Total Per Quarter";

                CellRange cellQ1 = worksheet.Range[startRow, 3, startRow, 5];
                cellQ1.Merge();
                cellQ1.Value = (totalPebJan + totalPebFeb + totalPebMar).ToString();

                CellRange cellQ2 = worksheet.Range[startRow, 6, startRow, 8];
                cellQ2.Merge();
                cellQ2.Value = (totalPebApr + totalPebMay + totalPebJun).ToString();

                CellRange cellQ3 = worksheet.Range[startRow, 9, startRow, 11];
                cellQ3.Merge();
                cellQ3.Value = (totalPebJul + totalPebAug + totalPebSep).ToString();

                CellRange cellQ4 = worksheet.Range[startRow, 12, startRow, 14];
                cellQ4.Merge();
                cellQ4.Value = (totalPebOct + totalPebNov + totalPebDec).ToString();
                #endregion

                #region Average
                startRow += 1;
                for (int i = 0; i < dataAverage.Count; i++)
                {
                    worksheet.InsertRow(startRow + i);

                    CellRange cellDescription = worksheet.Range[startRow + i, 1, startRow + i, 2];
                    cellDescription.Merge();
                    cellDescription.Value = dataAverage[i].Description;

                    worksheet.SetCellValue(startRow + i, 3, dataAverage[i].Jan.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 4, dataAverage[i].Feb.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 5, dataAverage[i].Mar.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 6, dataAverage[i].Apr.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 7, dataAverage[i].May.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 8, dataAverage[i].Jun.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 9, dataAverage[i].Jul.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 10, dataAverage[i].Aug.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 11, dataAverage[i].Sep.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 12, dataAverage[i].Oct.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 13, dataAverage[i].Nov.ToString(CultureInfo.InvariantCulture));
                    worksheet.SetCellValue(startRow + i, 14, dataAverage[i].Dec.ToString(CultureInfo.InvariantCulture));
                }

                CellRange cellTotal = worksheet.Range[startRow - 2, 15, startRow + dataAverage.Count - 1, 15];
                cellTotal.Merge();
                cellTotal.Style.HorizontalAlignment = HorizontalAlignType.Center;
                cellTotal.Value = totalPeb.ToString();
                #endregion

                #region Average YTD
                startRow += dataAverage.Count;
                worksheet.InsertRow(startRow);

                CellRange cellMonthlyAvgTitle = worksheet.Range[startRow, 1, startRow, 2];
                cellMonthlyAvgTitle.Merge();
                cellMonthlyAvgTitle.Value = "Monthly Average (YTD)";

                CellRange cellMonthlyAvg = worksheet.Range[startRow, 3, startRow, 15];
                cellMonthlyAvg.Merge();
                cellMonthlyAvg.Style.HorizontalAlignment = HorizontalAlignType.Center;
                cellMonthlyAvg.Value = dataAverageYtd.YtdMonthlyAvg.ToString(CultureInfo.InvariantCulture);


                startRow += 1;
                worksheet.InsertRow(startRow);

                CellRange cellWeeklyAvgTitle = worksheet.Range[startRow, 1, startRow, 2];
                cellWeeklyAvgTitle.Merge();
                cellWeeklyAvgTitle.Value = "Weekly Average (YTD)";

                CellRange cellWeeklyAvg = worksheet.Range[startRow, 3, startRow, 15];
                cellWeeklyAvg.Merge();
                cellWeeklyAvg.Style.HorizontalAlignment = HorizontalAlignType.Center;
                cellWeeklyAvg.Value = dataAverageYtd.YtdWeeklyAvg.ToString(CultureInfo.InvariantCulture);


                startRow += 1;
                worksheet.InsertRow(startRow);

                CellRange cellDailyAvgTitle = worksheet.Range[startRow, 1, startRow, 2];
                cellDailyAvgTitle.Merge();
                cellDailyAvgTitle.Value = "Daily Average (YTD)";

                CellRange cellDailyAvg = worksheet.Range[startRow, 3, startRow, 15];
                cellDailyAvg.Merge();
                cellDailyAvg.Style.HorizontalAlignment = HorizontalAlignType.Center;
                cellDailyAvg.Value = dataAverageYtd.YtdDailyAvg.ToString(CultureInfo.InvariantCulture);
                #endregion

                workbook.SaveToStream(output, FileFormat.Version97to2003);

                return output;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
