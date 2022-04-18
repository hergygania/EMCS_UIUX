using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Domain;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcContainer
    {
        public const string CacheName = "App.EMCS.SvcContainer";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic ContainerList(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "Id";
                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[sp_get_cargo_list] @Search = '" + crit.Term + "' ";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpCargoList>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static Data.Domain.EMCS.CargoContainer GetDataById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CargoContainers.Where(a => a.Id == id).FirstOrDefault();
                return data;

            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        public static Data.Domain.EMCS.CargoItem GetDataItemById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.CargoItemData.Where(a => a.Id == id).FirstOrDefault();
                return data;

            }
        }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

        public static long Insert(Data.Domain.EMCS.CargoContainer item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", item.Id.ToString()));
                parameterList.Add(new SqlParameter("@CargoId", item.CargoId.ToString()));
                parameterList.Add(new SqlParameter("@Number", item.Number ?? ""));
                parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                parameterList.Add(new SqlParameter("@ContainerType", item.ContainerType ?? ""));
                parameterList.Add(new SqlParameter("@SealNumber", item.SealNumber ?? ""));
                parameterList.Add(new SqlParameter("@ActionBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@IsDelete", "0"));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(" exec [dbo].[sp_insert_update_container] @Id, @CargoId, @Number, @Description, @ContainerType, @SealNumber, @ActionBy, @IsDelete", parameters).FirstOrDefault();
                if (data != null) return data.Id;
            }

            return 0;
        }

        public static long Update(Data.Domain.EMCS.CargoContainer item, string dml)
        {
            using (new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return 0;
            }
        }

        public static long Delete(long id)
        {
            using (new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return 0;
            }
        }

    }
}
