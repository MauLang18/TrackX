using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class TrackingNoLoginApplication : ITrackingNoLoginApplication
    {
        private readonly ISecretService _secretService;

        public TrackingNoLoginApplication(ISecretService secretService)
        {
            _secretService = secretService;
        }

        private async Task<AuthenticationConfig?> GetConfigAsync()
        {
            var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
            var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
            return SecretResponse?.Data?.Data;
        }

        [Obsolete]
        private async Task<string> GetAccessTokenAsync()
        {
            var Config = await GetConfigAsync();

            string clientId = Config!.ClientId!;
            string clientSecret = Config!.ClientSecret!;
            string authority = Config!.Authority!;
            string crmUrl = Config!.CrmUrl!;

            ClientCredential credentials = new ClientCredential(clientId, clientSecret);
            var authContext = new AuthenticationContext(authority);
            var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
            return result.AccessToken;
        }

        private async Task<HttpClient> ConfigureHttpClientAsync(string accessToken)
        {
            var Config = await GetConfigAsync(); // Espera el resultado aquí

            string crmUrl = Config!.CrmUrl!; // Ahora puedes acceder a CrmUrl

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(crmUrl),
                Timeout = TimeSpan.FromSeconds(300)
            };

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
            httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return httpClient;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByIDTRA(string idtra)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingNoLogin>>();

            try
            {
                string accessToken = await GetAccessTokenAsync();
                using var httpClient = await ConfigureHttpClientAsync(accessToken);

                string entityName = "incidents";
                string requestUri = $"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(title,'{idtra}')&$orderby=title asc";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse)!;

                    response.IsSuccess = true;
                    response.Data = dynamicsObject;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByPO(string po)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingNoLogin>>();

            try
            {
                string accessToken = await GetAccessTokenAsync();
                using var httpClient = await ConfigureHttpClientAsync(accessToken);

                string entityName = "incidents";
                string requestUri = $"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(new_po,'{po}')&$orderby=title asc";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse)!;

                    response.IsSuccess = true;
                    response.Data = dynamicsObject;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByBCF(string bcf)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingNoLogin>>();

            try
            {
                string accessToken = await GetAccessTokenAsync();
                using var httpClient = await ConfigureHttpClientAsync(accessToken);

                string entityName = "incidents";
                string requestUri = $"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(new_bcf,'{bcf}')&$orderby=title asc";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse)!;

                    response.IsSuccess = true;
                    response.Data = dynamicsObject;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingNoLogin>>> TrackingByContenedor(string contenedor)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingNoLogin>>();

            try
            {
                string accessToken = await GetAccessTokenAsync();
                using var httpClient = await ConfigureHttpClientAsync(accessToken);

                string entityName = "incidents";
                string requestUri = $"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=contains(new_contenedor,'{contenedor}')&$orderby=title asc";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse)!;

                    response.IsSuccess = true;
                    response.Data = dynamicsObject;
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }
    }
}