using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class Staging
    {
        /// <summary>
        /// Get Data Staging 4Bn48R.</summary>
        public static List<Data.Domain.Staging4Bn48R> GetListStaging4Bn48R()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.Staging4Bn48R select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging CORE.</summary>
        public static List<Data.Domain.StagingInventoryAdjustment> GetListStagingIA()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingIA select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging CORE.</summary>
        public static List<Data.Domain.StagingCORE> GetListStagingCORE()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCORE select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create BER.</summary>
        public static List<Data.Domain.StagingCreateBER> GetListStagingCreateBER()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateBER select a;
                return tb.OrderByDescending(o => o.ModifiedDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create JC.</summary>
        public static List<Data.Domain.StagingCreateJC> GetListStagingCreateJC()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateJC select a;
                return tb.OrderByDescending(o => o.ModifiedDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create MO.</summary>
        public static List<Data.Domain.StagingCreateMO> GetListStagingCreateMO()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateMO select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create SQ.</summary>
        public static List<Data.Domain.StagingCreateSQ> GetListStagingCreateSQ()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateSQ select a;
                return tb.OrderByDescending(o => o.ModifiedDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create ST.</summary>
        public static List<Data.Domain.StagingCreateST> GetListStagingCreateST()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateST select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Create WIP.</summary>
        public static List<Data.Domain.StagingCreateWIP> GetListStagingCreateWIP()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingCreateWIP select a;
                return tb.OrderByDescending(o => o.ModifiedDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Delete Doc RW.</summary>
        public static List<Data.Domain.StagingDeleteDocRW> GetListStagingDeleteDocRW()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingDeleteDocRW select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Delete MO.</summary>
        public static List<Data.Domain.StagingDeleteMO> GetListStagingDeleteMO()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingDeleteMO select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Received MO.</summary>
        public static List<Data.Domain.StagingReceivedMO> GetListStagingReceivedMO()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingReceivedMO select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Received ST.</summary>
        public static List<Data.Domain.StagingReceivedST> GetListStagingReceivedST()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingReceivedST select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Sales 500.</summary>
        public static List<Data.Domain.StagingSales500> GetListStagingSales500()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingSales500 select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }

        /// <summary>
        /// Get Data Staging Sales 800.</summary>
        public static List<Data.Domain.StagingSales800> GetListStagingSales800()
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from a in db.StagingSales800 select a;
                return tb.OrderByDescending(o => o.LastUpdateDate).ToList();
            }
        }
    }
}
