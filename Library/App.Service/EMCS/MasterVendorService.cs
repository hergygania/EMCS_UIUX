using App.Data.Domain.EMCS;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.EMCS
{
    public class MasterVendorService
    {
        public static List<Data.Domain.EMCS.Vendor> GetVendorList()
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var tb = db.Vendors;
                    return tb.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static List<Data.Domain.EMCS.Vendor> GetVendorList(MasterSearchForm crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
                    if(name == "")
                    {
                        var list = from c in GetVendorList()
                                   select c;
                        return list.ToList();
                    }
                    else
                    {
                        var list = from c in db.Vendors
                                   where c.Name.Contains(name)  select c;
                        return list.ToList();
                    }
                    
                }
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }

        }
        public static long InsertVendor(Vendor objModel)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", objModel.Id));
                    parameterList.Add(new SqlParameter("@Name", objModel.Name));
                    parameterList.Add(new SqlParameter("@Code", 'M'));
                    parameterList.Add(new SqlParameter("@Address",objModel.Address));
                    parameterList.Add(new SqlParameter("@City", objModel.City));
                    parameterList.Add(new SqlParameter("@Telephone", objModel.Telephone));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@IsManualEntry", true));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[SP_MasterVendorAdd]@Id, @Name,@Code, @Address,@City,@Telephone, @CreateBy,@UpdateBy,@IsManualEntry", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static Data.Domain.EMCS.Vendor GetDataById(long Id)
        {
            var data = GetVendorList().Where(a => a.Id == Id).FirstOrDefault();
            return data;
        }
        public static long DeleteData(long Id)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", Id));
                    SqlParameter[] parameters = parameterList.ToArray();

                    db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[SP_MasterVendorDelete] @Id", parameters).FirstOrDefault();
                    return Id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
