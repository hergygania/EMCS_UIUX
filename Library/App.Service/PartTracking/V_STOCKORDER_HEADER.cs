using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace App.Service.PartTracking
{
	public class V_STOCKORDER_HEADER
	{
		public static List<Data.Domain.V_STOCKORDER_HEADER> GetList(Data.Domain.V_STOCKORDER_HEADER model)
		{

			if(model.selStoreList_Nos != null)
			{
				foreach(string store in model.selStoreList_Nos)
				{
					if(store != "")
						model.storeList_No += "'" + store + "'" + ",";
				}
				if(model.storeList_No != null && model.storeList_No.Length > 1)
					model.storeList_No = model.storeList_No.Substring(0, model.storeList_No.Length - 1);
			}

			if(model.filter_type == "HUB")
				model.hub_id = model.filter_by;
			else if(model.filter_type == "AREA")
				model.area_id = model.filter_by;

			if(!model.doc_date_start.HasValue)
			{
				throw new Exception("Please enter Doc Entry date ...!");
			}
			if(model.doc_date_start.HasValue && model.doc_date_end.HasValue && (model.doc_date_end.Value - model.doc_date_start.Value).TotalDays > 31)
			{
				throw new Exception("'Doc Entry' maximum range date must less than 31 days ...! ");
			}

			var _chk = model.order_number + "" + model.case_no + "" + model.part_number + "" + model.part_desc + "" + model.MaterialType;
			if(string.IsNullOrEmpty(_chk) && (model.doc_date_end.Value - model.doc_date_start.Value).TotalDays > 1)
			{
				if(string.IsNullOrEmpty(model.storeList_No) && string.IsNullOrEmpty(model.filter_by))
				{
					throw new Exception("Please select Area/Hub, store Number or other field, at-least one ...!");
				}
			}

			var data = new List<Data.Domain.V_STOCKORDER_HEADER>();

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				db.DbContext.Database.CommandTimeout = 600;
				List<SqlParameter> parameterList = new List<SqlParameter>();
				parameterList.Add(new SqlParameter("@rporne", DBNull.Value));
				parameterList.Add(new SqlParameter("@ordsos", DBNull.Value));
				if(model.storeList_No != null)
					parameterList.Add(new SqlParameter("@store_no", model.storeList_No));
				else
					parameterList.Add(new SqlParameter("@store_no", DBNull.Value));

				parameterList.Add(new SqlParameter("@order_number", model.order_number != null ? model.order_number.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@filter_type", model.filter_type != null ? model.filter_type.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@hub_id", model.hub_id != null ? model.hub_id.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@area_id", model.area_id != null ? model.area_id.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@sr_type", model.sr_type != null ? model.sr_type.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@order_class", model.order_class != null ? model.order_class : string.Empty));
				parameterList.Add(new SqlParameter("@order_profile", model.order_profile != null ? model.order_profile : string.Empty));
				parameterList.Add(new SqlParameter("@shp_type", model.shp_type != null ? model.shp_type : string.Empty));
				parameterList.Add(new SqlParameter("@agreement", model.agreement != null ? model.agreement : string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_start", model.doc_date_start.HasValue ? model.doc_date_start.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_end", model.doc_date_end.HasValue ? model.doc_date_end.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@supply_docinv", model.supply_docinv != null ? model.supply_docinv : string.Empty));
				parameterList.Add(new SqlParameter("@supply_docinv_date", model.supply_docinv_date.HasValue ? model.supply_docinv_date.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@case_no", model.case_no != null ? model.case_no.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@part_number", model.part_number != null ? model.part_number.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@part_desc", model.part_desc != null ? model.part_desc.Trim() : string.Empty));
                parameterList.Add(new SqlParameter("@MaterialType", model.MaterialType != null ? model.MaterialType.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@doc_status", model.doc_status != null ? model.doc_status.Trim() : string.Empty));
				SqlParameter[] parameters = parameterList.ToArray();

                //data = db.DbContext.Database.SqlQuery<Data.Domain.V_STOCKORDER_HEADER>("dbo.spGetStockOrderHeader_New @rporne, @ordsos, @store_no, @order_number, @filter_type, @hub_id, @area_id, @sr_type, @order_class, @order_profile, @shp_type, @agreement, @doc_date_start, @doc_date_end, @supply_docinv, @supply_docinv_date, @case_no, @part_number, @part_desc, @sos_group, @doc_status", parameters).ToList();
                data = db.DbContext.Database.SqlQuery<Data.Domain.V_STOCKORDER_HEADER>("dbo.spGetStockReplenishment_New @rporne, @ordsos, @store_no, @order_number, @filter_type, @hub_id, @area_id, @sr_type, @order_class, @order_profile, @shp_type, @agreement, @doc_date_start, @doc_date_end, @supply_docinv, @supply_docinv_date, @case_no, @part_number, @part_desc, @MaterialType, @doc_status", parameters).ToList();
                if (data.Count == 1)
					return data;
			}

			var grp = data
			.GroupBy(g => new { g.RPORNE, g.store_no, g.order_number, g.supply_docinv })
			.Select(g => new
			{
				xRPORNE = g.Key.RPORNE.Trim(),
				xStoreno = g.Key.store_no,
				xOrderNo = g.Key.order_number,
                xinvoiceno = g.Key.supply_docinv,
				xDocDate = g.Min(m => m.doc_date), //request angga 31-jan-16
				xId = g.Min(m=>m.id),
				xCount = g.Count()
			}).ToList();

			var list = (from c in data
									from d in grp.Where(w => w.xRPORNE == c.RPORNE.Trim() && w.xStoreno == c.store_no && w.xOrderNo == c.order_number && w.xinvoiceno == c.supply_docinv
									&& w.xDocDate == c.doc_date && w.xId == c.id) // && w.xStatusNo == c.doc_status_num)// && w.xEtlDate == c.Etl_Date)
									select c)
									.OrderByDescending(o => o.doc_date).ThenByDescending(o => o.store_no).ThenByDescending(o => (o.supply_docinv + "")).ToList();

			return list;
		}


		public static List<Data.Domain.V_STOCKODER_DETAIL> GetDetailList(Data.Domain.V_STOCKORDER_HEADER model)
		{
			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				string ordsos = model.ORDSOS != null ? model.ORDSOS.Trim() : string.Empty;
				string storeNo = model.store_no != null ? model.store_no.ToString().Trim() : string.Empty;
				string orderno = model.order_number != null ? model.order_number.ToString().Trim() : string.Empty;
				string trackingId = model.tracking_id != null ? model.tracking_id.Trim() : string.Empty;

				List<SqlParameter> parameterList = new List<SqlParameter>();
				parameterList.Add(new SqlParameter("@rporne", model.RPORNE != null ? model.RPORNE.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@ordsos", model.ORDSOS != null ? model.ORDSOS.Trim() : string.Empty));
				parameterList.Add(new SqlParameter("@store_no", storeNo));
				parameterList.Add(new SqlParameter("@orderNo", orderno));
				parameterList.Add(new SqlParameter("@invoiceNo", string.IsNullOrEmpty(model.supply_docinv) ? string.Empty : model.supply_docinv.Trim()));
				parameterList.Add(new SqlParameter("@doc_date", model.doc_date.HasValue ? model.doc_date.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@invoice_date", model.supply_docinv_date.HasValue ? model.supply_docinv_date.Value.ToString("yyyy-MM- dd") : string.Empty));
				parameterList.Add(new SqlParameter("@receiveDate", model.receiveDate.HasValue ? model.receiveDate.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@tracking_id", trackingId));
				parameterList.Add(new SqlParameter("@podDate", model.podDate.HasValue ? model.podDate.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@pupDate", model.pupDate.HasValue ? model.pupDate.Value.ToString("yyyy-MM-dd") : string.Empty));


				SqlParameter[] parameters = parameterList.ToArray();

				try
				{
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.V_STOCKODER_DETAIL>("dbo.spGetStockReplenishmentDetail @rporne, @ordsos, @store_no, @orderNo, @invoiceNo, @doc_date, @invoice_date, @receiveDate,@tracking_id,@podDate,@pupDate", parameters).ToList();
                    if (data.Count == 1)
						return data;

					var grp = data
					.GroupBy(g => new { g.part_no, g.doc_number, g.invoice_no, g.case_number, g.doc_line })
					.Select(g => new
					{
						xpart_no = g.Key.part_no,
						xdoc_number = g.Key.doc_number,
						xinvoice_no = g.Key.invoice_no,
						xCase = g.Key.case_number,
						xDocDate = g.Min(m => m.doc_date),
                        xDocLine = g.Key.doc_line,
						xId = g.Max(m=>m.id)
					}).ToList();

					var list = (from c in data
											from d in grp.Where(w => w.xpart_no == c.part_no && w.xdoc_number == c.doc_number && w.xinvoice_no == c.invoice_no
											&& w.xCase == c.case_number && w.xDocDate == c.doc_date && w.xDocLine == c.doc_line && w.xId==c.id) // && w.xStatusNo == c.doc_status_num && w.xEtlDate == c.Etl_Date)
											select c)
											.OrderBy(o => o.part_no).ToList();
					return list;
				}
				catch(Exception ex)
				{
					var exx = ex.Message;
					string msg = "";
					if(string.IsNullOrEmpty(storeNo))
					{
						msg = msg + "Store_No Empty; ";
					}

					throw new Exception(msg + exx);
				}

			}
		}
	}
}
