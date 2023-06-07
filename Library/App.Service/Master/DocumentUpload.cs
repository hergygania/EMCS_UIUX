using App.Data;
using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class DocumentUpload
    {
        private const string cacheName = "App.master.DocumentUpload";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        private static List<Data.Domain.DocumentUpload> ListDocumentUpload(string _ModulName)
        {
            List<Data.Domain.DocumentUpload> list = new List<Data.Domain.DocumentUpload>();
            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                IQueryable<Data.Domain.DocumentUpload> tb =
                    db.CreateRepository<Data.Domain.DocumentUpload>().Table.Select(e => e);
                list = tb.ToList();
            }

            return list.Where(a => a.ModulName == _ModulName).ToList();
        }

        public List<Data.Domain.Extensions.DocumentUpload> GetListDocumentUpload(string _modulName)
        {
            List<Data.Domain.Extensions.DocumentUpload> list = new List<Data.Domain.Extensions.DocumentUpload>();
            var doc = Service.Master.DocumentUpload.getListDocumentUpload(_modulName);

            return doc.OrderByDescending(o => o.EntryDate).ToList();
        }

        public static List<Data.Domain.Extensions.DocumentUpload> getListDocumentUpload(string _modulName)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from s in db.UserAccesses
                          join c in db.DocumentUpload on s.UserID equals c.EntryBy
                          where c.ModulName == _modulName
                          select new Data.Domain.Extensions.DocumentUpload
                          {
                              ModulName = c.ModulName,
                              FileName = c.FileName,
                              URL = c.URL,
                              EntryBy = s.FullName,
                              EntryDate = c.EntryDate,
                              Status = c.Status == 0 ? "In Progress" : c.Status == 1 ? "Success" : "Failed"
                          };

                return list.OrderByDescending(o => o.EntryDate).ToList();
            }
        }

        public static int crud(Data.Domain.DocumentUpload itm, string dml)
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
                return db.CreateRepository<Data.Domain.DocumentUpload>().CRUD(dml, itm);
            }
        }
    }
}
