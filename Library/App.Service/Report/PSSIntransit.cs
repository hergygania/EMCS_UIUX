using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class PSSIntransits
    {
        public static List<RptPSSIntransit> GetList(string groupType, string filterBy, string storeNo, DateTime? startDate, DateTime? endDate, string[] pssCustID)
        {
            using (var db = new ReportDbContext())
            {
               
                IEnumerable<RptPSSIntransit> tbl = db.RptPSSIntransits.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.pss_CreatedOn.Date >= startDate.Value.Date && w.pss_CreatedOn.Date <= endDate.Value.Date);

                }

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.pss_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.pss_AreaID == filter) : tbl.Where(w => w.pss_HubID == filter);
                    }
                }
                //if (pssCustID != null)
                //    tbl = tbl.Where(w => pssCustID.Contains(w.));
                return tbl.ToList();
            }
        }

        //public static List<RptPSSIntransit> GetListCustomers()
        //{
        //    using (var db = new ReportDbContext())
        //    {
        //        var tbl = db.RptPSSIntransits.GroupBy(d => d.pss_CaseNo).Select(d => d.FirstOrDefault());
        //        return tbl.ToList();
        //    }
        //}

    }
}
