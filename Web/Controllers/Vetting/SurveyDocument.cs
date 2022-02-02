using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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


		#region Survey  document crud
		[HttpGet]
		public ActionResult SurveyAttachmentAdd()
		{
			int random = Convert.ToInt32(DateTime.Now.ToString("hhmmssfff"));
			var model = new Data.Domain.SurveyDocument();
			model.SurveyID = -1;
			model.SurveyDocumentID = random;
			model.EntryDate = DateTime.Now;
			model.ModifiedDate = DateTime.Now;
			model.EntryBy = "user";
			model.ModifiedBy = "user";
			ViewBag.crudMode = "I";
			return PartialView("Survey.iud.document", model);
		}


		[HttpPost]
		public ActionResult SurveyAttachmentUpload(Data.Domain.ShipmentDocument item)
		{
			string msg = "";
			string pathOrig = Server.MapPath(ConfigurationManager.AppSettings["ImagesData.document"] + "/" + item.DocumentTypeID.ToString());
			string filePath = pathOrig;
			string fileName = Request.Files[0].FileName;
			string dml="I";

            string[] validFileTypes = { "doc", "xls", "pdf", "zip" };
            System.IO.FileInfo fi = new System.IO.FileInfo(Request.Files[0].FileName);
            string ext = fi.Extension;
            bool isValidFile = false;
            for (int i = 0; i < validFileTypes.Length; i++)
            {
                if (ext == "." + validFileTypes[i])
                {
                    isValidFile = true;
                    break;
                }
            }

            if (!isValidFile)
            {
                return Json(new
                {
                    Status = 1,
                    Msg = "Upload File Failed " +//"Invalid File. Please upload a File with extension " +
                               string.Join(",", validFileTypes)
                });
            }

            if (ext.Length > 0 &&
!MIMETypesDictionary.ContainsKey(ext.Remove(0, 1)))
            {
                return Json(new
                {
                    Status = 1,
                    Msg = "Upload File Failed " +//"Invalid File. Please upload a File with extension " +
                                string.Join(",", validFileTypes)
                });
            }

            var attachment = Session[_sessionSurveyDocument] as List<Data.Domain.SurveyDocument>;
			if(attachment == null || attachment.Count() == 0)
			{
				attachment = new List<Data.Domain.SurveyDocument>();
			}

			if(attachment.Where(w => w.FileName.ToLower() == fileName.ToLower()).Count() > 0)
			{
				dml = "(duplicate)";
			}
            if (App.Web.Helper.Extensions.FileExtention.isSurveyDocumentFile(fileName))
            {
                if (dml == "I")
                {
                    var excel = new Framework.Infrastructure.Documents();
                    var ret = excel.Upload(Request.Files[0], ref filePath, ref msg, true);
                }
            }
			

			string docDesc = Service.Master.DocumentTypes.GetId(item.DocumentTypeID).DocumentName;
			int random = Convert.ToInt32(DateTime.Now.ToString("hhmmssfff"));

			attachment.Add(new Data.Domain.SurveyDocument
			{
				SurveyDocumentID = random,
				SurveyID = -1, //item.ShipmentID,
				DocumentTypeID = item.DocumentTypeID,
				DocumentName = docDesc,
				FilePath = pathOrig,
				FileName = fileName,
				dml = dml
			});

			Session[_sessionSurveyDocument] = attachment;

			return Json(new { success = true });
		}

        [HttpPost]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "vetting-process/sea-freight")]
        public ActionResult SurveyAttachmentDelete(int id)
		{
			var list = Session[_sessionSurveyDocument] as List<Data.Domain.SurveyDocument>;
            string a = User.Identity.Name;
			if(list != null && list.Where(w => w.SurveyDocumentID == id && w.EntryBy == User.Identity.Name).Count() > 0)
			{
				list.RemoveAll(w => w.dml == "I" && w.SurveyDocumentID == id);

				list.ToList().ForEach((x) =>
				{
					x.dml = (x.SurveyDocumentID == id ? "D" : x.dml);
				});

			}

			Session[_sessionSurveyDocument] = list.ToList();

			return Json(new { success = true, Msg = "" });
		}

        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            {"doc", "application/msword"},
            {"pdf", "application/pdf"},
            {"xls", "application/vnd.ms-excel"},
            {"zip", "application/zip"}
        };

        #endregion

    }
}