using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class OrderProfile
    {
        public static List<Data.Domain.OrderProfile> GetList(string orderClass)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (orderClass == "W")
                    orderClass = "E";
                var tb = db.CreateRepository<Data.Domain.OrderProfile>().TableNoTracking.Select(e => e).Where(o=>o.OrderClass == orderClass);
                return tb.ToList();
            }

        }
    }
}
