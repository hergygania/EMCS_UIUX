using System.Net.Http;
using WindowsServiceEbupot.Model;

namespace WindowsServiceEbupot.ApiStaging
{
    public class DaftarSptService : ApiBupotService<DaftarSptModel>
    {
        private static readonly string _urlApi = ApiConfig.EndpointDaftarSpt;
        public DaftarSptService(HttpClient client) : base(client, _urlApi) { }
    }
}
