using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
    public class JobFlags
	{
		private const string cacheName = "App.master.JobFlag";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.JobFlag> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{
					var tb = db.CreateRepository<Data.Domain.JobFlag>().TableNoTracking.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}

		public static List<Data.Domain.JobFlag> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.JobName).Trim().ToLower().Contains(name))
								 orderby c.JobName
								 select c;
			return list.ToList();
		}
		public static Data.Domain.JobFlag GetId(int id)
		{
			var item = GetList().Where(w => w.JobID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.JobFlag itm, string dml)
		{
			
			_cacheManager.Remove(cacheName);

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				return db.CreateRepository<Data.Domain.JobFlag>().CRUD(dml, itm);
			}
		}

	}
}
