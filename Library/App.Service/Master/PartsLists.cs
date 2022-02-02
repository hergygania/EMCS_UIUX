using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using App.Data;
using App.Data.Caching;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using System.Data.SqlClient;
using FastMember;
using System.Data;
using System.Configuration;

namespace App.Service.Master
{
    public class PartsLists
    {
        private const string cacheName = "App.master.PartsLists";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.PartsList> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.PartsLists.AsNoTracking().AsParallel().ToList()
                             from h in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OMID).DefaultIfEmpty()
                             select new Data.Domain.PartsList()
                             {
                                 PartsID = c.PartsID,
                                 PartsNumber = c.PartsNumber,
                                 //PartsNumberReformat = c.PartsNumberReformat,
                                 ManufacturingCode = c.ManufacturingCode,
                                 PartsName = c.PartsName,
                                 Description = c.Description,
                                 //Status = c.Status,
                                 OMID = c.OMID,
                                 EntryDate = c.EntryDate,
                                 ModifiedDate = c.ModifiedDate,
                                 EntryBy = c.EntryBy,
                                 ModifiedBy = c.ModifiedBy,
                                 OMCode = (h == null ? "" : h.OMCode),
                                 Add_Change = c.Add_Change,
                                 PPNBM = c.PPNBM,
                                 Description_Bahasa = c.Description_Bahasa,
                                 Pref_Tarif = c.Pref_Tarif
                             };

