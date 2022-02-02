using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterOrderClass
    {
        public static List<App.Data.Domain.MasterOrderClass> GetList()
        {
            List<App.Data.Domain.MasterOrderClass> result = new List<Data.Domain.MasterOrderClass>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterOrderClass>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
