using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using System;
using App.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcRequestCl
    {
        public const string CacheName = "App.EMCS.SvcRequestCl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region Cargo Data
        public static List<SpRequestCl> GetList(GridListFilter filter)
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
                var data = db.DbContext.Database.SqlQuery<SpRequestCl>(@"exec [dbo].[sp_get_task_cl]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                return data;
            }
        }

        public static RequestCl GetRequestCl(long id)
        {
            using (var db = new Data.EmcsContext())
            {

                db.Database.CommandTimeout = 600;
                var result = db.RequestCl.Where(a => a.IdCl == id.ToString()).FirstOrDefault();
                return result;

            }
        }

        public static int GetTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var username = crit.Username ?? "";
                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_task_cl]@Username='" + username + "', @isTotal='1'").FirstOrDefault();
                if (tb != null)
                    return tb.Total;
            }

            return 0;
        }
        #endregion

        #region SI Data
        public static List<SpRequestCl> GetSiList(GridListFilter filter)
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
                var data = db.DbContext.Database.SqlQuery<SpRequestCl>(@"exec [dbo].[sp_get_task_si]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                return data;
            }
        }

        public static int GetSiTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var username = crit.Username ?? "";
                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_task_si]@Username='" + username + "', @isTotal='1'").FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            return 0;
        }
        #endregion

        #region Npe Data
        public static List<SpRequestNpePeb> GetNpePebList(GridListFilter filter)
        {
            try
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
                    var data = db.DbContext.Database.SqlQuery<SpRequestNpePeb>(@"exec [dbo].[sp_get_task_npe_peb]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                    return data;
                }
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static int GetNpePebTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var username = crit.Username ?? "";

                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_task_npe_peb]@Username='" + username + "', @isTotal='1'").FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            return 0;
        }
        #endregion

        #region Bl Data
        public static List<SpRequestCl> GetBlList(GridListFilter filter)
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
                var data = db.DbContext.Database.SqlQuery<SpRequestCl>(@"exec [dbo].[sp_get_task_bl]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                return data;
            }
        }

        public static int GetBlTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var username = crit.Username ?? "";
                var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_get_task_bl]@Username='" + username + "', @isTotal='1'").FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            return 0;
        }
        public static int GetRFCTotalList(GridListFilter crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var username = crit.Username ?? "";
                    var tb = db.Database.SqlQuery<CountData>("[dbo].[sp_RequestForChangeHistory] @Approver='" + username + "',  @IsTotal='1'").FirstOrDefault();
                    if (tb != null) return tb.Total;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return 0;
        }
        #endregion
    }
}
