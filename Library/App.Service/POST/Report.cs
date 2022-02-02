using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.POST;

namespace App.Service.POST
{
    public static class Report
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region List

        public static List<ReportSLAModel> GetListReportSla(string user, SearchReport param)
        {
            if (param.startDeliveryDate != null) param.startDeliveryDate = DateTime.ParseExact(param.startDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.endDeliveryDate != null) param.endDeliveryDate = DateTime.ParseExact(param.endDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.startPODate != null) param.startPODate = DateTime.ParseExact(param.startPODate, Global.dateformatParam, null).ToString();
            if (param.endPODate != null) param.endPODate = DateTime.ParseExact(param.endPODate, Global.dateformatParam, null).ToString();


            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                parameterList.Add(new SqlParameter("@statusPO", param.statusPO ?? ""));
                parameterList.Add(new SqlParameter("@startPODate", param.startPODate ?? ""));
                parameterList.Add(new SqlParameter("@endPODate", param.endPODate ?? ""));
                parameterList.Add(new SqlParameter("@branch", param.branch ?? ""));
                parameterList.Add(new SqlParameter("@supplier", param.supplier ?? ""));
                parameterList.Add(new SqlParameter("@userPIC", param.userPIC ?? ""));              
                parameterList.Add(new SqlParameter("@startDeliveryDate", param.startDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@endDeliveryDate", param.endDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit == 0 ? 10 : param.limit));
                parameterList.Add(new SqlParameter("@isExport", param.isExport ? 1 : 0));
                parameterList.Add(new SqlParameter("@isTotal", param.isTotal ? 1 : 0));
                parameterList.Add(new SqlParameter("@order", param.order ?? "DESC"));
                parameterList.Add(new SqlParameter("@sort", param.sort ?? "item.PO_Number"));
                parameterList.Add(new SqlParameter("@pono", param.pono ??""));
                parameterList.Add(new SqlParameter("@invoiceno", param.invoiceno ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ReportSLAModel>(@"exec [dbo].[SP_ReportSLA_LIST]
	                @user		   
                  , @statusPO	   
                  , @startPODate	   
                  , @endPODate		   
                  , @branch			   
                  , @supplier		   
                  , @userPIC
                  , @startDeliveryDate 
                  , @endDeliveryDate   
                  , @skip
                  , @take
                  , @isExport
                  , @isTotal
                  , @order
                  , @sort
                  , @pono
                  , @invoiceno
                ", parameters).ToList();
                return data;
            }
        }

        public static int GetTotalRowSla(string user, SearchReport param)
        {
            //if (param.startDeliveryDate != null) param.startDeliveryDate = DateTime.ParseExact(param.startDeliveryDate, Global.dateformatParam, null).ToString();
            //if (param.endDeliveryDate != null) param.endDeliveryDate = DateTime.ParseExact(param.endDeliveryDate, Global.dateformatParam, null).ToString();
            //if (param.startPODate != null) param.startPODate = DateTime.ParseExact(param.startPODate, Global.dateformatParam, null).ToString();
            //if (param.endPODate != null) param.endPODate = DateTime.ParseExact(param.endPODate, Global.dateformatParam, null).ToString();


            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                parameterList.Add(new SqlParameter("@statusPO", param.statusPO ?? ""));
                parameterList.Add(new SqlParameter("@startPODate", param.startPODate ?? ""));
                parameterList.Add(new SqlParameter("@endPODate", param.endPODate ?? ""));
                parameterList.Add(new SqlParameter("@branch", param.branch ?? ""));
                parameterList.Add(new SqlParameter("@supplier", param.supplier ?? ""));
                parameterList.Add(new SqlParameter("@userPIC", param.userPIC ?? ""));
                parameterList.Add(new SqlParameter("@startDeliveryDate", param.startDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@endDeliveryDate", param.endDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit == 0 ? 10 : param.limit));
                parameterList.Add(new SqlParameter("@isExport", "0"));
                parameterList.Add(new SqlParameter("@isTotal", "1"));
                parameterList.Add(new SqlParameter("@order", param.order ?? "DESC"));
                parameterList.Add(new SqlParameter("@sort", param.sort ?? "item.PO_Number"));
                parameterList.Add(new SqlParameter("@pono", param.pono ?? ""));
                parameterList.Add(new SqlParameter("@invoiceno", param.invoiceno ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<TotalSla>(@"exec [dbo].[SP_ReportSLA_LIST]
	                @user		   
                  , @statusPO	   
                  , @startPODate	   
                  , @endPODate		   
                  , @branch			   
                  , @supplier		   
                  , @userPIC		   
                  , @startDeliveryDate 
                  , @endDeliveryDate   
                  , @skip
                  , @take
                  , @isExport
                  , @isTotal
                  , @order
                  , @sort
                  , @pono
                  , @invoiceno
                ", parameters).FirstOrDefault();
                return data.CountTotal;
            }
        }

        #endregion


    }
}
