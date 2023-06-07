using App.Data;
using System;
using System.Linq;
namespace App.Service.SOVetting
{
    public class CustomerOrderSummary
    {
        public static Data.Domain.SOVetting.CustomerOrderSummary GetCustomerOrderSummary(string salesDocNo, Int32 salesDocItem)
        {
            EfDbContext efDbContext = new EfDbContext();
            Data.Domain.SOVetting.CustomerOrderSummary result = efDbContext.CustomerOrderSummary
                .Where(d => d.SalesDocNo2 == salesDocNo)
                .FirstOrDefault(t => t.SalesDocItem == salesDocItem);
            return result;
        }
    }
}