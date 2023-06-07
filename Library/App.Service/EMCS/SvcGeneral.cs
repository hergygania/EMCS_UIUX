using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Dynamic;
using App.Data.Domain.EMCS;
using App.Domain;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{
    public class SvcGeneral
    {
        public const string CacheName = "App.EMCS.SvcGeneral";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic ProblemList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_get_problem_history_list] @id = '" + crit.Id + "', @Type='" + crit.Cat + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<SpGetProblemList>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static dynamic DocumentList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_get_document_list] @id = '" + crit.Id + "', @category='" + crit.Cat + "' ";
                var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<SpGetDocumentList>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static dynamic CiplDocumentList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_get_cipl_document_list] @IdCipl = '" + crit.IdCipl + "'";
                var data = db.Database.SqlQuery<SPGetCiplDocumentList>(sql).ToList();
                
                return data;
            }
        }

        public static List<PlantBusiness> GetPlantList(MasterSearchForm crit)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@PlantCode", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<PlantBusiness>(@"exec [dbo].[sp_get_cipl_businessarea_list]@PlantCode", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.MasterArea> GetBareaList(MasterSearchForm crit)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var dataQuery = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.MasterArea>(@"select * from dbo.MasterArea where BAreaCode like '%" + search + "%' OR BAreaName like '%" + search + "%'").ToList();
                return dataQuery;
            }
        }

        public static List<Data.Domain.EMCS.MasterKppbc> GetKppbc(MasterSearchForm crit)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var dataQuery = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.MasterKppbc>(@"select * from dbo.MasterArea where BAreaCode like '%" + search + "%' OR BAreaName like '%" + search + "%'").ToList();
                return dataQuery;
            }
        }

        public static List<MasterBranchCkb> GetSelectOption(MasterBranchCkb crit)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
               
                db.DbContext.Database.CommandTimeout = 600;
                var search = (String.IsNullOrEmpty(crit.Name) || crit.Name == "null") ? "" : crit.Name;
                if (search != null)
                {
                    search = Regex.Replace(search, @"[^0-9a-zA-Z]+", "");
                }
            

                var dataQuery = db.DbContext.Database.SqlQuery<MasterBranchCkb>(@"select * from dbo.MasterBranchCkb where Name like '%" + search + "%' OR Address like '%" + search + "%' OR City like '%" + search + "%'").ToList();
                return dataQuery;
            }
        }

        public static List<StringData> GetVehicleType(string term = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var where = (term != "" ? " WHERE VehicleType like '%" + term + "%'" : "");
                var sql = @"SELECT DISTINCT VehicleType text From dbo.GoodsReceive" + where;
                var dataQuery = db.DbContext.Database.SqlQuery<StringData>(sql).ToList();
                return dataQuery;
            }
        }

        public static List<StringData> GetVehicleMerk(string term = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var where = (term != "" ? " WHERE VehicleMerk like '%" + term + "%'" : "");
                var sql = @"SELECT DISTINCT VehicleMerk text From dbo.GoodsReceive" + where;
                var dataQuery = db.DbContext.Database.SqlQuery<StringData>(sql).ToList();
                return dataQuery;
            }
        }


        public static List<SpDelegationTo> GetCurrentNextSuperior()
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                var currentUser = SiteConfiguration.UserName;
                var sql = @"sp_get_next_superior '" + currentUser + "'";
                var data = db.DbContext.Database.SqlQuery<SpDelegationTo>(sql).ToList();
                return data;
            }
        }
    }
}
