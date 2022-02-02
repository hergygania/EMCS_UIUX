using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class InventoryList
    {
        private const string cacheName = "App.CAT.InventoryList";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory.
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.InventoryList> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.InventoryList>().Table.Select(e => e);
                    return tb.ToList();
                }
            });
            return list;
        }

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Data.Domain.SP_DataInventory> SP_GetList(App.Data.Domain.CAT.InventoryFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_part_no", filter.ref_part_no ?? ""));
                parameterList.Add(new SqlParameter("@alt_part_no", filter.alt_part_no ?? ""));
                parameterList.Add(new SqlParameter("@comp_inv_no", filter.comp_inv_no ?? ""));
                parameterList.Add(new SqlParameter("@app_model", filter.app_model ?? ""));
                parameterList.Add(new SqlParameter("@prefix", filter.prefix ?? ""));
                parameterList.Add(new SqlParameter("@smcs_code", filter.smcs_code ?? ""));
                parameterList.Add(new SqlParameter("@component", filter.component ?? ""));
                parameterList.Add(new SqlParameter("@mod", filter.mod ?? ""));
                parameterList.Add(new SqlParameter("@StoreID", filter.StoreID ?? 0));
                parameterList.Add(new SqlParameter("@core_model", filter.core_model ?? ""));
                parameterList.Add(new SqlParameter("@SOSID", filter.SOSID ?? ""));
                parameterList.Add(new SqlParameter("@family", filter.family ?? ""));
                parameterList.Add(new SqlParameter("@crc_tat", filter.crc_tat ?? ""));
                parameterList.Add(new SqlParameter("@SectionID", filter.SectionID ?? 0));
                parameterList.Add(new SqlParameter("@surplus", filter.surplus ?? ""));
                parameterList.Add(new SqlParameter("@statusid", filter.statusid ?? ""));

                //New filter
                parameterList.Add(new SqlParameter("@doctransfer", filter.doctransfer ?? ""));
                parameterList.Add(new SqlParameter("@wcsl", filter.wcsl ?? ""));
                parameterList.Add(new SqlParameter("@WO", filter.WO ?? ""));
                parameterList.Add(new SqlParameter("@rebuildstatuscms", filter.rebuildstatuscms ?? ""));
                parameterList.Add(new SqlParameter("@originalschedule", filter.originalschedule == null ? "null" : filter.originalschedule.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@unitno", filter.unitno ?? ""));
                parameterList.Add(new SqlParameter("@serialno", filter.serialno ?? ""));
                parameterList.Add(new SqlParameter("@location", filter.location ?? ""));
                parameterList.Add(new SqlParameter("@customer", filter.customer ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventory>
                    (@"[cat].[spGetDataListInventory] @ref_part_no, @alt_part_no, @comp_inv_no, @app_model, @prefix, @smcs_code, @component, @mod, @StoreID, @core_model, @SOSID, @family, @crc_tat, @SectionID, @surplus, @statusid
                , @doctransfer, @wcsl, @WO, @rebuildstatuscms, @unitno, @serialno, @location, @customer, @originalschedule", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure untuk proses download.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Data.Domain.SP_DataInventoryForDownload> SP_GetListForDownload(App.Data.Domain.CAT.InventoryFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ref_part_no", filter.ref_part_no ?? ""));
                parameterList.Add(new SqlParameter("@alt_part_no", filter.alt_part_no ?? ""));
                parameterList.Add(new SqlParameter("@comp_inv_no", filter.comp_inv_no ?? ""));
                parameterList.Add(new SqlParameter("@app_model", filter.app_model ?? ""));
                parameterList.Add(new SqlParameter("@prefix", filter.prefix ?? ""));
                parameterList.Add(new SqlParameter("@smcs_code", filter.smcs_code ?? ""));
                parameterList.Add(new SqlParameter("@component", filter.component ?? ""));
                parameterList.Add(new SqlParameter("@mod", filter.mod ?? ""));
                parameterList.Add(new SqlParameter("@StoreID", filter.StoreID ?? 0));
                parameterList.Add(new SqlParameter("@core_model", filter.core_model ?? ""));
                parameterList.Add(new SqlParameter("@SOSID", filter.SOSID ?? ""));
                parameterList.Add(new SqlParameter("@family", filter.family ?? ""));
                parameterList.Add(new SqlParameter("@crc_tat", filter.crc_tat ?? ""));
                parameterList.Add(new SqlParameter("@SectionID", filter.SectionID ?? 0));
                parameterList.Add(new SqlParameter("@surplus", filter.surplus ?? ""));
                parameterList.Add(new SqlParameter("@statusid", filter.statusid ?? ""));

                //New filter
                parameterList.Add(new SqlParameter("@doctransfer", filter.doctransfer ?? ""));
                parameterList.Add(new SqlParameter("@wcsl", filter.wcsl ?? ""));
                parameterList.Add(new SqlParameter("@WO", filter.WO ?? ""));
                parameterList.Add(new SqlParameter("@rebuildstatuscms", filter.rebuildstatuscms ?? ""));
                parameterList.Add(new SqlParameter("@originalschedule", filter.originalschedule == null ? "null" : filter.originalschedule.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@unitno", filter.unitno ?? ""));
                parameterList.Add(new SqlParameter("@serialno", filter.serialno ?? ""));
                parameterList.Add(new SqlParameter("@location", filter.location ?? ""));
                parameterList.Add(new SqlParameter("@customer", filter.customer ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventoryForDownload>
                    (@"[cat].[spGetDataListInventoryForDownload] @ref_part_no, @alt_part_no, @comp_inv_no, @app_model, @prefix, @smcs_code, @component, @mod, @StoreID, @core_model, @SOSID, @family, @crc_tat, @SectionID, @surplus, @statusid
                        , @doctransfer, @wcsl, @WO, @rebuildstatuscms, @unitno, @serialno, @location, @customer, @originalschedule", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure untuk proses download.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static List<Data.Domain.SP_DataInventoryEditForDownload> SP_GetListEditForDownload()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventoryEditForDownload>
                    ("[cat].[spDownloadListInventoryforEdit] ").ToList();

                return data;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Data.Domain.SP_DataInventory SP_GetData(long ID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@InventoryID", ID));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventory>("[cat].[spGetDataInventory] @InventoryID", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengambilan data Inventory menggunakan Store Procedure By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Data.Domain.SP_DataInventory SP_GetDataEdit(long ID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@InventoryID", ID));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_DataInventory>("[cat].[spGetDataEditInventory] @InventoryID", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// Pengambilan data Inventory By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryList GetData(long ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var inventorylist = db.InventoryList.Where(w => w.ID == ID).FirstOrDefault();

                return inventorylist;
            }
        }

        /// <summary>
        /// Pengambilan data Inventory By KAL.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryList GetDataByKAL(string KAL)
        {
            using (var db = new Data.EfDbContext())
            {
                var inventorylist = db.InventoryList.Where(w => w.KAL == KAL).FirstOrDefault();

                return inventorylist;
            }
        }

        /// <summary>
        /// insert update delete data Inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.InventoryList item, string dml)
        {
            item.CreatedDate = DateTime.Now;
            item.UpdateDate = DateTime.Now;
            int AllocationID = 0;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (item.InventoryAllocation != null)
                {
                    foreach (var invAll in item.InventoryAllocation)
                    {
                        invAll.CreatedBy = Domain.SiteConfiguration.UserName;
                        invAll.UpdatedBy = Domain.SiteConfiguration.UserName;
                        invAll.CreatedDate = DateTime.Now;
                        invAll.UpdatedDate = DateTime.Now;
                        db.CreateRepository<Data.Domain.InventoryAllocation>().CRUD(dml, invAll);
                        if (invAll.IsActive) AllocationID = invAll.ID;
                    }
                }

                item.Allocated = AllocationID;
                return db.CreateRepository<Data.Domain.InventoryList>().CRUD(dml, item);
            }
        }

        /// <summary>
        /// Pengambilan data Inventory KAL terakhir.
        /// </summary>
        /// <param name="KAL"></param>
        /// <returns></returns>
        public static Data.Domain.InventoryList GetDataLastKAL()
        {
            using (var db = new Data.EfDbContext())
            {
                var inventorylist = db.InventoryList.OrderByDescending(w => w.KAL).FirstOrDefault();

                return inventorylist;
            }
        }

        /// <summary>
        /// insert update delete data Inventory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int Insert(Data.Domain.InventoryList item, string dml)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@KAL", item.KAL));
                    parameterList.Add(new SqlParameter("@AlternetPartNumber", item.AlternetPartNumber));
                    parameterList.Add(new SqlParameter("@RGNumber", item.RGNumber));
                    
                    parameterList.Add(new SqlParameter("@SOS", item.SOS));
                    parameterList.Add(new SqlParameter("@Store", item.StoreNumber));

                    SqlParameter[] parameters = parameterList.ToArray();

                    db.DbContext.Database.ExecuteSqlCommand(@" exec [cat].[InsertInventoryDatafrombtn] @KAL, @AlternetPartNumber , @RGNumber ,
	                                                           @SOS , @Store ", parameters);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        /// <summary>
        /// Update data Inventory menggunakan Store Procedure By ID.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static int SP_UpdateDataInventory(Data.Domain.InventoryList item, string dml)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@InventoryID", item.ID));
                    parameterList.Add(new SqlParameter("@KAL", item.KAL));
                    parameterList.Add(new SqlParameter("@AlternetPartNumber", item.AlternetPartNumber));
                    parameterList.Add(new SqlParameter("@RGNumber", item.RGNumber));
                    parameterList.Add(new SqlParameter("@DocWCSL", item.DocWCSL));
                    parameterList.Add(new SqlParameter("@UnitNumber", item.UnitNumber));
                    parameterList.Add(new SqlParameter("@TUID", item.TUID));
                    parameterList.Add(new SqlParameter("@DocSales", item.DocSales));
                    parameterList.Add(new SqlParameter("@LastStatus", item.LastStatus));
                    if (item.DocDate == null)
                    {
                        parameterList.Add(new SqlParameter("@DocDate", DBNull.Value));
                    }
                    else
                    {
                        parameterList.Add(new SqlParameter("@DocDate", item.DocDate));
                    }
                  
                    parameterList.Add(new SqlParameter("@EquipmentNumber", item.EquipmentNumber));
                    parameterList.Add(new SqlParameter("@MO", item.MO));
                    parameterList.Add(new SqlParameter("@DocReturn", item.DocReturn));
                    parameterList.Add(new SqlParameter("@NewWO6F", item.NewWO6F));
                    parameterList.Add(new SqlParameter("@Customer_ID", item.CUSTOMER_ID));
                    parameterList.Add(new SqlParameter("@DocTransfer", item.DocTransfer));
                    if (item.DocDateTransfer == null)
                    {
                        parameterList.Add(new SqlParameter("@DocDateTransfer", DBNull.Value));
                    }
                    else
                    {
                        parameterList.Add(new SqlParameter("@DocDateTransfer", item.DocDateTransfer));
                    }
                    
                    parameterList.Add(new SqlParameter("@SOS", item.SOS));
                    parameterList.Add(new SqlParameter("@Store", item.StoreNumber));
                    
                    SqlParameter[] parameters = parameterList.ToArray();

                    db.DbContext.Database.ExecuteSqlCommand(@" exec [cat].[UpdateInventoryDatafrombtn] @InventoryID,@KAL, @AlternetPartNumber , @RGNumber ,
	                                                            @DocWCSL , @UnitNumber , @TUID , @DocSales , @LastStatus ,
	                                                            @DocDate , @EquipmentNumber , @MO , @DocReturn , @NewWO6F ,
	                                                            @Customer_ID , @DocTransfer , @DocDateTransfer , @SOS , @Store ", parameters);
                    
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
