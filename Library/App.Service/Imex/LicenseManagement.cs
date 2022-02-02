using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using App.Data.Caching;
using App.Data.Domain;
using System.Data.SqlClient;

namespace App.Service.Imex
{
    public class Licenses
    {
        private const string cacheName = "App.imex.LicenseManagement";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        private static List<Data.Domain.LicenseManagement> RefreshList()
        {

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.LicenseManagements.AsNoTracking().ToList()
                           from grp in Service.Master.LicenseGroup.GetList(c.GroupID).DefaultIfEmpty()
                           from por in Service.Master.LicensePorts.GetList(c.PortsID ?? 0).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList(c.OM ?? 0).DefaultIfEmpty()
                           select new Data.Domain.LicenseManagement()
                           {
                               LicenseManagementID = c.LicenseManagementID,
                               Description = c.Description,
                               LicenseNumber = c.LicenseNumber,
                               Status = c.Status,
                               Serie = grp == null ? "" : grp.Serie,
                               GroupID = c.GroupID,
                               PortsID = c.PortsID,
                               Ports = c.Ports,
                               ReleaseDate = c.ReleaseDate,
                               ExpiredDate = c.ExpiredDate,
                               Validity = c.Validity,
                               Quota = c.Quota,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               GroupName = grp == null ? "" : grp.Description,
                               PortsName = por == null ? "" : por.Description,
                               OM = c.OM,
                               OMCode = om == null ? "" : om.OMCode,
                               RegulationCode = c.RegulationCode,
                               ListHSCode = db.LicenseManagementsHS.Where(p => p.LicenseID == c.LicenseManagementID).ToList(),
                               ListPartNumber = db.LicenseManagementsPartNumber.Where(p => p.LicenseID == c.LicenseManagementID).ToList()
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.LicenseManagement> GetList()
        {
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            //    return RefreshList();
            //});
            //return list;
            return RefreshList();
        }

        public static List<Data.Domain.LicenseManagement> GetListById(int Id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.LicenseManagements.AsNoTracking().Where(w => w.LicenseManagementID == Id).ToList()
                           from grp in Service.Master.LicenseGroup.GetList(c.GroupID).DefaultIfEmpty()
                           from por in Service.Master.LicensePorts.GetList(c.PortsID ?? 0).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList(c.OM ?? 0).DefaultIfEmpty()
                           select new Data.Domain.LicenseManagement()
                           {
                               LicenseManagementID = c.LicenseManagementID,
                               Description = c.Description,
                               LicenseNumber = c.LicenseNumber,
                               Status = c.Status,
                               Serie = grp == null ? "" : grp.Serie,
                               GroupID = c.GroupID,
                               PortsID = c.PortsID,
                               Ports = c.Ports,
                               ReleaseDate = c.ReleaseDate,
                               ExpiredDate = c.ExpiredDate,
                               Validity = c.Validity,
                               Quota = c.Quota,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               GroupName = grp == null ? "" : grp.Description,
                               PortsName = por == null ? "" : por.Description,
                               OM = c.OM,
                               OMCode = om == null ? "" : om.OMCode,
                               RegulationCode = c.RegulationCode,
                               ListHSCode = db.LicenseManagementsHS.Where(p => p.LicenseID == c.LicenseManagementID).ToList(),
                               ListPartNumber = db.LicenseManagementsPartNumber.Where(p => p.LicenseID == c.LicenseManagementID).ToList()
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.LicenseManagement> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = GetList();
            return list.ToList();
        }

        public static List<Data.Domain.LicenseManagement> GetList(int? Status, string LicenseNumber, string Description, DateTime? ReleaseDate, DateTime? ExpiredDate,
           IEnumerable<string> selSerie, IEnumerable<string> selGroup, IEnumerable<string> selPorts, IEnumerable<string> selOM)
        {
            #region Handel NULL
            Status = Status ?? 0;
            if (string.IsNullOrWhiteSpace(LicenseNumber)) LicenseNumber = "";
            if (string.IsNullOrWhiteSpace(Description)) Description = "";
            ReleaseDate = ReleaseDate ?? DateTime.MinValue;
            ExpiredDate = ExpiredDate ?? DateTime.MinValue;
            selGroup = selGroup ?? new List<string>();
            selSerie = selSerie ?? new List<string>();
            selPorts = selPorts ?? new List<string>();
            selOM = selOM ?? new List<string>();
            #endregion

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.LicenseManagements.AsNoTracking().ToList()
                           from grp in Service.Master.LicenseGroup.GetList(c.GroupID).DefaultIfEmpty()
                           from por in Service.Master.LicensePorts.GetList(c.PortsID ?? -1).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList(c.OM ?? -1).DefaultIfEmpty()
                           where (Status <= 0 || (Status > 0 && c.Status == Status.Value)) &&
                                (LicenseNumber == "" || (LicenseNumber != "" && c.LicenseNumber.Trim().ToLower().Contains(LicenseNumber.Trim().ToLower()))) &&
                                (Description == "" || (Description != "" && c.Description.Trim().ToLower().Contains(Description.Trim().ToLower()))) &&
                                (ReleaseDate == DateTime.MinValue || (ReleaseDate != DateTime.MinValue && c.ReleaseDate >= ReleaseDate)) &&
                                (ExpiredDate == DateTime.MinValue || (ExpiredDate != DateTime.MinValue && c.ExpiredDate <= ExpiredDate)) &&
                                (selSerie.Count() <= 0 || (selSerie.Count() > 0 && selSerie.Any(a => a == c.Serie.ToString()))) &&
                                (selGroup.Count() <= 0 || (selGroup.Count() > 0 && selGroup.Any(a => a == c.GroupID.ToString()))) &&
                                (selPorts.Count() <= 0 || (selPorts.Count() > 0 && selPorts.Any(a => a == c.PortsID.ToString()))) &&
                                (selOM.Count() <= 0 || (selOM.Count() > 0 && selOM.Any(a => a == c.OM.ToString())))
                           select new Data.Domain.LicenseManagement()
                           {
                               LicenseManagementID = c.LicenseManagementID,
                               Description = c.Description,
                               LicenseNumber = c.LicenseNumber,
                               Status = c.Status,
                               Serie = grp == null ? "" : grp.Serie,
                               GroupID = c.GroupID,
                               PortsID = c.PortsID,
                               Ports = c.Ports,
                               ReleaseDate = c.ReleaseDate,
                               ExpiredDate = c.ExpiredDate,
                               Validity = c.Validity,
                               Quota = c.Quota,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               GroupName = grp == null ? "" : grp.Description,
                               PortsName = por == null ? "" : por.Description,
                               OM = c.OM,
                               OMCode = om == null ? "" : om.OMCode,
                               RegulationCode = c.RegulationCode,
                               ListHSCode = db.LicenseManagementsHS.Where(p => p.LicenseID == c.LicenseManagementID).ToList(),
                               ListPartNumber = db.LicenseManagementsPartNumber.Where(p => p.LicenseID == c.LicenseManagementID).ToList()
                           };

                return list.ToList();
            }
        }

        public static Data.Domain.LicenseManagement GetId(int id)
        {
            if (id == 0)
                return new Data.Domain.LicenseManagement();

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.LicenseManagements.AsNoTracking().Where(w => w.LicenseManagementID == id).ToList()
                           from grp in Service.Master.LicenseGroup.GetList(c.GroupID).DefaultIfEmpty()
                           from por in Service.Master.LicensePorts.GetList(c.PortsID ?? 0).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList(c.OM ?? 0).DefaultIfEmpty()
                           select new Data.Domain.LicenseManagement()
                           {
                               LicenseManagementID = c.LicenseManagementID,
                               Description = c.Description,
                               LicenseNumber = c.LicenseNumber,
                               Status = c.Status,
                               Serie = grp == null ? "" : grp.Serie,
                               GroupID = c.GroupID,
                               PortsID = c.PortsID,
                               Ports = c.Ports,
                               ReleaseDate = c.ReleaseDate,
                               ExpiredDate = c.ExpiredDate,
                               Validity = c.Validity,
                               Quota = c.Quota,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               GroupName = grp == null ? "" : grp.Description,
                               PortsName = por == null ? "" : por.Description,
                               OM = c.OM,
                               OMCode = om == null ? "" : om.OMCode,
                               RegulationCode = c.RegulationCode,
                               ListHSCode = db.LicenseManagementsHS.Where(p => p.LicenseID == c.LicenseManagementID).ToList(),
                               ListPartNumber = db.LicenseManagementsPartNumber.Where(p => p.LicenseID == c.LicenseManagementID).ToList()
                           };

                return list.FirstOrDefault();
            }
        }

        public static List<Data.Domain.LicenseManagement> GetListHistory(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.LicenseManagementHistories.AsNoTracking().Where(w => w.LicenseManagementID == id).ToList()
                           from grp in Service.Master.LicenseGroup.GetList(c.GroupID).Where(w => c.GroupID > 0).DefaultIfEmpty()
                           from por in Service.Master.LicensePorts.GetList(c.PortsID).Where(w => c.PortsID > 0).DefaultIfEmpty()
                           select new Data.Domain.LicenseManagement()
                           {
                               LicenseManagementID = c.LicenseManagementID,
                               Description = c.Description,
                               LicenseNumber = c.LicenseNumber,
                               Status = c.Status,
                               Serie = (grp == null ? "" : grp.Serie),
                               GroupID = c.GroupID,
                               PortsID = c.PortsID,
                               Ports = c.Ports,
                               ReleaseDate = c.ReleaseDate,
                               ExpiredDate = c.ExpiredDate,
                               Validity = c.Validity,
                               Quota = c.Quota,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               GroupName = grp == null ? "" : grp.Description,
                               PortsName = por == null ? "" : por.Description
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.LicenseManagementHS> GetListLicenseHSByLicenseID(int LicenseId)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.LicenseManagementsHS.Where(p => p.LicenseID == LicenseId).ToList();
            }
        }

        public static List<Data.Domain.LicenseManagementPartNumber> GetListLicensePartNumberByLicenseID(int LicenseId)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.LicenseManagementsPartNumber.Where(p => p.LicenseID == LicenseId).ToList();
            }
        }

