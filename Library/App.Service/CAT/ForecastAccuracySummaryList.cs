using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class ForecastAccuracySummaryList
    {
        private const string cacheName = "App.CAT.ForecastAccuracySummaryList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// pemnagmbilan data Forecast Accuracy Summary.
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="month_id"></param>
        /// <param name="store_id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IList<Data.Domain.ForecastAccuracySummaryList> GetList(string customer_id, int? month_id, string store_id, int? year)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Customer", customer_id ?? ""));
                parameterList.Add(new SqlParameter("@Month", month_id ?? 0));
                parameterList.Add(new SqlParameter("@Store", store_id ?? ""));
                parameterList.Add(new SqlParameter("@Year", year ?? 0));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ForecastAccuracySummaryList>("[cat].[spGetRptForecastSummary] @Customer, @Month, @Store, @Year", parameters).ToList();

                return data;
            }
        }
    }
}
