using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterIncoterms
    {
        public const string CacheName = "App.EMCS.MasterIncoterms";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterIncoterms> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterIncoTerm.Where(a => a.Description.Contains(search) || a.Number.Contains(search));
                return tb.ToList();
            }
        }

        public static List<SelectItem2> GetSelectList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterIncoTerm.Select(i => new
                {
                    id = (int)i.Id,
                    text = i.Number
                }).ToList();

                return data.Select(i => new SelectItem2
                {
                    Id = i.text,
                    Text = i.text
                }).ToList();
            }
        }

        public static dynamic GetSelectOption(Domain.MasterSearchForm crit)
        {
            var search = crit.searchName ?? "";
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterIncoTerm
                    .Where(a => a.IsDeleted == false && (a.Number.Contains(search) || a.Description.Contains(search)))
                    .OrderBy(a => a.Number)
                    .Skip(0).Take(100)
                    .AsQueryable()
                    .ToList();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterIncoterms GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterIncoTerm.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterIncoterms GetDataByNumber(string number, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterIncoTerm.Where(a => a.Number == number);
                if (id != 0)
                {
                    data = data.Where(a => a.Id != id);
                }
                return data.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterIncoterms itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterIncoterms>().CRUD(dml, itm);
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
