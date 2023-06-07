using System;
using System.Collections.Generic;
using System.Linq;
<<<<<<< HEAD
<<<<<<< HEAD
using System.Web.Mvc;
using App.Domain;
using App.Web.App_Start;
=======
using System.Web;
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
using System.Web.Mvc;
using App.Domain;
using App.Web.App_Start;
<<<<<<< HEAD
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterVideo InitilizeVideo(long videoId)
        {
            MasterVideo video = new MasterVideo();
            if (videoId == 0)
            {
                return video;
            }

            video = Service.EMCS.MasterVideo.GetId(videoId);
            return video;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterVideo()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterVideo")]
        public ActionResult VideoPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return VideoPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterVideo")]
        public ActionResult VideoPageXt()
        {
            Func<MasterSearchForm, IList<MasterVideo>> func = delegate (MasterSearchForm crit)
         {
             List<MasterVideo> list = Service.EMCS.MasterVideo.GetVideoList(crit);
             return list.OrderBy(o => o.UpdateDate).ToList();
         };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult PreviewVideo(long id)
        {
            var videoData = new VideoModel();
            videoData.Video = InitilizeVideo(id);
            

            if (videoData.Video == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.PreviewVideo", videoData);
        }

        [HttpGet]
        public ActionResult VideoCreate()
        {
            ViewBag.crudMode = "I";
            var videoData = new VideoModel();
            videoData.Video = InitilizeVideo(0);
            videoData.StatusList = YesNoList();
            return PartialView("Modal.FormVideo", videoData);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult VideoCreate(VideoModel items)
        {
            var ResultVideo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Video.Name, "`^<>");
            if (!ResultVideo)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterVideo.Update(items.Video, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterVideo")]
        [HttpGet]
        public ActionResult VideoEdit(long id)
        {
            ViewBag.crudMode = "U";
            var videoData = new VideoModel();
            videoData.Video = InitilizeVideo(id);
            videoData.StatusList = YesNoList();
            if (videoData.Video == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormVideo", videoData);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult VideoEdit(VideoModel items)
        {
            var ResultVideo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Video.Name, "`^<>");
            if (!ResultVideo)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterVideo video = Service.EMCS.MasterVideo.GetId(items.Video.Id);
                video.Id = items.Video.Id;
                video.Name = items.Video.Name;
                video.StartedDate = items.Video.StartedDate;
                video.FinishedDate = items.Video.FinishedDate;
                video.IsActive = items.Video.IsActive;

                Service.EMCS.MasterVideo.Update(video, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterVideo")]
        [HttpGet]
        public ActionResult VideoUpload(long id)
        {
            var videoData = new VideoModel();
            videoData.Video = InitilizeVideo(id);
            videoData.StatusList = YesNoList();
            if (videoData.Video == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormUploadVideo", videoData);
        }

        public string UploadVideo(string dir, long id)
        {
            string fileName = "";
            string strFiles = "";

            if (Request.Files.Count > 0)
            {

                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    if (App.Web.Helper.Extensions.FileExtention.isVideoFile(fileName))
                    {
                        if (fileName != null) strFiles = fileName;

                        fileName = strFiles;

                        var ext = Path.GetExtension(fileName);
                        var path = Server.MapPath("~/File/EMCS/Video/");
                        bool isExists = Directory.Exists(path);
                        fileName = id.ToString() + ext;

                        if (!isExists)
                            Directory.CreateDirectory(path);

                        var fullPath = Path.Combine(path, id.ToString() + ext);

                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        file.SaveAs(fullPath);
                    }
                    else
                    {
                        fileName = "";
                    }                 
                                       
                    return fileName;
                }
            }
            return fileName;
        }

        [HttpPost]
        public ActionResult VideoUpload(MasterVideo form)
        {
            ViewBag.crudMode = "U";
            string fileResult = UploadVideo("Video", form.Id);
            if (fileResult != "")
            {
                var item = Service.EMCS.MasterVideo.GetId(form.Id);
                item.Video = fileResult;
                Service.EMCS.MasterVideo.Update(item, ViewBag.crudMode);
                return Json(new { status = true, msg = "Upload Video Successfully" });
            }
            else
            {
                return Json(new { status = false, msg = "Upload Video gagal" });
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "MasterVideo")]
        [HttpPost]
        public ActionResult VideoDelete(MasterVideo items)
        {
            ViewBag.crudMode = "D";
            Service.EMCS.MasterVideo.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpPost]
        public ActionResult VideoDeleteById(int videoId)
        {
            MasterVideo item = Service.EMCS.MasterVideo.GetId(videoId);
            Service.EMCS.MasterVideo.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

    }
}