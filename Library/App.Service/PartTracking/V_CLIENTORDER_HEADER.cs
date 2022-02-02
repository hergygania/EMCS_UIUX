using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class V_CLIENTORDER_HEADER
    {
        public static IList<Data.Domain.V_CLIENTORDER_HEADER> GetList(Data.Domain.V_CLIENTORDER_HEADER model)
        {

            if (model.selCustList_Nos != null)
            {
                foreach (string cust in model.selCustList_Nos)
                {
                    if (cust != "")
                        model.cust_no += "'" + cust + "'" + ",";
                }

                if (model.cust_no != null && model.cust_no.Length > 1)
                    model.cust_no = model.cust_no.Substring(0, model.cust_no.Length - 1);
            }
            else
                model.cust_no = string.Empty;



            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_doc_no", model.ref_doc_no != null ? model.ref_doc_no : string.Empty));
                parameterList.Add(new SqlParameter("@cust_group_no", model.cust_group_no != null ? model.cust_group_no : string.Empty));
                parameterList.Add(new SqlParameter("@cust_no", model.cust_no != null ? model.cust_no : string.Empty));
                parameterList.Add(new SqlParameter("@cust_po_no", model.cust_po_no != null ? model.cust_po_no : string.Empty));
                parameterList.Add(new SqlParameter("@model", model.model != null ? model.model : string.Empty));
                parameterList.Add(new SqlParameter("@serial", model.serial != null ? model.serial : string.Empty));
                parameterList.Add(new SqlParameter("@equip_number", model.equip_number != null ? model.equip_number : string.Empty));
                parameterList.Add(new SqlParameter("@part_number", model.part_number != null ? model.part_number : string.Empty));
                parameterList.Add(new SqlParameter("@part_desc", model.part_desc != null ? model.part_desc : string.Empty));
                parameterList.Add(new SqlParameter("@cust_po_date_start", model.cust_po_date_start.HasValue ? model.cust_po_date_start.Value.ToString("yyyyMMdd") : string.Empty));
                parameterList.Add(new SqlParameter("@cust_po_date_end", model.cust_po_date_end.HasValue ? model.cust_po_date_end.Value.ToString("yyyyMMdd") : string.Empty));
                parameterList.Add(new SqlParameter("@userType", "External"));

                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CLIENTORDER_HEADER>("dbo.spGetClientOrderHeader @ref_doc_no, @cust_group_no, @cust_no, @cust_po_no, @model, @serial, @equip_number, @part_number, @part_desc, @cust_po_date_start, @cust_po_date_end", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CLIENTORDER_HEADER>("dbo.spGetOrderHeaderInternalExternal @ref_doc_no, " +
                            "@cust_group_no, @cust_no, @cust_po_no, @store_no, @model_type, @model, @serial, " +
                            "@equip_number, @doc_date_start, @doc_date_end, @filter_type, @hub_id, @area_id, " +
                            "@part_number, @part_desc, @order_status, @userType", parameters).ToList();
                return data;
            }
        }
    }
}
