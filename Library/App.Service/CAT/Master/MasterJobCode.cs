using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Master
{

    /// <summary>
    /// Services Proses Master Job Code.</summary>                
    public class MasterJobCode
    {
        public const string cacheName = "App.master.MasterJobCode";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master Job Code.</summary>
        public static List<Data.Domain.MasterJobCode> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.EfDbContext())
            {
                var tb = db.MasterJobCode.Where(e => e.IsActive);
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Master Job Code with parameter serach.</summary>
        /// <param name="crit"> paramater value on search</param>
        /// <seealso cref="Domain.MasterSearchForm"></seealso>
        public static List<Data.Domain.MasterJobCode> GetList(Domain.MasterSearchForm crit)
        {
            var JobCode = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (JobCode == "" || (c.JobCode).Trim().ToLower().Contains(JobCode)) && c.IsActive
                       orderby c.JobCode
                       select c;
            return list.ToList();
        }

        /// <summary>
        /// Get Data Master Job Code with paramater int id.</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.MasterJobCode GetId(int id)
        {
            var item = GetList().Where(i => i.ID == id && i.IsActive).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Get Data Master Job Code with paramater string code.</summary>
        /// <param name="code"> field code on master job code</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterJobCode GetCode(string code)
        {
            var item = GetList().Where(i => i.JobCode.Trim().ToLower() == code.Replace(" ", "").ToLower()).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Checking data if exist on master job code.</summary>
        /// <param name="item"> data master job code</param>
        /// <seealso cref="Data.Domain.MasterJobCode"></seealso>
        public static string ExistMasterJobCode(Data.Domain.MasterJobCode item)
        {
            if (item.JobCode != null)
            {
                var existitem = Service.CAT.Master.MasterJobCode.GetCode(item.JobCode.Trim().ToLower());
                if (existitem != null)
                {
                    return existitem.ID.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Proses insert update delete on master job code</summary>
        /// <param name="item"> data on master job code</param>
        /// <param name="dml"> flag on insert (I), update (U) and delete (D)</param>
        /// <seealso cref="Data.Domain.MasterJobCode"></seealso>
        public static int crud(Data.Domain.MasterJobCode item, string dml)
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
                return db.CreateRepository<Data.Domain.MasterJobCode>().CRUD(dml, item);
            }
        }
    }
}
