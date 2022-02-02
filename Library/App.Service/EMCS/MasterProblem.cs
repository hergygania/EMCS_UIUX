using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterProblemCategory
    {
        public const string CacheName = "App.EMCS.MasterProblem";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterProblemCategory> GetProblemList()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterProblem;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterProblemCategory> GetProblemList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterProblem.Where(a => a.Case.Contains(search) || a.Category.Contains(search)).AsQueryable().ToList();
                return tb;
            }
        }

        public static Data.Domain.EMCS.MasterProblemCategory GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterProblem.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterProblemCategory itm, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                if (dml == "I")
                {
                    itm.CreateBy = Domain.SiteConfiguration.UserName;
                    itm.CreateDate = DateTime.Now;
                }

                itm.UpdateBy = Domain.SiteConfiguration.UserName;
                itm.UpdateDate = DateTime.Now;

                CacheManager.Remove(CacheName);

                return db.CreateRepository<Data.Domain.EMCS.MasterProblemCategory>().CRUD(dml, itm);
            }
        }

        public static List<MasterStatus> StatusList()
        {
            List<MasterStatus> listStat = new List<MasterStatus>();
            listStat.Add(new MasterStatus() { Value = true, Text = "No" });
            listStat.Add(new MasterStatus() { Value = false, Text = "Yes" });
            return listStat;
        }
    }
}
