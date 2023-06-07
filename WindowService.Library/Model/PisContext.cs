namespace WindowService.Library.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class PisContext : DbContext
    {
        public PisContext()
            : base("name=pisConnection")
        {
        }

        public virtual DbSet<PartsOrder> PartsOrders { get; set; }
        public virtual DbSet<PartsOrderCase> PartsOrderCases { get; set; }
        public virtual DbSet<PartsOrderCaseTMP> PartsOrderCaseTMPs { get; set; }
        public virtual DbSet<PartsOrderDetail> PartsOrderDetails { get; set; }
        public virtual DbSet<PartsOrderDetailTMP> PartsOrderDetailTMPs { get; set; }
        public virtual DbSet<PartsOrderTMP> PartsOrderTMPs { get; set; }
        public virtual DbSet<ShippingInstruction> ShippingInstructions { get; set; }
        public virtual DbSet<BOMITTMP> BOMITTMPs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
