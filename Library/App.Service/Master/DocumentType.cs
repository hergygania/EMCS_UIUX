using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
	public class DocumentTypes
	{
		private const string cacheName = "App.master.DocumentType";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.DocumentType> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				using (var db = new Data.RepositoryFactory(new Data.EfDbContext())) {
					var tb = db.CreateRepository<Data.Domain.DocumentType>().TableNoTracking.Select(e => e);
					return tb.ToList();
				}
			});

			return list;
		}

		public static List<Data.Domain.DocumentType> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.DocumentName).Trim().ToLower().Contains(name))
								 orderby c.DocumentName
								 select c;
			return list.ToList();
		}
		public static Data.Domain.DocumentType GetId(int id)
		{
			var item = GetList().Where(w => w.DocumentTypeID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.DocumentType itm, string dml)
		{
			if (dml == "I") {
				itm.EntryBy = Domain.SiteConfiguration.UserName; 
				itm.EntryDate = DateTime.Now;
			}
			
			itm.ModifiedBy = Domain.SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

			using (var db = new Data.RepositoryFactory(new Data.EfDbContext())) {
				return db.CreateRepository<Data.Domain.DocumentType>().CRUD(dml, itm);
			}
		}

	}
}
