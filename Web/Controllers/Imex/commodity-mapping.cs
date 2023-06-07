using App.Framework.Mvc;
using App.Web.App_Start;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Helper;

namespace App.Web.Controllers.Imex
{
    public partial class ImexController
	{

		[Route("CommodityMapping")]
        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult commodityMapping()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			var model = new CommodityMappingView();
			this.PaginatorBoot.Remove("SessionTRN");
			return View("CommodityMapping", model);
		}

		#region paging
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
		public ActionResult commodityMappingPage()
		{
			this.PaginatorBoot.Remove("SessionTRN");
			return commodityMappingPageXt();
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
		public ActionResult commodityMappingPageXt()
		{
			Func<CommodityMappingView, List<Data.Domain.CommodityMapping>> func = delegate(CommodityMappingView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<CommodityMappingView>(param);
				}

				var tbl = Service.Imex.CommodityMapping.GetList().Where(w => w.Status == crit.Status);

				if(!string.IsNullOrEmpty(crit.HSDescription))
					tbl = tbl.Where(w => (w.HSCode.ToString() + " | " + w.HSDescription).ToLower().Contains(crit.HSDescription.ToLower())).ToList();

				if(!string.IsNullOrEmpty(crit.CommodityName))
					tbl = tbl.Where(w => w.CommodityCap.ToLower().Contains(crit.CommodityName.ToLower())).ToList();



				if(crit.selHSCodeList_Ids != null)
					tbl = tbl.Where(w => crit.selHSCodeList_Ids.Any(a => a == w.HSId.ToString())).ToList();


				return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.CommodityCode).ToList();
			};

