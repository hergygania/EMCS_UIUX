using App.Data.Caching;
using App.Data.Domain.POST;
using App.Domain;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace App.Service.POST
{
    public static class Ebupot
    {
        public const string CacheName = "App.POST.BupotSpt23";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        public readonly static XSSFWorkbook workbook = new XSSFWorkbook();

        public static BsTablePagingResult GetPagingDataEbupot(PaginationParamEbupot param)
        {
            string whereCondition = SetWhereCondition(param);
            string orderBy = SetOrderBy(param);

            BsTablePagingResult table = new BsTablePagingResult();
            table.rows = GetEbupotList(param, whereCondition, orderBy);
            table.total = GetEbupotListTotal(whereCondition);

            return table;
        }

        public static BsTablePagingResult GetPagingDataEbupotVendor(PaginationParamEbupot param)
        {
            string whereCondition = SetWhereCondition(param);
            string orderBy = SetOrderBy(param);

            BsTablePagingResult table = new BsTablePagingResult();
            table.rows = GetEbupotVendorList(param, whereCondition, orderBy);
            table.total = GetEbupotVendorListTotal(whereCondition);

            return table;
        }

        private static string SetOrderBy(PaginationParamEbupot param)
        {
            string orderBy = param.Sort;

            if (string.IsNullOrEmpty(orderBy))
                return "";

            if (param.Order == "asc")
                orderBy = orderBy + " asc";
            else
                orderBy = orderBy + " desc";

            return orderBy;
        }

        private static string SetWhereCondition(PaginationParamEbupot filter)
        {
            string whereCondition = "";
            if (!string.IsNullOrEmpty(filter.Cabang))
            {
                whereCondition += " and a.namaCabang='" + filter.Cabang + "'";
            }
            if (!string.IsNullOrEmpty(filter.NpwpVendor))
            {
                whereCondition += " and npwpVendor='" + filter.NpwpVendor + "'";
            }
            else
            {
                if (!string.IsNullOrEmpty(filter.Vendor))
                {
                    whereCondition += " and namaVendor='" + filter.Vendor + "'";
                }
            }
            if (!string.IsNullOrEmpty(filter.NoBuktiPotong))
            {
                whereCondition += " and noBupot like '%" + filter.NoBuktiPotong + "%'";
            }
            if (!string.IsNullOrEmpty(filter.DariMasa))
            {
                whereCondition += " and format(tgl, 'yyyyMM') >='" + filter.DariMasa + "'";
            }
            if (!string.IsNullOrEmpty(filter.KeMasa))
            {
                whereCondition += " and format(tgl, 'yyyyMM') <='" + filter.KeMasa + "'";
            }

            return whereCondition;
        }

        public static List<EBupotList> GetEbupotList(PaginationParamEbupot param, string whereCondition, string orderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@WhereCondition", whereCondition));
                parameterList.Add(new SqlParameter("@OrderBy", orderBy));
                parameterList.Add(new SqlParameter("@Offset", param.Offset.ToString()));
                parameterList.Add(new SqlParameter("@Fetch", param.Limit == 0 ? 1 : param.Limit));
                parameterList.Add(new SqlParameter("@IsTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<EBupotList>(@"exec [dbo].[SP_GetEbupotList] @user,@WhereCondition,@OrderBy,@Offset,@Fetch,@IsTotal", parameters).ToList();
                return data;
            }
        }

        public static int GetEbupotListTotal(string whereCondition)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@WhereCondition", whereCondition));
                parameterList.Add(new SqlParameter("@OrderBy", ""));
                parameterList.Add(new SqlParameter("@Offset", "0"));
                parameterList.Add(new SqlParameter("@Fetch", ""));
                parameterList.Add(new SqlParameter("@IsTotal", "1"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SP_GetEbupotList] @user,@WhereCondition,@OrderBy,@Offset,@Fetch,@IsTotal", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<EBupotList> GetEbupotVendorList(PaginationParamEbupot param, string whereCondition, string orderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@WhereCondition", whereCondition));
                parameterList.Add(new SqlParameter("@OrderBy", orderBy));
                parameterList.Add(new SqlParameter("@Offset", param.Offset.ToString()));
                parameterList.Add(new SqlParameter("@Fetch", param.Limit == 0 ? 1 : param.Limit));
                parameterList.Add(new SqlParameter("@IsTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<EBupotList>(@"exec [dbo].[SP_GetEbupotVendorList] @user,@WhereCondition,@OrderBy,@Offset,@Fetch,@IsTotal", parameters).ToList();
                return data;
            }
        }

        public static int GetEbupotVendorListTotal(string whereCondition)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@WhereCondition", whereCondition));
                parameterList.Add(new SqlParameter("@OrderBy", ""));
                parameterList.Add(new SqlParameter("@Offset", "0"));
                parameterList.Add(new SqlParameter("@Fetch", ""));
                parameterList.Add(new SqlParameter("@IsTotal", "1"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SP_GetEbupotVendorList] @user,@WhereCondition,@OrderBy,@Offset,@Fetch,@IsTotal", parameters).FirstOrDefault();
                return data;
            }
        }

        public static string Crud(Data.Domain.POST.EbupotFormModel itm)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                string dml = "I";

                using (var db2 = new Data.POSTContext())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(itm.Cabang))
                        {
                            var cabang = db2.MtMappingUserBranch.FirstOrDefault(o => o.UserID == SiteConfiguration.UserName);
                            if (cabang != null)
                                itm.Cabang = cabang.NamaCabang;
                        }

                        var orgID = (Guid.NewGuid()).ToString();
                        var org = db2.BupotOrganizationDetail.FirstOrDefault(o => o.nama == itm.Cabang);
                        if (org != null)
                            orgID = org.organizationID;

                        var refID = (Guid.NewGuid()).ToString();
                        var refs = db2.BupotSpt23Ref.FirstOrDefault(o => o.noDok == itm.InvoiceNo);
                        if (refs != null)
                            refID = refs.refsID;

                        BupotSpt23 bupotSpt = new BupotSpt23();
                        bupotSpt.BupotSpt23ID = (Guid.NewGuid()).ToString();
                        bupotSpt.masa = itm.MasaPajak.Month;
                        bupotSpt.tahun = itm.MasaPajak.Year;
                        bupotSpt.pembetulan = 0;
                        bupotSpt.sebelumnya = "";
                        bupotSpt.organizationID = orgID;
                        bupotSpt.status = "FINISH";
                        bupotSpt.message = null;
                        bupotSpt.currentState = "";
                        bupotSpt.flowStateAccess = "";
                        bupotSpt.flowState = "";
                        bupotSpt.createdBy = SiteConfiguration.UserName;
                        bupotSpt.createdDate = DateTime.Now;
                        bupotSpt.lastModifiedBy = SiteConfiguration.UserName;
                        bupotSpt.lastModifiedDate = DateTime.Now;
                        db.CreateRepository<Data.Domain.POST.BupotSpt23>().CRUD(dml, bupotSpt);

                        BupotSpt23Detail detail = new BupotSpt23Detail();
                        detail.BupotSpt23DetailID = (Guid.NewGuid()).ToString();
                        detail.seq = 1;
                        detail.row = 1;
                        detail.rev = 0;
                        detail.no = itm.NoBuktiPotong;
                        detail.referensi = null;
                        detail.tgl = itm.MasaPajak;
                        detail.npwpPenandatangan = "";
                        detail.namaPenandatangan = "";
                        detail.signAs = true;
                        detail.status = "FINISH"; //?
                        detail.fasilitas = 0;
                        detail.noSkb = null;
                        detail.tglSkb = null;
                        detail.noDtp = null;
                        detail.ntpn = null;
                        detail.BupotSpt23ID = bupotSpt.BupotSpt23ID;
                        detail.kode = itm.Kode;
                        detail.tarif = itm.Tarif;
                        detail.bruto = itm.Bruto;
                        detail.pph = itm.PPH;
                        detail.nik = itm.NIK;
                        detail.tin = itm.Tin;
                        detail.identity = null;
                        var vendorData = db2.MtVendor.FirstOrDefault(o => o.Name == itm.NamaVendor && o.NPWP == itm.NpwpVendor);
                        if (vendorData != null)
                        {
                            detail.npwp = itm.NpwpVendor;
                            detail.nama = vendorData.Name;
                            detail.alamat = vendorData.Address;
                            detail.kabupaten = vendorData.City;
                        }
                        detail.kelurahan = null;
                        detail.kecamatan = null;
                        detail.provinsi = null;
                        detail.kodePos = null;
                        detail.email = null;
                        detail.noTelp = null;
                        detail.message = null;
                        detail.refLogFileId = null;
                        detail.refXmlId = null;
                        detail.refIdBefore = null;
                        detail.idBupotDjp = null;
                        detail.report = true;
                        detail.userId = "";
                        detail.createdBy = SiteConfiguration.UserName;
                        detail.createdDate = DateTime.Now;
                        detail.lastModifiedBy = SiteConfiguration.UserName;
                        detail.lastModifiedDate = DateTime.Now;
                        db.CreateRepository<Data.Domain.POST.BupotSpt23Detail>().CRUD(dml, detail);

                        BupotSpt23Ref detailRef = new BupotSpt23Ref();
                        detailRef.refsID = refID;
                        detailRef.BupotSpt23DetailID = detail.BupotSpt23DetailID;
                        detailRef.jenis = "02";
                        detailRef.noDok = itm.InvoiceNo;
                        detailRef.tgl = itm.Date;
                        detailRef.bp23 = null;
                        detailRef.bp26 = null;
                        detailRef.createdBy = SiteConfiguration.UserName;
                        detailRef.createdDate = DateTime.Now;
                        detailRef.lastModifiedBy = SiteConfiguration.UserName;
                        detailRef.lastModifiedDate = DateTime.Now;
                        db.CreateRepository<Data.Domain.POST.BupotSpt23Ref>().CRUD(dml, detailRef);

                        return detail.BupotSpt23DetailID;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                CacheManager.Remove(CacheName);

                return "";
            }
        }

        public static Data.Domain.POST.BupotSpt23 GetBupotSpt23ById(string Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.BupotSpt23.FirstOrDefault(a => a.BupotSpt23ID == Id);
                return tb;
            }
        }

        public static Data.Domain.POST.BupotSpt23Detail GetBupotSpt23DetailById(string Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.BupotSpt23Detail.FirstOrDefault(a => a.BupotSpt23DetailID == Id);
                return tb;
            }
        }

        public static Data.Domain.POST.BupotOrganizationDetail GetBupotOrganizationDetailById(string Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.BupotOrganizationDetail.FirstOrDefault(a => a.organizationID == Id);
                return tb;
            }
        }

        public static Data.Domain.POST.BupotSpt23Ref GetBupotSpt23RefById(string Id)
        {
            using (var db = new Data.POSTContext())
            {
                var tb = db.BupotSpt23Ref.FirstOrDefault(a => a.refsID == Id);
                return tb;
            }
        }

        public static List<Select2Result4> GetSelectVendor(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result4>(@"exec [dbo].[SP_Vendor_SELECT] @Search", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result4> GetSelectCabang(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result4>(@"exec [dbo].[SP_Cabang_SELECT] @Search,@User", parameters).ToList();
                return data;
            }
        }

        public static async Task<string> UploadToPortal(string id, string fileName, HttpPostedFileBase file, string codeAttachment)
        {
            try
            {
                byte[] fileData;
                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    fileData = memoryStream.ToArray();
                }

                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(new MemoryStream(fileData)), fileName);

                        var url = $"http://netapps.trakindo.co.id:8042/AutoSendFileToSharepointService.svc/sharepoint/uploadfile/0006/{fileName}/eBupotDocument/TestFolder/0/dit.system.wssp";
                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            string path = $"http://netapps.trakindo.co.id:8042/AutoSendFileToSharepointService.svc/sharepoint/downloadfile/{fileName}/0006/eBupotDocument/TestFolder/0/dit.system.wssp";
                            //Mapping Attachment to POST
                            MappingAttachment(id, fileName, path, codeAttachment);
                        }
                        else
                        {
                            throw new Exception("Upload failed");
                        }
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static void MappingAttachment(string id, string filename, string path, string codeAttachment)
        {
            try
            {
                var model = new TrAttachment();
                var userLogin = SiteConfiguration.UserName;

                using (var db = new Data.POSTContext())
                {
                    model.Code = id;
                    model.Path = path;
                    model.FileName = filename;
                    model.FileNameOri = Path.GetFileName(filename);
                    model.CodeAttachment = codeAttachment;
                    model.UploadedDate = DateTime.Now;
                    model.UploadedBy = userLogin;

                    db.TrAttachment.Add(model);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static string SaveFileAttachmentBupot(string id, string fileNameOri, HttpPostedFileBase file, string codeAttachment, string path)
        {
            try
            {
                var model = new TrAttachment();
                var userLogin = SiteConfiguration.UserName;
                var fileName = fileNameOri;

                using (var db = new Data.POSTContext())
                {
                    model.Code = id;
                    model.Path = path;
                    model.FileName = fileName;
                    model.FileNameOri = Path.GetFileName(fileNameOri);
                    model.CodeAttachment = codeAttachment;
                    model.UploadedDate = DateTime.Now;
                    model.UploadedBy = userLogin;

                    db.TrAttachment.Add(model);
                    db.SaveChanges();




                    fileName = model.CodeAttachment + "_" + model.ID + "_" + fileNameOri + Path.GetExtension(fileNameOri);
                    path = Global.CreateShareFolderBupot(path, model.UploadedDate, id);
                    Global.SaveFileToShareFolderRequest(path, fileName, file);

                    model.FileName = fileName;
                    model.Path = path + fileName;
                    db.SaveChanges();

                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static TrAttachment GetFileBupotById(int id)
        {
            using (var db = new Data.POSTContext())
            {
                var detail = db.BupotSpt23Detail.FirstOrDefault(o => o.ID == id);
                var data = db.TrAttachment.FirstOrDefault(o => o.Code == detail.BupotSpt23DetailID);
                return data;
            }
        }

        public static List<TrAttachment> GetMultiFileBupot(List<long> id)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from c in db.BupotSpt23Detail.Where(o => id.Contains(o.ID))
                            from d in db.TrAttachment.Where(o => o.Code == c.BupotSpt23DetailID)
                            select d).ToList();
                //var detail = db.BupotSpt23Detail.FirstOrDefault(o => o.ID == id);
                //var data = db.TrAttachment.FirstOrDefault(o => o.Code == detail.BupotSpt23DetailID);
                //return data;
                return data;
            }
        }


        public static MemoryStream DownloadToExcelEbupot(SearchReport model, string user)
        {
            try
            {
                //Create new Excel Sheet
                XSSFWorkbook workbookSLA = new XSSFWorkbook();
                ISheet sheetSLA = workbookSLA.CreateSheet();

                //Create a header row
                var font = workbookSLA.CreateFont();
                var headerRow = sheetSLA.CreateRow(0);
                headerRow.CreateCell(0).SetCellValue("Status SPT");
                headerRow.CreateCell(1).SetCellValue("Nama Cabang");
                headerRow.CreateCell(2).SetCellValue("NPWP Cabang");
                headerRow.CreateCell(3).SetCellValue("Masa");
                headerRow.CreateCell(4).SetCellValue("Tahun");
                headerRow.CreateCell(5).SetCellValue("Pembetulan");
                headerRow.CreateCell(6).SetCellValue("Pesan");
                headerRow.CreateCell(7).SetCellValue("No Bupot");
                headerRow.CreateCell(8).SetCellValue("Rev Bupot");
                headerRow.CreateCell(9).SetCellValue("Status Bupot");
                headerRow.CreateCell(10).SetCellValue("Cetak");
                headerRow.CreateCell(11).SetCellValue("Pesan");
                headerRow.CreateCell(12).SetCellValue("Tin");
                headerRow.CreateCell(13).SetCellValue("NPWP Vendor");
                headerRow.CreateCell(14).SetCellValue("NIK");
                headerRow.CreateCell(15).SetCellValue("Nama Vendor");
                headerRow.CreateCell(16).SetCellValue("Email");
                headerRow.CreateCell(17).SetCellValue("Invoice No");
                headerRow.CreateCell(18).SetCellValue("Kode");
                headerRow.CreateCell(19).SetCellValue("Bruto");
                headerRow.CreateCell(20).SetCellValue("PPH");
                headerRow.CreateCell(21).SetCellValue("XML ID");
                headerRow.CreateCell(22).SetCellValue("Ref Log File Id");
                headerRow.CreateCell(23).SetCellValue("Tanggal BP");
                headerRow.CreateCell(24).SetCellValue("Pembuat");
                headerRow.CreateCell(25).SetCellValue("Dibuat");
                headerRow.CreateCell(26).SetCellValue("Diupdate Oleh");
                headerRow.CreateCell(27).SetCellValue("Terakhir Update");

                int rowNumber = 1;
                model.isExport = true;
                PaginationParamEbupot param = new PaginationParamEbupot();

                string whereCondition = SetWhereCondition(param);
                string orderBy = SetOrderBy(param);
                var data = GetEbupotList(param, whereCondition, orderBy);

                foreach (var item in data)
                {
                    var rowQuestion = sheetSLA.CreateRow(rowNumber);
                    rowQuestion.CreateCell(0).SetCellValue(item.status);
                    rowQuestion.CreateCell(1).SetCellValue(item.namaCabang);
                    rowQuestion.CreateCell(2).SetCellValue(item.npwpCabang);
                    rowQuestion.CreateCell(3).SetCellValue(item.masa.ToString());
                    rowQuestion.CreateCell(4).SetCellValue(item.tahun.ToString());
                    if (item.pembetulan.HasValue)
                        rowQuestion.CreateCell(5).SetCellValue(item.pembetulan.ToString());
                    rowQuestion.CreateCell(6).SetCellValue(item.message);
                        rowQuestion.CreateCell(7).SetCellValue(item.noBupot);

                    if (item.revBupot.HasValue)
                        rowQuestion.CreateCell(8).SetCellValue(item.revBupot.ToString());
                    rowQuestion.CreateCell(9).SetCellValue(item.statusBupot);

                    if (item.cetak.HasValue)
                        rowQuestion.CreateCell(10).SetCellValue(item.cetak.ToString());

                    rowQuestion.CreateCell(11).SetCellValue(item.pesanBupot);
                    rowQuestion.CreateCell(12).SetCellValue(item.tin1);

                        rowQuestion.CreateCell(13).SetCellValue(item.npwpVendor);

                    rowQuestion.CreateCell(14).SetCellValue(item.nik);
                    rowQuestion.CreateCell(15).SetCellValue(item.namaVendor);
                    rowQuestion.CreateCell(16).SetCellValue(item.emailVendor);
                    rowQuestion.CreateCell(17).SetCellValue(item.invoiceNo);
                    rowQuestion.CreateCell(18).SetCellValue(item.kode);
                    if (item.bruto.HasValue)
                        rowQuestion.CreateCell(19).SetCellValue(item.bruto.ToString());
                    if (item.pph.HasValue)
                        rowQuestion.CreateCell(20).SetCellValue(item.pph.ToString());
                    if (item.refXmlId.HasValue)
                        rowQuestion.CreateCell(21).SetCellValue(item.refXmlId.ToString());
                    rowQuestion.CreateCell(22).SetCellValue(item.refLogFileId);
                    if (item.tgl.HasValue)
                        rowQuestion.CreateCell(23).SetCellValue(item.tgl.ToString());
                    rowQuestion.CreateCell(24).SetCellValue(item.createdBy);
                    if (item.createdDate.HasValue)
                        rowQuestion.CreateCell(25).SetCellValue(item.createdDate.ToString());
                    rowQuestion.CreateCell(26).SetCellValue(item.lastModifiedBy);
                    if (item.lastModifiedDate.HasValue)
                        rowQuestion.CreateCell(27).SetCellValue(item.lastModifiedDate.ToString());

                    rowNumber = rowNumber + 1;
                }
                MemoryStream output = new MemoryStream();
                workbookSLA.Write(output);

                return output;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                MemoryStream output = new MemoryStream();
                workbook.Write(output);
                return output;
            }
        }
    }
}
