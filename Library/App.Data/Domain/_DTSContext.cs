using System.Data.Entity;

namespace App.Data
{
    using App.Data.Domain;

    public partial class DTSContext : DbContext
    {
        public DTSContext()
            : base("name=dtsConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //}
    }
}
