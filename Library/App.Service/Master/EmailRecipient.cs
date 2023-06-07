using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
    public class EmailRecipients
    {
        private const string cacheName = "App.master.EmailRecipient";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EmailRecipient> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.EmailRecipient>().Table.Where(e => e.Status == 1).Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }
        public static List<Data.Domain.EmailRecipient> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Purpose).Trim().ToLower().Contains(name)&&(c.Status==1))
                       orderby c.Purpose
                       select c;
            return list.ToList();
        }


        public static Data.Domain.EmailRecipient GetId(int id)
        {
            var item = GetList().Where(w => w.EmailRecipientID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.EmailRecipient itm, string dml)
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
                return db.CreateRepository<Data.Domain.EmailRecipient>().CRUD(dml, itm);
            }
        }

        public static bool ValidateInputHtmlInjection(string strData, string strValueValidate)
        {
            bool bValidate = true;
            if (strData == null)
            {
                strData = "";
            }

            var strResult = strData.IndexOfAny(strValueValidate.ToCharArray()) != -1;

            if (strResult)
            {
                bValidate = false;
            }

            return bValidate;
        }

    }
}
