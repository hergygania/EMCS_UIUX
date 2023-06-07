using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Imex
{
    public class RegulationManagementDetail
    {
        private const string cacheName = "App.imex.RegulationManagementDetail";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        private static List<Data.Domain.RegulationManagementDetail> RefreshList()
        {

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagementDetails.AsNoTracking().ToList()
                           from reg in Service.Imex.RegulationManagement.GetList().Where(w => w.ID == c.RegulationManagementID)
                           from hs in Service.Master.HSCodeLists.GetList().Where(w => w.HSID == c.HSID)
                           from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == reg.OM).DefaultIfEmpty()  //c.OMID)
                           from lar in Service.Master.Lartas.GetList().Where(w => w.LartasID == c.LartasId).DefaultIfEmpty()
                           from lic in Service.Imex.Licenses.GetList().Where(w => w.LicenseManagementID == c.LicenseManagementID).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagementDetail()
                           {
                               RegulationManagementID = c.RegulationManagementID,
                               DetailID = c.DetailID,
                               HSID = c.HSID,
                               OMID = c.OMID,
                               LicenseManagementID = c.LicenseManagementID,
                               Status = c.Status,
                               QtyOfParts = c.QtyOfParts,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               HSCode = hs.HSCode.ToString(),
                               HSDescription = hs.Description,
                               HSCodeCap = hs.HSCode.ToString() + " ~ " + ("" + hs.Description).Replace(".", ""),
                               OMCode = om == null ? "" : om.OMCode,
                               LicenseNumber = lic == null ? "" : lic.LicenseNumber,
                               //IssuedBy = reg.IssuedBy,
                               //IssuedDate = reg.IssuedDate,
                               LartasId = c.LartasId,
                               Regulation = reg.Regulation,
                               LartasDesc = lar == null ? " " : lar.Description
                           };

                return list.ToList();
            }
        }

        //public static List<Data.Domain.RegulationManagementDetail> GetList()
        //{
        //    string key = string.Format(cacheName);

        //    var list = _cacheManager.Get(key, () =>
        //    {
        //        return RefreshList();
        //    });
        //    return list;
        //}

        public static List<Data.Domain.RegulationManagementDetail> GetList()
        {
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            //    return RefreshList();
            //});
            //return list;
            return RefreshList().ToList();
        }

        public static List<Data.Domain.RegulationManagementDetail> GetList(int ID)
        {

            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.RegulationManagementDetails.AsNoTracking().Where(w => w.DetailID == ID).ToList()
                           from reg in Service.Imex.RegulationManagement.GetList(c.RegulationManagementID).DefaultIfEmpty()
                           from hs in Service.Master.HSCodeLists.GetListById(c.HSID).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList(reg.OM ?? -1).DefaultIfEmpty()  //c.OMID)
                           from lar in Service.Master.Lartas.GetListById(c.LartasId ?? -1).DefaultIfEmpty()
                           from lic in Service.Imex.Licenses.GetListById(c.LicenseManagementID ?? -1).DefaultIfEmpty()
                           select new Data.Domain.RegulationManagementDetail()
                           {
                               RegulationManagementID = c.RegulationManagementID,
                               DetailID = c.DetailID,
                               HSID = c.HSID,
                               OMID = c.OMID,
                               LicenseManagementID = c.LicenseManagementID,
                               Status = c.Status,
                               QtyOfParts = c.QtyOfParts,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               HSCode = hs.HSCode.ToString(),
                               HSDescription = hs.Description,
                               HSCodeCap = hs.HSCode.ToString() + " ~ " + ("" + hs.Description).Replace(".", ""),
                               OMCode = om == null ? "" : om.OMCode,
                               LicenseNumber = lic == null ? "" : lic.LicenseNumber,
                               //IssuedBy = reg.IssuedBy,
                               //IssuedDate = reg.IssuedDate,
                               LartasId = c.LartasId,
                               Regulation = reg.Regulation,
                               LartasDesc = lar == null ? " " : lar.Description
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.RegulationManagementDetail> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = GetList();
            return list.ToList();
        }

        public static Data.Domain.RegulationManagementDetail GetId(int id)
        {
            if (id == 0)
                return new Data.Domain.RegulationManagementDetail();

            var item = GetList().Where(w => w.DetailID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.RegulationManagementDetail itm, string dml)
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

                db.CreateRepository<Data.Domain.RegulationManagementDetail>().CRUD(dml, itm);
                return UpdateCache(itm, dml);
            }
        }

        public static int UpdateBulk(List<Data.Domain.RegulationManagementDetail> list, string dml, ref string msg)
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
                            //long id = 0;
                            //try { id = db.CreateRepository<Data.Domain.RegulationManagementDetail>().GetAll().Max(c => c.DetailID); }
                            //catch { }
                            //d.DetailID = id + 1;

                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;
                            d.Status = 1;

                            msg = "regulation:" + d.Regulation + ", hscode:" + d.HSCode.ToString() + ", LicenseNumber:" + d.LicenseNumber;

                            ret = db.CreateRepository<Data.Domain.RegulationManagementDetail>().CRUD("I", d);

                            UpdateCache(d, dml);
                        }
                    }

                    ts.Complete();
                    return ret;
                }
            }
        }

        private static object s_syncObject = new object();

        private static int UpdateCache(Data.Domain.RegulationManagementDetail item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetList().ToList();
                if (newlist.Where(w => w.DetailID == item.DetailID).Count() > 0)
                {
                    newlist.RemoveAll(w => w.DetailID == item.DetailID);
                }

                if (dml != "D")
                {
                    var reg = Service.Imex.RegulationManagement.GetId(item.RegulationManagementID);
                    var hs = Service.Master.HSCodeLists.GetId(item.HSID);
                    var om = Service.Master.OrderMethods.GetId(reg.OM);// item.OMID);
                    var lic = new Data.Domain.LicenseManagement();
                    var lar = new Data.Domain.Lartas();
                    if (item.LicenseManagementID.HasValue)
                        lic = Service.Imex.Licenses.GetId(item.LicenseManagementID.Value);
                    if (item.LartasId.HasValue)
                        lar = Service.Master.Lartas.GetId(item.LartasId.Value);

                    item.HSCode = hs.HSCode.ToString();
                    item.HSDescription = hs.Description;
                    item.HSCodeCap = hs.HSCode.ToString() + " ~ " + ("" + hs.Description).Replace(".", "");
                    item.OMCode = om == null ? "" : om.OMCode;
                    item.LicenseNumber = lic == null ? "" : lic.LicenseNumber;
                    //item.IssuedBy = reg.IssuedBy;
                    //item.IssuedDate = reg.IssuedDate;
                    //item.LartasId = reg.LartasId;
                    item.LartasDesc = lar == null ? " " : lar.Description;
                    item.Regulation = reg.Regulation;

                    newlist.Add(item);
                }

                _cacheManager.Remove(cacheName);
                _cacheManager.Set(cacheName, newlist);
            }

            return 0;
        }

        public static bool IfExist(int HSID, string RegulationCode)
        {
            using (var db = new Data.EfDbContext())
            {
                var data = db.RegulationManagementDetails.AsNoTracking().Where(w => w.HSID == HSID && w.RegulationCode == RegulationCode).FirstOrDefault();
                if (data != null) return true;

                return false;
            }
        }

    }
}
