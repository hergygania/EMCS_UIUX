using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Report
{
    /// <summary>
    /// Services Proses Report Outstanding Old Core Summary.</summary>
    public class RptOutstandingOldCoreSummaryList
    {
        private const string cacheName = "App.CAT.RptOutstandingOldCoreSummaryList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Report Outstanding Old Core Summary By StoreID and LocationID.</summary>
        public static IList<Data.Domain.RptOutstandingOldCoreSummaryList> GetList(string StoreID, string DateFilter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Store", StoreID ?? ""));
                parameterList.Add(new SqlParameter("@DateFilter", DateFilter ?? ""));      
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.RptOutstandingOldCoreSummaryList>("[cat].[spGetDataRptOutstandingOldCoreSummary] @Store, @DateFilter", parameters).ToList();

                return data;
            }
        }
    }
}
