using App.Data.Caching;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Master
{
    public class MasterGeneric
    {
        private const string cacheName = "App.master.Generic";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.MasterGeneric> GetGenericList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.MasterGeneric>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.MasterGeneric> getGeneric(string code)
        {
            var list = GetGenericList().Where(w => w.Code == code);
            return list.ToList();
        }

        public static listNote getGenericNote(string code)
        {
            listNote allListNote = new listNote();
            var list = GetGenericList().Where(w => w.Code == code).ToList();

            if (list.Count() > 0)
            {
                allListNote.Note1 = list.Where(w1 => w1.Name == "Note1").Select(s => s.Value).FirstOrDefault().ToString();
                allListNote.Note2 = list.Where(w1 => w1.Name == "Note2").Select(s => s.Value).FirstOrDefault().ToString();
                allListNote.Note3 = list.Where(w1 => w1.Name == "Note3").Select(s => s.Value).FirstOrDefault().ToString();
                allListNote.Note4 = list.Where(w1 => w1.Name == "Note4").Select(s => s.Value).FirstOrDefault().ToString();
            }

            return allListNote;
        }

        public class listNote
        {
            public string Note1 { get; set; }
            public string Note2 { get; set; }
            public string Note3 { get; set; }
            public string Note4 { get; set; }
        }


        public static List<Data.Domain.MasterGeneric> getGenericCBM(string code, string CBM)
        {
            var list = GetGenericList().Where(w => w.Code == code).
                Where(w => w.Name == CBM);
            return list.ToList();
        }



        public static List<Data.Domain.MasterGeneric> GetGenericList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetGenericList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();

        }

        public static Data.Domain.MasterGeneric GetId(int id)
        {
            var item = GetGenericList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static Data.Domain.MasterGeneric GetIDByCodeName(string code, string name)
        {
            var item = GetGenericList().Where(w => w.Name.Trim().ToLower() == name.Trim().ToLower())
                .Where(w => w.Code.Trim().ToString() == code.Trim().ToString()).FirstOrDefault();
            return item;
        }

        public static int crud(Data.Domain.MasterGeneric itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterGeneric>().CRUD(dml, itm);
            }
        }


    }
}
