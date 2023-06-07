using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;

namespace App.Service.Report
{
    public class WHDocumentReprints
    {
        public static List<RptWHDocumentReprint> GetList(
            string whdocrepDocNo, 
            string whdocrepSos,
            string whdocrepPartNo,
            string whdocrepBinLoc,
            DateTime? startDate, DateTime? endDate

            )
        {
            using (var db = new ReportDbContext())
            {
                IEnumerable<RptWHDocumentReprint> tbl = db.RptWHDocumentReprints.ToList();
                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.whdocrep_ReprintDate >= startDate.Value && w.whdocrep_ReprintDate <= endDate.Value);

                }
                if (!string.IsNullOrEmpty(whdocrepDocNo))
                    tbl = tbl.Where(w => w.whdocrep_DocNo == whdocrepDocNo);
                if (!string.IsNullOrEmpty(whdocrepSos))
                    tbl = tbl.Where(w => w.whdocrep_SOS.Contains(whdocrepSos));
                if (!string.IsNullOrEmpty(whdocrepPartNo))
                    tbl = tbl.Where(w => w.whdocrep_PartNo.Contains(whdocrepPartNo));
                if (!string.IsNullOrEmpty(whdocrepBinLoc))
                    tbl = tbl.Where(w => w.whdocrep_Binloc == whdocrepBinLoc);

                return tbl.ToList();
            }
        }

    }
}
