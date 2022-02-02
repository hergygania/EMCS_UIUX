using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterFlow
    {
        public const string CacheName = "App.EMCS.Flow";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic GetListSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                string sort = crit.Sort ?? "Id";
                var sql = @"[dbo].[sp_get_flow] @term='" + crit.Term + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpFlow>(sql + ", @isTotal=0, @sort='" + sort + "', @order='" + crit.Order + "', @page='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static Data.Domain.EMCS.MasterFlow GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Flow.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterFlow GetDataByName(string name, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Flow.Where(a => a.Name == name);
                if (id != 0)
                {
                    data = data.Where(a => a.Id != id);
                }
                return data.FirstOrDefault();
            }
        }

        public static List<MasterFlow> GetAllFlow()
        {
            using (var db = new Data.EmcsContext())
            {
                List<MasterFlow> result = db.Database.SqlQuery<MasterFlow>("SELECT Id, Name, Type FROM dbo.Flow").ToList();
                return result;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterFlow itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterFlow>().CRUD(dml, itm);
            }
        }
    }
}
