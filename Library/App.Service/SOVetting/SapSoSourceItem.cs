using App.Data;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.SOVetting
{
    public class SapSoSourceItem
    {
        private static readonly EfDbContext EfDbContext = new EfDbContext();
        public static List<Data.Domain.SOVetting.SapSoSourceItem> GetListSearchSd(string salesDocument)
        {
            List<Data.Domain.SOVetting.SapSoSourceItem> result = EfDbContext.SapSoSourceItem
                .Where(s => s.SalesDocument == salesDocument)
                .ToList();
            return result;
        }        
    }
}
