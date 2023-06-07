using App.Data.Caching;
using System;
using System.Linq;
using App.Domain;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcUserLog
    {
        public const string CacheName = "App.EMCS.UserLog";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static int Crud()
        {
            string dml = "I";
            var itm = new UserLog();
            itm.TotalVisit = 0;
            using (var db = new Data.EmcsContext())
            {
                var data = db.UserLog.Where(a => a.Username == SiteConfiguration.UserName);
                if (data.Count() > 0)
                {
                    itm = data.FirstOrDefault();
                    if (itm != null && itm.LastVisit.ToString("dd MMM yyyy") != DateTime.Now.ToString("dd MMM yyyy"))
                    {
                        itm.TotalVisit = itm.TotalVisit + 1;
                    }
                    dml = "U";
                    if (itm != null) itm.Id = itm.Id;
                }
                else
                {
                    itm.TotalVisit = itm.TotalVisit + 1;
                }
            }

            if (itm != null)
            {
                itm.TotalVisit = itm.TotalVisit;
                itm.Username = SiteConfiguration.UserName;
                itm.LastVisit = DateTime.Now;


                CacheManager.Remove(CacheName);

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    return db.CreateRepository<UserLog>().CRUD(dml, itm);
                }
            }

            return 0;
        }

        public static int GetTotalVisitor()
        {
            int total;
            using (var db = new Data.EmcsContext())
            {
                total = Convert.ToInt32(db.UserLog.Where(a => a.LastVisit.Year == DateTime.Now.Year).Sum(a => a.TotalVisit));
                return total;
            }
        }

        public static DateTime GetLastVisit()
        {
           using (var db = new Data.EmcsContext())
            {
                var data = db.UserLog.Where(a => a.Username == SiteConfiguration.UserName).FirstOrDefault();
                // ReSharper disable once PossibleNullReferenceException
                return data.LastVisit;
            }
        }

        public static UserData GetUserDetail()
        {
            using (var db = new Data.EmcsContext())
            {
                var username = SiteConfiguration.UserName;
                var data = db.Database.SqlQuery<UserData>("select Employee_ID, Employee_Name, [Group] from dbo.fn_get_employee_internal_ckb() where AD_User = '" + username + "'").FirstOrDefault();
                return data;
            }
        }
    }
}
