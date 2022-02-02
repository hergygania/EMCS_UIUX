using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterOrderProfile
    {
        public static List<App.Data.Domain.MasterOrderProfile> GetList()
        {
            List<App.Data.Domain.MasterOrderProfile> result = new List<Data.Domain.MasterOrderProfile>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterOrderProfile>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
