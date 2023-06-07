using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class CounterPerformances
    {
        public static List<RptCounterPerformance> GetList(string groupType, string filterBy, string storeNo, DateTime? startDate, DateTime? endDate, string[] ctprfCustID)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptCounterPerformance> tbl = db.RptCounterPerformances.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.ctprf_DocDate.Value.Date >= startDate.Value.Date && w.ctprf_DocDate.Value.Date <= endDate.Value.Date);

                }

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.ctprf_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.ctprf_AreaID == filter) : tbl.Where(w => w.ctprf_HubID == filter);
                    }
                }
                if (ctprfCustID != null)
                    tbl = tbl.Where(w => ctprfCustID.Contains(w.ctprf_DBSUserID));
                return tbl.ToList();
            }
        }

        public static List<RptCounterPerformance> GetListUser()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptCounterPerformances.GroupBy(d => d.ctprf_DBSUserID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
