using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterFlowNext
    {
        public const string CacheName = "App.EMCS.FlowNext";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

     
        public static List<Data.Domain.EMCS.MasterFlowNext> GetList(Domain.MasterSearchForm crit)
        {

           using (var db = new Data.EmcsContext())
            {
                // ReSharper disable once UnusedVariable
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.FlowNext;
                return tb.ToList();
            }
        }
        public static Data.Domain.EMCS.MasterFlowNext GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.FlowNext.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterFlowNext itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterFlowNext>().CRUD(dml, itm);
            }
        }

        public static dynamic GetListSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                string sort = crit.Sort ?? "Id";
                var sql = @"[dbo].[sp_get_flow_next] @term='" + crit.Term + "', @IdStatus='" + crit.IdStatus + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpFlowNext>(sql + ", @isTotal=0, @sort='" + sort + "', @order='" + crit.Order + "', @page='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
    }
}
