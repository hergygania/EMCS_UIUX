using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Domain;
using System.Dynamic;
using System.Text.RegularExpressions;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{
    public class MasterSuperior
    {
        public const string CacheName = "App.EMCS.MasterSuperior";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.SPSuperior> GetSuperiorList(App.Data.Domain.EMCS.SuperiorListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@EmployeeUsername", filter.Username ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SPSuperior>(@"[dbo].[sp_get_Superior] @EmployeeUsername", parameters).ToList();
                return data;
            }
        }

        public static Data.Domain.EMCS.MasterSuperior GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterSuperior.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static List<SpGetListAllUser> GetUserList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var tb = db.DbContext.Database.SqlQuery<SpGetListAllUser>(@"exec [dbo].[SP_getListAllEmployee]");
                return tb.ToList();
            }
        }

        public static long CrudSp(Data.Domain.EMCS.MasterSuperior itm, string dml)
        {
            itm.IsDeleted = false;

            if (dml == "I")
            {
                itm.Id = 0;
                itm.CreateBy = Domain.SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            if (dml == "D")
                itm.IsDeleted = true;

            itm.UpdateBy = Domain.SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {

                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();

                    var sql = @"[dbo].[sp_insert_update_superior]
                                        @Id = '" + itm.Id + "'," +
                                        "@EmployeeUsername = '" + itm.EmployeeUsername + "'," +
                                        "@SuperiorUsername='" + itm.SuperiorUsername + "'," +
                                        "@Isdelete='" + itm.IsDeleted + "'," +
                                        "@UpdateBy ='" + itm.UpdateBy + "'," +
                                        "@UpdateDate ='" + itm.UpdateDate + "'," +
                                        "@CreateBy ='" + itm.CreateBy + "'," +
                                        "@CreateDate='" + itm.CreateDate + "'";

                    var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(sql).FirstOrDefault();
                    return 1;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            return 0;
        }

    }
}
