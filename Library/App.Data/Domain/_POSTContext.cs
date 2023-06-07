using System.Data.Entity;

namespace App.Data
{
    using App.Data.Domain;

    public partial class POSTContext : DbContext
    {
        public POSTContext()
            : base("name=postConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
    }
}
