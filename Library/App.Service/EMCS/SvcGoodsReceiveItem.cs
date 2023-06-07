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

        public static long CrudSp(GoodsReceiveItem data, string status)
        { 
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            { 
                db.DbContext.Database.CommandTimeout = 600;
                //var id = data.Id;
                //var idCipl = data.IdCipl;
                //var idGr = data.IdGr;
                //var doNo = data.DoNo ?? "";
                //var daNo = data.DaNo ?? "";
                //var fileName = data.FileName ?? "";
                //var createBy = SiteConfiguration.UserName;
                //var createDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                //var updateBy = SiteConfiguration.UserName;
                //var updateDate = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                //var isDelete = "0";

                //List<SqlParameter> parameterList = new List<SqlParameter>();
                //parameterList.Add(new SqlParameter("@Id", id));
                //parameterList.Add(new SqlParameter("@IdCipl", idCipl));
                //parameterList.Add(new SqlParameter("@IdGr", idGr));
                //parameterList.Add(new SqlParameter("@DoNo", doNo));
                //parameterList.Add(new SqlParameter("@DaNo", daNo));
                //parameterList.Add(new SqlParameter("@FileName", fileName));
                //parameterList.Add(new SqlParameter("@CreateBy", createBy));
                //parameterList.Add(new SqlParameter("@CreateDate", createDate));
                //parameterList.Add(new SqlParameter("@UpdateBy", updateBy));
                //parameterList.Add(new SqlParameter("@UpdateDate", updateDate));
                //parameterList.Add(new SqlParameter("@IsDelete", isDelete));

                //var query = "exec [dbo].[sp_insert_update_gr_item] @Id, @IdCipl, @IdGr, @DoNo, @DaNo, @FileName, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete";
                //var result = db.DbContext.Database.SqlQuery<IdData>(query, parameterList).FirstOrDefault();
                //if (result != null) return result.Id;
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

        public static GoodsReceiveItem GetById(long id)
        { 
            using (var db = new Data.EmcsContext())
            { 
                var tb = db.GoodsReceiveItem.Where(a => a.Id == id);
                return tb.FirstOrDefault();
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

        public static List<GoodsReceiveItem> GetByGrId(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.GoodsReceiveItem.Where(a => a.IdGr == id);
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
                var tb = db.GoodsReceiveItem.Where(a => a.IdCipl == id).FirstOrDefault();
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
    }
}
