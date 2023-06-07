using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class V_TRAKINDO_STATUS
    {
        //public static List<Data.Domain.V_TRAKINDO_STATUS> GetDetailTrakindoList(string id)
        //{
        //    using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
        //    {
        //        var data = new Data.Domain.V_TRAKINDO_STATUS()
        //        {
        //            id = 0,
        //            store_number = "31",
        //            case_number = "31C593750D",
        //            order_qty = "28",
        //            order_date = new DateTime(),
        //            supply_qty = "28",
        //            supply_date = new DateTime(),
        //            bo_qty = "0",
        //            bo_to_fill = "28",
        //            bo_to_date = new DateTime()
        //        };
        //        var list = new List<Data.Domain.V_TRAKINDO_STATUS>();
        //        list.Add(data);
        //        return list;
        //    }
        //}

        public static List<Data.Domain.V_TRAKINDO_STATUS> GetDetailTrakindoList(Data.Domain.V_POPUP_DETAIL model)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", model.type != null ? model.type.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@rporne", model.RPORNE != null ? model.RPORNE.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@ordsos", model.ORDSOS != null ? model.ORDSOS.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@store_no", model.store_no != null ? model.store_no.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@rcdcd", model.RCDCD != null ? model.RCDCD.ToString() : string.Empty));
				parameterList.Add(new SqlParameter("@partNumber", model.part_no != null ? model.part_no.Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@caseNo", model.case_number != null ? model.case_number : string.Empty));
                parameterList.Add(new SqlParameter("@jCode", model.JCode != null ? model.JCode.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@sos", model.sos != null ? model.sos.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@docDate", model.doc_date.HasValue ? model.doc_date.Value.ToString("yyyy-MM-dd") : string.Empty));
                parameterList.Add(new SqlParameter("@rfdcno", model.rfdcno != null ? model.rfdcno.ToString().Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@LineItem", model.LineItem != null ? model.LineItem.ToString().Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@Source", model.source != null ? model.source.ToString().Trim() : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                try
                {
                    //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_TRAKINDO_STATUS>("dbo.spGetTrakindoStatus @type, @rporne, @ordsos, @store_no, @rcdcd, @partNumber, @caseNo, @jCode, @sos, @docDate, @rfdcno ", parameters);
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.V_TRAKINDO_STATUS>("dbo.spGetTrakindoStatus_New @type, @rporne, @ordsos, @store_no, @rcdcd, @partNumber, @caseNo, @jCode, @sos, @docDate, @rfdcno, @LineItem, @Source ", parameters);
                    var list = data.ToList();
                    return list;
                }
                catch (Exception e)
                {
                    var ex = e.Message;
                    string msg = "";
                    throw new Exception(msg + ex);
                }
            }
        }
    }
}
