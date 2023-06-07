using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Imex
{
	public class TeamProfiles
	{
		private const string cacheName = "App.master.TeamProfile";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.TeamProfile> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using(var db = new Data.EfDbContext())
				{
					var tb = db.TeamProfiles.AsNoTracking().ToList();
					return tb;
				}
			});

			return list;
		}


		public static Data.Domain.TeamProfile GetId(int id)
		{
			var item = GetList().Where(w => w.ID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.TeamProfile itm, string dml)
		{
			if(dml == "I")
			{
				itm.EntryBy = Domain.SiteConfiguration.UserName;
				itm.EntryDate = DateTime.Now;
				itm.Status=1;
			}

			itm.ModifiedBy = Domain.SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				return db.CreateRepository<Data.Domain.TeamProfile>().CRUD(dml, itm);
			}
		}

	}
}
