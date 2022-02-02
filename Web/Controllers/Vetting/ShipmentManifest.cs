using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{

		#region Manifest detail paging from memory
		public ActionResult ManifestDetailMemory()
		{
			PaginatorBoot.Remove("SessionTRNManifestDetail");
			return ManifestDetailMemoryXt();
		}

		public ActionResult ManifestDetailMemoryXt()
		{
			Func<ShipmentView, List<Data.Domain.ShipmentManifestDetail>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
				if(list == null)
				{
					list = new List<Data.Domain.ShipmentManifestDetail>();
					Session[_sessionShipmentManifestDetail] = list;
				}
				
				//var list = Session[_sessionPartsOrderCase] as List<Data.Domain.ShipmentManifestDetail>;
				//if(list == null)
				//{
				//	list = new List<Data.Domain.ShipmentManifestDetail>();
				//	Session[_sessionPartsOrderCase] = list;
				//}

				var listSum = list.Where(w => w.ShipmentManifestID == crit.ShipmentManifestID).ToList()
				.GroupBy(g => new { g.PartsOrderID })
				.Select(g => new Data.Domain.ShipmentManifestDetail
				{
					DetailID = g.Key.PartsOrderID,
					PartsOrderID = g.Key.PartsOrderID,
					ShipmentManifestID = crit.ShipmentManifestID,
					InvoiceNo = g.Max(m => m.InvoiceNo),
					InvoiceDate = g.Max(m => m.InvoiceDate),
					CaseNo = g.Max(m => m.CaseNo),
					CaseType = g.Max(m => m.CaseType),
					CaseID = g.Max(m => m.CaseID),
					CaseDescription = g.Max(m => m.CaseDescription),
					WeightKG = g.Max(m => m.WeightKG),
					WideCM = g.Max(m => m.WideCM),
					LengthCM = g.Max(m => m.LengthCM),
					HeightCM = g.Max(m => m.HeightCM),
					JCode = g.Max(m => m.JCode),
					AgreementType = g.Max(m => m.AgreementType),
					StoreNumber = g.Max(m => m.StoreNumber),
					DA = g.Max(m => m.DA),
					ModifiedBy = g.Max(m => m.ModifiedBy),
					ModifiedDate = g.Max(m => m.ModifiedDate),
					EndDestination = "",
					totPackage = g.Count()
				}).ToList();

				return listSum.OrderBy(o => o.InvoiceNo).ThenBy(o => o.CaseNo).ToList();
				//return list.OrderBy(o => o.InvoiceNo).ThenBy(o => o.CaseNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNManifestDetail", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region manifest crud
		[HttpGet]
		public ActionResult ManifestAdd()
		{
			int random = Convert.ToInt32(DateTime.Now.ToString("HHmmssfff"));

			var model = new Data.Domain.ShipmentManifest();
			model.ShipmentID = -1;
			model.ShipmentManifestID = random;
			model.EntryDate = DateTime.Now;
			model.ModifiedDate = DateTime.Now;
			model.EntryBy = "user";
			model.ModifiedBy = "user";
			ViewBag.crudMode = "I";
			return PartialView("ShipmentManifest.form", model); //return PartialView("ShipmentManifest.iud", model);
		}

		[HttpPost]
		public ActionResult ManifestAdd(Data.Domain.ShipmentManifest item)
		{
			ViewBag.crudMode = "I";
			string msg="";
			bool isSuccess=true;

			if(ModelState.IsValid)
			{
				isSuccess = ManifestSessionCrud(item, ViewBag.crudMode, ref msg);
				return Json(new { success = isSuccess, Status = (isSuccess ? 0 : 2), Msg = msg });
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Status = 1, Msg = nsg });
			}
		}

		[HttpGet]
		public ActionResult ManifestEdit(int id)
		{
			ViewBag.crudMode = "U";
			var model = Service.Vetting.ShipmentManifest.GetId(id);
			if(model == null)
			{
				var manifest = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
				if(manifest != null && manifest.Count() > 0)
				{
					model = manifest.Where(w => w.ShipmentManifestID == id).FirstOrDefault();
				}
			}

			var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(manifestDetail == null || manifestDetail.Count() == 0)
			{
				Session[_sessionShipmentManifestDetail] = Service.Vetting.ShipmentManifestDetail.GetList(id);
			}

			return PartialView("ShipmentManifest.form", model);//PartialView("ShipmentManifest.iud", model);
		}

		private bool ManifestSessionCrud(Data.Domain.ShipmentManifest item, string crud, ref string msg)
		{
			var ret = true;

			var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(manifestDetail == null || manifestDetail.Count() == 0)
			{
				manifestDetail = new List<Data.Domain.ShipmentManifestDetail>();
			}

			var _manifest = manifestDetail.Where(w => w.ShipmentManifestID == item.ShipmentManifestID).ToList();
			if(_manifest.Count() == 0)
			{
				msg = "No detail manifest to update..";
				return false;
			}

			manifestDetail.RemoveAll(w => w.ShipmentManifestID == item.ShipmentManifestID);

			foreach(var e in _manifest.Where(w => w.ShipmentManifestID == item.ShipmentManifestID))
			{
				if(manifestDetail.Where(w => w.ShipmentManifestID != item.ShipmentManifestID && w.PartsOrderID == e.PartsOrderID).Count() > 0)
				{
					msg = "Invoice/SX " + e.InvoiceNo + " already exist ..";
					return false;
				}
				else {
					e.ShipmentID = item.ShipmentID;
					manifestDetail.Add(e);
				}
			}
			
			var sumWt = manifestDetail.Sum(s=>s.WeightKG);
			if(sumWt > Domain.SiteConfiguration.Max_manifest_kg)
			{
				msg = "Overweight manifest, max 20 tons ..!";
				return false;
			}

			Session[_sessionShipmentManifestDetail] = manifestDetail;


			var manifest = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
			if(manifest == null || manifest.Count() == 0)
			{
				manifest = new List<Data.Domain.ShipmentManifest>();
			}

			if(crud == "I")
			{
				if(manifest.Where(w => w.ManifestNumber.ToLower() == item.ManifestNumber.ToLower()).Count() > 0)
				{
					msg = "Manifest number already exists...";
					return false;
				}

				manifest.Add(new Data.Domain.ShipmentManifest
				{
					ShipmentID = item.ShipmentID,
					ShipmentManifestID = item.ShipmentManifestID,
					ManifestNumber = item.ManifestNumber,
					ContainerNumber = item.ContainerNumber,
					EntryBy = item.EntryBy,
					EntryDate = item.EntryDate,
					ModifiedBy = item.ModifiedBy,
					ModifiedDate = item.ModifiedDate,
					totPackage = manifestDetail.Where(w => w.ShipmentManifestID == item.ShipmentManifestID).Count()
				});
			}
			else
			{
				manifest.RemoveAll(w => w.ShipmentManifestID == item.ShipmentManifestID);

				item.totPackage = manifestDetail.Where(w => w.ShipmentManifestID == item.ShipmentManifestID).Count();
				manifest.Add(item);
			}

			Session[_sessionShipmentManifest] = manifest;

			return ret;
		}

		[HttpPost]
		public ActionResult ManifestEdit(Data.Domain.ShipmentManifest item)
		{
			ViewBag.crudMode = "U";
			string msg = "";
			var isSuccess = true;

			if(ModelState.IsValid)
			{
				isSuccess = ManifestSessionCrud(item, ViewBag.crudMode, ref msg);
				return Json(new { success = isSuccess, Status = (isSuccess ? 0 : 1), Msg = msg });
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}


		[HttpPost]
		public ActionResult ManifestDelete(int id)
		{
			var list = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
			if(list != null && list.Where(w => w.ShipmentManifestID == id).Count() > 0)
			{
				list.ToList().ForEach((x) =>
				{
					x.dml = (x.ShipmentManifestID == id ? "D" : x.dml);
				});
			}
			Session[_sessionShipmentManifest] = list.ToList();

			var _detail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(_detail != null && _detail.Where(w => w.ShipmentManifestID == id).Count() > 0)
			{
				_detail.RemoveAll(w => w.ShipmentManifestID == id);
			}
			Session[_sessionShipmentManifestDetail] = _detail.ToList();


			return Json(new { success = true, Msg = "" });
		}

		#endregion


		[HttpPost]
		public ActionResult ManifestDetailDelete(int id)
		{
			var _detail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(_detail != null && _detail.Where(w => w.PartsOrderID == id).Count() > 0)
			{
				_detail.RemoveAll(w => w.PartsOrderID == id);
			}
			Session[_sessionShipmentManifestDetail] = _detail.ToList();

			return Json(new { success = true, Msg = "" });
		}


		[Route("detailmanifest-{id}-{poId}")]
		public ActionResult ManifestDetailView(int? id, int? poId)
		{
			var list = new List<Data.Domain.ShipmentManifestDetail>();
			var _detail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(_detail != null && _detail.Count() > 0)
			{
				if(poId.HasValue)
					list = _detail.Where(w => w.PartsOrderID == poId.Value).ToList();
				else if(id.HasValue)
					list = _detail.Where(w => w.ShipmentManifestID == id.Value).ToList();
			}


			return View("ShipmentManifest.view", list);
		}


		[HttpGet]
		public ActionResult ManifestGetSxModal()
		{
			int random = Convert.ToInt32(DateTime.Now.ToString("HHmmssfff"));

			var model = new Data.Domain.ShipmentManifest();
			return PartialView("PartsOrder.listModal", model);
		}


		[HttpPost]
		public ActionResult ManifestGetSx(string arrObject)
		{
			var _list = new List<Data.Domain.PartsOrder>();
			var param = arrObject;
			if(!string.IsNullOrEmpty(param))
			{
				JavaScriptSerializer ser = new JavaScriptSerializer();
				_list = ser.Deserialize<List<Data.Domain.PartsOrder>>(param);
			}

			var shipmentManifestId = Convert.ToInt32(Request["ShipmentManifestID"] + "");

			var manifest = Service.Vetting.PartsOrderCase.GetList(_list);
			var list = manifest
			.OrderByDescending(o => o.CaseID)
			.GroupBy(g => new { g.PartsOrderID, g.CaseNo, g.CaseType, g.InvoiceNo, g.InvoiceDate })
			.Select(g => new Data.Domain.ShipmentManifestDetail
				{
					DetailID = g.Max(m => m.CaseID),
					ShipmentManifestID = shipmentManifestId,
					PartsOrderID = g.Key.PartsOrderID.Value,
					InvoiceNo = g.Key.InvoiceNo,
					InvoiceDate = g.Key.InvoiceDate,
					CaseNo = g.Key.CaseNo,
					CaseType = g.Key.CaseType,
					CaseID = g.Max(m => m.CaseID),
					CaseDescription = g.Max(m => m.CaseDescription),
					WeightKG = g.Max(m => m.WeightKG),
					WideCM = g.Max(m => m.WideCM),
					LengthCM = g.Max(m => m.LengthCM),
					HeightCM = g.Max(m => m.HeightCM),
					JCode = g.Max(m => m.JCode),
					AgreementType = g.Max(m => m.AgreementType),
					StoreNumber = g.Max(m => m.StoreNumber),
					DA = g.Max(m => m.DA),
					EndDestination = "",
					ModifiedBy = g.Max(m => m.ModifiedBy),
					ModifiedDate = g.Max(m => m.ModifiedDate)
				}).ToList();


			var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(manifestDetail == null)
			{
				manifestDetail = new List<Data.Domain.ShipmentManifestDetail>();
			}

			foreach(var e in list)
			{
				if(manifestDetail.Where(w => w.CaseNo == e.CaseNo && w.InvoiceNo == e.InvoiceNo).Count() == 0)
					manifestDetail.Add(e);
			}

			Session[_sessionShipmentManifestDetail] = manifestDetail.ToList();
			return Json(new { success = true, Msg = "" });
		}

	}
}