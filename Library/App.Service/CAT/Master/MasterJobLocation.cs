using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Master
{
    /// <summary>
    /// Services Proses Master Job Location.</summary>                
    public class MasterJobLocation
    {
        public const string cacheName = "App.master.MasterJobLocation";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master Job Location</summary>
        public static List<Data.Domain.MasterJobLocation> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.EfDbContext())
            {
                var tb = db.MasterJobLocation.Where(e => e.IsActive);
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Master Job Location</summary>
        /// <param name="crit"> id master job Location</param>
        /// <seealso cref="Domain.MasterSearchForm"></seealso>
        public static List<Data.Domain.MasterJobLocation> GetList(Domain.MasterSearchForm crit)
        {
            var JobLocation = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (JobLocation == "" || (c.JobLocation).Trim().ToLower().Contains(JobLocation)) && c.IsActive
                       orderby c.JobLocation
                       select c;
            return list.ToList();
        }

        /// <summary>
        /// Get Data Master Job Location by id</summary>
        /// <param name="id"> id master job Location</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.MasterJobLocation GetId(int id)
        {
            var item = GetList().Where(i => i.ID == id && i.IsActive).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Get Data Master Job Location</summary>
        /// <param name="code"> field code on master job Location</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterJobLocation GetCode(string code)
        {
            var item = GetList().Where(i => i.JobLocation.Trim().ToLower() == code.Replace(" ", "").ToLower()).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Check data if exist on master job location</summary>
        /// <param name="item"> data master job Location</param>
        /// <seealso cref="Data.Domain.MasterJobLocation"></seealso>
        public static string ExistMasterJobLocation(Data.Domain.MasterJobLocation item)
        {
            if (item.JobLocation != null)
            {
                var existitem = Service.CAT.Master.MasterJobLocation.GetCode(item.JobLocation.Trim().ToLower());
                if (existitem != null)
                {
                    return existitem.ID.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Proses insert update delete on master job location</summary>
        /// <param name="item"> data on master job location</param>
        /// <param name="dml"> flag on insert (I), update (U) and delete (D)</param>
        /// <seealso cref="Data.Domain.MasterJobLocation"></seealso>
        public static int crud(Data.Domain.MasterJobLocation item, string dml)
        {
            if (dml == "I")
            {
                item.EntryBy = Domain.SiteConfiguration.UserName;
                item.EntryDate = DateTime.Now;
            }

            item.ModifiedBy = Domain.SiteConfiguration.UserName;
            item.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.MasterJobLocation>().CRUD(dml, item);
            }
        }
    }
}
