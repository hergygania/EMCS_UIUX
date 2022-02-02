using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;

namespace App.Service.Master
{
	public class Area
	{
		private const string cacheName = "App.master.Area";

		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		public static List<Data.Domain.Area> GetList()
		{
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
					var tb = db.CreateRepository<Data.Domain.Area>().Table.Select(e => e);
					return tb.ToList();
				}
            //});

            //return list;
		}

		public static List<Data.Domain.Area> GetList(Domain.MasterSearchForm crit)
		{
			var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

			var list = from c in GetList()
								 where (name == "" || (c.Name).Trim().ToLower().Contains(name))
								 orderby c.Name
								 select c;
			return list.ToList();
		}
		public static Data.Domain.Area GetId(int id)
		{
            var item = GetList().Where(w => w.AreaID == id).FirstOrDefault();
			return item;
		}

		public static int Update(Data.Domain.Area itm, string dml)
		{
            if (dml == "I")
            {
				itm.EntryBy = Domain.SiteConfiguration.UserName; 
				itm.EntryDate = DateTime.Now;
			}
			
			itm.ModifiedBy = Domain.SiteConfiguration.UserName;
			itm.ModifiedDate = DateTime.Now;

			_cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
				return db.CreateRepository<Data.Domain.Area>().CRUD(dml, itm);
			}
		}

        public static List<Select2Result> GetListByUser(string userId)
        {

                using (var db = new Data.EfDbContext())
                {
                    var tb = from s in db.Areas
                             join us in db.UserAccess_Area on s.AreaID equals us.AreaID
                             join ua in db.UserAccesses on us.UserID equals ua.UserID
                             where ua.UserID == userId
                             select new Select2Result()
                             {
                                 id = s.AreaID.ToString(),
                                 text = s.Name
                             };

                    return tb.ToList();
                }
        }

	}
}
