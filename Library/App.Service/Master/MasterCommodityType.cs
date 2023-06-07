using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterCommodityType
    {

        public static List<App.Data.Domain.MasterCommodityType> GetList()
        {
            List<App.Data.Domain.MasterCommodityType> result = new List<Data.Domain.MasterCommodityType>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterCommodityType>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
