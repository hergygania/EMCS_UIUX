using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class InventoryAllocation
    {
        private const string cacheName = "App.CAT.InventoryAllocation";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory Allocation.
        /// </summary>
        /// <param name="KAL"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.InventoryAllocation> GetList(string KAL, string searchkey)
        {
            using (var db = new Data.EfDbContext())
            {
                List<Data.Domain.InventoryAllocation> list = db.InventoryAllocation
                    .Where( i => i.KAL == KAL 
                            && (searchkey == "" || (i.UnitNo.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()) || (i.SerialNo.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))
                    ).ToList();

                return list;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory Allocation by ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryAllocation GetByID(int ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.InventoryAllocation.Where(i => i.ID == ID).FirstOrDefault();
                return item;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory Allocation by KAL and Cycle.
        /// </summary>
        /// <param name="KAL"></param>
        /// <param name="Cycle"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryAllocation GetByKAL(string KAL, int Cycle)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.InventoryAllocation.Where(i => i.KAL == KAL && i.Cycle == Cycle).FirstOrDefault();
                return item;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory Allocation by KAL.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        public static List<Data.Domain.InventoryAllocation> GetByKAL(string KAL)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = db.InventoryAllocation.Where(i => i.KAL == KAL).ToList();
                return list;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory Allocation berdasrkan yang aktif tetapi bukan untuk ID yg sekarang.
        /// </summary>
        /// <param name="KAL"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryAllocation GetByIsActive(string KAL, int ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.InventoryAllocation.Where(i => i.KAL == KAL && i.ID != ID && i.IsActive && !i.IsUsed).FirstOrDefault();
                return item;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory Allocation yang aktif.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryAllocation GetAllocationActive(string KAL)
        {
            using (var db = new Data.EfDbContext())
            {
                return db.InventoryAllocation.Where(i => i.KAL == KAL && i.IsActive && !i.IsUsed).FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengeckan data Inventory Allocation.
        /// </summary>
        /// <param name="KAL"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string ExistAllocation(string KAL, Data.Domain.InventoryAllocation item)
        {
            if (item.UnitNo != null)
            {
                var existitem = GetList(KAL, "").Where(i => i.IsActive && !i.IsUsed &&
                    i.UnitNo == item.UnitNo && i.SerialNo == item.SerialNo && i.ID != item.ID &&
                    i.PONumber == item.PONumber).FirstOrDefault();
                if (existitem != null)
                {
                    return existitem.ID.ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Pengambilan value Cycle By KAL.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        public static int GetCycle(string KAL)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.InventoryAllocation.Where(i => i.KAL == KAL).OrderByDescending(w => w.Cycle).FirstOrDefault();
                if (item != null)
                    return item.Cycle + 1;

                return 1;
            }
        }

        /// <summary>
        /// Pengambilan value Cycle By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int GetCycleByID(int ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.InventoryAllocation.Where(i => i.ID == ID).OrderByDescending(w => w.Cycle).FirstOrDefault();
                if (item != null)
                    return item.Cycle;

                return 0;
            }
        }

        /// <summary>
        /// insert update delete data Inventory Allocation.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.InventoryAllocation item, string dml)
        {
            if (dml == "I")
            {
                item.CreatedBy = Domain.SiteConfiguration.UserName;
                item.CreatedDate = DateTime.Now;
            }

            var cust = Service.CAT.Master.MasterCustomer.GetData(Convert.ToInt32(item.CUSTOMER_ID));
            if (cust != null) item.Customer = cust.CUSTOMERNAME;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {

                if (item.IsActive)
                {
                    var active = GetAllocationActive(item.KAL);
                    if (active != null)
                    {
                        if (active.ID != item.ID)
                            UpdateCycle(active.ID, GetCycleByID(item.ID), item.KAL);
                    }

                    item.Cycle = 1;
                }

                item.UpdatedBy = Domain.SiteConfiguration.UserName;
                item.UpdatedDate = DateTime.Now;
                return db.CreateRepository<Data.Domain.InventoryAllocation>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// Update value cycle.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Cycle"></param>
        /// <param name="KAL"></param>
        private static void UpdateCycle(int ID, int Cycle, string KAL)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    db.DbContext.Database.ExecuteSqlCommand("UPDATE CAT.InventoryAllocation SET Cycle = 0, IsUsed = 1, UpdatedDate = GETDATE() WHERE ID = " + ID + "");
                }

                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    db.DbContext.Database.ExecuteSqlCommand("UPDATE CAT.InventoryAllocation SET Cycle=Cycle-1, UpdatedDate = GETDATE() WHERE KAL = '" + KAL + "' AND Cycle > " + Cycle + "");
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
