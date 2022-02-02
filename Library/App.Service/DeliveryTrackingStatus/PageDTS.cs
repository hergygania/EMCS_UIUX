using App.Data.Caching;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.DeliveryTrackingStatus
{
    public class PageDTS
    {
        private const string cacheName = "App.DeliveryTrackingStatus.SearchingDTS";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.V_DTS> GetDataViewDTS(Data.Domain.V_filterDTS model)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SN", !string.IsNullOrWhiteSpace(model.SN) ? model.SN.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@From", !string.IsNullOrWhiteSpace(model.From) ? model.From.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@To", !string.IsNullOrWhiteSpace(model.To) ? model.To.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@OutBoundDelivery", !string.IsNullOrWhiteSpace(model.OutBoundDelivery) ? model.OutBoundDelivery.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@SalesOrderNumber", !string.IsNullOrWhiteSpace(model.SalesOrderNumber) ? model.SalesOrderNumber.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@Model", !string.IsNullOrWhiteSpace(model.Model) ? model.Model.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@ETD", !string.IsNullOrWhiteSpace(model.ETD) ? model.ETD.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@ATD", !string.IsNullOrWhiteSpace(model.ATD) ? model.ATD.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@ETA", !string.IsNullOrWhiteSpace(model.ETA) ? model.ETA.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@ATA", !string.IsNullOrWhiteSpace(model.ATA) ? model.ATA.ToString() : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_DTS>("dbo.spGetDTS @SN, @From, @To, @OutBoundDelivery, @SalesOrderNumber, @Model, @ETD, @ATD, @ETA, @ATA", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.V_DetailDTS> GetDataViewDetailDTS(string NODA, string NODI)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@NODI", !string.IsNullOrWhiteSpace(NODI) ? NODI : string.Empty));
                parameterList.Add(new SqlParameter("@NODA", !string.IsNullOrWhiteSpace(NODA) ? NODA : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_DetailDTS>("dbo.spGetDetailDTS @NODI, @NODA", parameters).ToList();

                return data;
            }
        }

        public static int Update(Data.Domain.DeliveryTrackingStatus itm, string dml)
        {
            int ret = 0;
            var list = App.Service.DeliveryTrackingStatus.ImportDTS.GetDTSByNODI(itm.NODI);
            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var trans = db.DbContext.Database.BeginTransaction();
                try
                {
                    foreach (var data in list)
                    {
                        data.PICDriver = itm.PICDriver;
                        data.VendorName = itm.VendorName;
                        data.OperatingPlan = itm.OperatingPlan;
                        data.Cost = itm.Cost;
                        data.Remarks = itm.Remarks;
                        data.EntryBy = Convert.ToString(Domain.SiteConfiguration.UserName);
                        data.EntryDate = DateTime.Now;

                        ret += db.CreateRepository<Data.Domain.DeliveryTrackingStatus>().CRUD(dml, data);
                    }

                    trans.Commit();
                }
                catch (Exception)
                {
                    ret = 0;
                    trans.Rollback();
                }
            }

            return ret;
        }

        public static List<Data.Domain.MilestoneDTS> GetMilestoneDTS(string OBD, string SO)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@OBD", !string.IsNullOrWhiteSpace(OBD) ? OBD : string.Empty));
                parameterList.Add(new SqlParameter("@SO", !string.IsNullOrWhiteSpace(SO) ? SO : string.Empty));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.MilestoneDTS>(
                    "[dbo].[spGetMilestoneDTS] @OBD, @SO", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.DeliveryTracking> GetDeliveriTracking()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.DeliveryTracking>().Table.Select(e => e);
                return tbl.ToList();
            }
        }

        public static Data.Domain.DeliveryTracking GetDeliveriTrackingByID(int ID)
        {
            var item = GetDeliveriTracking().Where(p => p.ID == ID).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.PartsList> GetListPartNumber(int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsList>("dbo.spGetDetailDTS @next, @keysearch", parameters).ToList();

                return data;
            }
        }
    }
}
