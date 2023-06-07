using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{

    public class MasterVendor
    {
        public const string CacheName = "App.master.Vendor";

        public static List<Data.Domain.EMCS.Vendor> GetList(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var term = crit.Term ?? "";
                var tb = db.Vendors.Where(a => a.Name.Contains(term)).OrderBy(a => a.Name).Skip(0).Take(100).ToList();
                return tb;
            }
        }

        public static Data.Domain.EMCS.Vendor GetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.Vendors.Where(a => a.Id == id).OrderBy(a => a.Name);
                return tb.FirstOrDefault();
            }
        }
    }

}
