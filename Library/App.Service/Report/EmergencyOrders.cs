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
    public class EmergencyOrders
    {
        public static List<RptEmergencyOrder> GetList(string groupType, string filterBy, string storeNo, string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptEmergencyOrder>("dbo.spGetReportEmergencyOrderUserId @UserId", parameters).AsQueryable();

              //  tbl = tbl.Where(w => w.emgor_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);


                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.emgor_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.emgor_AreaID == filter) : tbl.Where(w => w.emgor_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.emgor_CustID));
                return tbl.ToList();

            }
        }
        public static List<RptEmergencyOrder> GetListCustomers(string username)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", username));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptEmergencyOrder>("dbo.spGetReportEmergencyOrderUserId @UserId", parameters).AsQueryable();
                //var tbl = db.RptEmergencyOrders.GroupBy(d => d.emgor_CustID).Select(d => d.FirstOrDefault());
                return tbl.GroupBy(x => x.emgor_CustID).Select(x => x.FirstOrDefault()).ToList();
            }
        }
    }
}
