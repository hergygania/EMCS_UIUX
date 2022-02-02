using App.Data.Caching;
using System;
using App.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcDelegation
    {
        public const string CacheName = "App.EMCS.SvcDelegation";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static void DelegationCipl(long idReq, string delegationTo)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = @"sp_insert_update_delegation 'CIPL', " + idReq + ", '" + SiteConfiguration.UserName + "', 'user', '" + delegationTo + "'";
                db.Database.ExecuteSqlCommand(sql);
            }
        }
    }
}
