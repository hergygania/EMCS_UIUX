// ReSharper disable once CheckNamespace
namespace App.Data
{
    using System.Data.Entity;
    using Domain.EMCS;

    public partial class EmcsContext
    {
        //DTS
        public virtual DbSet<DashboardNetWeight> DashboardNetWeight { get; set; }
        public virtual DbSet<DashboardExportToday> DashboardExportToday { get; set; }
        public virtual DbSet<DashboardExportToday2> DashboardExportToday2 { get; set; }
        public virtual DbSet<DashboardExportValue> DashboardExportValue { get; set; }
        public virtual DbSet<DashboardOutstanding> DashboardOutstanding { get; set; }
        public virtual DbSet<MasterCustomers> MasterCustomer { get; set; }
        public virtual DbSet<MasterBranch> MasterBranch { get; set; }
        public virtual DbSet<MasterBanner> MasterBanner { get; set; }
        public virtual DbSet<MasterCountry> MasterCountry { get; set; }
        public virtual DbSet<MasterArea> MasterArea { get; set; }
        public virtual DbSet<MasterSuperior> MasterSuperior { get; set; }
        public virtual DbSet<MasterIncoterms> MasterIncoTerm { get; set; }
        public virtual DbSet<MasterParameter> MasterParameter { get; set; }
        public virtual DbSet<MasterRunningText> MasterRunningText { get; set; }
        public virtual DbSet<MasterRegulation> MasterRegulation { get; set; }
        public virtual DbSet<MasterProblemCategory> MasterProblem { get; set; }
        public virtual DbSet<MasterKppbc> MasterKppbc { get; set; }
        public virtual DbSet<MasterPlant> MasterPlant { get; set; }
        public virtual DbSet<SpGetListAllKppbc> SpGetListAllKppbc { get; set; }
        public virtual DbSet<MasterKurs> MasterKurs { get; set; }
        public virtual DbSet<MasterDepartment> MasterDepartment { get; set; }
        public virtual DbSet<MasterAreaUserCkb> MasterAreaUserCkb { get; set; }
        public virtual DbSet<MasterSib> MasterSib { get; set; }
        public virtual DbSet<MasterVideo> MasterVideo { get; set; }
        public virtual DbSet<SpAreaUserCkb> SpAreaUserCkb { get; set; }
        public virtual DbSet<ProblemHistory> ProblemHistory { get; set; }
        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<ReferenceToCiplItem> ReferenceToCiplItem { get; set; }
        public virtual DbSet<Cipl> CiplData { get; set; }
        public virtual DbSet<CiplApprove> CiplApprove { get; set; }
        public virtual DbSet<CiplItem> CiplItemData { get; set; }
        public virtual DbSet<CiplItemInsert> CiplItemInsert { get; set; }
        public virtual DbSet<CiplItemListFilter> CiplItemListFilter { get; set; }
        public virtual DbSet<CiplForwader> CiplForwaders { get; set; }
        public virtual DbSet<CiplListFilter> CiplListFilter { get; set; }
        public virtual DbSet<SpCiplList> SpCiplList { get; set; }
        public virtual DbSet<SpCiplDeleteById> SpCiplDeleteById { get; set; }
        public virtual DbSet<SpGetCiplHistory> SpGetCiplHistory { get; set; }
<<<<<<< HEAD
        public virtual DbSet<SPGetCiplChangeHistory> SpGetCiplChangeHistory { get; set; }
=======
>>>>>>> 639d8d0 (Intial commit)
        public virtual DbSet<SPGetCiplDocument> SPGetCiplDocument { get; set; }
        public virtual DbSet<SpCiplProblemHistory> SpCiplProblemHistory { get; set; }
        public virtual DbSet<SpGetConsigneeName> SpGetConsigneeName { get; set; }
        public virtual DbSet<ReturnSpInsert> ReturnSpCiplInsert { get; set; }
        public virtual DbSet<Documents> Documents { get; set; }
        public virtual DbSet<DocumentList> DocumentList { get; set; }
        public virtual DbSet<IdData> IdData { get; set; }
        public virtual DbSet<MasterFlow> Flow { get; set; }
        public virtual DbSet<MasterFlowStep> FlowStep { get; set; }
        public virtual DbSet<MasterFlowStatus> FlowStatus { get; set; }
        public virtual DbSet<MasterFlowNext> FlowNext { get; set; }
        public virtual DbSet<SpFlowNext> SpFlowNext { get; set; }
        public virtual DbSet<SpFlowStatus> SpFlowStatus { get; set; }
        public virtual DbSet<SelectItem> SelectItem { get; set; }
        public virtual DbSet<SelectItem3> SelectItem3 { get; set; }
        public virtual DbSet<GoodsReceive> GoodsReceive { get; set; }
        public virtual DbSet<GoodsReceiveItem> GoodsReceiveItem { get; set; }
        public virtual DbSet<Cargo> CargoData { get; set; }
        public virtual DbSet<CargoItem> CargoItemData { get; set; }
        public virtual DbSet<CargoAddCipl> CargoAddCipl { get; set; }
        public virtual DbSet<SpCargoProblemHistory> SpCargoProblemHistory { get; set; }
        public virtual DbSet<SpGetCargoHistory> SpGetCargoHistory { get; set; }
        public virtual DbSet<SpGetReference> GetReference { get; set; }
        public virtual DbSet<SpGetGrList> GetGrList { get; set; }
        public virtual DbSet<ShippingInstructions> ShippingInstruction { get; set; }
        public virtual DbSet<SpCargoList> GetCargoList { get; set; }
<<<<<<< HEAD
        public virtual DbSet<SPShippingInstruction> GetShippingInstructionList { get; set; }
        public virtual DbSet<SPShippingSummary> GetShippingSummary { get; set; }
        public virtual DbSet<SPNpePeb> GetNpePebList { get; set; }
        public virtual DbSet<SPBlAwb> GetBLAWBList { get; set; }
        public virtual DbSet<SpCargoDetail> GetCargoDetail { get; set; }
        public virtual DbSet<NpePeb> NpePebs { get; set; }
        public virtual DbSet<CargoContainer> CargoContainers { get; set; }
        public virtual DbSet<CargoDocument> CargoDocument { get; set; }
        public virtual DbSet<GoodReceiveDocument> GoodsReceiveDocument { get; set; }
=======
        public virtual DbSet<SpCargoDetail> GetCargoDetail { get; set; }
        public virtual DbSet<NpePeb> NpePebs { get; set; }
        public virtual DbSet<CargoContainer> CargoContainers { get; set; }
>>>>>>> 639d8d0 (Intial commit)
        public virtual DbSet<TaskSi> TaskSi { get; set; }
        public virtual DbSet<TaskNpePeb> TaskNpePeb { get; set; }
        public virtual DbSet<TaskBlAwb> TaskBlAwb { get; set; }
        public virtual DbSet<BlAwb> BlAwb { get; set; }
        public virtual DbSet<SpGetCiplAvailable> CiplAvailables { get; set; }
        public virtual DbSet<CargoCipl> CargoCipls { get; set; }
        public virtual DbSet<CargoHistory> CargoHistory { get; set; }
        public virtual DbSet<SpGetProblemList> SpGetProblemLists { get; set; }
        public virtual DbSet<SpGetDocumentList> SpGetDocumentLists { get; set; }
        public virtual DbSet<RequestCl> RequestCl { get; set; }
        public virtual DbSet<RequestCipl> RequestCipl { get; set; }
        public virtual DbSet<PlantBusiness> PlantBusinesses { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<SpGoodReceive> SpGoodReceives { get; set; }
        public virtual DbSet<Regulation> Regulation { get; set; }
        public virtual DbSet<SpRTotalExportMonthly> SpRTotalExportMonthly { get; set; }
        public virtual DbSet<SpRTotalExportPort> SpRTotalExportPort { get; set; }
        public virtual DbSet<RequestGr> RequestGrs { get; set; }
        public virtual DbSet<CiplItemUpdateHistory> CiplItemUpdateHistories { get; set; }
        public virtual DbSet<NotificationQueue> NotificationQueue { get; set; }
        public virtual DbSet<UserLog> UserLog { get; set; }
        public virtual DbSet<MasterAirSeaPort> MasterAirSeaPorts { get; set; }
        public virtual DbSet<MasterBranchCkb> MasterBranchCkbs { get; set; }
        public virtual DbSet<SpGetCiplTotalData> SpGetCiplTotalDatas { get; set; }
<<<<<<< HEAD

        public virtual DbSet<Type> Type { get; set; }
=======
>>>>>>> 639d8d0 (Intial commit)
    }
}
