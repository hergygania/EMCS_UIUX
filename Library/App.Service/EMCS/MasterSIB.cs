using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.IO;
using Spire.Xls;
using System.Data;
using System.Configuration;

namespace App.Service.EMCS
{
    public class MasterSib
    {
        public const string CacheName = "App.EMCS.MasterSIB";
          
        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterSib> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterSib.Where(a => a.Description.Contains(search) || a.DlrWO.Contains(search));
                return tb.ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterSib> GetSibList()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterSib;
                return tb.ToList();
            }
        }

        public static List<MasterSib> ProcessSib()
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                var data = db.DbContext.Database.SqlQuery<MasterSib>("exec [dbo].[SP_Process_SIB]").ToList();

                return data;

            }
        }

        public static MemoryStream GetSibStream(List<Data.Domain.EMCS.MasterSib> data, string fileExcel)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(fileExcel);
            workbook.ConverterSetting.SheetFitToPage = true;

            MemoryStream output = new MemoryStream();

            #region tabularSheet
            Worksheet tabularSheet = workbook.Worksheets[0];
            int startRow = 2;
            for (int i = 0; i < data.Count; i++)
            {
                tabularSheet.InsertRow(startRow + i);

                //tabularSheet.SetCellValue(startRow + i, 1, data[i].ReqDate.ToString());
                tabularSheet.SetCellValue(startRow + i, 1, data[i].ReqNumber);
                tabularSheet.SetCellValue(startRow + i, 2, data[i].DlrWO);
                tabularSheet.SetCellValue(startRow + i, 3, data[i].DlrClm);
                tabularSheet.SetCellValue(startRow + i, 4, data[i].SvcClm);
                tabularSheet.SetCellValue(startRow + i, 5, data[i].PartNo);
                tabularSheet.SetCellValue(startRow + i, 6, data[i].SerialNumber);
                tabularSheet.SetCellValue(startRow + i, 7, data[i].Description);
                tabularSheet.SetCellValue(startRow + i, 8, data[i].DlrCode);
                tabularSheet.SetCellValue(startRow + i, 9, data[i].UnitPrice.ToString());
                tabularSheet.SetCellValue(startRow + i, 10, data[i].Currency);
            }
            #endregion

            workbook.SaveToStream(output, FileFormat.Version97to2003);

            return output;
        }

        public static Data.Domain.EMCS.MasterSib GetDataById(string dlrWo)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterSib.Where(a => a.DlrWO == dlrWo).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterSib itm, string dml)
        {
            if (dml == "I")
            {
                itm.CreateBy = Domain.SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = Domain.SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return db.CreateRepository<Data.Domain.EMCS.MasterSib>().CRUD(dml, itm);
            }
        }

        public static bool InsertBulk(string tempTableName, DataTable dt, int count)
        {
            var db = new Data.EmcsContext();
            var emcsConnection = ConfigurationManager.ConnectionStrings["emcsConnection"].ConnectionString;

            db.Database.ExecuteSqlCommand("TRUNCATE TABLE " + tempTableName);

            using (SqlConnection cn = new SqlConnection(@emcsConnection))
            {
                cn.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                {
                    for (var i = 0; i <= count; i++)
                    {
                        copy.ColumnMappings.Add(i, i);
                    }
                    copy.DestinationTableName = tempTableName;
                    copy.WriteToServer(dt);
                }
                cn.Close();    
            }
            return true;
        }
    }
}
