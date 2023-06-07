using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class V_SUPPLIER_STATUS
    {
        public static List<Data.Domain.V_SUPPLIER_STATUS> GetDetailSupplierList(Data.Domain.V_POPUP_DETAIL model)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@order_number", model.order_number != null ? model.order_number.Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@partNumber", model.part_no != null ? model.part_no.Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@caseNo", model.case_number != null ? model.case_number : string.Empty));
                parameterList.Add(new SqlParameter("@jCode", model.JCode != null ? model.JCode.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@sos", model.sos != null ? model.sos.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@rfdcno", model.rfdcno != null ? model.rfdcno.Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@qty_bo", model.qty_bo != null ? model.qty_bo.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@type", model.type != null ? model.type.ToString() : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_SUPPLIER_STATUS>("dbo.spGetSupplierStatus @order_number, @partNumber, @caseNo, @jCode, @sos, @rfdcno, @qty_bo, @type", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_SUPPLIER_STATUS>("dbo.spGetSupplierStatus_New @order_number, @partNumber, @caseNo, @jCode, @sos, @rfdcno, @qty_bo, @type", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.V_SUPPLIER_STATUS> GetDetailSupplierListDummy()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var list = new List<Data.Domain.V_SUPPLIER_STATUS>();
                var data = new Data.Domain.V_SUPPLIER_STATUS()
                {
                    id = 0,
                    case_number = "376346",
                    asn_number = "750016961003",
                    case_desc = "SBH",
                    case_type = "SBH"
                    
                };
                list.Add(data);
                return list;
               
            }
        }
    }
}
