using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data;
using App.Data.Domain;

namespace App.Service.Report
{
    public class DocumentReprints
    {
        public static List<RptDocumentReprint> GetList(string groupType, string filterBy, string storeNo,  string[] rackCustID,string userId)
        {
            using (var db = new EfDbContext())
            {

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptDocumentReprint>("dbo.spGetReportDocumentReprintUserId @UserId", parameters).AsQueryable();


                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.docrep_Store.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.docrep_AreaID == filter) : tbl.Where(w => w.docrep_HubID == filter);
                    }
                }

                if (rackCustID != null)
                    tbl = tbl.Where(w => rackCustID.Contains(w.docrep_CustID));

            //    tbl = tbl.Where(w => w.docrep_CreatedOn.Date <= DateTime.Now.AddDays(-1).Date);

               
                return tbl.ToList();

            }
        }

        public static IEnumerable<RptDocumentReprint> GetListCustomers()
        {
            using (var db = new ReportDbContext())
            {
                var tbl = db.RptDocumentReprints.GroupBy(d => d.docrep_CustID).Select(d => d.FirstOrDefault());
                return tbl.ToList();
            }
        }
    }
}
