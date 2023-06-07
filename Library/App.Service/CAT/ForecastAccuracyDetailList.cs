using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class ForecastAccuracyDetailList
    {
        private const string cacheName = "App.CAT.ForecastAccuracyDetailList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// pengambilan data forcast accuracy detail.
        /// </summary>
        /// <param name="customer_id"></param>
        /// <param name="month_id"></param>
        /// <param name="store_id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static IList<Data.Domain.ForecastAccuracyDetailList> GetList(string customer_id, int? month_id, string store_id, int? year)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.ForecastAccuracyDetailList> forecastaccuracydetail = db.ForecastAccuracyDetailList.ToList();

                if (!string.IsNullOrWhiteSpace(customer_id))
                    forecastaccuracydetail = forecastaccuracydetail.Where(i => (i.Customer.Length >= 10 ? (i.Customer ?? "").Trim().ToUpper().Substring(0, 10) : i.Customer) == (customer_id.Length >= 10 ? customer_id.Trim().ToUpper().Substring(0, 10) : customer_id));

                if (!string.IsNullOrWhiteSpace(store_id))
                    forecastaccuracydetail = forecastaccuracydetail.Where(i => (i.Store ?? "").Trim().ToUpper() == store_id.Trim().ToUpper());

                if (month_id != null)
                    forecastaccuracydetail = forecastaccuracydetail.Where(i => i.Month == month_id);

                if (year != null)
                    forecastaccuracydetail = forecastaccuracydetail.Where(i => i.Year == year);

                return forecastaccuracydetail.ToList();
            }
        }
    }
}
