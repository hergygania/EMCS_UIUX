

namespace App.Data
{
    using System.Data.Entity;
    using Domain.POST;
    public partial class POSTContext
     {

        public virtual DbSet<StParameter> StParameter { get; set; }
        public virtual DbSet<MtVendor> MtVendor { get; set; }
        public virtual DbSet<TrRequest> TrRequest { get; set; }
        public virtual DbSet<TrRequestAttachment> TrRequestAttachment { get; set; }
        public virtual DbSet<TrItem> TrItem { get; set; }
        public virtual DbSet<TrPO> TrPO { get; set; }        
        public virtual DbSet<MappingUploadItem> MappingUploadItem { get; set; }
        public virtual DbSet<TrRequestComment> TrRequestComment { get; set; }
        public virtual DbSet<TrRequestCommentRead> TrRequestCommentRead { get; set; }
        public virtual DbSet<BupotSpt23> BupotSpt23 { get; set; }
        public virtual DbSet<BupotOrganizationDetail> BupotOrganizationDetail { get; set; }
        public virtual DbSet<BupotSpt23Detail> BupotSpt23Detail { get; set; }
        public virtual DbSet<BupotSpt23Ref> BupotSpt23Ref { get; set; }
        public virtual DbSet<TrAttachment> TrAttachment { get; set; }
        public virtual DbSet<MtMappingUserBranch> MtMappingUserBranch { get; set; }
        public virtual DbSet<TrItemPartialQty> TrItemPartialQty { get; set; }
    }
}
