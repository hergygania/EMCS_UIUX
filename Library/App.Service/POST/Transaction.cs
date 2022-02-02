using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using System.IO;
using System.Web;

namespace App.Service.POST
{
    public static class Transaction
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region Transaction
        public static long UpdateRequest(TrRequest request)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestID", request.ID));
                parameterList.Add(new SqlParameter("@FlowProcessStatusID", request.FlowProcessStatusID));
                parameterList.Add(new SqlParameter("@Comment", request.Comment ?? ""));
                parameterList.Add(new SqlParameter("@UserStamp", SiteConfiguration.UserName ?? ""));
                parameterList.Add(new SqlParameter("@Flowtype", ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[SP_CreateRequestNumber]@RequestID,@FlowProcessStatusID,@Comment,@UserStamp,@Flowtype", parameters).FirstOrDefault();
                return data;
            }
        }
        public static long UpdateRequestPrePayment(TrRequest request)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestID", request.ID));
                parameterList.Add(new SqlParameter("@FlowProcessStatusID", request.FlowProcessStatusID));
                parameterList.Add(new SqlParameter("@Comment", request.Comment ?? ""));
                parameterList.Add(new SqlParameter("@UserStamp", SiteConfiguration.UserName ?? ""));
                parameterList.Add(new SqlParameter("@Flowtype", ""));
                parameterList.Add(new SqlParameter("@PrePayment", request.Prepayment));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[SP_CreateRequestNumberPrePayment]@RequestID,@FlowProcessStatusID,@Comment,@UserStamp,@Flowtype,@PrePayment", parameters).FirstOrDefault();
                return data;
            }
        }
        public static int SendEmail(string type, int idRequest, string idItem)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Type", type ?? ""));
                parameterList.Add(new SqlParameter("@ID", idRequest));
                parameterList.Add(new SqlParameter("@IDItem", idItem));
                SqlParameter[] parameters = parameterList.ToArray();
                //var data = 1;
                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SendingEmail] @Type,@ID,@IDItem", parameters).FirstOrDefault();
                return data;
            }
        }

        public static int RejectPO(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SP_RejectPO_Insert] @id", parameters).FirstOrDefault();
                return data;
            }
        }

        public static CountPODashboard GetDataDashboard()
        {
            var userLogin = SiteConfiguration.UserName;
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", userLogin ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<CountPODashboard>(@"exec [dbo].[SP_CountPODashboard_LIST]
                    @User
                    ", parameters).FirstOrDefault();
                return data;
            }
        }

        public static string SaveLogUser(string user, string role, string usertype, string aspxauth)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                parameterList.Add(new SqlParameter("@role", role ?? ""));
                parameterList.Add(new SqlParameter("@usertype", usertype ?? ""));
                parameterList.Add(new SqlParameter("@aspxauth", aspxauth ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_InputLogUser]
                    @user,@role,@usertype,@aspxauth
                    ", parameters).FirstOrDefault();
                return data;
            }
        }

        public static string UpadateItem(UpdateTrItem item)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", item.id ?? ""));
                parameterList.Add(new SqlParameter("@status", (object)item.status ?? DBNull.Value));
                parameterList.Add(new SqlParameter("@eta", (object)item.eta ?? DBNull.Value));
                parameterList.Add(new SqlParameter("@ata", (object)item.ata ?? DBNull.Value));
                parameterList.Add(new SqlParameter("@etd", (object)item.etd ?? DBNull.Value));
                parameterList.Add(new SqlParameter("@atd", (object)item.atd ?? DBNull.Value));
                parameterList.Add(new SqlParameter("@applyFor", item.applyFor ?? ""));
                parameterList.Add(new SqlParameter("@notes", item.notes ?? ""));
                parameterList.Add(new SqlParameter("@position", item.position ?? ""));
                parameterList.Add(new SqlParameter("@QtyPartial", item.QtyPartial ?? ""));
                parameterList.Add(new SqlParameter("@QtyPartial_Id", item.QtyPartialId ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();
                
                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_TrItem_UPDATE]
                	@id		  
                  , @status  
                  , @eta	  
                  , @ata	 
                  , @etd	  
                  , @atd	
                  , @applyFor 
                  , @notes	
                  ,@position
                  ,@QtyPartial
                  ,@QtyPartial_Id
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        public static string UpadateItemNotes(UpdateTrItem item)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", item.id ?? ""));
                parameterList.Add(new SqlParameter("@applyFor", item.applyFor ?? ""));
                parameterList.Add(new SqlParameter("@notes", item.notes ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_TrItemNotes_UPDATE]
                	@id		  
                  , @applyFor 
                  , @notes	  
                ", parameters).FirstOrDefault();

                return data;
            }
        }
        public static int UpdateInvoice(int id,  string suratketerangan)
        {
           
            using (var db = new Data.POSTContext())
            {
                
                var data = (from p in db.TrPO where p.IdRequest == id select p).FirstOrDefault();
                if (data != null)
                {
                    data.suratketerangantidakpotongpajak = suratketerangan;  
                    db.SaveChanges();
                }
            }
            return 1;
        }
        public static string UpadateItemQtyPartial(string idItem, string qtyPartial)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", idItem));
                parameterList.Add(new SqlParameter("@qtyPartial", qtyPartial ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_TrItemPartialQty_UPDATE]
                	@id		  
                  , @qtyPartial 
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        public static AttachmentTrItem CountRequestAttachmentByType(ParamAttachTrItem item)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", item.id));
                parameterList.Add(new SqlParameter("@Type", item.type ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<AttachmentTrItem>(@"exec [dbo].[SP_CountRequestAttachmentByType_GET]
                	@id		  
                  , @Type  
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        public static int UpdateShipment(int requestId, string shipment)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestId", requestId));
                parameterList.Add(new SqlParameter("@Shipment", shipment));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SP_UpdateShipmentPO_UPDATE]
                	@RequestId		  
                  , @Shipment	  
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        #endregion
        #region Count 
        public static int TaskByUser(string user, int requestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RequestId", requestId));
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SP_TaskByUser_COUNT]
                	@RequestId		  
                	,@User		  
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        #endregion
        #region DataSAP
        public static List<ItemGR> GetDataGRByPO(string poNo, string itemId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@PoNo", poNo ?? ""));
                //parameterList.Add(new SqlParameter("@POItem", itemId ?? ""));
                parameterList.Add(new SqlParameter("@POItem", itemId ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemGR>(@"exec [dbo].[SP_SAPDataGRByPO_GET]
                	@PoNo		  
                	,@POItem		  
                ", parameters).ToList();

                return data;
            }
        }

        public static ValidateClosePO ValidateClosePO(int id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ValidateClosePO>(@"exec [dbo].[SP_ValidateClosePO]
                	@id
                ", parameters).FirstOrDefault();

                return data;
            }
        }

        public static List<ItemInvoice> GetDataInvoiceByPO(string poNo, string itemId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@PoNo", poNo ?? ""));
                parameterList.Add(new SqlParameter("@POItem", itemId ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<ItemInvoice>(@"exec [dbo].[SP_SAPDataInvoiceByPO_GET]
                	@PoNo		  
                	,@POItem		  
                ", parameters).ToList();

                return data;
            }
        }
        #endregion

        #region Attachment

        public static int UpdateItemMappingUploadInvoice(int idItem, int attachmentId, bool selected,int requestId)
        {
            var model = new MappingUploadItem();
            var userLogin = SiteConfiguration.UserName;
            //Itemmapping ItemMap = new Itemmapping();
            using (var db = new Data.POSTContext())
            {
                if (selected)
                {
                    var dataitem = (from r in db.MappingUploadItem
                                    join p in db.TrRequestAttachment on r.AttachmentID equals p.ID
                                    where p.RequestID == requestId && p.ItemID == idItem && p.CodeAttachment == "BAST"
                                    select new Itemmapping
                                    {
                                        itemId = r.ItemID,
                                        AttachmentId = p.ID
                                    }).AsQueryable().ToList();


                    foreach (var n in dataitem)
                    {
                        model.AttachmentID = attachmentId;
                        model.ItemID = idItem;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = userLogin;
                        db.MappingUploadItem.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var data = (from p in db.MappingUploadItem where p.ItemID == idItem && p.AttachmentID == attachmentId select p).FirstOrDefault();
                    db.MappingUploadItem.Remove(data);
                    db.SaveChanges();
                }
            }
            return 1;
        }


        public static int UpdateItemMappingUpload(int idItem, int attachmentId, bool selected)
        {
            var model = new MappingUploadItem();
            var userLogin = SiteConfiguration.UserName;
            using (var db = new Data.POSTContext())
            {
                if (selected)
                {
                    var data = (from p in db.MappingUploadItem where p.ItemID == idItem && p.AttachmentID == attachmentId select p).FirstOrDefault();
                    if (data == null)
                    {
                        model.AttachmentID = attachmentId;
                        model.ItemID = idItem;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = userLogin;
                        db.MappingUploadItem.Add(model);
                        db.SaveChanges();
                    }
                }
                else
                {
                    var data = (from p in db.MappingUploadItem where p.ItemID == idItem && p.AttachmentID == attachmentId select p).FirstOrDefault();
                    db.MappingUploadItem.Remove(data);
                    db.SaveChanges();
                }
            }
            return 1;
        }
     
        public static int UpdateItemDocType(int idItem, int attachmentId, string dataselected)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == attachmentId select p).FirstOrDefault();
                data.DocType = dataselected;
                db.SaveChanges();
            }
            return 1;
        }

        public static int UpdateItemPartialForBast(int idItem, int attachmentId, string dataselected)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == attachmentId select p).FirstOrDefault();
                data.QtyPartial = dataselected;
                db.SaveChanges();
            }
            return 1;
        }
               

        public static int RejectAttachment(int attachmentId, bool reject,string rejectnote)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == attachmentId select p).FirstOrDefault();
                data.IsRejected = reject;
                data.IsApprove = false;
                data.rejectnote = rejectnote;
                data.UploadedDate = DateTime.Now;
                db.SaveChanges();
            }
            return 1;
        }

        public static int ApproveAttachment(int attachmentId, bool approve,string role,string note)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == attachmentId select p).FirstOrDefault();
                data.IsRejected = false;

                data.ApproveDateFinance = DateTime.Now;
                if (role == "Administrator,POSTFINANCE" ||  role == "Administrator,POSTFINANCEBRANCH")
                {
                    data.IsApprove = approve;
                    data.Notes += " Finance : " + note;
                }
                else if (role == "Administrator,POSTTAX")
                {
                    data.IsApproveTax = approve;
                    data.Notes += " TAX : " + note;
                }

                db.SaveChanges();
            }
            return 1;
        }

        public static int UpdateAttachItemProgressPercent(string progress, int attachmentId, string type)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == attachmentId select p).FirstOrDefault();
                if (data != null)
                {
                    data.Progress = progress;
                    db.SaveChanges();
                }


                if (type == "BAST")
                {
                    var dataItem = (from p in db.MappingUploadItem where p.AttachmentID == attachmentId select p).ToList();
                    foreach (var p in dataItem)
                    {
                        using (var dbs = new Data.RepositoryFactory(new Data.POSTContext()))
                        {
                            dbs.DbContext.Database.CommandTimeout = 600;
                            List<SqlParameter> parameterList = new List<SqlParameter>();
                            parameterList.Add(new SqlParameter("@id", p.ItemID.ToString() ?? ""));
                            parameterList.Add(new SqlParameter("@applyFor", "ITEM"));
                            parameterList.Add(new SqlParameter("@position", progress));
                            SqlParameter[] parameters = parameterList.ToArray();

                            var datas = dbs.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_TrItemPosition_UPDATE]
                	        @id		  
                	        ,@applyFor		  
                	        ,@position		  
                        ", parameters).ToList();
                            Console.Write(datas.Count);
                        }
                    }
                }
            }
            return 1;
        }

        public static long SaveFileAttachmentRequest(int requestId, string fileNameOri, HttpPostedFileBase file, string codeAttachment)
        {
            var model = new TrRequestAttachment();
            var path = Global.GetParameterByName("PATH_ATTACHMENT");
            var userLogin = SiteConfiguration.UserName;
            var fileName = fileNameOri;
            //var FileNameKOFAX = "";
            var PO_Number = "";
            var Destination = "";
            var BusinessArea = "";

            using (var db = new Data.POSTContext())
            {
                var ItemID = 0;
                int FlowProcessID = 0;
                int FlowProcessStatusID = 0;
                var Approved = false;
                if (codeAttachment == "BAST" || codeAttachment == "INVOICE")
                {
                    var DataItem = PO.GetItemListById(requestId);
                    requestId = (int)DataItem.Request_Id;
                    ItemID = (int)DataItem.Item_Id;
                    PO_Number = DataItem.Po_Number;
                    Destination = DataItem.Destination;
                    BusinessArea = DataItem.BusinessArea;

                    if (codeAttachment == "BAST") {
                        Approved = true;
                        FlowProcessID = 1665;
                        FlowProcessStatusID = 2938;

                    }
                    else if (codeAttachment == "INVOICE")
                    {
                        FlowProcessID = 1667;
                        FlowProcessStatusID = 3217;

                    }
                }

                model.RequestID = requestId;
                model.ItemID = ItemID;
                model.Path = path;
                model.FileName = fileName;
                model.FileNameOri = Path.GetFileName(fileNameOri);
                model.CodeAttachment = codeAttachment;
                model.IsActive = true;
                model.UploadedDate = DateTime.Now;
                model.UploadedBy = userLogin;
                model.Name = "";
                model.Progress = "";
                model.IsApprove = Approved;
                model.IsApproveTax = false;
                model.IsRejected = false;
                model.FlowID = 1072;
                model.FlowProcessID = FlowProcessID;
                model.FlowProcessStatusID = FlowProcessStatusID;

                db.TrRequestAttachment.Add(model);
                db.SaveChanges();


                fileName = model.CodeAttachment + "_" + model.ID + "_" + fileNameOri;
                path = Global.CreateShareFolderRequest(path, model.UploadedDate, model.RequestID);
                Global.SaveFileToShareFolderRequest(path, fileName, file);

              


                model.FileName = fileName;
                model.Path = path + fileName;
                //model.FileNameKOFAX = FileNameKOFAX;
                db.SaveChanges();

                if (codeAttachment == "BAST" )
                {
                    UpdateItemMappingUpload(ItemID, (int)model.ID, true);
                }
                else if (codeAttachment == "INVOICE")
                {
                    UpdateItemMappingUpload(ItemID, (int)model.ID, true);

                    //SEND TO SHARE FOLDER KOFAX
                    //FileNameKOFAX = "POST_BusArea_PONo_xxxxx.pdf";
                    //FileNameKOFAX = "POST" + "_" + BusinessArea + "_" + PO_Number + ".pdf";

                    //path = Global.CreateShareFolderRequest(path, model.UploadedDate, model.RequestID);
                    //Global.SaveFileToShareFolderRequest(path, FileNameKOFAX, file);
                }

                return model.ID;
            }
        }

        public static TrRequestAttachment GetDataRequestAttachmentById(int id)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == id select p).FirstOrDefault();
                return data;
            }
        }
        public static List<TrRequestAttachment> GetDataRequestAttachmentByRequestId(int requestid)
        {
            List<TrRequestAttachment> DataAttachment = new List<TrRequestAttachment>();
            using (var db = new Data.POSTContext())
            {
                DataAttachment = (from p in db.TrRequestAttachment where p.RequestID == requestid orderby p.FileNameOri select p ).ToList();
                return DataAttachment;
            }
        }
        public static TrRequestAttachment UpdateNameAttachment(int id, string name)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == id select p).FirstOrDefault();

                data.Name = name;
                data.UploadedDate = DateTime.Now;
                db.SaveChanges();

                return data;
            }
        }

        public static TrRequestAttachment UpdateAttachmentNotes(int id, string notes)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == id select p).FirstOrDefault();
                //data.Notes = notes;
                data.Notes = notes;
                data.UploadedDate = DateTime.Now;
                db.SaveChanges();

                return data;
            }
        }

        public static void DeleteRequestAttachment(int? id)
        {
            using (var db = new Data.POSTContext())
            {
                var data = (from p in db.TrRequestAttachment where p.ID == id select p).FirstOrDefault();
                if (data == null) return;
                data.IsActive = false;
                db.TrRequestAttachment.Remove(data);
                db.SaveChanges();


                var dataMapping = (from p in db.MappingUploadItem where p.AttachmentID == id select p).ToList();
                db.MappingUploadItem.RemoveRange(dataMapping);
                db.SaveChanges();


                var path = Global.GetParameterByName("PATH_ATTACHMENT");
                File.Delete(path + Path.DirectorySeparatorChar + data.RequestID + Path.DirectorySeparatorChar + data.FileName);
            }
        }
        #endregion
        #region Popup
        public static PopUp CheckPopUp( string vendorid)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();                
                parameterList.Add(new SqlParameter("@vendorid", vendorid));               
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PopUp>(@"exec [dbo].[SP_CheckPopUp]
                	@vendorid		  
                ", parameters).FirstOrDefault();

                return data;
            }
        }
        public static PopUp CheckPopUpInvoice(string vendorid,string PoNo)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@vendorid", vendorid));
                parameterList.Add(new SqlParameter("@PoNo", PoNo));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PopUp>(@"exec [dbo].[SP_CheckPopUpInvoice]
                	@vendorid,@PoNo		  
                ", parameters).FirstOrDefault();

                return data;
            }
        }
        public static PopUp SavePopUpInvoice(string vendorid,Boolean isChecked,string description,string PoNo)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@vendorid", vendorid));
                parameterList.Add(new SqlParameter("@ischecked", isChecked));
                parameterList.Add(new SqlParameter("@description", description));
                parameterList.Add(new SqlParameter("@PoNo", PoNo));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PopUp>(@"exec [dbo].[SP_UpdatePopUpInvoice_UPDATE]
                	@vendorid,@ischecked,@description,@PoNo
                ", parameters).FirstOrDefault();

                return data;
            }            
        }
        public static PopUp SavePopUp(string vendorid, Boolean isChecked, string description)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@vendorid", vendorid));
                parameterList.Add(new SqlParameter("@ischecked", isChecked));
                parameterList.Add(new SqlParameter("@description", description));
            
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<PopUp>(@"exec [dbo].[SP_UpdatePopUp_UPDATE]
                	@vendorid,@ischecked,@description
                ", parameters).FirstOrDefault();

                return data;
            }
        }
        #endregion
    }
}
