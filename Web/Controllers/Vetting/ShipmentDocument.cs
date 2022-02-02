using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{


		#region shipment document crud
		[HttpGet]
		public ActionResult ShipmentDocumentAdd()
		{
			int random = Convert.ToInt32(DateTime.Now.ToString("hhmmssfff"));
			var model = new Data.Domain.ShipmentDocument();
			model.ShipmentID = -1;
			model.ShipmentDocumentID = random;
			model.EntryDate = DateTime.Now;
			model.ModifiedDate = DateTime.Now;
			model.EntryBy = "user";
			model.ModifiedBy = "user";
			ViewBag.crudMode = "I";
			return PartialView("ShipmentManifest.iud.doc", model);
		}


		[HttpPost]
		public ActionResult ShipmentDocumentUpload(Data.Domain.ShipmentDocument item)
		{
			string msg = "";
			string pathOrig = Server.MapPath(ConfigurationManager.AppSettings["ImagesData.document"] + "/" + item.DocumentTypeID.ToString());
			string filePath = pathOrig;
			string fileName = Request.Files[0].FileName;
			string dml="I";

			var attachment = Session[_sessionShipmentDocument] as List<Data.Domain.ShipmentDocument>;
			if(attachment == null || attachment.Count() == 0)
			{
				attachment = new List<Data.Domain.ShipmentDocument>();
			}

			if(attachment.Where(w => w.FileName.ToLower() == fileName.ToLower()).Count() > 0)
			{
				dml = "(duplicate)";
			}

			if(dml == "I") { 
				var excel = new Framework.Infrastructure.Documents();
				var ret = excel.Upload(Request.Files[0], ref filePath, ref msg, true);
			}

			string docDesc = Service.Master.DocumentTypes.GetId(item.DocumentTypeID).DocumentName;
			int random = Convert.ToInt32(DateTime.Now.ToString("hhmmssfff"));

			attachment.Add(new Data.Domain.ShipmentDocument
			{
				ShipmentDocumentID = random,
				ShipmentID = -1, //item.ShipmentID,
				DocumentTypeID = item.DocumentTypeID,
				DocumentName = docDesc,
				FilePath = pathOrig,
				FileName = fileName,
				dml = dml
			});

			Session[_sessionShipmentDocument] = attachment;

			return Json(new { success = true });
		}

		[HttpPost]
		public ActionResult ShipmentDocumentDelete(int id)
		{
			var list = Session[_sessionShipmentDocument] as List<Data.Domain.ShipmentDocument>;

			if(list != null && list.Where(w => w.ShipmentDocumentID == id).Count() > 0)
			{
				list.RemoveAll(w => w.dml =="I" &&  w.ShipmentDocumentID == id);

				list.ToList().ForEach((x) =>
				{
					x.dml = (x.ShipmentDocumentID == id ? "D" : x.dml);
				});

			}

			Session[_sessionShipmentDocument] = list.ToList();

			return Json(new { success = true, Msg = "" });
		}

		#endregion

	}
}