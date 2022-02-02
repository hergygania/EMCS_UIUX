using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class PartOrderReports
    {
        public static List<V_PART_ORDER_REPORT> GetList(
            string invoiceNo, DateTime? invoiceStartDate,
            DateTime? invoiceEndDate, string groupType,
            string filterBy, string storeNo, string jCode,
            int? shippingInstruction, bool? isHazardous,
            string agreementType, string sos
            )
        {
            using (var db = new EfDbContext())
            {
                IEnumerable<V_PART_ORDER_REPORT> tbl = db.V_PART_ORDER_REPORTS.ToList();
                if (invoiceStartDate.HasValue && invoiceEndDate.HasValue)
                {
                    tbl = tbl.Where(w => w.InvoiceDate.Date >= invoiceStartDate.Value.Date && w.InvoiceDate.Date <= invoiceEndDate.Value.Date);

                }

                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    tbl = tbl.Where(w => w.InvoiceNo.Replace(" ", "").ToUpper().Contains(invoiceNo.Replace(" ", "").ToUpper()));

                }

                if (!string.IsNullOrEmpty(jCode))
                {
                    tbl = tbl.Where(w => w.JCode.Contains(jCode));

                }

                if (shippingInstruction != null)
                {
                    tbl = tbl.Where(w => w.ShippingInstructionID == shippingInstruction);
                }
                if (isHazardous != null)
                {
                    tbl = tbl.Where(w => w.IsHazardous == isHazardous);
                }

                if (!string.IsNullOrEmpty(agreementType))
                {
                    tbl = tbl.Where(w => w.AgreementType == agreementType);
                }

                if (!string.IsNullOrEmpty(jCode))
                {
                    tbl = tbl.Where(w => w.JCode == jCode);
                }

                if (!string.IsNullOrEmpty(storeNo))
                {
                    tbl = tbl.Where(w => (w.StoreID != null)&&(w.StoreID.Contains(storeNo)));
                }
                if (!string.IsNullOrEmpty(sos))
                {
                    tbl = tbl.Where(w => (w.SOS != null) && (w.SOS.Contains(sos)));
                }
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.AreaID == filter) : tbl.Where(w => w.HubID == filter);
                    }
                }

                return tbl.ToList();

            }
        }

        public static List<string> GetAgreementTypes()
        {
            using (var db = new EfDbContext())
            {
                IQueryable<string> tbl = db.V_PART_ORDER_REPORTS.OrderBy(a => a.AgreementType).GroupBy(a => a.AgreementType).Select(a => a.Key);
                return tbl.ToList();
            }
        }

    }
}
