using App.Data;
using System.Data.Entity;
using System.Linq;

namespace App.Service.SOVetting
{
    public class SapSoSearch
    {
        private static readonly EfDbContext EfDbContext = new EfDbContext();
        public static Data.Domain.SOVetting.SapSoSearch GetListByUser(string id)
        {
            Data.Domain.SOVetting.SapSoSearch result = EfDbContext.SapSoSearch
                .FirstOrDefault(s => s.user_id == id);

            return result;
        }
        public static void InsertDb(Data.Domain.SOVetting.SapSoSearch search)
        {
            EfDbContext.SapSoSearch.Add(search);
            EfDbContext.SaveChanges();
        }
        public static void UpdateDb(Data.Domain.SOVetting.SapSoSearch search)
        {
            EfDbContext.Entry(search).State = EntityState.Modified;
            EfDbContext.SaveChanges();
        }
        public static bool CheckSearchUser(string id)
        {
            return EfDbContext.SapSoSearch.Count(e => e.user_id == id) > 0;
        }
        public static Data.Domain.SOVetting.SapSoSearch SearchUser(string id)
        {
            Data.Domain.SOVetting.SapSoSearch result = EfDbContext.SapSoSearch
                .FirstOrDefault(s => s.user_id == id);
            return result;
        }
    }
}
