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
    public class DocumentAwaitingAcks
    {
        public static List<RptDocumentAwaitingAck> GetList(string groupType, string filterBy, string storeNo, string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentAwaitingAck>("dbo.spGetReportDocumentAwaitingAckUserId @UserId", parameters).AsQueryable();

              //  tbl = tbl.Where(w => w.rack_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rack_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rack_AreaID == filter) : tbl.Where(w => w.rack_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.rack_CustID));
                return tbl.ToList();
            }
        }

        public static List<RptDocumentAwaitingAck> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentAwaitingAcks.GroupBy(d => d.rack_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
