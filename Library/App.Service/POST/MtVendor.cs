using App.Data.Caching;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.POST
{
    public static class MtVendor
    {
        public const string CacheName = "App.POST.MtVendor";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.POST.MtVendor> GetData(PaginationParam param)
        {
            param.Page = 0;
            param.Skip = 10;
            param.Pagesize = 10;
            
            using (var db = new Data.POSTContext())
            {
                var tb = db.MtVendor.Take(param.Pagesize);
                return tb.ToList();
            }
        }

        public static Data.Domain.POST.MtVendor GetVendorById(long Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.MtVendor.Where(a=>a.Id == Id);
                return tb.FirstOrDefault();
            }
        }

        public static Data.Domain.POST.MtVendor GetVendorByName(string Name)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.MtVendor.Where(a => a.Name == Name);
                return tb.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.POST.MtVendor itm, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                if (dml == Global.dmlinsert)
                {
                    itm.CreateBy = SiteConfiguration.UserName;
                    itm.CreateDate = DateTime.Now;
                    
                }
                else
                {
                    itm.UpdateBy = SiteConfiguration.UserName;
                    itm.UpdateDate = DateTime.Now;
                }



                CacheManager.Remove(CacheName);

                return db.CreateRepository<Data.Domain.POST.MtVendor>().CRUD(dml, itm);
            }
        }
    }
}
