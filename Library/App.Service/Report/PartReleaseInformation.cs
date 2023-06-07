using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class PartReleaseInformations
    {
        public static List<RptPartReleaseInformation> GetList(string groupType, string storeNo, DateTime? startDate, DateTime? endDate, string[] rackCustID)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptPartReleaseInformation> tbl = db.RptPartReleaseInformations.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.rpartrel_DocDate.Value.Date >= startDate.Value.Date && w.rpartrel_DocDate.Value.Date <= endDate.Value.Date);

                }
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rpartrel_Store.Trim() == storeNo.Trim());
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.rpartrel_CustID));
                return tbl.ToList();

            }
        }
  

    }
}
