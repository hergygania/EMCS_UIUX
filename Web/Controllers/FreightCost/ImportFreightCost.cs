using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using App.Web.App_Start;
using App.Service.FreightCost;
using saveToExcel = App.Service.Master.saveFileExcel;
using App.Domain;
using NPOI.SS.UserModel;
using System.Transactions;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        // GET: ImportFreightCalculator
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ImportMasterFreightCost()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            return View();
        }

        #region grid
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "FreightCostOld")]
        public ActionResult IndexPageLogImport()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetLogImportFCC();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "FreightCostOld")]
        public ActionResult GetLogImportFCC()
        {
            Func<MasterSearchForm, IList<Data.Domain.Extensions.DocumentUpload>> func = delegate (MasterSearchForm crit)
            {
                var doc = Service.Master.DocumentUpload.getListDocumentUpload("Freight Cost Calculator");

                return doc.ToList();

            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        [HttpPost]
        public ActionResult SaveUpload()
        {
            ImportFreight SS = new ImportFreight();
            string date = DateTime.Now.ToString("yyMMddHHmmss");
            string Modul = "Freight Cost Calculator";
            string retAction = "ImportMasterFreightCost";
            string _fileName = string.Empty;
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();
            var _itemModa = new Data.Domain.ModaOfCondition();
            string ErrTemplate = string.Empty;

            try
            {
                HttpPostedFileBase fileUpload = null;
                fileUpload = Request.Files[0];

                if (fileUpload.ContentLength > 0)
                {
                    string msg = "", filePathName = "";
                    string[] validFileTypes = { "doc", "xls", "pdf", "zip" };
                    System.IO.FileInfo fi = new System.IO.FileInfo(fileUpload.FileName);
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

                    var ret = saveToExcel.InsertHistoryUpload(fileUpload, ref _file, ref _logImport, Modul, ref filePathName, ref msg);
                    if (ret == false) return Json(new { Status = 1, Msg = msg });
                    _fileName = System.IO.Path.GetFileName(filePathName);

                    if (App.Web.Helper.Extensions.FileExtention.isExcelFile(_fileName))
                    {
                        ImportFreight.ALLsheet allSheet = new ImportFreight.ALLsheet();
                        allSheet = SS.getAllSheet(filePathName);

                        var resultHeader = checkHeader(allSheet);

                        if (resultHeader.Item1)
                        {
                            insertDB(allSheet, _fileName, Modul);

                            _file.Status = 1; //Success
                            Service.Master.DocumentUpload.crud(_file, "U");
                            //return JsonMessage("Your file successfuly uploaded", 0, "Success");
                            return Json(new { Status = 0, Msg = "Your file successfuly uploaded" });
                        }
                        else
                        {
                            _file.Status = 2;
                            Service.Master.DocumentUpload.crud(_file, "U");
                            ImportFreight.setException(_logImport, resultHeader.Item2, _fileName, Modul, "");
                            return Json(new { Status = 1, Msg = resultHeader.Item2 });
                        }
                    }

                    
                }
            }
            catch (Exception ex)
            {
                #region Update History Upload
                if (_file != null)
                {
                    _file.Status = 2;
                    Service.Master.DocumentUpload.crud(_file, "U");
                }

                ImportFreight.setException(_logImport, ex.Message.ToString(), _fileName, Modul, "");
                #endregion

                TempData["Message"] = ex.Message;
                return JsonMessage(ex.Message, 1, "Failed");
            }

            return RedirectToAction(retAction);
        }
      
        private Tuple<bool, string> checkHeader(ImportFreight.ALLsheet allSheet)
        {
            try
            {
                bool ret = false;
                string ErrTemplate = string.Empty;

                ErrTemplate += Service.FreightCost.ImportFreight.CheckHeaderExcel(allSheet.sheetRate, "Rate");
                ErrTemplate += Service.FreightCost.ImportFreight.CheckHeaderExcel(allSheet.sheetInboundRate, "Inbound Rate");
                ErrTemplate += Service.FreightCost.ImportFreight.CheckHeaderExcel(allSheet.sheetSurcharge, "Surcharge");
                ErrTemplate += Service.FreightCost.ImportFreight.CheckHeaderExcel(allSheet.sheetTruckRate, "Trucking Rate");

                if (string.IsNullOrWhiteSpace(ErrTemplate)) ret = true;

                return new Tuple<bool, string>(ret, ErrTemplate);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportMasterFreightCost")]
        private void insertDB(ImportFreight.ALLsheet allSheet, string _fileName, string Modul)
        {
            var _logImport = new Data.Domain.LogImport();

            try
            {
                MasterModaOfConditionTruckingRateImport readTruck = new MasterModaOfConditionTruckingRateImport();
                MasterRateImport readRate = new MasterRateImport();
                MasterInboundRateImport readInRate = new MasterInboundRateImport();
                MasterSurchargeImport readSurcharge = new MasterSurchargeImport();

                var _dataTruck = readTruck.GetDataTruck(allSheet.sheetTruckRate, _fileName, Modul);
                var _dataRate = readRate.GetRate(allSheet.sheetRate, _fileName, Modul);
                var _dataInRate = readInRate.GetInboundRate(allSheet.sheetInboundRate, _fileName, Modul);
                var _dataSurcharge = readSurcharge.GetSurcharge(allSheet.sheetSurcharge, _fileName, Modul);

                if (_dataTruck.Count > 0)
                    readTruck.SaveDataTrucking(_dataTruck);
                else
                    ImportFreight.setException(_logImport, "No Data Trucking Rate in sheet Trucking Rate", _fileName, Modul, "Trucking Rate");

                if (_dataRate.Count > 0)
                    readRate.SaveDataRate(_dataRate);
                else
                    ImportFreight.setException(_logImport, "No Data Rate in sheet Rate", _fileName, Modul, "Rate");

                if (_dataInRate.Count > 0)
                    readInRate.SaveInboundRate(_dataInRate);
                else
                    ImportFreight.setException(_logImport, "No Data Inbound Rate in sheet Inbound Rate", _fileName, Modul, "Inbound Rate");

                if (_dataSurcharge.Count > 0)
                    readSurcharge.SaveSurcharge(_dataSurcharge);
                else
                    ImportFreight.setException(_logImport, "No Data Surcharge in sheet Surcharge", _fileName, Modul, "Surcharge");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>
        {
            //{"ai", "application/postscript"},
            //{"aif", "audio/x-aiff"},
            //{"aifc", "audio/x-aiff"},
            //{"aiff", "audio/x-aiff"},
            //{"asc", "text/plain"},
            //{"atom", "application/atom+xml"},
            //{"au", "audio/basic"},
            //{"avi", "video/x-msvideo"},
            //{"bcpio", "application/x-bcpio"},
            //{"bin", "application/octet-stream"},
            //{"bmp", "image/bmp"},
            //{"cdf", "application/x-netcdf"},
            //{"cgm", "image/cgm"},
            //{"class", "application/octet-stream"},
            //{"cpio", "application/x-cpio"},
            //{"cpt", "application/mac-compactpro"},
            //{"csh", "application/x-csh"},
            //{"css", "text/css"},
            //{"dcr", "application/x-director"},
            //{"dif", "video/x-dv"},
            //{"dir", "application/x-director"},
            //{"djv", "image/vnd.djvu"},
            //{"djvu", "image/vnd.djvu"},
            //{"dll", "application/octet-stream"},
            //{"dmg", "application/octet-stream"},
            //{"dms", "application/octet-stream"},
            {"doc", "application/msword"},
            //{"docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            //{"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            //{"docm","application/vnd.ms-word.document.macroEnabled.12"},
            //{"dotm","application/vnd.ms-word.template.macroEnabled.12"},
            //{"dtd", "application/xml-dtd"},
            //{"dv", "video/x-dv"},
            //{"dvi", "application/x-dvi"},
            //{"dxr", "application/x-director"},
            //{"eps", "application/postscript"},
            //{"etx", "text/x-setext"},
            //{"exe", "application/octet-stream"},
            //{"ez", "application/andrew-inset"},
            //{"gif", "image/gif"},
            //{"gram", "application/srgs"},
            //{"grxml", "application/srgs+xml"},
            //{"gtar", "application/x-gtar"},
            //{"hdf", "application/x-hdf"},
            //{"hqx", "application/mac-binhex40"},
            //{"htc", "text/x-component"},
            //{"htm", "text/html"},
            //{"html", "text/html"},
            //{"ice", "x-conference/x-cooltalk"},
            //{"ico", "image/x-icon"},
            //{"ics", "text/calendar"},
            //{"ief", "image/ief"},
            //{"ifb", "text/calendar"},
            //{"iges", "model/iges"},
            //{"igs", "model/iges"},
            //{"jnlp", "application/x-java-jnlp-file"},
            //{"jp2", "image/jp2"},
            //{"jpe", "image/jpeg"},
            //{"jpeg", "image/jpeg"},
            //{"jpg", "image/jpeg"},
            //{"js", "application/x-javascript"},
            //{"kar", "audio/midi"},
            //{"latex", "application/x-latex"},
            //{"lha", "application/octet-stream"},
            //{"lzh", "application/octet-stream"},
            //{"m3u", "audio/x-mpegurl"},
            //{"m4a", "audio/mp4a-latm"},
            //{"m4b", "audio/mp4a-latm"},
            //{"m4p", "audio/mp4a-latm"},
            //{"m4u", "video/vnd.mpegurl"},
            //{"m4v", "video/x-m4v"},
            //{"mac", "image/x-macpaint"},
            //{"man", "application/x-troff-man"},
            //{"mathml", "application/mathml+xml"},
            //{"me", "application/x-troff-me"},
            //{"mesh", "model/mesh"},
            //{"mid", "audio/midi"},
            //{"midi", "audio/midi"},
            //{"mif", "application/vnd.mif"},
            //{"mov", "video/quicktime"},
            //{"movie", "video/x-sgi-movie"},
            //{"mp2", "audio/mpeg"},
            //{"mp3", "audio/mpeg"},
            //{"mp4", "video/mp4"},
            //{"mpe", "video/mpeg"},
            //{"mpeg", "video/mpeg"},
            //{"mpg", "video/mpeg"},
            //{"mpga", "audio/mpeg"},
            //{"ms", "application/x-troff-ms"},
            //{"msh", "model/mesh"},
            //{"mxu", "video/vnd.mpegurl"},
            //{"nc", "application/x-netcdf"},
            //{"oda", "application/oda"},
            //{"ogg", "application/ogg"},
            //{"pbm", "image/x-portable-bitmap"},
            //{"pct", "image/pict"},
            //{"pdb", "chemical/x-pdb"},
            {"pdf", "application/pdf"},
            //{"pgm", "image/x-portable-graymap"},
            //{"pgn", "application/x-chess-pgn"},
            //{"pic", "image/pict"},
            //{"pict", "image/pict"},
            //{"png", "image/png"},
            //{"pnm", "image/x-portable-anymap"},
            //{"pnt", "image/x-macpaint"},
            //{"pntg", "image/x-macpaint"},
            //{"ppm", "image/x-portable-pixmap"},
            //{"ppt", "application/vnd.ms-powerpoint"},
            //{"pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            //{"potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
            //{"ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            //{"ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            //{"pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            //{"potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
            //{"ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            //{"ps", "application/postscript"},
            //{"qt", "video/quicktime"},
            //{"qti", "image/x-quicktime"},
            //{"qtif", "image/x-quicktime"},
            //{"ra", "audio/x-pn-realaudio"},
            //{"ram", "audio/x-pn-realaudio"},
            //{"ras", "image/x-cmu-raster"},
            //{"rdf", "application/rdf+xml"},
            //{"rgb", "image/x-rgb"},
            //{"rm", "application/vnd.rn-realmedia"},
            //{"roff", "application/x-troff"},
            //{"rtf", "text/rtf"},
            //{"rtx", "text/richtext"},
            //{"sgm", "text/sgml"},
            //{"sgml", "text/sgml"},
            //{"sh", "application/x-sh"},
            //{"shar", "application/x-shar"},
            //{"silo", "model/mesh"},
            //{"sit", "application/x-stuffit"},
            //{"skd", "application/x-koan"},
            //{"skm", "application/x-koan"},
            //{"skp", "application/x-koan"},
            //{"skt", "application/x-koan"},
            //{"smi", "application/smil"},
            //{"smil", "application/smil"},
            //{"snd", "audio/basic"},
            //{"so", "application/octet-stream"},
            //{"spl", "application/x-futuresplash"},
            //{"src", "application/x-wais-source"},
            //{"sv4cpio", "application/x-sv4cpio"},
            //{"sv4crc", "application/x-sv4crc"},
            //{"svg", "image/svg+xml"},
            //{"swf", "application/x-shockwave-flash"},
            //{"t", "application/x-troff"},
            //{"tar", "application/x-tar"},
            //{"tcl", "application/x-tcl"},
            //{"tex", "application/x-tex"},
            //{"texi", "application/x-texinfo"},
            //{"texinfo", "application/x-texinfo"},
            //{"tif", "image/tiff"},
            //{"tiff", "image/tiff"},
            //{"tr", "application/x-troff"},
            //{"tsv", "text/tab-separated-values"},
            //{"txt", "text/plain"},
            //{"ustar", "application/x-ustar"},
            //{"vcd", "application/x-cdlink"},
            //{"vrml", "model/vrml"},
            //{"vxml", "application/voicexml+xml"},
            //{"wav", "audio/x-wav"},
            //{"wbmp", "image/vnd.wap.wbmp"},
            //{"wbmxl", "application/vnd.wap.wbxml"},
            //{"wml", "text/vnd.wap.wml"},
            //{"wmlc", "application/vnd.wap.wmlc"},
            //{"wmls", "text/vnd.wap.wmlscript"},
            //{"wmlsc", "application/vnd.wap.wmlscriptc"},
            //{"wrl", "model/vrml"},
            //{"xbm", "image/x-xbitmap"},
            //{"xht", "application/xhtml+xml"},
            //{"xhtml", "application/xhtml+xml"},
            {"xls", "application/vnd.ms-excel"},
            //{"xml", "application/xml"},
            //{"xpm", "image/x-xpixmap"},
            //{"xsl", "application/xml"},
            //{"xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            //{"xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            //{"xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
            //{"xltm","application/vnd.ms-excel.template.macroEnabled.12"},
            //{"xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
            //{"xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            //{"xslt", "application/xslt+xml"},
            //{"xul", "application/vnd.mozilla.xul+xml"},
            //{"xwd", "image/x-xwindowdump"},
            //{"xyz", "chemical/x-xyz"},
            {"zip", "application/zip"}
        };

    }
}