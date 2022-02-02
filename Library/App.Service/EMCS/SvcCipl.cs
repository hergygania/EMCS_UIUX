using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using System.Dynamic;
using System.Text.RegularExpressions;
using App.Data.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcCipl
    {
        public const string CacheName = "App.EMCS.SvcCipl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<SpCiplList> CiplList(CiplListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                string ConsigneeName = "";
                if (filter.ConsigneeName != null)
                {
                    ConsigneeName = Regex.Replace(filter.ConsigneeName, @"[^0-9a-zA-Z.]+", "");
                }

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();

                parameterList.Add(new SqlParameter("@ConsigneeName", ConsigneeName ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpCiplList>(@"[dbo].[SP_CiplGetList] @ConsigneeName, @CreateBy", parameters).ToList();
                return data;
            }
        }

        public static bool CiplHisOwned(long id, string userId)
        {
            using (var db = new Data.EmcsContext())
            {
                var result = false;

                var tb = db.CiplData.FirstOrDefault(a => a.Id == id && a.CreateBy == userId);
                if (tb != null)
                {
                    result = true;
                }

                return result;
            }
        }

        public static Cipl CiplGetById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Cipl>("[dbo].[SP_CiplGetById] @id", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        public static CiplForwader CiplForwaderGetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CiplForwaders.Where(a => a.IdCipl == id);
                return data.FirstOrDefault();
            }
        }

        public static List<Documents> CiplDocumentsGetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Documents.Where(a => a.IdRequest == id && a.IsDelete == false);
                return data.ToList();
            }
        }

        public static List<SelectItem2> GetCurrencyCipl()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterKurs.Select(i => new
                {
                    id = i.Curr,
                    text = i.Curr
                }).Distinct().OrderBy(i => i.text).ToList();

                return data.Select(i => new SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<SelectItem> GetSelectList(string group)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter.Where(i => i.Group.ToUpper().Equals(@group.ToUpper())).OrderBy(i => i.Sort).Select(i => new
                {
                    id = i.Value,
                    text = i.Name
                }).ToList();

                return data.Select(i => new SelectItem { Id = int.Parse(i.id), Text = i.text }).ToList();
            }
        }

        public static List<CiplItem> CiplItemGetById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<CiplItem>("[dbo].[SP_CiplItemGetById] @id", parameters).ToList();
                return data;
            }
        }

        public static Reference CiplItemGetByIdReference(long idReference)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.Reference.Where(a => a.Id == idReference).AsQueryable().OrderBy(a => a.Id);
                return tb.FirstOrDefault();
            }
        }

        public static List<Reference> GetCategoryItem(string item)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.Reference.Where(a => a.Category == item).AsQueryable().OrderBy(a => a.Id);
                return tb.ToList();
            }
        }

        public static bool InsertCiplItem(List<CiplItemInsert> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                for (var j = 0; j < data.Count; j++)
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", data[j].IdCipl));
                    parameterList.Add(new SqlParameter("@IdReference", data[j].Id));
                    parameterList.Add(new SqlParameter("@ReferenceNo", data[j].ReferenceNo ?? ""));
                    parameterList.Add(new SqlParameter("@IdCustomer", data[j].IdCustomer ?? ""));
                    parameterList.Add(new SqlParameter("@Name", data[j].Name ?? ""));
                    parameterList.Add(new SqlParameter("@Uom", data[j].UnitUom ?? ""));
                    parameterList.Add(new SqlParameter("@PartNumber", data[j].PartNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Sn", data[j].Sn ?? ""));
                    parameterList.Add(new SqlParameter("@JCode", data[j].JCode ?? ""));
                    parameterList.Add(new SqlParameter("@Ccr", data[j].Ccr ?? ""));
                    parameterList.Add(new SqlParameter("@CaseNumber", data[j].CaseNumber ?? ""));
                    parameterList.Add(new SqlParameter("@ASNNumber", data[j].ASNNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Type", data[j].Type ?? ""));
                    parameterList.Add(new SqlParameter("@IdNo", data[j].IdNo ?? ""));
                    parameterList.Add(new SqlParameter("@YearMade", data[j].YearMade ?? 0));
                    parameterList.Add(new SqlParameter("@Quantity", data[j].AvailableQuantity));
                    parameterList.Add(new SqlParameter("@UnitPrice", data[j].UnitPrice));
                    parameterList.Add(new SqlParameter("@ExtendedValue", data[j].Quantity * data[j].UnitPrice));
                    parameterList.Add(new SqlParameter("@Length", data[j].Length ?? 0));
                    parameterList.Add(new SqlParameter("@Width", data[j].Width ?? 0));
                    parameterList.Add(new SqlParameter("@Height", data[j].Height ?? 0));
                    parameterList.Add(new SqlParameter("@Volume", data[j].Volume ?? 0));
                    parameterList.Add(new SqlParameter("@GrossWeight", data[j].GrossWeight ?? 0));
                    parameterList.Add(new SqlParameter("@NetWeight", data[j].NetWeight ?? 0));
                    parameterList.Add(new SqlParameter("@Currency", data[j].Currency ?? ""));
                    parameterList.Add(new SqlParameter("@CoO", data[j].CoO ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                    parameterList.Add(new SqlParameter("@UpdateDate", ""));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@IdItem", data[j].IdItem));
                    parameterList.Add(new SqlParameter("@IdParent", data[j].IdParent));
                    parameterList.Add(new SqlParameter("@SIBNumber", data[j].SibNumber ?? ""));
                    parameterList.Add(new SqlParameter("@WONumber", data[j].WoNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Claim", data[j].Claim ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplItemInsert] @IdCipl, @IdReference, @ReferenceNo, @IdCustomer, @Name, @Uom, @PartNumber, @Sn, @JCode, @Ccr, @CaseNumber, @Type, @IdNo, @YearMade, @Quantity, @UnitPrice, @ExtendedValue, @Length, @Width, @Height, @Volume, @GrossWeight, @NetWeight, @Currency, @CoO, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdItem, @IdParent, @SIBNumber, @WONumber, @Claim, @ASNNumber", parameters);
                }
                return true;
            }
        }

        public static bool InsertCiplDocument(List<CiplDocumentInsert> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                for (var j = 0; j < data.Count; j++)
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", data[j].Id));
                    parameterList.Add(new SqlParameter("@IdCipl", data[j].IdCipl));
                    parameterList.Add(new SqlParameter("@DocumentDate", data[j].DocumentDate));
                    parameterList.Add(new SqlParameter("@DocumentName", data[j].DocumentName ?? ""));
                    parameterList.Add(new SqlParameter("@Filename", data[j].Filename ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                    parameterList.Add(new SqlParameter("@UpdateDate", ""));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplDocumentInsert] @Id, @IdCipl, @DocumentDate, @DocumentName, @Filename, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate ,@IsDelete", parameters);
                }
                return true;
            }
        }

        public static bool UpdateFileCiplDocument(long id, string filename)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                parameterList.Add(new SqlParameter("@Filename", filename));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                SqlParameter[] parameters = parameterList.ToArray();
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplDocumentUpdateFile] @Id, @Filename, @UpdateBy", parameters);
            }
            return true;
        }

        // ReSharper disable once IdentifierTypo
        public static long ReferenceToCiplItem(List<ReferenceToCiplItem> data, long idCipl)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                for (var j = 0; j < data.Count; j++)
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idCipl));
                    parameterList.Add(new SqlParameter("@IdReference", data[j].Id));
                    parameterList.Add(new SqlParameter("@ReferenceNo", ""));
                    parameterList.Add(new SqlParameter("@IdCustomer", data[j].IdCustomer ?? ""));
                    parameterList.Add(new SqlParameter("@Name", data[j].UnitName));
                    parameterList.Add(new SqlParameter("@Uom", data[j].UnitUom ?? ""));
                    parameterList.Add(new SqlParameter("@PartNumber", data[j].PartNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Sn", data[j].UnitSn ?? ""));
                    parameterList.Add(new SqlParameter("@JCode", data[j].JCode ?? ""));
                    parameterList.Add(new SqlParameter("@Ccr", data[j].Ccr ?? ""));
                    parameterList.Add(new SqlParameter("@CaseNumber", data[j].CaseNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Type", ""));
                    parameterList.Add(new SqlParameter("@IdNo", data[j].IDNo ?? ""));
                    parameterList.Add(new SqlParameter("@YearMade", data[j].YearMade ?? ""));
                    parameterList.Add(new SqlParameter("@Quantity", data[j].Quantity));
                    parameterList.Add(new SqlParameter("@UnitPrice", data[j].UnitPrice));
                    parameterList.Add(new SqlParameter("@ExtendedValue", data[j].ExtendedValue));
                    parameterList.Add(new SqlParameter("@Length", data[j].Length));
                    parameterList.Add(new SqlParameter("@Width", data[j].Width));
                    parameterList.Add(new SqlParameter("@Height", data[j].Height));
                    parameterList.Add(new SqlParameter("@Volume", data[j].Volume));
                    parameterList.Add(new SqlParameter("@GrossWeight", data[j].GrossWeight));
                    parameterList.Add(new SqlParameter("@NetWeight", data[j].NetWeight));
                    parameterList.Add(new SqlParameter("@Currency", data[j].Currency ?? ""));
                    parameterList.Add(new SqlParameter("@CoO", data[j].CoO ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@IdItem", data[j].Id));
                    parameterList.Add(new SqlParameter("@IdParent", 0));
                    parameterList.Add(new SqlParameter("@SIBNumber", data[j].SibNumber ?? ""));
                    parameterList.Add(new SqlParameter("@WONumber", data[j].WoNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Claim", data[j].Claim ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplItemInsert] @IdCipl, @IdReference, @ReferenceNo, @IdCustomer, @Name, @Uom, @PartNumber, @Sn, @JCode, @Ccr, @CaseNumber, @Type, @IdNo, @YearMade, @Quantity, @UnitPrice, @ExtendedValue, @Length, @Width, @Height, @Volume, @GrossWeight, @NetWeight, @Currency, @CoO, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdItem, @IdParent, @SIBNumber, @WONumber, @Claim", parameters);
                }
                return 1;
            }
        }

        public static bool CiplItemCancel(long idCipl)
        {
            using (new Data.EmcsContext())
            {
                return true;
            }
        }

        public static List<ReturnSpInsert> InsertCipl(CiplForwader forwader, Cipl cipl, string dml, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Category", cipl.Category ?? ""));
                parameterList.Add(new SqlParameter("@CategoriItem", cipl.CategoriItem ?? ""));
                parameterList.Add(new SqlParameter("@ExportType", cipl.ExportType ?? ""));
                parameterList.Add(new SqlParameter("@ExportTypeItem", cipl.ExportTypeItem ?? ""));
                parameterList.Add(new SqlParameter("@SoldConsignee", cipl.SoldConsignee ?? ""));
                parameterList.Add(new SqlParameter("@SoldToName", cipl.SoldToName ?? ""));
                parameterList.Add(new SqlParameter("@SoldToAddress", cipl.SoldToAddress ?? ""));
                parameterList.Add(new SqlParameter("@SoldToCountry", cipl.SoldToCountry ?? ""));
                parameterList.Add(new SqlParameter("@SoldToTelephone", cipl.SoldToTelephone ?? ""));
                parameterList.Add(new SqlParameter("@SoldToFax", cipl.SoldToFax ?? ""));
                parameterList.Add(new SqlParameter("@SoldToPic", cipl.SoldToPic ?? ""));
                parameterList.Add(new SqlParameter("@SoldToEmail", cipl.SoldToEmail ?? ""));
                parameterList.Add(new SqlParameter("@ShipDelivery", cipl.ShipDelivery ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeName", cipl.ConsigneeName ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeAddress", cipl.ConsigneeAddress ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeCountry", cipl.ConsigneeCountry ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeTelephone", cipl.ConsigneeTelephone ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeFax", cipl.ConsigneeFax ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneePic", cipl.ConsigneePic ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeEmail", cipl.ConsigneeEmail.Replace(",", ";") ?? ""));
                parameterList.Add(new SqlParameter("@NotifyName", cipl.NotifyName ?? ""));
                parameterList.Add(new SqlParameter("@NotifyAddress", cipl.NotifyAddress ?? ""));
                parameterList.Add(new SqlParameter("@NotifyCountry", cipl.NotifyCountry ?? ""));
                parameterList.Add(new SqlParameter("@NotifyTelephone", cipl.NotifyTelephone ?? ""));
                parameterList.Add(new SqlParameter("@NotifyFax", cipl.NotifyFax ?? ""));
                parameterList.Add(new SqlParameter("@NotifyPic", cipl.NotifyPic ?? ""));
                parameterList.Add(new SqlParameter("@NotifyEmail", cipl.NotifyEmail.Replace(",", ";") ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeSameSoldTo", cipl.ConsigneeSameSoldTo));
                parameterList.Add(new SqlParameter("@NotifyPartySameConsignee", cipl.NotifyPartySameConsignee));
                parameterList.Add(new SqlParameter("@Area", cipl.Area ?? ""));
                parameterList.Add(new SqlParameter("@Branch", cipl.Branch ?? ""));
                parameterList.Add(new SqlParameter("@Currency", cipl.Currency ?? ""));
                parameterList.Add(new SqlParameter("@Rate", cipl.Rate));
                parameterList.Add(new SqlParameter("@PaymentTerms", cipl.PaymentTerms ?? ""));
                parameterList.Add(new SqlParameter("@ShippingMethod", cipl.ShippingMethod ?? ""));
                parameterList.Add(new SqlParameter("@CountryOfOrigin", cipl.CountryOfOrigin ?? ""));
                parameterList.Add(new SqlParameter("@LcNoDate", cipl.LcNoDate ?? ""));
                parameterList.Add(new SqlParameter("@IncoTerm", cipl.IncoTerm ?? ""));
                parameterList.Add(new SqlParameter("@FreightPayment", cipl.FreightPayment ?? ""));
                parameterList.Add(new SqlParameter("@ShippingMarks", cipl.ShippingMarks ?? ""));
                parameterList.Add(new SqlParameter("@Remarks", cipl.Remarks ?? ""));
                parameterList.Add(new SqlParameter("@SpecialInstruction", cipl.SpecialInstruction ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateDate", ""));
                parameterList.Add(new SqlParameter("@Status", status));
                parameterList.Add(new SqlParameter("@IsDelete", false));
                parameterList.Add(new SqlParameter("@LoadingPort", cipl.LoadingPort ?? ""));
                parameterList.Add(new SqlParameter("@DestinationPort", cipl.DestinationPort ?? ""));
                parameterList.Add(new SqlParameter("@PickUpPic", cipl.PickUpPic ?? ""));
                parameterList.Add(new SqlParameter("@PickUpArea", cipl.PickUpArea ?? ""));
                parameterList.Add(new SqlParameter("@CategoryReference", cipl.CategoryReference ?? ""));
                parameterList.Add(new SqlParameter("@ReferenceNo", cipl.ReferenceNo ?? ""));
                parameterList.Add(new SqlParameter("@Consolidate", cipl.Consolidate));
                // FORWADER
                parameterList.Add(new SqlParameter("@Forwader", forwader.Forwader ?? ""));
                parameterList.Add(new SqlParameter("@BranchForwarder", forwader.Branch ?? ""));
                parameterList.Add(new SqlParameter("@Attention", forwader.Attention ?? ""));
                parameterList.Add(new SqlParameter("@Company", forwader.Company ?? ""));
                parameterList.Add(new SqlParameter("@SubconCompany", forwader.SubconCompany ?? ""));
                parameterList.Add(new SqlParameter("@Address", forwader.Address ?? ""));
                parameterList.Add(new SqlParameter("@AreaForwarder", forwader.Area ?? ""));
                parameterList.Add(new SqlParameter("@City", forwader.City ?? ""));
                parameterList.Add(new SqlParameter("@PostalCode", forwader.PostalCode ?? ""));
                parameterList.Add(new SqlParameter("@Contact", forwader.Contact ?? ""));
                parameterList.Add(new SqlParameter("@FaxNumber", forwader.FaxNumber ?? ""));
                parameterList.Add(new SqlParameter("@Forwading", forwader.Forwading ?? ""));
                parameterList.Add(new SqlParameter("@Email", forwader.Email.Replace(",", ";") ?? ""));
<<<<<<< HEAD
<<<<<<< HEAD
                parameterList.Add(new SqlParameter("@Type", forwader.Type ?? ""));
                parameterList.Add(new SqlParameter("@ExportShipmentType", forwader.ExportShipmentType ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<ReturnSpInsert>(" exec [dbo].[SP_CiplInsert] @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email,@Type,@ExportShipmentType", parameters).ToList();
=======

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<ReturnSpInsert>(" exec [dbo].[SP_CiplInsert] @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email", parameters).ToList();
>>>>>>> 639d8d0 (Intial commit)
=======
                parameterList.Add(new SqlParameter("@Type", forwader.Type ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<ReturnSpInsert>(" exec [dbo].[SP_CiplInsert] @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email,@Type", parameters).ToList();
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
                return data;
            }
        }

<<<<<<< HEAD

        public static long UpdateCipl(CiplForwader forwader, Cipl cipl, string status)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", cipl.Id));
                    parameterList.Add(new SqlParameter("@Category", cipl.Category ?? ""));
                    parameterList.Add(new SqlParameter("@CategoriItem", cipl.CategoriItem ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", cipl.ExportType ?? ""));
                    parameterList.Add(new SqlParameter("@ExportTypeItem", cipl.ExportTypeItem ?? ""));
                    parameterList.Add(new SqlParameter("@SoldConsignee", cipl.SoldConsignee ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToName", cipl.SoldToName ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToAddress", cipl.SoldToAddress ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToCountry", cipl.SoldToCountry ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToTelephone", cipl.SoldToTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToFax", cipl.SoldToFax ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToPic", cipl.SoldToPic ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToEmail", cipl.SoldToEmail.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@ShipDelivery", cipl.ShipDelivery ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeName", cipl.ConsigneeName ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeAddress", cipl.ConsigneeAddress ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeCountry", cipl.ConsigneeCountry ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeTelephone", cipl.ConsigneeTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeFax", cipl.ConsigneeFax ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneePic", cipl.ConsigneePic ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeEmail", cipl.ConsigneeEmail ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyName", cipl.NotifyName ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyAddress", cipl.NotifyAddress ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyCountry", cipl.NotifyCountry ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyTelephone", cipl.NotifyTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyFax", cipl.NotifyFax ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyPic", cipl.NotifyPic ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyEmail", cipl.NotifyEmail.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeSameSoldTo", cipl.ConsigneeSameSoldTo));
                    parameterList.Add(new SqlParameter("@NotifyPartySameConsignee", cipl.NotifyPartySameConsignee));
                    parameterList.Add(new SqlParameter("@Area", cipl.Area ?? ""));
                    parameterList.Add(new SqlParameter("@Branch", cipl.Branch ?? ""));
                    parameterList.Add(new SqlParameter("@Currency", cipl.Currency ?? ""));
                    parameterList.Add(new SqlParameter("@Rate", cipl.Rate));
                    parameterList.Add(new SqlParameter("@PaymentTerms", cipl.PaymentTerms ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMethod", cipl.ShippingMethod ?? ""));
                    parameterList.Add(new SqlParameter("@CountryOfOrigin", cipl.CountryOfOrigin ?? ""));
                    parameterList.Add(new SqlParameter("@LcNoDate", cipl.LcNoDate ?? ""));
                    parameterList.Add(new SqlParameter("@IncoTerm", cipl.IncoTerm ?? ""));
                    parameterList.Add(new SqlParameter("@FreightPayment", cipl.FreightPayment ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMarks", cipl.ShippingMarks ?? ""));
                    parameterList.Add(new SqlParameter("@Remarks", cipl.Remarks ?? ""));
                    parameterList.Add(new SqlParameter("@SpecialInstruction", cipl.SpecialInstruction ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", DBNull.Value));
                    parameterList.Add(new SqlParameter("@CreateDate", DBNull.Value));
                    parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@Status", status));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@LoadingPort", cipl.LoadingPort ?? ""));
                    parameterList.Add(new SqlParameter("@DestinationPort", cipl.DestinationPort ?? ""));
                    parameterList.Add(new SqlParameter("@PickUpPic", cipl.PickUpPic ?? ""));
                    parameterList.Add(new SqlParameter("@PickUpArea", cipl.PickUpArea ?? ""));
                    parameterList.Add(new SqlParameter("@CategoryReference", cipl.CategoryReference ?? ""));
                    parameterList.Add(new SqlParameter("@ReferenceNo", cipl.ReferenceNo ?? ""));
                    parameterList.Add(new SqlParameter("@Consolidate", cipl.Consolidate));
                    // FORWADER
                    parameterList.Add(new SqlParameter("@Forwader", forwader.Forwader ?? ""));
                    parameterList.Add(new SqlParameter("@BranchForwarder", forwader.Branch ?? ""));
                    parameterList.Add(new SqlParameter("@Attention", forwader.Attention ?? ""));
                    parameterList.Add(new SqlParameter("@Company", forwader.Company ?? ""));
                    parameterList.Add(new SqlParameter("@SubconCompany", forwader.SubconCompany ?? ""));
                    parameterList.Add(new SqlParameter("@Address", forwader.Address ?? ""));
                    parameterList.Add(new SqlParameter("@AreaForwarder", forwader.Area ?? ""));
                    parameterList.Add(new SqlParameter("@City", forwader.City ?? ""));
                    parameterList.Add(new SqlParameter("@PostalCode", forwader.PostalCode ?? ""));
                    parameterList.Add(new SqlParameter("@Contact", forwader.Contact ?? ""));
                    parameterList.Add(new SqlParameter("@FaxNumber", forwader.FaxNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Forwading", forwader.Forwading ?? ""));
                    parameterList.Add(new SqlParameter("@Email", forwader.Email.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@Type", forwader.Type ?? ""));
                    parameterList.Add(new SqlParameter("@ExportShipmentType", forwader.ExportShipmentType ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplUpdate] @id, @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email,@Type,@ExportShipmentType", parameters);
                    return 1;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static long UpdateCiplByApprover(CiplForwader forwader, Cipl cipl, string status)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", cipl.Id));
                    parameterList.Add(new SqlParameter("@Category", cipl.Category ?? ""));
                    parameterList.Add(new SqlParameter("@CategoriItem", cipl.CategoriItem ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", cipl.ExportType ?? ""));
                    parameterList.Add(new SqlParameter("@ExportTypeItem", cipl.ExportTypeItem ?? ""));
                    parameterList.Add(new SqlParameter("@SoldConsignee", cipl.SoldConsignee ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToName", cipl.SoldToName ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToAddress", cipl.SoldToAddress ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToCountry", cipl.SoldToCountry ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToTelephone", cipl.SoldToTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToFax", cipl.SoldToFax ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToPic", cipl.SoldToPic ?? ""));
                    parameterList.Add(new SqlParameter("@SoldToEmail", cipl.SoldToEmail.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@ShipDelivery", cipl.ShipDelivery ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeName", cipl.ConsigneeName ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeAddress", cipl.ConsigneeAddress ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeCountry", cipl.ConsigneeCountry ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeTelephone", cipl.ConsigneeTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeFax", cipl.ConsigneeFax ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneePic", cipl.ConsigneePic ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeEmail", cipl.ConsigneeEmail ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyName", cipl.NotifyName ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyAddress", cipl.NotifyAddress ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyCountry", cipl.NotifyCountry ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyTelephone", cipl.NotifyTelephone ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyFax", cipl.NotifyFax ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyPic", cipl.NotifyPic ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyEmail", cipl.NotifyEmail.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@ConsigneeSameSoldTo", cipl.ConsigneeSameSoldTo));
                    parameterList.Add(new SqlParameter("@NotifyPartySameConsignee", cipl.NotifyPartySameConsignee));
                    parameterList.Add(new SqlParameter("@Area", cipl.Area ?? ""));
                    parameterList.Add(new SqlParameter("@Branch", cipl.Branch ?? ""));
                    parameterList.Add(new SqlParameter("@Currency", cipl.Currency ?? ""));
                    parameterList.Add(new SqlParameter("@Rate", cipl.Rate));
                    parameterList.Add(new SqlParameter("@PaymentTerms", cipl.PaymentTerms ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMethod", cipl.ShippingMethod ?? ""));
                    parameterList.Add(new SqlParameter("@CountryOfOrigin", cipl.CountryOfOrigin ?? ""));
                    parameterList.Add(new SqlParameter("@LcNoDate", cipl.LcNoDate ?? ""));
                    parameterList.Add(new SqlParameter("@IncoTerm", cipl.IncoTerm ?? ""));
                    parameterList.Add(new SqlParameter("@FreightPayment", cipl.FreightPayment ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMarks", cipl.ShippingMarks ?? ""));
                    parameterList.Add(new SqlParameter("@Remarks", cipl.Remarks ?? ""));
                    parameterList.Add(new SqlParameter("@SpecialInstruction", cipl.SpecialInstruction ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", DBNull.Value));
                    parameterList.Add(new SqlParameter("@CreateDate", DBNull.Value));
                    parameterList.Add(new SqlParameter("@UpdateBy", "Ariliago"));
                    parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@Status", status));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@LoadingPort", cipl.LoadingPort ?? ""));
                    parameterList.Add(new SqlParameter("@DestinationPort", cipl.DestinationPort ?? ""));
                    parameterList.Add(new SqlParameter("@PickUpPic", cipl.PickUpPic ?? ""));
                    parameterList.Add(new SqlParameter("@PickUpArea", cipl.PickUpArea ?? ""));
                    parameterList.Add(new SqlParameter("@CategoryReference", cipl.CategoryReference ?? ""));
                    parameterList.Add(new SqlParameter("@ReferenceNo", cipl.ReferenceNo ?? ""));
                    parameterList.Add(new SqlParameter("@Consolidate", cipl.Consolidate));
                    // FORWADER
                    parameterList.Add(new SqlParameter("@Forwader", forwader.Forwader ?? ""));
                    parameterList.Add(new SqlParameter("@BranchForwarder", forwader.Branch ?? ""));
                    parameterList.Add(new SqlParameter("@Attention", forwader.Attention ?? ""));
                    parameterList.Add(new SqlParameter("@Company", forwader.Company ?? ""));
                    parameterList.Add(new SqlParameter("@SubconCompany", forwader.SubconCompany ?? ""));
                    parameterList.Add(new SqlParameter("@Address", forwader.Address ?? ""));
                    parameterList.Add(new SqlParameter("@AreaForwarder", forwader.Area ?? ""));
                    parameterList.Add(new SqlParameter("@City", forwader.City ?? ""));
                    parameterList.Add(new SqlParameter("@PostalCode", forwader.PostalCode ?? ""));
                    parameterList.Add(new SqlParameter("@Contact", forwader.Contact ?? ""));
                    parameterList.Add(new SqlParameter("@FaxNumber", forwader.FaxNumber ?? ""));
                    parameterList.Add(new SqlParameter("@Forwading", forwader.Forwading ?? ""));
                    parameterList.Add(new SqlParameter("@Email", forwader.Email.Replace(",", ";") ?? ""));
                    parameterList.Add(new SqlParameter("@Type", forwader.Type ?? ""));
                    parameterList.Add(new SqlParameter("@ExportShipmentType", forwader.ExportShipmentType ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplUpdate_ByApprover] @id, @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email,@Type,@ExportShipmentType", parameters);
                    return 1;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
=======
        public static long UpdateCipl(CiplForwader forwader, Cipl cipl, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", cipl.Id));
                parameterList.Add(new SqlParameter("@Category", cipl.Category ?? ""));
                parameterList.Add(new SqlParameter("@CategoriItem", cipl.CategoriItem ?? ""));
                parameterList.Add(new SqlParameter("@ExportType", cipl.ExportType ?? ""));
                parameterList.Add(new SqlParameter("@ExportTypeItem", cipl.ExportTypeItem ?? ""));
                parameterList.Add(new SqlParameter("@SoldConsignee", cipl.SoldConsignee ?? ""));
                parameterList.Add(new SqlParameter("@SoldToName", cipl.SoldToName ?? ""));
                parameterList.Add(new SqlParameter("@SoldToAddress", cipl.SoldToAddress ?? ""));
                parameterList.Add(new SqlParameter("@SoldToCountry", cipl.SoldToCountry ?? ""));
                parameterList.Add(new SqlParameter("@SoldToTelephone", cipl.SoldToTelephone ?? ""));
                parameterList.Add(new SqlParameter("@SoldToFax", cipl.SoldToFax ?? ""));
                parameterList.Add(new SqlParameter("@SoldToPic", cipl.SoldToPic ?? ""));
                parameterList.Add(new SqlParameter("@SoldToEmail", cipl.SoldToEmail.Replace(",", ";") ?? ""));
                parameterList.Add(new SqlParameter("@ShipDelivery", cipl.ShipDelivery ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeName", cipl.ConsigneeName ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeAddress", cipl.ConsigneeAddress ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeCountry", cipl.ConsigneeCountry ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeTelephone", cipl.ConsigneeTelephone ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeFax", cipl.ConsigneeFax ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneePic", cipl.ConsigneePic ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeEmail", cipl.ConsigneeEmail ?? ""));
                parameterList.Add(new SqlParameter("@NotifyName", cipl.NotifyName ?? ""));
                parameterList.Add(new SqlParameter("@NotifyAddress", cipl.NotifyAddress ?? ""));
                parameterList.Add(new SqlParameter("@NotifyCountry", cipl.NotifyCountry ?? ""));
                parameterList.Add(new SqlParameter("@NotifyTelephone", cipl.NotifyTelephone ?? ""));
                parameterList.Add(new SqlParameter("@NotifyFax", cipl.NotifyFax ?? ""));
                parameterList.Add(new SqlParameter("@NotifyPic", cipl.NotifyPic ?? ""));
                parameterList.Add(new SqlParameter("@NotifyEmail", cipl.NotifyEmail.Replace(",", ";") ?? ""));
                parameterList.Add(new SqlParameter("@ConsigneeSameSoldTo", cipl.ConsigneeSameSoldTo));
                parameterList.Add(new SqlParameter("@NotifyPartySameConsignee", cipl.NotifyPartySameConsignee));
                parameterList.Add(new SqlParameter("@Area", cipl.Area ?? ""));
                parameterList.Add(new SqlParameter("@Branch", cipl.Branch ?? ""));
                parameterList.Add(new SqlParameter("@Currency", cipl.Currency ?? ""));
                parameterList.Add(new SqlParameter("@Rate", cipl.Rate));
                parameterList.Add(new SqlParameter("@PaymentTerms", cipl.PaymentTerms ?? ""));
                parameterList.Add(new SqlParameter("@ShippingMethod", cipl.ShippingMethod ?? ""));
                parameterList.Add(new SqlParameter("@CountryOfOrigin", cipl.CountryOfOrigin ?? ""));
                parameterList.Add(new SqlParameter("@LcNoDate", cipl.LcNoDate ?? ""));
                parameterList.Add(new SqlParameter("@IncoTerm", cipl.IncoTerm ?? ""));
                parameterList.Add(new SqlParameter("@FreightPayment", cipl.FreightPayment ?? ""));
                parameterList.Add(new SqlParameter("@ShippingMarks", cipl.ShippingMarks ?? ""));
                parameterList.Add(new SqlParameter("@Remarks", cipl.Remarks ?? ""));
                parameterList.Add(new SqlParameter("@SpecialInstruction", cipl.SpecialInstruction ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@CreateDate", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@Status", status));
                parameterList.Add(new SqlParameter("@IsDelete", false));
                parameterList.Add(new SqlParameter("@LoadingPort", cipl.LoadingPort ?? ""));
                parameterList.Add(new SqlParameter("@DestinationPort", cipl.DestinationPort ?? ""));
                parameterList.Add(new SqlParameter("@PickUpPic", cipl.PickUpPic ?? ""));
                parameterList.Add(new SqlParameter("@PickUpArea", cipl.PickUpArea ?? ""));
                parameterList.Add(new SqlParameter("@CategoryReference", cipl.CategoryReference ?? ""));
                parameterList.Add(new SqlParameter("@ReferenceNo", cipl.ReferenceNo ?? ""));
                parameterList.Add(new SqlParameter("@Consolidate", cipl.Consolidate));
                // FORWADER
                parameterList.Add(new SqlParameter("@Forwader", forwader.Forwader ?? ""));
                parameterList.Add(new SqlParameter("@BranchForwarder", forwader.Branch ?? ""));
                parameterList.Add(new SqlParameter("@Attention", forwader.Attention ?? ""));
                parameterList.Add(new SqlParameter("@Company", forwader.Company ?? ""));
                parameterList.Add(new SqlParameter("@SubconCompany", forwader.SubconCompany ?? ""));
                parameterList.Add(new SqlParameter("@Address", forwader.Address ?? ""));
                parameterList.Add(new SqlParameter("@AreaForwarder", forwader.Area ?? ""));
                parameterList.Add(new SqlParameter("@City", forwader.City ?? ""));
                parameterList.Add(new SqlParameter("@PostalCode", forwader.PostalCode ?? ""));
                parameterList.Add(new SqlParameter("@Contact", forwader.Contact ?? ""));
                parameterList.Add(new SqlParameter("@FaxNumber", forwader.FaxNumber ?? ""));
                parameterList.Add(new SqlParameter("@Forwading", forwader.Forwading ?? ""));
                parameterList.Add(new SqlParameter("@Email", forwader.Email.Replace(",", ";") ?? ""));
                parameterList.Add(new SqlParameter("@Type", forwader.Type ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplUpdate] @id, @Category, @CategoriItem, @ExportType, @ExportTypeItem, @SoldConsignee, @SoldToName, @SoldToAddress, @SoldToCountry, @SoldToTelephone, @SoldToFax, @SoldToPic, @SoldToEmail, @ShipDelivery, @ConsigneeName, @ConsigneeAddress, @ConsigneeCountry, @ConsigneeTelephone, @ConsigneeFax, @ConsigneePic, @ConsigneeEmail, @NotifyName, @NotifyAddress, @NotifyCountry, @NotifyTelephone, @NotifyFax, @NotifyPic, @NotifyEmail, @ConsigneeSameSoldTo, @NotifyPartySameConsignee, @Area, @Branch, @Currency, @Rate, @PaymentTerms, @ShippingMethod, @CountryOfOrigin, @LcNoDate, @IncoTerm, @FreightPayment, @ShippingMarks, @Remarks, @SpecialInstruction, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Status, @IsDelete, @LoadingPort, @DestinationPort, @PickUpPic, @PickUpArea, @CategoryReference, @ReferenceNo, @Consolidate, @Forwader, @BranchForwarder, @Attention, @Company, @SubconCompany, @Address, @AreaForwarder, @City, @PostalCode, @Contact, @FaxNumber, @Forwading, @Email,@Type", parameters);
                return 1;
            }
        }

>>>>>>> 639d8d0 (Intial commit)
        public static List<SPGetCiplDocument> CiplDocumentListById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<SPGetCiplDocument>("[dbo].[sp_get_cipl_document_list_byid] @id", parameters).ToList();

                return data;
            }
            //using (var db = new Data.EmcsContext())
            //{
            //    var data = db.CiplD.Where(a => a.IdRequest == id && a.IsDelete == false);
            //    return data.ToList();
            //}
        }

        public static long DeleteCiplDocumentById(long idDocument)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", idDocument));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@IsDelete", true));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplDocumentDelete] @id, @UpdateBy, @UpdateDate, @IsDelete", parameters);
                return 1;
            }
        }

        public static long DeleteCiplById(long idCipl, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", idCipl));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@Status", status));
                parameterList.Add(new SqlParameter("@IsDelete", true));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CiplDelete] @id, @UpdateBy, @UpdateDate, @Status, @IsDelete", parameters);
                return 1;
            }
        }

<<<<<<< HEAD
        public static List<Data.Domain.EMCS.MasterCustomers> GetIdCustomer()
=======
        public static List<MasterCustomers> GetIdCustomer()
>>>>>>> 639d8d0 (Intial commit)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterCustomer.OrderBy(a => a.Id);
                return tb.ToList();
            }

        }

        public static List<SpGetReference> GetReference(SpGetReference crit, string categoryReference)
        {
            using (var db = new Data.EmcsContext())
            {
                var category = crit.Category ?? "";
                var reference = crit.ReferenceNo ?? "";
                var categoryReference2 = categoryReference ?? "";
                var lastReference = crit.LastReference ?? "";
                var idCustomer = crit.IdCustomer ?? "";
<<<<<<< HEAD
                if (category != null)
=======
                if (category!=null)
>>>>>>> 639d8d0 (Intial commit)
                {
                    category = Regex.Replace(category, @"[^0-9a-zA-Z]+", "");
                }
                if (reference != null)
                {
                    reference = Regex.Replace(reference, @"[^0-9a-zA-Z]+", "");
                }
                if (categoryReference2 != null)
                {
                    categoryReference2 = Regex.Replace(categoryReference2, @"[^0-9a-zA-Z]+", "");
                }

<<<<<<< HEAD

=======
               
>>>>>>> 639d8d0 (Intial commit)
                var tb = db.Database.SqlQuery<SpGetReference>("[dbo].[sp_get_reference_no]@Category='" + category + "', @ReferenceNo='" + reference + "', @CategoryReference='" + categoryReference2 + "', @LastReference='" + lastReference + "', @IdCustomer='" + idCustomer + "'").ToList();
                return tb;
            }
        }

        public static dynamic GetReferenceItem(GridListFilter crit, string column, string columnValue, string category)
        {
            using (var db = new Data.EmcsContext())
            {

                category = category ?? "";
                column = column ?? "";
                columnValue = columnValue ?? "";
                string Order = crit.Order ?? "";

                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                column = column ?? "";
                columnValue = columnValue ?? "";
<<<<<<< HEAD
                if (category != null)
=======
                if (category!=null)
>>>>>>> 639d8d0 (Intial commit)
                {
                    category = Regex.Replace(category, @"[^0-9a-zA-Z]+", "");
                }
                if (column != null)
                {
                    column = Regex.Replace(column, @"[^0-9a-zA-Z]+", "");
                }
                if (columnValue != null)
                {
                    columnValue = Regex.Replace(columnValue, @"[^0-9a-zA-Z-, ]+", "");
                }
                if (Order != null)
                {
                    Order = Regex.Replace(Order, @"[^0-9a-zA-Z]+", "");
                }

<<<<<<< HEAD

                var sql = @"[dbo].[sp_get_reference_item] @Column = '" + column + "', @ColumnValue = '" + columnValue + "', @Category = '" + category + "' ";
                //var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(SQL + ", @isTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<ReferenceToCiplItem>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit= 50000").ToList();
                if (data.Count > 0)
                {
                    data.ToList().ForEach(c => c.PartNumber = c.PartNumber.Replace(@":AA", string.Empty));
                }
                dynamic result = new ExpandoObject();

=======
                
                var sql = @"[dbo].[sp_get_reference_item] @Column = '" + column + "', @ColumnValue = '" + columnValue + "', @Category = '" + category + "' ";
                //var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(SQL + ", @isTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<ReferenceToCiplItem>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit= 50000").ToList();

                dynamic result = new ExpandoObject();
>>>>>>> 639d8d0 (Intial commit)
                //result.total = count.total;
                result.rows = data;
                return result;
            }
        }

        public static List<SpGetConsigneeName> GetConsigneeName(string id, string category, string categoryreference)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC sp_get_consignee_name @ReferenceNo ='" + id + "', @Category='" + category + "', @CategoryReference='" + categoryreference + "'";
                var data = db.Database.SqlQuery<SpGetConsigneeName>(sql).ToList();
                return data;
            }
        }

        public static CiplItemCargo GetCiplItemById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                string sql = @"select * from dbo.CiplItem where Id=" + id;
                var table = db.Database.SqlQuery<CiplItemCargo>(sql).FirstOrDefault();
                return table;
            }
        }

        public static decimal GetLastestKurs()
        {
            using (var db = new Data.EmcsContext())
            {
                var table = db.MasterKurs.Where(a => a.Curr == "USD").OrderByDescending(a => a.Id).Select(a => a.Rate).FirstOrDefault();
                return table;
            }
        }
        public static long GetIdCIPL(string CIPLNo)
        {
            using (var db = new Data.EmcsContext())
            {
                var IdCIPL = db.CiplData.Where(a => a.CiplNo == CIPLNo).Select(a => a.Id).FirstOrDefault();
                return IdCIPL;
            }
        }
        public static long GetIdReference(string Refno)
        {
            using (var db = new Data.EmcsContext())
            {
                var IdReference = db.Reference.Where(a => a.ReferenceNo == Refno).Select(a => a.Id).FirstOrDefault();
                return IdReference;
            }
        }
        public static List<SelectItem2> GetCountryList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterCountry.OrderBy(i => i.Description).Select(i => new
                {
                    id = i.Id,
                    text = i.Description
                }).ToList();

                return data.Select(i => new SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<SelectItem2> GetBranchList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterBranch.OrderBy(i => i.BranchDesc).Select(i => new
                {
                    id = i.Id,
                    text = i.BranchDesc
                }).ToList();

                return data.Select(i => new SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static List<SelectItem2> GetCategoryReferencet(string group)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterParameter
                    .Where(i => i.Group.Equals(@group))
                    .OrderBy(i => i.Sort)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Value,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new SelectItem2 { Id = i.id, Text = i.text }).ToList();
            }
        }

        public static List<SelectItem2> GetAreaList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterArea.OrderBy(i => i.BAreaName).Select(i => new
                {
                    id = i.Id,
                    text = i.BAreaName
                }).ToList();

                return data.Select(i => new SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

        public static dynamic GetListSp(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CiplHistoryGetById] @id='" + crit.Term + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @IsTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<SpGetCiplHistory>(sql + ", @IsTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset.ToString() + "', @limit='" + crit.Limit.ToString() + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

<<<<<<< HEAD
        #region RequestForChange
        public static dynamic GetListSpRequestForChangeDetails(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CiplChangeHistoryGetById] @id='" + crit.Term + "', @formtype='" + crit.FormType + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @IsTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<SPGetCiplChangeHistory>(sql + ", @IsTotal=0, @sort='" + crit.Sort + "',  @order='" + crit.Order + "', @offset='" + crit.Offset.ToString() + "', @limit='" + crit.Limit.ToString() + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
        public static dynamic GetListSpRequestForChangeByFormType(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CiplChangeHistoryGetByFormType] @id='" + crit.Term + "', @formtype='" + crit.FormType + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @IsTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<SPGetCiplChangeHistory>(sql + ", @IsTotal=0, @sort='" + crit.Sort + "',  @order='" + crit.Order + "', @offset='" + crit.Offset.ToString() + "', @limit='" + crit.Limit.ToString() + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
        public static dynamic GetRequestForChangeList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                crit.Order = "DESC";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_RequestForChangeHistory]";
                var count = db.Database.SqlQuery<CountData>(sql + " @IsTotal=0,@Approver='" + SiteConfiguration.UserName + "'").FirstOrDefault();
                var data = db.Database.SqlQuery<Sp_RequestForChangeHistory>(sql + "@IsTotal=0, @sort='" + crit.Sort + "',  @order='" + crit.Order + "', @offset='" + crit.Offset.ToString() + "', @limit='" + crit.Limit.ToString() + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
        public static dynamic GetSpChangeHistoryReason(string idTerm, string formtype)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "Select TOP 1 Reason from RequestForChange WHERE  Id = '" + idTerm + "'";
                var data = db.Database.SqlQuery<string>(sql).FirstOrDefault();
                return data;
            }
        }

        public static dynamic CheckRequestExists(int formId, string formtype)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "select TOP 1(Id) from RequestForChange WHERE FormId = '" + formId + "' AND FormType = '" + formtype + "' and [Status] = 0";
                var data = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                return data;
            }
        }
        public static dynamic CheckRequestNotApproved(int Id, string formtype)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "select TOP 1(Id) from RequestForChange WHERE Id = '" + Id + "' AND FormType = '" + formtype + "' and [Status] = 0";
                var data = db.Database.SqlQuery<int>(sql).FirstOrDefault();
                return data;
            }
        }
        public static List<RFCItem> GetRequestForChangeDataList(string idTerm)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "SELECT RF.RFCID,RF.TableName,RF.FieldName,RF.BeforeValue,RF.AfterValue FROM RequestForChange R INNER JOIN RFCItem RF ON R.ID = RF.RFCID WHERE R.[Status] = 0 AND R.Id = '" + idTerm + "'";
                var data = db.Database.SqlQuery<RFCItem>(sql).ToList();
                return data;
            }
        }
        public static long ApproveRequestForChangeHistory(int formId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", formId));
                parameterList.Add(new SqlParameter("@UpdatedBy", SiteConfiguration.UserName));
                SqlParameter[] parameters = parameterList.ToArray();

                db.DbContext.Database.ExecuteSqlCommand("exec [dbo].[SP_ApproveChangeHistory]  @Id,@UpdatedBy", parameters);
                return 1;
            }

            return 0;
        }
        public static long RejectRequestForChangeHistory(int formId, string reason)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", formId));
                parameterList.Add(new SqlParameter("@Reason", reason));
                parameterList.Add(new SqlParameter("@UpdatedBy", SiteConfiguration.UserName));
                SqlParameter[] parameters = parameterList.ToArray();

                db.DbContext.Database.ExecuteSqlCommand("exec [dbo].[SP_RejectChangeHistory]  @Id,@Reason,@UpdatedBy", parameters);
                return 1;
            }

            return 0;
        }
        public static int InsertRequestChangeHistory(Data.Domain.RequestForChange data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@FormType", data.FormType));
                parameterList.Add(new SqlParameter("@FormNo", data.FormNo));
                parameterList.Add(new SqlParameter("@FormId", data.FormId));
                parameterList.Add(new SqlParameter("@Reason", data.Reason ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var obj = db.DbContext.Database.SqlQuery<int>(@" exec [dbo].[Sp_RequestForChange_Insert] @FormType, @FormNo,@FormId, @Reason, @CreateBy", parameters).First();
                return obj;
            }
        }
        public static int InsertChangeHistory(Data.Domain.RequestForChange data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@FormType", data.FormType));
                parameterList.Add(new SqlParameter("@FormNo", data.FormNo));
                parameterList.Add(new SqlParameter("@FormId", data.FormId));
                parameterList.Add(new SqlParameter("@Reason", data.Reason ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var obj = db.DbContext.Database.SqlQuery<int>(@" exec [dbo].[Sp_ChangeHistory_Insert] @FormType, @FormNo,@FormId, @Reason, @CreateBy", parameters).First();
                return obj;
            }
        }
        public static bool InsertRFCItem(List<Data.Domain.RFCItem> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                for (var j = 0; j < data.Count; j++)
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@RFCID", data[j].RFCID));
                    parameterList.Add(new SqlParameter("@TableName", data[j].TableName ?? ""));
                    parameterList.Add(new SqlParameter("@LableName", data[j].LableName ?? ""));
                    parameterList.Add(new SqlParameter("@FieldName", data[j].FieldName ?? ""));
                    parameterList.Add(new SqlParameter("@BeforeValue", data[j].BeforeValue ?? ""));
                    parameterList.Add(new SqlParameter("@AfterValue", data[j].AfterValue ?? ""));

                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion
                    var obj = db.DbContext.Database.SqlQuery<int>(@" exec [dbo].[Sp_RFCItem_Insert] @RFCID, @TableName, @LableName, @FieldName , @BeforeValue , @AfterValue", parameters).First();
                }
                return true;
            }
        }

        #endregion
=======
>>>>>>> 639d8d0 (Intial commit)
        public static dynamic GetListSpDocument(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_CiplDocumentGetById] @id='" + Convert.ToInt64(crit.Term) + "'";
                var count = db.Database.SqlQuery<CountData>(sql + ", @IsTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<SpCiplDocument>(sql + ", @IsTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset.ToString() + "', @limit='" + crit.Limit.ToString() + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static List<SpGetCiplHistory> CiplHistoryGetById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpGetCiplHistory>("[dbo].[SP_CiplHistoryGetById] @id", parameters).ToList();

                return data;
            }
        }

        public static List<SpCiplProblemHistory> SP_CiplProblemHistory(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpCiplProblemHistory>("[dbo].[SP_CiplProblemHistoryGetById] @id", parameters).ToList();

                return data;
            }
        }

        public static bool CrudSp(Cipl itm, CiplApprove status, string dml)
        {
<<<<<<< HEAD
            try
            {

                if (dml == "I")
                {
                    itm.CreateBy = SiteConfiguration.UserName;
                    itm.CreateDate = DateTime.Now;
                }

                itm.UpdateBy = SiteConfiguration.UserName;
                itm.UpdateDate = DateTime.Now;

                CacheManager.Remove(CacheName);

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {

                    db.DbContext.Database.CommandTimeout = 600;
                    var sql = @"[dbo].[sp_update_request_cipl] @IdCipl='" + itm.Id + "'" +
                              "," + "@Username='" + itm.UpdateBy + "'" +
                              ", @NewStatus='" + status.Status + "'" +
                              ", @Notes='" + status.Notes + "'";

                    db.DbContext.Database.ExecuteSqlCommand(sql);
                    return true;
                }

            }
            catch (Exception ex)
            {

                throw ex;
=======
            if (dml == "I")
            {
                itm.CreateBy = SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                db.DbContext.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_update_request_cipl] @IdCipl='" + itm.Id + "'" +
                          "," + "@Username='" + itm.UpdateBy + "'" +
                          ", @NewStatus='" + status.Status + "'" +
                          ", @Notes='" + status.Notes + "'";

                db.DbContext.Database.ExecuteSqlCommand(sql);
                return true;
>>>>>>> 639d8d0 (Intial commit)
            }

        }
        public static bool removeCiplItem(CiplItem itm)
        {


            itm.UpdateBy = SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_remove_CiplItem] @IdCipl='" + itm.IdCipl + "'";

                db.DbContext.Database.ExecuteSqlCommand(sql);
                return true;
            }
        }
        public static List<SpGetCiplAvailable> GetCiplAvailable(SpGetCiplAvailable crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var cargoid = crit.Id;
                var ciplno = crit.CiplNo ?? "";
                var ciplList = "";
                db.Database.CommandTimeout = 600;

                var sql = @"[dbo].[sp_get_cipl_available] 
                            @Search='" + ciplno + "'" +
                            ", @CiplList='" + ciplList + "'" +
                            ", @CargoId='" + cargoid + "'" +
                            ", @Consignee='" + crit.ConsigneeName + "'" +
                            ", @Notify='" + crit.NotifyName + "'" +
                            ", @ExportType='" + crit.ExportType + "'" +
                            ", @Category='" + crit.Category + "'" +
                            ", @Incoterms='" + crit.IncoTerm + "'" +
                            ", @ShippingMethod='" + crit.ShippingMethod + "'";

                var data = db.Database.SqlQuery<SpGetCiplAvailable>(sql).ToList();
                return data;
            }
        }

        public static List<DocumentList> DocumentList()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.DocumentList.Where(a => a.Category == "CIPL").AsQueryable();
                return tb.ToList();
            }
        }

        public static Documents GetDocument(Documents crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.Documents.Where(a => a.IdRequest == crit.IdRequest && a.Tag == crit.Tag && a.IsDelete == false);
                return tb.FirstOrDefault();
            }
        }

        public static long InsertDocument(RequestCipl step, string filename, string typeDoc)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdRequest", long.Parse(step.IdCipl)));
                parameterList.Add(new SqlParameter("@Category", "CIPL"));
                parameterList.Add(new SqlParameter("@Status", step.Status));
                parameterList.Add(new SqlParameter("@Step", step.IdStep));
                parameterList.Add(new SqlParameter("@Name", filename));
                parameterList.Add(new SqlParameter("@Tag", typeDoc));
                parameterList.Add(new SqlParameter("@FileName", filename));
                parameterList.Add(new SqlParameter("@Date", DateTime.Now));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@IsDelete", false));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[sp_insert_document] @IdRequest, @Category, @Status, @Step, @Name, @Tag, @FileName, @Date, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete", parameters);
                return 1;
            }
        }

        public static RequestCipl GetRequestCiplById(string id, string category)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.RequestCipl.Where(a => a.IdCipl == id);
                return data.FirstOrDefault();
            }
        }

        public static List<ConsigneeData> GetConsigneeHistory(string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "SELECT DISTINCT TOP 100 ConsigneeName, ConsigneeAddress, ConsigneeCountry, ConsigneeTelephone, ConsigneeFax, ConsigneePic, ConsigneeEmail from dbo.Cipl where ConsigneeName like '%" + term + "%'";
                var data = db.Database.SqlQuery<ConsigneeData>(sql).ToList();
                return data;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        public static List<string> GetUOMHistory(string term)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "Select TOP 100 Uom from dbo.CiplItem WHERE Uom Like '%" + term + "%'";
                var data = db.Database.SqlQuery<string>(sql).ToList();
                return data;
            }
        }

