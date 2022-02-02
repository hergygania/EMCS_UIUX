using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.SAP
{
    public class Customer
    {
        public static List<Data.Domain.SAP.Customer>
            GetSelectCustomer(string searchName, string customer_group)
        {
            var list = new List<Data.Domain.SAP.Customer>();
            //int offset = pageSize * (page - 1);
            try
            {
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.Customer.AsNoTracking().ToList()
                             join a in db.MasterCustomerGroup.AsNoTracking().ToList() on c.CustomerNo equals a.CustomerID
                             where (c.CustomerNo.ToLower().Contains(searchName) || c.NAME1.ToLower().Contains(searchName))
                                    && a.CustomerGroup.Contains(customer_group)
                             select new Data.Domain.SAP.Customer()
                             {
                                 CustomerNo = c.CustomerNo,
                                 NAME1 = c.CustomerNo + " - " + c.NAME1
                             };

                    return tb
                        //.Skip(offset)
                        //.Take(pageSize)
                        .OrderBy(o => o.CustomerNo)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return list;

        }
    }
}
