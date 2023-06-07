using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcNotificationQueue
    {
        public const string CacheName = "App.EMCS.SvcNotificationQueue";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.NotificationQueue> GetList(int isRead, int isDelete)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    return db.NotificationQueue.Where(i=> i.NotificationTo == SiteConfiguration.UserName && (i.IsRead == (isRead == 1) || isRead == -1) && i.IsDelete == (isDelete == 1)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static long UpdateStatus(List<long> id, bool isRead, bool isDelete)
        {
            using (var db = new Data.EmcsContext())
            {
                List<Data.Domain.EMCS.NotificationQueue> items = db.NotificationQueue.Where(i => id.Contains(i.Id)).ToList();
                string updatedBy = SiteConfiguration.UserName;

                foreach (var item in items) {
                    if (isRead)
                        item.IsRead = true;
                    item.IsDelete = isDelete;
                    item.UpdatedBy = updatedBy;
                    item.UpdatedDate = DateTime.Now;
                }
                db.SaveChanges();

                return 1;
            }
        }
    }
}
