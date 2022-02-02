using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace App.Service.EMCS
{
    public class MasterAreaUserCkb
    {
        public const string CacheName = "App.EMCS.MasterAreaUserCKB";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.SpAreaUserCkb> GetAreaUserCkbList(App.Data.Domain.EMCS.AreaUserCkbListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Name", filter.BAreaName ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpAreaUserCkb>(@"[dbo].[sp_get_areaUserCKB] @Name", parameters).ToList();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterAreaUserCkb GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterAreaUserCkb.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterAreaUserCkb item, string dml)
        {
            if (dml == "I")
            {
                item.Id = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                if (dml == "I")
                {
                    db.CreateRepository<Data.Domain.EMCS.MasterAreaUserCkb>().Add(item);
                    return Convert.ToInt32(item.Id);
                }
                else
                {
                    return db.CreateRepository<Data.Domain.EMCS.MasterAreaUserCkb>().CRUD(dml, item);
                }
            }
        }
    }
}
