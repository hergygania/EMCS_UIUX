using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterAgreementTypePO
    {
        public static List<App.Data.Domain.MasterAgreementTypePO> GetList()
        {
            List<App.Data.Domain.MasterAgreementTypePO> result = new List<Data.Domain.MasterAgreementTypePO>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterAgreementTypePO>().Table.Select(x => x).ToList();
            return result;
        }
    }
}
