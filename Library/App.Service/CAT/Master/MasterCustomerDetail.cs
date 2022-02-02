using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Master
{
    public class MasterCustomerDetail
    {
        public const string cacheName = "App.master.MasterCustomer";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List Data Master CustomerDetail.</summary>
        public static List<Data.Domain.MasterCustomerDetail> GetList(string headerid)
        {
            using (var db = new Data.EfDbContext())
            {
                int hdrid = Convert.ToInt32(headerid);
                return db.MasterCustomerDetail.Where(a => a.CUSTOMERHEADERID == hdrid).ToList();
            }
        }

        /// <summary>
        /// Get Data Master Customer paging.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="keysearch"></param>
        /// <returns></returns>
        public static List<Data.Domain.MasterCustomerDetail> GetList(int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != 0 ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterCustomerDetail>("[CAT].[spGetListCustomer] @next, @keysearch, 1", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Get Count Master Customer paging.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="keysearch"></param>
        /// <returns></returns>
        public static int GetListCount(int next, string keysearch)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[CAT].[spGetListCustomer] @next, @keysearch, 2", parameters).FirstOrDefault();

                return data;
            }
        }

        /// <summary>
        /// Checking data if exist on master CustomerDetail.</summary>
        /// <param name="item"> data master CustomerDetail</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static string ExistMasterCustomerDetail(string CustId, int headerid)
        {
            if (CustId != null)
            {
                var existitem = Service.CAT.Master.MasterCustomerDetail.GetCode(CustId, headerid);
                if (existitem != null)
                {
                    return existitem.CUSTOMER_ID.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get Data Master Customer Detail with paramater string code.</summary>
        /// <param name="code"> field code on master Customer Detail</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterCustomerDetail GetCode(string CustId, int headerid)
        {
            var item = GetList(headerid.ToString()).Where(i => i.CUSTOMER_ID == CustId && i.CUSTOMERHEADERID == headerid && i.IsActive == 1).FirstOrDefault();
            return item;
        }

        /// <summary>
        /// Proses insert update delete on master Customer</summary>
        /// <param name="item"> data on master Customer</param>
        /// <param name="dml"> flag on insert (I), update (U) and delete (D)</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static int crud(Data.Domain.MasterCustomerDetail item, string dml)
        {
            if (dml == "I")
            {
                item.LASTUPDATE = DateTime.Now;
                //item.EntryDate = DateTime.Now;
            }

            if (dml == "U")
            {
                item.LASTUPDATE = DateTime.Now;
                item.IsActive = 0;
            }

            //item.LASTUPDATE = Domain.SiteConfiguration.UserName;
            item.LASTUPDATE = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.MasterCustomerDetail>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// Get Data Master Customer with parameter serach.</summary>
        /// <param name="crit"> paramater value on search</param>
        /// <seealso cref="Domain.MasterSearchForm"></seealso>
        public static List<Data.Domain.MasterCustomerDetail> GetList(Domain.MasterSearchForm crit, string headerid)
        {
            var cust = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList(headerid)
                       where (cust == "" || (c.CUSTOMERNAME.ToLower()).Contains(cust)) && c.IsActive == 1
                       orderby c.CUSTOMERNAME
                       select c;
            return list.ToList();
        }

    }
}
