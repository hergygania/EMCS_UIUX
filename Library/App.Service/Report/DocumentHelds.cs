using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class DocumentHelds
    {
        public static List<RptDocumentHeld> GetList(string groupType, string filterBy, string storeNo, string[] rackCustID, string userId)
        {
            using (var db = new EfDbContext())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentHeld>("dbo.spGetReportDocumentHeldByUserId @UserId", parameters).AsQueryable();


                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rhld_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rhld_AreaID == filter) : tbl.Where(w => w.rhld_HubID == filter);
                    }
                }

                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.rhld_CustID));


              //  tbl = tbl.Where(w => w.rhld_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);
                return tbl.ToList();

            }
        }
        public static List<RptDocumentHeld> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentHelds.GroupBy(d => d.rhld_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }

    }
}
