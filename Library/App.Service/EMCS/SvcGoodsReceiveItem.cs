using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Domain;
using System.Dynamic;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcGoodsReceiveItem
    {
        public const string CacheName = "App.EMCS.SvcGoodsReceiveItem";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic GetList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.GoodsReceiveItem
                         join t1 in db.CiplData on t0.DoNo equals t1.EdoNo
                         where t0.IdGr == crit.Id
                         select new
                         {
                             t0.Id,
                             t0.DaNo,
                             t0.DoNo,
                             t0.FileName,
                             t0.IdGr,
                             t0.CreateBy,
                             t0.CreateDate,
                             t1.Category,
                             t1.CategoriItem
                         };
                var data = tb.ToList();
                dynamic result = new ExpandoObject();
                result.rows = data.ToList();
                result.total = data.Count();
                return result;
            }
        }
        public static dynamic GetName(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.GoodsReceiveItem
                         join t1 in db.CiplData on t0.DoNo equals t1.EdoNo
                         where t0.IdGr == Id
                         select new
                         {
                             t0.Id,
                             t0.DaNo,
                             t0.DoNo,
                             t0.FileName,
                             t0.IdGr,
                             t0.CreateBy,
                             t0.CreateDate,
                             t1.Category,
                             t1.CategoriItem
                         };

                return tb.ToList();
            }
        }

        public static long CrudSp(GoodsReceiveItem data, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var Id = data.Id;
                var IdCipl = data.IdCipl;
                var IdGr = data.IdGr;
                var DoNo = data.DoNo ?? "";
                var DaNo = data.DaNo ?? "";
                var FileName = data.FileName ?? "";
                var CreateBy = SiteConfiguration.UserName;
                var CreateDate = DateTime.Now;
                var UpdateBy = SiteConfiguration.UserName;
                var UpdateDate = DateTime.Now;
                var IsDelete = "0";

                var query = "exec [dbo].[sp_insert_update_gr_item] @Id='" + Id + "', @IdCipl ='" + IdCipl + "', @IdGr='" + IdGr + "', @DoNo='" + DoNo + "', @DaNo='" + DaNo + "', @FileName='" + FileName + "', @CreateBy='" + CreateBy + "', @CreateDate='" + CreateDate + "', @UpdateBy='" + UpdateBy + "', @UpdateDate='" + UpdateDate + "', @IsDelete='" + IsDelete + "'";
                var result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(query).FirstOrDefault();
                if (result != null) return result.Id;
            }

            return 0;
        }
        public static List<ShippingFleet> GetListArmada(long Id, long IdGr)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var data = db.Database.SqlQuery<ShippingFleet>("exec Sp_GetArmdaList '" + IdGr + "','" + Id + "'").ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static long SaveArmada(ShippingFleet data)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;

                    var Id = data.Id;
                    var IdCipl = "0";
                    var IdGr = data.IdGr;
                    var DoNo = data.DoNo ?? "";
                    var DaNo = data.DaNo ?? "";
                    var PicName = data.PicName ?? "";
                    var PhoneNumber = data.PhoneNumber ?? "";
                    var KtpNumber = data.KtpNumber ?? "";
                    var SimNumber = data.SimNumber ?? "";
                    var SimExpiryDate = data.SimExpiryDate;
                    var StnkNumber = data.StnkNumber ?? "";
                    var KirNumber = data.KirNumber ?? "";
                    var KirExpire = data.KirExpire;
                    var NopolNumber = data.NopolNumber ?? "";
                    var EstimationTimePickup = data.EstimationTimePickup;
                    var Apar = data.Apar;
                    var Apd = data.Apd;
                    var Bast = data.Bast;
                    var query = "exec [dbo].[sp_AddArmada] @Id='" + Id + "', @IdCipl ='" + IdCipl + "', @IdGr='" + IdGr + "', @DoNo='" + DoNo + "', @DaNo='" + DaNo + "', @PicName='" + PicName + "', @PhoneNumber='" + PhoneNumber + "', @KtpNumber='" + KtpNumber +
                        "', @SimNumber='" + SimNumber + "', @SimExpiryDate='" + SimExpiryDate + "', @StnkNumber='" + StnkNumber + "', @KirNumber='" + KirNumber + "', @KirExpire='" + KirExpire +
                        "', @NopolNumber='" + NopolNumber + "', @EstimationTimePickup='" + EstimationTimePickup + "', @Apar='" + Apar + "', @Apd='" + Apd + "',@Bast='" + Bast + "'";
                    var result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(query).FirstOrDefault();




                    return result.Id;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static long SaveArmadaHistory(ShippingFleet data, string Status)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var Id = "0";
                    var IdShippingFleet = data.Id;
                    var IdCipl = "0";
                    var IdGr = data.IdGr;
                    var DoNo = data.DoNo ?? "";
                    var DaNo = data.DaNo ?? "";
                    var PicName = data.PicName ?? "";
                    var PhoneNumber = data.PhoneNumber ?? "";
                    var KtpNumber = data.KtpNumber ?? "";
                    var SimNumber = data.SimNumber ?? "";
                    var SimExpiryDate = data.SimExpiryDate;
                    var StnkNumber = data.StnkNumber ?? "";
                    var KirNumber = data.KirNumber ?? "";
                    var KirExpire = data.KirExpire;
                    var NopolNumber = data.NopolNumber ?? "";
                    var EstimationTimePickup = data.EstimationTimePickup;
                    var Apar = data.Apar;
                    var Apd = data.Apd;
                    var Bast = data.Bast;

                    var query = "exec [dbo].[sp_AddArmadaHistory] @Id='" + Id + "', @IdShippingFleet='" + IdShippingFleet + "', @IdCipl ='" + IdCipl + "', @IdGr='" + IdGr + "', @DoNo='" + DoNo + "', @DaNo='" + DaNo + "', @PicName='" + PicName + "', @PhoneNumber='" + PhoneNumber + "', @KtpNumber='" + KtpNumber +
                        "', @SimNumber='" + SimNumber + "', @SimExpiryDate='" + SimExpiryDate + "', @StnkNumber='" + StnkNumber + "', @KirNumber='" + KirNumber + "', @KirExpire='" + KirExpire +
                        "', @NopolNumber='" + NopolNumber + "', @EstimationTimePickup='" + EstimationTimePickup + "', @Apar='" + Apar + "', @Apd='" + Apd + "',@Bast='" + Bast + "',@Status='" + Status + "'";
                    var result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(query).FirstOrDefault();




                    return result.Id;

                }




            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static long SaveArmadaForRFC(ShippingFleet data, string Status)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var Id = "0";
                    var IdShippingFleet = data.Id;
                    var IdCipl = "0";
                    var IdGr = data.IdGr;
                    var DoNo = data.DoNo ?? "";
                    var DaNo = data.DaNo ?? "";
                    var PicName = data.PicName ?? "";
                    var PhoneNumber = data.PhoneNumber ?? "";
                    var KtpNumber = data.KtpNumber ?? "";
                    var SimNumber = data.SimNumber ?? "";
                    var SimExpiryDate = data.SimExpiryDate;
                    var StnkNumber = data.StnkNumber ?? "";
                    var KirNumber = data.KirNumber ?? "";
                    var KirExpire = data.KirExpire;
                    var NopolNumber = data.NopolNumber ?? "";
                    var EstimationTimePickup = data.EstimationTimePickup;
                    var FileName = data.FileName;
                    var Apar = data.Apar;
                    var Apd = data.Apd;
                    var Bast = data.Bast;

                    var query = "exec [dbo].[sp_AddArmadaForRFC] @Id='" + Id + "', @IdShippingFleet='" + IdShippingFleet + "', @IdCipl ='" + IdCipl + "', @IdGr='" + IdGr + "', @DoNo='" + DoNo + "', @DaNo='" + DaNo + "', @PicName='" + PicName + "', @PhoneNumber='" + PhoneNumber + "', @KtpNumber='" + KtpNumber +
                        "', @SimNumber='" + SimNumber + "', @SimExpiryDate='" + SimExpiryDate + "', @StnkNumber='" + StnkNumber + "', @KirNumber='" + KirNumber + "', @KirExpire='" + KirExpire +
                        "', @NopolNumber='" + NopolNumber + "', @EstimationTimePickup='" + EstimationTimePickup + "', @Apar='" + Apar + "', @Apd='" + Apd + "',@Bast='" + Bast + "',@Status='" + Status + "',@FileName='" + FileName +"'";
                    var result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(query).FirstOrDefault();   




                    return result.Id;

                }




            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static int UpdateRFCChangeForRg(SpGoodReceive item)
        {
            try
            {
                try
                {
                    using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                    {
                        db.DbContext.Database.CommandTimeout = 600;
                        List<SqlParameter> parameterList = new List<SqlParameter>();
                        db.DbContext.Database.ExecuteSqlCommand("exec [sp_update_RFCGR] @Id='" + item.Id + "', @Vendor='" + item.Vendor + "',@VehicleType='" + item.VehicleType + "', @VehicleMerk='" + item.VehicleMerk + "',@PickupPoint='" + item.PickupPoint + "',@PickupPic='" + item.PickupPic + "',@Notes='" + item.Notes + "'");

                        return 1;
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static long SaveArmadaRefrence(ShippingFleet data)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;

                    var query = "exec [dbo].[sp_AddArmadaRefrence]  @IdShippingFleet='" + data.Id + "', @IdGr ='" + data.IdGr + "', @IdCipl='" + data.IdCipl + "', @DoNo='" + data.DoNo + "'";
                    db.DbContext.Database.SqlQuery<long>(query).FirstOrDefault();

                    return 1;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static SP_ShippingFleetItem SaveArmadaItem(ShippingFleet data, long ciplitemid)
        {
            try
            {
                SP_ShippingFleetItem result = new SP_ShippingFleetItem();
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var Id = 0;
                    //var IdShippingFleet = data.Id;
                    //var IdCipl = data.IdCipl;
                    //var IdGr = data.IdGr;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    //parameterList.Add(new SqlParameter("@IdShippingFleet", Convert.ToString(data.Id)));
                    //parameterList.Add(new SqlParameter("@IdGr", data.IdGr));
                    ////parameterList.Add(new SqlParameter("@IdCiplItem", ciplitemid));
                    //parameterList.Add(new SqlParameter("@IdCipl", ciplitemid));
                    parameterList.Add(new SqlParameter("@Id", Id));
                    parameterList.Add(new SqlParameter("@IdShippingFleet", data.Id));
                    parameterList.Add(new SqlParameter("@IdGr", data.IdGr));
                    parameterList.Add(new SqlParameter("@IdCipl", "0"));
                    parameterList.Add(new SqlParameter("@IdCiplItem", ciplitemid));
                    SqlParameter[] parameters = parameterList.ToArray();
                    result = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SP_ShippingFleetItem>("[dbo].[sp_addShippingFleetItem] @Id,@IdShippingFleet, @IdGr, @IdCipl,@IdCiplItem", parameters).FirstOrDefault();

                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static long DeleteItemShippingFleet(long id, long idCiplItem)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", id));
                    parameterList.Add(new SqlParameter("@idCiplItem", idCiplItem));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.SqlQuery<SP_ShippingFleetItem>("[dbo].[SP_deleteShippingFleet] @id ,@idCiplItem", parameters).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return 1;
            }
        }
        public static List<GoodsReceive> UpdateGr(Data.Domain.EMCS.SpGoodReceive itm)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {

                    int apar = itm.Apar == true ? 1 : 0;
                    int apd = itm.Apd == true ? 1 : 0;

                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", itm.Id));
                    parameterList.Add(new SqlParameter("@PickupPoint", itm.PickupPoint));
                    parameterList.Add(new SqlParameter("@PickupPic", itm.PickupPic));
                    //parameterList.Add(new SqlParameter("@Status", itm.Id));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<GoodsReceive>("[dbo].[SP_UpdateGr] @Id, @PickupPoint, @PickupPic", parameters).ToList();

                    return data;
                }
                catch (Exception err)
                {
                    throw err;
                }
            }


        }
        public static long DeleteArmada(long Id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", Id));

                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.SqlQuery<SP_ShippingFleetItem>("[dbo].[SP_deleteArmada] @id", parameters).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return 1;
            }
        }
        public static long DeleteArmadaChange(long Id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", Id));

                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.SqlQuery<SP_ShippingFleetItem>("[dbo].[SP_deleteArmadaChange] @id", parameters).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return 1;
            }
        }
        public static List<SpCiplItemList_Armada> GetCiplAvailableForShippingFleet(string idcipl, long idgr, long idShippingFleet)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    //if(idshippingfleetitem == null)
                    //{
                    //    idshippingfleetitem = 0;
                    //}
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idcipl));
                    parameterList.Add(new SqlParameter("@IdGr", idgr));
                    parameterList.Add(new SqlParameter("@IdShippingFleet", idShippingFleet));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpCiplItemList_Armada>("[dbo].[SP_GetAvailableShippingCiplItem] @IdCipl,@IdGr,@IdShippingFleet", parameters).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<SpCiplItemList_Armada> CiplItemAvailableInArmada(string idcipl, long idgr, long idShippingFleet)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idcipl));
                    parameterList.Add(new SqlParameter("@IdGr", idgr));
                    parameterList.Add(new SqlParameter("@IdShippingFleet", idShippingFleet));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<SpCiplItemList_Armada>("[dbo].[sp_CiplItemInArmada] @IdCipl,@IdGr,@IdShippingFleet", parameters).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Cipl> GetCiplIdFromDoNo(string DoNo)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@DoNo", DoNo));
                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<Cipl>("[dbo].[SP_GetCiplId] @DoNo", parameters).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetCiplItem(long idcipl)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idcipl));
                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<int>("[dbo].[SP_GetCiplItem] @IdCipl", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetCiplItemCount(string idcipl, long idgr, long idshippingfleet)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idcipl));
                    parameterList.Add(new SqlParameter("@IdGr", idgr));
                    parameterList.Add(new SqlParameter("@IdShippingFleet", idshippingfleet));
                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<int>("[dbo].[SP_GetCiplItemCount] @IdCipl,@IdGr,@IdShippingFleet", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetCiplItemInShippingFleetItem(string idcipl, long idgr)
        {
            try
            {

                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCipl", idcipl));
                    parameterList.Add(new SqlParameter("@IdGr", idgr));
                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<int>("[dbo].[SP_GetCiplItemInShippingFleetItem] @IdCipl,@IdGr", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static GoodsReceiveItem GetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.GoodsReceiveItem.Where(a => a.Id == id);
                //var tb = db.ShippingFleetRefrence.Where(a => a.Id == id);
                return tb.FirstOrDefault();
            }
        }
        public static GoodReceiveDocument GetDocById(long id)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var tb = db.GoodsReceiveDocument.Where(a => a.Id == id);
                    return tb.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static Cipl GetIdCipl(string doNo)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.CiplData.Where(a => a.EdoNo == doNo);
                return tb.FirstOrDefault();
            }
        }

        public static List<ShippingFleetRefrence> GetByGrId(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                //var tb = db.GoodsReceiveItem.Where(a => a.IdGr == id);
                var tb = db.ShippingFleetRefrence.Where(a => a.IdGr == id);
                return tb.ToList();
            }
        }

        public static int Crud(GoodsReceiveItem itm, string dml)
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
                return db.CreateRepository<GoodsReceiveItem>().CRUD(dml, itm);
            }
        }

        public static long GetGrIdByIdCipl(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                //var tb = db.GoodsReceiveItem.Where(a => a.IdCipl == id).FirstOrDefault();
                var tb = db.ShippingFleetRefrence.Where(a => a.IdCipl == id).FirstOrDefault();
                if (tb != null)
                {
                    var idGr = tb.IdGr;
                    return idGr;
                }
            }

            return 0;
        }

        public static List<Cipl> GetEdoNoGrItemList(string area, long idGr)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.Database.SqlQuery<Cipl>("exec sp_get_edi_gritem_edit '" + area + "', '" + idGr + "'").ToList();
                return data;
            }
        }
        public static dynamic GetArmadaItemList(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.Database.SqlQuery<int>("exec Sp_checkarmadadata @Id='" + Id + "' ").ToList();
                return data;
            }
        }
        public static dynamic GetFileName(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet
                         where t0.IdGr == Id
                         select t0.FileName;
                return tb.ToList();

            }

        }
        public static dynamic GetItemInGr(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet
                         where t0.IdGr == Id
                         select t0;
                return tb.Count();

            }
        }
        public static dynamic GetRFCItemSIById(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet_Change
                         where t0.Id == Id
                         select t0;
                return tb.FirstOrDefault();

            }
        }
        #region

        public static List<ShippingFleet_History> GetDataByIdGrForHistory(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet_History  where t0.IdGr == Id 
                         orderby t0.Id descending select t0 ;
                return tb.ToList();
            }
        }
        public static ShippingFleet_History GetHistoryDataById(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet_History where t0.IdShippingFleet == Id select t0;
                return tb.FirstOrDefault();
            }
        }
        public static List<ShippingFleet_Change> GetDataByIdGrForRFC(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet_Change
                         where t0.IdGr == Id
                         orderby t0.Id descending
                         select t0;
                return tb.ToList();
            }
        }
        public static ShippingFleet GetArmadaById(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet
                         where t0.Id == Id
                         select t0;
                return tb.FirstOrDefault();
            }
        }
        public static ShippingFleet_Change GetDataByIdShippingFleetById(long Id)
        {
            using(var db= new Data.EmcsContext())
            {
                var tb = from t0 in db.ShippingFleet_Change
                         where t0.IdShippingFleet == Id
                         select t0;
                return tb.FirstOrDefault();
            }
        }
        public static bool UploadHistoryOfDocument(long id, string filename)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdShippingFleet", id));
                    parameterList.Add(new SqlParameter("@FileName", filename));
                    SqlParameter[] parameters = parameterList.ToArray();
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_UpdateFileForHistory] @IdShippingFleet, @FileName", parameters);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }
}
