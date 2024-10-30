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
    public class CreditoClienteApplication : ICreditoClienteApplication
    {
        private readonly ISecretService _secretService;

        public CreditoClienteApplication(ISecretService secretService)
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
        public async Task<BaseResponse<Dynamics<DynamicsCreditoCliente>>> CreditoCliente(string code)
        {
            var response = new BaseResponse<Dynamics<DynamicsCreditoCliente>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                var credentials = new ClientCredential(clientId, clientSecret);
                var authContext = new AuthenticationContext(authority);
                var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
                string accessToken = result.AccessToken;

                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(crmUrl);
                    httpClient.Timeout = TimeSpan.FromSeconds(300);
                    httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Construye el URI de la solicitud
                    string entityName = "accounts";
                    string requestUri = $"api/data/v9.2/{entityName}?$select=_transactioncurrencyid_value,new_financiamiento,paymenttermscode,new_3,new_creditonoincluye,new_diasdecredito,new_fechadeiniciodecredito,new_fechaderenovaciondecredito,new_intersmoratoriomensual,creditlimit,new_tipodeproveedor&$filter=accountid eq {code}";

                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                    httpResponseMessage.EnsureSuccessStatusCode();

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                        Dynamics<DynamicsCreditoCliente> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsCreditoCliente>>(jsonResponse)!;

                        response.IsSuccess = true;
                        response.Data = dynamicsObject;
                        response.Message = ReplyMessage.MESSAGE_QUERY;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    }

                    return response;
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