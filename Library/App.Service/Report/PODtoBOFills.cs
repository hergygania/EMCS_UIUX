using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class PODtoBOFills
    {
        public static List<RptPODtoBOFill> GetList(
            string docNo,
            string partNo,
            string sos,
            DateTime? startReceiptDate,
            DateTime? endReceiptDate,
            int? quantity
           )
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptPODtoBOFill> tbl = db.RptPODtoBOFills.ToList();

                if (startReceiptDate.HasValue && endReceiptDate.HasValue)
                    tbl = tbl.Where(w => w.podbo_ReceiptDateTime >= startReceiptDate.Value && w.podbo_ReceiptDateTime <= endReceiptDate.Value);
                if (!string.IsNullOrEmpty(partNo))
                    tbl = tbl.Where(w => w.podbo_PartNo.Contains(partNo));
                if (!string.IsNullOrEmpty(docNo))
                    tbl = tbl.Where(w => w.podbo_DocNo.Contains(docNo));
                if (!string.IsNullOrEmpty(sos))
                    tbl = tbl.Where(w => w.podbo_SOS.Contains(sos));
                if (quantity.HasValue)
                    tbl = tbl.Where(w => w.podbo_Qty == quantity.Value);
                return tbl.ToList();
            }
        }

    }
}
