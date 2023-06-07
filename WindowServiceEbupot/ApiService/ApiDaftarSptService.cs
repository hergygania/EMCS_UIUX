using WindowsServiceEbupot.Services;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WindowsServiceEbupot.Services.ExceptionHandler;

namespace WindowsServiceEbupot.ApiStaging
{
    public class ApiBupotService<TModel> : IApiService<TModel> where TModel : class
    {
        private string _urlApi = "";
        protected readonly HttpClient _client;

        public ApiBupotService(HttpClient client, string urlApi)
        {
            client.BaseAddress = new Uri(ApiConfig.BASE_URL);

            _urlApi = urlApi;
            _client = client;
        }

        public async Task<PaginatedResponse<TModel>> GetAsync(PaginationParam paginationParam)
        {
            string param = JsonConvert.SerializeObject(paginationParam);

            HttpContent httpContent = new StringContent(param, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync($"{_urlApi}", httpContent);

            if (!response.IsSuccessStatusCode)
            {
                var responseException = new ResponseException(response);

                return ProtocolResponse.FromException<PaginatedResponse<TModel>>(responseException.Exception);
            }

            return await ProtocolResponse.FromHttpResponseAsync<PaginatedResponse<TModel>>(response).ConfigureAwait(true);
        }

        public Task<RequestResponse<TModel>> GetAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<RequestResponse<TModel>> PostAsync(TModel model)
        {
            throw new NotImplementedException();
        }

        public Task<RequestResponse<TModel>> PutAsync(long id, TModel model)
        {
            throw new NotImplementedException();
        }
    }
}
