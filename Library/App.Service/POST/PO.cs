using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using System.Globalization;

namespace App.Service.POST
{
    public static class PO
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region List
        public static List<POIncoming_LIST> GetListPoInComing(string user, SearchHeader param)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = DateTime.ParseExact(param.StartDatePoReceipt, Global.dateformatParam, null).ToString();
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = DateTime.ParseExact(param.EndDatePoReceipt, Global.dateformatParam, null).ToString();

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", "INCOMING"));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POIncoming_LIST>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ,@skip
                    ,@take
                    ,@isTotal
                    ", parameters).ToList();
                return data;
            }
        }

        public static int GetTotalIncomingList(string user, SearchHeader param)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = DateTime.ParseExact(param.StartDatePoReceipt, Global.dateformatParam, null).ToString();
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = DateTime.ParseExact(param.EndDatePoReceipt, Global.dateformatParam, null).ToString();

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", "INCOMING"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<TotalSla>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ", parameters).FirstOrDefault();
                return data.CountTotal;
            }
        }

        public static List<POInProgress_LIST> GetListPoInProgress(string user, SearchHeader param)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = DateTime.ParseExact(param.StartDatePoReceipt, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = DateTime.ParseExact(param.EndDatePoReceipt, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", "PROGRESS"));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POInProgress_LIST>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ,@skip
                    ,@take
                    ,@isTotal
                    ", parameters).ToList();
                return data;
            }
        }

        public static List<PODone_LIST> GetListPoDone(string user, SearchHeader param)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = DateTime.ParseExact(param.StartDatePoReceipt, Global.dateformatParam, null).ToString();
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = DateTime.ParseExact(param.EndDatePoReceipt, Global.dateformatParam, null).ToString();

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", "DONE"));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PODone_LIST>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ,@skip
                    ,@take
                    ,@isTotal
                    ", parameters).ToList();

                return data;
            }
        }

        public static List<PODone_LIST> GetListPoReject(string user, SearchHeader param)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, null).ToString();
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = DateTime.ParseExact(param.StartDatePoReceipt, Global.dateformatParam, null).ToString();
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = DateTime.ParseExact(param.EndDatePoReceipt, Global.dateformatParam, null).ToString();

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", "REJECT"));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PODone_LIST>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ,@skip
                    ,@take
                    ,@isTotal
                    ", parameters).ToList();

                return data;
            }
        }

        public static List<AttachmentList> GetListAttachment(int id, string type,string status ="")
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                parameterList.Add(new SqlParameter("@Type", type ?? ""));
                parameterList.Add(new SqlParameter("@status", status ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<AttachmentList>(@"exec [dbo].[SP_RequestAttachment_GET]@Id,@Type,@status", parameters).ToList();
                return data;
            }
        }

        public static List<AttachmentListSAP> GetListAttachmentSAP(int id, string type)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                parameterList.Add(new SqlParameter("@Type", type ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<AttachmentListSAP>(@"exec [dbo].[SP_RequestAttachmentSAP_GET]@Id,@Type", parameters).ToList();
                return data;
            }
        }
        public static CountPOList GetCountPoList(string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<CountPOList>(@"exec [dbo].[SP_CountPOList_LIST]@User", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<ItemByRequestId_LIST> GetListItemByRequestId(int requestId, SearchDetail param, string user)
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = DateTime.ParseExact(param.StartDateDeliveryDate, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = DateTime.ParseExact(param.EndDateDeliveryDate, Global.dateformatParam, CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
<<<<<<< HEAD
                parameterList.Add(new SqlParameter("@RequestId", requestId ));
=======
                parameterList.Add(new SqlParameter("@RequestId", requestId));
>>>>>>> 639d8d0 (Intial commit)
                parameterList.Add(new SqlParameter("@ItemName", param.Item ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@Branch", param.Destination ?? ""));
                parameterList.Add(new SqlParameter("@Status", param.Status ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemByRequestId_LIST>(@"exec [dbo].[SP_ItemByRequestId_LIST]
                    @RequestId
                    , @ItemName
                    , @StartDateDeliveryDate
                    , @EndDateDeliveryDate
                    , @Branch
                    , @Status
                    , @user"
                , parameters).ToList();
                return data;
            }
        }

        public static List<POItemCKBByPO_LIST> GetDataItemCkbByPo(string poNo, string poItem)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@poNo", poNo ?? ""));
                parameterList.Add(new SqlParameter("@poItem", poItem ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POItemCKBByPO_LIST>(@"exec [dbo].[SP_GetCKBStatusDelivery]@poNo,@poItem", parameters).ToList();
                return data;
            }
        }

        public static List<POByRequestId_LIST> GetListPoByRequestId(int requestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestId", requestId));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POByRequestId_LIST>(@"exec [dbo].[SP_POByRequestId_LIST]@RequestId", parameters).ToList();
                return data;
            }
        }

        public static POByRequestId_LIST GetListPoByRequestIdTop(int requestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestId", requestId));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POByRequestId_LIST>(@"exec [dbo].[SP_POByRequestId_LIST]@RequestId", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<ItemHistoryById_LIST> GetListItemHistoryById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemHistoryById_LIST>(@"exec [dbo].[SP_ItemHistoryById_LIST]@Id", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result3> GetSelectItemByRequestId(string search, int requestId, string type)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestId", requestId));
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                parameterList.Add(new SqlParameter("@type", type ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result3>(@"exec [dbo].[SP_DataITEMByID_SELECT]
                @RequestId
                ,@Search
                ,@type
                ", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result3> GetSelectItemAttachmentProgress(string search, int id, string type)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                parameterList.Add(new SqlParameter("@Type", type));
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result3>(@"exec [dbo].[SP_AttachProgress_SELECT]@Id,@Type,@Search", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result3> GetSelectItemByAttachId(string search, int attachId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@AttachId", attachId));
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result3>(@"exec [dbo].[SP_DataITEMByAttachID_SELECT]@AttachId,@Search", parameters).ToList();
                return data;
            }
        }

        public static ItemById_LIST GetItemListById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemById_LIST>(@"exec [dbo].[SP_ItemById_LIST]@Id", parameters).FirstOrDefault();
                return data;
            }
        }
        public static List<ItemById_LIST> GetItemListByPO(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemById_LIST>(@"exec [dbo].[SP_ItemByPO_LIST]@Id", parameters).ToList();
                return data;
            }
        }
        public static List<HistoryPOById_List> GetListHistoryById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<HistoryPOById_List>(@"exec [dbo].[SP_HistoryPOById_List]@Id", parameters).ToList();
                return data;
            }
        }
        public static List<ItemById_LIST> GetItemPartialListById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemById_LIST>(@"exec [dbo].[SP_ItemPartialById_LIST]@Id", parameters).ToList();
                return data;
            }
        }
