using App.Data.Caching;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.DeliveryTrackingStatus
{
    public class MasterDTS
    {
        private const string cacheName = "App.Master.DeliveryTrackingStatus";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Select2Result> GetMasterCItys()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.City>().Table.Select(e => new { Name = e.StoreName, Code = e.Code, ST = e.ST });
                // tb = tb.OrderBy(o => o.Id).ToList();
                var data = tb.ToList().Select(e => new Select2Result() { id = e.Code, text = e.Name });
                return data.ToList();
            }
        }

        public static List<Select2Result> GetMasterModa()
        { 
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Master_Moda>().Table.Select(e => new { Name = e.ModaDescription, ID = e.ModaID });

                var data = tb.ToList().Select(e => new Select2Result() { id = Convert.ToString(e.ID), text = e.Name });
                return data.ToList();
            }
        }

        public static List<Select2Result> GetMasterStatus()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Master_Status>().Table.Select(e => new { Name = e.Status, ID = e.StatusID });

                var data = tb.ToList().Select(e => new Select2Result() { id = Convert.ToString(e.ID), text = e.Name });
                return data.ToList();
            }
        }
        
        public static List<Select2Result> GetListSelectUnitType()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Master_UnitType>().Table.Select(e => new { Name = e.UnitTypeDescription, ID = e.UnitTypeID });

                var data = tb.ToList().Select(e => new Select2Result() { id = Convert.ToString(e.ID), text = e.Name });
                return data.ToList();
            }
        }

        public static Data.Domain.City GetIDCity(string storeName)
        {
            var item = GetCity().Where(w => w.StoreName.Trim().ToLower().Contains(storeName)).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Master_Moda GetIDMsModa(string moda)
        {
            var item = GetMsModa().Where(w => w.ModaDescription.Trim().ToLower() == moda.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Master_Status GetIDMsStatus(string status)
        {
            var item = GetMsStatus().Where(w => w.Status.Trim().ToLower() == status.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Master_UnitType GetIDMsUnitType(string unitType)
        {
            var item = GetMsUnitType().Where(w => w.UnitTypeDescription.Trim().ToLower() == unitType.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.Master_UnitType> GetMsUnitType()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.Master_UnitType>().Table.Select(e => e);
                return tbl.ToList();
            }
        }

        public static List<Data.Domain.Master_Status> GetMsStatus()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.Master_Status>().Table.Select(e => e);
                return tbl.ToList();
            }
        }

        public static List<Data.Domain.Master_Moda> GetMsModa()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.Master_Moda>().Table.Select(e => e);
                return tbl.ToList();
            }
        }

        public static List<Data.Domain.City> GetCity()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tbl = db.CreateRepository<Data.Domain.City>().Table.Select(e => e);
                return tbl.ToList();

            }
        }

        public static int CRUDModa(Data.Domain.Master_Moda item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.Master_Moda>().CRUD(dml, item);
            }
        }

        public static int CRUDStatus(Data.Domain.Master_Status item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.Master_Status>().CRUD(dml, item);
            }
        }

        public static int CRUDUnitType(Data.Domain.Master_UnitType item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.Master_UnitType>().CRUD(dml, item);
            }
        }
    }
}
