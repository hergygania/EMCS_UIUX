using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class SubmittedItems
    {
        public static List<RptSubmittedItem> GetList(string groupType, string filterBy, string storeNo, DateTime? startDate, DateTime? endDate)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptSubmittedItem> tbl = db.RptSubmittedItems.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.sbmitm_EntryDate.Value.Date >= startDate.Value.Date && w.sbmitm_EntryDate.Value.Date <= endDate.Value.Date);

                }

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.sbmitm_StoreNo.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.sbmitm_AreaID == filter) : tbl.Where(w => w.sbmitm_HubID == filter);
                    }
                }
                return tbl.ToList();
            }
        }

        

    }
}
