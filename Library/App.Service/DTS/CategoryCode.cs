using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class CategoryCode
    {
        public const string cacheName = "App.DTS.CategoryCode";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.CategoryCode item, string dml)
        {
            if (dml == "I")
            {
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    if (dml == "I")
                    {
                        return db.CreateRepository<Data.Domain.CategoryCode>().Add(item);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.CategoryCode>().CRUD(dml, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.CategoryCode> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.CategoryCode;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.CategoryCode> GetList(string cat)
        {
            var db = new Data.DTSContext();
            var tb = db.CategoryCode;
            var item = tb.Where(i => i.Category == cat).OrderBy(o => o.Ordering).ToList();
            return item;
        }

        public static List<Data.Domain.CategoryCode> GetList(string cat, string key)
        {
            var db = new Data.DTSContext();
            var tb = db.CategoryCode;
            if (key != null && key != "")
            {
                return tb.Where(i => i.Category == cat && i.Description1.Contains(key)).OrderBy(o => o.Ordering).ToList();
            }
            return tb.Where(i => i.Category == cat).OrderBy(o => o.Ordering).ToList();
        }

        public static Data.Domain.CategoryCode GetByCode(string cat, string code)
        {
            var db = new Data.DTSContext();
            var tb = db.CategoryCode;
            var item = tb.Where(i => i.Category == cat && i.Code == code).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.CategoryCode> GetListAction(string key)
        {
            return GetList("TACT", key);
        }

        public static List<Data.Domain.CategoryCode> GetListStatus(string key)
        {
            return GetList("TSTS", key);
        }

        public static Data.Domain.CategoryCode GetAction(string code)
        {
            return GetByCode("TACT", code);
        }

        public static Data.Domain.CategoryCode GetStatus(string code)
        {
            return GetByCode("TSTS", code);
        }

        public static bool CategoryCodeExist(Data.Domain.CategoryCode model)
        {
            var db = new Data.DTSContext();

            var item = db.CategoryCode
                .Where(i =>
                    (i.Category ?? "").Trim().ToLower() == model.Category.Trim().ToLower() &&
                    (i.Code ?? "").Trim().ToLower() == model.Code.Trim().ToLower()
                ).FirstOrDefault();
            return item != null;
        }
    }
}
