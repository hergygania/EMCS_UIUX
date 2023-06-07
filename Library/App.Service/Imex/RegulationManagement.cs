using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using App.Data.Caching;
using System.Data.SqlClient;

namespace App.Service.Imex
{
    public class RegulationManagement
    {
        private const string cacheName = "App.imex.RegulationManagement";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        //private static List<Data.Domain.RegulationManagement> RefreshList()
        //{

        //    using (var db = new Data.EfDbContext())
        //    {
        //        var list = from c in db.RegulationManagements.AsNoTracking().ToList()
        //                             //from lar in Service.Master.Lartas.GetList().Where(w => w.LartasID == c.LartasId)
        //                             from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OMID).DefaultIfEmpty()
        //                             select new Data.Domain.RegulationManagement()
        //                             {
        //                                 RegulationManagementID = c.RegulationManagementID,
        //                                 Regulation = c.Regulation,
        //                                 Description = c.Description,
        //                                 Status = c.Status,
        //                                 IssuedBy = c.IssuedBy,
        //                                 IssuedDate = c.IssuedDate,
        //                                 OMID = c.OMID,
        //                                 EntryDate = c.EntryDate,
        //                                 ModifiedDate = c.ModifiedDate,
        //                                 EntryBy = c.EntryBy,
        //                                 ModifiedBy = c.ModifiedBy,
        //                                 OMCode = om ==null ? "" : om.OMCode
        //                                 //LartasId=c.LartasId,
        //                                 //LartasDesc = lar.Description,
        //                             };

        //        return list.ToList();
        //    }
        //}

        private static List<Data.Domain.RegulationManagement> RefreshList()
        {

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagements.AsNoTracking().ToList()
                               //from lar in Service.Master.Lartas.GetList().Where(w => w.LartasID == c.LartasId)
                           from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OM).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagement()
                           {
                               ID = c.ID,
                               NoPermitCategory = c.NoPermitCategory,
                               CodePermitCategory = c.CodePermitCategory,
                               PermitCategoryName = c.PermitCategoryName,
                               HSCode = c.HSCode,
                               Lartas = c.Lartas,
                               Permit = c.Permit,
                               Regulation = c.Regulation,
                               Date = c.Date,
                               Description = c.Description,
                               OM = c.OM,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               OMCode = om == null ? "" : om.OMCode
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.RegulationManagement> GetList()
        {
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            return RefreshList();
            //});
            //return list;
        }

        public static List<Data.Domain.RegulationManagement> GetList(int ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagements.AsNoTracking().Where(w => w.ID == ID).ToList()
                               //from lar in Service.Master.Lartas.GetList().Where(w => w.LartasID == c.LartasId)
                           from om in Service.Master.OrderMethods.GetList(c.OM ?? -1).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagement()
                           {
                               ID = c.ID,
                               NoPermitCategory = c.NoPermitCategory,
                               CodePermitCategory = c.CodePermitCategory,
                               PermitCategoryName = c.PermitCategoryName,
                               HSCode = c.HSCode,
                               Lartas = c.Lartas,
                               Permit = c.Permit,
                               Regulation = c.Regulation,
                               Date = c.Date,
                               Description = c.Description,
                               OM = c.OM,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               OMCode = om == null ? "" : om.OMCode
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.RegulationManagement> GetList(string Regulation, List<string> ListCodePermitCategory, List<string> ListHSCode, List<int?> ListOM)
        {
            if (ListCodePermitCategory == null) ListCodePermitCategory = new List<string>();
            if (ListHSCode == null) ListHSCode = new List<string>();
            if (ListOM == null) ListOM = new List<int?>();
            if (string.IsNullOrWhiteSpace(Regulation)) Regulation = "";

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagements.AsNoTracking()
                           .Where(w => (Regulation == "" || (Regulation != "" && w.Regulation.Contains(Regulation))) &&
                                    (ListCodePermitCategory.Count <= 0 || (ListCodePermitCategory.Count > 0 && ListCodePermitCategory.Contains(w.CodePermitCategory))) &&
                                    (ListHSCode.Count <= 0 || (ListHSCode.Count > 0 && ListHSCode.Contains(w.HSCode))) &&
                                    (ListOM.Count <= 0 || (ListOM.Count > 0 && ListOM.Contains(w.OM)))
                           ).ToList()
                           from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OM).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagement()
                           {
                               ID = c.ID,
                               NoPermitCategory = c.NoPermitCategory,
                               CodePermitCategory = c.CodePermitCategory,
                               PermitCategoryName = c.PermitCategoryName,
                               HSCode = c.HSCode,
                               Lartas = c.Lartas,
                               Permit = c.Permit,
                               Regulation = c.Regulation,
                               Date = c.Date,
                               Description = c.Description,
                               OM = c.OM,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               OMCode = om == null ? "" : om.OMCode
                           };

                return list.Distinct().OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.HSCode).ToList();
            }
        }

