using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;
using App.Data.Domain.Extensions;

namespace App.Service.Master
{
    public class ToDoLists
    {
        private const string cacheName = "App.master.ToDoListTable";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        #region Supply
        public static List<SupplyDetailResult> GetToDoListSupplyHeader(string userId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                //// Tambahan Didi
                //parameterList.Add(new SqlParameter("@ReportName", "ToDoListSupply"));
                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.ToDoListTable>("dbo.SummaryofSupplyRpt @UserId", parameters).Where(a => a.CreatedOn.Date <= DateTime.Now.AddDays(-1).Date).ToList();
//                var data = db.DbContext.Database.SqlQuery<Data.Domain.ToDoListTable>("dbo.SummaryofSupplyRpt @UserId", parameters).ToList();
                //var data = db.DbContext.Database.SqlQuery<Data.Domain.ToDoListTable>("dbo.spToDoList @UserId, @ReportName", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<SupplyDetailResult>("pis.GetToDoListSupply @UserId", parameters).ToList();
               
                return data;
            }
        }

        public static List<SupplyDetail> GetToDoListSupply(string userId)
        {
            var SupplyHeader = GetToDoListSupplyHeader(userId).ToList();

            List<SupplyDetail> hierarchy = new List<SupplyDetail>();

            return hierarchy = SupplyHeader
                            .Where(m => m.ParentID == 0)
                            .Select(m => new SupplyDetail
                            {
                                ID = m.ID,
                                ParentID = m.ParentID,
                                Name = m.Name,
                                Url = m.Url,
                                HUBSTORE = m.HUBSTORE,
                                Value = m.Value,
                                children = GetSubReportDetailSupply(SupplyHeader, m.ID).ToList()
                            }).ToList();
        }

        public static List<SupplyDetail> GetSubReportDetailSupply(List<SupplyDetailResult> SupplyHeader, int parentID)
        {
            return SupplyHeader
                    .Where(m => m.ParentID == parentID)
                    .Select(m => new SupplyDetail
                    {
                        ID = m.ID,
                        ParentID = m.ParentID,
                        Name = m.Name,
                        Url = m.Url,
                        HUBSTORE = m.HUBSTORE,
                        Value = m.Value,
                        children = GetSubReportDetailSupply(SupplyHeader, m.ID)
                    })
                    .ToList();
        }


        #endregion

        #region Part Counter

        public static List<PartCounterDetailResult> GetToDoListPartCounterHeader(string userId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.ToDoListTable>("dbo.SummaryofPartsCounterOperationRptByStore @UserId", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<PartCounterDetailResult>("pis.GetToDoListPartCounter @UserId", parameters).ToList();
                return data;
            }
        }

        public static List<PartCounterDetail> GetToDoListPartCounter(string userId)
        {
            var PartCounterHeader = GetToDoListPartCounterHeader(userId).OrderBy(p => p.CreatedOn).ToList();

            List<PartCounterDetail> hierarchy = new List<PartCounterDetail>();

            return hierarchy = PartCounterHeader
                            .Where(m => m.ParentID == 0)
                            .Select(m => new PartCounterDetail
                            {
                                ID = m.ID,
                                ParentID = m.ParentID,
                                Name = m.Name,
                                Url = m.Url,
                                AREASTORE = m.AREASTORE,
                                Value = m.Value,
                                children = GetSubReportDetail(PartCounterHeader, m.ID).ToList()
                            }).ToList();
        }

        public static List<PartCounterDetail> GetSubReportDetail(List<PartCounterDetailResult> PartCounterHeader, int parentID)
        {
            return PartCounterHeader
                    .Where(m => m.ParentID == parentID)
                    .Select(m => new PartCounterDetail
                    {
                        ID = m.ID,
                        ParentID = m.ParentID,
                        Name = m.Name,
                        Url = m.Url,
                        AREASTORE = m.AREASTORE,
                        Value = m.Value,
                        children = GetSubReportDetail(PartCounterHeader, m.ID)
                    })
                    .ToList();
        }

        #endregion

        public static List<Data.Domain.ToDoListTable> GetToDoListImex()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                // Tambahan Didi
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ReportName", "ToDoListImex"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ToDoListTable>("dbo.spToDoListImex @ReportName", parameters).ToList();
                return data;
            }
        }

    }
}
