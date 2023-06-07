using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class InventoryTrackingStatus
    {
        private const string cacheName = "App.CAT.InventoryTrackingStatus";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory Tracking Status.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.InventoryTrackingStatus> GetList(int InventoryID, string searchkey)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.InventoryTrackingStatus> list = db.InventoryTrackingStatus.ToList();
                return list.Where(i => i.HeaderID == InventoryID && (searchkey == "" || (i.Status.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))).Distinct().ToList();
            }
        }

        public static Data.Domain.InventoryTrackingStatus GetData(Int64 ID)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.InventoryTrackingStatus.Where(w => w.ID == ID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengambilan data TrackingStatus Detail WOC menggunakan Store Procedure.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <returns></returns>
        public static Data.Domain.SP_TrackingStatusDetailWOC SP_TrackingStatusDetailWOC(Int64 ID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ID", ID));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_TrackingStatusDetailWOC>("[cat].[spGetTrackingStatusDetailWOC] @ID", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengambilan data Tracking Status Detail TTC.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <returns></returns>
        public static Data.Domain.SP_TrackingStatusDetailTTC SP_TrackingStatusDetailTTC(Int64 ID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ID", ID));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_TrackingStatusDetailTTC>("[cat].[spGetTrackingStatusDetailTTC] @ID", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengambilan data Tracking Status Detail CMS.
        /// </summary>
        /// <param name="InventoryID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static Data.Domain.TrackingStatusDetailCMS TrackingStatusDetailCMS(Int64 ID, string Status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ID", ID));
                parameterList.Add(new SqlParameter("@Status", Status));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.TrackingStatusDetailCMS>("[cat].[spGetTrackingStatusDetailCMS] @ID, @Status", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// insert update delete data Inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int InsertTrackingStatus(Data.Domain.InventoryTrackingStatus item, string dml)
        {
            item.LastUpdate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.InventoryTrackingStatus>().CRUD(dml, item);
            }
        }

    }
}
