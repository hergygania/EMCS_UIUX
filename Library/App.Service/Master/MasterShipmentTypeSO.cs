using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterShipmentTypeSO
    {

        public static List<App.Data.Domain.MasterShipmentTypeSO> GetList()
        {
            List<App.Data.Domain.MasterShipmentTypeSO> result = new List<Data.Domain.MasterShipmentTypeSO>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterShipmentTypeSO>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
