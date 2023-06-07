using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using Spire.Xls;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcRExportProblem
    {
        public const string CacheName = "App.EMCS.SvcRSailingEstimation";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRProblemHistory> ExportProblemList(CiplListFilter filter, string startDate, string endDate)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startdate", startDate ?? ""));
                parameterList.Add(new SqlParameter("@enddate", endDate ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRProblemHistory>("exec [dbo].[SP_RExportProblem]  @IsTotal, @sort, @order,  @offset, @limit, @startdate, @enddate", parameters).ToList();
                return data;
            }
        }

        public static List<SpRProblemHistory> GetProblemHistoryList(string startDate, string endDate)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@startdate", startDate ?? ""));
                    parameterList.Add(new SqlParameter("@enddate", endDate ?? ""));
                    parameterList.Add(new SqlParameter("@type", "category"));
                    parameterList.Add(new SqlParameter("@category", ""));
                    parameterList.Add(new SqlParameter("@case", ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRProblemHistory>("exec [dbo].[sp_get_report_problem_history]  @startdate, @enddate, @type, @category, @case", parameters).ToList();

                    return data
                            .Where(i => i.ParentId == 0)
                            .Select(i => new SpRProblemHistory
                            {
                                Id = i.Id,
                                ParentId = i.ParentId,
                                Name = i.Category,
                                ReqType = i.ReqType,
                                Category = i.Category,
                                Cases = i.Cases,
                                Causes = i.Causes,
                                Impact = i.Impact,
                                Total = i.TotalCategory,
                                TotalCauses = i.TotalCauses,
                                TotalCases = i.TotalCases,
                                TotalCategory = i.TotalCategory,
                                TotalCategoryPercentage = i.TotalCategoryPercentage,
                                Children = GetSub(data, i.Id)
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static RproblemCategory GetReportProblemHistory(string startDate, string endDate)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    var result = new RproblemCategory();
                    var footerList = new List<ItemFooter>();

                    var footer1 = new ItemFooter();
                    var footer2 = new ItemFooter();
                    var rows = new List<ItemProblem>();

                    startDate = startDate ?? "";
                    endDate = endDate ?? "";
                    if (startDate !=null)
                    {
                        startDate = Regex.Replace(startDate, @"[^0-9a-zA-Z-]+", "");
                    }
                    if (endDate !=null)
                    {
                        endDate = Regex.Replace(endDate, @"[^0-9a-zA-Z-]+", "");
                    }
                    

                    db.DbContext.Database.CommandTimeout = 600;
                    string type = "category";
                    string category = "";
                    string cases = "";

                    string sql = @"exec [dbo].[sp_get_report_problem_history] @startdate='{0}', @enddate='{1}', @type = '{2}', @category='{3}', @case='{4}'";
                    string sqlfinal = string.Format(sql, startDate, endDate, type, category, cases);
                    var data = db.DbContext
                                .Database
                                .SqlQuery<SpRProblemHistory>(sqlfinal)
                                .ToList();

                    int totalAllCategory = 0;
                    foreach (var categoriesItem in data)
                    {
                        var categoryItem = new ItemProblem();
                        string sqlcase = string.Format(sql, startDate, endDate, "case", categoriesItem.Category, cases);
                        var dataCase = db.DbContext
                                    .Database
                                    .SqlQuery<SpRProblemHistory>(sqlcase)
                                    .ToList();

                        var listCase = new List<ItemProblem>();
                        var catCase = 1;
                        categoryItem.Id = categoriesItem.Id;
                        categoryItem.Name = categoriesItem.Category;
                        categoryItem.TotalCategory = Convert.ToInt32(categoriesItem.TotalCategory);
                        categoryItem.TotalCategoryPercentage = categoriesItem.TotalCategoryPercentage;

                        foreach (var caseItem in dataCase)
                        {
                            var itemCase = new ItemProblem();
                            string sqlreason = string.Format(sql, startDate, endDate, "reason", categoriesItem.Category, caseItem.Cases);
                            var dataReason = db.DbContext.Database
                                             .SqlQuery<SpRProblemHistory>(sqlreason)
                                             .ToList();

                            string no = "" + categoriesItem.Id + "" + catCase + "";

                            var detailIdx = 1;
                            itemCase.Name = caseItem.Cases;
                            itemCase.Total = Convert.ToInt32(caseItem.Total);
                            itemCase.TotalCases = Convert.ToInt32(caseItem.TotalCases);
                            itemCase.TotalCategoryPercentage = caseItem.TotalCategoryPercentage;
                            itemCase.Id = Convert.ToInt64(no);
                            List<ItemProblem> listItemReason = new List<ItemProblem>();

                            if (dataReason.Count > 0)
                            {
                                foreach (var itemReason in dataReason)
                                {
                                    ItemProblem itemReasonData = new ItemProblem();
                                    string noSub = no + detailIdx;
                                    itemReasonData.TotalReason = Convert.ToInt32(itemReason.TotalCauses);
                                    itemReasonData.Id = Convert.ToInt64(noSub);
                                    itemReasonData.Name = itemReason.Causes;
                                    itemReasonData.Impact = itemReason.Impact;
                                    itemReasonData.Total = Convert.ToInt32(itemReason.Total);
                                    itemReasonData.TotalCases = Convert.ToInt32(itemReason.TotalCases);
                                    itemReasonData.TotalCategoryPercentage = itemReason.TotalCategoryPercentage;
                                    listItemReason.Add(itemReasonData);
                                    detailIdx++;
                                }
                            }

                            itemCase.children = listItemReason;
                            listCase.Add(itemCase);
                            catCase++;
                        }

                        categoryItem.children = listCase;
                        totalAllCategory += Convert.ToInt32(categoriesItem.TotalCategory);
                        rows.Add(categoryItem);
                    }

                    footer1.Name = "Grand Total";
                    footer1.IconCls = "fa fa-circle";
                    footer1.TotalCategory = totalAllCategory;
                    footerList.Add(footer1);

                    footer2.Name = "Result";
                    footer2.IconCls = "fa fa-circle";
                    footer2.TotalCategory = 0;
                    footer2.TotalCategoryPercentage = "EXCELLENT";
                    footerList.Add(footer2);
                    result.rows = rows;
                    result.total = totalAllCategory;
                    result.footer = footerList;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static List<SpRProblemHistory> GetSub(List<SpRProblemHistory> data, long parentId)
        {
            try
            {
                return data
                    .Where(i => i.ParentId == parentId)
                    .Select(i => new SpRProblemHistory
                    {
                        Id = i.Id,
                        ParentId = i.ParentId,
                        Name = i.Causes != "-" ? i.Causes : i.Cases,
                        ReqType = i.ReqType,
                        Category = i.Category,
                        Cases = i.Cases,
                        Causes = i.Causes,
                        Impact = i.Impact,
                        Total = i.Causes != "-" ? i.TotalCauses : i.TotalCases,
                        TotalCauses = i.TotalCauses,
                        TotalCases = i.TotalCases,
                        TotalCategory = i.TotalCategory,
                        TotalCategoryPercentage = i.TotalCategoryPercentage,
                        Children = GetSub(data, i.Id).Count > 0 ? GetSub(data, i.Id) : null
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRProblemHistory> GetProblemHistoryListExport(string startDate, string endDate)
        {
            try
            {
                List<SpRProblemHistory> treeData = GetProblemHistoryList(startDate, endDate);

                List<SpRProblemHistory> result = new List<SpRProblemHistory>();
                foreach (var ct in treeData)
                {
                    var category = new SpRProblemHistory
                    {
                        ReqType = ct.ReqType,
                        Category = ct.Category,
                        TotalCategory = ct.TotalCategory,
                        TotalCategoryPercentage = ct.TotalCategoryPercentage
                    };
                    result.Add(category);

                    foreach (var ca in ct.Children)
                    {
                        var cases = new SpRProblemHistory
                        {
                            Cases = ca.Cases,
                            TotalCases = ca.TotalCases
                        };
                        result.Add(cases);

                        foreach (var cu in ca.Children)
                        {
                            var causes = new SpRProblemHistory
                            {
                                Causes = cu.Causes,
                                Impact = cu.Impact,
                                TotalCauses = cu.TotalCauses
                            };
                            result.Add(causes);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetProblemHistoryStream(List<SpRProblemHistory> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();
                List<ChartData> categoryList = new List<ChartData>();

                #region tabularSheet
                Worksheet tabularSheet = workbook.Worksheets[0];
                int startRow = 4, categoryIdx = 1, nominalTotal = 0;
                for (int i = 0; i < data.Count; i++)
                {
                    tabularSheet.InsertRow(startRow + i);
                    if (data[i].TotalCases == null && data[i].TotalCauses == null)
                    {
                        tabularSheet.SetCellValue(startRow + i, 1, categoryIdx.ToString());
                        CellRange cell = tabularSheet.Range[startRow + i, 2, startRow + i, 5];
                        cell.Merge();
                        cell.Value = data[i].Category;

                        tabularSheet.SetCellValue(startRow + i, 8, data[i].TotalCategory);
                        tabularSheet.SetCellValue(startRow + i, 9, decimal.ToInt32((decimal.Parse(data[i].TotalCategoryPercentage) * 100)).ToString() + "%");

                        nominalTotal += int.Parse(data[i].TotalCategory);
                        categoryList.Add(new ChartData { Name = data[i].Category, Nominal = data[i].TotalCategory, Percentage = data[i].TotalCategoryPercentage });
                        categoryIdx++;
                    }
                    else if (data[i].TotalCategory == null && data[i].TotalCauses == null)
                    {
                        CellRange cell = tabularSheet.Range[startRow + i, 3, startRow + i, 5];
                        cell.Merge();
                        cell.Value = data[i].Cases;

                        tabularSheet.SetCellValue(startRow + i, 7, data[i].TotalCases);
                    }
                    else if (data[i].TotalCases == null && data[i].TotalCategory == null)
                    {
                        tabularSheet.SetCellValue(startRow + i, 4, data[i].Causes);
                        tabularSheet.SetCellValue(startRow + i, 5, data[i].Impact);
                        tabularSheet.SetCellValue(startRow + i, 6, data[i].TotalCauses);
                    }
                }
                #endregion

                Worksheet chartSheet = workbook.Worksheets[1];
                List<ChartCellRange> cellrange = GetReplacedCellsChart(chartSheet);

                if (WriteReplacedCellsChart(categoryList, chartSheet, cellrange, nominalTotal) == string.Empty)
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

        public static List<ChartCellRange> GetReplacedCellsChart(Worksheet worksheet)
        {
            List<ChartCellRange> cellrange = new List<ChartCellRange>();

            try
            {
                for (int i = 1; i <= 50; i++)
                {
                    cellrange.Add(
                        new ChartCellRange
                        {
                            Name = worksheet.FindString("[Name" + i + "]", false, false),
                            Nominal = worksheet.FindString("[Nominal" + i + "]", false, false),
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

        public static string WriteReplacedCellsChart(List<ChartData> data, Worksheet worksheet, List<ChartCellRange> range, int nominalTotalVal)
        {
            string msg = string.Empty;
            try
            {
                CellRange nominalTotal = worksheet.FindString("[NominalTotal]", false, false);
                nominalTotal.Value = nominalTotalVal.ToString();

                CellRange percentageTotal = worksheet.FindString("[PercentageTotal]", false, false);
                percentageTotal.Value = "1";
                percentageTotal.NumberFormat = "0%";

                for (int i = 0; i < 50; i++)
                {
                    if (i < data.Count)
                    {
                        worksheet.Replace(range[i].Name.Value, data[i].Name);
                        worksheet.Replace(range[i].Nominal.Value, data[i].Nominal);

                        range[i].Percentage.Value = data[i].Percentage;
                        range[i].Percentage.NumberFormat = "0%";
                    }
                    else
                    {
                        worksheet.Replace(range[i].Name.Value, string.Empty);
                        worksheet.Replace(range[i].Nominal.Value, string.Empty);
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
    }

    public class ChartCellRange
    {
        public CellRange Name { get; set; }
        public CellRange Nominal { get; set; }
        public CellRange Percentage { get; set; }
    }

    public class ChartData
    {
        public string Name { get; set; }
        public string Nominal { get; set; }
        public string Percentage { get; set; }
    }
}
