using System;
using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Domain;
using System.Data.Sql;
using System.Data.SqlClient;

namespace App.Service.Report
{
    public class PartOrderDetailReports
    {

        public static List<V_PART_ORDER_DETAIL_REPORT> GetList(
            string groupType, string filterBy, string storeNo,
            string invoiceNo, DateTime? startDate, DateTime? endDate,
            string caseNo, string partNo,
            string partName, string coo,
            string customerReff, string sos
            )
        {

            try
            {
                using (var db = new EfDbContext())
                {
                    db.Database.CommandTimeout = 600;
                    IEnumerable<V_PART_ORDER_DETAIL_REPORT> tbl = db.V_PART_ORDER_DETAIL_REPORTS.ToList();
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        tbl = tbl.Where(w => w.InvoiceDate.Date >= startDate.Value.Date && w.InvoiceDate.Date <= endDate.Value.Date);
                    }

                    if (!string.IsNullOrEmpty(storeNo))
                    {
                        tbl = tbl.Where(w => (w.StoreID != null) && w.StoreID.Trim().Contains(storeNo));
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
                        tbl = tbl.Where(w => w.CaseNo == caseNo);
                    }

                    if (!string.IsNullOrEmpty(invoiceNo))
                    {
                        tbl = tbl.Where(w => w.InvoiceNo.Replace(" ", "").ToUpper().Contains(invoiceNo.Replace(" ", "").ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(partName))
                    {
                        tbl = tbl.Where(w => (w.PartsDescriptionShort != null) && (w.PartsDescriptionShort.Contains(partName)));
                    }
                    if (!string.IsNullOrEmpty(partNo))
                    {
                        tbl = tbl.Where(w => (w.PartsNumber != null) && (w.PartsNumber.Contains(partNo)));
                    }
                    if (!string.IsNullOrEmpty(coo))
                    {
                        tbl = tbl.Where(w => (w.COODescription != null) && (w.COODescription.Contains(coo)));
                    }
                    if (!string.IsNullOrEmpty(customerReff))
                    {
                        tbl = tbl.Where(w => (w.CustomerReff != null) && (w.CustomerReff.Contains(customerReff)));
                    }
                    if (!string.IsNullOrEmpty(sos))
                    {
                        tbl = tbl.Where(w => w.SOS == sos);
                    }
                    return tbl.ToList();

                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public static IEnumerable<CooDescription> GetCooDescriptions()
        {
            using (var db = new EfDbContext())
            {
                return db.CooDescriptions.ToList();
            }
        }

    }
}