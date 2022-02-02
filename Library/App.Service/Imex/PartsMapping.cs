using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using App.Data.Caching;
using System.Data.SqlClient;
using App.Data.Domain;
using System.Configuration;
using System.Data;
using App.Framework.Mvc.UI.Sorting;
using FastMember;

namespace App.Service.Imex
{
    public class PartsMapping
    {
        private const string cacheName = "App.imex.PartsMapping";
        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        private static List<Data.Domain.PartsMapping> RefreshList(int offset, int pagingsize)
        {

            using (var db = new Data.EfDbContext())
            {
                var rela = (from c in db.PartsMappings
                            from h in db.HSCodeLists.Where(w => w.HSID == c.HSId).DefaultIfEmpty()
                            from p in db.PartsLists.Where(w => w.PartsID == c.PartsId).DefaultIfEmpty()
                            from o in db.OrderMethods.Where(w => w.OMID == p.OMID).DefaultIfEmpty()
                            select new
                            {
                                c,
                                h.HSCode,
                                //h.HSCodeReformat,
                                HSDescription = h.Description,
                                p.PartsName,
                                p.PartsNumber,
                                //p.PartsNumberReformat,
                                c.ManufacturingCode,
                                p.PPNBM,
                                p.Pref_Tarif,
                                p.Description_Bahasa,
                                p.Add_Change,
                                omCode = (o == null ? "" : o.OMCode)
                            }).OrderBy(o => o.c.PartsId)
                    .Skip(offset).Take(pagingsize)
                    .ToList();

                var list = from c in rela
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.c.PartsMappingID,
                               PartsId = c.c.PartsId,
                               ManufacturingCode = c.ManufacturingCode,
                               PPNBM = c.PPNBM,
                               Pref_Tarif = c.Pref_Tarif,
                               Add_Change = c.Add_Change,
                               Description_Bahasa = c.Description_Bahasa,
                               HSId = c.c.HSId,
                               Status = c.c.Status,
                               EntryDate = c.c.EntryDate,
                               ModifiedDate = c.c.ModifiedDate,
                               EntryBy = c.c.EntryBy,
                               ModifiedBy = c.c.ModifiedBy,
                               ActionUser = c.c.ActionUser,
                               Source = c.c.Source,
                               HSCode = c.HSCode,
                               HSCodeCap = c.HSCode.ToString() + " ~ " + (c.HSDescription + "").Replace(".", ""),
                               HSDescription = c.HSDescription,
                               PartsName = c.PartsName,
                               PartsNameCap = c.PartsNumber + " - " + c.PartsName + " ~ " + c.omCode,
                               //PartsNameCap = c.PartsNumber + " - " + c.PartsName
                               OMCode = c.omCode
                           };

                return list.ToList();

                #region old
                /*
								var tbl = db.PartsMappings.ToListAsync().Result.ToList();

								var list = from c in tbl
													 from h in Service.Master.HSCodeLists.GetList().Where(w => w.HSID == c.HSId)
													 from p in Service.Master.PartsLists.GetList().Where(w => w.PartsID == c.PartsId)
													 select new Data.Domain.PartsMapping()
													 {
														 PartsMappingID = c.PartsMappingID,
														 PartsId = c.PartsId,
														 HSId = c.HSId,
														 Status = c.Status,
														 EntryDate = c.EntryDate,
														 ModifiedDate = c.ModifiedDate,
														 EntryBy = c.EntryBy,
														 ModifiedBy = c.ModifiedBy,
														 ActionUser = c.ActionUser,
														 Source = c.Source,
														 HSCode = h.HSCode,
														 HSCodeReformat = h.HSCodeReformat,
														 HSDescription = h.Description,
														 PartsName = p.PartsName,
														 PartsNumber = p.PartsNumber,
														 PartsNameCap = p.PartsNumber + " - " + p.PartsName + " ~ " + p.OMCode,
														 PartsNumberReformat = p.PartsNumberReformat,
														 OMCode = p.OMCode
													 };
				 */
                #endregion
            }
        }

