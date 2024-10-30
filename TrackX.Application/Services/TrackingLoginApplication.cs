using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class TrackingLoginApplication : ITrackingLoginApplication
    {
        private readonly IClienteApplication _clienteApplication;
        private readonly IDistributedCache _distributedCache;
        private readonly ISecretService _secretService;

        public TrackingLoginApplication(IClienteApplication clienteApplication, IDistributedCache distributedCache, ISecretService secretService)
        {
            _clienteApplication = clienteApplication;
            _distributedCache = distributedCache;
            _secretService = secretService;
        }

        private async Task<AuthenticationConfig?> GetConfigAsync()
        {
            var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
            var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
            return SecretResponse?.Data?.Data;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingLogin>>> TrackingFinalizadoByCliente(string cliente)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingLogin>>();
            string serializedList;
            var cacheKey = $"CasosLogisticosFinalizados-{cliente}";
            var redisListado = await _distributedCache.GetAsync(cacheKey);
            var Config = await GetConfigAsync();

            if (redisListado != null)
            {
                serializedList = Encoding.UTF8.GetString(redisListado);
                response.IsSuccess = true;
                response.Data = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingLogin>>(serializedList);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
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

                        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(
                            $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_new_facturacompaia,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_proyecciondeingreso&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000023 or new_preestado2 eq 100000022 or new_preestado2 eq 100000021 or new_preestado2 eq 100000008 or new_preestado2 eq 100000009 or new_preestado2 eq 100000010 or new_preestado2 eq 100000011))&$orderby=title desc");

                        httpResponseMessage.EnsureSuccessStatusCode();

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                            Dynamics<DynamicsTrackingLogin> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingLogin>>(jsonResponse)!;

                            var shipperValues = dynamicsObject.value!
                                .Where(item => item._new_shipper_value != null)
                                .Select(item => item._new_shipper_value)
                                .Distinct()
                                .ToList();

                            Dictionary<string, string> clienteNames = new Dictionary<string, string>();

                            if (shipperValues.Any())
                            {
                                var clientesResult = await _clienteApplication.NombreCliente(shipperValues!);

                                clienteNames = clientesResult.Data!.value!
                                    .Select(clientes => clientes.name!)
                                    .Distinct()
                                    .ToDictionary(name => name, name => name);
                            }

                            foreach (var item in dynamicsObject.value!)
                            {
                                if (item._new_shipper_value != null)
                                {
                                    item._new_shipper_value = clienteNames.TryGetValue(item._new_shipper_value, out var clienteName)
                                        ? clienteName
                                        : "";
                                }
                                else
                                {
                                    item._new_shipper_value = "";
                                }
                            }

                            response.IsSuccess = true;
                            response.Data = dynamicsObject;
                            response.Message = ReplyMessage.MESSAGE_QUERY;

                            serializedList = JsonConvert.SerializeObject(dynamicsObject);
                            redisListado = Encoding.UTF8.GetBytes(serializedList);

                            var options = new DistributedCacheEntryOptions()
                                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(35));

                            await _distributedCache.SetAsync(cacheKey, redisListado, options);
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
            }

            return response;
        }

        [Obsolete]
        public async Task<BaseResponse<Dynamics<DynamicsTrackingLogin>>> TrackingActivoByCliente(string cliente)
        {
            var response = new BaseResponse<Dynamics<DynamicsTrackingLogin>>();
            string serializedList;
            var cacheKey = $"CasosLogisticosActivos-{cliente}";
            var redisListado = await _distributedCache.GetAsync(cacheKey);
            var Config = await GetConfigAsync();

            if (redisListado != null)
            {
                serializedList = Encoding.UTF8.GetString(redisListado);
                response.IsSuccess = true;
                response.Data = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingLogin>>(serializedList);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
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

                        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(
                            $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_new_facturacompaia,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_proyecciondeingreso&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc");

                        httpResponseMessage.EnsureSuccessStatusCode();

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                            Dynamics<DynamicsTrackingLogin> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingLogin>>(jsonResponse)!;

                            var shipperValues = dynamicsObject.value!
                                .Where(item => item._new_shipper_value != null)
                                .Select(item => item._new_shipper_value)
                                .Distinct()
                                .ToList();

                            Dictionary<string, string> clienteNames = new Dictionary<string, string>();

                            if (shipperValues.Any())
                            {
                                var clientesResult = await _clienteApplication.NombreCliente(shipperValues!);

                                clienteNames = clientesResult.Data!.value!
                                    .Select(clientes => clientes.name!)
                                    .Distinct()
                                    .ToDictionary(name => name, name => name);
                            }

                            foreach (var item in dynamicsObject.value!)
                            {
                                if (item._new_shipper_value != null)
                                {
                                    item._new_shipper_value = clienteNames.TryGetValue(item._new_shipper_value, out var clienteName)
                                        ? clienteName
                                        : "";
                                }
                                else
                                {
                                    item._new_shipper_value = "";
                                }
                            }

                            response.IsSuccess = true;
                            response.Data = dynamicsObject;
                            response.Message = ReplyMessage.MESSAGE_QUERY;

                            serializedList = JsonConvert.SerializeObject(dynamicsObject);
                            redisListado = Encoding.UTF8.GetBytes(serializedList);

                            var options = new DistributedCacheEntryOptions()
                                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(35));

                            await _distributedCache.SetAsync(cacheKey, redisListado, options);
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
            }

            return response;
        }
    }
}