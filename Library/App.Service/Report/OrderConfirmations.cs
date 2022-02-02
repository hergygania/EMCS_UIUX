using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class OrderConfirmations
    {
        public static List<RptOrderConfirmation> GetList(string groupType, string storeNo, DateTime? startDate, DateTime? endDate, string[] rackCustID)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptOrderConfirmation> tbl = db.RptOrderConfirmations.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.ordcnf_DocDate.Value.Date >= startDate.Value.Date && w.ordcnf_DocDate.Value.Date <= endDate.Value.Date);

                }
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.ToString() == storeNo);
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.ordcnf_CustID));
                return tbl.ToList();

            }
        }
        public static IEnumerable<RptOrderConfirmation> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptOrderConfirmations.GroupBy(d => d.ordcnf_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
       
    }
}
