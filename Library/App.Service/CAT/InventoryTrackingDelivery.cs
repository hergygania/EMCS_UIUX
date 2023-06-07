using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class InventoryTrackingDelivery
    {
        private const string cacheName = "App.CAT.InventoryTrackingDelivery";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory Tracking Delivery.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.SP_DataInventoryTrackingDelivery> GetList(int InventoryID, string searchkey)
        {
            //using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            //{
            //    IEnumerable<Data.Domain.InventoryTrackingDelivery> list = db.CreateRepository<Data.Domain.InventoryTrackingDelivery>().Table.ToList();
            //    return list.Where(i => i.InventoryID == InventoryID && (searchkey == "" || (i.DANo.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))).ToList();
            //}

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@InventoryID", InventoryID));
                parameterList.Add(new SqlParameter("@searchkey", searchkey ?? ""));  
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventoryTrackingDelivery>("[cat].[spGetDataTrackingDelivery] @InventoryID, '', @searchkey, 1", parameters).ToList();

                return data;
            }
        }

        public static Data.Domain.SP_DataInventoryTrackingDelivery GetData(int InventoryID, string DANumber)
        {    
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@InventoryID", InventoryID));
                parameterList.Add(new SqlParameter("@searchkey", ""));
                parameterList.Add(new SqlParameter("@DANumber", DANumber ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventoryTrackingDelivery>("[cat].[spGetDataTrackingDelivery] @InventoryID, @DANumber, @searchkey, 2", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Get Data Inventory Tracking Delivery Detail.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.SP_DataInventoryTrackingDeliveryDetail> GetListDetail(string DaNumber)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@DaNumber", DaNumber ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventoryTrackingDeliveryDetail>("[cat].[spGetDataTrackingDeliveryDetail] @DaNumber", parameters).ToList();

                return data;
            }
        }
    }
}
