using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcWizard
    {
        public const string CacheName = "App.EMCS.SvcWizard";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<IdData> GetCargoByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result = new List<IdData>();

                if (page != "cargo")
                {
                    var sql = "";
                    switch (page)
                    {
                        case "cipl":
                            sql = "select t0.IdCargo ID from dbo.CargoCipl t0 WHERE t0.IsDelete = 0 AND t0.IdCipl = " + id;
                            break;
                        case "si":
                            sql = "select CAST(t0.IdCl as bigint) ID from dbo.ShippingInstruction t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "bl":
                            sql = "select t0.IdCl ID from dbo.BlAwb t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "ss":
                            sql = "select t0.Id ID from dbo.Cargo t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "cargo":
                            sql = "select t0.Id ID from dbo.Cargo t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "npepeb":
                            sql = "select t0.IdCl ID from dbo.NpePeb t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "rg":
                            sql = "select t1.IdCargo ID " +
                                  "from dbo.GoodsReceiveItem t0 " +
                                  "join dbo.CargoCipl t1 on t0.IdCipl = t1.IdCipl AND t1.IsDelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                    }

                    result = db.Database.SqlQuery<IdData>(sql).ToList();

                }
                else
                {
                    var current = new IdData();
                    current.Id = id;
                    result.Add(current);
                }
                return result;
            }
        }

        public static List<IdData> GetCiplByPage(string page, long id)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    List<IdData> result = new List<IdData>();
                    List<long> newIds = new List<long>();

                    if (page != "cipl")
                    {
                        if (page != "rg")
                        {
                            switch (page)
                            {
                                case "ss":
                                    var resultss = db.CargoData.Where(a => a.Id == id).Select(a => new { id = a.Id }).ToList();
                                    foreach (var itemss in resultss)
                                    {
                                        newIds.Add(itemss.id);
                                    }
                                    break;
                                case "cargo":
                                    var resultcl = db.CargoData.Where(a => a.Id == id).Select(a => new { id = a.Id }).ToList();
                                    foreach (var itemcl in resultcl)
                                    {
                                        newIds.Add(itemcl.id);
                                    }
                                    break;
                                case "si":
                                    var resultsi = db.ShippingInstruction.Where(a => a.Id == id).Select(a => new { IdCL = a.IdCl }).ToList();
                                    foreach (var itemsi in resultsi)
                                    {
                                        newIds.Add(Convert.ToInt64(itemsi.IdCL));
                                    }
                                    break;
                                case "npepeb":
                                    var resultpeb = db.NpePebs.Where(a => a.Id == id).Select(a => new { a.IdCl }).ToList();
                                    foreach (var itempeb in resultpeb)
                                    {
                                        newIds.Add(itempeb.IdCl);
                                    }
                                    break;
                                case "blawb":
                                    var resultbl = db.BlAwb.Where(a => a.Id == id).Select(a => new { a.IdCl }).ToList();
                                    foreach (var itembl in resultbl)
                                    {
                                        newIds.Add(itembl.IdCl);
                                    }

                                    break;
                            }
                            result = db.CargoCipls.Where(x => newIds.Contains(x.IdCargo)).ToList().Select(a => new IdData { Id = a.IdCipl }).ToList();
                        }
                        else
                        {
                            var rgitem = db.GoodsReceiveItem.Where(a => a.IdGr == id).ToList();

                            foreach (var itemrg in rgitem)
                            {
                                newIds.Add(itemrg.IdCipl);
                            }

                            result = newIds.ToList().Select(a => new IdData { Id = a }).ToList();

                        }
                    }
                    else
                    {
                        var resSingle = db.CargoCipls.Where(a => a.IdCipl == id).AsQueryable().ToList();
                        foreach (var data in resSingle)
                        {
                            IdData dataId = new IdData();
                            dataId.Id = data.IdCipl;
                            result.Add(dataId);
                        }
                    }

                    return result;
                }
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                throw;
            }
        }

        public static List<IdData> GetCiplByPage1(string page, long id)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    List<IdData> result = new List<IdData>();
                    List<long> newIds = new List<long>();

                    var resSingle = db.CargoCipls.Where(a => a.IdCipl == id).AsQueryable().ToList();
                    foreach (var data in resSingle)
                    {
                        IdData dataId = new IdData();
                        dataId.Id = data.IdCipl;
                        result.Add(dataId);
                    }                   

                    return result;
                }
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                throw;
            }
        }

        public static List<IdData> GetGrByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result = new List<IdData>();
                List<long> newIds = new List<long>();

                if (page != "rg")
                {
                    if (page != "cipl")
                    {
                        switch (page)
                        {
                            case "cargo":
                                var resultcl = db.CargoData.Where(a => a.Id == id).Select(a => new { id = a.Id }).ToList();
                                foreach (var itemcl in resultcl)
                                {
                                    newIds.Add(itemcl.id);
                                }
                                break;
                            case "ss":
                                var resultss = db.CargoData.Where(a => a.Id == id).Select(a => new { id = a.Id }).ToList();
                                foreach (var itemss in resultss)
                                {
                                    newIds.Add(itemss.id);
                                }
                                break;
                            case "si":
                                var resultsi = db.ShippingInstruction.Where(a => a.Id == id).Select(a => new { IdCL = a.IdCl }).ToList();
                                foreach (var itemsi in resultsi)
                                {
                                    newIds.Add(Convert.ToInt64(itemsi.IdCL));
                                }
                                break;
                            case "cipl":
                                break;
                            case "npepeb":
                                var resultpeb = db.NpePebs.Where(a => a.Id == id).Select(a => new { a.IdCl }).ToList();
                                foreach (var itempeb in resultpeb)
                                {
                                    newIds.Add(itempeb.IdCl);
                                }
                                break;
                            case "blawb":
                                var resultbl = db.BlAwb.Where(a => a.Id == id).Select(a => new { a.IdCl }).ToList();
                                foreach (var itembl in resultbl)
                                {
                                    newIds.Add(itembl.IdCl);
                                }
                                break;
                        }

                        var resultdata = (from t1 in db.CargoCipls
                                          join t2 in db.GoodsReceiveItem on t1.IdCipl equals t2.IdCipl
                                          where newIds.Contains(t1.IdCargo)
                                          select new
                                          {
                                              t1,
                                              t2
                                          }).ToList();

                        result = resultdata.Select(x => new IdData { Id = x.t2.IdGr }).ToList();
                    }
                    else
                    {
                        var resultcipl = db.GoodsReceiveItem.Where(a => a.IdCipl == id && a.IsDelete == false).Select(a => new { a.IdGr }).ToList();
                        foreach (var itemcipl in resultcipl)
                        {
                            newIds.Add(Convert.ToInt64(itemcipl.IdGr));
                        }
                        result = newIds.ToList().Select(a => new IdData { Id = a }).ToList();
                    }
                }
                else
                {
                    var data = new IdData();
                    data.Id = id;
                    result.Add(data);
                }

                return result;
            }
        }

        public static List<IdData> GetSsByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result = new List<IdData>();

                if (page != "ss")
                {
                    var sql = "";
                    switch (page)
                    {
                        case "cipl":
                            sql = "select t0.IdCargo ID from dbo.CargoCipl t0 WHERE t0.IsDelete = 0 AND t0.IdCipl = " + id;
                            break;
                        case "si":
                            sql = "select CAST(t0.IdCl as bigint) ID from dbo.ShippingInstruction t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "bl":
                            sql = "select t0.IdCl ID from dbo.BlAwb t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "cargo":
                            sql = "select t0.Id ID from dbo.Cargo t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "npepeb":
                            sql = "select t0.IdCl ID from dbo.NpePeb t0 WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "rg":
                            sql = "select t1.IdCargo ID " +
                                  "from dbo.GoodsReceiveItem t0 " +
                                  "join dbo.CargoCipl t1 on t0.IdCipl = t1.IdCipl AND t1.isDelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                    }

                    result = db.Database.SqlQuery<IdData>(sql).ToList();
                }
                else
                {
                    var data = new IdData();
                    data.Id = id;
                    result.Add(data);
                }

                return result;
            }
        }

        public static List<IdData> GetSiByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result = new List<IdData>();

                if (page != "si")
                {
                    var sql = "";
                    switch (page)
                    {
                        case "cipl":
                            sql = "select t1.Id ID from dbo.CargoCipl t0 " +
                                  "join dbo.ShippingInstruction t1 on t0.IdCargo = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.IdCipl = " + id;
                            break;
                        case "npepeb":
                            sql = "select t1.Id ID from dbo.NpePeb t0 " +
                                  "join dbo.ShippingInstruction t1 on t0.IdCl = CAST(t1.IdCL as bigint) AND t1.isdelete = 0" +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "bl":
                            sql = "select t1.Id ID from dbo.BlAwb t0 " +
                                  "join dbo.ShippingInstruction t1 on t0.IdCl = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;

                            break;
                        case "cargo":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.ShippingInstruction t1 on t0.Id = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;

                            break;
                        case "ss":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.ShippingInstruction t1 on t0.Id = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "rg":
                            sql = "select t1.IdCargo ID " +
                                  "from dbo.GoodsReceiveItem t0 " +
                                  "join dbo.CargoCipl t1 on t0.IdCipl = t1.IdCipl AND t1.isdelete = 0 " +
                                  "join dbo.ShippingInstruction t2 on t2.IdCL = CAST(t2.IdCL as bigint) AND t2.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                    }

                    result = db.Database.SqlQuery<IdData>(sql).ToList();
                }
                else
                {
                    var data = new IdData();
                    data.Id = id;
                    result.Add(data);
                }

                return result;
            }
        }

        public static bool HasSsNumber(long idCargo)
        {
            using (var db = new Data.EmcsContext())
            {
                var item = db.Database.SqlQuery<CountData>("select count(*) total from (select distinct IdCipl from dbo.CargoItem where IdCargo = " + idCargo + " AND isdelete = 0 ) as tb1").FirstOrDefault();
                if (item.Total > 1)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public static List<IdData> GetNpeByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result;

                if (page != "npepeb")
                {
                    var sql = "";
                    switch (page)
                    {
                        case "cipl":
                            sql = "select t1.Id ID from dbo.CargoCipl t0 " +
                                  "join dbo.NpePeb t1 on t1.IdCl = t0.IdCargo AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.IdCipl = " + id;
                            break;
                        case "si":
                            sql = "select t1.Id ID from dbo.ShippingInstruction t0 " +
                                  "join dbo.NpePeb t1 on t1.IdCl = CAST(t0.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "bl":
                            sql = "select t1.Id ID from dbo.BlAwb t0 " +
                                  "join dbo.NpePeb t1 on t0.IdCl = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "cargo":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.NpePeb t1 on t0.Id = t1.IdCL AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "ss":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.NpePeb t1 on t0.Id = t1.IdCL AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "rg":
                            sql = "select t1.IdCargo ID " +
                                  "from dbo.GoodsReceiveItem t0 " +
                                  "join dbo.CargoCipl t1 on t0.IdCipl = t1.IdCipl AND t1.isdelete = 0 " +
                                  "join dbo.NpePeb t2 on t2.IdCL = CAST(t2.IdCL as bigint) AND t2.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                    }

                    result = db.Database.SqlQuery<IdData>(sql).ToList();
                }
                else
                {
                    result = db.NpePebs.Where(a => a.Id == id).Select(x => new IdData { Id = x.Id }).ToList();
                }

                return result;
            }
        }

        public static List<IdData> GetBlByPage(string page, long id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<IdData> result = new List<IdData>();

                if (page != "blawb")
                {
                    var sql = "";
                    switch (page)
                    {
                        case "cipl":
                            sql = "select t1.Id ID from dbo.CargoCipl t0 " +
                                  "join dbo.BlAwb t1 on t1.IdCl = t0.IdCargo AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.IdCipl = " + id;
                            break;
                        case "si":
                            sql = "select t1.Id ID from dbo.ShippingInstruction t0 " +
                                  "join dbo.BlAwb t1 on t1.IdCl = CAST(t0.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "npepeb":
                            sql = "select t1.Id ID from dbo.NpePeb t0 " +
                                  "join dbo.BlAwb t1 on t0.IdCl = CAST(t1.IdCL as bigint) AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "cargo":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.BlAwb t1 on t0.Id = t1.IdCL AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "ss":
                            sql = "select t1.Id ID from dbo.Cargo t0 " +
                                  "join dbo.BlAwb t1 on t0.Id = t1.IdCL AND t1.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                        case "rg":
                            sql = "select t1.IdCargo ID " +
                                  "from dbo.GoodsReceiveItem t0 " +
                                  "join dbo.CargoCipl t1 on t0.IdCipl = t1.IdCipl AND t1.isdelete = 0 " +
                                  "join dbo.BlAwb t2 on t2.IdCL = CAST(t2.IdCL as bigint) AND t2.isdelete = 0 " +
                                  "WHERE t0.IsDelete = 0 AND t0.Id = " + id;
                            break;
                    }

                    result = db.Database.SqlQuery<IdData>(sql).ToList();
                }
                else
                {
                    var data = new IdData();
                    data.Id = id;
                    result.Add(data);
                }

                return result;
            }
        }

        public static WizardData GetWizardData(string page, long id)
        {
            WizardData wizard = new WizardData();
            wizard.Ciplid = GetCiplByPage(page, id);
            wizard.Grid = GetGrByPage(page, id);
            wizard.Cargoid = GetCargoByPage(page, id);
            wizard.Ssid = GetSsByPage(page, id);
            wizard.Siid = GetSiByPage(page, id);
            wizard.Npeid = GetNpeByPage(page, id);
            wizard.Blid = GetBlByPage(page, id);
            wizard.Progress = GetCurrentProgressNo(wizard);

            var idCargo = wizard.Cargoid;
            if (idCargo.Count == 1)
            {
                IdData first = null;
                foreach (var data in idCargo)
                {
                    first = data;
                    break;
                }

                if (first != null)
                {
                    var noCargo = first.Id;
                    wizard.HasSs = HasSsNumber(noCargo);
                }
            }
            else
            {
                wizard.HasSs = false;
            }

            return wizard;
        }

        public static int GetCurrentProgressNo(WizardData wizard)
        {
            int progress = 0;
            if (wizard.Ciplid.Count > 0)
            {
                progress = 0;
            }

            if (wizard.Grid.Count > 0)
            {
                progress = 1;
            }

            if (wizard.Cargoid.Count > 0)
            {
                progress = 2;
            }

            if (wizard.Ssid.Count > 0)
            {
                progress = 3;
            }

            if (wizard.Siid.Count > 0)
            {
                progress = 4;
            }

            if (wizard.Npeid.Count > 0)
            {
                progress = 5;
            }

            if (wizard.Blid.Count > 0)
            {
                progress = 6;
            }

            return progress;
        }
    }
}
