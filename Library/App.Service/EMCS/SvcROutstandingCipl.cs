using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using Spire.Xls;
using System.IO;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcROutstandingCipl
    {
        public const string CacheName = "App.EMCS.SvcROutstandingCipl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpROutstandingCipl> OutstandingCiplList(CiplListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var data = db.DbContext.Database.SqlQuery<SpROutstandingCipl>(@"[dbo].[SP_ROutstandingCipl]").ToList();
                return data;
            }
        }

        public static List<SpROutstandingCipl> GetOutstandingCiplList(DateTime startDate, DateTime endDate)
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
                    var data = db.DbContext.Database.SqlQuery<SpROutstandingCipl>("exec [dbo].[SP_ROutstandingCipl] @StartDate, @EndDate", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetROutstandingCiplStream(List<SpROutstandingCipl> data, string fileExcel)
        {
            try
            {
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(fileExcel);
                workbook.ConverterSetting.SheetFitToPage = true;

                MemoryStream output = new MemoryStream();

                #region tabularSheet
                Worksheet tabularSheet = workbook.Worksheets[0];
                int startRow = 3;
                for (int i = 0; i < data.Count; i++)
                {
                    tabularSheet.InsertRow(startRow + i);

                    tabularSheet.SetCellValue(startRow + i, 1, data[i].Cycle);
                    tabularSheet.SetCellValue(startRow + i, 2, data[i].PicName);
                    tabularSheet.SetCellValue(startRow + i, 3, data[i].Department);
                    tabularSheet.SetCellValue(startRow + i, 4, data[i].Branch);
                    tabularSheet.SetCellValue(startRow + i, 5, data[i].CiplNo);
                    tabularSheet.SetCellValue(startRow + i, 6, data[i].SubmitDate);
                }
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
