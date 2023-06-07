using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using App.Data.Caching;
using App.Data.Domain;

using System.Data;
using System.Data.Entity;

namespace App.Service.Imex
{
	public class CommodityMapping
	{
		private const string cacheName = "App.imex.CommodityMapping";
		private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

		private static List<Data.Domain.CommodityMapping> RefreshList()
		{

			using (var db = new Data.EfDbContext())
			{
				var tbl = from c in db.CommodityMapping
									from h in db.HSCodeLists.Where(w => w.HSID == c.HSId)
									from com in db.CommodityImex.Where(w => w.ID == c.CommodityID)
									select new { c, h.HSCode, hsDes = h.Description, com.CommodityCode, com.CommodityName };

				var list = from c in tbl.ToList()
									 select new Data.Domain.CommodityMapping()
									 {
										 MappingID = c.c.MappingID,
										 CommodityID = c.c.CommodityID,
										 HSId = c.c.HSId,
										 Status = c.c.Status,
										 EntryDate = c.c.EntryDate,
										 ModifiedDate = c.c.ModifiedDate,
										 EntryBy = c.c.EntryBy,
										 ModifiedBy = c.c.ModifiedBy,
										 HSCode = c.HSCode,
										 HSDescription = c.hsDes,
										 HSCodeCap = c.HSCode.ToString() + " ~ " + (c.hsDes + "").Replace(".", ""),
										 CommodityCode = c.CommodityCode,
										 CommodityName = c.CommodityName,
										 CommodityCap = c.CommodityCode + " - " + c.CommodityName,
									 };

				return list.ToList();
			}
		}

		public static List<Data.Domain.CommodityMapping> GetList()
		{
			string key = string.Format(cacheName);

			var list = _cacheManager.Get(key, () =>
			{
				var tbl = RefreshList();
				return tbl;
			});
			return list;
		}

		public static List<Data.Domain.CommodityMapping> GetList(int id)
		{
			var list = GetList().Where(w => w.CommodityID == id).ToList();
			return list;
		}

		public static List<Data.Domain.CommodityImex> GetGroupList()
		{
			var list = GetList().Where(w => w.Status == 1)
				.GroupBy(g => g.CommodityID)
				.Select(s => new { CommodityID = s.Key, CommodityCode = s.Max(m => m.CommodityCode), CommodityName = s.Max(m => m.CommodityName) })
				.AsParallel()
				.Select(s => new Data.Domain.CommodityImex()
									 {
										 ID = s.CommodityID,
										 CommodityCode = s.CommodityCode,
										 CommodityName = s.CommodityName
									 })
				.ToList();
			return list;
		}


		public static Data.Domain.CommodityMapping GetId(int id)
		{
			if (id == 0)
				return new Data.Domain.CommodityMapping();

			var item = GetList().Where(w => w.MappingID == id).FirstOrDefault();
			return item;
		}


		public async static Task<int> Update(Data.Domain.CommodityMapping itm, string dml)
		{
			string userName = Domain.SiteConfiguration.UserName;
			itm.ModifiedBy = userName;
			itm.ModifiedDate = DateTime.Now;

			using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				if (dml == "I")
				{
					itm.EntryBy = userName;
					itm.EntryDate = DateTime.Now;
					itm.Status = 1;
				}

				await db.CreateRepository<Data.Domain.CommodityMapping>().CrudAsync(dml, itm);
				return UpdateCache(itm, dml);
			}
		}

		public static int UpdateBulk(List<Data.Domain.CommodityMapping> list, string dml)
		{
			var ret = 0;
			using (TransactionScope ts = new System.Transactions.TransactionScope())
			{
				using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{

					if (dml == "I")
					{
						foreach (var d in list)
						{
							d.ModifiedBy = Domain.SiteConfiguration.UserName;
							d.ModifiedDate = DateTime.Now;
							d.EntryBy = Domain.SiteConfiguration.UserName;
							d.EntryDate = DateTime.Now;

							ret = db.CreateRepository<Data.Domain.CommodityMapping>().CRUD("I", d);
							UpdateCache(d, dml);
						}
					}

					ts.Complete();
					return ret;
				}
			}
		}


		private static object s_syncObject = new object();
		private static int UpdateCache(Data.Domain.CommodityMapping item, string dml)
		{

			lock (s_syncObject)
			{
				var newlist = GetList().ToList();
				if (newlist.Where(w => w.MappingID == item.MappingID).Count() > 0)
				{
					newlist.RemoveAll(w => w.MappingID == item.MappingID);
				}

				if (dml != "D")
				{
					var h = Master.HSCodeLists.GetId(item.HSId);
					var c = Master.CommodityImex.GetId(item.CommodityID);
					item.MappingID = item.MappingID;
					item.Status = item.Status;
					item.ModifiedDate = item.ModifiedDate;
					item.ModifiedBy = item.ModifiedBy;
					item.HSCode = h.HSCode;
					item.HSDescription = h.Description;
					item.HSCodeCap = h.HSCode.ToString() + " ~ " + (h.Description + "").Replace(".", "");
					item.CommodityCode = c.CommodityCode;
					item.CommodityName = c.CommodityName;
					item.CommodityCap = c.CommodityCode + " - " + c.CommodityName;

					newlist.Add(item);
				}

				_cacheManager.Remove("App.master.CommodityImex");
				_cacheManager.Remove(cacheName);
				_cacheManager.Set(cacheName, newlist);
			}

			return 0;
		}

	}
}
