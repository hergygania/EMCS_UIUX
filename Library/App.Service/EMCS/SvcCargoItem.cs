using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Domain;
using System.Dynamic;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

<<<<<<< HEAD
<<<<<<< HEAD

    /// <summary> Services Proses Shipment inbound.</summary>                
=======
    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
>>>>>>> 639d8d0 (Intial commit)
=======

    /// <summary> Services Proses Shipment inbound.</summary>                
>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
    public class SvcCargoItem
    {
        public const string CacheName = "App.EMCS.SvcCargoItem";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static SpCargoItemDetail GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = "EXEC [sp_get_cargo_item_data] @Id='" + id + "'";
                var data = db.Database.SqlQuery<SpCargoItemDetail>(sql).FirstOrDefault();
                return data;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        public static List<SpCargoItemDetail> GetDataByCargoId(List<long> ids)
        {
            try
            {
                var idList = string.Join(",", ids.Select(n => "" + n + "").ToArray());
                
                using (var db = new Data.EmcsContext())
                {
                    var sql = "EXEC [sp_get_cargo_item_data_by_cargoId] @Id='" + idList + "'";
                    var data = db.Database.SqlQuery<SpCargoItemDetail>(sql).ToList();
                    return data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

=======
>>>>>>> 639d8d0 (Intial commit)
=======
        public static List<SpCargoItemDetail> GetDataByCargoId(long id)
=======
        public static List<SpCargoItemDetail> GetDataByCargoId(List<long> ids)
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        {
            try
            {
                var idList = string.Join(",", ids.Select(n => "" + n + "").ToArray());
                
                using (var db = new Data.EmcsContext())
                {
                    var sql = "EXEC [sp_get_cargo_item_data_by_cargoId] @Id='" + idList + "'";
                    var data = db.Database.SqlQuery<SpCargoItemDetail>(sql).ToList();
                    return data;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

>>>>>>> b773f28 (intial commit for changes from himanshu and vijendra)
        public static long Insert(CargoItem item, long itemId, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", item.Id));
                parameterList.Add(new SqlParameter("@ItemId", itemId));
                parameterList.Add(new SqlParameter("@IdCargo", item.IdCargo));
                parameterList.Add(new SqlParameter("@ContainerNumber", item.ContainerNumber ?? ""));
                parameterList.Add(new SqlParameter("@ContainerType", item.ContainerType ?? ""));
                parameterList.Add(new SqlParameter("@ContainerSealNumber", item.ContainerSealNumber ?? ""));
                parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@Length", item.Length ?? 0));
                parameterList.Add(new SqlParameter("@Width", item.Width ?? 0));
                parameterList.Add(new SqlParameter("@Height", item.Height ?? 0));
                parameterList.Add(new SqlParameter("@GrossWeight", item.Gross ?? 0));
                parameterList.Add(new SqlParameter("@NetWeight", item.Net ?? 0));
                parameterList.Add(new SqlParameter("@isDelete", item.IsDelete));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once RedundantNameQualifier
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>("exec [dbo].[sp_insert_update_cargo_item] @Id, @ItemId, @IdCargo, @ContainerNumber, @ContainerType, @ContainerSealNumber, @ActionBy, @Length, @Width, @Height, @GrossWeight, @NetWeight, @isDelete", parameters).FirstOrDefault();
                if (data != null) return data.Id;
            }

            return 0;
        }

        public static long Update(CargoItem item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", item.Id));
                parameterList.Add(new SqlParameter("@ItemId", item.Id));
                parameterList.Add(new SqlParameter("@IdCargo", item.IdCargo));
                parameterList.Add(new SqlParameter("@ContainerNumber", item.ContainerNumber ?? ""));
                parameterList.Add(new SqlParameter("@ContainerType", item.ContainerType ?? ""));
                parameterList.Add(new SqlParameter("@ContainerSealNumber", item.ContainerSealNumber ?? ""));
                parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@Length", item.Length ?? 0));
                parameterList.Add(new SqlParameter("@Width", item.Width ?? 0));
                parameterList.Add(new SqlParameter("@Height", item.Height ?? 0));
                parameterList.Add(new SqlParameter("@GrossWeight", item.Gross ?? 0));
                parameterList.Add(new SqlParameter("@NetWeight", item.Net ?? 0));
                parameterList.Add(new SqlParameter("@isDelete", item.IsDelete));

                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<IdData>("exec [dbo].[sp_insert_update_cargo_item] @Id, @ItemId, @IdCargo, @ContainerNumber, @ContainerType, @ContainerSealNumber, @ActionBy, @Length, @Width, @Height, @GrossWeight, @NetWeight, @isDelete", parameters).FirstOrDefault();
                if (data != null) return data.Id;
            }

            return 0;
        }

        public static List<CargoItem> GetList(GridListFilter filter)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CargoItemData.ToList();
                return data;

            }
        }

        public static bool Remove(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CargoItemData.FirstOrDefault(a => a.Id == id);
                if (data != null) db.CargoItemData.Remove(data);
                db.SaveChanges();
                return true;
            }
        }

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        //public static dynamic GetTotalDataCargo(long id)
        //{
        //    using (var db = new Data.EmcsContext())
        //    {
        //        var dataItem = (from t0 in db.CargoItemData
        //                        join t1 in db.CargoData on t0.IdCargo equals t1.Id
        //                        join t2 in db.CiplItemData on t0.IdCiplItem equals t2.Id
        //                        join t3 in db.CiplData on t2.IdCipl equals t3.Id
        //                        where t0.IdCargo == id
        //                        select new
        //                        {
        //                            t0.Id,
        //                            t0.Net,
        //                            t0.Gross,
        //                            t0.Height,
        //                            t0.Width,
        //                            t3.EdoNo,
        //                            t0.Length,
        //                            t0.IdCargo,
        //                            t2.CaseNumber,
        //                            t2.Sn,
        //                            t2.IdCipl
        //                        }).ToList();

        //        dynamic result = new ExpandoObject();
        //        if (dataItem.Count > 0)
        //        {
        //            result.totalPackage = dataItem.GroupBy(a => new { a.CaseNumber, a.IdCipl }).Count();
        //            //result.totalPackage = dataItem.GroupBy(a => a.CaseNumber && a.IdCipl).Count();
        //            // ReSharper disable once PossibleInvalidOperationException
        //            result.totalNetWeight = dataItem.GroupBy(a => a.IdCargo).Select(a => a.Sum(x => x.Net)).FirstOrDefault().Value;
        //            // ReSharper disable once PossibleInvalidOperationException
        //            result.totalGrossWeight = dataItem.GroupBy(a => a.IdCargo).Select(x => x.Sum(y => y.Gross)).FirstOrDefault().Value;
        //            // ReSharper disable once PossibleInvalidOperationException
        //            decimal volume = dataItem.GroupBy(a => a.Id).Select(x => x.Sum(y => y.Length * y.Width * y.Height)).ToList().Sum(x => x.Value);
        //            decimal volumeM3 = volume / 1000000;
        //            result.totalVolume = volumeM3;
        //        }
        //        else
        //        {
        //            result.totalPackage = 0;
        //            result.totalNetWeight = 0;
        //            result.totalGrossWeight = 0;
        //            result.totalVolume = 0;
        //        }
        //        return result;
        //    }
        //}
        public static dynamic GetTotalDataCargo(long id,string selectvalue) 
<<<<<<< HEAD
=======
        public static dynamic GetTotalDataCargo(long id)
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        {
            using (var db = new Data.EmcsContext())
            {
                var dataItem = (from t0 in db.CargoItemData
                                join t1 in db.CargoData on t0.IdCargo equals t1.Id
                                join t2 in db.CiplItemData on t0.IdCiplItem equals t2.Id
                                join t3 in db.CiplData on t2.IdCipl equals t3.Id
                                where t0.IdCargo == id
                                select new
                                {
                                    t0.Id,
                                    t0.Net,
                                    t0.Gross,
                                    t0.Height,
                                    t0.Width,
                                    t3.EdoNo,
                                    t0.Length,
                                    t0.IdCargo,
                                    t2.CaseNumber,
                                    t2.Sn,
                                    t2.IdCipl
                                }).ToList();

                dynamic result = new ExpandoObject();
                if (dataItem.Count > 0)
                {
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                    if(selectvalue == "NoItem")
                    {
                        result.totalPackage = dataItem.GroupBy(a => new { a.Id, a.IdCipl }).Count();
                    }
                    else
                    {
                        result.totalPackage = dataItem.GroupBy(a => new { a.CaseNumber, a.IdCipl }).Count();
                    }
                    
<<<<<<< HEAD
=======
                    result.totalPackage = dataItem.GroupBy(a => new { a.CaseNumber, a.IdCipl }).Count();
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
                    //result.totalPackage = dataItem.GroupBy(a => a.CaseNumber && a.IdCipl).Count();
                    // ReSharper disable once PossibleInvalidOperationException
                    result.totalNetWeight = dataItem.GroupBy(a => a.IdCargo).Select(a => a.Sum(x => x.Net)).FirstOrDefault().Value;
                    // ReSharper disable once PossibleInvalidOperationException
                    result.totalGrossWeight = dataItem.GroupBy(a => a.IdCargo).Select(x => x.Sum(y => y.Gross)).FirstOrDefault().Value;
                    // ReSharper disable once PossibleInvalidOperationException
                    decimal volume = dataItem.GroupBy(a => a.Id).Select(x => x.Sum(y => y.Length * y.Width * y.Height)).ToList().Sum(x => x.Value);
                    decimal volumeM3 = volume / 1000000;
                    result.totalVolume = volumeM3;
                }
                else
                {
                    result.totalPackage = 0;
                    result.totalNetWeight = 0;
                    result.totalGrossWeight = 0;
                    result.totalVolume = 0;
                }
                return result;
            }
        }

        public static bool GetChangesData(CargoItem newData, long idCiplItem)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = @"select CAST(count(*) as bigint) as ID From dbo.CiplItem t0
                                where t0.Id = " + newData.IdCiplItem + "" +
                          " AND t0.NetWeight = '" + newData.Net + "'" +
                          " AND t0.GrossWeight = '" + newData.Gross + "'" +
                          " AND t0.Height = '" + newData.Height + "'" +
                          " AND t0.Width = '" + newData.Width + "' AND t0.[Length] = '" + newData.Length + "'";
                var item = db.Database.SqlQuery<IdData>(sql).FirstOrDefault();


                if (item != null)
                {
                    var total = item.Id;

                    return (total == 0);
                }
            }

            return false;
        }

        public static bool SaveChangeHistory(string dml, SpCargoItemDetail oldData, CargoItem newData)
        {
            using (var db = new Data.EmcsContext())
            {
                try
                {
                    if (dml == "U")
                    {
                        var dataExists = db.CiplItemUpdateHistories.Where(a => a.IdCargo == oldData.IdCargo && a.IdCipl == oldData.IdCipl && a.IdCiplItem == oldData.IdCiplItem).FirstOrDefault();
                        if (dataExists != null)
                        {
                            dataExists.OldLength = oldData.Length;
                            dataExists.OldWidth = oldData.Width;
                            dataExists.OldHeight = oldData.Height;
                            dataExists.OldNetWeight = oldData.NetWeight;
                            dataExists.OldGrossWeight = oldData.GrossWeight;
                            dataExists.NewLength = newData.Length;
                            dataExists.NewWidth = newData.Width;
                            dataExists.NewHeight = newData.Height;
                            dataExists.NewNetWeight = newData.Net;
                            dataExists.NewGrossWeight = newData.Gross;
                            dataExists.UpdateBy = SiteConfiguration.UserName;
                            dataExists.UpdateDate = DateTime.Now;
                        }

                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        var dataHistory = new CiplItemUpdateHistory();
                        dataHistory.IdCargo = oldData.IdCargo;
                        dataHistory.IdCipl = oldData.IdCipl;
                        dataHistory.IdCiplItem = oldData.IdCiplItem;
                        dataHistory.OldLength = oldData.Length;
                        dataHistory.OldWidth = oldData.Width;
                        dataHistory.OldHeight = oldData.Height;
                        dataHistory.OldNetWeight = oldData.NetWeight;
                        dataHistory.OldGrossWeight = oldData.GrossWeight;
                        dataHistory.NewLength = newData.Length;
                        dataHistory.NewWidth = newData.Width;
                        dataHistory.NewHeight = newData.Height;
                        dataHistory.NewNetWeight = newData.Net;
                        dataHistory.NewGrossWeight = newData.Gross;
                        dataHistory.CreateBy = SiteConfiguration.UserName;
                        dataHistory.CreateDate = DateTime.Now;
                        db.CiplItemUpdateHistories.Add(dataHistory);
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static CiplItemUpdateHistory IsAlreadyUpdate(long idCargo, long idCipl, long idCiplItem)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CiplItemUpdateHistories.Where(a => a.IdCargo == idCargo && a.IdCipl == idCipl && a.IdCiplItem == idCiplItem).FirstOrDefault();
                return data;
            }
        }
    }
}
