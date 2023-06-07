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
    public class SvcRDheBi
    {
        public const string CacheName = "App.EMCS.SvcRDheBI";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRDheBi> DheBiList(CiplListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var data = db.DbContext.Database.SqlQuery<SpRDheBi>(@"[dbo].[SP_RDheBI]").ToList();
                return data;
            }
        }

        public static List<SpRDheBi> GetDhebiList(DateTime startDate, DateTime endDate, string category, string exportType)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartDate", startDate));
                    parameterList.Add(new SqlParameter("@EndDate", endDate));
                    parameterList.Add(new SqlParameter("@Category", category ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", exportType ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRDheBi>("exec [dbo].[SP_RDheBI] @StartDate, @EndDate, @Category, @ExportType", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<SpRDheBi> GetDhebiList_old(RDheBIListFilter filter)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@StartDate", filter.startDate));
                    parameterList.Add(new SqlParameter("@EndDate", filter.endDate));
                    parameterList.Add(new SqlParameter("@Category", filter.category ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", filter.exportType ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRDheBi>("exec [dbo].[SP_RDheBI] @StartDate, @EndDate, @Category, @ExportType", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetDhebiStream(List<SpRDheBi> data, string fileExcel)
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

                    tabularSheet.SetCellValue(startRow + i, 1, data[i].NomorIdentifikasi);
                    tabularSheet.SetCellValue(startRow + i, 2, data[i].Npwp);
                    tabularSheet.SetCellValue(startRow + i, 3, data[i].NamaPenerimaDhe);
                    tabularSheet.SetCellValue(startRow + i, 4, data[i].SandiKantorPabean);
                    tabularSheet.SetCellValue(startRow + i, 5, data[i].NomorPendaftaranPeb);
                    tabularSheet.SetCellValue(startRow + i, 6, data[i].TanggalPeb);
                    tabularSheet.SetCellValue(startRow + i, 7, data[i].JenisValutaDhe);
                    tabularSheet.SetCellValue(startRow + i, 8, data[i].NilaiDhe);
                    tabularSheet.SetCellValue(startRow + i, 9, data[i].NilaiPeb);
                    tabularSheet.SetCellValue(startRow + i, 10, data[i].SandiKeterangan);
                    tabularSheet.SetCellValue(startRow + i, 11, data[i].KelengkapanDokumen);
                    tabularSheet.SetCellValue(startRow + i, 12, data[i].JenisValutaPeb);
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
