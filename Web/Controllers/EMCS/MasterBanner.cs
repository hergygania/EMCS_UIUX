using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;
using System.IO;
using NPOI.SS.Formula.Functions;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterBanner InitilizeBanner(long bannerId)
        {
            MasterBanner banner = new MasterBanner();
            if (bannerId == 0)
            {
                return banner;
            }

            banner = Service.EMCS.MasterBanner.GetId(bannerId);
            return banner;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Banner")]
        public ActionResult Banner()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Banner")]
        public ActionResult BannerPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return BannerPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Banner")]
        public ActionResult BannerPageXt()
        {
            Func<MasterSearchForm, IList<MasterBanner>> func = delegate (MasterSearchForm crit)
            {
                List<MasterBanner> list = Service.EMCS.MasterBanner.GetBannerList(crit);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        public List<MasterStatus> YesNoList()
        {
            List<MasterStatus> listStat = new List<MasterStatus>();
            listStat.Add(new MasterStatus() { Value = false, Text = "No" });
            listStat.Add(new MasterStatus() { Value = true, Text = "Yes" });
            return listStat;
        }

        [HttpGet]
        public ActionResult PreviewImage(long id)
        {
            var bannerData = new BannerModel();
            bannerData.Banner = InitilizeBanner(id);
            bannerData.StatusList = YesNoList();

            if (bannerData.Banner == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.PreviewBanner", bannerData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Banner")]
        [HttpGet]
        public ActionResult BannerCreate()
        {
            ViewBag.crudMode = "I";
            var bannerData = new BannerModel();
            bannerData.Banner = InitilizeBanner(0);
            bannerData.StatusList = YesNoList();
            return PartialView("Modal.FormBanner", bannerData);
        }

        [HttpPost, ValidateInput(false)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Banner")]
        public ActionResult BannerCreate(BannerModel items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Banner.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Banner.Description, "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterBanner.Update(items.Banner, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Banner")]
        [HttpGet]
        public ActionResult BannerEdit(long id)
        {
            ViewBag.crudMode = "U";
            var bannerData = new BannerModel();
            bannerData.Banner = InitilizeBanner(id);
            bannerData.StatusList = YesNoList();
            if (bannerData.Banner == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormBanner", bannerData);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Banner")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult BannerEdit(BannerModel items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Banner.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Banner.Description, "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterBanner banner = Service.EMCS.MasterBanner.GetId(items.Banner.Id);
                banner.Id = items.Banner.Id;
                banner.Name = items.Banner.Name;
                banner.Description = items.Banner.Description;
                banner.StartedDate = items.Banner.StartedDate;
                banner.FinishedDate = items.Banner.FinishedDate;
                banner.IsActive = items.Banner.IsActive;

                Service.EMCS.MasterBanner.Update(banner, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Banner")]
        [HttpGet]
        public ActionResult BannerUpload(long id)
        {
            Service.EMCS.MasterBanner.GetId(id);
            var bannerData = new BannerModel();
            bannerData.Banner = InitilizeBanner(id);
            bannerData.StatusList = YesNoList();
            if (bannerData.Banner == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormUploadBanner", bannerData);
        }

        public string UploadFile(string dir, long id)
        {
            string fileName = "";

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    return GetFileName(file, id);
                }
            }
            return fileName;
        }

        public string GetFileName(System.Web.HttpPostedFileBase file, long id)
        {
            var fileName = Path.GetFileName(file.FileName);

            if (fileName != null)
            {
                if (App.Web.Helper.Extensions.FileExtention.isImageFile(fileName))
                {
                    var ext = Path.GetExtension(fileName);
                    var path = Server.MapPath("~/Images/EMCS/Banner/");
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
               
            }

            return fileName;
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Banner")]
        [HttpPost]
        public ActionResult BannerUpload(MasterBanner form)
        {
            ViewBag.crudMode = "U";
          
            string fileResult = UploadFile("Banner", form.Id);
            if (fileResult != "")
            {
                var item = Service.EMCS.MasterBanner.GetId(form.Id);
                item.Images = fileResult;
                Service.EMCS.MasterBanner.Update(item, ViewBag.crudMode);
                return Json(new { status = true, msg = "Upload File Successfully" });
            }
            else
            {
                return Json(new { status = false, msg = "Upload File gagal" });
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Banner")]
        [HttpGet]
        public ActionResult BannerDelete(long id)
        {
            ViewBag.crudMode = "D";
            var bannerData = new BannerModel();
            bannerData.Banner = InitilizeBanner(id);
            bannerData.StatusList = YesNoList();

            if (bannerData.Banner == null)
            {
                return HttpNotFound();
            }

            return PartialView("Banner", bannerData);
        }

        [HttpPost]
        public ActionResult BannerDelete(MasterBanner items)
        {
            ViewBag.crudMode = "D";
            Service.EMCS.MasterBanner.Update(
                items,
                ViewBag.crudMode);
            return JsonCRUDMessage(ViewBag.crudMode);
        }

        [HttpPost]
        public ActionResult BannerDeleteById(int bannerId)
        {
            MasterBanner item = Service.EMCS.MasterBanner.GetId(bannerId);
            Service.EMCS.MasterBanner.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

    }
}