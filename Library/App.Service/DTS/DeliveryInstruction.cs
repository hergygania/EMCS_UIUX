using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class DeliveryInstruction
    {
        public const string cacheName = "App.DTS.DeliveryInstruction";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.DeliveryInstruction item, string dml)
        {
            if (dml == "I")
            {
                item.ID = 0;
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;
            
            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    if (dml == "I")
                    {
                        var dataRes = db.CreateRepository<Data.Domain.DeliveryInstruction>().Add(item);
                        return Convert.ToInt32(item.ID);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.DeliveryInstruction>().CRUD(dml, item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.DeliveryInstruction> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryInstruction;
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.DeliveryInstructionView> GetListFilter(App.Data.Domain.DTS.DeliveryInstructionFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                string search = "";
                if (filter.searchName !=null)
                {
                    search = Regex.Replace(filter.searchName, @"[^0-9a-zA-Z]+", "");
                }
              
                parameterList.Add(new SqlParameter("@searchName", search == null ? "" : search));
                parameterList.Add(new SqlParameter("@createdby", filter.requestor == true ? Domain.SiteConfiguration.UserName : ""));
                parameterList.Add(new SqlParameter("@requestor", filter.requestor == true ? true : false));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryInstructionView>
                    (@"exec [dbo].[SP_GetDataDeliveryInstruction] @searchName, @requestor, @createdby", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.DeliveryInstruction GetId(Int64 ID)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryInstruction;
            var item = tb.Where(i => i.ID == ID).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryInstruction GetDetail(Int64 key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryInstruction;
            var item = tb.ToList().Where(i => i.ID == key).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryInstruction GetDetailByCustomKey(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryInstruction;
            var item = tb.Where(i => i.KeyCustom.Equals(key)).FirstOrDefault();
            return item;
        }

        public static Data.Domain.UserAccess GetDetailUser(string UserID)
        {
            var db = new Data.EfDbContext();
            var tb = db.UserAccesses;
            var item = tb.Where(a => a.UserID.Equals(UserID)).FirstOrDefault();
            return item;
        }


    }
}
