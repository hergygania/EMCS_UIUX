using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterRegulation
    {
        public const string CacheName = "App.EMCS.MasterRegulation";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterRegulation> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterRegulation.Where(a => a.Instansi.Contains(search) || a.Nomor.Contains(search) || a.RegulationType.Contains(search) || a.Category.Contains(search) || a.RegulationNo.Contains(search) || a.Description.Contains(search));
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterRegulation GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterRegulation.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterRegulation GetDataByNumber(string number, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterRegulation.Where(a => a.Nomor == number);
                if (id != 0)
                {
                    data = data.Where(a => a.Id != id);
                }
                return data.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterRegulation itm, string dml)
        {
            if (dml == "I")
            {
                itm.CreateBy = Domain.SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = Domain.SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return db.CreateRepository<Data.Domain.EMCS.MasterRegulation>().CRUD(dml, itm);
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
