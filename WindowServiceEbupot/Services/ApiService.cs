using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WindowsServiceEbupot.Services.ExceptionHandler;

namespace WindowsServiceEbupot.Services
{
    public class ApiService<TModel> : IApiService<TModel> where TModel : class
    {
        private static ClaimsIdentity identity => (ClaimsIdentity)HttpContext.Current.User.Identity;

        private static IEnumerable<Claim> claims => identity.Claims;

        public static string Token
        {
            get
            {
                try
                {
                    return claims.Where(w => w.Type == "access_token").FirstOrDefault()?.Value;
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        private string _urlApi = "";
        protected readonly HttpClient _client;

        public ApiService(string urlApi)
        {
            _client = new HttpClient();
            _client.SetToken("Bearer", Token);

            _urlApi = urlApi;
        }

        public async Task<RequestResponse<TModel>> GetAsync(long id)
        {
            var response = await _client.GetAsync($"{_urlApi}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var responseException = new ResponseException(response);

                return ProtocolResponse.FromException<RequestResponse<TModel>>(responseException.Exception);
            }

            return await ProtocolResponse.FromHttpResponseAsync<RequestResponse<TModel>>(response).ConfigureAwait(true);
        }

        public async Task<PaginatedResponse<TModel>> GetAsync(PaginationParam paginationParam)
        {
            string param = JsonConvert.SerializeObject(paginationParam);

            HttpContent httpContent = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_urlApi}/pageable", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var responseException = new ResponseException(response);

                return ProtocolResponse.FromException<PaginatedResponse<TModel>>(responseException.Exception);
            }

            return await ProtocolResponse.FromHttpResponseAsync<PaginatedResponse<TModel>>(response).ConfigureAwait(true);
        }

        public async Task<RequestResponse<TModel>> PostAsync(TModel model)
        {
            string param = JsonConvert.SerializeObject(model);

            HttpContent httpContent = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_urlApi, httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var responseException = new ResponseException(response);

                return ProtocolResponse.FromException<RequestResponse<TModel>>(responseException.Exception);
            }

            return await ProtocolResponse.FromHttpResponseAsync<RequestResponse<TModel>>(response).ConfigureAwait(true);
        }

        public async Task<RequestResponse<TModel>> PutAsync(long id, TModel model)
        {
            string param = JsonConvert.SerializeObject(model);

            HttpContent httpContent = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"{_urlApi}/{id}", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var responseException = new ResponseException(response);

                return ProtocolResponse.FromException<RequestResponse<TModel>>(responseException.Exception);
            }

            return await ProtocolResponse.FromHttpResponseAsync<RequestResponse<TModel>>(response).ConfigureAwait(true);
        }
    }
}
