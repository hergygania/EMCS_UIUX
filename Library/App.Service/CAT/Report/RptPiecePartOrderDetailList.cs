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
    /// Services Proses Report Piece Part Order Detail.</summary>
    public class RptPiecePartOrderDetailList
    {
        private const string cacheName = "App.CAT.RptPiecePartOrderDetailList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Report Piece Part Order Detail with search param.</summary>
        public static IList<Data.Domain.RptPiecePartOrderDetailList> GetList(string ref_part_no, string model, string prefix, string smcs, string component, string mod)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.RptPiecePartOrderDetailList> data = db.RptPiecePartOrderDetailList.ToList();
                if (!String.IsNullOrEmpty(ref_part_no))
                    data = data.Where(i => i.RefPartNo.Trim().ToUpper().Equals(ref_part_no.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(model))
                    data = data.Where(i => i.Model.Trim().ToUpper().Equals(model.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(prefix))
                    data = data.Where(i => i.Prefix.Trim().ToUpper().Equals(prefix.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(smcs))
                    data = data.Where(i => i.SMCS.Trim().ToUpper().Equals(smcs.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(component))
                    data = data.Where(i => i.Component.Trim().ToUpper().Equals(component.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(mod))
                    data = data.Where(i => i.MOD.Trim().ToUpper().Equals(mod.Trim().ToUpper()));

                return data.ToList();
            }
        }

        /// <summary>
        ///  Get Data Report Piece Part Order Detail with search param By Store Procedure.
        /// </summary>
        /// <param name="ref_part_no"></param>
        /// <param name="model"></param>
        /// <param name="prefix"></param>
        /// <param name="smcs"></param>
        /// <param name="component"></param>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static IList<Data.Domain.RptPiecePartOrderDetailList> SP_GetList(string ref_part_no, string model, string prefix, string smcs, string component, string mod, string DateFilter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                string startDate = Convert.ToString(DateFilter.Split('#')[0]);
                string endDate = Convert.ToString(DateFilter.Split('#')[1]);

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_part_no", ref_part_no ?? ""));
                parameterList.Add(new SqlParameter("@model", model ?? ""));
                parameterList.Add(new SqlParameter("@prefix", prefix ?? ""));
                parameterList.Add(new SqlParameter("@smcs", smcs ?? ""));
                parameterList.Add(new SqlParameter("@component", component ?? ""));
                parameterList.Add(new SqlParameter("@mod", mod ?? ""));
                parameterList.Add(new SqlParameter("@StartDate", startDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDate", endDate ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                return db.DbContext.Database.SqlQuery<Data.Domain.RptPiecePartOrderDetailList>("[cat].[spGetDataRptPieceDetail] @ref_part_no, @model, @prefix, @smcs, @component, @mod, @StartDate, @EndDate", parameters).ToList();
            }
        }

        /// <summary>
        /// Get Data Range Week.
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.RangeWeekForPiecePartOrderDetail> GetRangeWeek()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;                                                                     

                return db.DbContext.Database.SqlQuery<Data.Domain.RangeWeekForPiecePartOrderDetail>("[cat].[spGetDataRangeWeekForPiecePartOrderDetail]").ToList();
            }
        }
    }
}
