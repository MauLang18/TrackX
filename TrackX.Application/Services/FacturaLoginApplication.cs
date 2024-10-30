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
    public class FacturaLoginApplication : IFacturaLoginApplication
    {
        private readonly IClienteApplication _clienteApplication;
        private readonly ISecretService _secretService;

        public FacturaLoginApplication(IClienteApplication clienteApplication, ISecretService secretService)
        {
            _clienteApplication = clienteApplication;
            _secretService = secretService;
        }

        private async Task<AuthenticationConfig?> GetConfigAsync()
        {
            var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
            var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
            return SecretResponse?.Data?.Data;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsFacturas>>> TrackingByFactura(string factura, string cliente)
        {
            var response = new BaseResponse<Dynamics<DynamicsFacturas>>();

            var Config = await GetConfigAsync();

            try
            {
                string clientId = Config!.ClientId!;
                string clientSecret = Config!.ClientSecret!;
                string authority = Config!.Authority!;
                string crmUrl = Config!.CrmUrl!;

                ClientCredential credentials = new ClientCredential(clientId, clientSecret);
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

                    string entityName = "incidents";
                    string requestUri = $"api/data/v9.2/{entityName}?$select=title,new_contenedor,_new_shipper_value,new_commodity,new_servicio&$filter=((_customerid_value eq {cliente}) and contains(new_new_facturacompaia,'{factura}'))&$orderby=title asc";

                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
                    httpResponseMessage.EnsureSuccessStatusCode();

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                        Dynamics<DynamicsFacturas> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsFacturas>>(jsonResponse)!;

                        foreach (var item in dynamicsObject.value!)
                        {
                            string shipperValue = item._new_shipper_value!;

                            if (shipperValue is not null)
                            {
                                var shipperValuesList = new List<string> { shipperValue };
                                var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

                                foreach (var items in nuevoValorCliente.Data!.value!)
                                {
                                    item._new_shipper_value = items.name;
                                }
                            }
                            else
                            {
                                item._new_shipper_value = "";
                            }
                        }

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