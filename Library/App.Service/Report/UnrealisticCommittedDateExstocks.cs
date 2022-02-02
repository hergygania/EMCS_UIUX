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
    public class UnrealisticCommittedDateExstocks
    {
        public static List<RptUnrealisticCommittedDateExstock> GetList(string groupType, string filterBy, string storeNo,string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptUnrealisticCommittedDateExstock>("dbo.spGetReportCommittedDateExstockByUserId @UserId", parameters).AsQueryable();

               
                   // tbl =tbl.Where(w =>w.urcdx_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.urcdx_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA"
                            ? tbl.Where(w => w.urcdx_AreaID == filter)
                            : tbl.Where(w => w.urcdx_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.urcdx_CustID));
                return tbl.ToList();
            }
        }

        public static List<RptUnrealisticCommittedDateExstock> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptUnrealisticCommittedDateExstocks.GroupBy(d => d.urcdx_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
