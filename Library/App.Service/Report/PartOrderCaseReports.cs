using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class PartOrderCaseReports
    {
        public static List<V_PART_ORDER_CASE_REPORT> GetList
            (
            string groupType, string filterBy,
            string storeNo, string caseNo,
            string caseType, string caseDescription,
            string invoiceNo, DateTime? invoiceStartDate,
            DateTime? invoiceEndDate, decimal? weightFrom,
            decimal? weightTo, decimal? lengthFrom,
            decimal? lengthTo, decimal? wideFrom,
            decimal? wideTo, decimal? heightFrom,
            decimal? heightTo)
        {
            using (var db = new EfDbContext())
            {
                IEnumerable<V_PART_ORDER_CASE_REPORT> tbl = db.V_PART_ORDER_CASE_REPORTS.ToList();
                if (invoiceStartDate.HasValue && invoiceEndDate.HasValue)
                {
                    tbl = tbl.Where(
                        w => w.InvoiceDate.Date >= invoiceStartDate.Value.Date
                            && w.InvoiceDate.Date <= invoiceEndDate.Value.Date);

                }

                if (!string.IsNullOrEmpty(storeNo))
                {
                    tbl = tbl.Where(w => (w.StoreID != null) && (w.StoreID.Trim().Contains(storeNo)));
                }
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.AreaID == filter) : tbl.Where(w => w.HubID == filter);
                    }
                }

                if (!string.IsNullOrEmpty(caseNo))
                {
                    tbl = tbl.Where(w => w.CaseNo.Trim() == caseNo.Trim());
                }

                if (!string.IsNullOrEmpty(caseType))
                {
                    tbl = tbl.Where(w => w.CaseType == caseType);
                }
                if (!string.IsNullOrEmpty(caseDescription))
                {
                    tbl = tbl.Where(w => w.CaseDescription == caseDescription);
                }
                if (!string.IsNullOrEmpty(invoiceNo))
                {
                    tbl = tbl.Where(w => w.InvoiceNo.ToUpper().Replace(" ", "").ToUpper().Contains(invoiceNo.Replace(" ", "").ToUpper()));
                }
                if (weightFrom != null && weightTo != null)
                {
                    tbl = tbl.Where(
                       w => w.WeightKG >= weightFrom
                           && w.WeightKG <= weightTo);
                }
                if (lengthFrom != null && lengthTo != null)
                {
                    tbl = tbl.Where(
                       w => w.LengthCM >= lengthFrom
                           && w.LengthCM <= lengthTo);
                }
                if (wideFrom != null && wideFrom != null)
                {
                    tbl = tbl.Where(
                       w => w.WideCM >= wideFrom
                           && w.WideCM <= wideTo);
                }
                if (heightFrom != null && heightTo != null)
                {
                    tbl = tbl.Where(
                       w => w.HeightCM >= heightFrom
                           && w.HeightCM <= heightTo);
                }
                return tbl.ToList();

            }
        }

        public static List<string> GetCaseTypes()
        {
            using (var db = new EfDbContext())
            {
                return db.PartsOrderCases.OrderBy(a => a.CaseType).GroupBy(a => a.CaseType).Select(a => a.Key).ToList();
            }
        }
        public static List<string> GetCaseDescription()
        {
            using (var db = new EfDbContext())
            {
                return db.PartsOrderCases.OrderBy(a => a.CaseDescription).GroupBy(a => a.CaseDescription).Select(a => a.Key).ToList();
            }
        }

    }
}
