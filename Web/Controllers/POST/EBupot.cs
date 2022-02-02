using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Data.Domain.POST;
using App.Domain;
using App.Service.POST;
using App.Web.App_Start;
using ICSharpCode.SharpZipLib.Zip;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        //#region View

        public ActionResult EBupot()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 6;

            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult EBupotVendor()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Menu = 6;

            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public JsonResult GetEbupot(PaginationParamEbupot param)
        {
            try
            {
                var data = Service.POST.Ebupot.GetPagingDataEbupot(param);
                return Json(new { status = "SUCCESS", data.total, data.rows }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetEbupotVendor(PaginationParamEbupot param)
        {
            try
            {
                var data = Service.POST.Ebupot.GetPagingDataEbupotVendor(param);
                return Json(new { status = "SUCCESS", data.total, data.rows }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EbupotCreate(Data.Domain.POST.EbupotFormModel items)
        {
            try
            {
                var newID = Service.POST.Ebupot.Crud(items);
                return Json(new { status = "SUCCESS", result = newID }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> UploadFileBupot(string code, string id)
        {
            try
            {
                var attachment = Request.Files;
                var user = SiteConfiguration.UserName;
                var file = Request.Files[0];
                var fileNameOri = file.FileName;
                var FileExtension = Path.GetExtension(file.FileName);
                var pathAttachment = Global.GetParameterByName("PATH_ATTACHMENT_Ebupot");

                //var ErrorMessage = Service.POST.Ebupot.SaveFileAttachmentBupot(id, fileNameOri, file, code, pathAttachment);
                var ErrorMessage = await Service.POST.Ebupot.UploadToPortal(id, fileNameOri, file, code);

                if (ErrorMessage == "")
                    return Json(new { result = "SUCCESS", msg = "uploaded." }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { result = "FAILED", message = ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "FAILED", message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DownloadFileBupot(string fileName, string path)
        {
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var fileBytes = wc.DownloadData(path);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public ActionResult ThrowDownloadId(List<long> id)
        {
            Guid guid = Guid.NewGuid();

            Session[guid.ToString()] = id;

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public FileResult DownloadMultiFileBupot(string guid)
        {
            var ids = Session[guid] as List<long>;
            var fileName = string.Format("{0}_Bupot.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_" + RandomString(25));
            var path = Global.GetParameterByName("PATH_ATTACHMENT_Ebupot");
            var tempOutPutPath = path + fileName;
            //var tempOutPutPath = Global.GetParameterByName("PATH_ATTACHMENT_Ebupot");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];
                var data = Service.POST.Ebupot.GetMultiFileBupot(ids);

                for (int i = 0; i < data.Count; i++)
                {
                    var pathFile = data[i].Path;
                    var fileNameOri = data[i].FileNameOri;

                    ZipEntry entry = new ZipEntry(fileNameOri);
                    //entry.DateTime = DateTime.Now;
                    entry.IsUnicodeText = true;
                    s.PutNextEntry(entry);
                    WebClient wc = new WebClient();
                    using (Stream stream = wc.OpenRead(pathFile))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = stream.Read(buffer, 0, buffer.Length);
                            s.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }

                    //using (FileStream fs = System.IO.File.OpenRead(fileNameOri))
                    //{
                    //    int sourceBytes;
                    //    do
                    //    {
                    //        sourceBytes = fs.Read(buffer, 0, buffer.Length);
                    //        s.Write(buffer, 0, sourceBytes);
                    //    } while (sourceBytes > 0);
                    //}
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (finalResult == null || !finalResult.Any())
                throw new Exception(String.Format("No Files found"));

            return File(finalResult, "application/zip", fileName);

        }

        public JsonResult GetSelectVendor(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.Ebupot.GetSelectVendor(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSelectCabang(string search)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.Ebupot.GetSelectCabang(search, userLogin);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult DownloadExcelEbupot(SearchReport model)
        {
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadToExcelEbupot(model);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        private FileResult DownloadToExcelEbupot(SearchReport model)
        {
            try
            {
                var user = HttpContext.User.Identity.Name;
                var output = Service.POST.Ebupot.DownloadToExcelEbupot(model, user);
                return File(output.ToArray(),
                 "application/vnd.ms-excel",
                 "Ebupot.xlsx");
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Ebupot.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }
    }
}