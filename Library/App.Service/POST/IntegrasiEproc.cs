using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using System.Data;
using System.Configuration;
using System.Text;

namespace App.Service.POST
{
    public static class IntegrasiEproc
    {
        public const string CacheName = "App.IntegrasiEproc";
        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        readonly static string Baseurl = ConfigurationManager.AppSettings["post.eproc.url"];
        //readonly static string Baseurl = "http://10.2.14.174:8081/";

        public static async Task<bool> CheckValidationEproc(string token)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpContent payload = new StringContent("{\"Token\":\"" + token + "\"}", Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Clear();

                    HttpResponseMessage Res = await client.PostAsync("api/User/validasiPost", payload);
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result == "true" || result == "True")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
            }
            catch (System.Exception err)
            {
                return true;
            }
        }

        public static async Task<string> GetUserIdPis(string xupj = "")
        {
            using (var db = new Data.EmcsContext())
            {
                var first_name = "";
                var sql = @"SELECT TOP 1 [Employee_ID],[Employee_Name],[Email],[AD_User],[SAP_User_ID] FROM[EMCS].[dbo].[Employee] where AD_User = '" + xupj + "'";
                var data = db.Database.SqlQuery<Employee>(sql).FirstOrDefault();

                if (data != null)
                {
                    string phrase = data.Email;
                    string[] words = phrase.Split('@');
                    first_name = words[0];
                    return first_name;
                }
                return "";
            }
        }
    }
}
