using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcProblemHistory
    {
        public const string CacheName = "App.EMCS.SvcProblemHistory";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.ProblemHistory> GetList(MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.ProblemHistory.Where(a => (a.Category == crit.searchName));
                return tb.ToList();
            }
        }

        public static dynamic GetListSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CiplProblemHistoryGetById] @id='" + crit.Id + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpCiplProblemHistory>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                var dataList = new List<Data.Domain.EMCS.SpCiplProblemHistory>();
                result.rows = data.Count() == 0 ? dataList : data;
                return result;
            }
        }

        public static dynamic GetListCargoSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CargoProblemHistoryGetById] @id='" + crit.Id + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpCargoProblemHistory>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null)
                    result.total = count.Total;
                var dataList = new List<Data.Domain.EMCS.SpCargoProblemHistory>();
                result.rows = data.Count() == 0 ? dataList : data;
                return result;
            }
        }

        public static long CrudSp(Data.Domain.EMCS.ProblemHistory data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var reqType = data.ReqType;
                var idRequest = data.IdRequest;
                var category = data.Category;
                var dataCase = data.Case;
                var causes = data.Causes;
                var status = data.Status;
                var idStep = data.IdStep;
                var impact = data.Impact;
                var comment = data.Comment;
                var caseDate = data.CaseDate;
                var createBy = SiteConfiguration.UserName;
                var createDate = DateTime.Now;
                var updateBy = SiteConfiguration.UserName;
                var updateDate = DateTime.Now;
                var isDelete = "0";

                var query = "exec [dbo].[sp_insert_update_problem_history] " +
                            "@ReqType='" + reqType + "', " +
                            "@IDRequest='" + idRequest + "', " +
                            "@Category='" + category + "', " +
                            "@Case='" + dataCase + "', " +
                            "@Causes='" + causes + "', " +
                            "@Impact='" + impact + "', " +
                            "@Comment='" + comment + "', " +
                            "@CaseDate='" + caseDate + "', " +
                            "@CreateBy='" + createBy + "', " +
                            "@CreateDate='" + createDate + "', " +
                            "@UpdateBy='" + updateBy + "', " +
                            "@UpdateDate='" + updateDate + "', " +
                            "@IdStep='" + idStep + "', " +
                            "@Status='" + status + "', " +
                            "@IsDelete='" + isDelete + "'";
                var result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(query).FirstOrDefault();
                if (result != null) return result.Id;
            }

            return 0;
        }

        public static List<Data.Domain.EMCS.StringData> GetProblemCategory(string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = "select distinct Category text from dbo.MasterProblemCategory where Category like '%" + term + "%'";
                List<Data.Domain.EMCS.StringData> data = db.Database.SqlQuery<Data.Domain.EMCS.StringData>(tb).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.StringData> GetProblemCase(string cat, string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = "select [Case] text from dbo.MasterProblemCategory where Category = '" + cat + "'";
                List<Data.Domain.EMCS.StringData> data = db.Database.SqlQuery<Data.Domain.EMCS.StringData>(tb).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.ProblemHistory> GetProblemCauses(string cat, string cases, string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = "select top 100 * from dbo.ProblemHistory where Category = '" + cat + "' AND [Case] = '" + cases + "' AND Causes like '%" + term + "%'";
                List<Data.Domain.EMCS.ProblemHistory> data = db.Database.SqlQuery<Data.Domain.EMCS.ProblemHistory>(tb).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.ProblemHistory> GetProblemImpact(string cat, string cases, string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = "select top 100 * from dbo.ProblemHistory where Category = '" + cat + "' AND [Case] = '" + cases + "' AND Impact like '%" + term + "%'";
                List<Data.Domain.EMCS.ProblemHistory> data = db.Database.SqlQuery<Data.Domain.EMCS.ProblemHistory>(tb).ToList();
                return data;
            }
        }

        public static int GetTotalProblem()
        {
            using (var db = new Data.EmcsContext())
            {
                //var total = db.ProblemHistory.Count();
                var total = db.ProblemHistory.Where(x => x.CaseDate.Year == DateTime.Now.Year).Count();
                var y = DateTime.Now.Year;
                return total;
            }
        }       
    }
}
