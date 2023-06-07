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
    public class UnrealisticCommittedDateBackorderItems
    {
        public static List<RptUnrealisticCommittedDateBackorderItem> GetList(string groupType, string filterBy, string storeNo, string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptUnrealisticCommittedDateBackorderItem>("dbo.spGetReportUnrealisticCommittedDateBackorderItemByUserId @UserId", parameters).AsQueryable();


                //tbl = tbl.Where(w => w.ucdbi_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);


                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.ucdbi_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA"
                            ? tbl.Where(w => w.ucdbi_AreaID == filter)
                            : tbl.Where(w => w.ucdbi_HubID == filter);
                    }
                }
                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.ucdbi_CustID));
                return tbl.ToList();

            }
        }
        public static List<RptUnrealisticCommittedDateBackorderItem> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptUnrealisticCommittedDateBackorderItems.GroupBy(d => d.ucdbi_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
