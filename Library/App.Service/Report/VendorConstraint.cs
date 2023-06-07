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
    public class VendorConstraints
    {
        public static List<RptVendorConstraint> GetList(string groupType, string filterBy, string storeNo, DateTime? startDate, DateTime? endDate,string userId)
        {
            using (var db = new EfDbContext())
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var tbl = db.Database.SqlQuery<Data.Domain.RptVendorConstraint>("dbo.spGetReportVendorConstraintByUserId @UserId", parameters).AsQueryable();

                if (startDate.HasValue && endDate.HasValue)
                {
                    tbl = tbl.Where(w => w.vcon_OrderDate.Value.Date >= startDate.Value.Date && w.vcon_OrderDate.Value.Date <= endDate.Value.Date);
                }

                if (!string.IsNullOrEmpty(storeNo))
                    tbl = tbl.Where(w => (w.vcon_StoreNo!=null)&&(w.vcon_StoreNo.Trim().Contains(storeNo)));
                else
                {
                    if (!string.IsNullOrEmpty(filterBy))
                    {
                        var filter = int.Parse(filterBy);
                        tbl = groupType.ToUpper() == "AREA" ? tbl.Where(w => w.vcon_AreaID == filter) : tbl.Where(w => w.vcon_HubID == filter);
                    }
                }
               return tbl.ToList();
            }
        }

    }
}
