namespace WindowService.Library.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class postContext : DbContext
    {
        public postContext()
            : base("name=postConnection")
        {
        }

        public virtual DbSet<StParameter> StParameter { get; set; }
        public virtual DbSet<MtVendor> MtVendor { get; set; }
        public virtual DbSet<TrRequest> TrRequest { get; set; }
        public virtual DbSet<TrRequestAttachment> TrRequestAttachment { get; set; }
        public virtual DbSet<TrPO> TrPO { get; set; }
        public virtual DbSet<MappingUploadItem> MappingUploadItem { get; set; }
        public virtual DbSet<TrRequestComment> TrRequestComment { get; set; }
        public virtual DbSet<TrRequestCommentRead> TrRequestCommentRead { get; set; }

    }
}
