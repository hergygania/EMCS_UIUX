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
    public class MasterSubcon
    {
        public static List<Data.Domain.EMCS.MasterSubConCompany> GetSubconList()
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var tb = db.MasterSubConCompany;
                    return tb.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public static List<Data.Domain.EMCS.MasterSubConCompany> GetSubconList(MasterSearchForm crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
                    var list = from c in GetSubconList()
                               select c;
                    return list.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex.InnerException;
            }
           
        }
        public static long InsertSubcon(MasterSubConCompany subConCompany)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", subConCompany.Id));
                    parameterList.Add(new SqlParameter("@Name", subConCompany.Name));
                    string Value = subConCompany.Name.Replace(" ", "");
                    parameterList.Add(new SqlParameter("@Value", Value));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[sp_SubConCompanyAdd]@Id, @Name, @Value, @CreateBy,@UpdateBy", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static MasterSubConCompany GetDataById(long Id)
        {
            var data = GetSubconList().Where(a => a.Id == Id).FirstOrDefault(); 
            return data;
        }
        public static long  DeleteData(long Id)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", Id));
                    SqlParameter[] parameters = parameterList.ToArray();

                    db.DbContext.Database.SqlQuery<long>(@"exec [dbo].[sp_SubConCompanyDelete]@Id", parameters).FirstOrDefault();
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