<<<<<<< HEAD
        public static List<InvoiceHardCopy_List> GetHardCopyInvoiceById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceHardCopy_List>(@"exec [dbo].[SP_HardCopyInvoiceById_LIST]@Id", parameters).ToList();
                return data;
            }
        }
        public static List<InvoiceHardCopy_List> GetHardCopyInvoiceByInvoiceId(int invoiceid)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", invoiceid));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceHardCopy_List>(@"exec [dbo].[SP_HardCopyInvoiceByInvoiceId_LIST]@Id", parameters).ToList();
                return data;
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)
        //public static int RemovePartialListById(int Id)
        //{
        //    using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
        //    {
        //        var data = db.DbContext.Database.SqlQuery<ItemById_LIST>(@"exec [dbo].[SP_ItemPartialById_LIST]@Id", Id).ToList();
        //        return 1;
        //    }
<<<<<<< HEAD

=======
     
>>>>>>> 639d8d0 (Intial commit)
        //}

        public static int RemovePartialListById(int Id)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrItemPartialQty where p.Id == Id select p).FirstOrDefault();
                data.IsActive = false;
                db.SaveChanges();
            }
            return 1;
        }
<<<<<<< HEAD
        public static int RemoveHarcopyInvoiceById(int Id)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.InvoiceHardCopy where p.Id == Id select p).FirstOrDefault();
                data.IsActive = false;
                db.SaveChanges();
            }
            return 1;
        }
=======
>>>>>>> 639d8d0 (Intial commit)

        public static ItemById_LIST GetItemPartialById(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemById_LIST>(@"exec [dbo].[SP_ItemPartialById_SINGLE]@Id", parameters).FirstOrDefault();
                return data;
            }
        }
        #endregion

        public static int GetTotalList(string user, SearchHeader param, string type = "INCOMING")
        {
            if (param.StartDateDeliveryDate != null) param.StartDateDeliveryDate = param.StartDateDeliveryDate;
            if (param.EndDateDeliveryDate != null) param.EndDateDeliveryDate = param.EndDateDeliveryDate;
            if (param.StartDatePoReceipt != null) param.StartDatePoReceipt = param.StartDatePoReceipt;
            if (param.EndDatePoReceipt != null) param.EndDatePoReceipt = param.EndDatePoReceipt;

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateDeliveryDate", param.StartDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateDeliveryDate", param.EndDateDeliveryDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDatePoReceipt", param.StartDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@EndDatePoReceipt", param.EndDatePoReceipt ?? ""));
                parameterList.Add(new SqlParameter("@typeTab", type));
                parameterList.Add(new SqlParameter("@skip", "0"));
                parameterList.Add(new SqlParameter("@take", "0"));
                parameterList.Add(new SqlParameter("@isTotal", "1"));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<TotalSla>(@"exec [dbo].[SP_POIncoming_LIST]
                    @User
                    ,@PoNo
                    ,@StartDateDeliveryDate
                    ,@EndDateDeliveryDate
                    ,@StartDatePoReceipt
                    ,@EndDatePoReceipt
                    ,@typeTab
                    ,@skip
                    ,@take
                    ,@isTotal
                    ", parameters).FirstOrDefault();
                return data == null ? 0 : data.CountTotal;
            }
        }

    }
}
