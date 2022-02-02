using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowServiceEbupot.ApiService;
using WindowServiceEbupot.Model;
using WindowsServiceEbupot.Model;
using WindowsServiceEbupot.Services;

namespace WindowServiceEbupot
{
    class Program
    {
        static List<BupotSpt23> ApiDaftarSptResult;
        static List<BupotSpt23Detail> ApiBupotDetailResult;
        static List<BupotSpt23Ref> ApiBupotRefResult;
        static List<BupotOrganizationDetail> ApiOrganizationResult;
        static List<BupotSpt23> DbDaftarSptResult;
        static List<BupotSpt23Detail> DbBupotDetailResult;
        static List<BupotSpt23Ref> DbBupotRefResult;
        static List<BupotOrganizationDetail> DbOrganizationResult;
        static List<TrAttachment> DbAttachment;
        static List<UploadFileModel> UploadFileModels;
        static string[] ID23;
        static string[] ID26;
        static string AuthorizeToken;
        static string connString = ConfigurationManager.AppSettings["connectionString"];
        static string BupotType23 = "Bupot23";
        static string BupotType26 = "Bupot26";
        static int RetryAttempt = 0;
        static bool IsGetSuccess = true;
        static bool IsSessionExpired = false;

        static void Main(string[] args)
        {
            Init();
        }

        static async Task Init()
        {
            ApiDaftarSptResult = new List<BupotSpt23>();
            ApiBupotDetailResult = new List<BupotSpt23Detail>();
            ApiBupotRefResult = new List<BupotSpt23Ref>();
            ApiOrganizationResult = new List<BupotOrganizationDetail>();
            DbDaftarSptResult = new List<BupotSpt23>();
            DbBupotDetailResult = new List<BupotSpt23Detail>();
            DbBupotRefResult = new List<BupotSpt23Ref>();
            DbOrganizationResult = new List<BupotOrganizationDetail>();
            DbAttachment = new List<TrAttachment>();
            UploadFileModels = new List<UploadFileModel>();
            ID23 = new string[] { };
            ID26 = new string[] { };
            await GetAuthorizeToken();
        }

        static async Task GetAuthorizeToken()
        {
            // Posting.  
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["BASE_URL"]);
                var request = new HttpRequestMessage(HttpMethod.Post, ApiConfig.EndpointLogin);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "YWJiZWI4OWEtMmMwMC00YzE3LWIyNzYtMDA4ODAwZWQyYmU3OjQzZDQyYWMxLTk3NWEtNGE4OC1iM2Q0LTE3NWYwMWQ5YTYzMw==");


                var formData = new List<KeyValuePair<string, string>>();
                formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                formData.Add(new KeyValuePair<string, string>("username", ConfigurationManager.AppSettings["username"]));
                formData.Add(new KeyValuePair<string, string>("password", ConfigurationManager.AppSettings["password"]));

                request.Content = new FormUrlEncodedContent(formData);
                var response = await client.SendAsync(request);

                Console.WriteLine("Getting Token Access...");
                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    var resData = await ProtocolResponse.FromHttpResponseAsync<RequestResponse<LoginResult>>(response).ConfigureAwait(true);

