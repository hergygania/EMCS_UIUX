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
    public class DocumentPendingReleases
    {
        public static List<RptDocumentPendingRelease> GetList(string groupType, string filterBy, string storeNo, string[] rncinvCustID,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentPendingRelease>("dbo.spGetReportPendingReleaseByUserId @UserId", parameters).AsQueryable();

               // tbl = tbl.Where(w => w.rpnd_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);


                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.rpnd_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.rpnd_AreaID == filter) : tbl.Where(w => w.rpnd_HubID == filter);
                    }
                }
                if (rncinvCustID != null)
                    tbl = tbl.Where(w => rncinvCustID.Contains(w.rpnd_CustID));

                return tbl.ToList();
            }
        }
        public static List<RptDocumentPendingRelease> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentPendingReleases.GroupBy(d => d.rpnd_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }

    }
}
