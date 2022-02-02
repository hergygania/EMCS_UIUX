using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Configuration;

namespace App.Data
{
    /// <summary>
    /// Get Data From Database
    /// </summary>
    public class DbTransaction
    {
        /// <summary>
        /// return sql connection string 
        /// </summary>
        private static string SqlConnectionString = ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString;
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(SqlConnectionString);
        }

        #region DbExecute
        /// <summary>
        /// Execute date base to from sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        /// <param name="Query">The key defining the data.</param>
        /// <param name="IsStorageProcedure">The data.</param>
        public static void DbExecute(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    sqlConnection.Execute(Query);
                else
                    sqlConnection.Execute(Query, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Execute date base to from sql query, 
        /// with  filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static void DbExecute(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    sqlConnection.Execute(Query, Parameter);
                else
                    sqlConnection.Execute(Query, Parameter, commandType: CommandType.StoredProcedure);

            }
        }
        #endregion


        #region DbToString
        /// <summary>
        /// Return System.string from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static string DbToString(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<string>(Query).FirstOrDefault();
                else
                    return sqlConnection.Query<string>(Query, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Return System.string from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static string DbToString(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<string>(Query, Parameter).FirstOrDefault();
                else
                    return sqlConnection.Query<string>(Query, Parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion


        #region DbToInt
        /// <summary>
        /// Return System.int from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static int DbToInt(string Query, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<int>(Query).FirstOrDefault();
                else
                    return sqlConnection.Query<int>(Query, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }

        /// <summary>
        /// Return System.int from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static int DbToInt(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (var sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<int>(Query, Parameter).FirstOrDefault();
                else
                    return sqlConnection.Query<int>(Query, Parameter, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }
        #endregion


        #region DbToList
        /// <summary>
        /// Return System.Collection.List<> from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static List<T> DbToList<T>(string Query, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<T>(Query).ToList();
                else
                    return sqlConnection.Query<T>(Query, commandType: CommandType.StoredProcedure).ToList();
            }
        }


        //// <summary>
        /// Return System.Collection.List<> from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static List<T> DbToList<T>(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<T>(Query, Parameter).ToList();
                else
                    return sqlConnection.Query<T>(Query, Parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion

        #region DbToDictionary
        // Summary:
        //     For use this method, the return name must be 'Key' and 'Value'
        //     
        //     
        //
        // Returns:
        //     Use DbToDictionary<K,V> by support by Dapper is required
        //     
        //     
        //     
        //
        // Exceptions:
        //   Return Value not 'Key' and 'Value'
        //     
        //     
        public static Dictionary<dynamic, dynamic> DBToDictionary(string Query, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query(Query).ToDictionary(row => row.Key, row => row.Value);
                else
                    return sqlConnection.Query(Query, commandType: CommandType.StoredProcedure).ToDictionary(row => row.Key, row => row.Value);
            }
        }


        //// <summary>
        /// Return System.Collection.List<> from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static Dictionary<dynamic, dynamic> DBToDictionary(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query(Query, Parameter).ToDictionary(row => row.Key, row => row.Value);

                else
                    return sqlConnection.Query(Query, Parameter, commandType: CommandType.StoredProcedure).ToDictionary(row => row.Key, row => row.Value);
            }
        }
        #endregion



        #region DbToDataSet
        public static DataSet DbToDataSet(string Query, object Parameter)
        {
            return ExecuteDataSet(Query, ObjetToSqlParameter(Parameter));
        }
        private static DataSet ExecuteDataSet(string Query, params SqlParameter[] arrParam)
        {
            DataSet ds = new DataSet();

            // Open the connection 
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Define the command 
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = Query;

                    // Handle the parameters
                    if (arrParam != null)
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);

                    // Define the data adapter and fill the dataset
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(ds);
                }
            }
            return ds;
        }
        #endregion
        
        #region DbToDataTable
        // Summary:
        //     This methode is not required because list object more faster than System.Data.DataTabe
        //     Gets the Pertamina.IP2P.Classes data for the current application's
        //     return data from sql query.
        //
        // Returns:
        //     Use DbToList<> by support by Dapper is required
        //     Returns a System.Data.DataTable object that
        //     contains the contents of the Pertamina.IP2P.Classes object
        //     for the current application's default connection.
        //
        // Exceptions:
        //   Pertamina.IP2P.Classes.DbToDataTableErrorsException:
        //     Could not retrieve a Pertamina.IP2P.Classes.DbTransaction object
        //     with the application settings data.
        public static DataTable DbToDataTable(string Query, bool IsStorageProcedure)
        {
            return ExecuteDataTable(IsStorageProcedure, Query, ObjetToSqlParameter(new { }));
        }

        // Summary:
        //     This methode is not required because list object more faster than System.Data.DataTabe
        //     Gets the Pertamina.IP2P.Classes data for the current application's
        //     return data from sql storage procedure.
        //
        // Returns:
        //     Use DbToList<> by support by Dapper is required
        //     Returns a System.Data.DataTable object that
        //     contains the contents of the Pertamina.IP2P.Classes object
        //     for the current application's default connection.
        //
        // Exceptions:
        //   Pertamina.IP2P.Classes.DbToDataTableErrorsException:
        //     Could not retrieve a Pertamina.IP2P.Classes.DbTransaction object
        //     with the application settings data.
        public static DataTable DbToDataTable(string Query, object Parameter, bool IsStorageProcedure)
        {
            return ExecuteDataTable(IsStorageProcedure, Query, ObjetToSqlParameter(Parameter));
        }

        private static SqlParameter[] ObjetToSqlParameter(object dataObject)
        {
            Type type = dataObject.GetType();
            PropertyInfo[] props = type.GetProperties();
            List<SqlParameter> paramList = new List<SqlParameter>();

            for (int i = 0; i < props.Length; i++)
            {

                if (props[i].PropertyType.IsValueType || props[i].PropertyType.Name == "String" || props[i].PropertyType.Name == "Object" || props[i].PropertyType.Name == "DataTable" || props[i].PropertyType.Name == "Byte[]")
                {
                    object fieldValue = type.InvokeMember(props[i].Name, BindingFlags.GetProperty, null, dataObject, null);
                    SqlParameter sqlParameter = new SqlParameter("@" + props[i].Name, fieldValue);
                    paramList.Add(sqlParameter);
                }
            }
            return paramList.ToArray();
        }

        private static DataTable ExecuteDataTable(bool IsStorageProcedure, string Query, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();

            // Open the connection 
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                // Define the command 
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConnection;
                    if (IsStorageProcedure)
                        cmd.CommandType = CommandType.StoredProcedure;
                    else
                        cmd.CommandType = CommandType.Text;

                    cmd.CommandText = Query;

                    //// Handle the parameters 
                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                            cmd.Parameters.Add(param);
                    }

                    //cmd.Parameters.Add("MenuId", 1);

                    // Define the data adapter and fill the dataset 
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        #endregion


        #region DbToDynamic
        /// <summary>
        /// Return System.Collection.dynamic from date base using sql query, 
        /// with parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static dynamic DbToDynamic(string Query, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<dynamic>(Query).ToList();
                else
                    return sqlConnection.Query<dynamic>(Query, commandType: CommandType.StoredProcedure).ToList();
            }
        }


        //// <summary>
        /// Return System.Collection.dynamic from date base using sql query, 
        /// with filtering parameter query string and 
        /// if query string is storage procedure
        /// </summary>
        public static dynamic DbToDynamic(string Query, object Parameter, bool IsStorageProcedure)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlConnectionString))
            {
                if (sqlConnection.State != ConnectionState.Open)
                    sqlConnection.Open();

                if (!IsStorageProcedure)
                    return sqlConnection.Query<dynamic>(Query, Parameter).ToList();
                else
                    return sqlConnection.Query<dynamic>(Query, Parameter, commandType: CommandType.StoredProcedure).ToList();
            }
        }
        #endregion

        
        #region 'BulkCopy'
        public static void BulkCopy(DataTable dt, string TableName, int IDForeignKey)
        {
            SqlBulkCopy bulkCopy = null;
            try
            {

                bulkCopy = new SqlBulkCopy(SqlConnectionString);
                bulkCopy.DestinationTableName = TableName;

                foreach (var column in dt.Columns)
                    bulkCopy.ColumnMappings.Add(column.ToString(), column.ToString());

                bulkCopy.WriteToServer(dt);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))
                {
                    string pattern = @"\d+";
                    Match match = Regex.Match(ex.Message.ToString(), pattern);
                    var index = Convert.ToInt32(match.Value) - 1;

                    FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                    var sortedColumns = fi.GetValue(bulkCopy);
                    var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                    FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                    var metadata = itemdata.GetValue(items[index]);

                    var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                    throw new DataException(String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                }
                throw;
            }

        }
        #endregion
    }
}
