using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class BORespondTimes
    {
        public static List<RptBORespondTime> GetList(
            string partNo,
            int? quantity, 
            string binLoc,
            int? weight,
            int? length,
            int? width,
            DateTime? pickupStartDate,
            DateTime? pickupEndDate)
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptBORespondTime> tbl = db.RptBORespondTimes.ToList();

                if (string.IsNullOrEmpty(partNo))
                    tbl = tbl.Where(w => w.borsp_PartNo.Contains(partNo));
                if (quantity.HasValue)
                    tbl = tbl.Where(w => w.borsp_Qty == quantity);
                if (string.IsNullOrEmpty(binLoc))
                    tbl = tbl.Where(w => w.borsp_Binloc == binLoc);
                if (weight.HasValue)
                    tbl = tbl.Where(w => w.borsp_Weight == weight);
                if (length.HasValue)
                    tbl = tbl.Where(w => w.borsp_Length == length);
                if (width.HasValue)
                    tbl = tbl.Where(w => w.borsp_Width == width);
                if (pickupStartDate.HasValue && pickupEndDate.HasValue)
                {
                    tbl = tbl.Where(w => w.borsp_PickupDate >= pickupStartDate.Value && w.borsp_PickupDate <= pickupEndDate.Value);

                }
                return tbl.ToList();
            }
        }

    }
}
