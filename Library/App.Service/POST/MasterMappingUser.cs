using App.Data.Caching;
using App.Domain;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace App.Service.POST
{
    public static class MasterMappingUser
    {
        public const string CacheName = "App.POST.MasterMappingUser";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.POST.MtMappingUserBranch> GetData()
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.MtMappingUserBranch.ToList();
                return tb;
            }
        }

        public static Data.Domain.POST.MtMappingUserBranch GetMappingUserById(long Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.MtMappingUserBranch.Where(a => a.ID == Id);
                return tb.FirstOrDefault();
            }
        }

        public static int Crud(Data.Domain.POST.MtMappingUserBranch itm, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                CacheManager.Remove(CacheName);

                if (dml == "I")
                {
                    using (var db2 = new Data.POSTContext())
                    {
                        var isExist = db2.MtMappingUserBranch.FirstOrDefault(a => a.UserID == itm.UserID && a.NPWP == itm.NPWP);
                        if (isExist != null)
                            return -1;
                    }
                }

                return db.CreateRepository<Data.Domain.POST.MtMappingUserBranch>().CRUD(dml, itm);
            }
        }

        public static List<Select2Result> GetSelectUser(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_User_SELECT] @Search", parameters).ToList();
                return data;
            }
        }
    }
}
