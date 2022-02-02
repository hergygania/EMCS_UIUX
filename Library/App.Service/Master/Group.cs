using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class Group
    {
        private const string cacheName = "App.master.Group";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.Group> GetGroupList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Group>().Table.Where(w => w.IsActive == true).Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.Group> GetGroupList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetGroupList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();

        }

        public static Data.Domain.Group GetId(int id)
        {
            var item = GetGroupList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Group GetCode(string code)
        {
            var item = GetGroupList().Where(w => w.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Group GetName(string name)
        {
            var item = GetGroupList().Where(w => w.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static int crud(Data.Domain.Group itm, string dml)
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
                return db.CreateRepository<Data.Domain.Group>().CRUD(dml, itm);
            }
        }
    }
}
