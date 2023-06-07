using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class SosGroup
    {
        public static List<Data.Domain.SosGroup> GetList(string groupName)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.SosGroup>().TableNoTracking.Select(e => e).Where(o => o.SOSGroup == groupName);
                return tb.ToList();
            }

        }

        public static Data.Domain.SosGroup GetOne(string groupName)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.SosGroup>().TableNoTracking.Select(e => e).Where(o => o.SOSGroup == groupName);
                return tb.FirstOrDefault();
            }

        }
    }
}
