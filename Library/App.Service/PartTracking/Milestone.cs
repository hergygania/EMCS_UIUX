using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class Milestone
    {
        public static List<Data.Domain.Milestone> GetMilestoneListOLD(App.Data.Domain.V_POPUP_DETAIL model)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;   
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", model.type != null ? model.type : string.Empty));
                parameterList.Add(new SqlParameter("@sos", model.sos != null ? model.sos : string.Empty));
                parameterList.Add(new SqlParameter("@order_class", model.order_class != null ? model.order_class : string.Empty));
                parameterList.Add(new SqlParameter("@doc_date", model.doc_date.HasValue ? model.doc_date.Value.ToShortDateString() : string.Empty));
                parameterList.Add(new SqlParameter("@trilc", model.trilc != null ? model.trilc : string.Empty));

                if (model.invoice_date.HasValue)
                    parameterList.Add(new SqlParameter("@invoice_date", model.invoice_date.Value.ToShortDateString()));
                else
                    parameterList.Add(new SqlParameter("@invoice_date", DBNull.Value));

                if (model.pupDate.HasValue)
                    parameterList.Add(new SqlParameter("@pupDate", model.pupDate.Value.ToShortDateString()));
                else
                    parameterList.Add(new SqlParameter("@pupDate", DBNull.Value));

                if (model.podDate.HasValue)
                    parameterList.Add(new SqlParameter("@podDate", model.podDate.Value.ToShortDateString()));
                else
                    parameterList.Add(new SqlParameter("@podDate", DBNull.Value));

                if (model.receiveDate.HasValue)
                    parameterList.Add(new SqlParameter("@receiveDate", model.receiveDate.Value.ToShortDateString()));
                else
                    parameterList.Add(new SqlParameter("@receiveDate", DBNull.Value));

                parameterList.Add(new SqlParameter("@case_no", model.case_number != null ? model.case_number : string.Empty));
                parameterList.Add(new SqlParameter("@stno", model.store_no != null ? model.store_no : string.Empty));
                parameterList.Add(new SqlParameter("@rporne", model.RPORNE != null ? model.RPORNE : string.Empty));
                parameterList.Add(new SqlParameter("@partNo", model.part_no != null ? model.part_no : string.Empty));
                parameterList.Add(new SqlParameter("@doc_number", model.doc_number != null ? model.doc_number : string.Empty));
                if (!string.IsNullOrEmpty(model.doc_invoice))
                    parameterList.Add(new SqlParameter("@doc_invoice", model.doc_invoice));
                else
                    parameterList.Add(new SqlParameter("@doc_invoice", DBNull.Value));

                parameterList.Add(new SqlParameter("@cmnt1", model.cmnt1 != null ? model.cmnt1 : string.Empty));
                if (model.ackDate.HasValue)
                    parameterList.Add(new SqlParameter("@boSubDate", model.ackDate.Value.ToShortDateString()));
                else
                    parameterList.Add(new SqlParameter("@boSubDate", DBNull.Value));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.Milestone>("dbo.spGetMilestoneList @type, @sos, @order_class, @doc_date, @trilc, @invoice_date,@pupDate,@podDate, @receiveDate, @case_no, @stno, @rporne, @partNo,@doc_number, @doc_invoice, @cmnt1, @boSubDate", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.Milestone> GetMilestoneList(App.Data.Domain.V_POPUP_DETAIL model)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@rporne", model.RPORNE != null ? model.RPORNE : string.Empty));                                            
                parameterList.Add(new SqlParameter("@partNo", model.part_no != null ? model.part_no : string.Empty));
                parameterList.Add(new SqlParameter("@type", model.type != null ? model.type : string.Empty));
                parameterList.Add(new SqlParameter("@sos", model.sos != null ? model.sos : string.Empty));
                //parameterList.Add(new SqlParameter("@XFORNO", model.xforno != null ? model.xforno : string.Empty));
                //parameterList.Add(new SqlParameter("@Source", model.source != null ? model.source : string.Empty));
                //parameterList.Add(new SqlParameter("@doc_invoice", model.doc_invoice != null ? model.doc_invoice : string.Empty));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.Milestone>(
                    "[pis].[spGetMilestone] @rporne, @partNo, @type, @sos", parameters).ToList();

                return data;
            }
        }
    }
}