<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        public static List<CiplItemReviseData> GetCiplItemReviseGetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = @"exec sp_get_revise_cipl " + id;
                var data = db.Database.SqlQuery<CiplItemReviseData>(sql).ToList();
                return data;
            }
        }
        public static long GetCiplNextStepGetById(long id)
        {
            long stepId = 0;
            using (var db = new Data.EmcsContext())
            {
                var sql = @"select IdNextStep from dbo.[fn_get_cipl_request_list_all]() WHere IdCipl = " + id + "";
                stepId = db.Database.SqlQuery<long>(sql).FirstOrDefault();
            }

            return stepId;
        }

        public static long GetCiplCargoStepGetById(long id)
        {
            long stepId = 0;
            using (var db = new Data.EmcsContext())
            {
                var sql = @"select idstep from CargoCipl CC Inner Join [fn_get_cl_request_list_all]() cl ON Cc.IdCargo = cl.idCL WHere cc.IdCipl = " + id + "";
                stepId = db.Database.SqlQuery<long>(sql).FirstOrDefault();
            }

            return stepId;
        }

        public static long GetCiplCargoFlowGetById(long id)
        {
            long flowId = 0;
            using (var db = new Data.EmcsContext())
            {
                var sql = @"select idflow from CargoCipl CC Inner Join [fn_get_cl_request_list_all]() cl ON Cc.IdCargo = cl.idCL WHere cc.IdCipl = " + id + "";
                flowId = db.Database.SqlQuery<long>(sql).FirstOrDefault();
            }

            return flowId;
        }
        public static int GetCiplItemReviseTotalGetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = @"exec sp_get_revise_cipl " + id + ", 1";
                var data = db.Database.SqlQuery<CountData>(sql).FirstOrDefault();
                if (data != null)
                {
                    var total = data.Total;
                    return total;
                }
            }

            return 0;
        }

        public static List<SelectItem3> GetPickUpPic(string user, string area)
        {
            using (var db = new Data.EmcsContext())
            {
                if (user != null)
                {
                    user = Regex.Replace(user, @"[^0-9a-zA-Z]+", "");
                }
<<<<<<< HEAD

=======
                    
>>>>>>> 639d8d0 (Intial commit)

                var sql = "SELECT Fn.AD_User id, Fn.Employee_Name text, Fn.BAreaName extra FROM [dbo].[fn_get_employee_internal_ckb]() Fn WHERE (Fn.Employee_Name LIKE '%" + user + "%' OR Fn.BAreaName LIKE '%" + user + "%') ORDER BY Fn.Employee_Name";
                var data = db.Database.SqlQuery<SelectItem3>(sql).Skip(0).Take(50).AsQueryable().ToList();
                return data;
            }
        }

        public static bool UpdateQuantityReference(List<CiplItemInsert> item, string status)
        {
            using (var db = new Data.EmcsContext())
            {
                for (var j = 0; j < item.Count; j++)
                {
                    var idReference = item[j].Id;
                    if (status == "Insert")
                    {
                        var reference = db.Reference.Where(a => a.Id == idReference).FirstOrDefault();
                        if (reference != null)
                            reference.AvailableQuantity = reference.AvailableQuantity - item[j].AvailableQuantity;
                        db.SaveChanges();
                    }

                    if (status == "Delete")
                    {
                        var reference = db.Reference.Where(a => a.Id == idReference).FirstOrDefault();
                        if (reference != null)
                            reference.AvailableQuantity = reference.AvailableQuantity + item[j].Quantity;
                        db.SaveChanges();
                    }

                    if (status == "Update")
                    {
                        var reference = db.Reference.Where(a => a.Id == idReference).FirstOrDefault();
                        if (reference != null) reference.AvailableQuantity = item[j].Quantity;
                        db.SaveChanges();
                    }

                }
                return true;
            }

        }

        public static string GetCellDataName(int cell)
        {
            string rowname = "";
            if (cell == 0)
            {
                rowname = "ReferenceNo";
            }
            else if (cell == 1)
            {
                rowname = "Description";
            }
            else if (cell == 2)
            {
                rowname = "Qty";
            }
            else if (cell == 3)
            {
                rowname = "PartNumber";
            }
            else if (cell == 4)
            {
                rowname = "J-Code";
            }
            else if (cell == 5)
            {
                rowname = "Country of Origin";
            }
            else if (cell == 6)
            {
                rowname = "Unit Price";
            }
            else if (cell == 7)
            {
                rowname = "Extended Value";
            }
            else if (cell == 8)
            {
                rowname = "Currency";
            }
            else if (cell == 9)
            {
                rowname = "ASN Number";
            }
            else if (cell == 10)
            {
                rowname = "Case Number";
            }
            else if (cell == 11)
            {
                rowname = "Type";
            }
            else if (cell == 12)
            {
                rowname = "Lenght";
            }
            else if (cell == 13)
            {
                rowname = "Widht";
            }
            else if (cell == 14)
            {
                rowname = "Height";
            }
            else if (cell == 15)
            {
                rowname = "Volume";
            }
            else if (cell == 16)
            {
                rowname = "Nett Weigth";
            }
            else if (cell == 17)
            {
                rowname = "Gross Weigth ";
            }
<<<<<<< HEAD

            return rowname;
        }

        public static List<Data.Domain.EMCS.SelectItem2> GetTypeSelectList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Type
                    .OrderBy(i => i.Id)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Id,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }



=======
          
            return rowname;
        }
<<<<<<< HEAD
      
>>>>>>> 639d8d0 (Intial commit)
=======

        public static List<Data.Domain.EMCS.SelectItem2> GetTypeSelectList()
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Type
                    .OrderBy(i => i.Id)
                    .Skip(0).Take(100)
                    .Select(i => new
                    {
                        id = i.Id,
                        text = i.Name
                    }).ToList();

                return data.Select(i => new Data.Domain.EMCS.SelectItem2 { Id = i.text, Text = i.text }).ToList();
            }
        }

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
    }
}
