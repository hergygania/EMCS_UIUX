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
    public class SvcRSailingEstimation
    {
        public const string CacheName = "App.EMCS.SvcRSailingEstimation";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRSailingEstimation> SailingEstimationList(SpRSailingEstimation filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@origin", filter.OriginCity ?? ""));
                parameterList.Add(new SqlParameter("@destination", filter.DestinationCountry ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRSailingEstimation>(@"[dbo].[SP_RSailingEstimation]  @origin, @destination", parameters).ToList();
                return data;
            }
        }

        public static List<SpRSailingEstimation> GetEstimationListExport(string origin, string destination)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@origin", origin ?? ""));
                    parameterList.Add(new SqlParameter("@destination", destination ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpRSailingEstimation>("exec [dbo].[SP_RSailingEstimation]  @origin, @destination", parameters).ToList();

                    return data;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static MemoryStream GetSailingEstimationStream(List<SpRSailingEstimation> data, string fileExcel)
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
                    
                    tabularSheet.SetCellValue(startRow + i, 1, data[i].DestinationCountry);
                    tabularSheet.SetCellValue(startRow + i, 2, data[i].PortDestination);
                    tabularSheet.SetCellValue(startRow + i, 3, data[i].OriginCity);
                    tabularSheet.SetCellValue(startRow + i, 4, data[i].PortOrigin);
                    tabularSheet.SetCellValue(startRow + i, 5, data[i].ShippingMethod);                    
                    tabularSheet.SetCellValue(startRow + i, 6, data[i].Etd);
                    tabularSheet.SetCellValue(startRow + i, 7, data[i].Eta);
                    tabularSheet.SetCellValue(startRow + i, 8, data[i].Estimation);
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
