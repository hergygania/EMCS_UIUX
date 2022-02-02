using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using App.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcRequestGr
    {
        public const string CacheName = "App.EMCS.SvcRequestGr";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRequestGr> GetList(GridListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Username", filter.Username ?? ""));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                parameterList.Add(new SqlParameter("@sort", "Id"));
                parameterList.Add(new SqlParameter("@order", "ASC"));
                parameterList.Add(new SqlParameter("@offset", filter.Offset));
                parameterList.Add(new SqlParameter("@limit", filter.Limit));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRequestGr>(@"exec [dbo].[sp_get_task_gr]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                return data;
            }
        }

        public static int GetTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var username = crit.Username ?? "";

                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_task_gr]@Username='" + username + "', @isTotal='1'").FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            return 0;
        }

        public static RequestGr GetRequestById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.RequestGrs.FirstOrDefault(a => a.Id == id);
                return data;
            }
        }

        public static RequestGr GetRequestByGrId(long id)
        {
            var idGr = Convert.ToString(id);
            using (var db = new Data.EmcsContext())
            {
                var data = db.RequestGrs.FirstOrDefault(a => a.IdGr == idGr);
                return data;
            }
        }

        public static int Crud(RequestGr itm, string dml)
        {
            if (dml == "I")
            {
                itm.CreateBy = SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return db.CreateRepository<RequestGr>().CRUD(dml, itm);
            }
        }
    }
}
