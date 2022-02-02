namespace App.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using App.Data.Domain;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class EfDbContext
    {
        public virtual DbSet<InventoryList> InventoryList { get; set; }
        public virtual DbSet<InventoryAllocation> InventoryAllocation { get; set; }
        public virtual DbSet<InventoryTrackingStatus> InventoryTrackingStatus { get; set; }
        public virtual DbSet<InventoryTrackingDelivery> InventoryTrackingDelivery { get; set; }
        public virtual DbSet<InventoryTrackingStatusCMS> InventoryTrackingStatusCMS { get; set; }
        public virtual DbSet<OWSS> OWSS { get; set; }
        public virtual DbSet<BO> BO { get; set; }
        public virtual DbSet<PartInfoDetail> PartInfoDetail { get; set; }

        //Master
        public virtual DbSet<MasterSOS> MasterSOS { get; set; }
        public virtual DbSet<MasterJobCode> MasterJobCode { get; set; }
        public virtual DbSet<MasterJobLocation> MasterJobLocation { get; set; }
        public virtual DbSet<MasterSection> MasterSection { get; set; }
        public virtual DbSet<MasterCustomer> MasterCustomer { get; set; }
        public virtual DbSet<MasterCustomerDetail> MasterCustomerDetail { get; set; }

        //Report
        public virtual DbSet<RptPiecePartOrderDetailList> RptPiecePartOrderDetailList { get; set; }
        public virtual DbSet<RptOutstandingOldCoreDetailList> RptOutstandingOldCoreDetailList { get; set; }
        public virtual DbSet<ForecastAccuracyDetailList> ForecastAccuracyDetailList { get; set; }

        //Staging
        public virtual DbSet<Staging4Bn48R> Staging4Bn48R { get; set; }
        public virtual DbSet<StagingCORE> StagingCORE { get; set; }
        public virtual DbSet<StagingInventoryAdjustment> StagingIA { get; set; }
        public virtual DbSet<StagingCreateBER> StagingCreateBER { get; set; }
        public virtual DbSet<StagingCreateJC> StagingCreateJC { get; set; }
        public virtual DbSet<StagingCreateMO> StagingCreateMO { get; set; }
        public virtual DbSet<StagingCreateSQ> StagingCreateSQ { get; set; }
        public virtual DbSet<StagingCreateST> StagingCreateST { get; set; }
        public virtual DbSet<StagingCreateWIP> StagingCreateWIP { get; set; }
        public virtual DbSet<StagingDeleteDocRW> StagingDeleteDocRW { get; set; }
        public virtual DbSet<StagingDeleteMO> StagingDeleteMO { get; set; }
        public virtual DbSet<StagingReceivedMO> StagingReceivedMO { get; set; }
        public virtual DbSet<StagingReceivedST> StagingReceivedST { get; set; }
        public virtual DbSet<StagingSales500> StagingSales500 { get; set; }
        public virtual DbSet<StagingSales800> StagingSales800 { get; set; }
        
    }
}