                    SqlConnection MyConn = new SqlConnection(connString);
                    try
                    {
                        MyConn.Open();
                        var insertQuery = "INSERT INTO dbo.BupotLogin(access_token,token_type,refresh_token,expires_in,scope,apps) " +
                            "VALUES(" +
                        ConvertToParam(resData.Model.access_token) + "," +
                        ConvertToParam(resData.Model.token_type) + "," +
                        ConvertToParam(resData.Model.refresh_token) + "," +
                        ConvertToParam(resData.Model.expires_in) + "," +
                        ConvertToParam(resData.Model.scope) + "," +
                        ConvertToParam(resData.Model.apps) +
                        ")";

                        SqlCommand cmd = new SqlCommand(insertQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Access Granted");
                        CreateLog("Login", "", "Success");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Access to DB failed");
                        CreateLog("Login", ex.Message, "Failed");
                        await LogoutApi();
                    }
                    MyConn.Close();
                    AuthorizeToken = resData.Model.access_token;
                    RetryAttempt = 5; //First login success
                    IsSessionExpired = false;
                    await GetDaftarSpt();
                }
                else
                {
                    if (RetryAttempt < 5)
                    {
                        Console.WriteLine("Access Failed");
                        Console.WriteLine("Retry to connect in 5 minutes...");
                        await Task.Delay(Convert.ToInt32(300000));
                        RetryAttempt++;
                        await GetAuthorizeToken();
                    }
                    else
                    {
                        await ErrorLog("Login", response);
                    }
                }
            }
        }

        static async Task GetDaftarSpt()
        {
            using (var client = new HttpClient())
            {
                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizeToken);

                // Setting Base address.  
                client.BaseAddress = new Uri(ApiConfig.BASE_URL);

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                #region filter
                var filter = "";
                var dariMasa = ConfigurationManager.AppSettings["DariMasa"];
                var keMasa = ConfigurationManager.AppSettings["KeMasa"];
                var dariTahun = ConfigurationManager.AppSettings["DariTahun"];
                var keTahun = ConfigurationManager.AppSettings["KeTahun"];
                var terakhirUpdateDariBulan = Int32.Parse(ConfigurationManager.AppSettings["TerakhirUpdateDariBulan"]);
                var terakhirUpdateDariTahun = Int32.Parse(ConfigurationManager.AppSettings["TerakhirUpdateDariTahun"]);
                var terakhirUpdate = new DateTime(terakhirUpdateDariTahun, terakhirUpdateDariBulan, 1);

                //if (!string.IsNullOrEmpty(dariMasa))
                //    filter += "&masa.greaterOrEqualThan=" + dariMasa;
                //if (!string.IsNullOrEmpty(keMasa))
                //    filter += "&masa.lessOrEqualThan=" + keMasa;
                //if (!string.IsNullOrEmpty(dariTahun))
                //    filter += "&tahun.greaterOrEqualThan=" + dariTahun;
                //if (!string.IsNullOrEmpty(keTahun))
                //    filter += "&tahun.lessOrEqualThan=" + keTahun;
                #endregion

                // HTTP GET  
                response = await client.GetAsync($"{ApiConfig.EndpointDaftarSpt}?size=1000{filter}").ConfigureAwait(true);

                Console.WriteLine("Fetching the data from API...");
                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    CreateLog("Api Daftar SPT", "", "Success");
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<DaftarSptModel>>(content);
                        result = result.Where(o => (o.createdDate.HasValue && o.createdDate.Value >= terakhirUpdate.Date) || (o.lastModifiedDate.HasValue && o.lastModifiedDate.Value >= terakhirUpdate.Date)).ToList();
                        ApiDaftarSptResult = result.Select(o => new BupotSpt23
                        {
                            BupotSpt23ID = o.id,
                            masa = o.masa,
                            tahun = o.tahun,
                            pembetulan = o.pembetulan,
                            sebelumnya = o.sebelumnya,
                            organizationID = o.organization.id,
                            status = o.status,
                            message = o.message,
                            createdBy = o.createdBy,
                            createdDate = o.createdDate,
                            lastModifiedBy = o.lastModifiedBy,
                            lastModifiedDate = o.lastModifiedDate,
                            currentState = o.currentState,
                            flowStateAccess = o.flowStateAccess,
                            flowState = o.flowState,
                        }).ToList();

                        foreach (var res in result.ToList())
                        {
                            BupotOrganizationDetail org = new BupotOrganizationDetail();
                            org.organizationID = res.organization.id;
                            org.npwp = res.organization.npwp;
                            org.nama = res.organization.nama;
                            org.alamat = res.organization.alamat;
                            org.kota = res.organization.kota;
                            org.kodePos = res.organization.kodePos;
                            org.noTelp = res.organization.noTelp;
                            org.email = res.organization.email;
                            org.active = res.organization.active;
                            org.certExists = res.organization.certExists;
                            org.password = res.organization.password;
                            ApiOrganizationResult.Add(org);
                        }

                        Console.WriteLine("Fetching data bupot...");
                        foreach (var res in result)
                        {
                            await GetBupotDetail(res.id);
                        }
                        if (IsGetSuccess)
                            CreateLog("Api Bupot", "", "Success");
                        IsGetSuccess = true;

                        Console.WriteLine("Fetch data success");
                        await CRUD();
                        await GetReportPisah();
                        await UploadToPortal(UploadFileModels);
                        await LogoutApi();
                    }
                    catch (Exception ex)
                    {
                        CreateLog("Api Daftar SPT", ex.Message, "Failed");
                        await LogoutApi();
                    }
                }
                else
                {
                    await ErrorLog("Api Daftar SPT", response);
                    if (IsSessionExpired)
                        await GetAuthorizeToken();
                    else
                        await LogoutApi();
                }
            }
        }

        static async Task GetBupotDetail(string sptID)
        {
            // HTTP GET.  
            using (var client = new HttpClient())
            {
                // Setting Authorization.  
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizeToken);

                // Setting Base address.  
                client.BaseAddress = new Uri(ApiConfig.BASE_URL);

                // Initialization.  
                HttpResponseMessage response = new HttpResponseMessage();

                //Bupot 23
                response = await client.GetAsync($"{ApiConfig.EndpointBupotDetail}/{sptID}/espt23?size=1000").ConfigureAwait(true);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<BupotDetailModel>>(content);

                        var terakhirUpdateDariBulan = Int32.Parse(ConfigurationManager.AppSettings["TerakhirUpdateDariBulan"]);
                        var terakhirUpdateDariTahun = Int32.Parse(ConfigurationManager.AppSettings["TerakhirUpdateDariTahun"]);
                        var terakhirUpdate = new DateTime(terakhirUpdateDariTahun, terakhirUpdateDariBulan, 1);

                        result = result.Where(o => (o.createdDate.HasValue && o.createdDate.Value >= terakhirUpdate.Date) || (o.lastModifiedDate.HasValue && o.lastModifiedDate.Value >= terakhirUpdate.Date)).ToList();

                        foreach (var res in result)
                        {
                            BupotSpt23Detail detail = new BupotSpt23Detail();
                            detail.BupotSpt23DetailID = res.id;
                            detail.seq = res.seq;
                            detail.row = res.row;
                            detail.rev = res.rev;
                            detail.no = res.no;
                            detail.referensi = res.referensi;
                            detail.tgl = res.tgl;
                            detail.npwpPenandatangan = res.npwpPenandatangan;
                            detail.namaPenandatangan = res.namaPenandatangan;
                            detail.signAs = res.signAs;
                            detail.status = res.status;
                            detail.fasilitas = res.fasilitas;
                            detail.noSkb = res.noSkb;
                            detail.tglSkb = res.tglSkb;
                            detail.noDtp = res.noDtp;
                            detail.ntpn = res.ntpn;
                            detail.BupotSpt23ID = res.spt.id;
                            detail.kode = res.kode;
                            detail.tarif = res.tarif;
                            detail.bruto = res.bruto;
                            detail.pph = res.pph;
                            detail.npwp = res.npwp;
                            detail.nik = res.nik;
                            detail.identity = res.identity;
                            detail.nama = res.nama;
                            detail.alamat = res.alamat;
                            detail.kelurahan = res.kelurahan;
                            detail.kecamatan = res.kecamatan;
                            detail.kabupaten = res.kabupaten;
                            detail.provinsi = res.provinsi;
                            detail.kodePos = res.kodePos;
                            detail.email = res.email;
                            detail.noTelp = res.noTelp;
                            detail.message = res.message;
                            detail.refLogFileId = res.refLogFileId;
                            detail.refXmlId = res.refXmlId;
                            detail.refIdBefore = res.refIdBefore;
                            detail.idBupotDjp = res.idBupotDjp;
                            detail.report = res.report;
                            detail.userId = res.userId;
                            detail.type = BupotType23;
                            detail.createdBy = res.createdBy;
                            detail.createdDate = res.createdDate;
                            detail.lastModifiedBy = res.lastModifiedBy;
                            detail.lastModifiedDate = res.lastModifiedDate;
                            ApiBupotDetailResult.Add(detail);

                            foreach (var item in res.refs)
                            {
                                BupotSpt23Ref bupotRef = new BupotSpt23Ref();
                                bupotRef.refsID = item.id;
                                bupotRef.BupotSpt23DetailID = res.id;
                                bupotRef.jenis = item.jenis;
                                bupotRef.noDok = item.noDok;
                                bupotRef.tgl = item.tgl;
                                bupotRef.bp23 = item.bp23;
                                bupotRef.bp26 = item.bp26;
                                bupotRef.createdBy = item.createdBy;
                                bupotRef.createdDate = item.createdDate;
                                bupotRef.lastModifiedBy = item.lastModifiedBy;
                                bupotRef.lastModifiedDate = item.lastModifiedDate;
                                ApiBupotRefResult.Add(bupotRef);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        IsGetSuccess = false;
                        CreateLog("Api Bupot 23", ex.Message, "Failed");
                        await LogoutApi();
                    }
                }
                else
                {
                    IsGetSuccess = false;
                    await ErrorLog("Api Bupot 23", response);
                }

                //Bupot 26
                response = await client.GetAsync($"{ApiConfig.EndpointBupotDetail26}/{sptID}/espt23?size=1000").ConfigureAwait(true);

                // Verification  
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<List<BupotDetailModel>>(content);

                        foreach (var res in result)
                        {
                            BupotSpt23Detail detail = new BupotSpt23Detail();
                            detail.BupotSpt23DetailID = res.id;
                            detail.seq = res.seq;
                            detail.row = res.row;
                            detail.rev = res.rev;
                            detail.no = res.no;
                            detail.referensi = res.referensi;
                            detail.tgl = res.tgl;
                            detail.npwpPenandatangan = res.npwpPenandatangan;
                            detail.namaPenandatangan = res.namaPenandatangan;
                            detail.signAs = res.signAs;
                            detail.status = res.status;
                            detail.fasilitas = res.fasilitas;
                            detail.noSkb = res.noSkb;
                            detail.tglSkb = res.tglSkb;
                            detail.tarifSkd = res.tarifSkd;
                            detail.noSkd = res.noSkd;
                            detail.noDtp = res.noDtp;
                            detail.ntpn = res.ntpn;
                            detail.tin = res.tin;
                            detail.BupotSpt23ID = res.spt.id;
                            detail.kode = res.kode;
                            detail.netto = res.netto;
                            detail.tarif = res.tarif;
                            detail.bruto = res.bruto;
                            detail.pph = res.pph;
                            detail.npwp = res.npwp;
                            detail.nik = res.nik;
                            detail.identity = res.identity;
                            detail.nama = res.nama;
                            detail.alamat = res.alamat;
                            detail.kelurahan = res.kelurahan;
                            detail.kecamatan = res.kecamatan;
                            detail.kabupaten = res.kabupaten;
                            detail.provinsi = res.provinsi;
                            detail.kodePos = res.kodePos;
                            detail.email = res.email;
                            detail.noTelp = res.noTelp;
                            detail.dob = res.dob;
                            detail.negara = res.negara;
                            detail.noPassport = res.noPassport;
                            detail.noKitas = res.noKitas;
                            detail.message = res.message;
                            detail.refLogFileId = res.refLogFileId;
                            detail.refXmlId = res.refXmlId;
                            detail.refIdBefore = res.refIdBefore;
                            detail.idBupotDjp = res.idBupotDjp;
                            detail.report = res.report;
                            detail.userId = res.userId;
                            detail.type = BupotType26;
                            detail.createdBy = res.createdBy;
                            detail.createdDate = res.createdDate;
                            detail.lastModifiedBy = res.lastModifiedBy;
                            detail.lastModifiedDate = res.lastModifiedDate;
                            ApiBupotDetailResult.Add(detail);

                            foreach (var item in res.refs)
                            {
                                BupotSpt23Ref bupotRef = new BupotSpt23Ref();
                                bupotRef.refsID = item.id;
                                bupotRef.BupotSpt23DetailID = res.id;
                                bupotRef.jenis = item.jenis;
                                bupotRef.noDok = item.noDok;
                                bupotRef.tgl = item.tgl;
                                bupotRef.bp23 = item.bp23;
                                bupotRef.bp26 = item.bp26;
                                bupotRef.createdBy = item.createdBy;
                                bupotRef.createdDate = item.createdDate;
                                bupotRef.lastModifiedBy = item.lastModifiedBy;
                                bupotRef.lastModifiedDate = item.lastModifiedDate;
                                ApiBupotRefResult.Add(bupotRef);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        IsGetSuccess = false;
                        CreateLog("Api Bupot 26", ex.Message, "Failed");
                        await LogoutApi();
                    }
                }
                else
                {
                    IsGetSuccess = false;
                    await ErrorLog("Api Bupot 26", response);
                }
            }
        }

        static async Task GetReportPisah()
        {
            // Posting.  
            using (var client = new HttpClient())
            {
                var now = DateTime.Now;
                client.BaseAddress = new Uri(ApiConfig.BASE_URL);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizeToken);
                client.Timeout = TimeSpan.FromMinutes(30);
                Console.WriteLine("Getting files bupot 23");
                for (int i = 0; i < ID23.Length; i++)
                {
                    await GetReportFromApi(client, ApiConfig.EndpointBupotDetail, ID23[i]);
                }
                if (IsGetSuccess)
                    CreateLog("Get Bupot 23 Document", ID23.Length + " new document(s)", "Success");
                IsGetSuccess = true;

                Console.WriteLine("Getting files bupot 26");
                for (int i = 0; i < ID26.Length; i++)
                {
                    await GetReportFromApi(client, ApiConfig.EndpointBupotDetail26, ID26[i]);
                }
                if (IsGetSuccess)
                    CreateLog("Get Bupot 26 Document", ID26.Length + " new document(s)", "Success");
                IsGetSuccess = true;
            }
        }

        static async Task CRUD()
        {
            SqlConnection MyConn = new SqlConnection(connString);

            try
            {
                MyConn.Open();
                Console.WriteLine("Fetch data from DB...");
                var sqlQuery = "SELECT * FROM dbo.BupotSpt23";

                Console.WriteLine("Fetch SPT list from DB");
                using (SqlCommand command = new SqlCommand(sqlQuery, MyConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BupotSpt23 spt = new BupotSpt23();
                            spt.ID = ConvertObject(reader[0]);
                            spt.BupotSpt23ID = ConvertObject(reader[1]);
                            spt.masa = ConvertObject(reader[2]);
                            spt.tahun = ConvertObject(reader[3]);
                            spt.pembetulan = ConvertObject(reader[4]);
                            spt.sebelumnya = ConvertObject(reader[5]);
                            spt.organizationID = ConvertObject(reader[6]);
                            spt.status = ConvertObject(reader[7]);
                            spt.message = ConvertObject(reader[8]);
                            spt.createdBy = ConvertObject(reader[9]);
                            spt.createdDate = ConvertObject(reader[10]);
                            spt.lastModifiedBy = ConvertObject(reader[11]);
                            spt.lastModifiedDate = ConvertObject(reader[12]);
                            spt.currentState = ConvertObject(reader[13]);
                            spt.flowStateAccess = ConvertObject(reader[14]);
                            spt.flowState = ConvertObject(reader[15]);
                            DbDaftarSptResult.Add(spt);
                        }
                    }
                }

                Console.WriteLine("Fetch bupot detail from DB");
                var sqlQuery2 = "SELECT * FROM dbo.BupotSpt23Detail";
                using (SqlCommand command = new SqlCommand(sqlQuery2, MyConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BupotSpt23Detail spt = new BupotSpt23Detail();
                            spt.ID = ConvertObject(reader[0]);
                            spt.BupotSpt23DetailID = ConvertObject(reader[1]);
                            spt.seq = ConvertObject(reader[2]);
                            spt.row = ConvertObject(reader[3]);
                            spt.rev = ConvertObject(reader[4]);
                            spt.no = ConvertObject(reader[5]);
                            spt.referensi = ConvertObject(reader[6]);
                            spt.tgl = ConvertObject(reader[7]);
                            spt.npwpPenandatangan = ConvertObject(reader[8]);
                            spt.namaPenandatangan = ConvertObject(reader[9]);
                            spt.signAs = ConvertObject(reader[10]);
                            spt.status = ConvertObject(reader[11]);
                            spt.fasilitas = ConvertObject(reader[12]);
                            spt.noSkb = ConvertObject(reader[13]);
                            spt.tglSkb = ConvertObject(reader[14]);
                            spt.tarifSkd = ConvertObject(reader[15]);
                            spt.noSkd = ConvertObject(reader[16]);
                            spt.noDtp = ConvertObject(reader[17]);
                            spt.ntpn = ConvertObject(reader[18]);
                            spt.tin = ConvertObject(reader[19]);
                            spt.BupotSpt23ID = ConvertObject(reader[20]);
                            spt.kode = ConvertObject(reader[21]);
                            spt.netto = ConvertObject(reader[22]);
                            spt.tarif = ConvertObject(reader[23]);
                            spt.bruto = ConvertObject(reader[24]);
                            spt.pph = ConvertObject(reader[25]);
                            spt.npwp = ConvertObject(reader[26]);
                            spt.nik = ConvertObject(reader[27]);
                            spt.identity = ConvertObject(reader[28]);
                            spt.nama = ConvertObject(reader[29]);
                            spt.alamat = ConvertObject(reader[30]);
                            spt.kelurahan = ConvertObject(reader[31]);
                            spt.kecamatan = ConvertObject(reader[32]);
                            spt.kabupaten = ConvertObject(reader[33]);
                            spt.provinsi = ConvertObject(reader[34]);
                            spt.kodePos = ConvertObject(reader[35]);
                            spt.email = ConvertObject(reader[36]);
                            spt.noTelp = ConvertObject(reader[37]);
                            spt.dob = ConvertObject(reader[38]);
                            spt.negara = ConvertObject(reader[39]);
                            spt.noPassport = ConvertObject(reader[40]);
                            spt.noKitas = ConvertObject(reader[41]);
                            spt.message = ConvertObject(reader[42]);
                            spt.refLogFileId = ConvertObject(reader[43]);
                            spt.refXmlId = ConvertObject(reader[44]);
                            spt.refIdBefore = ConvertObject(reader[45]);
                            spt.idBupotDjp = ConvertObject(reader[46]);
                            spt.report = ConvertObject(reader[47]);
                            spt.userId = ConvertObject(reader[48]);
                            spt.type = ConvertObject(reader[49]);
                            spt.createdBy = ConvertObject(reader[50]);
                            spt.createdDate = ConvertObject(reader[51]);
                            spt.lastModifiedBy = ConvertObject(reader[52]);
                            spt.lastModifiedDate = ConvertObject(reader[53]);
                            DbBupotDetailResult.Add(spt);
                        }
                    }
                }

                Console.WriteLine("Fetch organization from DB");
                var sqlQuery3 = "SELECT * FROM dbo.BupotOrganizationDetail";
                using (SqlCommand command = new SqlCommand(sqlQuery3, MyConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BupotOrganizationDetail spt = new BupotOrganizationDetail();
                            spt.ID = ConvertObject(reader[0]);
                            spt.organizationID = ConvertObject(reader[1]);
                            spt.npwp = ConvertObject(reader[2]);
                            spt.nama = ConvertObject(reader[3]);
                            spt.alamat = ConvertObject(reader[4]);
                            spt.kota = ConvertObject(reader[5]);
                            spt.kodePos = ConvertObject(reader[6]);
                            spt.noTelp = ConvertObject(reader[7]);
                            spt.email = ConvertObject(reader[8]);
                            spt.active = ConvertObject(reader[9]);
                            spt.certExists = ConvertObject(reader[10]);
                            spt.password = ConvertObject(reader[11]);
                            DbOrganizationResult.Add(spt);
                        }
                    }
                }

                Console.WriteLine("Fetch Bupot Ref from DB");
                var sqlQuery4 = "SELECT * FROM dbo.BupotSpt23Ref";
                using (SqlCommand command = new SqlCommand(sqlQuery4, MyConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BupotSpt23Ref spt = new BupotSpt23Ref();
                            spt.ID = ConvertObject(reader[0]);
                            spt.refsID = ConvertObject(reader[1]);
                            spt.BupotSpt23DetailID = ConvertObject(reader[2]);
                            spt.jenis = ConvertObject(reader[3]);
                            spt.noDok = ConvertObject(reader[4]);
                            spt.tgl = ConvertObject(reader[5]);
                            spt.bp23 = ConvertObject(reader[6]);
                            spt.bp26 = ConvertObject(reader[7]);
                            spt.createdBy = ConvertObject(reader[8]);
                            spt.createdDate = ConvertObject(reader[9]);
                            spt.lastModifiedBy = ConvertObject(reader[10]);
                            spt.lastModifiedDate = ConvertObject(reader[11]);
                            DbBupotRefResult.Add(spt);
                        }
                    }
                }

                Console.WriteLine("Fetch Attachment from DB");
                var sqlQuery5 = "SELECT * FROM dbo.TrAttachment";
                using (SqlCommand command = new SqlCommand(sqlQuery5, MyConn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TrAttachment spt = new TrAttachment();
                            spt.ID = ConvertObject(reader[0]);
                            spt.Code = ConvertObject(reader[1]);
                            spt.FileName = ConvertObject(reader[2]);
                            spt.FileNameOri = ConvertObject(reader[3]);
                            spt.CodeAttachment = ConvertObject(reader[4]);
                            spt.Path = ConvertObject(reader[5]);
                            spt.IsActive = ConvertObject(reader[6]);
                            spt.UploadedDate = ConvertObject(reader[7]);
                            spt.UploadedBy = ConvertObject(reader[8]);
                            DbAttachment.Add(spt);
                        }
                    }
                }

                Console.WriteLine("Comparing DB to API");
                //COMPARE
                var sptExistID = new HashSet<string>(DbDaftarSptResult.Select(o => o.BupotSpt23ID));
                var bupotDetailExistID = new HashSet<string>(DbBupotDetailResult.Select(o => o.BupotSpt23DetailID));
                var organizationExistID = new HashSet<string>(DbOrganizationResult.Select(o => o.organizationID));
                var bupotRefExistID = new HashSet<string>(DbBupotRefResult.Select(o => o.refsID));

                var dataSptExist = ApiDaftarSptResult.Where(p => sptExistID.Contains(p.BupotSpt23ID)).GroupBy(o => o.BupotSpt23ID).Select(grp => grp.First()).ToList();
                var dataBupotDetailExist = ApiBupotDetailResult.Where(p => bupotDetailExistID.Contains(p.BupotSpt23DetailID)).GroupBy(o => o.BupotSpt23DetailID).Select(grp => grp.First()).ToList();
                var dataOrganizationExist = ApiOrganizationResult.Where(p => organizationExistID.Contains(p.organizationID)).GroupBy(o => o.organizationID).Select(grp => grp.First()).ToList();
                var dataBupotRefExist = ApiBupotRefResult.Where(p => bupotRefExistID.Contains(p.refsID)).GroupBy(o => o.refsID).Select(grp => grp.First()).ToList();

                List<BupotSpt23> updatedSpt = new List<BupotSpt23>();
                List<BupotSpt23Detail> updatedBupotDetail = new List<BupotSpt23Detail>();
                List<BupotSpt23Ref> updatedBupotRef = new List<BupotSpt23Ref>();
                foreach (var item in dataSptExist)
                {
                    var item1 = DbDaftarSptResult.FirstOrDefault(o => o.BupotSpt23ID == item.BupotSpt23ID);
                    var item2 = ApiDaftarSptResult.FirstOrDefault(o => o.BupotSpt23ID == item.BupotSpt23ID);
                    if (item1.lastModifiedDate != item2.lastModifiedDate)
                        updatedSpt.Add(item2);
                }
                foreach (var item in dataBupotDetailExist)
                {
                    var item1 = DbBupotDetailResult.FirstOrDefault(o => o.BupotSpt23DetailID == item.BupotSpt23DetailID);
                    var item2 = ApiBupotDetailResult.FirstOrDefault(o => o.BupotSpt23DetailID == item.BupotSpt23DetailID);
                    if (item1.lastModifiedDate != item2.lastModifiedDate)
                    {
                        updatedBupotDetail.Add(item2);

                        if (item.type == BupotType23)
                            ID23 = ID23.Concat(new string[] { item2.BupotSpt23DetailID }).ToArray();
                        else
                            ID26 = ID26.Concat(new string[] { item2.BupotSpt23DetailID }).ToArray();
                    }
                }
                foreach (var item in dataBupotRefExist)
                {
                    var item1 = DbBupotRefResult.FirstOrDefault(o => o.refsID == item.refsID);
                    var item2 = ApiBupotRefResult.FirstOrDefault(o => o.refsID == item.refsID);
                    if (item1.lastModifiedDate != item2.lastModifiedDate)
                        updatedBupotRef.Add(item2);
                }

                var dataSptNotExist = ApiDaftarSptResult.Where(p => !sptExistID.Contains(p.BupotSpt23ID)).GroupBy(o => o.BupotSpt23ID).Select(grp => grp.First()).ToList();
                var dataBupotDetailNotExist = ApiBupotDetailResult.Where(p => !bupotDetailExistID.Contains(p.BupotSpt23DetailID)).GroupBy(o => o.BupotSpt23DetailID).Select(grp => grp.First()).ToList();
                var dataOrganizationNotExist = ApiOrganizationResult.Where(p => !organizationExistID.Contains(p.organizationID)).GroupBy(o => o.organizationID).Select(grp => grp.First()).ToList();
                var dataBupotRefNotExist = ApiBupotRefResult.Where(p => !bupotRefExistID.Contains(p.refsID)).GroupBy(o => o.refsID).Select(grp => grp.First()).ToList();

                foreach (var item in dataBupotDetailNotExist)
                {
                    if (item.type == BupotType23)
                        ID23 = ID23.Concat(new string[] { item.BupotSpt23DetailID }).ToArray();
                    else
                        ID26 = ID26.Concat(new string[] { item.BupotSpt23DetailID }).ToArray();
                }

                var attachmentExistID = new HashSet<string>(DbAttachment.Select(o => o.Code));
                var attachmentNotExist = ApiBupotDetailResult.Where(p => !attachmentExistID.Contains(p.BupotSpt23DetailID)).GroupBy(o => o.BupotSpt23DetailID).Select(grp => grp.First()).ToList();
                foreach (var item in attachmentNotExist)
                {
                    if (item.type == BupotType23)
                    {
                        if (!ID23.Contains(item.BupotSpt23DetailID))
                            ID23 = ID23.Concat(new string[] { item.BupotSpt23DetailID }).ToArray();
                    }
                    else
                    {
                        if (!ID26.Contains(item.BupotSpt23DetailID))
                            ID26 = ID26.Concat(new string[] { item.BupotSpt23DetailID }).ToArray();
                    }
                }

                #region CRUD
                //INSERT
                Console.WriteLine("Inserting new SPT...");
                var isSuccess = true;
                foreach (var data in dataSptNotExist)
                {
                    try
                    {
                        var insertQuery = "INSERT INTO dbo.BupotSpt23(BupotSpt23ID,masa,tahun,pembetulan,sebelumnya,organizationID,status,message,createdBy,createdDate,lastModifiedBy,lastModifiedDate,currentState,flowStateAccess,flowState) " +
                            "VALUES(" +
                            ConvertToParam(data.BupotSpt23ID) + "," +
                            ConvertToParam(data.masa) + "," +
                            ConvertToParam(data.tahun) + "," +
                            ConvertToParam(data.pembetulan) + "," +
                            ConvertToParam(data.sebelumnya) + "," +
                            ConvertToParam(data.organizationID) + "," +
                            ConvertToParam(data.status) + "," +
                            ConvertToParam(data.message) + "," +
                            ConvertToParam(data.createdBy) + "," +
                            ConvertToParam(data.createdDate) + "," +
                            ConvertToParam(data.lastModifiedBy) + "," +
                            ConvertToParam(data.lastModifiedDate) + "," +
                            ConvertToParam(data.currentState) + "," +
                            ConvertToParam(data.flowStateAccess) + "," +
                            ConvertToParam(data.flowState) +
                            ")";

                        SqlCommand cmd = new SqlCommand(insertQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Insert New SPT", ex.Message, "Success");
                    }
                }
                if (isSuccess)
                    CreateLog("Insert New SPT", dataSptNotExist.Count + " new record(s)", "Success");

                Console.WriteLine("Inserting new bupot detail...");
                isSuccess = true;
                foreach (var data in dataBupotDetailNotExist)
                {
                    try
                    {
                        var insertQuery = "INSERT INTO dbo.BupotSpt23Detail(BupotSpt23DetailID,seq,row,rev,no,referensi,tgl,npwpPenandatangan,namaPenandatangan,signAs,status," +
                                            "fasilitas,noSkb,tglSkb,tarifSkd,noSkd,noDtp,ntpn,tin,BupotSpt23ID,kode,netto,tarif,bruto,pph,npwp,nik,[identity],nama,alamat,kelurahan,kecamatan,kabupaten," +
                                            "provinsi,kodePos,email,noTelp,dob,negara,noPassport,noKitas,message,refLogFileId,refXmlId,refIdBefore,idBupotDjp,report,userId,[type],createdBy,createdDate,lastModifiedBy,lastModifiedDate) " +
                            "VALUES(" +
                            ConvertToParam(data.BupotSpt23DetailID) + "," +
                            ConvertToParam(data.seq) + "," +
                            ConvertToParam(data.row) + "," +
                            ConvertToParam(data.rev) + "," +
                            ConvertToParam(data.no) + "," +
                            ConvertToParam(data.referensi) + "," +
                            ConvertToParam(data.tgl) + "," +
                            ConvertToParam(data.npwpPenandatangan) + "," +
                            ConvertToParam(data.namaPenandatangan) + "," +
                            ConvertToParam(data.signAs) + "," +
                            ConvertToParam(data.status) + "," +
                            ConvertToParam(data.fasilitas) + "," +
                            ConvertToParam(data.noSkb) + "," +
                            ConvertToParam(data.tglSkb) + "," +
                            ConvertToParam(data.tarifSkd) + "," +
                            ConvertToParam(data.noSkd) + "," +
                            ConvertToParam(data.noDtp) + "," +
                            ConvertToParam(data.ntpn) + "," +
                            ConvertToParam(data.tin) + "," +
                            ConvertToParam(data.BupotSpt23ID) + "," +
                            ConvertToParam(data.kode) + "," +
                            ConvertToParam(data.netto) + "," +
                            ConvertToParam(data.tarif) + "," +
                            ConvertToParam(data.bruto) + "," +
                            ConvertToParam(data.pph) + "," +
                            ConvertToParam(data.npwp) + "," +
                            ConvertToParam(data.nik) + "," +
                            ConvertToParam(data.identity) + "," +
                            ConvertToParam(data.nama) + "," +
                            ConvertToParam(data.alamat) + "," +
                            ConvertToParam(data.kelurahan) + "," +
                            ConvertToParam(data.kecamatan) + "," +
                            ConvertToParam(data.kabupaten) + "," +
                            ConvertToParam(data.provinsi) + "," +
                            ConvertToParam(data.kodePos) + "," +
                            ConvertToParam(data.email) + "," +
                            ConvertToParam(data.noTelp) + "," +
                            ConvertToParam(data.dob) + "," +
                            ConvertToParam(data.negara) + "," +
                            ConvertToParam(data.noPassport) + "," +
                            ConvertToParam(data.noKitas) + "," +
                            ConvertToParam(data.message) + "," +
                            ConvertToParam(data.refLogFileId) + "," +
                            ConvertToParam(data.refXmlId) + "," +
                            ConvertToParam(data.refIdBefore) + "," +
                            ConvertToParam(data.idBupotDjp) + "," +
                            ConvertToParam(data.report) + "," +
                            ConvertToParam(data.userId) + "," +
                            ConvertToParam(data.type) + "," +
                            ConvertToParam(data.createdBy) + "," +
                            ConvertToParam(data.createdDate) + "," +
                            ConvertToParam(data.lastModifiedBy) + "," +
                            ConvertToParam(data.lastModifiedDate) +
                            ")";

                        SqlCommand cmd = new SqlCommand(insertQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Insert New Bupot", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Insert New Bupot", dataBupotDetailNotExist.Count + " new record(s)", "Success");

                Console.WriteLine("Inserting new organization...");
                isSuccess = true;
                foreach (var data in dataOrganizationNotExist)
                {
                    try
                    {
                        var insertQuery = "INSERT INTO dbo.BupotOrganizationDetail(organizationID,npwp,nama,alamat,kota,kodePos,noTelp,email,active,certExists,password) " +
                            "VALUES(" +
                            ConvertToParam(data.organizationID) + "," +
                            ConvertToParam(data.npwp) + "," +
                            ConvertToParam(data.nama) + "," +
                            ConvertToParam(data.alamat) + "," +
                            ConvertToParam(data.kota) + "," +
                            ConvertToParam(data.kodePos) + "," +
                            ConvertToParam(data.noTelp) + "," +
                            ConvertToParam(data.email) + "," +
                            ConvertToParam(data.active) + "," +
                            ConvertToParam(data.certExists) + "," +
                            ConvertToParam(data.password) +
                            ")";

                        SqlCommand cmd = new SqlCommand(insertQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Insert New Organization", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Insert New Organization", dataOrganizationNotExist.Count + " new record(s)", "Success");

                Console.WriteLine("Inserting new bupot ref");
                foreach (var data in dataBupotRefNotExist)
                {
                    try
                    {
                        var insertQuery = "INSERT INTO dbo.BupotSpt23Ref(refsID,BupotSpt23DetailID,jenis,noDok,tgl,bp23,bp26,createdBy,createdDate,lastModifiedBy,lastModifiedDate) " +
                            "VALUES(" +
                            ConvertToParam(data.refsID) + "," +
                            ConvertToParam(data.BupotSpt23DetailID) + "," +
                            ConvertToParam(data.jenis) + "," +
                            ConvertToParam(data.noDok) + "," +
                            ConvertToParam(data.tgl) + "," +
                            ConvertToParam(data.bp23) + "," +
                            ConvertToParam(data.bp26) + "," +
                            ConvertToParam(data.createdBy) + "," +
                            ConvertToParam(data.createdDate) + "," +
                            ConvertToParam(data.lastModifiedBy) + "," +
                            ConvertToParam(data.lastModifiedDate) +
                            ")";

                        SqlCommand cmd = new SqlCommand(insertQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Insert New Bupot Ref", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Insert New Bupot Ref", dataBupotRefNotExist.Count + " new record(s)", "Success");

                //UPDATE
                Console.WriteLine("Update existing SPT...");
                isSuccess = true;
                foreach (var data in updatedSpt)
                {
                    try
                    {
                        var updateQuery = "UPDATE dbo.BupotSpt23 SET " +
                            "masa=" + ConvertToParam(data.masa) +
                            ",tahun=" + ConvertToParam(data.tahun) +
                            ",pembetulan=" + ConvertToParam(data.pembetulan) +
                            ",sebelumnya=" + ConvertToParam(data.sebelumnya) +
                            ",organizationID=" + ConvertToParam(data.organizationID) +
                            ",status=" + ConvertToParam(data.status) +
                            ",message=" + ConvertToParam(data.message) +
                            ",createdBy=" + ConvertToParam(data.createdBy) +
                            ",createdDate=" + ConvertToParam(data.createdDate) +
                            ",lastModifiedBy=" + ConvertToParam(data.lastModifiedBy) +
                            ",lastModifiedDate=" + ConvertToParam(data.lastModifiedDate) +
                            ",currentState=" + ConvertToParam(data.currentState) +
                            ",flowStateAccess=" + ConvertToParam(data.flowStateAccess) +
                            ",flowState=" + ConvertToParam(data.flowState) +
                            " WHERE BupotSpt23ID=" + ConvertToParam(data.BupotSpt23ID);

                        SqlCommand cmd = new SqlCommand(updateQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Update Existing SPT", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Update Existing SPT", updatedSpt.Count + " record(s) updated", "Success");

                Console.WriteLine("Update existing bupot detail...");
                isSuccess = true;
                foreach (var data in updatedBupotDetail)
                {
                    try
                    {
                        var updateQuery = "UPDATE dbo.BupotSpt23Detail SET " +
                            "seq=" + ConvertToParam(data.seq) +
                            ",row=" + ConvertToParam(data.row) +
                            ",rev=" + ConvertToParam(data.rev) +
                            ",no=" + ConvertToParam(data.no) +
                            ",referensi=" + ConvertToParam(data.referensi) +
                            ",tgl=" + ConvertToParam(data.tgl) +
                            ",npwpPenandatangan=" + ConvertToParam(data.npwpPenandatangan) +
                            ",namaPenandatangan=" + ConvertToParam(data.namaPenandatangan) +
                            ",signAs=" + ConvertToParam(data.signAs) +
                            ",status=" + ConvertToParam(data.status) +
                            ",fasilitas=" + ConvertToParam(data.fasilitas) +
                            ",noSkb=" + ConvertToParam(data.noSkb) +
                            ",tglSkb=" + ConvertToParam(data.tglSkb) +
                            ",tarifSkd=" + ConvertToParam(data.tarifSkd) +
                            ",noSkd=" + ConvertToParam(data.noSkd) +
                            ",noDtp=" + ConvertToParam(data.noDtp) +
                            ",ntpn=" + ConvertToParam(data.ntpn) +
                            ",tin=" + ConvertToParam(data.tin) +
                            ",BupotSpt23ID=" + ConvertToParam(data.BupotSpt23ID) +
                            ",kode=" + ConvertToParam(data.kode) +
                            ",netto=" + ConvertToParam(data.netto) +
                            ",tarif=" + ConvertToParam(data.tarif) +
                            ",bruto=" + ConvertToParam(data.bruto) +
                            ",pph=" + ConvertToParam(data.pph) +
                            ",npwp=" + ConvertToParam(data.npwp) +
                            ",nik=" + ConvertToParam(data.nik) +
                            ",[identity]=" + ConvertToParam(data.identity) +
                            ",nama=" + ConvertToParam(data.nama) +
                            ",alamat=" + ConvertToParam(data.alamat) +
                            ",kelurahan=" + ConvertToParam(data.kelurahan) +
                            ",kecamatan=" + ConvertToParam(data.kecamatan) +
                            ",kabupaten=" + ConvertToParam(data.kabupaten) +
                            ",provinsi=" + ConvertToParam(data.provinsi) +
                            ",kodePos=" + ConvertToParam(data.kodePos) +
                            ",email=" + ConvertToParam(data.email) +
                            ",noTelp=" + ConvertToParam(data.noTelp) +
                            ",dob=" + ConvertToParam(data.dob) +
                            ",negara=" + ConvertToParam(data.negara) +
                            ",noPassport=" + ConvertToParam(data.noPassport) +
                            ",noKitas=" + ConvertToParam(data.noKitas) +
                            ",message=" + ConvertToParam(data.message) +
                            ",refLogFileId=" + ConvertToParam(data.refLogFileId) +
                            ",refXmlId=" + ConvertToParam(data.refXmlId) +
                            ",refIdBefore=" + ConvertToParam(data.refIdBefore) +
                            ",idBupotDjp=" + ConvertToParam(data.idBupotDjp) +
                            ",report=" + ConvertToParam(data.report) +
                            ",userId=" + ConvertToParam(data.userId) +
                            ",createdBy=" + ConvertToParam(data.createdBy) +
                            ",createdDate=" + ConvertToParam(data.createdDate) +
                            ",lastModifiedBy=" + ConvertToParam(data.lastModifiedBy) +
                            ",lastModifiedDate=" + ConvertToParam(data.lastModifiedDate) +
                            " WHERE BupotSpt23DetailID=" + ConvertToParam(data.BupotSpt23DetailID);

                        SqlCommand cmd = new SqlCommand(updateQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Update Existing Bupot", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Update Existing Bupot", updatedBupotDetail.Count + " record(s) updated", "Success");

                Console.WriteLine("Update existing bupot ref...");
                isSuccess = true;
                foreach (var data in updatedBupotRef)
                {
                    try
                    {
                        var updateQuery = "UPDATE dbo.BupotSpt23Ref SET " +
                            "BupotSpt23DetailID=" + ConvertToParam(data.BupotSpt23DetailID) +
                            "jenis=" + ConvertToParam(data.jenis) +
                            ",noDok=" + ConvertToParam(data.noDok) +
                            ",tgl=" + ConvertToParam(data.tgl) +
                            ",bp23=" + ConvertToParam(data.bp23) +
                            ",bp26=" + ConvertToParam(data.bp26) +
                            ",createdBy=" + ConvertToParam(data.createdBy) +
                            ",createdDate=" + ConvertToParam(data.createdDate) +
                            ",lastModifiedBy=" + ConvertToParam(data.lastModifiedBy) +
                            ",lastModifiedDate=" + ConvertToParam(data.lastModifiedDate) +
                            " WHERE refsID=" + ConvertToParam(data.refsID);

                        SqlCommand cmd = new SqlCommand(updateQuery, MyConn);
                        cmd.CommandTimeout = 2000;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                        CreateLog("Update Existing Bupot Ref", ex.Message, "Failed");
                    }
                }
                if (isSuccess)
                    CreateLog("Update Existing Bupot Ref", updatedBupotRef.Count + " record(s) updated", "Success");
                #endregion

                MyConn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Insert failed");
                CreateLog("Database Connection", ex.Message, "Failed");
                await LogoutApi();
                MyConn.Close();
            }
        }

        static async Task<string> LogoutApi()
        {
            Console.WriteLine("Loging out...");
            using (var client = new HttpClient())
            {
                // Setting Base address.  
                client.BaseAddress = new Uri(ConfigurationManager.AppSettings["BASE_URL"]);
                var request = new HttpRequestMessage(HttpMethod.Delete, ApiConfig.EndpointLogout);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizeToken);

                var response = await client.DeleteAsync(ApiConfig.EndpointLogout);

                if (response.IsSuccessStatusCode)
                {
                    CreateLog("Logout", "", "Success");
                    return "Success";
                }
            }

            return "Failed";
        }

        static dynamic ConvertObject(object obj)
        {
            var objType = obj.GetType().Name;
            switch (objType)
            {
                case "String":
                    return (string)obj;
                case "Int32":
                    return (int)obj;
                case "Int64":
                    return (long)obj;
                case "DateTime":
                    return (DateTime)obj;
                case "Boolean":
                    return (bool)obj;
                default:
                    return null;
            }
        }

        static string ConvertToParam(object obj)
        {
            if (obj != null)
            {
                var objType = obj.GetType().Name;
                if (objType == "String")
                    return "'" + ((string)obj).Replace("'", "''") + "'";
                if (objType == "DateTime")
                    return "'" + (DateTime)obj + "'";
                if (objType == "Boolean")
                {
                    if ((bool)obj)
                        return "1";

                    return "0";
                }
                if (objType == "List`1")
                {
                    var data = "'";
                    foreach (var item in (IList)obj)
                    {
                        if (data == "'")
                            data += item.ToString();
                        else
                            data += "," + item.ToString();
                    }
                    data += "'";
                    return data;
                }
            }

            var obj1 = Convert.ToString(obj);
            if (string.IsNullOrEmpty(obj1))
                return "null";

            return obj1;
        }

        static void MappingAttachment(string id, string filename, string fullpath, DateTime datenow)
        {
            SqlConnection MyConn = new SqlConnection(connString);
            MyConn.Open();
            using (SqlCommand sqlCommand = new SqlCommand($"SELECT COUNT(Code) FROM dbo.TrAttachment WHERE Code={ConvertToParam(id)}", MyConn))
            {
                int isExist = (int)sqlCommand.ExecuteScalar();
                var query = "";
                if (isExist > 0)
                {
                    query = "UPDATE dbo.TrAttachment SET " +
                        "FileName=" + ConvertToParam(filename) +
                        ",FileNameOri=" + ConvertToParam(filename) +
                        ",Path=" + ConvertToParam(fullpath) +
                        ",IsActive=" + ConvertToParam(true) +
                        ",UploadedDate=" + ConvertToParam(datenow) +
                        ",UploadedBy=" + ConvertToParam("JOB") +
                        " WHERE Code=" + ConvertToParam(id);
                }
                else
                {
                    query = "INSERT INTO dbo.TrAttachment(Code,FileName,FileNameOri,CodeAttachment,Path,IsActive,UploadedDate,UploadedBy) " +
                    "VALUES(" +
                        ConvertToParam(id) + "," +
                        ConvertToParam(filename) + "," +
                        ConvertToParam(filename) + "," +
                        ConvertToParam("Ebupot") + "," +
                        ConvertToParam(fullpath) + "," +
                        ConvertToParam(true) + "," +
                        ConvertToParam(datenow) + "," +
                        ConvertToParam("JOB") +
                    ")";
                }
                SqlCommand cmd = new SqlCommand(query, MyConn);
                cmd.CommandTimeout = 2000;
                cmd.ExecuteNonQuery();
            }
        }


        static void CreateLog(string activity, string description, string status)
        {
            var date = DateTime.Now;

            SqlConnection MyConn = new SqlConnection(connString);
            MyConn.Open();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                string query = "INSERT INTO dbo.LogBupotApi (Activity,Description,Status,Date) " +
                "VALUES(" +
                    ConvertToParam(activity) + "," +
                    ConvertToParam(description) + "," +
                    ConvertToParam(status) + "," +
                    ConvertToParam(date) +
                ")";
                SqlCommand cmd = new SqlCommand(query, MyConn);
                cmd.CommandTimeout = 2000;
                cmd.ExecuteNonQuery();
            }
        }

        static async Task ErrorLog(string activity, HttpResponseMessage response)
        {
            var error = await ProtocolResponse.FromHttpResponseAsync<RequestResponse<ErrorResultModel>>(response).ConfigureAwait(true);
            var err = error.Model;
            if (err != null)
            {
                CreateLog(activity, err.error_description ?? err.detail, "Failed");
                if (err.error_description != null && err.error_description.Contains("Anda sudah logout"))
                    IsSessionExpired = true;
            }
            else
            {
                var html = await response.Content.ReadAsStringAsync();
                Match title = Regex.Match(html, "<title>(.*)</title>", RegexOptions.Singleline);
                Match body = Regex.Match(html, "<body>(.*)</body>", RegexOptions.Singleline);
                var title2 = title.Groups[1].Value;
                var body2 = body.Groups[1].Value;
                if (!string.IsNullOrEmpty(body2))
                    CreateLog(activity, body2, "Failed");
                else if (!string.IsNullOrEmpty(title2))
                    CreateLog(activity, title2, "Failed");
            }
        }

        static async Task UploadToPortal(List<UploadFileModel> models)
        {
            var now = DateTime.Now;
            var isSuccess = true;
            Console.WriteLine("Copy Files to Server");
            foreach (var model in models)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StreamContent(new MemoryStream(model.FileData)), model.FileName);

                            var url = $"http://netapps.trakindo.co.id:8042/AutoSendFileToSharepointService.svc/sharepoint/uploadfile/0006/{model.FileName}/eBupotDocument/TestFolder/0/dit.system.wssp";
                            var response = await client.PostAsync(url, content);

                            if (response.IsSuccessStatusCode)
                            {
                                string path = $"http://netapps.trakindo.co.id:8042/AutoSendFileToSharepointService.svc/sharepoint/downloadfile/{model.FileName}/0006/eBupotDocument/TestFolder/0/dit.system.wssp";
                                //Mapping Attachment to POST
                                MappingAttachment(model.ID, model.FileName, path, now);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    CreateLog("Upload Files", model.FileName + ": " + ex.Message, "Failed");
                }
            }
            if (isSuccess)
                CreateLog("Upload Files", models.Count + " file(s) uploaded", "Success");
        }

        static async Task GetReportFromApi(HttpClient client, string endpoint, string id)
        {
            var response = await client.GetAsync($"{endpoint}/{id}/report").ConfigureAwait(true);

            // Verification  
            if (response.IsSuccessStatusCode)
            {
                var resData = await ProtocolResponse.FromHttpResponseAsync<RequestResponse<string>>(response).ConfigureAwait(true);

                if (string.IsNullOrEmpty(resData.Model))
                {
                    IsGetSuccess = false;
                    if (endpoint.Contains("23"))
                        CreateLog("Get Bupot 23 Document", id + ": File not found", "Failed");
                    else
                        CreateLog("Get Bupot 26 Document", id + ": File not found", "Failed");
                }
                else
                {
                    using (WebClient myWebClient = new WebClient())
                    {
                        try
                        {
                            string filename = "";
                            byte[] fileData = myWebClient.DownloadData(resData.Model);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(resData.Model);
                            HttpWebResponse res = (HttpWebResponse)request.GetResponse();
                            using (Stream rstream = res.GetResponseStream())
                            {
                                filename = res.Headers["Content-Disposition"] != null ?
                                    res.Headers["Content-Disposition"].Replace("attachment; filename=", "").Replace("\"", "") :
                                    res.Headers["Location"] != null ? Path.GetFileName(res.Headers["Location"]) :
                                    Path.GetFileName(resData.Model).Contains('?') || Path.GetFileName(resData.Model).Contains('=') ?
                                    Path.GetFileName(res.ResponseUri.ToString().Substring(0, res.ResponseUri.ToString().IndexOf("?"))) : "";
                            }
                            res.Close();
                            UploadFileModel fileModel = new UploadFileModel()
                            {
                                FileData = fileData,
                                FileName = filename,
                                ID = id
                            };
                            UploadFileModels.Add(fileModel);
                            //await UploadToPortal(fileData, filename, id, now);
                        }
                        catch (Exception ex)
                        {
                            IsGetSuccess = false;
                            if (endpoint.Contains("23"))
                                CreateLog("Get Bupot 23 Document", id + ": " + ex.Message, "Failed");
                            else
                                CreateLog("Get Bupot 26 Document", id + ": " + ex.Message, "Failed");
                        }
                    }
                }
            }
            else
            {
                IsGetSuccess = false;
                if (endpoint.Contains("23"))
                    await ErrorLog("Get Bupot 23 Document", response);
                else
                    await ErrorLog("Get Bupot 26 Document", response);

                if (IsSessionExpired)
                    await GetAuthorizeToken();
            }
        }
    }
}
