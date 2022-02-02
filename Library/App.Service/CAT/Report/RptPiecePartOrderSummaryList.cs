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
    /// Services Proses Report Piece Part Order Summary.
    /// </summary>
    public class RptPiecePartOrderSummaryList
    {
        private const string cacheName = "App.CAT.RptPiecePartOrderSummaryList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Report Piece Part Order Summary with search param.
        /// </summary>
        /// <param name="ref_part_no"></param>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <param name="smcs"></param>
        /// <param name="component"></param>
        /// <param name="mod"></param>
        /// <param name="DateFilter"></param>
        /// <returns></returns>
        public static IList<Data.Domain.RptPiecePartOrderSummaryList> GetList(string ref_part_no, string model, string prefix, string smcs, string component, string mod, string DateFilter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_part_no", ref_part_no ?? ""));
                parameterList.Add(new SqlParameter("@model", model ?? ""));
                parameterList.Add(new SqlParameter("@prefix", prefix ?? ""));
                parameterList.Add(new SqlParameter("@smcs", smcs ?? ""));
                parameterList.Add(new SqlParameter("@component", component ?? ""));
                parameterList.Add(new SqlParameter("@mod", mod ?? ""));
                parameterList.Add(new SqlParameter("@DateFilter", DateFilter ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                return db.DbContext.Database.SqlQuery<Data.Domain.RptPiecePartOrderSummaryList>("[cat].[spGetDataRptPieceSummary] @ref_part_no, @model, @prefix, @smcs, @component, @mod, @DateFilter", parameters).ToList();
            }
        }
    }
}
