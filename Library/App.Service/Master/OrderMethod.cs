using System;
using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using System.Data.SqlClient;
namespace App.Service.Master
{
    public class OrderMethods
    {
        private const string cacheName = "App.master.OrderMethod";

        private static readonly ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<OrderMethod> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                var tbl = db.CreateRepository<OrderMethod>().TableNoTracking.ToList();

                return tbl;
            }
        }

        public static List<OrderMethod> GetList(int id)
        {
            string key = string.Format(cacheName);
            
            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                var tbl = db.CreateRepository<OrderMethod>().TableNoTracking.Where(w => w.OMID == id).ToList();

                return tbl;
            }
        }

        public static dynamic GetListDataForSelect2()
        {
            return GetList().Select(p => new { id = p.OMID, text = p.OMCode }).ToList();
        }

        public static IQueryable<Data.Domain.OrderMethodByPartNumber> getDataOM(Data.EfDbContext db, string PartNumber, string ManufacturingCode)
        {

            db.Database.CommandTimeout = 600;
            SqlParameter parameter = new SqlParameter();
            parameter.ParameterName = "@PartsNumber";
            parameter.Value = PartNumber ?? "";

            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@PartsNumber", PartNumber ?? ""));
            parameterList.Add(new SqlParameter("@ManufacturingCode", ManufacturingCode ?? ""));     
            SqlParameter[] parameters = parameterList.ToArray();

            var data = db.Database.SqlQuery<Data.Domain.OrderMethodByPartNumber>("[pis].[GetOMByPartNumber] @PartsNumber, @ManufacturingCode", parameters);
            return data.AsQueryable();

        }

        public static List<Data.Domain.OrderMethodByPartNumber> getDataOM(string PartNumber, string ManufacturingCode)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@PartsNumber";
                parameter.Value = PartNumber ?? "";

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@PartsNumber", PartNumber ?? ""));
                parameterList.Add(new SqlParameter("@ManufacturingCode", ManufacturingCode ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.OrderMethodByPartNumber>("[pis].[GetOMByPartNumber] @PartsNumber, @ManufacturingCode", parameters);
                return data.ToList();
            }
        }

        public static List<OrderMethod> GetList(MasterSearchForm crit)
        {
            string name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            IOrderedEnumerable<OrderMethod> list = from c in GetList()
                                                   where (name == "" || (c.Description).Trim().ToLower().Contains(name))
                                                   orderby c.Description
                                                   select c;
            return list.ToList();
        }

        public static int GetOMIDByCode(string OMCode)
        {
            int ID = 0;
            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                var data = db.CreateRepository<OrderMethod>().TableNoTracking.Where(p => p.OMCode.Trim().ToUpper() == OMCode.Trim().ToUpper()).FirstOrDefault();
                if (data != null)
                    ID = data.OMID;
            }
            return ID;
        }

        public static OrderMethod GetId(int? id)
        {
            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                var tbl = db.CreateRepository<OrderMethod>().TableNoTracking.Where(w => w.OMID == id).FirstOrDefault();

                return tbl;
            }
        }

        public static int Update(OrderMethod itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }

            itm.ModifiedBy = SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                return db.CreateRepository<OrderMethod>().CRUD(dml, itm);
            }
        }
    }
}