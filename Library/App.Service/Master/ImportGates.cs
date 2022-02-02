using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
	public class ImportGates
	{
		private const string cacheName = "App.master.ImportGate";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.ImportGate> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using (var db = new Data.EfDbContext())
				{
					var tb = from i in db.ImportGates.AsNoTracking().ToList()
									 from s in Service.Master.FreightPort.GetList().Where(a => a.IsSeaFreight == true && a.PortID == i.SeaPortID)
									 from a in Service.Master.FreightPort.GetList().Where(a => a.IsSeaFreight == false && a.PortID == i.AirPortID)
									 select new ImportGate()
									 {
										 EntryBy = i.EntryBy,
										 EntryDate = i.EntryDate,
										 ModifiedBy = i.ModifiedBy,
										 ModifiedDate = i.ModifiedDate,
										 Status = i.Status,
										 AirPortID = i.AirPortID,
										 AirPortCode = a.PortName + " - " + a.PortCode,
										 C3Code = i.C3Code,
										 GateID = i.GateID,
										 JCode = i.JCode,
										 SeaPortCode = s.PortName + " - " + s.PortCode,
										 SeaPortID = i.SeaPortID,
										 StoreName = i.StoreName
									 };

					return tb.ToList();

				}
			});

			return list;
		}

		public static List<Data.Domain.ImportGate> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.StoreName).Trim().ToLower().Contains(name))
								 orderby c.StoreName
								 select c;
			return list.ToList();
		}
		public static Data.Domain.ImportGate GetId(int id)
		{
			var item = GetList().Where(w => w.GateID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.ImportGate itm, string dml)
		{
			if (dml == "I")
			{
				itm.EntryBy = Domain.SiteConfiguration.UserName;
				itm.EntryDate = DateTime.Now;
				itm.Status=1;
			}

			itm.ModifiedBy = Domain.SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

			using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				return db.CreateRepository<Data.Domain.ImportGate>().CRUD(dml, itm);
			}
		}

	}
}
