using System;
using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Caching;
using App.Domain;

namespace App.Service.Master
{
	public class CommodityImex
	{
		private const string cacheName = "App.master.CommodityImex";

		private static readonly ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.CommodityImex> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using (var db = new RepositoryFactory(new EfDbContext()))
				{
					var tbl = db.CreateRepository<Data.Domain.CommodityImex>().TableNoTracking.ToList();
					return tbl;
				}
			});

			return list;
		}

		public static List<Data.Domain.CommodityImex> GetMappingList()
		{
			var list = from c in GetList()
								from c2 in Imex.CommodityMapping.GetGroupList().Where(w => w.ID == c.ID)
								select c;
			return list.ToList();
		}

		public static List<Data.Domain.CommodityImex> GetList(MasterSearchForm crit)
		{
			string name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.CommodityCode + " | " + c.CommodityName).Trim().ToLower().Contains(name))
								 orderby c.CommodityCode
								 select c;
			return list.ToList();
		}

		public static Data.Domain.CommodityImex GetId(int id)
		{
			var item = GetList().Where(w => w.ID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.CommodityImex itm, string dml)
		{
			if (dml == "I")
			{
				itm.EntryBy = SiteConfiguration.UserName;
				itm.EntryDate = DateTime.Now;
				itm.Status = 1;
			}

			itm.ModifiedBy = SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

			using (var db = new RepositoryFactory(new EfDbContext()))
			{
				return db.CreateRepository<Data.Domain.CommodityImex>().CRUD(dml, itm);
			}
		}
	}
}