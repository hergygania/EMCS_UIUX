using System;
using App.Data.Caching;
using App.Domain;

namespace App.Web.Controllers.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcDelegation
    {
        public const string CacheName = "App.EMCS.SvcDelegation";

        public static void DelegationCipl(long idReq, string delegationTo)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    db.Database.CommandTimeout = 600;
                    var sql = @"sp_insert_update_delegation 'CIPL', " + idReq + ", '" + SiteConfiguration.UserName + "', 'user', '" + delegationTo + "'";
                    db.Database.ExecuteSqlCommand(sql);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }

        }
    }
}
