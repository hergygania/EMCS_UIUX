using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
	public class AgreementType
	{
		private const string cacheName = "App.master.AgreementType";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.AgreementTypes> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using (var db = new Data.RepositoryFactory(new Data.EfDbContext())) {
					var tb = db.CreateRepository<Data.Domain.AgreementTypes>().TableNoTracking.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}

		public static List<Data.Domain.AgreementTypes> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.Description).Trim().ToLower().Contains(name))
								 orderby c.Description
								 select c;
			return list.ToList();
		}

		public static int Update(Data.Domain.AgreementTypes itm, string dml)
		{
			_cacheManager.Remove(cacheName);

			using (var db = new Data.RepositoryFactory(new Data.EfDbContext())) {
				return db.CreateRepository<Data.Domain.AgreementTypes>().CRUD(dml, itm);
			}
		}

	}
}
