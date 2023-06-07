using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class V_FORWADER_STATUS
    {
        public static List<Data.Domain.V_FORWADER_STATUS> GetDetailForwarderList(string id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var data = new Data.Domain.V_FORWADER_STATUS()
                {
                    id = 0,
                    case_number = "376346",
                    da_number = "750016961003",
                    last_location = "SBH",
                    origin = "SBH",
                    destination = "SRG",
                    service_type = "XQ",
                    status = "PUP",
                    status_date = DateTime.Now
                };
                var list = new List<Data.Domain.V_FORWADER_STATUS>();
                list.Add(data);
                return list;
            }
        }

        public static List<Data.Domain.V_FORWADER_STATUS> GetDetailForwarderList(Data.Domain.V_POPUP_DETAIL model)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", model.type != null ? model.type.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@rcdcd", model.RCDCD != null ? model.RCDCD.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@rporne", model.RPORNE != null ? model.RPORNE.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@caseNo", model.case_number != null ? model.case_number : string.Empty));
                parameterList.Add(new SqlParameter("@cmnt1", model.cmnt1 != null ? model.cmnt1.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@trilc", model.trilc != null ? model.trilc.ToString() : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_FORWADER_STATUS>("dbo.spGetForwarderStatus @type, @rcdcd, @rporne, @caseNo, @cmnt1, @trilc", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_FORWADER_STATUS>("dbo.spGetForwarderStatus_New @type, @rcdcd, @rporne, @caseNo, @cmnt1, @trilc", parameters).ToList();
                var list = data.OrderBy(o => o.status_date).ToList();
                return list;
            }
        }
    }
}
