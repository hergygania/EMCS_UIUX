using FastMember;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
    public class ManualVetting
    {
        //
        // Summary:
        //     Initializes a get data manual vetting of the Data.Domain.ManualVetting
        //     class using the specified name of the list 
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static List<Data.Domain.ManualVetting> GetList()
        {
            using (var db = new Data.EfDbContext())
            {
                var list = db.ManualVettings.ToList();

                return list;
            }
        }

        //
        // Summary:
        //     Initializes a get data manual vetting of the Data.Domain.SP_ManualVetting
        //     class using the specified name of the list (method stroe procedeure)
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static List<Data.Domain.SP_ManualVetting> GetSPList(int StartNum, int EndNum, string PartsNumber, string HSID, string OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@PartsNumber", PartsNumber));
                parameterList.Add(new SqlParameter("@HSID", HSID));
                parameterList.Add(new SqlParameter("@OMCode", OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_ManualVetting>("[pis].[GetManualVetting] 1, @PartsNumber, @HSID, @OMCode, @StartNum, @EndNum, @OrderBy", parameters).ToList();
                return data;
            }
        }

        //
        // Summary:
        //     Initializes a get count data manual vetting of the Data.Domain.SP_ManualVetting
        //     class using the specified name of the list (method stroe procedeure)
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static List<Data.Domain.SP_ManualVetting> GetSPCount(int StartNum, int EndNum, string PartsNumber, string HSID, string OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@PartsNumber", PartsNumber));
                parameterList.Add(new SqlParameter("@HSID", HSID));
                parameterList.Add(new SqlParameter("@OMCode", OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_ManualVetting>("[pis].[GetManualVetting] 1, @PartsNumber, @HSID, @OMCode, @StartNum, @EndNum, @OrderBy", parameters).ToList();
                return data;
            }
        }

        //
        // Summary:
        //     Initializes a get data manual vetting by user login of the Data.Domain.SP_ManualVetting
        //     class using the specified name of the list (method stroe procedeure)
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static List<Data.Domain.SP_ManualVetting> GetSPList_Paging(string username, int StartNum, int EndNum, string PartsNumber, string HSID, string OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 3000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserName", username));
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@PartsNumber", PartsNumber));
                parameterList.Add(new SqlParameter("@HSID", HSID));
                parameterList.Add(new SqlParameter("@OMCode", OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_ManualVetting>("[pis].[GetManualVettingByUsername] @UserName, 1, @PartsNumber, @HSID, @OMCode, @StartNum, @EndNum, @OrderBy", parameters).ToList();
                return data;
            }
        }

        //
        // Summary:
        //     Initializes a get count data manual vetting by user login of the Data.Domain.SP_ManualVetting
        //     class using the specified name of the list (method stroe procedeure)
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static int GetSPCount_Paging(string username, int StartNum, int EndNum, string PartsNumber, string HSID, string OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 1000;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserName", username));
                parameterList.Add(new SqlParameter("@startnum", StartNum));
                parameterList.Add(new SqlParameter("@endnum", EndNum));
                parameterList.Add(new SqlParameter("@PartsNumber", PartsNumber));
                parameterList.Add(new SqlParameter("@HSID", HSID));
                parameterList.Add(new SqlParameter("@OMCode", OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[pis].[GetManualVettingByUsername] @UserName, 2, @PartsNumber, @HSID, @OMCode, @StartNum, @EndNum, @OrderBy", parameters).ToList();
                return data.FirstOrDefault();
            }
        }

        //
        // Summary:
        //     Initializes a get data manual vetting of the Data.Domain.ManualVetting
        //     class using the specified by ID
        // Parameters:
        //   name:
        //     The ID of the data manual vetting.
        // Returns:
        //     The list Data.Domain.ManualVetting.
        public static Data.Domain.ManualVetting GetById(Int64 ID)
        {
            using (var db = new Data.EfDbContext())
            {
                var data = db.ManualVettings.Where(w => w.ID == ID).FirstOrDefault();

                return data;
            }
        }


        //
        // Summary:
        //     save data manual vetting of the Data.Domain.ManualVetting
        //     class using the specified by type crud
        // Parameters:
        //   name:
        //     The item of class the Data.Domain.ManualVetting and string type crud.
        // Returns:
        //     The int 1 or 0.
        public static int Update(Data.Domain.ManualVetting itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                return db.CreateRepository<Data.Domain.ManualVetting>().CRUD(dml, itm);
            }
        }

        public static int UpdateBulkNew(List<Data.Domain.ManualVetting> list, string dml)
        {
            var ret = 0;
            try
            {
                Delete(Domain.SiteConfiguration.UserName);

                string connString = ConfigurationManager.ConnectionStrings["pisConnection"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);

                    bulkCopy.DestinationTableName = "pis.ManualVetting";
                    bulkCopy.BulkCopyTimeout = 600;

                    // Add your column mappings here
                    bulkCopy.ColumnMappings.Add("PRIMPSO", "PRIMPSO");
                    bulkCopy.ColumnMappings.Add("PartNumber", "PartNumber");
                    bulkCopy.ColumnMappings.Add("ManufacturingCode", "ManufacturingCode");
                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                    bulkCopy.ColumnMappings.Add("CustomerRef", "CustomerRef");
                    bulkCopy.ColumnMappings.Add("CustomerCode", "CustomerCode");
                    bulkCopy.ColumnMappings.Add("Status", "Status");
                    bulkCopy.ColumnMappings.Add("OM", "OM");
                    bulkCopy.ColumnMappings.Add("OMCode", "OMCode");
                    bulkCopy.ColumnMappings.Add("OrderClassCode", "OrderClassCode");
                    bulkCopy.ColumnMappings.Add("ProfileNumber", "ProfileNumber");
                    bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                    bulkCopy.ColumnMappings.Add("EntryDate", "EntryDate");
                    bulkCopy.ColumnMappings.Add("ModifiedDate", "ModifiedDate");
                    bulkCopy.ColumnMappings.Add("EntryBy", "EntryBy");
                    bulkCopy.ColumnMappings.Add("ModifiedBy", "ModifiedBy");

                    connection.Open();

                    // write the data in the “dataTable”

                    var data = list.Select(p => new
                    {
                        p.PRIMPSO,
                        p.PartNumber,
                        p.ManufacturingCode,
                        p.PartName,
                        p.CustomerCode,
                        p.CustomerRef,
                        p.Status,
                        p.OM,
                        p.OMCode,
                        p.OrderClassCode,
                        p.ProfileNumber,
                        p.Remarks,
                        p.RemarksName,
                        p.EntryBy,
                        p.EntryDate,
                        p.ModifiedBy,
                        p.ModifiedDate
                    }).Distinct().ToList();

                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(data,
                        "PRIMPSO", "PartNumber", "ManufacturingCode", "PartName", "CustomerRef", "CustomerCode", "Status", "OM", "OMCode", "OrderClassCode", "ProfileNumber", "Remarks", "EntryDate", "ModifiedDate", "EntryBy", "ModifiedBy"))
                    {
                        table.Load(reader);
                    }

                    bulkCopy.WriteToServer(table);

                    connection.Close();
                }

                return ret;
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when Insert Data Manual Vetting. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when Insert Data Manual Vetting. Error Message: " + ex.InnerException.Message);
            }
        }

        //
        // Summary:
        //     update field Remarks of manual vetting after uploads file to manual vetting
        // Returns:
        //     The int 1 or 0.
        public static int UpdateRemarks()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                int data = db.DbContext.Database.ExecuteSqlCommand("EXEC [pis].[UpdateRemarksManualVetting]");
                return data;
            }
        }

        public static void Delete(string UserName)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    db.DbContext.Database.ExecuteSqlCommand("DELETE FROM [pis].[ManualVetting] WHERE EntryBy = '" + UserName + "'");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.InnerException.Message);
            }
        }

    }
}
