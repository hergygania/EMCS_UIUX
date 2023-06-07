using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterFlowStatus
    {
        public const string CacheName = "App.EMCS.FlowStatus";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterFlowStatus> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.FlowStatus.Where(a => a.Status.Contains(search));
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterFlowStatus GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.FlowStatus.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterFlowStatus GetDataByName(string name, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.FlowStatus.Where(a => a.Status == name);
                if (id != 0)
                {
                    data = data.Where(a => a.Id != id);
                }
                return data.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterFlowStatus itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterFlowStatus>().CRUD(dml, itm);
            }
        }

        public static dynamic GetListSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                string sort = crit.Sort ?? "Id";
                var sql = @"[dbo].[sp_get_flow_status] @term='" + crit.Term + "', @IdStep='" + crit.IdStep + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpFlowStatus>(sql + ", @isTotal=0, @sort='" + sort + "', @order='" + crit.Order + "', @page='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static List<Data.Domain.EMCS.MasterFlowNext> GetDataByStatusId(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.FlowNext.Where(a => a.IdStatus == id).ToList();
                return data;
            }
        }
    }
}
