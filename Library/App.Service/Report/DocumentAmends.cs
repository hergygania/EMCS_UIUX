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
    public class DocumentAmends
    {
        public static List<RptDocumentAmend> GetList(string groupType, string filterBy, string storeNo,  string[] amdocCustID, string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentAmend>("dbo.spGetReportDocAmendUserId @UserId", parameters).AsQueryable();

               // tbl = tbl.Where(w => w.amdoc_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.amdoc_STNo == storeNo);
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.amdoc_AreaID == filter) : tbl.Where(w => w.amdoc_HubID == filter);
                    }
                }
                if (amdocCustID != null)
                    tbl = tbl.Where(w => amdocCustID.Contains(w.amdoc_UserID));
                return tbl.ToList();
            }
        }

        public static List<RptDocumentAmend> GetListCustomers(string username)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", username));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentAmend>("dbo.spGetReportDocAmendUserId @UserId", parameters).AsQueryable();
                tbl.GroupBy(x => x.amdoc_UserID).Select(x => x.FirstOrDefault());
                //var tbl = db.RptDocumentAmends.GroupBy(d => d.amdoc_UserID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }

    }
}
