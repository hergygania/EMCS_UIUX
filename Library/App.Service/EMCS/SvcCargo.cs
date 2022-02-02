using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Domain;
using System.Dynamic;
using App.Data.Domain.EMCS;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcCargo
    {
        public const string CacheName = "App.EMCS.SvcCargo";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic CargoList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                string Term = "";
                string Order = "";
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
   
                if (crit.Term != null)
                {
                    Term = Regex.Replace(crit.Term, @"[^0-9a-zA-Z]+", "");
                }
               
                if (crit.Order != null)
                {
                    Order = Regex.Replace(crit.Order, @"[^0-9a-zA-Z]+", "");
                }
                

                var sql = @"[dbo].[sp_get_cargo_list] @Username='" + SiteConfiguration.UserName + "', @Search = '" + Term + "' ";
                var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<SpCargoList>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static SpCargoDetail GetCargoById(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC sp_get_cargo_data @Id = " + cargoId;
                SpCargoDetail data = db.Database.SqlQuery<SpCargoDetail>(sql).FirstOrDefault();
                return data;
            }
        }

<<<<<<< HEAD
        public static Data.Domain.EMCS.CargoFormData GetCargoFormDataById(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "select * from cargo where id = " + cargoId;
                Data.Domain.EMCS.CargoFormData data = db.Database.SqlQuery<Data.Domain.EMCS.CargoFormData>(sql).FirstOrDefault();
                return data;
            }
        }
		
		public static bool CargoHisOwned(long id, string userId)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var result = false;

                    var tb = db.CargoData.FirstOrDefault(a => a.Id == id && a.CreateBy == userId);
                    if (tb != null)
                    {
                        result = true;
                    }

                    return result;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

