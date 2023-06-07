using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class STRespondTimes
    {
        public static List<RptSTRespondTime> GetList(string groupType, string filterBy, string storeNo,
            DateTime? startDate, DateTime? endDate)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptSTRespondTime> tbl = db.RptSTRespondTimes.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl =
                        tbl.Where(
                            w =>
                                w.strsp_CreatedOn >= startDate.Value.Date &&
                                w.strsp_CreatedOn.Date <= endDate.Value.Date);

                }

                if (storeNo != "ALL" && !String.IsNullOrEmpty(storeNo))
                {
                    tbl = tbl.Where(w => w.strsp_StoreName.Contains(storeNo));
                }
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA"
                            ? tbl.Where(w => w.strsp_AreaID == filter)
                            : tbl.Where(w => w.strsp_HubID == filter);
                    }
                }
                return tbl.ToList();
            }
        }
    }
}