                    return tb.ToList();
                }

            });

            return list;
        }

        public static List<Data.Domain.SP_PartsList> SP_GetListPerPage(int StartNum, int EndNum, string SeacrhName, string OrderBy)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SeacrhName", SeacrhName.Trim().ToUpper()));
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToUpper()));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_PartsList>("[imex].[SP_PartsList] 0, @SeacrhName, @StartNum, @EndNum, @OrderBy", parameters).ToList();

                return data.ToList();
            }
        }

        public static List<Int32> SP_GetCountPerPage(int StartNum, int EndNum, string SeacrhName, string OrderBy)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SeacrhName", SeacrhName.Trim().ToUpper()));
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToUpper()));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Int32>("[imex].[SP_PartsList] 1, @SeacrhName, @StartNum, @EndNum, @OrderBy", parameters).ToList();

                return data.ToList();
            }
        }

        public static bool DataExist(string PartsNumber, string MC)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = db.PartsLists.AsNoTracking().Where(w => w.PartsNumber == PartsNumber && w.ManufacturingCode == MC).FirstOrDefault();
                if (tb != null) return true;

                return false;
            }
        }

        public static List<Data.Domain.PartsList> ListNotExist(List<Data.Domain.PartsList> list)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = (from c in list
                          where !db.PartsLists.Any(a => (a.PartsNumber == c.PartsNumber) && (a.ManufacturingCode == c.ManufacturingCode))
                          select new Data.Domain.PartsList
                          {
                              PartsID = c.PartsID,
                              PartsNumber = c.PartsNumber,
                              ManufacturingCode = c.ManufacturingCode,
                              PartsName = c.PartsName,
                              Description = c.Description,
                              //Status = c.Status,
                              OMID = c.OMID,
                              EntryDate = c.EntryDate,
                              ModifiedDate = c.ModifiedDate,
                              EntryBy = c.EntryBy,
                              ModifiedBy = c.ModifiedBy,
                              OMCode = c.OMCode,
                              Add_Change = c.Add_Change,
                              PPNBM = c.PPNBM,
                              Description_Bahasa = c.Description_Bahasa,
                              Pref_Tarif = c.Pref_Tarif
                          });

                return tb.ToList();
            }
        }

        public static List<Data.Domain.PartsListGroupByPartNumber> GetListGroupByPartNumber()
        {
            string key = string.Format("App.master.PartsListsGroupByPartsNumber");

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsListGroupByPartNumber>("[pis].[spGetPartListGroupByPartsNumber]").ToList();

                    return data;
                }
            });

            return list;
        }

        public static List<Data.Domain.PartsList> GetListById(int id)
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.PartsLists.Where(w => w.PartsID == id).AsNoTracking().AsParallel().ToList()
                             from h in Service.Master.OrderMethods.GetList(c.OMID ?? 0).DefaultIfEmpty()
                             select new Data.Domain.PartsList()
                             {
                                 PartsID = c.PartsID,
                                 PartsNumber = c.PartsNumber,
                                 //PartsNumberReformat = c.PartsNumberReformat,
                                 ManufacturingCode = c.ManufacturingCode,
                                 PartsName = c.PartsName,
                                 Description = c.Description,
                                 //Status = c.Status,
                                 OMID = c.OMID,
                                 EntryDate = c.EntryDate,
                                 ModifiedDate = c.ModifiedDate,
                                 EntryBy = c.EntryBy,
                                 ModifiedBy = c.ModifiedBy,
                                 OMCode = (h == null ? "" : h.OMCode),
                                 Add_Change = c.Add_Change,
                                 PPNBM = c.PPNBM,
                                 Description_Bahasa = c.Description_Bahasa,
                                 Pref_Tarif = c.Pref_Tarif
                             };

                    return tb.ToList();
                }

            });

            return list;
        }

        public static List<Data.Domain.PartsNumberList> GetListByNewId(int id)
        {
            string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.EfDbContext())
            {
                var tb = from c in db.PartsNumberList.Where(w => w.PartsID == id).AsNoTracking().AsParallel().ToList()
                         from h in Service.Master.OrderMethods.GetList(c.OMID ?? 0).DefaultIfEmpty()
                         select new Data.Domain.PartsNumberList()
                         {
                             PartsID = c.PartsID,
                             PartsNumber = c.PartsNumber,
                             //PartsNumberReformat = c.PartsNumberReformat,
                             ManufacturingCode = c.ManufacturingCode,
                             PartsName = c.PartsName,
                             Description = c.Description,
                             DeletionFlag = c.DeletionFlag,
                             OMID = c.OMID,
                             EntryDate = c.EntryDate,
                             ModifiedDate = c.ModifiedDate,
                             EntryBy = c.EntryBy,
                             ModifiedBy = c.ModifiedBy,
                             OMCode = (h == null ? "" : h.OMCode),
                             Add_Change = c.Add_Change,
                             PPNBM = c.PPNBM,
                             Description_Bahasa = c.Description_Bahasa,
                             Pref_Tarif = c.Pref_Tarif
                         };

                return tb.ToList();
            }

            //});

            //return tb;
        }

        public static List<Data.Domain.PartsList> GetListNew(string searchTerm, int offset, int pageSize)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.EfDbContext())
            {
                var tb = (from c in db.PartsLists.AsNoTracking().AsParallel().ToList()
                          where (c.PartsNumber + "|" + c.PartsName).ToLower().Contains(searchTerm.ToLower())
                          select new Data.Domain.PartsList()
                          {
                              PartsID = c.PartsID,
                              PartsNumber = c.PartsNumber,
                              //PartsNumberReformat = c.PartsNumberReformat,
                              ManufacturingCode = c.ManufacturingCode,
                              PartsName = c.PartsName,
                              Description = c.Description,
                              //Status = c.Status,
                              OMID = c.OMID,
                              EntryDate = c.EntryDate,
                              ModifiedDate = c.ModifiedDate,
                              EntryBy = c.EntryBy,
                              ModifiedBy = c.ModifiedBy,
                              Add_Change = c.Add_Change,
                              PPNBM = c.PPNBM,
                              Description_Bahasa = c.Description_Bahasa,
                              Pref_Tarif = c.Pref_Tarif
                          });

                return tb.OrderBy(o => o.PartsNumber).ToList();
            }

        }

        public static List<Data.Domain.PartsList> GetListPartNumber(int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsList>("[mp].[spGetListPartNumber] @next, @keysearch, 1", parameters).ToList();

                return data;
            }
        }

        public static int GetListCountPartNumber(int next, string keysearch)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[mp].[spGetListPartNumber] @next, @keysearch, 2", parameters).FirstOrDefault();

                return data;
            }
        }

        public static List<Data.Domain.PartsList> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetListTable(name)
                       orderby c.PartsName
                       select c;
            return list.ToList();
        }

        public static Data.Domain.PartsList GetId(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from c in db.PartsLists.Where(w => w.PartsID == id).AsNoTracking().AsParallel().ToList()
                         from h in Service.Master.OrderMethods.GetList(c.OMID ?? 0).DefaultIfEmpty()
                         select new Data.Domain.PartsList()
                         {
                             PartsID = c.PartsID,
                             PartsNumber = c.PartsNumber,
                             //PartsNumberReformat = c.PartsNumberReformat,
                             PartsName = c.PartsName,
                             Description = c.Description,
                             //Status = c.Status,
                             OMID = c.OMID,
                             EntryDate = c.EntryDate,
                             ModifiedDate = c.ModifiedDate,
                             EntryBy = c.EntryBy,
                             ModifiedBy = c.ModifiedBy,
                             OMCode = (h == null ? "" : h.OMCode),
                             ManufacturingCode = c.ManufacturingCode,
                             PPNBM = c.PPNBM,
                             Add_Change = c.Add_Change,
                             Pref_Tarif = c.Pref_Tarif,
                             Description_Bahasa = c.Description_Bahasa
                         };

                return tb.FirstOrDefault();
            }

            //var item = GetList().Where(w => w.PartsID == id).FirstOrDefault();
            //return item;
        }

        public static Data.Domain.PartsNumberList GetIdNew(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = from c in db.PartsLists.Where(w => w.PartsID == id).AsNoTracking().AsParallel().ToList()
                         from h in Service.Master.OrderMethods.GetList(c.OMID ?? 0).DefaultIfEmpty()
                         select new Data.Domain.PartsNumberList()
                         {
                             PartsID = c.PartsID,
                             PartsNumber = c.PartsNumber,
                             //PartsNumberReformat = c.PartsNumberReformat,
                             PartsName = c.PartsName,
                             Description = c.Description,
                             DeletionFlag = c.DeletionFlag ?? 1,
                             OMID = c.OMID,
                             EntryDate = c.EntryDate,
                             ModifiedDate = c.ModifiedDate,
                             EntryBy = c.EntryBy,
                             ModifiedBy = c.ModifiedBy,
                             OMCode = (h == null ? "" : h.OMCode),
                             ManufacturingCode = c.ManufacturingCode,
                             PPNBM = c.PPNBM ?? 0,
                             Add_Change = c.Add_Change ?? 0,
                             Pref_Tarif = c.Pref_Tarif ?? 0,
                             Description_Bahasa = c.Description_Bahasa,
                             RemandIndicator = c.RemandIndicator,
                             UTN = c.UTN,
                             ChangedOMCode = c.ChangedOMCode,
                             OrderMethods = OrderMethods.GetList()

            };

                return tb.FirstOrDefault();
            }

            //var item = GetList().Where(w => w.PartsID == id).FirstOrDefault();
            //return item;
        }



        public static List<Data.Domain.PartsListNumber> GetListByRegulationCode_Paging(string RegulationCode, int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RegulationCode", !string.IsNullOrWhiteSpace(RegulationCode) ? RegulationCode.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsListNumber>("[imex].[spGetPartNumberByRegulationCode_Paging] @RegulationCode, @next, @keysearch, 1", parameters).ToList();

                return data;
            }

        }

        public static int GetListByRegulationCode_Count(string RegulationCode, int next, string keysearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RegulationCode", !string.IsNullOrWhiteSpace(RegulationCode) ? RegulationCode.ToString() : string.Empty));
                parameterList.Add(new SqlParameter("@next", next != null ? next : 0));
                parameterList.Add(new SqlParameter("@keysearch", !string.IsNullOrWhiteSpace(keysearch) ? keysearch : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[imex].[spGetPartNumberByRegulationCode_Paging] @RegulationCode, @next, @keysearch, 2", parameters).ToList();

                return data.FirstOrDefault();
            }

        }

        public static List<Data.Domain.PartsListNumber> GetListByRegulationCode(string RegulationCode)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@RegulationCode", !string.IsNullOrWhiteSpace(RegulationCode) ? RegulationCode.ToString() : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsListNumber>("[imex].[spGetPartNumberByRegulationCode] @RegulationCode", parameters).ToList();

                return data;
            }

        }

        public async static Task<int> Update(Data.Domain.PartsList itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
                //itm.Status = 1;
            }

            itm.ModifiedBy = Domain.SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var ret = await db.CreateRepository<Data.Domain.PartsList>().CrudAsync(dml, itm);
                if (ret > 0)
                    return UpdateCache(itm, dml);

                return ret;
            }
        }

        public async static Task<int> Update(Data.Domain.PartsNumberList itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
                itm.DeletionFlag = 1;
            }

            itm.ModifiedBy = Domain.SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var ret = await db.CreateRepository<Data.Domain.PartsNumberList>().CrudAsync(dml, itm);
                if (ret > 0)
                    return 0;// UpdateCache(itm, dml);

                return ret;
            }
        }

        private static object s_syncObject = new object();
        private static int UpdateCache(Data.Domain.PartsList item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetListById(item.PartsID);
                if (newlist.Count() > 0)
                {
                    newlist.RemoveAll(w => w.PartsID == item.PartsID);
                }

                if (dml != "D" && item.OMID != null)
                {
                    var om = Master.OrderMethods.GetId(item.OMID);
                    item.OMCode = om.OMCode;
                    newlist.Add(item);
                }

                _cacheManager.Remove(cacheName);
                _cacheManager.Set(cacheName, newlist);
            }

            return 0;
        }

        private static int UpdateCache(Data.Domain.PartsNumberList item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetListByNewId(item.PartsID);
                if (newlist.Count() > 0)
                {
                    newlist.RemoveAll(w => w.PartsID == item.PartsID);
                }

                if (dml != "D" && item.OMID != null)
                {
                    var om = Master.OrderMethods.GetId(item.OMID);
                    item.OMCode = om.OMCode;
                    newlist.Add(item);
                }

                _cacheManager.Remove(cacheName);
                _cacheManager.Set(cacheName, newlist);
            }

            return 0;
        }

        public static int UpdateBulk(List<PartsList> list, String dml)
        {
            var ret = 0;
            using (var ts = new TransactionScope())
            {
                using (var db = new RepositoryFactory(new EfDbContext()))
                {
                    if (dml == "I")
                    {
                        foreach (var d in list)
                        {

                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;

                            ret = db.CreateRepository<Data.Domain.PartsList>().CRUD("I", d);
                            UpdateCache(d, dml);
                        }
                    }

                    ts.Complete();
                    return ret;
                }
            }
        }

        public static int UpdateBulkNew(List<PartsList> list, String dml)
        {
            var ret = 0;
            string partsNumber = string.Empty;
            using (var ts = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { Timeout = new TimeSpan(5, 0, 0) }))
            {
                try
                {
                    string connString = ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connString))
                    {
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);

                        bulkCopy.DestinationTableName = "mp.PartsList";
                        bulkCopy.BulkCopyTimeout = 3000;

                        // Add your column mappings here
                        bulkCopy.ColumnMappings.Add("PartsNumber", "PartsNumber");
                        bulkCopy.ColumnMappings.Add("ManufacturingCode", "ManufacturingCode");
                        bulkCopy.ColumnMappings.Add("PartsName", "PartsName");
                        bulkCopy.ColumnMappings.Add("Description", "Description");
                        bulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                        bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                        bulkCopy.ColumnMappings.Add("PPNBM", "PPNBM");
                        bulkCopy.ColumnMappings.Add("Pref_Tarif", "Pref_Tarif");
                        bulkCopy.ColumnMappings.Add("Description_Bahasa", "Description_Bahasa");
                        bulkCopy.ColumnMappings.Add("Add_Change", "Add_Change");
                        bulkCopy.ColumnMappings.Add("EntryBy", "EntryBy");
                        bulkCopy.ColumnMappings.Add("ModifiedBy", "ModifiedBy");

                        connection.Open();

                        // write the data in the “dataTable”
                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(list))
                        {
                            table.Load(reader);
                        }

                        bulkCopy.WriteToServer(table);

                        connection.Close();
                    }

                    ts.Complete();

                    string key = string.Format(cacheName);

                    _cacheManager.Remove(key);

                    return ret;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                        throw new Exception("Error when Insert Data Parts List. Error Message: " + ex.Message);
                    else
                        throw new Exception("Error when Insert Data Parts List. Error Message: " + ex.InnerException.Message);
                }
            }
        }

        public static List<Data.Domain.PartsList> GetListTable()
        {
            string key = string.Format(cacheName);
            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.EfDbContext())
                {
                    var tbl = from c in db.PartsLists
                              join h in db.OrderMethods on c.OMID equals h.OMID into ji
                              from j in ji.DefaultIfEmpty()
                              select new PartListTable
                              {
                                  PartsID = c.PartsID,
                                  PartsNumber = c.PartsNumber,
                                  //PartsNumberReformat = c.PartsNumberReformat,
                                  ManufacturingCode = c.ManufacturingCode,
                                  PartsName = c.PartsName,
                                  Description = c.Description,
                                  //Status = c.Status,
                                  OMID = j.OMID,
                                  EntryDate = c.EntryDate,
                                  ModifiedDate = c.ModifiedDate,
                                  EntryBy = c.EntryBy,
                                  ModifiedBy = c.ModifiedBy,
                                  OMCode = j.OMCode,
                                  PPNBM = c.PPNBM,
                                  Add_Change = c.Add_Change,
                                  Description_Bahasa = c.Description_Bahasa,
                                  Pref_Tarif = c.Pref_Tarif
                              };

                    return ConvertToPartList(tbl.ToList());
                }
            });
            return list;
        }

        public static List<Data.Domain.PartsList> GetListTable(string name)
        {
            string key = string.Format(cacheName);
            if (string.IsNullOrWhiteSpace(name))
                return GetList();

            using (var db = new Data.EfDbContext())
            {
                var tbl = from c in db.PartsLists.Where(w => name == "" || (w.PartsName + "|" + w.PartsNumber + "|" + w.Description).Trim().ToLower().Contains(name))
                          join h in db.OrderMethods on c.OMID equals h.OMID into ji
                          from j in ji.DefaultIfEmpty()
                          select new PartListTable
                          {
                              PartsID = c.PartsID,
                              PartsNumber = c.PartsNumber,
                              //PartsNumberReformat = c.PartsNumberReformat,
                              ManufacturingCode = c.ManufacturingCode,
                              PartsName = c.PartsName,
                              Description = c.Description,
                              //Status = c.Status,
                              OMID = j.OMID,
                              EntryDate = c.EntryDate,
                              ModifiedDate = c.ModifiedDate,
                              EntryBy = c.EntryBy,
                              ModifiedBy = c.ModifiedBy,
                              OMCode = j.OMCode,
                              PPNBM = c.PPNBM,
                              Add_Change = c.Add_Change,
                              Description_Bahasa = c.Description_Bahasa,
                              Pref_Tarif = c.Pref_Tarif
                          };

                return ConvertToPartList(tbl.ToList());
            }
        }

        public static List<PartsList> ConvertToPartList(List<PartListTable> dataList)
        {
            List<PartsList> partList = new List<PartsList>();
            foreach (var d in dataList)
            {
                partList.Add(new PartsList()
                {
                    EntryBy = d.EntryBy,
                    Description = d.Description,
                    EntryDate = d.EntryDate,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedDate = d.ModifiedDate,
                    OMCode = d.OMCode,
                    OMID = d.OMID,
                    PartsID = d.PartsID,
                    PartsName = d.PartsName,
                    PartsNumber = d.PartsNumber,
                    //PartsNumberReformat = d.PartsNumberReformat,
                    //Status = d.Status,
                    ManufacturingCode = d.ManufacturingCode,
                    PPNBM = d.PPNBM,
                    Add_Change = d.Add_Change,
                    Description_Bahasa = d.Description_Bahasa,
                    Pref_Tarif = d.Pref_Tarif
                });
            }

            return partList;
        }
    }
}


