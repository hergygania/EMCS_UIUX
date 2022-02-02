using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Master
{
    /// <summary>
    /// Services Proses Master SOS.</summary>                
    public class MasterSOS
    {
        public const string cacheName = "App.master.MasterSOS";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master SOS.</summary>
        public static List<Data.Domain.MasterSOS> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.EfDbContext())
            {
                var tb = db.MasterSOS.Where(w => w.IsActive);
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Master SOS with parameter serach.</summary>
        /// <param name="crit"> paramater value on search</param>
        /// <seealso cref="Domain.MasterSearchForm"></seealso>
        public static List<Data.Domain.MasterSOS> GetList(Domain.MasterSearchForm crit)
        {
            var sos = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (sos == "" || (c.SOS.ToString()).Contains(sos)) && c.IsActive
                       orderby c.SOS
                       select c;
            return list.ToList();
        }

        /// <summary>
        /// Get Data Master SOS with paramater int id.</summary>
        /// <param name="id"> id master SOS</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.MasterSOS GetId(int sos)
        {
            var item = GetList().Where(i => i.SOS == sos && i.IsActive).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Get Data Master SOS with paramater int id.</summary>
        /// <param name="id"> id master SOS</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.MasterSOS GetSOSbyID(int id)
        {
            var item = GetList().Where(i => i.ID == id && i.IsActive).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Get Data Master SOS with paramater string code.</summary>
        /// <param name="code"> field code on master SOS</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterSOS GetCode(int code)
        {
            var item = GetList().Where(i => i.SOS == code).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Get Data Master SOS with paramater string code.</summary>
        /// <param name="code"> field code on master SOS</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterSOS GetCodebyID(int id)
        {
            var item = GetList().Where(i => i.ID == id).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Checking data if exist on master SOS.</summary>
        /// <param name="item"> data master SOS</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static string ExistMasterSOS(Data.Domain.MasterSOS item)
        {
            if (item.SOS != null)
            {
                var existitem = Service.CAT.Master.MasterSOS.GetCode(item.SOS);
                if (existitem != null)
                {
                    return existitem.ID.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Proses insert update delete on master SOS</summary>
        /// <param name="item"> data on master SOS</param>
        /// <param name="dml"> flag on insert (I), update (U) and delete (D)</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static int crud(Data.Domain.MasterSOS item, string dml)
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
                return db.CreateRepository<Data.Domain.MasterSOS>().CRUD(dml, item);
            }
        }
    }
}
