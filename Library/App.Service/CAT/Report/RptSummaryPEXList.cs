using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Report
{
    /// <summary>
    /// Services Proses Report Summary PEX.</summary>
    public class RptSummaryPEXList
    {
        private const string cacheName = "App.CAT.RptSummaryPEXList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Report Summary PEX by ref_part_no, model and component.</summary>
        public static IList<Data.Domain.RptSummaryPEXList> GetList(string ref_part_no, string model, string component, string SOS)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_part_no", ref_part_no ?? ""));
                parameterList.Add(new SqlParameter("@model", model ?? ""));
                parameterList.Add(new SqlParameter("@component", component ?? ""));   
                parameterList.Add(new SqlParameter("@SOS", SOS ?? ""));   
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.RptSummaryPEXList>("[cat].[spGetDataRptSummaryPEX] @ref_part_no, @model, @component, @SOS", parameters).ToList();

                return data;
            }
        }
    }
}
