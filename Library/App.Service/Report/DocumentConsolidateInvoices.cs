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
    public class DocumentConsolidateInvoices
    {
        public static List<RptDocumentConsolidateInvoice> GetList
            (string groupType, string filterBy, string storeNo, string[] rcinvCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentConsolidateInvoice>("dbo.spGetReportDocumentConsolidateInvoiceUserId @UserId", parameters).AsQueryable();


              //  tbl = tbl.Where(w => w.rcinv_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rcinv_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rcinv_AreaID == filter) : tbl.Where(w => w.rcinv_HubID == filter);
                    }
                }
                if (rcinvCustID != null)
                    tbl = tbl.Where(w => rcinvCustID.Contains(w.rcinv_CustID));

                return tbl.ToList();
            }
        }

        public static List<RptDocumentConsolidateInvoice> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentConsolidateInvoices.GroupBy(d => d.rcinv_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