        public static List<Data.Domain.PartsMapping> RefreshListWithPage(int offset, int pagingsize, string HSDescription, string PartsName, string PartsId, string HSId, string ManufacturingCode, string OmCode)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@offset", offset != null ? offset : 0));
                parameterList.Add(new SqlParameter("@pagingsize", pagingsize != null ? pagingsize : 0));
                parameterList.Add(new SqlParameter("@HSDescription", !string.IsNullOrWhiteSpace(HSDescription) ? HSDescription : string.Empty));
                parameterList.Add(new SqlParameter("@PartsName", !string.IsNullOrWhiteSpace(PartsName) ? PartsName : string.Empty));
                parameterList.Add(new SqlParameter("@PartsId", !string.IsNullOrWhiteSpace(PartsId) ? PartsId : string.Empty));
                parameterList.Add(new SqlParameter("@HSId", !string.IsNullOrWhiteSpace(HSId) ? HSId : string.Empty));
                parameterList.Add(new SqlParameter("@ManufacturingCode", !string.IsNullOrWhiteSpace(ManufacturingCode) ? ManufacturingCode : string.Empty));
                parameterList.Add(new SqlParameter("@OmCode", !string.IsNullOrWhiteSpace(OmCode) ? OmCode : string.Empty));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.PartsMapping>("[Imex].[spGetPartmapping] @offset, @pagingsize, @HSDescription, @PartsName, @PartsId, @HSId, @ManufacturingCode, @OmCode, 1", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.PartsMapping> GetList()
        {
            var tbl = RefreshList(1, 10);
            return tbl;
        }

        public static List<Data.Domain.PartsMapping> GetList(int Id)
        {
            using (var db = new Data.EfDbContext())
            {
                var rela = (from c in db.PartsMappings.Where(w => w.PartsMappingID == Id)
                            from h in db.HSCodeLists.Where(w => w.HSID == c.HSId).DefaultIfEmpty()
                            from p in db.PartsLists.Where(w => w.PartsID == c.PartsId).DefaultIfEmpty()
                            from o in db.OrderMethods.Where(w => w.OMID == p.OMID).DefaultIfEmpty()
                            select new
                            {
                                c,
                                h.HSCode,
                                //h.HSCodeReformat,
                                HSDescription = h.Description,
                                p.PartsName,
                                p.PartsNumber,
                                //p.PartsNumberReformat,
                                c.ManufacturingCode,
                                p.PPNBM,
                                p.Pref_Tarif,
                                p.Description_Bahasa,
                                p.Add_Change,
                                omCode = (o == null ? "" : o.OMCode)
                            }).ToListAsync().Result.ToList();

                var list = from c in rela
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.c.PartsMappingID,
                               PartsId = c.c.PartsId,
                               ManufacturingCode = c.ManufacturingCode,
                               PPNBM = c.PPNBM,
                               Pref_Tarif = c.Pref_Tarif,
                               Add_Change = c.Add_Change,
                               Description_Bahasa = c.Description_Bahasa,
                               HSId = c.c.HSId,
                               Status = c.c.Status,
                               EntryDate = c.c.EntryDate,
                               ModifiedDate = c.c.ModifiedDate,
                               EntryBy = c.c.EntryBy,
                               ModifiedBy = c.c.ModifiedBy,
                               ActionUser = c.c.ActionUser,
                               Source = c.c.Source,
                               HSCode = c.HSCode,
                               HSCodeCap = c.HSCode.ToString() + " ~ " + (c.HSDescription + "").Replace(".", ""),
                               HSDescription = c.HSDescription,
                               PartsName = c.PartsName,
                               PartsNumber = c.PartsNumber,
                               PartsNameCap = c.PartsNumber + " - " + c.PartsName + " ~ " + c.omCode,
                               OMCode = c.omCode
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.PartsMapping> GetListUnmapping(int Id)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = new Data.Domain.PartsMapping();

                var rela = (from p in db.PartsLists.Where(x => x.PartsID == Id)
                            from c in db.PartsMappings.Where(x => x.PartsId == p.PartsID).DefaultIfEmpty()
                            from h in db.HSCodeLists.Where(w => w.HSID == c.HSId).DefaultIfEmpty()
                            from o in db.OrderMethods.Where(w => w.OMID == p.OMID).DefaultIfEmpty()
                            select new
                            {
                                p,
                                h.HSCode,
                                //h.HSCodeReformat,
                                HSDescription = h.Description,
                                p.Description,
                                p.Description_Bahasa,
                                p.PartsNumber,
                                //p.PartsNumberReformat,
                                c.ManufacturingCode,
                                p.PPNBM,
                                p.Pref_Tarif,
                                p.Add_Change,
                                omCode = (o == null ? "" : o.OMCode)
                            }).ToListAsync().Result.ToList();

                var list = from c in rela
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.p.PartsID,
                               PartsId = c.p.PartsID,
                               ManufacturingCode = c.ManufacturingCode,
                               PPNBM = c.PPNBM,
                               Pref_Tarif = c.Pref_Tarif,
                               Add_Change = c.Add_Change,
                               Description_Bahasa = c.Description_Bahasa == null ? c.Description : c.Description_Bahasa,
                               HSId = 0,
                               Status = 0,
                               EntryDate = c.p.EntryDate,
                               ModifiedDate = c.p.ModifiedDate,
                               EntryBy = c.p.EntryBy,
                               ModifiedBy = c.p.ModifiedBy,
                               ActionUser = string.Empty,
                               Source = string.Empty,
                               HSCode = string.Empty,
                               HSCodeCap = string.Empty,
                               HSDescription = string.Empty,
                               PartsName = string.Empty,
                               PartsNumber = c.PartsNumber,
                               PartsNameCap = c.PartsNumber + " ~ " + c.omCode,
                               OMCode = c.omCode
                           };

                return list.ToList();
            }
        }

        public static bool IfExist(int PartsMappingID, int PartsId, int HSId, int Status)
        {
            using (var db = new Data.EfDbContext())
            {
                var rela = db.PartsMappings.Where(w => w.PartsMappingID != PartsMappingID && w.PartsId == PartsId && w.HSId == HSId && w.Status == Status).FirstOrDefault();

                if (rela != null)
                    return true;

                return false;
            }
        }

