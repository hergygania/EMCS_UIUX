using App.Data.Domain.EMCS;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Service.EMCS
{
    public class SVCShippingSummary
    {
        public static dynamic SSList(GridListFilter crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    string Term = "";
                    string Order = "";
                    crit.Sort = crit.Sort ?? "Id";
                    db.Database.CommandTimeout = 600;

                    if (crit.Term != null)
                    {
                        Term = Regex.Replace(crit.Term, @"[^0-9a-zA-Z]+", "");
                    }

                    if (crit.Order != null)
                    {
                        Order = Regex.Replace(crit.Order, @"[^0-9a-zA-Z]+", "");
                    }


                    var sql = @"[dbo].[sp_get_shippingsummary_list] @Username='" + SiteConfiguration.UserName + "', @Search = '" + crit.Term + "' ";
                    var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").ToList();
                    
                    var data = db.Database.SqlQuery<SPShippingSummary>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                    


                        dynamic result = new ExpandoObject();
                    if (count != null) result.total = Convert.ToInt32(count.Count);
                    result.rows = data;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
