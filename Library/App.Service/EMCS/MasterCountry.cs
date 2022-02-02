using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterCountry
    {
        public const string CacheName = "App.EMCS.MasterCountry";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int Crud(Data.Domain.EMCS.MasterCountry item, string dml)
        {
            if (dml == "I")
            {
                item.Id = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                if (dml == "I")
                {
                    db.CreateRepository<Data.Domain.EMCS.MasterCountry>().Add(item);
                    return Convert.ToInt32(item.Id);
                }
                else
                {
                    return db.CreateRepository<Data.Domain.EMCS.MasterCountry>().CRUD(dml, item);
                }
            }
        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.EMCS.MasterCountry> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
<<<<<<< HEAD
                var tb = db.MasterCountry.Where(a => a.Description.Contains(search) || a.CountryCode.Contains(search) && !a.IsDeleted);
=======
                var tb = db.MasterCountry.Where(a => a.Description.Contains(search) || a.CountryCode.Contains(search));
>>>>>>> 639d8d0 (Intial commit)
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterCountry GetCountry(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterCountry.Where(a => a.Id == id).FirstOrDefault();
                return tb;
            }
        }
    }
}
