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
    /// Services Proses Report Outstanding Old Core Detail.</summary>
    public class RptOutstandingOldCoreDetailList
    {
        private const string cacheName = "App.CAT.RptOutstandingOldCoreDetailList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Report Outstanding Old Core Detail with parameter serach.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Data.Domain.RptOutstandingOldCoreDetailList> GetList(App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter filter)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.RptOutstandingOldCoreDetailList> outstandingoldcoredetail = new List<Data.Domain.RptOutstandingOldCoreDetailList>();//db.RptOutstandingOldCoreDetailList.ToList();                

                //if (store_id != null)
                //    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.StoreID == store_id);

                //if (sos_id != null)
                //    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.SOSID == sos_id);

                if (!String.IsNullOrEmpty(filter.kal))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.KAL.Trim().ToUpper().Equals(filter.kal.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.model))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.Model.Trim().ToUpper().Equals(filter.model.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.component))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.Component.Trim().ToUpper().Equals(filter.component.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.sn_unit))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.UnitNoSN.Trim().ToUpper().Equals(filter.sn_unit.Trim().ToUpper()));

                //if (customer_id != null)
                //    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.CustomerID == customer_id);

                if (!String.IsNullOrEmpty(filter.prefix))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.Prefix.Trim().ToUpper().Equals(filter.prefix.Trim().ToUpper()));

                //if (filter.store_supplied_date_start.HasValue && filter.store_supplied_date_end.HasValue)
                //    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.StoreSuppliedDate >= filter.store_supplied_date_start.Value.Date && i.StoreSuppliedDate <= filter.store_supplied_date_end.Value.Date);

                //
                if (!String.IsNullOrEmpty(filter.reconditioned_wo))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.ReconditionedWO.Trim().ToUpper().Equals(filter.reconditioned_wo.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.doc_c))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.SaleDoc.Trim().ToUpper().Equals(filter.doc_c.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.doc_r))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.ReturnDoc.Trim().ToUpper().Equals(filter.doc_r.Trim().ToUpper()));

                if (!String.IsNullOrEmpty(filter.wcsl))
                    outstandingoldcoredetail = outstandingoldcoredetail.Where(i => i.WCSL.Trim().ToUpper().Equals(filter.wcsl.Trim().ToUpper()));

                return outstandingoldcoredetail.ToList();
            }
        }

        /// <summary>
        /// Get Data Report Outstanding Old Core Detail with parameter serach By Stored Procedure.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IList<Data.Domain.SP_RptOutstandingOldCoreDetail> SP_GetList(App.Data.Domain.CAT.RptOutstandingOldCoreDetailFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Store", filter.store_id ?? ""));
                parameterList.Add(new SqlParameter("@SOS", filter.sos_id ?? ""));
                parameterList.Add(new SqlParameter("@PartNo", filter.part_no ?? ""));
                parameterList.Add(new SqlParameter("@KAL", filter.kal ?? ""));
                parameterList.Add(new SqlParameter("@Model", filter.model ?? ""));
                parameterList.Add(new SqlParameter("@Component", filter.component ?? ""));
                parameterList.Add(new SqlParameter("@SNUnit", filter.sn_unit ?? ""));
                parameterList.Add(new SqlParameter("@Customer", filter.customer_id ?? ""));
                parameterList.Add(new SqlParameter("@Prefix", filter.prefix ?? ""));
                parameterList.Add(new SqlParameter("@store_supplied_date_start", filter.store_supplied_date_start ?? ""));
                parameterList.Add(new SqlParameter("@store_supplied_date_end", filter.store_supplied_date_end ?? ""));
                parameterList.Add(new SqlParameter("@NewWO6F", filter.reconditioned_wo ?? ""));
                parameterList.Add(new SqlParameter("@DocSale", filter.doc_c ?? ""));
                parameterList.Add(new SqlParameter("@DocReturn", filter.doc_r ?? ""));
                parameterList.Add(new SqlParameter("@DocWCSL", filter.wcsl ?? ""));
                parameterList.Add(new SqlParameter("@DateFilter", filter.DateFilter ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_RptOutstandingOldCoreDetail>
                    ("[cat].[spGetDataRptOutstandingOldCoreDetail] @Store, @SOS, @PartNo, @KAL, @Model, @Component, @SNUnit, @Customer, @Prefix, @store_supplied_date_start, @store_supplied_date_end, @NewWO6F, @DocSale, @DocReturn, @DocWCSL, @DateFilter", parameters).ToList();

                return data;
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

                return db.DbContext.Database.SqlQuery<Data.Domain.RangeWeekForPiecePartOrderDetail>("[cat].[spGetDataRangeWeekForOutStandingOldCoreDetail]").ToList();
            }
        }

    }
}
