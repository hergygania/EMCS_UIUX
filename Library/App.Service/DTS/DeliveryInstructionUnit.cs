using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class DeliveryInstructionUnit
    {
        public const string CacheName = "App.DTS.DeliveryInstructionUnit";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int Crud(Data.Domain.DeliveryInstructionUnit item, string dml)
        {
            if (dml == "I")
            {
                item.ID = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    if (dml == "I")
                    {
                        // ReSharper disable once UnusedVariable
                        var dataRes = db.CreateRepository<Data.Domain.DeliveryInstructionUnit>().Add(item);
                        return Convert.ToInt32(item.ID);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.DeliveryInstructionUnit>().CRUD(dml, item);
                    }
                }
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }

        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.DeliveryInstructionUnit> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryInstructionUnit;
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="ID"> id master job code</param>
        /// <seealso cref="int"></seealso>
        // ReSharper disable once InconsistentNaming
        public static Data.Domain.DeliveryInstructionUnit GetId(Int64 ID)
        {
            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryInstructionUnit;
                var item = tb.Where(i => i.ID == ID).FirstOrDefault();
                return item;
            }
        }

        public static Data.Domain.DeliveryInstructionUnit GetDetail(Int64 key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryInstructionUnit;
            var item = tb.ToList().Where(i => i.ID == key).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.DeliveryInstructionUnit> GetDetailByHeaderId(long headerId)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryInstructionUnit;

            return tb.Where(i => i.HeaderID.Equals(headerId)).ToList();
        }

        public static Data.Domain.UserAccess GetDetailUser(string userId)
        {
            var db = new Data.EfDbContext();
            var tb = db.UserAccesses;
            var item = tb.Where(a => a.UserID.Equals(userId)).FirstOrDefault();
            return item;
        }

        public static void DeleteByHeaderId(long headerId)
        {
            var list = GetDetailByHeaderId(headerId);

            foreach (var data in list)
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    db.CreateRepository<Data.Domain.DeliveryInstructionUnit>().Delete(data);
                }
            }
        }

        public static bool GetRoleDRApproval(long HeaderId, string userId)
        {
            var result = false;
            using (var db = new Data.EfDbContext())
            {
               
                var UserRole = (from p in db.UserAccesses where p.UserID == userId select p).FirstOrDefault();
                if (UserRole.RoleID == 11 || UserRole.RoleID == 20 || UserRole.RoleID == 21)
                {
                    result = true;
                }
                else if (UserRole.RoleID == 12)
                {
                    if (HeaderId == 0)
                    {
                        result = true;
                    }
                    else
                    {
                        if (DIHisOwned(HeaderId, userId))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        public static bool DIHisOwned(Int64 id, string userId)
        {
            using (var db = new Data.DTSContext())
            {
                var result = false;

                var tb = db.DeliveryInstruction.FirstOrDefault(a => a.ID == id && a.CreateBy == userId);
                if (tb != null)
                {
                    result = true;
                }

                return result;
            }
        }
    }
}
