using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterModelType
    {
        public static List<App.Data.Domain.MasterModelType> GetList()
        {
            List<App.Data.Domain.MasterModelType> result = new List<Data.Domain.MasterModelType>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterModelType>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