        public static List<Data.Domain.LicenseManagementDetailExtend> GetListDetailExtend(int LicenseID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@LicenseID", LicenseID));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.LicenseManagementDetailExtend>("[imex].[spGetLicenseDetailExtend] @LicenseID", parameters).ToList();

                return data;
            }
        }

        #region extend

        public static List<Data.Domain.LicenseManagementExtend> GetExtendList(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from p in db.LicenseManagementExtends.AsNoTracking().Where(w => w.LicenseManagementID == id).ToList()
                           select new Data.Domain.LicenseManagementExtend()
                           {
                               ExtendID = p.ExtendID,
                               LicenseManagementID = p.LicenseManagementID,
                               ApplyDate = p.ApplyDate,
                               NewReleaseDate = p.NewReleaseDate,
                               NewExpiredDate = p.NewExpiredDate,
                               NewValidity = p.NewValidity,
                               NewQuota = p.NewQuota,
                               Requirement = p.Requirement,
                               EntryDate = p.EntryDate,
                               ModifiedDate = p.ModifiedDate,
                               EntryBy = p.EntryBy,
                               ModifiedBy = p.ModifiedBy,
                               RegulationCode = db.LicenseManagements.Where(w => w.LicenseManagementID == p.LicenseManagementID).Select(s => s.RegulationCode).FirstOrDefault(),
                               HSCode = db.LicenseManagementsHS.Where(w => w.LicenseID == p.LicenseManagementID).Select(s => s.HSCode).FirstOrDefault(),
                               PartNumber = db.LicenseManagementsPartNumber.Where(w => w.LicenseID == p.LicenseManagementID).Select(s => s.PartNumber).FirstOrDefault()
                           };

                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            }
        }

        public static List<Data.Domain.LicenseManagementExtendComment> GetExtendCommentList(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = db.LicenseManagementExtendComments.AsNoTracking().Where(w => w.LicenseManagementID == id);
                return list.OrderByDescending(o => o.EntryDate).Take(10).ToList();
            }
        }

        public static Data.Domain.LicenseManagementExtend GetExtendId(int id)
        {
            var list = GetExtendList(id).OrderByDescending(o => o.ModifiedDate).ToList();
            if (list.Count > 0)
                return list[0];
            else
                return new Data.Domain.LicenseManagementExtend();
        }
        #endregion

        public static int Update(Data.Domain.LicenseManagement itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    //int id = itm.LicenseManagementID;
                    //try { id = db.CreateRepository<Data.Domain.LicenseManagement>().GetAll().Max(c => c.LicenseManagementID); }
                    //catch { }

                    //itm.LicenseManagementID = id + 1;

                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                if (dml != "I")
                {
                    try
                    {
                        var his = GetId(itm.LicenseManagementID);
                        var itemHis = new Data.Domain.LicenseManagementHistory
                        {
                            HistoryID = 0,
                            LicenseManagementID = his.LicenseManagementID,
                            GroupID = his.GroupID,
                            PortsID = his.PortsID.HasValue ? his.PortsID.Value : 0,
                            Ports = his.Ports,
                            Description = his.Description,
                            LicenseNumber = his.LicenseNumber,
                            ReleaseDate = his.ReleaseDate,
                            ExpiredDate = his.ExpiredDate,
                            Validity = his.Validity,
                            Quota = his.Quota,
                            Status = his.Status,
                            EntryDate = his.EntryDate,
                            ModifiedDate = his.ModifiedDate,
                            EntryBy = his.EntryBy,
                            ModifiedBy = his.ModifiedBy
                        };

                        #region delete HS & Part Number
                        var dataHS = GetListLicenseHSByLicenseID(his.LicenseManagementID);
                        if (dataHS.Count() > 0)
                        {

                            foreach (var data in dataHS)
                            {
                                db.CreateRepository<Data.Domain.LicenseManagementHS>().Delete(data);
                            }
                        }

                        var dataPartNumber = GetListLicensePartNumberByLicenseID(his.LicenseManagementID);
                        if (dataPartNumber.Count() > 0)
                        {

                            foreach (var data in dataPartNumber)
                            {
                                db.CreateRepository<Data.Domain.LicenseManagementPartNumber>().Delete(data);
                            }
                        }
                        #endregion

                        db.CreateRepository<Data.Domain.LicenseManagementHistory>().CRUD("I", itemHis);
                    }
                    catch { }
                }

                var resutl = db.CreateRepository<Data.Domain.LicenseManagement>().CRUD(dml, itm);

                if (itm.ListHSCode != null)
                {
                    foreach (var model in itm.ListHSCode)
                    {
                        model.LicenseID = itm.LicenseManagementID;
                        model.RegulationCode = itm.RegulationCode;
                        model.EntryBy = userName;
                        model.EntryDate = DateTime.Now;
                        model.ModifiedBy = userName;
                        model.ModifiedDate = DateTime.Now;

                        UpdateHS(model);
                    }
                }
                if (itm.ListPartNumber != null)
                {
                    foreach (var model in itm.ListPartNumber)
                    {
                        model.LicenseID = itm.LicenseManagementID;
                        model.RegulationCode = itm.RegulationCode;
                        model.EntryBy = userName;
                        model.ModifiedDate = DateTime.Now;
                        model.ModifiedBy = userName;
                        model.ModifiedDate = DateTime.Now;

                        UpdatePartNumber(model);
                    }
                }

                _cacheManager.Remove(cacheName);

                return resutl;
            }
        }

        public static int UpdateFromExtend(Data.Domain.LicenseManagement itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    //int id = itm.LicenseManagementID;
                    //try { id = db.CreateRepository<Data.Domain.LicenseManagement>().GetAll().Max(c => c.LicenseManagementID); }
                    //catch { }

                    //itm.LicenseManagementID = id + 1;

                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                if (dml != "I")
                {
                    try
                    {
                        var his = GetId(itm.LicenseManagementID);
                        var itemHis = new Data.Domain.LicenseManagementHistory
                        {
                            HistoryID = 0,
                            LicenseManagementID = his.LicenseManagementID,
                            GroupID = his.GroupID,
                            PortsID = his.PortsID.HasValue ? his.PortsID.Value : 0,
                            Description = his.Description,
                            LicenseNumber = his.LicenseNumber,
                            ReleaseDate = his.ReleaseDate,
                            ExpiredDate = his.ExpiredDate,
                            Validity = his.Validity,
                            Quota = his.Quota,
                            Status = his.Status,
                            EntryDate = his.EntryDate,
                            ModifiedDate = his.ModifiedDate,
                            EntryBy = his.EntryBy,
                            ModifiedBy = his.ModifiedBy
                        };
                              
                        db.CreateRepository<Data.Domain.LicenseManagementHistory>().CRUD("I", itemHis);
                    }
                    catch { }
                }

                var resutl = db.CreateRepository<Data.Domain.LicenseManagement>().CRUD(dml, itm);
                
                _cacheManager.Remove(cacheName);

                return resutl;
            }
        }

        private static int UpdateHS(Data.Domain.LicenseManagementHS model)
        {
            string userName = Domain.SiteConfiguration.UserName;
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (model != null)
                {
                    model.EntryBy = userName;
                    model.EntryDate = DateTime.Now;
                    model.ModifiedBy = userName;
                    model.ModifiedDate = DateTime.Now;

                    return db.CreateRepository<Data.Domain.LicenseManagementHS>().CRUD("I", model);
                }

                return 0;
            }
        }

        private static int UpdatePartNumber(Data.Domain.LicenseManagementPartNumber model)
        {
            string userName = Domain.SiteConfiguration.UserName;
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (model != null)
                {
                    model.EntryBy = userName;
                    model.EntryDate = DateTime.Now;
                    model.ModifiedBy = userName;
                    model.ModifiedDate = DateTime.Now;

                    return db.CreateRepository<Data.Domain.LicenseManagementPartNumber>().CRUD("I", model);
                }

                return 0;
            }
        }

        public static int UpdateBulk(List<Data.Domain.LicenseManagement> list, string dml)
        {
            var ret = 0;
            string userName = Domain.SiteConfiguration.UserName;

            using (TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {

                    if (dml == "I")
                    {
                        foreach (var d in list)
                        {
                            //int id = 0;
                            //try { id = db.CreateRepository<Data.Domain.LicenseManagement>().GetAll().Max(c => c.LicenseManagementID); }
                            //catch { }

                            //d.LicenseManagementID = id + 1;
                            d.Status = 1;
                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;

                            ret = db.CreateRepository<Data.Domain.LicenseManagement>().CRUD("I", d);

                            if (d.ListHSCode != null)
                            {
                                foreach (var model in d.ListHSCode)
                                {
                                    model.LicenseID = d.LicenseManagementID;
                                    model.RegulationCode = d.RegulationCode;
                                    model.EntryBy = userName;
                                    model.EntryDate = DateTime.Now;
                                    model.ModifiedBy = userName;
                                    model.ModifiedDate = DateTime.Now;

                                    UpdateHS(model);
                                }
                            }
                            if (d.ListPartNumber != null)
                            {
                                foreach (var model in d.ListPartNumber)
                                {
                                    var parts = Service.Master.PartsLists.GetListGroupByPartNumber().Where(w => w.PartsNumber == model.PartNumber);
                                    if (parts != null)
                                    {
                                        foreach (var p in parts)
                                        {
                                            model.LicenseID = d.LicenseManagementID;
                                            model.RegulationCode = d.RegulationCode;
                                            model.ManufacturingCode = p.ManufacturingCode;
                                            model.EntryBy = userName;
                                            model.ModifiedDate = DateTime.Now;
                                            model.ModifiedBy = userName;
                                            model.ModifiedDate = DateTime.Now;

                                            UpdatePartNumber(model);
                                        }
                                    }
                                    else
                                    {
                                        model.LicenseID = d.LicenseManagementID;
                                        model.RegulationCode = d.RegulationCode;
                                        model.ManufacturingCode = "NULL";
                                        model.EntryBy = userName;
                                        model.ModifiedDate = DateTime.Now;
                                        model.ModifiedBy = userName;
                                        model.ModifiedDate = DateTime.Now;

                                        UpdatePartNumber(model);
                                    }
                                }
                            }

                        }
                    }

                    ts.Complete();
                    _cacheManager.Remove(cacheName);
                    return ret;
                }
            }
        }

        public static int UpdateExtend(Data.Domain.LicenseManagementExtend itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }


                return db.CreateRepository<Data.Domain.LicenseManagementExtend>().CRUD(dml, itm);
            }
        }

        public static int UpdateExtendComment(Data.Domain.LicenseManagementExtendComment itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                return db.CreateRepository<Data.Domain.LicenseManagementExtendComment>().CRUD(dml, itm);
            }
        }

    }
}
