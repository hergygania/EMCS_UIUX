using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.SAP
{
    public class MaterialDescription
    {
        public static List<Data.Domain.SAP.MaterialDescription> GetMaterialDescriptionList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.SAP.MaterialDescription>()
                    .Table.Where(w => w.IsActive == true).Select(e => e).OrderBy(o => o.MaterialDesc).ToList();
                return tb;
            }
        }

        public static int GetCountMaterialDescription()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.SAP.MaterialDescription>()
                    .Table.Where(w => w.IsActive == true).Select(e => e).Count();
                return tb;
            }
        }

        public static List<Data.Domain.SAP.MaterialDescription>
            GetSelectMaterialDescription(string searchName, int pageSize, int page)
        {
            var list = new List<Data.Domain.SAP.MaterialDescription>();
            int offset = pageSize * (page - 1);
            try
            {
                using (var db = new Data.EfDbContext())
                {
                    var tb = from c in db.MaterialDescription.AsNoTracking()
                                 .Where(w => (w.MaterialType.ToString() + "|" + w.MaterialDesc.Replace(".", "")).ToLower().Contains(searchName.ToLower())).ToList()
                             select new Data.Domain.SAP.MaterialDescription()
                             {
                                 ID = c.ID,
                                 MaterialType = c.MaterialType,
                                 MaterialDesc = c.MaterialDesc,
                                 IsActive = c.IsActive,
                                 EntryBy = c.EntryBy,
                                 EntryDate = c.EntryDate,
                                 ModifiedBy = c.ModifiedBy,
                                 ModifiedDate = c.ModifiedDate,
                             };

                    return tb
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.MaterialDesc)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
            }

            return list;

        }
    }
}
