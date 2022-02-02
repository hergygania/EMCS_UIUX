using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{

		private string _sessionSurveyPart = "SurveyPart";
		private string _sessionSurveyPartDetail = "SurveyPartDetail";
		private string _sessionSurveyDocument = "SurveyDocument";

        #region Survey List paging
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public ActionResult SurveyList()
		{
			PaginatorBoot.Remove("SessionSurveyVrNo");
			return SurveyListXt();
		}
		public ActionResult SurveyListXt()
		{
			Func<SurveyView, List<Data.Domain.Survey>> func = delegate(SurveyView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<SurveyView>(param);
				}
				int freightId = (crit.Freight + "").ToLower() == "air" ? 2 : 1;
				var list = Service.Vetting.Survey.GetList(freightId, crit.Id, crit.VRNo, crit.VONo, crit.DateSta, crit.DateFin);

				if(crit.mode == "verify")
					list = list.Where(w => w.RFIDate.HasValue == false).ToList();
				else if(crit.mode == "rfi")
					list = list.Where(w => w.VONo != null && w.SurveyDone.HasValue==false).ToList();
				else if(crit.mode == "els")
					list = list.Where(w => w.SurveyDone.HasValue).ToList();

				return list.OrderByDescending(o => o.ModifiedDate).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionSurveyVrNo", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion


		#region Survey Parts paging
		public ActionResult SurveyPartPage()
		{
			PaginatorBoot.Remove("SessionSurveyPart");
			return SurveyPartPageXt();
		}
		public ActionResult SurveyPartPageXt()
		{
			Func<ShipmentView, List<Data.Domain.SurveyDetail>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;
				if(list == null)
				{
					list = new List<Data.Domain.SurveyDetail>();
					Session[_sessionSurveyPartDetail] = list;
				}

				return list.Where(w => w.dml != "D").ToList(); //.OrderBy(o => o.).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionSurveyPart", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion


		#region Survey document paging
		public ActionResult SurveyDocumentPage()
		{
			PaginatorBoot.Remove("SessionTRNDocument");
			return SurveyDocumentPageXt();
		}
		public ActionResult SurveyDocumentPageXt()
		{
			Func<ShipmentView, List<Data.Domain.SurveyDocument>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Session[_sessionSurveyDocument] as List<Data.Domain.SurveyDocument>;
				if(list == null)
				{
					list = new List<Data.Domain.SurveyDocument>();
					Session[_sessionSurveyDocument] = list;
				}

				return list.Where(w => w.dml != "D").OrderBy(o => o.DocumentName).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDocument", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
        #endregion
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu= "air-freight")]

        public PartialViewResult SurveyKso()
		{
			var freight = Request["freight"];
			int freightId = (freight + "").ToLower() == "air" ? 2 : 1;

			var model = new Data.Domain.Survey();
			model.SurveyID = -1;
			model.Status = 1;
			model.Freight = Convert.ToByte(freightId);
			model.VRDate = DateTime.Now;
			model.EntryDate = DateTime.Now;
			model.ModifiedDate = DateTime.Now;
			model.EntryBy = "User";
			model.ModifiedBy = "User";
			model.VettingRoute = 2;
			ViewBag.crudMode = "I";
			ViewBag.formMode = "create";

			Session.Remove(_sessionSurveyPart);
			Session.Remove(_sessionSurveyPartDetail);
			Session.Remove(_sessionSurveyDocument);

			var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == "createkso").ToList();
			if(emailList.Count() > 0)
			{
				model.EmailKso = emailList[0].EmailAddress;
				model.Email = emailList[0].EmailAddress;
				//var myStr = Service.Master.EmailRecipients.GetList().Where(w=>w.Purpose.ToLower()=="createkso").FirstOrDefault().EmailAddress;
				//var x= myStr.Split(',').Select(i => i).ToList();
			}

			return PartialView("Survey.iud", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public PartialViewResult SurveyVerify()
		{
			var freight = Request["freight"];
			int freightId = (freight + "").ToLower() == "air" ? 2 : 1;

			var model = new SurveyView();
			model.SurveyID = -1;
			model.VettingRoute = 2;
			ViewBag.formMode = "verify";

			Session.Remove(_sessionSurveyPart);
			Session.Remove(_sessionSurveyPartDetail);
			Session.Remove(_sessionSurveyDocument);

			return PartialView("Survey", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public PartialViewResult SurveyRfi()
		{
			var freight = Request["freight"];
			int freightId = (freight + "").ToLower() == "air" ? 2 : 1;

			var model = new SurveyView();
			model.SurveyID = -1;
			model.VettingRoute = 2;
			ViewBag.formMode = "rfi";

			Session.Remove(_sessionSurveyPart);
			Session.Remove(_sessionSurveyPartDetail);
			Session.Remove(_sessionSurveyDocument);

			return PartialView("Survey", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public PartialViewResult SurveyELS()
		{
			var freight = Request["freight"];
			int freightId = (freight + "").ToLower() == "air" ? 2 : 1;

			var model = new SurveyView();
			model.SurveyID = -1;
			model.VettingRoute = 2;
			ViewBag.formMode = "els";

			Session.Remove(_sessionSurveyPart);
			Session.Remove(_sessionSurveyPartDetail);
			Session.Remove(_sessionSurveyDocument);

			return PartialView("Survey", model);
		}

		#region survey crud

		[HttpPost]
		public async Task<ActionResult> SurveyAdd(Data.Domain.Survey item)
		{
			ViewBag.crudMode = "I";
			var msg = "";
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

			var surveyDetail = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;
			if(surveyDetail == null || surveyDetail.Count() == 0)
			{
				return Json(new { success = false, Msg = "No detail survey to be update.." });
			}

			var attachment = Session[_sessionSurveyDocument] as List<Data.Domain.SurveyDocument>;
			if(attachment == null || attachment.Count() == 0)
			{
				//return Json(new { success = false, Msg = "No document/attachment to be update.." });
				attachment = new List<Data.Domain.SurveyDocument>();
			}
			attachment.RemoveAll(w => (w.dml + "").ToLower() == "(duplicate)");

			if(!string.IsNullOrEmpty(item.VRNo) && Service.Vetting.Survey.GetList(item.Freight, item.SurveyID,item.VRNo, "", null, null).Count() > 0)
			{
				return Json(new { success = false, Msg = "VR Number " + item.VRNo + " already exists..." });
			}

			if(ModelState.IsValid)
			{
				await App.Service.Vetting.Survey.Update(item, surveyDetail, attachment, "I");

				//send email
				if(!string.IsNullOrEmpty(item.Email))
				{
					var subject = "Survey Created, VR No: " + item.VRNo;
					var sendemail = await SurveyEmail(item, surveyDetail, attachment, subject);
				}

				return JsonCRUDMessage(ViewBag.crudMode, item.SurveyID); //item.VRNo);
			}
			else
			{
				msg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = msg });
			}
		}


		[HttpGet]
		public ActionResult SurveyEdit(int id, string formMode)
		{
			ViewBag.crudMode = "U";
			ViewBag.formMode = string.IsNullOrEmpty(formMode) ? "rfi" : formMode;
			try
			{
				var model = Service.Vetting.Survey.GetId(id);
				model.VettingRoute = 2;
				Session.Remove(_sessionSurveyPart);
				Session[_sessionSurveyPartDetail] = Service.Vetting.SurveyDetail.GetList(id);
				Session[_sessionSurveyDocument] = Service.Vetting.SurveyDocument.GetList(id);

				string emailPurpose = formMode;
				if((""+formMode).ToLower() == "verify")
				{
					emailPurpose = string.IsNullOrEmpty(model.VONo) || !model.RFIDate.HasValue ? "vo" : "rfi";
				}

				var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == emailPurpose).ToList();
				if(emailList.Count() > 0)
				{
					if(emailPurpose == "vo")
						model.EmailVO = emailList[0].EmailAddress;
					else
						model.EmailRFI = emailList[0].EmailAddress;

					model.Email = emailList[0].EmailAddress;
				}
				else
				{
					model.Email = null;
				}

				return PartialView("survey.iud", model);
			}
			catch(Exception e)
			{
				return Json(new { success = false, Msg = e.InnerException.Message });
			}
		}

		[HttpPost]
		public async Task<ActionResult> SurveyEdit(Data.Domain.Survey item)
		{
			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				string email = "";
				if(!string.IsNullOrEmpty(item.Email))
				{
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
				}

				var surveyDetail = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;
				var attachment = Session[_sessionSurveyDocument] as List<Data.Domain.SurveyDocument>;
				attachment.RemoveAll(w => (w.dml + "").ToLower() == "(duplicate)");

				if(surveyDetail == null || surveyDetail.Count() == 0)
				{
					return Json(new { success = false, Msg = "No detail survey to be update.." });
				}

				await App.Service.Vetting.Survey.Update(item, surveyDetail, attachment, "U");

				//send email
				if(!string.IsNullOrEmpty(item.Email))
				{
					var subject = "Survey Done, VO No: " + item.VONo;
					if(!item.RFIDate.HasValue)
						subject = "Survey Verification, VO No: " + item.VONo;

					var sendemail = await SurveyEmail(item, surveyDetail, attachment, subject);
				}

				return JsonCRUDMessage(ViewBag.crudMode, (item.SurveyID+ " VrNo:"+ item.VRNo));
			}
			else
			{
				var nsg = Helper.Error.ModelStateErrors(ModelState);
				return Json(new { success = false, Msg = nsg });
			}
		}

		private async Task<int> SurveyEmail(Data.Domain.Survey item,
		List<Data.Domain.SurveyDetail> surveyDetail,
		List<Data.Domain.SurveyDocument> attachment,
		string subject)
		{
			var emailTo = item.Email;
			var emailCC = "";
			var strUrlAddress = Request.Url.Scheme + "://" + Request.Url.Authority.ToString() + "/";

			string tbl = "<style>table,th,td{border:1px solid black;border-collapse:collapse;} th,td{padding:5px;}</style>";
			tbl = tbl + "<table style='width:99%'><tr>" +
				 "<th>Parts Number</th>" +
				 "<th style='white-space:nowrap'>Parts Description</th>" +
				 "<th style='text-align:right'>Qty</th>" +
				 "<th>HS Code</th>" +
				 "<th style='text-align:right'>Nett WT</th>" +
				 "<th>Invoice No</th>" +
				 "<th style='white-space:nowrap;text-align:right'>Invoice Date</th>" +
				 "</tr>";
			foreach(var f in surveyDetail)
			{
				tbl = tbl + "<tr>" +
					 "<td>" + f.PartsNumber + "</td>" +
					 "<td>" + f.PartsName + "</td>" +
					 "<td style='text-align:right'>" + f.InvoiceItemQty + "</td>" +
					 "<td>" + f.HSCode + "</td>" +
					 "<td style='text-align:right'>" + f.PartGrossWeight + "</td>" +
					 "<td>" + f.InvoiceNo + "</td>" +
					 "<td style='text-align:right'>" + f.InvoiceDate.ToString("dd MMM yyyy") + "</td>" +
					 "</tr>";
			}
			tbl = tbl + "</table>";

			string emailBody = "Dear Sir,<br><br>" +
				"For your information, please be verified '" + (item.RFIDate.HasValue ? "RFI - " : "") + "Survey' below:<br/> " +
				"<br/>VR No : <b>" + item.VRNo + "</b>" +
				"<br/>VR Date : <b>" + item.VRDate.ToString("dd MMMM yyyy") + "</b>" +
				(!string.IsNullOrEmpty(item.VONo) ? "<br/>VO No : <b>" + item.VONo + "</b>" : "") +
				(item.RFIDate.HasValue ? "<br/>RFI Date : " + item.RFIDate.Value.ToString("dd MMMM yyyy") : "") +
				(item.SurveyDate.HasValue ? "<br/>Survey Date : " + item.SurveyDate.Value.ToString("dd MMMM yyyy") : "") +
				(item.SurveyDone.HasValue ? "<br/>Survey Done : " + item.SurveyDone.Value.ToString("dd MMMM yyyy") : "") +
				"<br/><br/>" +
				tbl +
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
		private static bool isValidEmail(string inputEmail)
		{
			string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
						@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
						@".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

			System.Text.RegularExpressions.Regex re = new Regex(strRegex);
			if(re.IsMatch(inputEmail))
				return (true);
			else
				return (false);
		}

		private string GetMimeType(string fileName)
		{
			string mimeType = "application/unknown";
			try
			{
				string ext = System.IO.Path.GetExtension(fileName).ToLower();
				Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
				if(regKey != null && regKey.GetValue("Content Type") != null)
					mimeType = regKey.GetValue("Content Type").ToString();
			}
			catch { }
			return mimeType;
		}

		[HttpPost]
		public ActionResult SurveyDelete(int id)
		{
			var list = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;

			if(list != null && list.Where(w => w.SurveyDetailID == id).Count() > 0)
			{
				list.RemoveAll(w => w.SurveyID == -1 && w.SurveyDetailID == id);

				list.ToList().ForEach((x) =>
				{
					x.dml = (x.SurveyDetailID == id ? "D" : x.dml);
				});
			}

			Session[_sessionSurveyPartDetail] = list.ToList();

			return Json(new { success = true, Msg = "" });
		}

		#endregion



		[HttpPost]
		public ActionResult SurveyGetParts(string arrObject)
		{
			var _list = new List<Data.Domain.PartsOrderDetail>();
			var param = arrObject;
			if(!string.IsNullOrEmpty(param))
			{
				JavaScriptSerializer ser = new JavaScriptSerializer();
				_list = ser.Deserialize<List<Data.Domain.PartsOrderDetail>>(param);
			}

			var partsList = Service.Vetting.PartsOrderDetail.GetFilterList(_list);
			var list = (from c in partsList
									select new Data.Domain.SurveyDetail
									{
										SurveyID = -1,
										SurveyDetailID = Convert.ToInt32(c.DetailID),
										PartsOrderDetailID = Convert.ToInt32(c.DetailID),
										InvoiceNo = c.InvoiceNo,
										InvoiceDate = c.InvoiceDate,
										PrimPSO = c.PrimPSO,
										PartsNumber = c.PartsNumber,
										InvoiceItemQty = c.InvoiceItemQty,
										PartGrossWeight = c.PartGrossWeight,
										EntryDate = c.EntryDate,
										ModifiedDate = c.ModifiedDate,
										EntryBy = c.EntryBy,
										ModifiedBy = c.ModifiedBy,
										EntryDate_Date = c.EntryDate_Date,
										HSCode = c.HSCode,
										HSDescription = c.HSDescription,
										PartsName = c.PartsName,
										OMCode = c.OMCode,
                                        CaseNo = c.CaseNo
									}).ToList();


			var partDetail = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;
			if(partDetail == null)
			{
				partDetail = new List<Data.Domain.SurveyDetail>();
			}

			foreach(var e in list)
			{
				if(partDetail.Where(w => w.SurveyDetailID == e.SurveyDetailID).Count() == 0)
					partDetail.Add(e);
			}

			Session[_sessionSurveyPartDetail] = partDetail.ToList();

			return Json(new { success = true, Msg = "" });
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult SurveyXcel(int surveyId)
		{
			var model = Service.Vetting.Survey.GetId(surveyId);
			if(model == null)
			{
				var comid = Convert.ToInt32(Request["CommodityID"]);
				model = new Data.Domain.Survey();
				model.VRNo = Request["VRNo"];
				model.VRDate = Convert.ToDateTime(Request["VRDate"]);
				model.CommodityCode = Service.Master.CommodityImex.GetId(comid).CommodityCode;
				surveyId = Service.Vetting.Survey.GetMax();
			}
			model.VettingRoute = 2;

			var listCurrent = Session[_sessionSurveyPartDetail] as List<Data.Domain.SurveyDetail>;
			if(listCurrent == null)
				listCurrent = Service.Vetting.SurveyDetail.GetList(surveyId);

			ViewBag.Title = "Upload_VR"+ (model.VRNo+"")  + "_" + model.CommodityCode +"_"+surveyId;
			var sb = new System.Text.StringBuilder();
			//sb.Append("<b>Survey VR:"+ (model.VRNo+"")  + " [" + model.CommodityCode +"] Batch:"+surveyId + "</b>");
			sb.Append("<table cellspacing='3'>");
			sb.Append("<tr><td></td><td>Survey VR</td><td>: <b>"+ (model.VRNo+"")  + " [" + model.CommodityCode +"] Batch:"+surveyId + "</b>" + "</td></tr>");
			sb.Append("<tr><td></td><td>VR Date</td><td>: " + model.VRDate.ToString("dd MMM yyyy") + "</td></tr>");
			if(model.RFIDate.HasValue)
				sb.Append("<tr><td></td><td>RFI Date</td><td>: " + model.RFIDate.Value.ToString("dd MMM yyyy") + "</td></tr>");
			if(model.SurveyDate.HasValue)
				sb.Append("<tr><td></td><td>Survey Date</td><td>: " + model.SurveyDate.Value.ToString("dd MMM yyyy") + "</td></tr>");
			if(model.SurveyDone.HasValue)
				sb.Append("<tr><td></td><td>Survey Done</td><td>: " + model.SurveyDone.Value.ToString("dd MMM yyyy") + "</td></tr>");
			sb.Append("</table>");

			if(listCurrent != null)
			{
				var no = 0;
				decimal totWeight = 0;

				sb.Append("<table border=1 cellspacing='3'>");
				sb.Append("<tr>" +
					"<td>No</td>" +
					"<td>HS Code</td>" +
					"<td>Parts Number</td>" +
					"<td>Parts Name</td>" +
					"<td>Country of Origin</td>" +
					"<td style='align:right'>Qty</td>" +
					"<td style='align:center'>UoM</td>" +
					"<td style='align:right'>Net Weight</td>" +
					"<td style='align:right'>Total Weight</td>" +
					"<td style='align:center'>UoM</td>" +
					"<td>Preference Facility</td>" +
					"<td> NO. IP / IT / SPI</td>" +
					"</tr>");


				foreach(var e in listCurrent.ToList())
				{

					no++;
					totWeight = e.InvoiceItemQty.HasValue && e.PartGrossWeight.HasValue ? e.InvoiceItemQty.Value * e.PartGrossWeight.Value : 0;

					sb.Append("<tr>" +
					"<td>" + no + "</td>" +
					"<td>" + e.HSCode + "&nbsp;</td>" +
					"<td>" + e.PartsNumber + "&nbsp;</td>" +
					"<td>" + e.PartsName + "&nbsp;</td>" +
					"<td>" + e.COODescription + "&nbsp;</td>" +
					"<td style='align:right'>" + e.InvoiceItemQty + "</td>" +
					"<td style='align:center'>PCE</td>" +
					"<td style='align:right'>" + e.PartGrossWeight + "</td>" +
					"<td style='align:right'>" + totWeight + "</td>" +
					"<td style='align:center'>KGM</td>" +
					"<td>-</td>" +
					"<td>-</td>" +
					"</tr>");
				}
				sb.Append("</table>");
			}
			return View("excel", sb);
		}



	}
}