=======
>>>>>>> 639d8d0 (Intial commit)
        public static CargoHeaderData GetCargoHeaderData(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CargoID", cargoId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<CargoHeaderData>(@"[dbo].[SP_GetCargoHeader] @CargoID", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<CargoDetailData> GetCargoDetailData(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = (from ci in db.CargoItemData
                            join cpi in db.CiplItemData on ci.IdCipl equals cpi.Id
                            join cp in db.CiplData on cpi.IdCipl equals cp.Id
                            join it in db.MasterIncoTerm on cp.IncoTerm equals it.Number
                            //where ci.IdContainer == CargoID
                            select new
                            {
                                ID = ci.Id,
                                CiplID = ci.IdCipl,
                                cp.CiplNo,
                                ci.ContainerNumber,
                                ci.ContainerType,
                                ci.ContainerSealNumber,
                                cp.IncoTerm,
                                IncoTermNumber = it.Number,
                                //CaseNumber = ci.CaseNumber,
                                cp.EdoNo,
                                ci.InBoundDa,
                                //CargoDescription = ci.CargoDescription,
                                Length = cpi.Length ?? 0,
                                Width = cpi.Width ?? 0,
                                Height = cpi.Height ?? 0,
                                NetWeight = cpi.NetWeight ?? 0,
                                GrossWeight = cpi.GrossWeight ?? 0
                            }).ToList();
                return data.Select(i => new CargoDetailData
                {
                    Id = i.ID,
                    IdCipl = i.CiplID,
                    //Container = i.Container,
                    IncoTerm = i.IncoTerm,
                    IncoTermNumber = i.IncoTermNumber,
                    //CaseNumber = i.CaseNumber,
                    EdoNo = i.EdoNo,
                    InBoundDa = i.InBoundDa,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    Net = i.NetWeight,
                    Gross = i.GrossWeight
                }).ToList();
            }
        }

        public static dynamic GetConsignee(MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var search = crit.searchName ?? "";
                var data = db.CiplData
                    .Where(x => x.ConsigneeName.Contains(search))
                    .OrderBy(a => a.ConsigneeName).Skip(0).Take(100)
                    .GroupBy(a => a.ConsigneeName).AsQueryable()
                    .Select(a => new { ConsigneeName = a.Key })
                    .ToList();
                return data;
            }
        }

        public static dynamic GetNotifyParty(MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var search = crit.searchName ?? "";
                var data = db.CiplData
                    .Where(x => x.NotifyName.Contains(search))
                    .OrderBy(a => a.NotifyName).Skip(0).Take(100)
                    .GroupBy(a => a.NotifyName).AsQueryable()
                    .Select(a => new { NotifyName = a.Key })
                    .ToList();
                return data;
            }
        }

        public static List<SelectItem> GetCiplColumnList(string colName)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                long startId = 1;
                List<string> data = new List<string>();

                switch (colName)
                {
                    case ("ConsigneeName"):
                        data.AddRange(db.CiplData.Select(i => i.ConsigneeName).Distinct().ToList());
                        break;
                    case ("NotifyName"):
                        data.AddRange(db.CiplData.Select(i => i.NotifyName).Distinct().ToList());
                        break;
                    default:
                        data.AddRange(db.CiplData.Select(i => i.ConsigneeName).Distinct().ToList());
                        break;
                }

                List<SelectItem> list = new List<SelectItem>();
                foreach (var d in data)
                {
                    list.Add(new SelectItem { Id = startId++, Text = d });
                }

                return list;
            }
        }

        public static List<Cipl> GetEdoNoList(string area = "", string pic = "")
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.Database.SqlQuery<Cipl>("exec sp_get_edi_available '" + area + "', '" + pic + "'").ToList();
                return data;
            }
        }

        public static List<SelectItem> GetCiplNoList(string consignee, string notify, string exporttype, string category, string incoterms)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.CiplData.Where(i =>
                    i.ConsigneeName.ToUpper().Equals(consignee.ToUpper()) && i.NotifyName.ToUpper().Equals(notify.ToUpper()) &&
                    i.ExportType.Equals(exporttype) && i.Category.Equals(category) && i.IncoTerm.Equals(incoterms)
                ).Select(i => new { id = i.Id, text = i.CiplNo }).ToList();

                return data.Select(i => new SelectItem { Id = i.id, Text = i.text }).ToList();
            }
        }

        public static List<int> GetCiplNoByCargo(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = (from c in db.CargoData
                                //join ci in db.CargoItemData on c.id equals ci.IdCargo
                                //join cpi in db.CiplItemData on ci.IdCipl equals cpi.Id
                                //join cp in db.CiplData on cpi.IdCipl equals cp.id
                                //where c.id == CargoId
                            select (int)c.Id
                    ).Distinct().ToList();

                return data;
            }
        }

        public static List<Cipl> GetCiplbyCargo(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = @"select * From dbo.cipl where Id in (select IdCipl from dbo.CargoCipl where IdCargo = " + id + ")";
                var data = db.Database.SqlQuery<Cipl>(sql).ToList();
                return data;
            }
        }

        public static List<CargoAddCipl> GetCiplItems(GridListFilter crit)
        {
            // ReSharper disable once IdentifierTypo
            // ReSharper disable once CollectionNeverUpdated.Local
            List<long> ciplitemList = new List<long>();
            using (var db = new Data.EmcsContext())
            {
                var data = (from c in db.CiplData
                            join ci in db.CiplItemData on c.Id equals ci.IdCipl
                            join it in db.MasterIncoTerm on c.IncoTerm equals it.Number
                            //where CIPLNoList.Contains((int)c.id)
                            select new
                            {
                                ID = ci.Id,
                                CiplID = ci.Id,
                                c.CiplNo,
                                ci.CaseNumber,
                                c.EdoNo,
                                InboundDa = c.Da,
                                c.IncoTerm,
                                IncoTermNumber = it.Number,
                                Length = ci.Length ?? 0,
                                Width = ci.Width ?? 0,
                                Height = ci.Height ?? 0,
                                NetWeight = ci.NetWeight ?? 0,
                                GrossWeight = ci.GrossWeight ?? 0,
                                state = ciplitemList.Contains(ci.Id)
                            }).ToList();

                return data.Select(i => new CargoAddCipl
                {
                    Id = i.ID,
                    IdCipl = i.CiplID,
                    CiplNo = i.CiplNo,
                    CaseNumber = i.CaseNumber,
                    EdoNo = i.EdoNo,
                    InboundDa = i.InboundDa,
                    IncoTerm = i.IncoTerm,
                    IncoTermNumber = i.IncoTermNumber,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    NetWeight = i.NetWeight,
                    GrossWeight = i.GrossWeight,
                    State = i.state
                }).ToList();
            }
        }

        public static List<CargoAddCipl> GetCiplForCargo(List<long> ciplNoList, long cargoId)
        {
            // ReSharper disable once IdentifierTypo
            // ReSharper disable once CollectionNeverUpdated.Local
            List<long> ciplitemList = new List<long>();

            using (var db = new Data.EmcsContext())
            {
                //List<long> ciplitem_list = db.CargoItemData.Where(i => i.IdCargo == CargoID).Select(i => i.IdCipl).ToList();

                var data = (from c in db.CiplData
                            join ci in db.CiplItemData on c.Id equals ci.IdCipl
                            join it in db.MasterIncoTerm on c.IncoTerm equals it.Number
                            where ciplNoList.Contains(c.Id)
                            select new
                            {
                                ID = ci.Id,
                                CiplID = ci.Id,
                                c.CiplNo,
                                ci.CaseNumber,
                                c.EdoNo,
                                InboundDa = c.Da,
                                c.IncoTerm,
                                IncoTermNumber = it.Number,
                                Length = ci.Length ?? 0,
                                Width = ci.Width ?? 0,
                                Height = ci.Height ?? 0,
                                NetWeight = ci.NetWeight ?? 0,
                                GrossWeight = ci.GrossWeight ?? 0,
                                state = ciplitemList.Contains(ci.Id)
                            }).ToList();
                return data.Select(i => new CargoAddCipl
                {
                    Id = i.ID,
                    IdCipl = i.CiplID,
                    CiplNo = i.CiplNo,
                    CaseNumber = i.CaseNumber,
                    EdoNo = i.EdoNo,
                    InboundDa = i.InboundDa,
                    IncoTerm = i.IncoTerm,
                    IncoTermNumber = i.IncoTermNumber,
                    Length = i.Length,
                    Width = i.Width,
                    Height = i.Height,
                    NetWeight = i.NetWeight,
                    GrossWeight = i.GrossWeight,
                    State = i.state
                }).ToList();
            }
        }

        public static long CrudSp(CargoFormData item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
<<<<<<< HEAD
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@CargoId", item.Id));
                    parameterList.Add(new SqlParameter("@Consignee", item.Consignee ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyParty", item.NotifyParty ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", item.ExportType ?? ""));
                    parameterList.Add(new SqlParameter("@Category", item.Category ?? ""));
                    parameterList.Add(new SqlParameter("@Incoterms", item.Incoterms ?? ""));
                    parameterList.Add(new SqlParameter("@StuffingDateStarted", item.StuffingDateStarted.HasValue ? (object)item.StuffingDateStarted : DBNull.Value));
                    parameterList.Add(new SqlParameter("@StuffingDateFinished", item.StuffingDateFinished.HasValue ? (object)item.StuffingDateFinished : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ETA", item.Eta.HasValue ? (object)item.Eta : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ETD", item.Etd.HasValue ? (object)item.Eta : DBNull.Value));
                    parameterList.Add(new SqlParameter("@VesselFlight", item.VesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@ConnectingVesselFlight", item.ConnectingVesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@VoyageVesselFlight", item.VoyageVesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@VoyageConnectingVessel", item.VoyageConnectingVessel ?? ""));
                    parameterList.Add(new SqlParameter("@PortOfLoading", item.PortOfLoading ?? ""));
                    parameterList.Add(new SqlParameter("@PortOfDestination", item.PortOfDestination ?? ""));
                    parameterList.Add(new SqlParameter("@SailingSchedule", item.SailingSchedule.HasValue ? (object)item.SailingSchedule : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ArrivalDestination", item.ArrivalDestination.HasValue ? (object)item.ArrivalDestination : DBNull.Value));
                    parameterList.Add(new SqlParameter("@BookingNumber", item.BookingNumber ?? ""));
                    parameterList.Add(new SqlParameter("@BookingDate", item.BookingDate.HasValue ? (object)item.BookingDate : DBNull.Value));
                    parameterList.Add(new SqlParameter("@Liner", item.Liner ?? ""));
                    parameterList.Add(new SqlParameter("@Status", item.Status ?? ""));
                    parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));

                    if (item.Referrence != null)
                        parameterList.Add(new SqlParameter("@Referrence", string.Join(",", item.Referrence.ToArray())));
                    else
                        parameterList.Add(new SqlParameter("@Referrence", ""));

                    parameterList.Add(new SqlParameter("@CargoType", item.CargoType ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMethod", item.ShippingMethod ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<IdData>(" exec [dbo].[sp_insert_update_cargo] @CargoId, @Consignee, @NotifyParty, @ExportType, @Category, @Incoterms, @StuffingDateStarted, @StuffingDateFinished, @ETA, @ETD, @VesselFlight, @ConnectingVesselFlight, @VoyageVesselFlight, @VoyageConnectingVessel, @PortOfLoading, @PortOfDestination, @SailingSchedule, @ArrivalDestination, @BookingNumber, @BookingDate, @Liner, @Status, @ActionBy, @Referrence, @CargoType, @ShippingMethod", parameters).FirstOrDefault();
                    if (data != null) return data.Id;
                }
                catch(Exception ex)
                {
                    string a = ex.Message;
                }
            }

            return 0;
        }
        public static long UpdateCargoByApprover(CargoFormData item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@CargoId", item.Id));
                    parameterList.Add(new SqlParameter("@Consignee", item.Consignee ?? ""));
                    parameterList.Add(new SqlParameter("@NotifyParty", item.NotifyParty ?? ""));
                    parameterList.Add(new SqlParameter("@ExportType", item.ExportType ?? ""));
                    parameterList.Add(new SqlParameter("@Category", item.Category ?? ""));
                    parameterList.Add(new SqlParameter("@Incoterms", item.Incoterms ?? ""));
                    parameterList.Add(new SqlParameter("@StuffingDateStarted", item.StuffingDateStarted.HasValue ? (object)item.StuffingDateStarted : DBNull.Value));
                    parameterList.Add(new SqlParameter("@StuffingDateFinished", item.StuffingDateFinished.HasValue ? (object)item.StuffingDateFinished : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ETA", item.Eta.HasValue ? (object)item.Eta : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ETD", item.Etd.HasValue ? (object)item.Eta : DBNull.Value));
                    parameterList.Add(new SqlParameter("@VesselFlight", item.VesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@ConnectingVesselFlight", item.ConnectingVesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@VoyageVesselFlight", item.VoyageVesselFlight ?? ""));
                    parameterList.Add(new SqlParameter("@VoyageConnectingVessel", item.VoyageConnectingVessel ?? ""));
                    parameterList.Add(new SqlParameter("@PortOfLoading", item.PortOfLoading ?? ""));
                    parameterList.Add(new SqlParameter("@PortOfDestination", item.PortOfDestination ?? ""));
                    parameterList.Add(new SqlParameter("@SailingSchedule", item.SailingSchedule.HasValue ? (object)item.SailingSchedule : DBNull.Value));
                    parameterList.Add(new SqlParameter("@ArrivalDestination", item.ArrivalDestination.HasValue ? (object)item.ArrivalDestination : DBNull.Value));
                    parameterList.Add(new SqlParameter("@BookingNumber", item.BookingNumber ?? ""));
                    parameterList.Add(new SqlParameter("@BookingDate", item.BookingDate.HasValue ? (object)item.BookingDate : DBNull.Value));
                    parameterList.Add(new SqlParameter("@Liner", item.Liner ?? ""));
                    parameterList.Add(new SqlParameter("@Status", item.Status ?? ""));
                    parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));

                    if (item.Referrence != null)
                        parameterList.Add(new SqlParameter("@Referrence", string.Join(",", item.Referrence.ToArray())));
                    else
                        parameterList.Add(new SqlParameter("@Referrence", ""));

                    parameterList.Add(new SqlParameter("@CargoType", item.CargoType ?? ""));
                    parameterList.Add(new SqlParameter("@ShippingMethod", item.ShippingMethod ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    var data = db.DbContext.Database.SqlQuery<IdData>(" exec [dbo].[sp_update_cargo_ByApprover] @CargoId, @Consignee, @NotifyParty, @ExportType, @Category, @Incoterms, @StuffingDateStarted, @StuffingDateFinished, @ETA, @ETD, @VesselFlight, @ConnectingVesselFlight, @VoyageVesselFlight, @VoyageConnectingVessel, @PortOfLoading, @PortOfDestination, @SailingSchedule, @ArrivalDestination, @BookingNumber, @BookingDate, @Liner, @Status, @ActionBy, @Referrence, @CargoType, @ShippingMethod", parameters).FirstOrDefault();
                    if (data != null) return data.Id;
                }
                catch (Exception ex)
                {
                    string a = ex.Message;
                }
=======
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CargoId", item.Id));
                parameterList.Add(new SqlParameter("@Consignee", item.Consignee ?? ""));
                parameterList.Add(new SqlParameter("@NotifyParty", item.NotifyParty ?? ""));
                parameterList.Add(new SqlParameter("@ExportType", item.ExportType ?? ""));
                parameterList.Add(new SqlParameter("@Category", item.Category ?? ""));
                parameterList.Add(new SqlParameter("@Incoterms", item.Incoterms ?? ""));
                parameterList.Add(new SqlParameter("@StuffingDateStarted", item.StuffingDateStarted.HasValue ? (object)item.StuffingDateStarted : DBNull.Value));
                parameterList.Add(new SqlParameter("@StuffingDateFinished", item.StuffingDateFinished.HasValue ? (object)item.StuffingDateFinished : DBNull.Value));
                parameterList.Add(new SqlParameter("@ETA", item.Eta.HasValue ? (object)item.Eta : DBNull.Value));
                parameterList.Add(new SqlParameter("@ETD", item.Etd.HasValue ? (object)item.Eta : DBNull.Value));
                parameterList.Add(new SqlParameter("@VesselFlight", item.VesselFlight ?? ""));
                parameterList.Add(new SqlParameter("@ConnectingVesselFlight", item.ConnectingVesselFlight ?? ""));
                parameterList.Add(new SqlParameter("@VoyageVesselFlight", item.VoyageVesselFlight ?? ""));
                parameterList.Add(new SqlParameter("@VoyageConnectingVessel", item.VoyageConnectingVessel ?? ""));
                parameterList.Add(new SqlParameter("@PortOfLoading", item.PortOfLoading ?? ""));
                parameterList.Add(new SqlParameter("@PortOfDestination", item.PortOfDestination ?? ""));
                parameterList.Add(new SqlParameter("@SailingSchedule", item.SailingSchedule.HasValue ? (object)item.SailingSchedule : DBNull.Value));
                parameterList.Add(new SqlParameter("@ArrivalDestination", item.ArrivalDestination.HasValue ? (object)item.ArrivalDestination : DBNull.Value));
                parameterList.Add(new SqlParameter("@BookingNumber", item.BookingNumber ?? ""));
                parameterList.Add(new SqlParameter("@BookingDate", item.BookingDate.HasValue ? (object)item.BookingDate : DBNull.Value));
                parameterList.Add(new SqlParameter("@Liner", item.Liner ?? ""));
                parameterList.Add(new SqlParameter("@Status", item.Status ?? ""));
                parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));

                if (item.Referrence != null)
                    parameterList.Add(new SqlParameter("@Referrence", string.Join(",", item.Referrence.ToArray())));
                else
                    parameterList.Add(new SqlParameter("@Referrence", ""));

                parameterList.Add(new SqlParameter("@CargoType", item.CargoType ?? ""));
                parameterList.Add(new SqlParameter("@ShippingMethod", item.ShippingMethod ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<IdData>(" exec [dbo].[sp_insert_update_cargo] @CargoId, @Consignee, @NotifyParty, @ExportType, @Category, @Incoterms, @StuffingDateStarted, @StuffingDateFinished, @ETA, @ETD, @VesselFlight, @ConnectingVesselFlight, @VoyageVesselFlight, @VoyageConnectingVessel, @PortOfLoading, @PortOfDestination, @SailingSchedule, @ArrivalDestination, @BookingNumber, @BookingDate, @Liner, @Status, @ActionBy, @Referrence, @CargoType, @ShippingMethod", parameters).FirstOrDefault();
                if (data != null) return data.Id;
>>>>>>> 639d8d0 (Intial commit)
            }

            return 0;
        }

        public static long ApprovalRequest(CargoFormData item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var status = item.Status ?? "";
                var actionBy = SiteConfiguration.UserName;
                var comment = item.Detail != null ? (item.Detail.Comment != null ? item.Detail.Comment : "") : "";

                var sql = @"exec sp_update_request_cl @IdCl=" + item.Id + ", @Username='" + actionBy + "', @NewStatus='" + status + "', @Notes='" + comment + "'";
                db.DbContext.Database.ExecuteSqlCommand(sql);
                return 1;
            }
        }

        public static long InsertCargolItem(List<CargoItem> data, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE [dbo].[CargoItem] where IdCargo = " + id);
                for (var j = 0; j < data.Count; j++)
                {
                    db.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCargo", id));
                    parameterList.Add(new SqlParameter("@IdCIPL", data[j].IdCipl));
                    parameterList.Add(new SqlParameter("@InBoundDa", data[j].InBoundDa ?? ""));
                    parameterList.Add(new SqlParameter("@Length", data[j].Length ?? 0));
                    parameterList.Add(new SqlParameter("@Width", data[j].Width ?? 0));
                    parameterList.Add(new SqlParameter("@Height", data[j].Height ?? 0));
                    parameterList.Add(new SqlParameter("@Gross", data[j].Gross ?? 0));
                    parameterList.Add(new SqlParameter("@Net", data[j].Net ?? 0));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    db.Database.ExecuteSqlCommand(@" exec [dbo].[sp_CargoItemInsert] @IdCargo, @IdCIPL, @Container, @CaseNumber, @InBoundDa, @CargoDescription, @Length, @Width, @Height, @Net, @Gross", parameters);
                }

                List<SqlParameter> parameterListForSs = new List<SqlParameter>();
                parameterListForSs.Add(new SqlParameter("@IdCl", id));
                SqlParameter[] parametersForSs = parameterListForSs.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                db.Database.ExecuteSqlCommand(@" exec [dbo].[sp_set_ss_number] @IdCl", parametersForSs);

                return 1;
            }
        }

        public static long RemoveCargo(long cargoId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand(@" UPDATE [dbo].[Cargo] SET isdelete  = 1 where Id = " + cargoId);
                db.DbContext.Database.ExecuteSqlCommand(@" UPDATE [dbo].[CargoItem] SET isdelete  = 1 where IdCargo = " + cargoId);
                db.DbContext.Database.ExecuteSqlCommand(@" UPDATE [dbo].[RequestCl] SET isdelete  = 1 where IdCl = " + cargoId);
                db.DbContext.Database.ExecuteSqlCommand(@" UPDATE [dbo].[CargoHistory] SET isdelete  = 1 where IdCargo = " + cargoId);
                db.DbContext.Database.ExecuteSqlCommand(@" UPDATE [dbo].[CargoCipl] SET isdelete  = 1 where IdCargo = " + cargoId);
                return cargoId;
            }
        }

        public static List<SpGetCargoHistory> CargoHistoryGetById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpGetCargoHistory>("[dbo].[SP_CargoHistoryGetById] @id", parameters).ToList();
                return data;
            }
        }

        public static List<Documents> CargoDocumentsGetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Documents.Where(a => a.IdRequest == id);
                return data.ToList();
            }
        }

        public static List<SpCargoProblemHistory> SP_CargoProblemHistory(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpCargoProblemHistory>("[dbo].[SP_CargoProblemHistoryGetById] @id", parameters).ToList();

                return data;
            }
        }

        public static bool CiplItemAvailable(long idCargo)
        {
            var result = false;

            var ciplList = Service.EMCS.SvcCargo.GetCargoReferenceById(idCargo);
            var newData = new List<long>();
            foreach (var item in ciplList)
            {
                newData.Add(item.IdCipl);
            }
            var referrenceList = string.Join(",", newData);
            var data = Service.EMCS.SvcCargo.GetCiplItemAvailable(referrenceList, idCargo);

            if (data.total == 0)
                result = true;

            return result;
        }

        public static List<CargoCipl> GetCargoReferenceById(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.CargoCipls.Where(a => a.IdCargo == cargoId).ToList();
                return data;
            }
        }

        public static List<CargoCipl> SearchCargoByCipl(long IdCipl)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var data = db.CargoCipls.Where(a => a.IdCipl == IdCipl).ToList();
                return data;
            }
        }

        public static dynamic GetCiplItemAvailable(string idCipl, long idCargo)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_get_cipl_item_available @idCipl ='" + idCipl + "', @idCargo='" + idCargo + "'";
                var data = db.Database.SqlQuery<SpCiplItemList>(sql).ToList();

                dynamic obj = new ExpandoObject();
                obj.rows = data;
                obj.total = data.Count();

                return obj;
            }
        }

        public static dynamic GetCiplItemByContainer(long idContainer)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC [SP_get_item_by_container] @IdContainer='" + idContainer + "'";
                var data = db.Database.SqlQuery<SpCiplItemList>(sql).ToList();

                dynamic obj = new ExpandoObject();
                obj.rows = data;
                obj.total = data.Count();

                return obj;
            }
        }

        public static dynamic GetCargoListItem(GridListFilter filter)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC [sp_get_cargo_item_list] @Search='" + filter.Term + "', @IdCargo='" + filter.Id + "'";
                var data = db.Database.SqlQuery<CargoAddCipl>(sql).ToList();

                dynamic obj = new ExpandoObject();
                obj.rows = data;
                obj.total = data.Count();

                return obj;
            }
        }

        public static dynamic GetCargoListItemApproval(GridListFilter filter)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC [sp_get_cargo_item_list_approval] @Search='" + filter.Term + "', @IdCargo='" + filter.Id + "'";
                var data = db.Database.SqlQuery<CargoAddCipl>(sql).ToList();

                dynamic obj = new ExpandoObject();
                obj.rows = data;
                obj.total = data.Count();

                return obj;
            }
        }

        public static long GetCargoByCipl(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                // ReSharper disable once PossibleNullReferenceException
                var data = db.CargoCipls.FirstOrDefault(a => a.IdCipl == id).IdCargo;
                return data;
            }
        }

        public static List<GoodsReceiveItem> GetCargoByGr(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var dataGrList = db.GoodsReceiveItem.Where(a => a.IdGr == id).ToList();
                return dataGrList;
            }
        }

        public static long InsertDocument(RequestCl step, string filename, string typeDoc)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdRequest", long.Parse(step.IdCl)));
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
<<<<<<< HEAD
        public static bool InsertCargoDocument(List<CargoDocument> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {
                    
                    for (var i = 0; i < data.Count; i++)
                    {
                        db.DbContext.Database.CommandTimeout = 600;
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Id", data[i].Id));
                        parameters.Add(new SqlParameter("@IdCargo", data[i].IdCargo));
                        parameters.Add(new SqlParameter("@DocumentDate", data[i].DocumentDate));
                        parameters.Add(new SqlParameter("@DocumentName", data[i].DocumentName ?? ""));
                        parameters.Add(new SqlParameter("@Filename", data[i].Filename ?? ""));
                        parameters.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                        parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        parameters.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                        parameters.Add(new SqlParameter("@UpdateDate", ""));
                        parameters.Add(new SqlParameter("@IsDelete", false));
                        SqlParameter[] sql = parameters.ToArray();
                        db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CargoDocumentAdd] @Id,@IdCargo,@DocumentDate,@DocumentName,@Filename,@CreateBy,@CreateDate,@UpdateBy,@UpdateDate,@IsDelete", sql);

                    }
                    
                }
                catch(Exception ex)
                {
                    var a = ex.Message;
                }
                return true;
            }
            
        }
        public static dynamic CargoDocumentList(GridListFilter filter)
        {
            using(var db = new Data.EmcsContext())
            {
                //CargoDocument a = ewn 
                //filter.Cargoid =
                try
                {
                    filter.Sort = filter.Sort ?? "Id";
                    db.Database.CommandTimeout = 600;
                    var sql = @"[dbo].[sp_get_cargo_document_list] @IdCargo = '" + filter.Cargoid + "'";
                    var data = db.Database.SqlQuery<CargoDocument>(sql).ToList();

                    return data;
                }
                catch(Exception ex)
                {
                    var a = ex.Message;
                    return a;
                }
            }
        }
        public static long DeleteCargoDocumentById(long idDocument)
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
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CargoDocumentDelete] @id, @UpdateBy, @UpdateDate, @IsDelete", parameters);
                return 1;
            }
        }
        public static bool UpdateFileCargoDocument(long id, string filename)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", id));
                parameterList.Add(new SqlParameter("@Filename", filename));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                SqlParameter[] parameters = parameterList.ToArray();
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_CargoDocumentUpdateFile] @Id, @Filename, @UpdateBy", parameters);
            }
            return true;
        }
        public static List<CargoDocument> CargoDocumentListById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", id));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<CargoDocument>("[dbo].[sp_get_cargo_document_list_byid] @id", parameters).ToList();
                    return data;
                }
                catch(Exception ex)
                {
                    var b = ex.Message;
                    var a = db.DbContext.Database.SqlQuery<CargoDocument>("[dbo].[sp_get_cargo_document_list_byid] @id").ToList();
                    return a;
                }
                
            }
            //using (var db = new Data.EmcsContext())
            //{
            //    var data = db.CiplD.Where(a => a.IdRequest == id && a.IsDelete == false);
            //    return data.ToList();
            //}
        }
        public static dynamic SearchContainNumber(CargoItem cargoItem)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCargo", cargoItem.IdCargo));
                    parameterList.Add(new SqlParameter("@ContainerNumber", cargoItem.ContainerNumber));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<CargoItem>("[dbo].[sp_searchContainerNumber]  @IdCargo, @ContainerNumber", parameters).ToList();
                    
                    return data;
                }
                catch (Exception ex)
                {
                    var b = ex.Message;
                    var a = db.DbContext.Database.SqlQuery<CargoItem>("[dbo].[sp_searchContainerNumber] @IdCargo, @ContainerNumber").ToList();
                    return a;
                }

            }
            
        }
        public static dynamic GetCargoType(string  id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@ContainerType", "ContainerType"));
                    parameterList.Add(new SqlParameter("@Value", id));
                    SqlParameter[] parameters = parameterList.ToArray();
                    App.Data.Domain.EMCS.MasterParameter m = new App.Data.Domain.EMCS.MasterParameter();
                    var data = db.DbContext.Database.SqlQuery<App.Data.Domain.EMCS.MasterParameter>("[dbo].[sp_getcontainertype]  @ContainerType, @Value", parameters).ToList();
                    foreach(var i in data)
                    {
                        m.Description = i.Description;
                        
                        
                        
                    }
                    
                    return m;
                }
                catch (Exception ex)
                {
                    var b = ex.Message;
                    var a = db.DbContext.Database.SqlQuery<MasterParameter>("[dbo].[sp_getcontainertype] @ContainerType, @Value").ToList();
                    return a;
                }

            }

        }

=======
>>>>>>> 639d8d0 (Intial commit)
    }
}
