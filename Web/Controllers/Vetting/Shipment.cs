using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{

		private string _sessionShipmentManifest = "ShipmentManifest";
		private string _sessionShipmentManifestDetail = "ShipmentManifestDetail";
		private string _sessionShipmentDocument = "ShipmentDocument";

		#region Shipment paging
		public ActionResult ShipmentPage()
		{
			Session.Remove(_sessionShipmentManifest);
			Session.Remove(_sessionShipmentManifestDetail);

			PaginatorBoot.Remove("SessionTRN");
			return ShipmentPageXt();
		}

		public ActionResult ShipmentPageXt()
		{
			Func<ShipmentView, List<Data.Domain.Shipment>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;

				var list = Service.Vetting.Shipment.GetList(freightId, crit.VettingRoute, crit.BLAWB, crit.Vessel, crit.LoadingPortDesc, crit.DestinationPortDesc,
				crit.ManifestNo, crit.ETD, crit.ETA);

				if(!string.IsNullOrEmpty(crit.LoadingPortDesc))
					list = list.Where(c => c.LoadingPortDesc.Trim().ToLower().Contains(crit.LoadingPortDesc.ToLower())).ToList();
				if(!string.IsNullOrEmpty(crit.DestinationPortDesc))
					list = list.Where(c => c.DestinationPortDesc.Trim().ToLower().Contains(crit.DestinationPortDesc.ToLower())).ToList();


				return list.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.BLAWB).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Shipment manifest paging
		public ActionResult ShipmentManifestPage()
		{
			PaginatorBoot.Remove("SessionTRNManifest");
			Session.Remove("TotalPackage");
			return ShipmentManifestPageXt();
		}
		public ActionResult ShipmentManifestPageXt()
		{
			Func<ShipmentView, List<Data.Domain.ShipmentManifest>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
				if(list == null)
				{
					list = new List<Data.Domain.ShipmentManifest>();
					Session[_sessionShipmentManifest] = list;
				}

				Session["TotalPackage"] = list.Where(w => w.dml != "D").Sum(s => s.totPackage);

				return list.Where(w => w.dml != "D").OrderBy(o => o.ManifestNumber).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNManifest", func).Pagination.ToJsonResult();

			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		public JsonResult ShipmentManifestTotal()
		{
			return JsonObject(new { TotalPackage = Session["TotalPackage"] });
		}

		#endregion

		#region Shipment document paging
		public ActionResult ShipmentDocumentPage()
		{
			PaginatorBoot.Remove("SessionTRNDocument");
			return ShipmentDocumentPageXt();
		}
		public ActionResult ShipmentDocumentPageXt()
		{
			Func<ShipmentView, List<Data.Domain.ShipmentDocument>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Session[_sessionShipmentDocument] as List<Data.Domain.ShipmentDocument>;
				if(list == null)
				{
					list = new List<Data.Domain.ShipmentDocument>();
					Session[_sessionShipmentDocument] = list;
				}

				return list.Where(w => w.dml != "D").OrderBy(o => o.DocumentName).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDocument", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region Shipment PartsOrder
		public ActionResult ShipmentPartsOrder()
		{
			PaginatorBoot.Remove("SessionTRNDetail");
			return ShipmentPartsOrderXt();
		}

		public ActionResult ShipmentPartsOrderXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrder>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}

				int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;

				crit.JCode = crit.selJCode == null ? null : string.Join("|", crit.selJCode.ToArray());
				crit.AgreementType = crit.selAgreementType == null ? null : string.Join("|", crit.selAgreementType.ToArray());

				var partList = Service.Vetting.PartsOrder.GetList(freightId, crit.FreightShippId, crit.VettingRoute, crit.ShipmentMode, crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.JCode, crit.AgreementType, crit.StoreNumber, crit.DANumber);
				var listSum =
				(
					from c in partList
					from si in Service.Master.ShippingInstruction.GetList().Where(w => w.ShippingInstructionID == c.ShippingInstructionID).DefaultIfEmpty()
					select new { c, shipnm = si == null ? "" : si.Description }
				)
				.GroupBy(g => new { g.c.InvoiceNo, g.c.InvoiceDate, g.c.JCode })
				.Select(g => new Data.Domain.PartsOrder
				{
					PartsOrderID = g.Max(m => m.c.PartsOrderID),
					InvoiceNo = g.Key.InvoiceNo,
					InvoiceDate = g.Key.InvoiceDate,
					JCode = g.Key.JCode,
					AgreementType = g.Max(m => m.c.AgreementType),
					StoreNumber = g.Max(m => m.c.StoreNumber),
					ShippingIDASN = g.Max(m => m.c.ShippingIDASN),
					ShippingInstruction = g.Max(m => m.shipnm),
					DA = g.Max(m => m.c.DA),
					TotalAmount = g.Max(m => m.c.TotalAmount),
					ModifiedBy = g.Max(m => m.c.ModifiedBy),
					ModifiedDate = g.Max(m => m.c.ModifiedDate)
				});

				var list = new List<Data.Domain.PartsOrder>();
				var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
				if(manifestDetail != null && manifestDetail.Count() > 0)
				{
					var manifest = manifestDetail.Select(s => new { s.PartsOrderID }).Distinct();

					list = (from c in listSum.ToList()
									from m in manifest.Where(w => w.PartsOrderID == c.PartsOrderID).DefaultIfEmpty()
									where m == null
									select c).ToList();
				}
				else
				{
					list = listSum.ToList();
				}

				return list.OrderByDescending(o => o.InvoiceNo).ThenBy(o => o.InvoiceNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDetail", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion


		#region shipment crud
		[HttpGet]
		public PartialViewResult ShipmentAddPage(string freight, byte vettingRoute)
		{
			var model = new Data.Domain.Shipment();
			model.ShipmentID = -1;
			model.Status = 1;
			model.VettingRoute = vettingRoute;
			model.ShippingInstructionID = (freight.ToLower() == "air" ? 2 : 1);
			model.IsSeaFreight = (freight.ToLower() == "air" ? false : true);
			model.ETD = DateTime.Today;
			model.ETA = DateTime.Today;
			model.EntryDate = DateTime.Now;
			model.ModifiedDate = DateTime.Now;
			model.EntryBy = "User";
			model.ModifiedBy = "User";
			ViewBag.crudMode = "I";

			Session.Remove(_sessionShipmentManifest);
			Session.Remove(_sessionShipmentManifestDetail);
			//Session.Remove(_sessionPartsOrderCase);
			Session.Remove(_sessionShipmentDocument);

			var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == "shipment").ToList();
			if(emailList.Count() > 0)
			{
				model.Email = emailList[0].EmailAddress;
			}

			return PartialView("Shipment.iud", model);
		}

		[HttpPost]
		public async Task<ActionResult> ShipmentAdd(Data.Domain.Shipment item)
		{
			ViewBag.crudMode = "I";
			var nsg = "";
			string email = "";
			var emailList = item.Email.Split(',').Select(i => i).ToList();
			foreach(var e in emailList)
			{
				if(!string.IsNullOrEmpty(e) && !isValidEmail(e.Trim()))
				{
					return Json(new { success = false, Msg = "invalid email address for: " + e });
				}
				email = email + e.Trim() + ",";
			}
			email = email.TrimEnd(',');

			var manifest = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
			if(manifest == null || manifest.Count() == 0)
			{
				return Json(new { success = false, Msg = "No manifest to be update.." });
			}

			var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
			if(manifestDetail == null || manifestDetail.Count() == 0)
			{
				return Json(new { success = false, Msg = "No detail manifest to be update.." });
			}

			var attachment = Session[_sessionShipmentDocument] as List<Data.Domain.ShipmentDocument>;
			if(attachment == null || attachment.Count() == 0)
			{
				return Json(new { success = false, Msg = "No document/attachment to be update.." });
			}
			attachment.RemoveAll(w => (w.dml + "").ToLower() == "(duplicate)");

			if(Service.Vetting.Shipment.GetList(item.ShippingInstructionID, item.VettingRoute, item.BLAWB, "", "", "", "", null, null).Count() > 0)
			{
				return Json(new { success = false, Msg = "BL / AWB  already exists..." });
			}

			if(ModelState.IsValid)
			{
				if(item.LoadingPortID == 0)
					return Json(new { success = false, Msg = "Loading Port required..." });
				if(item.DestinationPortID == 0)
					return Json(new { success = false, Msg = "Destination Port required..." });
				if(item.LoadingPortID == item.DestinationPortID)
					return Json(new { success = false, Msg = "Loading Port must not equal Destination Port..." });

				await App.Service.Vetting.Shipment.Update(item, manifest, manifestDetail, attachment, "I");

				//send email
				if(!string.IsNullOrEmpty(item.Email))
				{
					var subject = "Shipment, BL/AWB: " + item.BLAWB;
					var sendemail = await ShipmentEmail(item, manifestDetail, attachment, subject);
				}

				return JsonCRUDMessage(ViewBag.crudMode, item.BLAWB);
			}
			else
			{
				nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}

		[HttpGet]
		public ActionResult ShipmentEdit(int id)
		{
			ViewBag.crudMode = "U";
			try
			{
				var model = Service.Vetting.Shipment.GetId(id);
				if(model == null)
				{
					return HttpNotFound();
				}

				//Session.Remove(_sessionPartsOrderCase);
				Session[_sessionShipmentManifest] = Service.Vetting.ShipmentManifest.GetList(id);
				Session[_sessionShipmentDocument] = Service.Vetting.ShipmentDocument.GetList(id);
				Session[_sessionShipmentManifestDetail] = Service.Vetting.ShipmentManifestDetail.GetList(null, id);

				model.totPackage = Service.Vetting.ShipmentManifestDetail.GetTotalPackage(id);

				return PartialView("Shipment.iud", model);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

		[HttpPost]
		public async Task<ActionResult> ShipmentEdit(Data.Domain.Shipment item)
		{
			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				if(item.LoadingPortID == 0)
					return Json(new { success = false, Msg = "Loading Port required..." });
				if(item.DestinationPortID == 0)
					return Json(new { success = false, Msg = "Destination Port required..." });
				if(item.LoadingPortID == item.DestinationPortID)
					return Json(new { success = false, Msg = "Loading Port must not equal Destination Port..." });

				var manifest = Session[_sessionShipmentManifest] as List<Data.Domain.ShipmentManifest>;
				var manifestDetail = Session[_sessionShipmentManifestDetail] as List<Data.Domain.ShipmentManifestDetail>;
				var attachment = Session[_sessionShipmentDocument] as List<Data.Domain.ShipmentDocument>;
				attachment.RemoveAll(w => (w.dml + "").ToLower() == "(duplicate)");

				if(manifestDetail == null || manifestDetail.Count() == 0)
				{
					return Json(new { success = false, Msg = "No detail manifest to be update.." });
				}
				if(attachment == null || attachment.Count() == 0)
				{
					return Json(new { success = false, Msg = "No document/attachment to be update.." });
				}

				await App.Service.Vetting.Shipment.Update(item, manifest, manifestDetail, attachment, "U");
				return JsonCRUDMessage(ViewBag.crudMode, item.BLAWB);
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}

		private async Task<int> ShipmentEmail(Data.Domain.Shipment item,
		List<Data.Domain.ShipmentManifestDetail> dataDetail,
		List<Data.Domain.ShipmentDocument> attachment,
		string subject)
		{
			var emailTo = item.Email;
			var emailCC = "";
			var strUrlAddress = Request.Url.Scheme + "://" + Request.Url.Authority.ToString() + "/";
			decimal totwight = 0;

			string tbl = "<style>table,th,td{border:1px solid black;border-collapse:collapse;} th,td{padding:5px;}</style>";
			tbl = tbl + "<table style='width:99%'><tr>" +
				 "<th>CaseNo</th>" +
				 "<th>CaseDescription</th>" +
				 "<th>CaseType</th>" +
				 "<th style='text-align:right'>Length(cm)</th>" +
				 "<th style='text-align:right'>Weight(kg)</th>" +
				 "<th style='text-align:right'>Height(cm)</th>" +
				 "<th>Invoice No</th>" +
				 "<th style='white-space:nowrap;text-align:right'>Invoice Date</th>" +
				 "</tr>";
			foreach(var f in dataDetail)
			{
				tbl = tbl + "<tr>" +
					 "<td>" + f.CaseNo + "</td>" +
					 "<td>" + f.CaseDescription + "</td>" +
					 "<td>" + f.CaseType + "</td>" +
					 "<td style='text-align:right'>" + f.LengthCM + "</td>" +
					 "<td style='text-align:right'>" + f.WeightKG + "</td>" +
					 "<td style='text-align:right'>" + f.HeightCM + "</td>" +
					 "<td>" + f.InvoiceNo + "</td>" +
					 "<td style='text-align:right'>" + f.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
					 "</tr>";

				totwight = totwight + (f.WeightKG.HasValue ? f.WeightKG.Value : 0);

			}
			tbl = tbl + "</table>";

			string emailBody = "Dear Sir,<br><br>" +
				"For your information, please be verified 'shipment' below:<br/> " +
				"<br/>BL/AWB : <b>" + item.BLAWB + "</b>" +
				"<br/>Vessel/Voyage : <b>" + item.Vessel + "</b>" +
				"<br/>ETD Date : <b>" + item.ETD.ToString("dd MMMM yyyy") + "</b>" +
				"<br/>ETA Date : <b>" + item.ETA.ToString("dd MMMM yyyy") + "</b>" +
				(item.ATD.HasValue ? "<br/>ATD Date : " + item.ATD.Value.ToString("dd MMMM yyyy") : "") +
				(item.LoadingPortID > 0 ? "<br/>Loading Port : <b>" + Service.Master.FreightPort.GetId(item.LoadingPortID).PortNameCap + "</b>" : "") +
				(item.DestinationPortID > 0 ? "<br/>Destination Port : <b>" + Service.Master.FreightPort.GetId(item.DestinationPortID).PortNameCap + "</b>" : "") +
				"<br/><br/>" +
				tbl +
				(totwight > 20 ? "<br/>*)Check this weight" : "") +
				"<br/><br/><br/>For more detail, click here " + strUrlAddress + " enter to the application.<br/><br/>Has been submitted at <b>" + DateTime.Now.ToString("dd-MMM-yyyy HH:mm") + "</b> by: pis";

			var _email = new Data.Domain.Email
			{
				EmailDate = DateTime.Today,
				EmailFrom = "Pis",
				EmailFromAddress = strUrlAddress,
				EmailSubject = subject,
				EmailBody = emailBody,
				EmailTo = emailTo,
				Status = "OUTBOX"
			};

			var documents = new List<Data.Domain.EmailAttachment>();
			foreach(var e in attachment)
			{
				var fileName = e.FilePath + "\\" + e.FileName;

				byte[] imgDate = System.IO.File.ReadAllBytes(fileName);
				var type = GetMimeType(fileName);

				documents.Add(new Data.Domain.EmailAttachment
				{
					EmailID = -1,
					Name = e.DocumentName,
					FileName = fileName,
					Content = imgDate,
					ContentType = type
				});
			}

			try
			{
				await App.Service.Master.Emails.Update(_email, documents, "I");
			}
			catch { }

			try
			{
				Framework.Email.SendAsync(subject, emailTo, emailCC, emailBody);
			}
			catch { }

			return 1;
		}
        #endregion

        public JsonResult DownloadShipmentToExcel(string freight, int vettingRoute)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadShipment data = new Helper.Service.DownloadShipment();

            Session[guid.ToString()] = data.DownloadToExcel(freight, vettingRoute);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}