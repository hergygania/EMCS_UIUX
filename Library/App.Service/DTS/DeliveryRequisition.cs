using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class DeliveryRequisition
    {
        public const string cacheName = "App.DTS.DeliveryRequisition";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.DeliveryRequisition item, string dml)
        {
            if (dml == "I")
            {
                item.ID = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    if (dml == "I")
                    {
                        var dataRes = db.CreateRepository<Data.Domain.DeliveryRequisition>().Add(item);
                        return Convert.ToInt32(item.ID);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.DeliveryRequisition>().CRUD(dml, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int crud(string dml, Data.Domain.DeliveryRequisition item, List<Data.Domain.DeliveryRequisitionUnit> units)
        {
            if (dml == "I")
            {
                item.ID = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;
   
            if (item.Status != null && (item.Status.ToLower() == "submit" && item.Status.ToLower() == "revised"))
            {
                item.ActivityTracking = "DR CREATION";
            }

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {

                    using (System.Data.Entity.DbContextTransaction dbTran = db.DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            int dataRes = 0;
                            if (dml == "I")
                            {
                                dataRes = db.CreateRepository<Data.Domain.DeliveryRequisition>().Add(item);                                
                            }
                            else
                            {
                                dataRes = db.CreateRepository<Data.Domain.DeliveryRequisition>().CRUD(dml, item);
                                if (dataRes > 0)
                                {
                                    if (units != null && units.Count() > 0)
                                    {
                                        int noOfRowDeleted = db.DbContext.Database
                                            .ExecuteSqlCommand("DELETE from DeliveryRequisitionUnit where HeaderId=" + item.ID);

                                        Console.WriteLine(noOfRowDeleted);
                                    }
                                }
                            }

                            if (dataRes > 0)
                            {
                                if (units != null && units.Count() > 0)
                                {
                                    foreach (Data.Domain.DeliveryRequisitionUnit unit in units)
                                    {
                                        unit.HeaderID = item.ID;
                                        unit.RefNo = (unit.RefNo ?? "").Trim();
                                        unit.Checked = true;
                                        unit.CreateBy = Domain.SiteConfiguration.UserName;
                                        unit.CreateDate = DateTime.Now;
                                        unit.UpdateBy = Domain.SiteConfiguration.UserName;
                                        unit.UpdateDate = DateTime.Now;
                                        unit.Province = unit.Province.Replace("-", "");
                                        unit.Kabupaten = unit.Kabupaten.Replace("-", "");
                                        unit.Kecamatan = unit.Kecamatan.Replace("-", "");
                                        db.CreateRepository<Data.Domain.DeliveryRequisitionUnit>().Add(unit);
                                    }
                                }
                            }
                            //commit transaction  
                            dbTran.Commit();
                            return Convert.ToInt32(item.ID);
                        }
                        catch (Exception ex)
                        {
                            //Rollback transaction if exception occurs  
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int crudreroute(string dml, Data.Domain.DeliveryRequisition item, List<Data.Domain.DeliveryRequisitionUnit> units, App.Data.Domain.DeliveryRequisition_Reroute item_Reroute)
        {
     

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {

                    using (System.Data.Entity.DbContextTransaction dbTran = db.DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            int dataRes = 0;
                                dataRes = db.CreateRepository<Data.Domain.DeliveryRequisition>().CRUD(dml, item);
                            //commit transaction  
                            dbTran.Commit();
                            return Convert.ToInt32(item.ID);
                        }
                        catch (Exception ex)
                        {
                            //Rollback transaction if exception occurs  
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static String GetTerritoryName(string parameterId, string parametername)
        {
            string TerritoryValue = "";

            try
            {

                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;

                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@parameterId", parameterId));
                    parameterList.Add(new SqlParameter("@parametername", parametername));

                    SqlParameter[] parameters = parameterList.ToArray();
                    TerritoryValue = db.DbContext.Database.SqlQuery<string>
                        (@"exec [dbo].[SP_GetTerritory] @parameterId, @parametername", parameters)
                        .FirstOrDefault();
                    return TerritoryValue;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public static int delete(Data.Domain.DeliveryRequisition item)
        {
            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    using (System.Data.Entity.DbContextTransaction dbTran = db.DbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var dataRes = db.CreateRepository<Data.Domain.DeliveryRequisition>().CRUD("D", item);
                            if (dataRes > 0)
                            {
                                int noOfRowDeleted = db.DbContext.Database
                                            .ExecuteSqlCommand("DELETE from DeliveryRequisitionUnit where HeaderId=" + item.ID);
                            }
                            //commit transaction  
                            dbTran.Commit();
                            return Convert.ToInt32(item.ID);
                        }
                        catch (Exception ex)
                        {
                            //Rollback transaction if exception occurs  
                            dbTran.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.DeliveryRequisition> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryRequisition;
                return tb.ToList();
            }
        }
        public static List<Data.Domain.DRChart> GetChartList(string userID,bool isSPChain)
        {
            int todaysYear = DateTime.Now.Year;
            
            try
            {              
                using (var db = new Data.DTSContext())
                {
                    db.Database.CommandTimeout = 600;
                    if (isSPChain)
                    {
                        var data = (from p in db.DeliveryRequisition
                                    where p.CreateDate.Value.Year >= todaysYear

                                    select new Data.Domain.DRChart
                                    {
                                        month = p.CreateDate.Value.Month,
                                        status = p.Status
                                    }
                                ).ToList();


                        return data;
                    }
                    else
                    {
                        var data = (from p in db.DeliveryRequisition
                                    where p.ReqID == userID && p.CreateDate.Value.Year >= todaysYear

                                    select new Data.Domain.DRChart
                                    {
                                        month = p.CreateDate.Value.Month,
                                        status = p.Status
                                    }
                                ).ToList();


                        return data;
                    }

                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.DeliveryRequisition> GetListFilter(App.Data.Domain.DTS.DeliveryRequisitionFilter filter, bool forReport = false)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                string search = "";
                string status = "";

                

                if (filter.searchName !=null)
                {
                    search = Regex.Replace(filter.searchName, @"[^0-9a-zA-Z]+", "");
                }
                if (filter.status !=null)
                {
                    status = Regex.Replace(filter.status, @"[^0-9a-zA-Z]+", "");
                }             

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string UserId = Domain.SiteConfiguration.UserName;
                string RoleName = GetRoleName(UserId);
                if (RoleName == "DTSApproval" || RoleName == "DTSSpu")
                {
                    parameterList.Add(new SqlParameter("@createdby", ""));                   
                }
                else
                {
                    parameterList.Add(new SqlParameter("@createdby", filter.requestor == true ? Domain.SiteConfiguration.UserName : ""));
                   
                }
                parameterList.Add(new SqlParameter("@requestor", filter.requestor == true ? true : false));
                
                parameterList.Add(new SqlParameter("@searchName", search == null ? "" : search));            
               
                parameterList.Add(new SqlParameter("@forReport", forReport));
                parameterList.Add(new SqlParameter("@status", status == null ? "" : status));               
                parameterList.Add(new SqlParameter("@today", filter.today == null ? "" : filter.today));
                parameterList.Add(new SqlParameter("@typesearch", filter.typesearch == null ? "" : filter.typesearch));

                string filterColumns = String.Empty;
                if (filter.filterColumns != null && filter.filterColumns.Count > 0)
                {
                    foreach (App.Data.Domain.DTS.FilterColumn col in filter.filterColumns)
                    {
                        if (col.value != null && col.value != "")
                        {
                            if (col.field == "ExpectedTimeLoading")
                            {
                                filterColumns += String.Format(" AND {0} LIKE '%{1}%'", "CONVERT(VARCHAR(25), ExpectedTimeLoading, 126)", col.value);
                            }
                            else
                            {
                                filterColumns += String.Format(" AND {0} LIKE '%{1}%'", col.field, col.value);
                            }
                            
                        }
                    }
                }
                parameterList.Add(new SqlParameter("@filters", filterColumns));

                SqlParameter[] parameters = parameterList.ToArray();


                var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisition>
                    (@"exec [dbo].[SP_GetDataDeliveryRequesition] @searchName, @requestor, @createdby, @filters, @forReport, @status,@today,@typesearch", parameters).ToList();

                return data;
            }
        }

      

        public static List<Data.Domain.ReportDeliveryRequisition> GetReportListFilter(App.Data.Domain.DTS.ReportDeliveryRequisitionFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Activity", filter.Activity == null ? "" : filter.Activity.ToUpper()));
                parameterList.Add(new SqlParameter("@Status", filter.Status == null ? "" : filter.Status.ToUpper()));

                string filterColumns = String.Empty;
                if (filter.filterColumns != null && filter.filterColumns.Count > 0)
                {
                    foreach (App.Data.Domain.DTS.FilterColumn col in filter.filterColumns)
                    {
                        if (col.value != null && col.value != "")
                        {
                            if (col.field == "ExpectedTimeLoading")
                            {
                                filterColumns += String.Format(" AND {0} LIKE '%{1}%'", "CONVERT(VARCHAR(25), ExpectedTimeLoading, 126)", col.value);
                            }
                            else
                            {
                                filterColumns += String.Format(" AND {0} LIKE '%{1}%'", col.field, col.value);
                            }

                        }
                    }
                }
                parameterList.Add(new SqlParameter("@filters", filterColumns));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ReportDeliveryRequisition>
                    (@"exec [dbo].[SP_GetReportDeliveryRequesition] @Activity, @Status, @filters", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.DeliveryRequisitionRef> GetListForTemplate()
        {
            string key = string.Format(cacheName);
            using (var db = new Data.DTSContext())
            {
                db.Database.CommandTimeout = 600;
                var data = (from p in db.DeliveryRequisition
                            join q in db.DeliveryRequisitionUnit on p.ID equals q.HeaderID 
                            where (p.Status == "complete" || p.Status == "rerouted") && p.ActivityTracking !="POD" && p.StatusTracking != "BAST" 
                            select new DeliveryRequisitionRef
                            {
                                ID = p.ID,
                                KeyCustom = p.KeyCustom,
                                RefNo = q.RefNo,
                                ItemId = q.RefItemId,
                                Model = q.Model,
                                SerialNumber = q.SerialNumber,
                                Batch = q.Batch,                              
                            }
                            ).ToList();

                return data;
            }
        }
        public static List<Data.Domain.DRStatusGroup> GroupByStatusBefore(string userID)
        {
            string key = string.Format(cacheName);
            using (var db = new Data.DTSContext())
            {
                db.Database.CommandTimeout = 600;
                var data = (from p in db.DeliveryRequisition
                            where p.ReqID == userID || p.Sales1ID == userID || p.Sales2ID == userID
                            group p by p.Status into g
                            select new
                            {
                                Status = g.Key,
                                Count_Id = g.Count()
                            }
                            ).ToList();
                List<Data.Domain.DRStatusGroup> listdata = new List<Data.Domain.DRStatusGroup>();
                foreach (var d in data)
                {
                    listdata.Add(new Data.Domain.DRStatusGroup { Status = d.Status, Count_Id = d.Count_Id });
                }
                return listdata;
            }
        }

        public static List<Data.Domain.DRStatusGroup> GroupByStatus(string userID,bool isSPChain)
        {
            List<Data.Domain.DRStatusGroup> listdata = new List<Data.Domain.DRStatusGroup>();
            using (var db = new Data.DTSContext())
            {                
                db.Database.CommandTimeout = 600;
                if (isSPChain)
                {
                    var data = (from p in db.DeliveryRequisition
                               
                                group p by p.Status into g
                                select new
                                {
                                    Status = g.Key,
                                    Count_Id = g.Count()
                                }
                                           ).ToList();                    
                    foreach (var d in data)
                    {
                        listdata.Add(new Data.Domain.DRStatusGroup { Status = d.Status, Count_Id = d.Count_Id });
                    }
                }
                else
                {
                    var data = (from p in db.DeliveryRequisition
                                where p.ReqID == userID || p.Sales1ID == userID || p.Sales2ID == userID
                                group p by p.Status into g
                                select new
                                {
                                    Status = g.Key,
                                    Count_Id = g.Count()
                                }
                                             ).ToList();
                 
                    foreach (var d in data)
                    {
                        listdata.Add(new Data.Domain.DRStatusGroup { Status = d.Status, Count_Id = d.Count_Id });
                    }
                }
             
               
                return listdata;
            }
        }
        public static Int32 GroupByStatusToday(string userID, bool isSPChain)
        {
            Int32 CountToday = 0;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            string key = string.Format(cacheName);
            using (var db = new Data.DTSContext())
            {
                
                db.Database.CommandTimeout = 600;
                if (isSPChain)
                {
                    var data = (from p in db.DeliveryRequisition
                                where (p.Status == "submit" || p.Status == "rerouted") && p.CreateDate >= startDateTime && p.CreateDate <= endDateTime
                                select p
                               );
                    CountToday = data.Count();
                }
                else
                {
                    var data = (from p in db.DeliveryRequisition
                                where p.CreateDate >= startDateTime && p.CreateDate <= endDateTime && p.CreateBy == userID && p.Status == "submit" && p.ReqID == userID
                                select p
                               );
                    CountToday = data.Count();
                }
               
                return CountToday;
            }
        }
        public static Int32 CountNotifAll(string userID, bool isSPChain)
        {
            Int32 Count = 0;
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            string key = string.Format(cacheName);
            using (var db = new Data.DTSContext())
            {
                db.Database.CommandTimeout = 600;
                if (isSPChain)
                {
                    var data = (from p in db.DeliveryRequisition
                                where p.CreateDate >= startDateTime && p.CreateDate <= endDateTime && p.Status == "submit"
                                select p );

                    Count = data.Count();
                }
                else
                {
                    var data = (from p in db.DeliveryRequisition
                                where p.CreateDate >= startDateTime && p.CreateDate <= endDateTime && p.ReqID == userID && p.Status == "approve"
                                select p);

                    Count = data.Count();
                }
                  
                return Count;
            }

         
        }
        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.DeliveryRequisition GetId(Int64 ID)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition;
            var item = tb.Where(i => i.ID == ID).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryRequisition GetDetail(Int64 key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition;
            var item = tb.ToList().Where(i => i.ID == key).FirstOrDefault();
            return item;
        }
        public static Data.Domain.DeliveryRequisition GetDRExistDetail(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition;
            var item = tb.ToList().Where(i => i.RefNo == key && i.IsDemob == false && i.Status != "reject").FirstOrDefault();
            return item;
        }
        public static Data.Domain.DeliveryRequisition GetStatusDR(Int64 Id)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition;
            var item = tb.ToList().Where(i => i.ID == Id && i.IsDemob == false && i.Status != "reject").FirstOrDefault();
            return item;
        }
        public static Data.Domain.DeliveryRequisition GetByKeyCustom(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisition;
            var item = tb.Where(i => i.KeyCustom.Equals(key)).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryRequisition GetDetailOutbound(string key)
        {
            var db = new Data.DTSContext();

            var data = (from p in db.DeliveryRequisition
                        join q in db.DeliveryRequisitionUnit on p.ID equals q.HeaderID into tmp
                        from q in tmp.DefaultIfEmpty()
                        where p.KeyCustom == key || q.SerialNumber == key
                        select p).FirstOrDefault();

            return data;
        }

        public static Data.Domain.UserAccess GetDetailUser(string UserID)
        {
            var db = new Data.EfDbContext();
            var tb = db.UserAccesses;
            var item = tb.Where(a => a.UserID.Equals(UserID)).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.DeliveryRequisitionRef> GetReferenceReroute(string keyNumber)
        {
            string key = string.Format(cacheName);
            try
            {
                
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    if (keyNumber != null)
                    {                        
                        keyNumber = Regex.Replace(keyNumber, @"\s+\$|\s+(?=\w+$)", "");
                    }
                   
                    parameterList.Add(new SqlParameter("@keyNumber", keyNumber == null ? "" : keyNumber));

                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisitionRef>
                        (@"exec [dbo].[SP_GetDRReferenceReroute]  @keyNumber", parameters)
                        .ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Data.Domain.DeliveryRequisitionRef> GetReference(string keyType, string keyNumber)
        {
            string key = string.Format(cacheName);
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    if (keyNumber != null)
                    {
                        //Regex.Replace(stw, @"\s+\$|\s+(?=\w+$)", ",");
                        keyNumber = Regex.Replace(keyNumber, @"\s+\$|\s+(?=\w+$)", "");
                    }


                    parameterList.Add(new SqlParameter("@keyType", keyType == null ? "SO" : keyType));
                    parameterList.Add(new SqlParameter("@keyNumber", keyNumber == null ? "" : keyNumber));

                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisitionRef>
                        (@"exec [dbo].[SP_GetDRReference] @keyType, @keyNumber", parameters)
                        .ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SendDBMail(Int64 headerId, string EmailType, string EmailCode, Int64 LogId)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    if (EmailType == "DI")
                    {
                        parameterList.Add(new SqlParameter("@ID", headerId));
                        parameterList.Add(new SqlParameter("@EmailCode", EmailCode == null ? "" : EmailCode));
                        SqlParameter[] parameters = parameterList.ToArray();
                        db.DbContext.Database.ExecuteSqlCommand(@"exec [dbo].[SP_SendEmailDI] @ID, @EmailCode", parameters);
                    }
                    else
                    {
                        parameterList.Add(new SqlParameter("@ID", headerId));
                        parameterList.Add(new SqlParameter("@EmailType", EmailType == null ? "" : EmailType));
                        parameterList.Add(new SqlParameter("@EmailCode", EmailCode == null ? "" : EmailCode));
                        parameterList.Add(new SqlParameter("@LogId", LogId));
                        SqlParameter[] parameters = parameterList.ToArray();
                        db.DbContext.Database.ExecuteSqlCommand(@"exec [dbo].[SP_SendEmailDR] @ID, @EmailType, @EmailCode, @LogId", parameters);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Data.Domain.DeliveryRequisitionRef GetDataWithReference(string keyType, string keyNumber)
        {
            string key = string.Format(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@keyType", keyType == null ? "SO" : keyType));
                    parameterList.Add(new SqlParameter("@keyNumber", keyNumber == null ? "" : keyNumber));
                    parameterList.Add(new SqlParameter("@formType", "U"));

                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisitionRef>
                        (@"exec [dbo].[SP_GetDRReference] @keyType, @keyNumber, @formType", parameters)
                        .First();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetRoleName(string UserID)
        {
            try
            {
                using (var db = new Data.EfDbContext())
                {
                    var UserRole = (from p in db.UserAccesses
                                        join q in db.RoleAccesses on p.RoleID equals q.RoleID
                                        where p.UserID == UserID
                                        select q
                                    ).FirstOrDefault();
                    return UserRole.RoleName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string MonthName (int monthdate)
        {
            string strmonth = "";
            if (monthdate == 1)
            {
                strmonth = "JANUARY";
            }
            else if (monthdate == 2)
            {
                strmonth = "FEBRUARY";
            }
            else if (monthdate == 3)
            {
                strmonth = "MARCH";
            }
            else if (monthdate == 4)
            {
                strmonth = "APRIL";
            }
            else if (monthdate == 5)
            {
                strmonth = "MEI";
            }
            else if (monthdate == 6)
            {
                strmonth = "JUNI";
            }
            else if (monthdate == 7)
            {
                strmonth = "JULY";
            }
            else if (monthdate == 8)
            {
                strmonth = "AGUSTUS";
            }
            else if (monthdate == 9)
            {
                strmonth = "SEPTEMBER";
            }
            else if (monthdate == 10)
            {
                strmonth = "OCTOBER";
            }
            else if (monthdate == 11)
            {
                strmonth = "NOVEMBER";
            }
            else if (monthdate == 12)
            {
                strmonth = "DECEMBER";
            }
            return strmonth;

        }

        public static bool DRHisOwned(Int64 id, string userId)
        {
            using (var db = new Data.DTSContext())
            {
                var result = false;

                var tb = db.DeliveryRequisition.FirstOrDefault(a => a.ID == id && a.CreateBy == userId);
                if (tb != null)
                {
                    result = true;
                }

                return result;
            }
        }
        public static bool GetRoleDRApproval(Int64 number,string userId)
        {
            var result = false;
            using (var db = new Data.EfDbContext())
            {
               
                var UserRole = (from p in db.UserAccesses where p.UserID == userId select p).FirstOrDefault();
                if (UserRole.RoleID == 11 || UserRole.RoleID == 20 || UserRole.RoleID == 21)
                {
                    result = true;
                }
                else if (UserRole.RoleID == 12)
                {
                    if (number == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        if (DRHisOwned(number, userId))
                        {
                            result = true;
                        }
                    }

                }
            }

            return result;
        }
    }
}
