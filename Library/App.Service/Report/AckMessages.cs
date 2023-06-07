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
    public class AckMessages
    {
        public static List<RptAckMessage> GetList(string groupType, string filterBy, string storeNo,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptAckMessage>("dbo.spGetReportAckMessageByUserId @UserId", parameters).AsQueryable();
              
                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => w.ackm_StoreNo.Trim() == storeNo.Trim());
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.ackm_AreaID == filter) : tbl.Where(w => w.ackm_HubID == filter);
                    }
                }
                return tbl.ToList();

            }
        }

    }
}
