using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class DocumentNonConsolidateInvoices
    {
        public static List<RptDocumentNonConsolidateInvoice>
            GetList(string groupType, string filterBy, string storeNo, string[] rncinvCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentNonConsolidateInvoice>("dbo.spGetReportDocumentNonConsolidateInvoiceUserId @UserId", parameters).AsQueryable();

              //  tbl = tbl.Where(w => w.rncinv_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rncinv_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rncinv_AreaID == filter) : tbl.Where(w => w.rncinv_HubID == filter);
                    }
                }
                if (rncinvCustID != null)
                    tbl = tbl.Where(w => rncinvCustID.Contains(w.rncinv_CustID));

                return tbl.ToList();
            }
        }

        public static List<RptDocumentNonConsolidateInvoice> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentNonConsolidateInvoices.GroupBy(d => d.rncinv_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
