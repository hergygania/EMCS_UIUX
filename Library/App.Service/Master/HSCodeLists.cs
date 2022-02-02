using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Threading.Tasks;
using System.Data.Entity;
using App.Data;
using App.Data.Caching;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using System.Data.SqlClient;

namespace App.Service.Master
{
    public class HSCodeLists
    {
        //private const string cacheName = "App.master.HSCodeLists";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.HSCodeList> GetList()
        {
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            var pattern = @"-[^\/]";
            using (var db = new Data.EfDbContext())
            {
                var tb = from c in db.HSCodeLists.AsNoTracking().ToList()
                         from o in db.OrderMethods.Where(a => a.OMID == c.OrderMethodID).DefaultIfEmpty()
                         select new Data.Domain.HSCodeList()
                         {
                             BeaMasuk = c.BeaMasuk,
                             Description = (c.Description != null) ? Regex.Replace(c.Description, pattern, "") : "",
                             EntryBy = c.EntryBy,
                             EntryDate = c.EntryDate,
                             HSCode = c.HSCode,
                             HSCodeReformat = c.HSCodeReformat,
                             HSID = c.HSID,
                             ModifiedBy = c.ModifiedBy,
                             ModifiedDate = c.ModifiedDate,
                             OrderMethodID = c.OrderMethodID,
                             Status = c.Status,
                             OMCode = o == null ? "" : o.OMCode
                         };

                return tb.ToList();
            }
            //});

            //return list;
        }

        public static Data.Domain.HSCodeList GetListByHSCode(string HSCode)
        {
            try
            {
                using (var db = new Data.EfDbContext())
                {
                    return db.HSCodeLists.AsNoTracking().Where(w => w.HSCode == HSCode).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var e = ex;

                return null;
            }
        }

        public static bool ExistHSCode(int HSID, string HSCode)
        {
            try
            {
                using (var db = new Data.EfDbContext())
                {
                    var data = db.HSCodeLists.AsNoTracking().Where(w => w.HSID != HSID && w.HSCode == HSCode).FirstOrDefault();
                    if (data != null) return true;

                    return false;
                }
            }
            catch (Exception ex)
            {
                var e = ex;

                return false;
            }
        }

        public static List<Data.Domain.HSCodeList> GetList(string searchName)
        {
            var list = new List<Data.Domain.HSCodeList>();
            try
            {
                var pattern = @"-[^\/]";
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.HSCodeLists.AsNoTracking().Where(w => (w.HSCode.ToString() + "|" + w.Description.Replace(".", "")).ToLower().Contains(searchName.ToLower())).ToList()
                                 //from o in db.OrderMethods.Where(a => a.OMID == c.OrderMethodID).DefaultIfEmpty()
                             select new Data.Domain.HSCodeList()
                             {
                                 BeaMasuk = c.BeaMasuk,
                                 Description = (c.Description != null) ? Regex.Replace(c.Description, pattern, "") : "",
                                 EntryBy = c.EntryBy,
                                 EntryDate = c.EntryDate,
                                 HSCode = c.HSCode,
                                 HSCodeReformat = c.HSCodeReformat,
                                 HSID = c.HSID,
                                 ModifiedBy = c.ModifiedBy,
                                 ModifiedDate = c.ModifiedDate,
                                 OrderMethodID = c.OrderMethodID,
                                 Status = c.Status,
                                 //OMCode = o == null ? "" : o.OMCode
                                 OMCode = "-"
                             };

                    return tb.OrderBy(o => o.HSCode).ToList();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return list;
        }

        public static List<Data.Domain.HSCodeList> GetListById(int id)
        {
            var list = new List<Data.Domain.HSCodeList>();
            try
            {
                var pattern = @"-[^\/]";
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.HSCodeLists.Where(w => w.HSID == id).AsNoTracking().ToList()
                             from o in db.OrderMethods.Where(a => a.OMID == c.OrderMethodID).DefaultIfEmpty()
                             select new Data.Domain.HSCodeList()
                             {
                                 BeaMasuk = c.BeaMasuk,
                                 Description = (c.Description != null) ? Regex.Replace(c.Description, pattern, "") : "",
                                 EntryBy = c.EntryBy,
                                 EntryDate = c.EntryDate,
                                 HSCode = c.HSCode,
                                 HSCodeReformat = c.HSCodeReformat,
                                 HSID = c.HSID,
                                 ModifiedBy = c.ModifiedBy,
                                 ModifiedDate = c.ModifiedDate,
                                 OrderMethodID = c.OrderMethodID,
                                 Status = c.Status,
                                 OMCode = o == null ? "" : o.OMCode
                             };

                    return tb.ToList();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return list;
        }

        public static List<Data.Domain.HSCodeList> GetListPerPage(string Param, int offset, int pagingsize)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Param", Param));
                parameterList.Add(new SqlParameter("@offset", offset != null ? offset : 0));
                parameterList.Add(new SqlParameter("@pagingsize", pagingsize != null ? pagingsize : 0));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.HSCodeList>("[imex].[HSCodeList_get] @Param, @offset, @pagingsize, 1", parameters).ToList();

                return data.ToList();
            }
        }

        public static int GetListCountHSCode(string Param, int offset, int pagingsize)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Param", Param));
                parameterList.Add(new SqlParameter("@offset", offset != null ? offset : 0));
                parameterList.Add(new SqlParameter("@pagingsize", pagingsize != null ? pagingsize : 0));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<int>("[imex].[SP_HSCodeList] @Param, @offset, @pagingsize, 2", parameters).FirstOrDefault();

                return data;
            }
        }

