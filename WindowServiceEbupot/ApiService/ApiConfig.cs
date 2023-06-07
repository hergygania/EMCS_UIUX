namespace WindowServiceEbupot.ApiService
{
    public class ApiConfig
    {
        public static readonly string BASE_URL = "http://10.2.2.71/";
        public static readonly string EndpointLogin = "oauth/token";
        public static readonly string EndpointLogout = "api/logout";
        public static readonly string EndpointReportPisah = "api/ebupot/bp23/report/pisah";
        public static readonly string EndpointDaftarSpt = "api/ebupot/espt23";
        public static readonly string EndpointBupotDetail = "api/ebupot/bp23";
        public static readonly string EndpointBupotDetail26 = "api/ebupot/bp26";
    }
}