        public static bool DataExist(int PartsID, int HSId, string MC)
        {
            using (var db = new Data.EfDbContext())
            {
                var rela = db.PartsMappings.Where(w => w.PartsId == PartsID && w.HSId == HSId && w.ManufacturingCode == MC).FirstOrDefault();

                if (rela != null)
                    return true;

                return false;
            }
        }

        public static List<Data.Domain.PartsMapping> GetListPagingServer(int offset, int pagingsize)
        {
            var tbl = RefreshList(offset, pagingsize);
            return tbl;
        }

        public static List<Data.Domain.SP_PartsMapping> SP_GetListPerPage(int StartNum, int EndNum, int status, string HSDescription, string PartsName,
            string selPartsList_Ids, string selHSCodeList_Ids, string ManufacturingCode, string selOrderMethods, string OrderBy, bool IsNullHSCode)
        {
            //var name = Param.searchName != null ? Param.searchName.Trim().ToLower() : "";
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 3000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@status", status));
                parameterList.Add(new SqlParameter("@HSDescription", HSDescription));
                parameterList.Add(new SqlParameter("@PartsName", PartsName));
                parameterList.Add(new SqlParameter("@selPartsList_Ids", selPartsList_Ids));
                parameterList.Add(new SqlParameter("@selHSCodeList_Ids", selHSCodeList_Ids));
                parameterList.Add(new SqlParameter("@ManufacturingCode", ManufacturingCode));
                parameterList.Add(new SqlParameter("@selOrderMethods", selOrderMethods));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToLower()));
                parameterList.Add(new SqlParameter("@IsNullHSCode", IsNullHSCode));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_PartsMapping>("[mp].[spRefreshList] @startnum, @endnum, 1, @status, @HSDescription, @PartsName, @selPartsList_Ids, @selHSCodeList_Ids, @ManufacturingCode, @selOrderMethods, @orderby, @IsNullHSCode", parameters).ToList();

                return data.ToList();
            }
        }

        public static List<Int32> SP_GetCountPerPage(int StartNum, int EndNum, int status, string HSDescription, string PartsName,
            string selPartsList_Ids, string selHSCodeList_Ids, string ManufacturingCode, string selOrderMethods, string OrderBy, bool IsNullHSCode)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@status", status));
                parameterList.Add(new SqlParameter("@HSDescription", HSDescription));
                parameterList.Add(new SqlParameter("@PartsName", PartsName));
                parameterList.Add(new SqlParameter("@selPartsList_Ids", selPartsList_Ids));
                parameterList.Add(new SqlParameter("@selHSCodeList_Ids", selHSCodeList_Ids));
                parameterList.Add(new SqlParameter("@ManufacturingCode", ManufacturingCode));
                parameterList.Add(new SqlParameter("@selOrderMethods", selOrderMethods));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy.Trim().ToLower()));
                parameterList.Add(new SqlParameter("@IsNullHSCode", IsNullHSCode));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[mp].[spRefreshList] @startnum, @endnum, 2, @status, @HSDescription, @PartsName, @selPartsList_Ids, @selHSCodeList_Ids, @ManufacturingCode, @selOrderMethods, @orderby, @IsNullHSCode", parameters).ToList();

                return data.ToList();
            }
        }

        //public static List<Data.Domain.PartsMapping> GetList(Domain.MasterSearchForm crit)
        //{
        //	var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

        //	var list = GetList();
        //	return list.ToList();
        //}

        public static List<Data.Domain.PartsMapping> GetListInsert(List<Data.Domain.PartsMapping> _list, List<Data.Domain.PartsListGroupByPartNumber> psrtList, List<Data.Domain.HSCodeList> hsList)
        {
            using (var db = new Data.EfDbContext())
            {
                List<Data.Domain.PartsMapping> list = new List<Data.Domain.PartsMapping>();

                list = (from c in _list.AsParallel()
                        join p in psrtList.AsParallel().Where(p => p.PartsNumber != null && p.ManufacturingCode != null) on c.PartsNumber.Trim().ToUpper() + c.ManufacturingCode.Trim().ToUpper() 
                        equals (p.PartsNumber.Trim().ToUpper() + p.ManufacturingCode.Trim().ToUpper())
                        into jp
                        from j in jp.DefaultIfEmpty()
                        join h in hsList.AsParallel() on c.HSCode equals (h.HSCode) into jh
                        from j2 in jh.DefaultIfEmpty()
                        select new Data.Domain.PartsMapping()
                        {
                            PartsMappingID = 0,
                            PartsId = j != null ? j.PartsID : 0,
                            HSId = j2!= null ? j2.HSID : 0,
                            HSCode = c.HSCode,
                            PartsNumber = c.PartsNumber,
                            ManufacturingCode = c.ManufacturingCode,
                            Description_Bahasa = c.Description_Bahasa,
                            PPNBM = c.PPNBM,
                            Pref_Tarif = c.Pref_Tarif,
                            Add_Change = c.Add_Change,
                            EntryBy = Domain.SiteConfiguration.UserName,
                            EntryDate = DateTime.Now,
                            ModifiedBy = Domain.SiteConfiguration.UserName,
                            ModifiedDate = DateTime.Now,
                            Status = 1,
                            Source = c.Source,
                            ActionUser = c.ActionUser
                        }).ToList();

                list = list.Where(w => !db.PartsMappings.Any(a => a.PartsId == w.PartsId)).ToList();

                return list;
            }
        }

        public static List<Data.Domain.PartsMapping> GetListUpdate(List<Data.Domain.PartsMapping> _list, List<Data.Domain.PartsListGroupByPartNumber> psrtList, List<Data.Domain.HSCodeList> hsList)
        {
            using (var db = new Data.EfDbContext())
            {
                var partsMapping = (from pl in psrtList
                                    join p in db.PartsMappings on pl.PartsID equals (p.PartsId)
                                    select new Data.Domain.PartsMapping()
                                    {
                                        PartsMappingID = p.PartsMappingID,
                                        PartsId = p.PartsId,
                                        HSId = p.HSId,
                                        HSCode = p.HSCode,
                                        PartsNumber = pl.PartsNumber,
                                        ManufacturingCode = pl.ManufacturingCode,
                                        EntryBy = p.EntryBy,
                                        EntryDate = p.EntryDate,
                                        ModifiedBy = Domain.SiteConfiguration.UserName,
                                        ModifiedDate = DateTime.Today,
                                        Status = 1,
                                        Source = p.Source,
                                        ActionUser = p.ActionUser
                                    }).ToList();

                var list = (from l in _list.AsParallel()
                            join pm in partsMapping.AsParallel() on l.PartsNumber.Trim().ToUpper() + l.ManufacturingCode.Trim().ToUpper() equals (pm.PartsNumber.Trim().ToUpper() + pm.ManufacturingCode.Trim().ToUpper())
                            from h in hsList.Where(w => w.HSCode == l.HSCode).DefaultIfEmpty()
                            select new Data.Domain.PartsMapping()
                            {
                                PartsMappingID = pm.PartsMappingID,
                                PartsId = pm.PartsId,
                                HSId = h.HSID,
                                HSCode = h.HSCode,
                                PartsNumber = l.PartsNumber,
                                ManufacturingCode = l.ManufacturingCode,
                                Description_Bahasa = l.Description_Bahasa,
                                PPNBM = l.PPNBM,
                                Pref_Tarif = l.Pref_Tarif,
                                Add_Change = l.Add_Change,
                                EntryBy = pm.EntryBy,
                                EntryDate = pm.EntryDate,
                                ModifiedBy = Domain.SiteConfiguration.UserName,
                                ModifiedDate = DateTime.Today,
                                Status = 1,
                                Source = l.Source,
                                ActionUser = l.ActionUser
                            }).ToList();

                return list;
            }
        }

        public static Data.Domain.PartsMapping GetId(int id)
        {
            if (id == 0)
                return new Data.Domain.PartsMapping();

            using (var db = new Data.EfDbContext())
            {
                var rela = (from c in db.PartsMappings.Where(p => p.PartsMappingID == id)
                            from h in db.HSCodeLists.Where(w => w.HSID == c.HSId).DefaultIfEmpty()
                            from p in db.PartsLists.Where(w => w.PartsID == c.PartsId).DefaultIfEmpty()
                            from o in db.OrderMethods.Where(w => w.OMID == p.OMID).DefaultIfEmpty()
                            select new
                            {
                                c,
                                h.HSCode,
                                //h.HSCodeReformat,
                                HSDescription = h.Description,
                                p.PartsName,
                                p.PartsNumber,
                                //p.PartsNumberReformat,
                                c.ManufacturingCode,
                                p.PPNBM,
                                p.Pref_Tarif,
                                p.Description_Bahasa,
                                p.Add_Change,
                                omCode = (o == null ? "" : o.OMCode)
                            }).ToListAsync().Result.ToList();

                var list = from c in rela
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.c.PartsMappingID,
                               PartsId = c.c.PartsId,
                               ManufacturingCode = c.ManufacturingCode,
                               PPNBM = c.PPNBM,
                               Pref_Tarif = c.Pref_Tarif,
                               Add_Change = c.Add_Change,
                               Description_Bahasa = c.Description_Bahasa,
                               HSId = c.c.HSId,
                               Status = c.c.Status,
                               EntryDate = c.c.EntryDate,
                               ModifiedDate = c.c.ModifiedDate,
                               EntryBy = c.c.EntryBy,
                               ModifiedBy = c.c.ModifiedBy,
                               ActionUser = c.c.ActionUser,
                               Source = c.c.Source,
                               HSCode = c.HSCode,
                               HSCodeCap = c.HSCode.ToString() + " ~ " + (c.HSDescription + "").Replace(".", ""),
                               HSDescription = c.HSDescription,
                               PartsName = c.PartsName,
                               PartsNumber = c.PartsNumber,
                               PartsNameCap = c.PartsNumber + " - " + c.PartsName + " ~ " + c.omCode,
                               OMCode = c.omCode
                           };

                var item = list.FirstOrDefault();
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.HSCodeCap))
                    {
                        item.HSCodeCap = item.HSCode + " ~ " + ("" + item.HSDescription).Replace(".", "");
                    }
                }

                return item;
            }
        }

        public static Data.Domain.PartsMapping GetIdUnmapping(int id)
        {
            if (id == 0)
                return new Data.Domain.PartsMapping();

            using (var db = new Data.EfDbContext())
            {
                var item = new Data.Domain.PartsMapping();

                var rela = (from p in db.PartsLists.Where(x => x.PartsID == id)
                            from c in db.PartsMappings.Where(x => x.PartsId == p.PartsID).DefaultIfEmpty()
                            from h in db.HSCodeLists.Where(w => w.HSID == c.HSId).DefaultIfEmpty()
                            from o in db.OrderMethods.Where(w => w.OMID == p.OMID).DefaultIfEmpty()
                            select new
                            {
                                p,
                                h.HSCode,
                                //h.HSCodeReformat,
                                HSDescription = h.Description,
                                p.Description,
                                p.Description_Bahasa,
                                p.PartsNumber,
                                //p.PartsNumberReformat,
                                c.ManufacturingCode,
                                p.PPNBM,
                                p.Pref_Tarif,
                                p.Add_Change,
                                omCode = (o == null ? "" : o.OMCode)
                            }).ToListAsync().Result.ToList();

                var list = from c in rela
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.p.PartsID,
                               PartsId = c.p.PartsID,
                               ManufacturingCode = c.ManufacturingCode,
                               PPNBM = c.PPNBM,
                               Pref_Tarif = c.Pref_Tarif,
                               Add_Change = c.Add_Change,
                               Description_Bahasa = c.Description_Bahasa == null ? c.Description : c.Description_Bahasa,
                               HSId = 0,
                               Status = 0,
                               EntryDate = c.p.EntryDate,
                               ModifiedDate = c.p.ModifiedDate,
                               EntryBy = c.p.EntryBy,
                               ModifiedBy = c.p.ModifiedBy,
                               ActionUser = string.Empty,
                               Source = string.Empty,
                               HSCode = string.Empty,
                               HSCodeCap = string.Empty,
                               HSDescription = string.Empty,
                               PartsName = string.Empty,
                               PartsNumber = c.PartsNumber,
                               PartsNameCap = c.PartsNumber + " ~ " + c.omCode,
                               OMCode = c.omCode
                           };

                item = list.FirstOrDefault();
                if (item != null)
                {
                    if (string.IsNullOrEmpty(item.HSCodeCap))
                    {
                        item.HSCodeCap = item.HSCode + " ~ " + ("" + item.HSDescription).Replace(".", "");
                    }
                }

                return item;
            }
        }

        public static List<Data.Domain.PartsMapping> GetListHistory(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.PartsMappingHistories.Where(w => w.PartsMappingID == id).ToList()
                           from h in Service.Master.HSCodeLists.GetListById(c.HSId).DefaultIfEmpty()
                           from p in Service.Master.PartsLists.GetListById(c.PartsId).DefaultIfEmpty()
                           select new Data.Domain.PartsMapping()
                           {
                               PartsMappingID = c.PartsMappingID,
                               PartsId = c.PartsId,
                               ManufacturingCode = c.ManufacturingCode,
                               HSId = c.HSId,
                               Status = c.Status,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               ActionUser = c.ActionUser,
                               Source = c.Source,
                               HSCode = h != null ? h.HSCode : "",
                               HSDescription = h != null ? h.Description : "",
                               PartsName = p != null ? p.PartsName : "",
                               PartsNumber = p != null ? p.PartsNumber : "",
                               OMCode = p != null ? p.OMCode : ""
                           };

                return list.ToList();
            }
        }

        public async static Task<int> Update(Data.Domain.PartsMapping itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    //int id = itm.PartsMappingID;
                    //try { id = db.CreateRepository<Data.Domain.PartsMapping>().GetAll().Max(c => c.PartsMappingID); }
                    //catch { }
                    //itm.PartsMappingID = id + 1
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                var pl = Service.Master.PartsLists.GetId(itm.PartsId);

                if (pl != null)
                {
                    if (string.IsNullOrWhiteSpace(pl.PartsName)) pl.PartsName = "-";

                    pl.Add_Change = itm.Add_Change;
                    pl.Description_Bahasa = itm.Description_Bahasa;
                    pl.PPNBM = itm.PPNBM;
                    pl.Pref_Tarif = itm.Pref_Tarif;

                    await Service.Master.PartsLists.Update(pl, "U");
                }

                if (dml != "I")
                {
                    try
                    {
                        var his = GetId(itm.PartsMappingID);
                        var itemHis = new Data.Domain.PartsMappingHistory
                        {
                            PartsMappingID = his.PartsMappingID,
                            PartsId = his.PartsId,
                            HSId = his.HSId,
                            Status = his.Status,
                            ManufacturingCode = his.ManufacturingCode,
                            EntryDate = his.EntryDate,
                            ModifiedDate = his.ModifiedDate,
                            EntryBy = his.EntryBy,
                            ModifiedBy = his.ModifiedBy,
                            ActionUser = string.IsNullOrEmpty(his.ActionUser) ? "Update" : his.ActionUser,
                            Source = string.IsNullOrEmpty(his.Source) ? "Form" : his.Source,
                        };

                        await db.CreateRepository<Data.Domain.PartsMappingHistory>().CrudAsync("I", itemHis);
                    }
                    catch { }
                }

                await db.CreateRepository<Data.Domain.PartsMapping>().CrudAsync(dml, itm);
                return UpdateCache(itm, dml);
            }
        }

        public async static Task<int> UpdateUnmapping(Data.Domain.PartsMapping itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    //int id = itm.PartsMappingID;
                    //try { id = db.CreateRepository<Data.Domain.PartsMapping>().GetAll().Max(c => c.PartsMappingID); }
                    //catch { }
                    //itm.PartsMappingID = id + 1
                    itm.PartsMappingID = 0;
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                var pl = Service.Master.PartsLists.GetId(itm.PartsId);

                if (pl != null)
                {
                    if (string.IsNullOrWhiteSpace(pl.PartsName)) pl.PartsName = "-";

                    pl.Add_Change = itm.Add_Change;
                    pl.Description_Bahasa = itm.Description_Bahasa;
                    pl.PPNBM = itm.PPNBM;
                    pl.Pref_Tarif = itm.Pref_Tarif;

                    await Service.Master.PartsLists.Update(pl, "U");
                }

                //if (dml != "I")
                //{
                //    try
                //    {
                //        var his = GetIdUnmapping(itm.PartsMappingID);
                //        var itemHis = new Data.Domain.PartsMappingHistory
                //        {
                //            PartsMappingID = his.PartsMappingID,
                //            PartsId = his.PartsId,
                //            HSId = his.HSId,
                //            Status = his.Status,
                //            ManufacturingCode = his.ManufacturingCode,
                //            EntryDate = his.EntryDate,
                //            ModifiedDate = his.ModifiedDate,
                //            EntryBy = his.EntryBy,
                //            ModifiedBy = his.ModifiedBy,
                //            ActionUser = string.IsNullOrEmpty(his.ActionUser) ? "Update" : his.ActionUser,
                //            Source = string.IsNullOrEmpty(his.Source) ? "Form" : his.Source,
                //        };

                //        await db.CreateRepository<Data.Domain.PartsMappingHistory>().CrudAsync("I", itemHis);
                //    }
                //    catch { }
                //}

                await db.CreateRepository<Data.Domain.PartsMapping>().CrudAsync(dml, itm);
                return UpdateCacheUnmapping(itm, dml);
            }
        }

        public static int UpdateBulk(List<Data.Domain.PartsMapping> list, string dml)
        {
            var ret = 0;
            using (TransactionScope ts = new System.Transactions.TransactionScope())
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {

                    if (dml == "I")
                    {
                        foreach (var d in list)
                        {
                            //int id = 0;
                            //try { id = db.CreateRepository<Data.Domain.PartsMapping>().GetAll().Max(c => c.PartsMappingID); }
                            //catch { }
                            //d.PartsMappingID = id + 1;

                            d.ModifiedBy = Domain.SiteConfiguration.UserName;
                            d.ModifiedDate = DateTime.Now;
                            d.EntryBy = Domain.SiteConfiguration.UserName;
                            d.EntryDate = DateTime.Now;

                            ret = db.CreateRepository<Data.Domain.PartsMapping>().CRUD("I", d);
                            UpdateCache(d, dml);
                        }
                    }

                    ts.Complete();
                    return ret;
                }
            }
        }

        public static int InsertBulkNew(List<Data.Domain.PartsMapping> list)
        {
            var ret = 0;
            using (TransactionScope ts = new System.Transactions.TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { Timeout = new TimeSpan(10, 0, 0) }))
            {
                try
                {
                    string connString = ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(connString))
                    {
                        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);

                        bulkCopy.DestinationTableName = "imex.PartsMapping";
                        bulkCopy.BulkCopyTimeout = 3000;

                        // Add your column mappings here
                        bulkCopy.ColumnMappings.Add("PartsId", "PartsId");
                        bulkCopy.ColumnMappings.Add("HSId", "HSId");
                        bulkCopy.ColumnMappings.Add("ManufacturingCode", "ManufacturingCode");
                        bulkCopy.ColumnMappings.Add("ActionUser", "ActionUser");
                        bulkCopy.ColumnMappings.Add("Source", "Source");
                        bulkCopy.ColumnMappings.Add("Status", "Status");
                        bulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                        bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                        bulkCopy.ColumnMappings.Add("EntryBy", "EntryBy");
                        bulkCopy.ColumnMappings.Add("ModifiedBy", "ModifiedBy");

                        connection.Open();

                        // write the data in the “dataTable”     
                        DataTable table = new DataTable();
                        using (var reader = ObjectReader.Create(list,
                            "PartsId", "HSId", "ManufacturingCode", "ActionUser", "Source", "Status", "EntryDate", "ModifiedDate", "EntryBy", "ModifiedBy"))
                        {
                            table.Load(reader);
                        }

                        bulkCopy.WriteToServer(table);

                        connection.Close();
                    }

                    int pageSize = 1000, totPage = (list.Count() / pageSize) + 1;
                    var lstMap = new List<Data.Domain.PartsMapping>();

                    for (int i = 1; i <= totPage; i++)
                    {
                        int offset = pageSize * (i - 1);
                        lstMap = list.AsParallel().ToList();

                        lstMap = lstMap
                                    .Skip(offset)
                                    .Take(pageSize)
                                    .ToList();

                        UpdateDataPartsList(lstMap);
                    }

                    ts.Complete();

                    string key = string.Format(cacheName);

                    _cacheManager.Remove(key);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException == null)
                        throw new Exception("Error when Insert Data Parts Mapping. Error Message: " + ex.Message);
                    else
                        throw new Exception("Error when Insert Data Parts Mapping. Error Message: " + ex.InnerException.Message);
                }
            }

            string key2 = string.Format("App.master.PartsLists");

            _cacheManager.Remove(key2);

            return ret;
        }

        public static int UpdateBulkNew(List<Data.Domain.PartsMapping> list)
        {
            var ret = 0;
            using (TransactionScope ts = new System.Transactions.TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { Timeout = new TimeSpan(5, 0, 0) }))
            {
                // write the data in the “dataTable”     
                DataTable table = new DataTable();
                using (var reader = ObjectReader.Create(list,
                    "PartsMappingID", "PartsId", "HSId", "ManufacturingCode", "ModifiedBy"))
                {
                    table.Load(reader);
                }

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("", conn))
                    {
                        try
                        {
                            conn.Open();

                            //Creating temp table on database
                            command.CommandText = "CREATE TABLE #TmpTable (PartsMappingID INT, PartsId INT, HSId INT, ManufacturingCode NVARCHAR(20), ModifiedBy NVARCHAR(50))";
                            command.ExecuteNonQuery();

                            //Bulk insert into temp table
                            using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                            {
                                bulkcopy.BulkCopyTimeout = 3000;
                                bulkcopy.DestinationTableName = "#TmpTable";
                                bulkcopy.WriteToServer(table);
                                bulkcopy.Close();
                            }

                            // Updating destination table, and dropping temp table
                            command.CommandTimeout = 3000;
                            command.CommandText = "UPDATE T SET T.HSID = Temp.HSID, T.ManufacturingCode = Temp.ManufacturingCode, T.ModifiedBy = Temp.ModifiedBy, T.ModifiedDate = GETDATE() " +
                                                  " FROM imex.PartsMapping T INNER JOIN #TmpTable Temp ON T.PartsId = Temp.PartsId; DROP TABLE #TmpTable;";
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException == null)
                                throw new Exception("Error when Update Data Parts Mapping. Error Message: " + ex.Message);
                            else
                                throw new Exception("Error when Update Data Parts Mapping. Error Message: " + ex.InnerException.Message);
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }

                int pageSize = 1000, totPage = (list.Count() / pageSize) + 1;
                var lstMap = new List<Data.Domain.PartsMapping>();

                for (int i = 1; i <= totPage; i++)
                {
                    int offset = pageSize * (i - 1);
                    lstMap = list.AsParallel().ToList();

                    lstMap = lstMap
                                .Skip(offset)
                                .Take(pageSize)
                                .ToList();

                    UpdateDataPartsList(lstMap);
                }

                ts.Complete();

                string key = string.Format(cacheName);

                _cacheManager.Remove(key);

                string key2 = string.Format("App.master.PartsLists");

                _cacheManager.Remove(key2);
            }

            return ret;
        }

        #region old
        //public static int Update(Data.Domain.PartsMapping itm, string dml)
        //{
        //	string userName = Domain.SiteConfiguration.UserName;
        //	itm.ModifiedBy = userName;
        //	itm.ModifiedDate = DateTime.Now;

        //	using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
        //	{
        //		if(dml == "I")
        //		{
        //			//int id = itm.PartsMappingID;
        //			//try { id = db.CreateRepository<Data.Domain.PartsMapping>().GetAll().Max(c => c.PartsMappingID); }
        //			//catch { }
        //			//itm.PartsMappingID = id + 1;

        //			itm.EntryBy = userName;
        //			itm.EntryDate = DateTime.Now;
        //		}

        //		if(dml != "I")
        //		{
        //			try
        //			{
        //				var his = GetId(itm.PartsMappingID);
        //				var itemHis = new Data.Domain.PartsMappingHistory
        //				{
        //					PartsMappingID = his.PartsMappingID,
        //					PartsId = his.PartsId,
        //					HSId = his.HSId,
        //					Status = his.Status,
        //					EntryDate = his.EntryDate,
        //					ModifiedDate = his.ModifiedDate,
        //					EntryBy = his.EntryBy,
        //					ModifiedBy = his.ModifiedBy,
        //					ActionUser = string.IsNullOrEmpty(his.ActionUser) ? "Update" : his.ActionUser,
        //					Source = string.IsNullOrEmpty(his.Source) ? "Form" : his.Source,
        //				};

        //				db.CreateRepository<Data.Domain.PartsMappingHistory>().CRUD("I", itemHis);
        //			}
        //			catch { }
        //		}

        //		db.CreateRepository<Data.Domain.PartsMapping>().CRUD(dml, itm);
        //		return UpdateCache(itm, dml);
        //	}
        //}
        #endregion

        private static object s_syncObject = new object();
        private static int UpdateCache(Data.Domain.PartsMapping item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetList(item.PartsMappingID).ToList();
                if (newlist.Count() > 0)
                {
                    newlist.RemoveAll(w => w.PartsMappingID == item.PartsMappingID);
                }

                if (dml != "D")
                {
                    var p = Master.PartsLists.GetId(item.PartsId);
                    var h = Master.HSCodeLists.GetId(item.HSId);
                    item.PartsMappingID = item.PartsMappingID;
                    item.Status = item.Status;
                    item.ManufacturingCode = item.ManufacturingCode;
                    item.ModifiedDate = item.ModifiedDate;
                    item.ModifiedBy = item.ModifiedBy;
                    item.HSCode = h.HSCode;
                    item.HSDescription = h.Description;
                    item.HSCodeCap = h.HSCode + " ~ " + ("" + h.Description).Replace(".", "");
                    item.PartsName = p.PartsName;
                    item.PartsNumber = p.PartsNumber;
                    item.PartsNameCap = p.PartsNumber + " - " + p.PartsName + " ~ " + p.OMCode;
                    item.OMCode = p.OMCode;

                    newlist.Add(item);
                }

                _cacheManager.Remove(cacheName);
                _cacheManager.Set(cacheName, newlist);
            }

            return 0;
        }

        private static int UpdateCacheUnmapping(Data.Domain.PartsMapping item, string dml)
        {

            lock (s_syncObject)
            {
                var newlist = GetListUnmapping(item.PartsMappingID).ToList();
                if (newlist.Count() > 0)
                {
                    newlist.RemoveAll(w => w.PartsMappingID == item.PartsMappingID);
                }

                if (dml != "D")
                {
                    var p = Master.PartsLists.GetId(item.PartsId);
                    var h = Master.HSCodeLists.GetId(item.HSId);
                    item.PartsMappingID = item.PartsMappingID;
                    item.Status = item.Status;
                    item.ManufacturingCode = item.ManufacturingCode;
                    item.ModifiedDate = item.ModifiedDate;
                    item.ModifiedBy = item.ModifiedBy;
                    item.HSCode = h.HSCode;
                    item.HSDescription = h.Description;
                    item.HSCodeCap = h.HSCode + " ~ " + ("" + h.Description).Replace(".", "");
                    item.PartsName = p.PartsName;
                    item.PartsNumber = p.PartsNumber;
                    item.PartsNameCap = p.PartsNumber + " - " + p.PartsName + " ~ " + p.OMCode;
                    item.OMCode = p.OMCode;

                    newlist.Add(item);
                }

                _cacheManager.Remove(cacheName);
                _cacheManager.Set(cacheName, newlist);
            }

            return 0;
        }

        public static void UpdateDataPartsList(List<Data.Domain.PartsMapping> list)
        {
            DataTable table = new DataTable("Table");

            using (var reader = ObjectReader.Create(list,
                "PartsId", "PPNBM", "Pref_Tarif", "Description_Bahasa", "Add_Change"))
            {
                table.Load(reader);
            }

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand("", conn))
                {
                    try
                    {
                        conn.Open();

                        //Creating temp table on database                     
                        command.CommandTimeout = 3000;
                        command.CommandText = "CREATE TABLE #TmpTable (PartsId INT, PPNBM NUMERIC(18,2), Pref_Tarif NUMERIC(18,2), Description_Bahasa NVARCHAR(MAX), Add_Change NUMERIC(18,2))";
                        command.ExecuteNonQuery();

                        //Bulk insert into temp table
                        using (SqlBulkCopy bulkcopy = new SqlBulkCopy(conn))
                        {
                            bulkcopy.BulkCopyTimeout = 3000;
                            bulkcopy.DestinationTableName = "#TmpTable";
                            bulkcopy.WriteToServer(table);
                            bulkcopy.Close();
                        }

                        // Updating destination table, and dropping temp table
                        command.CommandTimeout = 3000;
                        command.CommandText = "UPDATE T SET T.PPNBM = Temp.PPNBM, T.Pref_Tarif = Temp.Pref_Tarif, T.Description_Bahasa = Temp.Description_Bahasa, T.Add_Change = Temp.Add_Change " +
                                              " FROM mp.PartsList T INNER JOIN #TmpTable Temp ON T.PartsId = Temp.PartsId; DROP TABLE #TmpTable;";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException == null)
                            throw new Exception("Error when Update Data Parts List. Error Message: " + ex.Message);
                        else
                            throw new Exception("Error when Update Data Parts List. Error Message: " + ex.InnerException.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}
