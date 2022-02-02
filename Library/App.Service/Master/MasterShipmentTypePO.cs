using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterShipmentTypePO
    {
        public static List<App.Data.Domain.MasterShipmentTypePO> GetList()
        {
            List<App.Data.Domain.MasterShipmentTypePO> result = new List<Data.Domain.MasterShipmentTypePO>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterShipmentTypePO>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
