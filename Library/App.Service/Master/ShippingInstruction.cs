using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
	public class ShippingInstruction
	{
		private const string cacheName = "App.master.ShippingInstruction";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.ShippingInstruction> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{
					var tb = db.CreateRepository<Data.Domain.ShippingInstruction>().TableNoTracking.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}

		public static List<Data.Domain.ShippingInstruction> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.Description).Trim().ToLower().Contains(name))
								 orderby c.Description
								 select c;
			return list.ToList();
		}
		public static Data.Domain.ShippingInstruction GetId(int id)
		{
			var item = GetList().Where(w => w.ShippingInstructionID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.ShippingInstruction itm, string dml)
		{
			if(dml == "I")
			{
				itm.EntryBy = Domain.SiteConfiguration.UserName;
				itm.EntryDate = DateTime.Now;
			}

			itm.ModifiedBy = Domain.SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				return db.CreateRepository<Data.Domain.ShippingInstruction>().CRUD(dml, itm);
			}
		}

	}
}
