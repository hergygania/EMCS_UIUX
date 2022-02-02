using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
namespace App.Service.SOVetting
{
    public class CustomerPOSummary
    {
        public static Data.Domain.SOVetting.CustomerPOSummary GetCustomerPOSummary(string salesDocNo, Int32 salesDocItem, string shipmentNo) { 
            EfDbContext efDbContext = new EfDbContext();
            Data.Domain.SOVetting.CustomerPOSummary result = efDbContext.CustomerPOSummary
                .Where(d => d.SalesDocNo == salesDocNo)
                .Where(d => d.SalesDocItem == salesDocItem)
                .FirstOrDefault(t => t.ShipmentNo == shipmentNo);
            return result;
        }
        public static Data.Domain.SOVetting.CustomerPOSummary GetCustomerPOSummary(string salesDocNo, Int32 salesDocItem)
        {
            EfDbContext efDbContext = new EfDbContext();
            Data.Domain.SOVetting.CustomerPOSummary result = efDbContext.CustomerPOSummary
                .Where(d => d.SalesDocNo == salesDocNo)
                .FirstOrDefault(t => t.SalesDocItem == salesDocItem);
            return result;
        }
        public static List<Data.Domain.SOVetting.CustomerPOSummary> GetListDaCustomerPOSummary(string salesDocNo)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CustomerPOSummary> result = efDbContext.CustomerPOSummary
                .Where(d => d.SalesDocNo == salesDocNo)
                .ToList();
            return result;
        }
    }
}
