using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using App.Data.Caching;
using System.Data.SqlClient;

namespace App.Service.CAT.Master
{
    /// <summary>
    /// Services Proses Master Store.</summary>
    public class MasterCustomer
    {
        public const string cacheName = "App.master.MasterCustomer";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List Data Master Customer.</summary>
        public static List<Data.Domain.MasterCustomer> GetList()
        {
            using (var db = new Data.EfDbContext())
            {
                return db.MasterCustomer.ToList();
            }
        }

        /// <summary>
        /// Get Data Master Customer paging.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="keysearch"></param>
        /// <returns></returns>
        public static List<Data.Domain.MasterCustomer> GetList(int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterCustomer>("[CAT].[spGetListCustomer] @next, @keysearch, 1", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Get Data Master Customer paging.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="keysearch"></param>
        /// <returns></returns>
        public static List<Data.Domain.CustomerDBS> GetListDBS(int? next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.CustomerDBS>("[CAT].[SPGetCustomerDBS] @next, @keysearch, 1", parameters).ToList();

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
        /// Get Count Master Customer paging.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="keysearch"></param>
        /// <returns></returns>
        public static int GetListDBSCount(int? next, string keysearch)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[CAT].[SPGetCustomerDBS] @next, @keysearch, 2", parameters).FirstOrDefault();

                return data;
            }
        }

        /// <summary>
        /// Get Data Master Customer.
        /// </summary>
        /// <returns></returns>
        public static Data.Domain.MasterCustomer GetData(int CUST_ID)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.MasterCustomer.Where(w => w.CUSTOMER_ID == CUST_ID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Checking data if exist on master Customer.</summary>
        /// <param name="item"> data master Customer</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static string ExistMasterCustomer(String CustName, string curdMode, int Custid)
        {
            if (CustName != null)
            {
                if (curdMode == "U")
                {
                    var existitem = Service.CAT.Master.MasterCustomer.GetCodeforValidationEdit(Custid, CustName);
                    if (existitem != null)
                    {
                        return existitem.CUSTOMERNAME.ToString();
                    }
                }
                else
                {
                    var existitem = Service.CAT.Master.MasterCustomer.GetCode(CustName);
                    if (existitem != null)
                    {
                        return existitem.CUSTOMERNAME.ToString();
                    }
                }
               
            }
            return string.Empty;
        }

        /// <summary>
        /// Get Data Master Customer with paramater string code.</summary>
        /// <param name="code"> field code on master Customer</param>
        /// <seealso cref="string"></seealso>
        public static Data.Domain.MasterCustomer GetCode(string Custname)
        {
            var item = GetList().Where(i => i.CUSTOMERNAME == Custname && i.IsActive == 1).FirstOrDefault();
            return item;
        }

        public static Data.Domain.MasterCustomer GetCodeforValidationEdit(int Custid, string Custname)
        {
            var item = GetList().Where(i => i.CUSTOMERNAME == Custname && i.CUSTOMER_ID != Custid).FirstOrDefault();
            return item;
        }


        /// <summary>
        /// Proses insert update delete on master Customer</summary>
        /// <param name="item"> data on master Customer</param>
        /// <param name="dml"> flag on insert (I), update (U) and delete (D)</param>
        /// <seealso cref="Data.Domain.MasterSOS"></seealso>
        public static int crud(Data.Domain.MasterCustomer item, string dml)
        {
            if (dml == "I")
            {
                item.LASTUPDATE = DateTime.Now;
                //item.EntryDate = DateTime.Now;
            }

            //item.LASTUPDATE = Domain.SiteConfiguration.UserName;
            item.LASTUPDATE = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.MasterCustomer>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// Get Data Master Customer with parameter serach.</summary>
        /// <param name="crit"> paramater value on search</param>
        /// <seealso cref="Domain.MasterSearchForm"></seealso>
        public static List<Data.Domain.MasterCustomer> GetList(Domain.MasterSearchForm crit)
        {
            var cust = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (cust == "" || (c.CUSTOMERNAME.ToLower()).Contains(cust)) && c.IsActive == 1
                       orderby c.CUSTOMERNAME
                       select c;
            return list.ToList();
        }

        /// <summary>
        /// Get Data Master Customer with paramater int id.</summary>
        /// <param name="id"> id master Customer</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.MasterCustomer GetId(int id)
        {
            var item = GetList().Where(i => i.CUSTOMER_ID == id).FirstOrDefault();
            return item;
        }
    }
}