        public static List<Data.Domain.SP_HSCodeList> SP_GetListPerPage(int StartNum, int EndNum, string SeacrhName, string OrderBy)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SeacrhName", SeacrhName.Trim().ToUpper()));
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToUpper()));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_HSCodeList>("[imex].[SP_HSCodeList] 0, @SeacrhName, @StartNum, @EndNum, @OrderBy", parameters).ToList();

                return data.ToList();
            }
        }

        public static List<Int32> SP_GetCountPerPage(int StartNum, int EndNum, string SeacrhName, string OrderBy)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SeacrhName", SeacrhName.Trim().ToUpper()));
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToUpper()));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Int32>("[imex].[SP_HSCodeList] 1, @SeacrhName, @StartNum, @EndNum, @OrderBy", parameters).ToList();

                return data.ToList();
            }
        }

        //public static List<Data.Domain.HSCodeList> GetList(Domain.MasterSearchForm crit, int currentPage, int limitPage)
        //{
        //    var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

        //    var list = from c in GetListPerPage(name, currentPage, limitPage)
        //               orderby c.HSID
        //               select c;
        //    return list.ToList();
        //}


        public static Data.Domain.HSCodeList GetId(int id)
        {
            var data = new Data.Domain.HSCodeList();
            try
            {
                var pattern = @"-[^\/]";
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.HSCodeLists.Where(w => w.HSID == id).AsNoTracking().ToList()
                             from o in db.OrderMethods.Where(a => a.OMID == c.OrderMethodID).DefaultIfEmpty()
                             select new Data.Domain.HSCodeList()
                             {
                                 BeaMasuk = c.BeaMasuk,
                                 Description = (c.Description != null) ? Regex.Replace(c.Description, pattern, "") : "",
                                 EntryBy = c.EntryBy,
                                 EntryDate = c.EntryDate,
                                 HSCode = c.HSCode,
                                 HSCodeReformat = c.HSCodeReformat,
                                 HSID = c.HSID,
                                 ModifiedBy = c.ModifiedBy,
                                 ModifiedDate = c.ModifiedDate,
                                 OrderMethodID = c.OrderMethodID,
                                 Status = c.Status,
                                 OMCode = o == null ? "" : o.OMCode,
                                 ChangedOMCode = c.ChangedOMCode
                             };

                    return tb.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return data;

            //var item = GetList().Where(w => w.HSID == id).FirstOrDefault();
            //return item;
        }

        public static int Update(Data.Domain.HSCodeList itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }

            itm.ModifiedBy = Domain.SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var ret = db.CreateRepository<Data.Domain.HSCodeList>().CRUD(dml, itm);
                //return UpdateCache(itm, dml);
                return ret;
            }
        }

        public static int UpdateBulk(List<HSCodeList> list, String dml)
        {
            var ret = 0;
            using (var ts = new TransactionScope())
            {
                using (var db = new RepositoryFactory(new EfDbContext()))
                {
                    if (dml == "I")
                    {
                        foreach (var d in list)
                        {

                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;
                            try
                            {
                                ret = db.CreateRepository<Data.Domain.HSCodeList>().CRUD("I", d);
                                UpdateCache(d, dml);
                            }
                            catch //(Exception ex)
                            {
                                throw;
                            }

                        }
                    }

                    ts.Complete();
                    return ret;
                }
            }
        }

        private static object s_syncObject = new object();

        private static int UpdateCache(Data.Domain.HSCodeList item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetList().ToList();
                if (newlist.Where(w => w.HSID == item.HSID).Count() > 0)
                {
                    newlist.RemoveAll(w => w.HSID == item.HSID);
                }

                if (dml != "D")
                {
                    var om = Master.OrderMethods.GetId(item.OrderMethodID);
                    item.OMCode = om == null ? "" : om.OMCode;
                    newlist.Add(item);
                }
            }

            return 0;
        }

        public static List<Data.Domain.Extensions.HSCodeListReport> GetListReport()
        {

            List<HSCodeListReport> list = new List<HSCodeListReport>();
            try
            {
                var pattern = @"-[^\/]";
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.HSCodeLists.AsNoTracking().ToList()
                             from o in db.OrderMethods.Where(a => a.OMID == c.OrderMethodID).DefaultIfEmpty()
                             select new Data.Domain.Extensions.HSCodeListReport()
                             {
                                 BeaMasuk = c.BeaMasuk,
                                 Description = (c.Description != null) ? Regex.Replace(c.Description, pattern, "") : "",
                                 EntryBy = c.EntryBy,
                                 EntryDate = c.EntryDate,
                                 HSCode = c.HSCode,
                                 HSID = c.HSID,
                                 ModifiedBy = c.ModifiedBy,
                                 ModifiedDate = c.ModifiedDate,
                                 OrderMethodID = c.OrderMethodID,
                                 Status = c.Status,
                                 OmCode = o == null ? "" : o.OMCode
                             };

                    return tb.ToList();
                };
            }
            catch (Exception ex)
            {
                var e = ex;
            }
            return list;
        }
    }
}
