namespace App.Data
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;
	using App.Data.Domain;

	public partial class EfDbContext
	{
		public virtual DbSet<CommodityMapping> CommodityMapping { get; set; }
		public virtual DbSet<PartsMapping> PartsMappings { get; set; }
		public virtual DbSet<PartsMappingHistory> PartsMappingHistories { get; set; }
        public virtual DbSet<RegulationManagement> RegulationManagements { get; set; }
        public virtual DbSet<ViewRegulationManagementHeader> ViewRegulationManagementsHeader { get; set; }
		public virtual DbSet<RegulationManagementDetail> RegulationManagementDetails { get; set; }
        public virtual DbSet<LicenseManagement> LicenseManagements { get; set; }
        public virtual DbSet<LicenseManagementHS> LicenseManagementsHS { get; set; }
        public virtual DbSet<LicenseManagementPartNumber> LicenseManagementsPartNumber { get; set; }
		public virtual DbSet<LicenseManagementExtend> LicenseManagementExtends { get; set; }
		public virtual DbSet<LicenseManagementExtendComment> LicenseManagementExtendComments { get; set; }
		public virtual DbSet<LicenseManagementHistory> LicenseManagementHistories { get; set; }
		public virtual DbSet<PartsOrder> PartsOrders { get; set; }
		public virtual DbSet<PartsOrderCase> PartsOrderCases { get; set; }
        public virtual DbSet<PartsOrderDetail> PartsOrderDetails { get; set; }
        public virtual DbSet<ManualVetting> ManualVettings { get; set; }
		public virtual DbSet<DocumentType> DocumentTypes { get; set; }
		public virtual DbSet<Shipment> Shipments { get; set; }
		public virtual DbSet<ShipmentDocument> ShipmentDocuments { get; set; }
		public virtual DbSet<ShipmentManifest> ShipmentManifests { get; set; }
		public virtual DbSet<ShipmentManifestDetail> ShipmentManifestDetails { get; set; }
		public virtual DbSet<Survey> Surveys { get; set; }
		public virtual DbSet<SurveyDetail> SurveyDetails { get; set; }
		public virtual DbSet<SurveyDocument> SurveyDocuments { get; set; }
		public virtual DbSet<TeamProfile> TeamProfiles { get; set; }

	}
}
