using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterFlowStep
    {
        public const string CacheName = "App.EMCS.FlowStep";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static int GetTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var offset = crit.Offset;
                var limit = crit.Limit == 0 ? 5 : crit.Limit;
                var sort = crit.Sort ?? "Id";
                var order = crit.Order ?? "ASC";
                var idFlow = crit.IdFlow;
                var term = crit.Term ?? "";
                int total = 1;

                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_flow_step] @page='" + offset + "', @limit='" + limit + "', @sort='" + sort + "', @order='" + order + "', @IdFlow='" + idFlow + "', @term='" + term + "', @isTotal='" + total + "'").FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            throw new InvalidOperationException();
        }

        public static List<SpFlowStep> GetList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var offset = crit.Offset;
                var limit = crit.Limit == 0 ? 5 : crit.Limit;
                var sort = crit.Sort ?? "Id";
                var order = crit.Order ?? "ASC";
                var idFlow = crit.IdFlow;
                var term = crit.Term ?? "";
                int total = crit.Total ? 1 : 0;

                var tb = db.Database.SqlQuery<SpFlowStep>("[dbo].[sp_get_flow_step] @page='" + offset + "', @limit='" + limit + "', @sort='" + sort + "', @order='" + order + "', @IdFlow='" + idFlow + "', @term='" + term + "', @isTotal='" + total + "'");
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterFlowStep GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.FlowStep.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterFlowStep itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterFlowStep>().CRUD(dml, itm);
            }
        }

        public static dynamic GetListSp(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                string sort = crit.Sort ?? "Id";
                var sql = @"[dbo].[sp_get_flow_step] @term='" + crit.Term + "', @IdFlow='" + crit.IdFlow + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<SpFlowStep>(sql + ", @isTotal=0, @sort='" + sort + "', @order='" + crit.Order + "', @page='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
    }
}
