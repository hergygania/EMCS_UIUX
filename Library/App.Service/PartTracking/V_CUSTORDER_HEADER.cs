using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Data.Caching;

namespace App.Service.PartTracking
{
	public class V_CUSTORDER_HEADER
	{
		private const string cacheName = "App.partTracking.V_CUSTORDER_HEADER";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.V_CUSTORDER_HEADER> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{
					var tb = db.CreateRepository<Data.Domain.V_CUSTORDER_HEADER>().Table.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}

		public static IList<Data.Domain.V_CUSTORDER_HEADER> GetList(Data.Domain.V_CUSTORDER_HEADER model)
		{
			if(model.selCustList_Nos != null)
			{
                //foreach(string cust in model.selCustList_Nos)
                //{
                //	if(cust != "")
                //		model.cust_no += "'" + cust + "'" + ",";
                //}
                model.cust_no = model.selCustList_Nos;
                
			}
			else
				model.cust_no = string.Empty;

			if(model.selStoreList_Nos != null)
			{
				foreach(string Plant in model.selStoreList_Nos)
				{
                    if (Plant != "")
                        model.storeList_No += "'" + Plant + "'" + ",";
				}
				if(model.storeList_No != null && model.storeList_No.Length > 1)
					model.storeList_No = model.storeList_No.Substring(0, model.storeList_No.Length - 1);
			}
			else
				model.storeList_No = string.Empty;

			if(model.filter_type == "HUB")
				model.hub_id = model.filter_by;
			else if(model.filter_type == "AREA")
				model.area_id = model.filter_by;

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{

				db.DbContext.Database.CommandTimeout = 600;
				List<SqlParameter> parameterList = new List<SqlParameter>();
				parameterList.Add(new SqlParameter("@ref_doc_no", model.ref_doc_no != null ? model.ref_doc_no : string.Empty));
				parameterList.Add(new SqlParameter("@cust_group_no", model.cust_group_no != null ? model.cust_group_no : string.Empty));
				parameterList.Add(new SqlParameter("@cust_no", model.cust_no != null ? model.cust_no : string.Empty));
				parameterList.Add(new SqlParameter("@cust_po_no", model.cust_po_no != null ? model.cust_po_no : string.Empty));
                parameterList.Add(new SqlParameter("@cust_po_date", model.cust_po_date.HasValue ? model.cust_po_date.Value.ToString("yyyy-MM-dd") : string.Empty));
                parameterList.Add(new SqlParameter("@plant", model.storeList_No != null ? model.storeList_No : string.Empty));
				parameterList.Add(new SqlParameter("@invoice_type", model.invoice_type != null ? model.invoice_type : string.Empty));
				parameterList.Add(new SqlParameter("@wo_number", model.wo_number != null ? model.wo_number : string.Empty));
				parameterList.Add(new SqlParameter("@seg_number", model.seg_number != null ? model.seg_number : string.Empty));
				parameterList.Add(new SqlParameter("@model_type", model.model_type != null ? model.model_type : string.Empty));
				parameterList.Add(new SqlParameter("@model", model.model != null ? model.model : string.Empty));
				parameterList.Add(new SqlParameter("@serial", model.serial != null ? model.serial : string.Empty));
				parameterList.Add(new SqlParameter("@equip_number", model.equip_number != null ? model.equip_number : string.Empty));
				parameterList.Add(new SqlParameter("@shp_type", model.shp_type != null ? model.shp_type : string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_start", model.doc_date_start.HasValue ? model.doc_date_start.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_end", model.doc_date_end.HasValue ? model.doc_date_end.Value.ToString("yyyy-MM-dd") : string.Empty));
				parameterList.Add(new SqlParameter("@filter_type", model.filter_type != null ? model.filter_type : string.Empty));
				parameterList.Add(new SqlParameter("@hub_id", model.hub_id != null ? model.hub_id : string.Empty));
				parameterList.Add(new SqlParameter("@area_id", model.area_id != null ? model.area_id : string.Empty));
				parameterList.Add(new SqlParameter("@part_number", model.part_number != null ? model.part_number : string.Empty));
				parameterList.Add(new SqlParameter("@part_desc", model.part_desc != null ? model.part_desc : string.Empty));
				parameterList.Add(new SqlParameter("@order_status", model.order_status != null ? model.order_status : string.Empty));
                parameterList.Add(new SqlParameter("@userType", "Internal"));
				//parameterList.Add(new SqlParameter("@cust_po_date_start", model.cust_po_date_start.HasValue ? model.cust_po_date_start.Value.ToString("yyyyMMdd") : string.Empty));
				//parameterList.Add(new SqlParameter("@cust_po_date_end", model.cust_po_date_end.HasValue ? model.cust_po_date_end.Value.ToString("yyyyMMdd") : string.Empty));

				SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTORDER_HEADER>("dbo.spGetCustOrderHeader_New @ref_doc_no, @cust_group_no, @cust_no, @cust_po_no, @store_no, @invoice_type, @wo_number, @seg_number,@model_type, @model, @serial, @equip_number, @shp_type, @doc_date_start, @doc_date_end, @filter_type, @hub_id, @area_id, @part_number, @part_desc, @order_status", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTORDER_HEADER>("dbo.spGetOrderHeaderInternalExternal @ref_doc_no, @cust_group_no, @cust_no, @cust_po_no, @cust_po_date, @plant, @invoice_type, @wo_number, @seg_number,@model_type, @model, @serial, @equip_number, @shp_type, @doc_date_start, @doc_date_end, @filter_type, @hub_id, @area_id, @part_number, @part_desc, @order_status, @userType", parameters).ToList();
                var list = data.OrderByDescending(o => o.doc_date).ToList();
				return list;
			}
		}

		public static Data.Domain.V_CUSTORDER_HEADER GetOne(string ref_doc_no)
		{
			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				List<SqlParameter> parameterList = new List<SqlParameter>();
				parameterList.Add(new SqlParameter("@ref_doc_no", ref_doc_no));
				parameterList.Add(new SqlParameter("@cust_group_no", string.Empty));
				parameterList.Add(new SqlParameter("@cust_no", string.Empty));
				parameterList.Add(new SqlParameter("@cust_po_no", string.Empty));
				parameterList.Add(new SqlParameter("@store_no", string.Empty));
				parameterList.Add(new SqlParameter("@invoice_type", string.Empty));
				parameterList.Add(new SqlParameter("@wo_number", string.Empty));
				parameterList.Add(new SqlParameter("@seg_number", string.Empty));
				parameterList.Add(new SqlParameter("@model_type", string.Empty));
				parameterList.Add(new SqlParameter("@model", string.Empty));
				parameterList.Add(new SqlParameter("@serial", string.Empty));
				parameterList.Add(new SqlParameter("@equip_number", string.Empty));
				parameterList.Add(new SqlParameter("@shp_type", string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_start", string.Empty));
				parameterList.Add(new SqlParameter("@doc_date_end", string.Empty));
				parameterList.Add(new SqlParameter("@filter_type", string.Empty));
				parameterList.Add(new SqlParameter("@hub_id", string.Empty));
				parameterList.Add(new SqlParameter("@area_id", string.Empty));
				parameterList.Add(new SqlParameter("@part_number", string.Empty));
				parameterList.Add(new SqlParameter("@part_desc", string.Empty));
				parameterList.Add(new SqlParameter("@order_status", string.Empty));
				//parameterList.Add(new SqlParameter("@cust_po_date_start", string.Empty));
				//parameterList.Add(new SqlParameter("@cust_po_date_end", string.Empty));

				SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTORDER_HEADER>("dbo.spGetCustOrderHeader @ref_doc_no, @cust_group_no, @cust_no, @cust_po_no, @store_no, @invoice_type, @wo_number, @seg_number, @model_type, @model, @serial, @equip_number, @shp_type, @doc_date_start, @doc_date_end, @filter_type, @hub_id, @area_id, @part_number, @part_desc, @order_status, @cust_po_date_start, @cust_po_date_end", parameters).FirstOrDefault();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTORDER_HEADER>("dbo.spGetCustOrderHeader_New @ref_doc_no, @cust_group_no, @cust_no, @cust_po_no, @store_no, @invoice_type, @wo_number, @seg_number, @model_type, @model, @serial, @equip_number, @shp_type, @doc_date_start, @doc_date_end, @filter_type, @hub_id, @area_id, @part_number, @part_desc, @order_status", parameters).FirstOrDefault();
                return data;
			}
		}

        public static List<Data.Domain.V_CUSTODER_DETAIL> GetDetailList(string ref_doc_no)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_doc_no", ref_doc_no.Trim()));
                SqlParameter[] parameters = parameterList.ToArray();

                //var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTODER_DETAIL>("dbo.spGetCustOrderDetail @ref_doc_no", parameters).ToList();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_CUSTODER_DETAIL>("dbo.spGetCustOrderDetail_New @ref_doc_no", parameters).ToList();
                if (data.Count == 1)
                    return data;

                var grp = data
                .GroupBy(g => new { g.part_no, g.source, g.line_item_no })
                .Select(g => new
                {
                    xpart_no = g.Key.part_no,
                    xsource = g.Key.source,
                    xLNNO = g.Key.line_item_no,
                    xid = g.Max(m => m.id)
                }).ToList();

                var list = (from c in data
                            from d in grp.Where(w => w.xpart_no == c.part_no && w.xsource == c.source && w.xid == c.id && w.xLNNO == c.line_item_no)
                            select c)
                                        .OrderBy(o => o.line_item_no).ToList();

                return list;
            }
        }
		public static List<Data.Domain.Store> GetStore(string filter_type, int? id)
		{
			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => new { HubID = e.HubID, AreaID = e.AreaID, StoreNo = e.StoreNo, Plant = e.Plant, Name = e.Plant + " - " + e.Name });

				if(filter_type == "HUB" && id.HasValue)
				{
					tb = tb.Where(o => o.HubID == id);
				}
				else if(filter_type == "AREA" && id.HasValue)
				{
					tb = tb.Where(o => o.AreaID == id);
				}

				var data = tb.ToList().Select(e => new Data.Domain.Store() { Plant = e.Plant, Name = e.Name });
				return data.ToList();
			}

		}

	}
}
