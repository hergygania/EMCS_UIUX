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
    public class DocumentSuspends
    {
        public static List<RptDocumentSuspend> GetList(string groupType, string filterBy, string storeNo, string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentSuspend>("dbo.spGetReportDocumentSuspendByUserId @UserId", parameters).AsQueryable();

              //  tbl = tbl.Where(w => w.rspnd_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);
                
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rspnd_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rspnd_AreaID == filter) : tbl.Where(w => w.rspnd_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.rspnd_CustID));
                return tbl.ToList();

            }
        }
        public static List<RptDocumentSuspend> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentSuspends.GroupBy(d => d.rspnd_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }

    }
}
