using System.Data.Entity;

namespace App.Data
{
    using App.Data.Domain;

    public partial class EmcsContext : DbContext
    {
        public EmcsContext()
            : base("name=emcsConnection")
        {
					this.Configuration.LazyLoadingEnabled=true;
        }
    }
}
