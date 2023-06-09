namespace App.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using App.Data.Domain;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using App.Data.Domain.SAP;
    using App.Data.Domain.SOVetting;
    using App.Data.Domain.VettingProcess;

    public partial class EfDbContext
    {
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<AgreementTypes> AgreementTypes { get; set; }
        public virtual DbSet<Hub> Hubs { get; set; }
        public virtual DbSet<RoleAccess> RoleAccesses { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<UserAccess> UserAccesses { get; set; }
        public virtual DbSet<UserAccess_Role> UserAccess_Role { get; set; }
        public virtual DbSet<UserAccess_Area> UserAccess_Area { get; set; }
        public virtual DbSet<UserAccess_Hub> UserAccess_Hub { get; set; }
        public virtual DbSet<UserAccess_Store> UserAccess_Store { get; set; }

        public virtual DbSet<Commodity> Commodities { get; set; }
        public virtual DbSet<SurveyLocation> SurveyLocations { get; set; }
        public virtual DbSet<CommodityImex> CommodityImex { get; set; }
        public virtual DbSet<HSCodeList> HSCodeLists { get; set; }
        public virtual DbSet<OrderMethod> OrderMethods { get; set; }
        public virtual DbSet<PartsList> PartsLists { get; set; }
        public virtual DbSet<Lartas> Lartas { get; set; }
        //public virtual DbSet<AirPort> AirPorts { get; set; }
        public virtual DbSet<EmailRecipient> EmailRecipient { get; set; }
        public virtual DbSet<Email> Email { get; set; }
        public virtual DbSet<EmailAttachment> EmailAttachment { get; set; }
        public virtual DbSet<FreightPort> FreightPort { get; set; }
        public virtual DbSet<ImportGate> ImportGates { get; set; }
        //public virtual DbSet<SeaPort> SeaPorts { get; set; }
        public virtual DbSet<LicenseGroup> LicenseGroups { get; set; }
        public virtual DbSet<LicensePorts> LicensePorts { get; set; }
        public virtual DbSet<ShippingInstruction> ShippingInstructions { get; set; }
        public virtual DbSet<OrderProfile> OrderProfiles { get; set; }
        public virtual DbSet<SosGroup> SosGroups { get; set; }
        public virtual DbSet<JobFlag> JobFlags { get; set; }
        public virtual DbSet<ToDoList> ToDoLists { get; set; }
        public virtual DbSet<CooDescription> CooDescriptions { get; set; }
        public virtual DbSet<V_PART_ORDER_REPORT> V_PART_ORDER_REPORTS { get; set; }
        public virtual DbSet<V_PART_ORDER_CASE_REPORT> V_PART_ORDER_CASE_REPORTS { get; set; }
        public virtual DbSet<V_PART_ORDER_DETAIL_REPORT> V_PART_ORDER_DETAIL_REPORTS { get; set; }
        //public virtual DbSet<spGetPartOrderDetailReport> spGetPartOrderDetailReport { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<EscalationLimit> Levels { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        public virtual DbSet<RoleAccessMenu> RoleAccessMenus { get; set; }

        public virtual DbSet<MasterTruckingRate> MasterTruckingRate { get; set; }

        public virtual DbSet<MasterTruckingRateLog> MasterTruckingRateLog { get; set; }

        public virtual DbSet<MasterRate> MasterRate { get; set; }
        public virtual DbSet<MasterRateLog> MasterRateLog { get; set; }

        public virtual DbSet<MasterInboundRate> MasterInboundRate { get; set; }
        public virtual DbSet<MasterInboundRateLog> MasterInboundRateLog { get; set; }

        public virtual DbSet<MasterSurcharge> MasterSurcharge { get; set; }
        public virtual DbSet<MasterSurchargeLog> MasterSurchargeLog { get; set; }
        public virtual DbSet<ModaOfCondition> ModaOfCondition { get; set; }
        public virtual DbSet<MasterGeneric> MasterGeneric { get; set; }

        public virtual DbSet<DocumentUpload> DocumentUpload { get; set; }

        public virtual DbSet<MaterialDescription> MaterialDescription { get; set; }
        public virtual DbSet<Station3LCKB> Station3LCKB { get; set; }
        public virtual DbSet<PartsNumberList> PartsNumberList { get; set; }
        public virtual DbSet<MasterInvoiceType> MasterInvoiceType { get; set; }
        public virtual DbSet<MasterShipmentTypePO> MasterShipmentTypePO { get; set; }
        public virtual DbSet<MasterShipmentTypeSO> MasterShipmentTypeSO { get; set; }
        public virtual DbSet<MasterModelType> MasterModelType { get; set; }
        public virtual DbSet<MasterOrderClass> MasterOrderClass { get; set; }
        public virtual DbSet<MasterOrderProfile> MasterOrderProfile { get; set; }
        public virtual DbSet<MasterAgreementTypePO> MasterAgreementTypePO { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<MasterCommodityType> MasterCommodityType { get; set; }
        public virtual DbSet<MasterCustomerGroup> MasterCustomerGroup { get; set; }
        public virtual DbSet<SapSoHeader> SapSoHeader { get; set; }
        public virtual DbSet<SapSoOrderItem> SapSoOrderItem { get; set; }
        public virtual DbSet<SapSoSourceItem> SapSoSourceItem { get; set; }
        public virtual DbSet<CKBDeliveryStatus> CKBDeliveryStatus { get; set; }
        public virtual DbSet<CKBDeliveryStatusTrack> CKBDeliveryStatusTrack { get; set; }
        public virtual DbSet<SapSoSearch> SapSoSearch { get; set; }
        public virtual DbSet<DpsSoSourceItem> DpsSoSourceItem { get; set; }
        public virtual DbSet<CustomerOrderSummary> CustomerOrderSummary { get; set; }
        public virtual DbSet<CustomerPOSummary> CustomerPOSummary { get; set; }
        public virtual DbSet<StagingPartsMapping> StagingPartsMapping { get; set; }
        public virtual DbSet<RequestForChange> RequestForChange { get; set; }
        public virtual DbSet<RFCItem> RFCItem { get; set; }
    }
}