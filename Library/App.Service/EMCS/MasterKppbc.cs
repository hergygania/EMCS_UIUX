using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace App.Service.EMCS
{
    public class MasterKppbc
    {
        public const string CacheName = "App.EMCS.MasterKppbc";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.SpGetListAllKppbc> GetKppbcList(App.Data.Domain.EMCS.KppbcListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Name", filter.SearchName ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpGetListAllKppbc>(@"[dbo].[getListAllKppbc] @Name", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.MasterKppbc> GetSelectOption(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterKppbc.Where(a => a.Name.Contains(search) || a.Propinsi.Contains(search));
                return tb.ToList();
            }
        }

        public static Data.Domain.EMCS.MasterKppbc GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterKppbc.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterKppbc item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
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

                if (dml == "I")
                {
                    db.CreateRepository<Data.Domain.EMCS.MasterKppbc>().Add(item);
                    return Convert.ToInt32(item.Id);
                }
                else
                {
                    return db.CreateRepository<Data.Domain.EMCS.MasterKppbc>().CRUD(dml, item);
                }
            }
        }
    }
}
