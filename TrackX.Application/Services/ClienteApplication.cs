using Microsoft.Identity.Client;
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
    public class ClienteApplication : IClienteApplication
    {
        private readonly ISecretService _secretService;
        private readonly HttpClient _httpClient;

        public ClienteApplication(ISecretService secretService, HttpClient httpClient)
        {
            _secretService = secretService;
            _httpClient = httpClient;
        }

        private async Task<AuthenticationConfig?> GetConfigAsync()
        {
            var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
            var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
            return SecretResponse?.Data?.Data;
        }

        public async Task<BaseResponse<Dynamics<DynamicsClientes>>> CodeCliente(string name)
        {
            var response = new BaseResponse<Dynamics<DynamicsClientes>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "accounts";
                var requestUri = $"api/data/v9.2/{entityName}?$select=name,accountid&$filter=contains(name,'{name}')";

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsClientes>>(jsonResponse) ?? new Dynamics<DynamicsClientes>();

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

        public async Task<BaseResponse<Dynamics<DynamicsClientes>>> NameCliente(string code)
        {
            var response = new BaseResponse<Dynamics<DynamicsClientes>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "accounts";
                var requestUri = $"api/data/v9.2/{entityName}?$select=name,accountid&$filter=accountid eq {code}";

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsClientes>>(jsonResponse) ?? new Dynamics<DynamicsClientes>();

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

        public async Task<BaseResponse<Dynamics<DynamicsClientes>>> NombreCliente(List<string> code)
        {
            var response = new BaseResponse<Dynamics<DynamicsClientes>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "accounts";
                var filter = string.Join(" or ", code.Select(c => $"accountid eq {c}"));
                var requestUri = $"api/data/v9.2/{entityName}?$select=name,accountid&$filter={filter}";

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsClientes>>(jsonResponse) ?? new Dynamics<DynamicsClientes>();

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

        public async Task<BaseResponse<Dynamics<DynamicsIdtraClientes>>> ClienteIdtra(string idtra)
        {
            var response = new BaseResponse<Dynamics<DynamicsIdtraClientes>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";
                var requestUri = $"api/data/v9.2/{entityName}?$select=title,incidentid,_customerid_value&$filter=contains(title,'{idtra}')";

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsIdtraClientes>>(jsonResponse) ?? new Dynamics<DynamicsIdtraClientes>();

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