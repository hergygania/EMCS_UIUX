using System.Data.Entity;

namespace App.Data
{
    using App.Data.Domain;

    public partial class EfDbContext : DbContext
    {
        public EfDbContext()
            : base("name=pisConnection")
        {
					this.Configuration.LazyLoadingEnabled=true;
        }

        //public virtual DbSet<Area> Areas
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<Hub> Hubs
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<RoleAccess> RoleAccesses
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<Store> Stores
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<UserAccess> UserAccesses
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<UserAccess_Role> UserAccess_Role
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<UserAccess_Area> UserAccess_Area
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<UserAccess_Hub> UserAccess_Hub
        //{
        //	get;
        //	set;
        //}
        //public virtual DbSet<UserAccess_Store> UserAccess_Store
        //{
        //	get;
        //	set;
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                    .Property(e => e.Name)
                    .IsUnicode(false);

            modelBuilder.Entity<Area>()
                    .Property(e => e.Description)
                    .IsUnicode(false);

            modelBuilder.Entity<Area>()
                    .Property(e => e.EntryBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Area>()
                    .Property(e => e.ModifiedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Hub>()
                    .Property(e => e.Name)
                    .IsUnicode(false);

            modelBuilder.Entity<Hub>()
                    .Property(e => e.Description)
                    .IsUnicode(false);

            modelBuilder.Entity<Hub>()
                    .Property(e => e.EntryBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Hub>()
                    .Property(e => e.ModifiedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<RoleAccess>()
                    .Property(e => e.RoleName)
                    .IsUnicode(false);

            modelBuilder.Entity<RoleAccess>()
                    .Property(e => e.Description)
                    .IsUnicode(false);

            modelBuilder.Entity<RoleAccess>()
                    .Property(e => e.EntryBy)
                    .IsUnicode(false);

            modelBuilder.Entity<RoleAccess>()
                    .Property(e => e.ModifiedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Store>()
                    .Property(e => e.StoreNo)
                    .IsFixedLength()
                    .IsUnicode(false);

            modelBuilder.Entity<Store>()
                    .Property(e => e.Name)
                    .IsUnicode(false);

            modelBuilder.Entity<Store>()
                    .Property(e => e.Description)
                    .IsUnicode(false);

            modelBuilder.Entity<Store>()
                    .Property(e => e.EntryBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Store>()
                    .Property(e => e.ModifiedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.UserID)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.FullName)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.Phone)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.Email)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.EntryBy)
                    .IsUnicode(false);

            modelBuilder.Entity<UserAccess>()
                    .Property(e => e.ModifiedBy)
                    .IsUnicode(false);

            modelBuilder.Entity<Commodity>()
                    .Property(e => e.CommodityNo)
                    .IsFixedLength()
                    .IsUnicode(false);

            modelBuilder.Entity<Commodity>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Commodity>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Commodity>()
                .Property(e => e.EntryBy)
                .IsUnicode(false);

            modelBuilder.Entity<Commodity>()
                .Property(e => e.ModifiedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Region>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<Region>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<Region>().Property(e => e.EntryBy).IsUnicode(false);
            modelBuilder.Entity<Region>().Property(e => e.ModifiedBy).IsUnicode(false);

            modelBuilder.Entity<Group>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<Group>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<Group>().Property(e => e.EntryBy).IsUnicode(false);
            modelBuilder.Entity<Group>().Property(e => e.ModifiedBy).IsUnicode(false);

            modelBuilder.Entity<EscalationLimit>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<EscalationLimit>().Property(e => e.EntryBy).IsUnicode(false);
            modelBuilder.Entity<EscalationLimit>().Property(e => e.ModifiedBy).IsUnicode(false);

            modelBuilder.Entity<MasterGeneric>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<MasterGeneric>().Property(e => e.EntryBy).IsUnicode(false);
            modelBuilder.Entity<MasterGeneric>().Property(e => e.ModifiedBy).IsUnicode(false);

            modelBuilder.Entity<Menu>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<Menu>().Property(e => e.URL).IsUnicode(false);
            modelBuilder.Entity<Menu>().Property(e => e.EntryBy).IsUnicode(false);
            modelBuilder.Entity<Menu>().Property(e => e.ModifiedBy).IsUnicode(false);

            modelBuilder.Entity<RoleAccessMenu>().Property(e => e.EntryBy).IsUnicode(false);

            modelBuilder.Entity<MasterInboundRate>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterInboundRate>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterInboundRateLog>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterInboundRateLog>().Property(e => e.Destination_Code).IsUnicode(false);

            modelBuilder.Entity<DocumentUpload>().Property(e => e.ModulName).IsUnicode(false);
            modelBuilder.Entity<DocumentUpload>().Property(e => e.FileName).IsUnicode(false);

            modelBuilder.Entity<MasterSurcharge>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterSurcharge>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterSurchargeLog>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterSurchargeLog>().Property(e => e.Destination_Code).IsUnicode(false);

            modelBuilder.Entity<LogImport>().Property(e => e.FileName).IsUnicode(false);

            modelBuilder.Entity<MasterTruckingRate>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterTruckingRate>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterTruckingRate>().Property(e => e.Remarks).IsUnicode(false);

            modelBuilder.Entity<MasterTruckingRateLog>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterTruckingRateLog>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterTruckingRateLog>().Property(e => e.Remarks).IsUnicode(false);

            modelBuilder.Entity<MasterGeneric>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterGeneric>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<MasterGeneric>().Property(e => e.Value).IsUnicode(false);
            modelBuilder.Entity<MasterGeneric>().Property(e => e.Description).IsUnicode(false);
            modelBuilder.Entity<ModaOfCondition>().Property(e => e.Moda).IsUnicode(false);
            modelBuilder.Entity<ModaOfCondition>().Property(e => e.Description).IsUnicode(false);
            modelBuilder.Entity<MasterRate>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterRate>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterRate>().Property(e => e.Service_Code).IsUnicode(false);

            modelBuilder.Entity<MasterRateLog>().Property(e => e.Origin_Code).IsUnicode(false);
            modelBuilder.Entity<MasterRateLog>().Property(e => e.Destination_Code).IsUnicode(false);
            modelBuilder.Entity<MasterRateLog>().Property(e => e.Service_Code).IsUnicode(false);

            modelBuilder.Entity<DeliveryTrackingStatus>().Property(e => e.NODA).IsUnicode(false);
            modelBuilder.Entity<DeliveryTrackingStatus>().Property(e => e.NODI).IsUnicode(false);

            modelBuilder.Entity<Master_Moda>().Property(e => e.ModaDescription).IsUnicode(false);

            modelBuilder.Entity<Master_Status>().Property(e => e.Status).IsUnicode(false);
            modelBuilder.Entity<Master_UnitType>().Property(e => e.UnitTypeDescription).IsUnicode(false);
            modelBuilder.Entity<City>().Property(e => e.StoreName).IsUnicode(false);
            modelBuilder.Entity<City>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<DeliveryTracking>().Property(e => e.OutBoundDelivery).IsUnicode(false);
            modelBuilder.Entity<DeliveryTracking>().Property(e => e.SalesOrderNumber).IsUnicode(false);
            modelBuilder.Entity<DeliveryTracking>().Property(e => e.Model).IsUnicode(false);          
            modelBuilder.Entity<SurveyLocation>().Property(e => e.Name).IsUnicode(false);
            modelBuilder.Entity<MasterInvoiceType>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterShipmentTypePO>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterShipmentTypeSO>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterModelType>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterOrderClass>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterOrderProfile>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterAgreementTypePO>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterCommodityType>().Property(e => e.Code).IsUnicode(false);
            modelBuilder.Entity<MasterCustomerGroup>().Property(e => e.CustomerID).IsUnicode(false);
            //modelBuilder.Entity<SurveyLocation>().ToTable("imex.SurveyLocation");

        }
    }
}
