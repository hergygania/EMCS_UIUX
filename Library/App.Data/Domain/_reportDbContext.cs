using System.Data.Entity;

namespace App.Data.Domain
{
    public partial class ReportDbContext : DbContext
    {
        public ReportDbContext()
            : base("name=pisReportConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public virtual DbSet<RptBackorderSubmission> RptBackorderSubmissions { get; set; }
        public virtual DbSet<RptBORespondTime> RptBORespondTimes { get; set; }
        public virtual DbSet<RptCounterPerformance> RptCounterPerformances { get; set; }
        public virtual DbSet<RptDocumentAmend> RptDocumentAmends { get; set; }
        public virtual DbSet<RptDocumentAwaitingAck> RptDocumentAwaitingAcks { get; set; }
        public virtual DbSet<RptDocumentConsolidateInvoice> RptDocumentConsolidateInvoices { get; set; }
        public virtual DbSet<RptDocumentHeld> RptDocumentHelds { get; set; }
        public virtual DbSet<RptDocumentNonConsolidateInvoice> RptDocumentNonConsolidateInvoices { get; set; }
        public virtual DbSet<RptDocumentPendingRelease> RptDocumentPendingReleases { get; set; }
        public virtual DbSet<RptDocumentReprint> RptDocumentReprints { get; set; }
        public virtual DbSet<RptDocumentSuspend> RptDocumentSuspends { get; set; }
        public virtual DbSet<RptEmergencyOrder> RptEmergencyOrders { get; set; }
        public virtual DbSet<RptOrderConfirmation> RptOrderConfirmations { get; set; }
        public virtual DbSet<RptOutstandingBackorderItem> RptOutstandingBackorderItems { get; set; }
        public virtual DbSet<RptPartReleaseInformation> RptPartReleaseInformations { get; set; }
        public virtual DbSet<RptPODtoBOFill> RptPODtoBOFills { get; set; }
        public virtual DbSet<RptPSSIntransit> RptPSSIntransits { get; set; }
        public virtual DbSet<RptSTRespondTime> RptSTRespondTimes { get; set; }
        public virtual DbSet<RptUnrealisticCommittedDateBackorderItem> RptUnrealisticCommittedDateBackorderItems { get; set; }
        public virtual DbSet<RptUnrealisticCommittedDateExstock> RptUnrealisticCommittedDateExstocks { get; set; }
        public virtual DbSet<RptWHDocumentReprint> RptWHDocumentReprints { get; set; }
        public virtual DbSet<RptAckMessage> RptAckMessages { get; set; }
        public virtual DbSet<RptLongIntransit> RptLongIntransits { get; set; }
        public virtual DbSet<RptLongPOD> RptLongPODs { get; set; }
        public virtual DbSet<RptSubmittedItem> RptSubmittedItems { get; set; }
        public virtual DbSet<RptVendorConstraint> RptVendorConstraints { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