			var paging = PaginatorBoot.Manage<CommodityMappingView, Data.Domain.CommodityMapping>("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}


		#endregion

		#region crud
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
		[HttpGet]
		public ActionResult CommodityMappingAdd()
		{
			var model = new Data.Domain.CommodityMapping(); //CommodityMappingView();
			model.Status = byte.Parse("1");
			ViewBag.crudMode = "I";
			return PartialView("commodity-mapping.iud", model);
		}

		[HttpPost]
		public async Task<ActionResult> CommodityMappingAdd(Data.Domain.CommodityMapping item)
		{
			ViewBag.crudMode = "I";
			if(ModelState.IsValid)
			{
				if(item.CommodityID == 0)
				{
					return Json(new JsonObject { Status = 1, Msg = "commodity  required ..!" });
				}
				if(item.HSId == 0)
				{
					return Json(new JsonObject { Status = 1, Msg = "HsCode required ..!" });
				}

				if(Service.Imex.CommodityMapping.GetList()
				.Where(w => w.CommodityID == item.CommodityID && w.HSId == item.HSId).Count() > 0)
				{
					return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
				}

				var ret = await App.Service.Imex.CommodityMapping.Update(item, "I");
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}


		[HttpGet]
		public ActionResult CommodityMappingEdit(int id)
		{
			ViewBag.crudMode = "U";
			try
			{
				//var model = new CommodityMappingView();
				var model = Service.Imex.CommodityMapping.GetId(id);

				if(model == null)
				{
					return HttpNotFound();
				}

				return PartialView("commodity-mapping.iud", model);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> CommodityMappingEdit(Data.Domain.CommodityMapping item)
		{
			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				if(item.CommodityID == 0)
				{
					return Json(new JsonObject { Status = 1, Msg = "commodity required ..!" });
				}

				if(Service.Imex.CommodityMapping.GetList()
				.Where(w => w.MappingID != item.MappingID
				&& w.CommodityID == item.CommodityID && w.HSId == item.HSId).Count() > 0)
				{
					return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
				}

				await App.Service.Imex.CommodityMapping.Update(item, "U");
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}

		[HttpGet]
		public ActionResult CommodityMappingView(int id)
		{
			ViewBag.crudMode = "V";
			try
			{
				var model = new CommodityMappingView();
				model.CommodityMapping = Service.Imex.CommodityMapping.GetId(id);
				if(model.CommodityMapping == null)
				{
					return HttpNotFound();
				}

				return PartialView("commodity-mapping.view", model);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

		#endregion


		[HttpPost]

		public ActionResult CommodityMappingUploadExcel()
		{
			string msg = "";
			string fileName = "";

			var excel = new Framework.Infrastructure.Documents();
			var ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

			if(ret == false)
			{
				return Json(new { Status = 1, Msg = msg });
			}

			var _item = new Data.Domain.CommodityMapping();
			var _list = new List<Data.Domain.CommodityMapping>();
			string commodityCode = "", hSCode = "";

			try
			{
				var excelTable = excel.ReadExcelToTable(fileName, "Upload$");
				IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

				//Get all the values from datatble
				foreach(DataRow dr in query)
				{
					commodityCode = dr["Comodity Code"] + "";
					hSCode = dr["HS Code"] + "";

					_item = new Data.Domain.CommodityMapping();
					_item.CommodityCode = commodityCode;
					_item.CommodityCode = Common.Sanitize(_item.CommodityCode);
					_item.HSCode = hSCode;
					_item.HSCode = Common.Sanitize(_item.HSCode);
					_list.Add(_item);
				}
			}
			catch(Exception ex)
			{
				return Json(new { Status = 1, Msg = ex.Message + "; commodity:" + commodityCode + "; hSCode:" + hSCode });
			}


			var _msgrec = "";
			if(_list.Count > 0)
			{
				var sb = new System.Text.StringBuilder();

				var hsList = Service.Master.HSCodeLists.GetList()
					.GroupBy(g => g.HSCode)
					.Select(s => new { HSCode = s.Key, HSID = s.Max(m => m.HSID), Description = s.Max(m => m.Description) })
					.AsParallel().ToList();

				var listHS = _list.Where(p => !hsList.Select(s => s.HSCode).Contains(p.HSCode))
				.GroupBy(g => g.HSCode).Select(s => new { hsCode = s.Key }).OrderBy(o => o.hsCode).ToList();

				if(listHS.Count() > 0)
				{
					sb.Append("<h3 class='table2excel'>HSCode not exists in HS List</h3>");
					sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
					sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; HS Code&nbsp;&nbsp; </th></tr>");
					int i = 0;
					foreach(var e in listHS)
					{
						i++;
						sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.hsCode + "</td></tr>");
					}
					sb.Append("</table>");
				}

				if(!string.IsNullOrEmpty(sb + ""))
				{
					return Json(new { Status = 1, Msg = "HS not exists in Master HsList", Data = sb.ToString() });
				}


				var listDup = _list
					.Where(p => Service.Imex.CommodityMapping.GetList().Select(s => s.CommodityCode.ToLower() + "|" + s.HSCode.ToString()).Contains(p.CommodityCode.ToLower() + "|" + p.HSCode.ToString()))
					.AsParallel().ToList();

				if(listDup.Count() > 0)
				{
					sb.Clear();
					sb.Append("<h3>Commodity Mapping already exists</h3>");
					sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:35%'>");
					sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Commodity Code&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;HS Code&nbsp;&nbsp;</th></tr>");
					int i = 0;
					foreach(var e in listDup)
					{
						i++;
						sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.CommodityCode + "</td><td>" + e.HSCode + "</td></tr>");
					}
					sb.Append("</table>");
					return Json(new { Status = 1, Msg = "Commodity Mapping already exists : " + i.ToString(), Data = sb.ToString() });
				}

				var list = (from c in _list.AsParallel()
										from h in hsList.Where(w => w.HSCode == c.HSCode).AsParallel()
										from p in Service.Master.CommodityImex.GetList().Where(w => w.CommodityCode.ToLower() == c.CommodityCode.ToLower()).AsParallel()
										select new Data.Domain.CommodityMapping()
										{
											MappingID = 0,
											CommodityID = p.ID,
											HSId = h.HSID,
											HSCode = c.HSCode,
											Status = 1
										}).ToList();


				string strRollback = "; rollback";
				var _tot = list.Count();
				_msgrec = " (" + _tot.ToString() + " of " + _list.Count().ToString() + ") ";

				if(_tot == _list.Count())
				{

					try
					{
						int pageSize = 500, totPage = (list.Count() / pageSize) + 1;
						var lstMap = new List<Data.Domain.CommodityMapping>();

						for(int i = 1; i <= totPage; i++)
						{
							int offset = pageSize * (i - 1);
							lstMap = list.AsParallel().ToList();

							lstMap = lstMap
										.Skip(offset)
										.Take(pageSize)
										.ToList();

							Service.Imex.CommodityMapping.UpdateBulk(lstMap, "I");
						}
					}
					catch(Exception ex)
					{
						return Json(new { Status = 1, Msg = ex.Message });
					}

					strRollback = "";
					return Json(new { Status = 0, Msg = msg + _msgrec });
				}

				return Json(new { Status = 1, Msg = "Uploaded " + _msgrec + strRollback });
			}
			else
			{
				msg = "Upload succesful, but no record to be process ..!";
				return Json(new { Status = 1, Msg = msg });
			}
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
		public JsonResult DownloadCommodityMappingToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadCommodityMaping data = new Helper.Service.DownloadCommodityMaping();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}