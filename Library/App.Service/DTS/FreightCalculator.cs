using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class FreightCalculator
    {
        public const string cacheName = "App.DTS.DeliveryRequisition";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.DeliveryRequisition item, string dml)
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
                    return db.CreateRepository<Data.Domain.DeliveryRequisition>().CRUD(dml, item);
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
        public static List<Data.Domain.DeliveryRequisition> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryRequisition;
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static DataTable GetListFilter(string sort, int limit = 0, int offset = 10, string destination = "", string origin = "")
        {
            string key = string.Format(cacheName);

            var unitModel = Service.DTS.FreightCalculator.GetListModel();
            var data = unitModel.ToList();

            System.Data.DataTable dtfreight = new System.Data.DataTable();
            dtfreight.Clear();
            dtfreight.Columns.Add("Origin");
            dtfreight.Columns.Add("Area");
            dtfreight.Columns.Add("Provinsi");
            dtfreight.Columns.Add("KabupatenKota");
            dtfreight.Columns.Add("IbuKotaKabupaten");

            for (int row = 0; row < data.Count; row++)
            {
                dtfreight.Columns.Add(data[row].Model);
            }

            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dtsConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = cnn;

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "offset";
            param1.Value = offset;
            cmd.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "limit";
            param2.Value = limit;
            cmd.Parameters.Add(param2);

            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "destination";
            param3.Value = destination;
            cmd.Parameters.Add(param3);

            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "origin";
            param4.Value = origin;
            cmd.Parameters.Add(param4);

            cmd.CommandText = "sp_getFreightCalculator";
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            DbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DataRow dtrow = dtfreight.NewRow();
                for (var i = 0; i < dtfreight.Columns.Count; i++)
                {
                    string columnName = dtfreight.Columns[i].ColumnName;
                    dtrow[columnName] = reader.GetValue(reader.GetOrdinal(columnName));
                }
                dtfreight.Rows.Add(dtrow);
            }

            cnn.Close();
            return dtfreight;
        }
        public static List<App.Data.Domain.FreightCost> GetListFilter(App.Data.Domain.DTS.FreightRouteOptions filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                string model = "";
                string route = "";
                string origin = "";
                string destination = "";
                if (filter.Model != null)
                {
                    model = filter.Model;
                }
                if (filter.Route != null)
                {
                    route = filter.Route;
                }
                if (filter.Origin != null)
                {
                    origin = filter.Origin;
                }
                if (filter.Destination != null)
                {
                    destination = filter.Destination;
                }
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@route", route));                
                parameterList.Add(new SqlParameter("@origin", origin));
                parameterList.Add(new SqlParameter("@destination", destination));
                parameterList.Add(new SqlParameter("@model", model));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.FreightCost>
                    (@"exec [dbo].[SP_GetNewFreightCalculator] @route,@origin,@destination,@model", parameters).ToList();

                return data;
            }
        }


        public static DataTable GetListFilterforDownload(string sort, string destination = "", string origin = "")
        {
            string key = string.Format(cacheName);
            int limit = 0;
            int offset = 10;
            var unitModel = Service.DTS.FreightCalculator.GetListModel();
            var data = unitModel.ToList();

            System.Data.DataTable dtfreight = new System.Data.DataTable();
            dtfreight.Clear();
            dtfreight.Columns.Add("Origin");
            dtfreight.Columns.Add("Area");
            dtfreight.Columns.Add("Provinsi");
            dtfreight.Columns.Add("KabupatenKota");
            dtfreight.Columns.Add("IbuKotaKabupaten");

            for (int row = 0; row < data.Count; row++)
            {
                dtfreight.Columns.Add(data[row].Model);
            }

            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dtsConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = cnn;

            SqlParameter param1 = new SqlParameter();
            param1.ParameterName = "offset";
            param1.Value = "";
            cmd.Parameters.Add(param1);

            SqlParameter param2 = new SqlParameter();
            param2.ParameterName = "limit";
            param2.Value = "";
            cmd.Parameters.Add(param2);

            SqlParameter param3 = new SqlParameter();
            param3.ParameterName = "destination";
            param3.Value = "";
            cmd.Parameters.Add(param3);

            SqlParameter param4 = new SqlParameter();
            param4.ParameterName = "origin";
            param4.Value = "";
            cmd.Parameters.Add(param4);

            cmd.CommandText = "sp_getFreightCalculator";
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            DbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DataRow dtrow = dtfreight.NewRow();
                for (var i = 0; i < dtfreight.Columns.Count; i++)
                {
                    string columnName = dtfreight.Columns[i].ColumnName;
                    dtrow[columnName] = reader.GetValue(reader.GetOrdinal(columnName));
                }
                dtfreight.Rows.Add(dtrow);
            }

            cnn.Close();
            return dtfreight;
        }

        public static int GetTotal(string origin = "", string destination = "")
        {
            var total = 0;
            string cnnString = System.Configuration.ConfigurationManager.ConnectionStrings["dtsConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(cnnString);
            SqlCommand cmd = new SqlCommand();
            SqlParameter param1 = new SqlParameter();
            SqlParameter param2 = new SqlParameter();
            SqlParameter param3 = new SqlParameter();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Connection = cnn;
            param1.ParameterName = "isCount";
            param1.Value = 1;
            cmd.Parameters.Add(param1);

            param2.ParameterName = "origin";
            param2.Value = origin;
            cmd.Parameters.Add(param2);

            param3.ParameterName = "destination";
            param3.Value = destination;
            cmd.Parameters.Add(param3);

            cmd.CommandText = "sp_getFreightCalculator";
            cnn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return Convert.ToInt32(reader.GetValue(reader.GetOrdinal("total")));
            }
            cnn.Close();
            return total;
        }

        public static List<Data.Domain.DTS.UnitModels> GetListModel()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                var data = db.DbContext.Database.SqlQuery<Data.Domain.DTS.UnitModels>(@"exec [dbo].[SP_Models]").ToList();
                return data;
            }
        }
        public static Data.Domain.DTS.FreightOptions GetDataFreightRouteSales(string Origin, string Destination, string Model)
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@origin", Origin));
                parameterList.Add(new SqlParameter("@destination", Destination));
                parameterList.Add(new SqlParameter("@model", Model));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.FreightOptions>
                    (@"exec [dbo].[SP_GetFreightCostData] @origin, @destination,@model", parameters).FirstOrDefault();
                return data;
            }
        }
        public static List<Data.Domain.DTS.FreightOptions> GetListFreightOption(string type = "origin", string key = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", type));
                parameterList.Add(new SqlParameter("@key", key));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.FreightOptions>
                    (@"exec [dbo].[SP_GetFreightOption] @type, @key", parameters).ToList();
                return data;
            }
        }
        public static List<Data.Domain.DTS.FreightRouteOptions> GetListFreightRouteOption(string key = "",string type ="")
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();               
                parameterList.Add(new SqlParameter("@key", key));
                parameterList.Add(new SqlParameter("@type", type));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.FreightRouteOptions>
                    (@"exec [dbo].[SP_GetFreightRouteOption] @key,@type", parameters).ToList();
                return data;
            }
        }
        public static Data.Domain.FreightCalculator GetDetailFreight(string origin = "", string destination = "", string model = "", string etd = "")
        {
            var db = new Data.DTSContext();
            var tb = db.FreightCalculators;
            try
            {
                var item = tb.Where(i => i.Origin.Equals(origin) && i.KabupatenKota.Equals(destination) && i.Model.Contains(model)).FirstOrDefault();
                return item;
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }

    }

}