        public static List<Data.Domain.RegulationManagement> GetHSListByRegulationCode(string RegulationCode)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.RegulationManagements.AsNoTracking().Where(w => w.CodePermitCategory.Trim().ToUpper() == RegulationCode.Trim().ToUpper()).ToList();
            }
        }

        public static List<App.Domain.Select2Result> GetSelect2IssueBy()
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagements.AsNoTracking().ToList().GroupBy(g => g.Permit).Select(s => new { issueBy = s.Key })
                           select new App.Domain.Select2Result()
                           {
                               id = c.issueBy,
                               text = c.issueBy
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.ViewRegulationManagementHeader> GetListHeader()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var data = db.DbContext.Database.SqlQuery<Data.Domain.ViewRegulationManagementHeader>("[pis].[GetRegulationManagementHeader]").ToList();
                return data;
            }
        }

        public static List<Data.Domain.RegulationManagement> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = GetList();
            return list.ToList();
        }

        public static Data.Domain.RegulationManagement GetId(int id)
        {
            if (id == 0)
                return new Data.Domain.RegulationManagement();

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagements.AsNoTracking().Where(w => w.ID == id).ToList()
                               //from lar in Service.Master.Lartas.GetList().Where(w => w.LartasID == c.LartasId)
                           from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OM).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagement()
                           {
                               ID = c.ID,
                               NoPermitCategory = c.NoPermitCategory,
                               CodePermitCategory = c.CodePermitCategory,
                               PermitCategoryName = c.PermitCategoryName,
                               HSCode = c.HSCode,
                               Lartas = c.Lartas,
                               Permit = c.Permit,
                               Regulation = c.Regulation,
                               Date = c.Date,
                               Description = c.Description,
                               OM = c.OM,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               OMCode = om == null ? "" : om.OMCode
                           };

                return list.FirstOrDefault();
            }

            //var item = GetList().Where(w => w.ID == id).FirstOrDefault();
            //return item;
        }

        public static Data.Domain.RegulationManagement GetByNoPermitCategory(int NoPermitCategory)
        {
            if (NoPermitCategory == 0)
                return new Data.Domain.RegulationManagement();

            using (var db = new Data.EfDbContext())
            {
                return db.RegulationManagements.AsNoTracking().Where(w => w.NoPermitCategory == NoPermitCategory).FirstOrDefault();
            }
        }

        //public static Data.Domain.RegulationManagement GetId(int id)
        //{
        //    if (id == 0)
        //        return new Data.Domain.RegulationManagement();

        //    var item = GetList().Where(w => w.RegulationManagementID == id).FirstOrDefault();
        //    return item;
        //}

        public static int Update(Data.Domain.RegulationManagement itm, string dml)
        {

            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    //int id = itm.RegulationManagementID;
                    //try { id = db.CreateRepository<Data.Domain.RegulationManagement>().GetAll().Max(c => c.RegulationManagementID); }
                    //catch { }

                    //itm.RegulationManagementID = id + 1;

                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                _cacheManager.Remove(cacheName);

                return db.CreateRepository<Data.Domain.RegulationManagement>().CRUD(dml, itm);
            }
        }

        public static int UpdateBulk(List<Data.Domain.RegulationManagement> list, string dml)
        {
            var ret = 0;
            using (TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {

                    if (dml == "I")
                    {
                        foreach (var d in list)
                        {
                            //int id = 0;
                            //try { id = db.CreateRepository<Data.Domain.RegulationManagement>().GetAll().Max(c => c.RegulationManagementID); }
                            //catch { }

                            //d.RegulationManagementID = id + 1;
                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;

                            ret = db.CreateRepository<Data.Domain.RegulationManagement>().CRUD("I", d);
                        }
                    }

                    ts.Complete();
                    _cacheManager.Remove(cacheName);
                    return ret;
                }
            }
        }

        public static void DeleteTruncate()
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    db.DbContext.Database.ExecuteSqlCommand("truncate table [pis].[RegulationManagement]");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.InnerException.Message);
            }
        }

    }
}
