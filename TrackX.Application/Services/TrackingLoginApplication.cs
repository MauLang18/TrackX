using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class TrackingLoginApplication : ITrackingLoginApplication
    {
        private readonly IClienteApplication _clienteApplication;
        private readonly IDistributedCache _distributedCache;

        public TrackingLoginApplication(IClienteApplication clienteApplication, IDistributedCache distributedCache)
        {
            _clienteApplication = clienteApplication;
            _distributedCache = distributedCache;
        }

        [Obsolete]
        public async Task<BaseResponse<DynamicsTrackingLogin>> TrackingFinalizadoByCliente(string cliente)
        {
            var response = new BaseResponse<DynamicsTrackingLogin>();
            string seruializedList;
            var cacheKey = $"CasosLogisticosFinalizados-{cliente}";
            var redisListado = await _distributedCache.GetAsync(cacheKey);

            if (redisListado != null)
            {
                seruializedList = Encoding.UTF8.GetString(redisListado);
                response.IsSuccess = true;
                response.Data = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(seruializedList);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
                try
                {
                    string clientId = "04f616d1-fb10-4c4f-ba02-45d2562fa9a8";
                    string clientSecrets = "1cn8Q~reOm4kQQ5fuaMUbR_X.cmtbQwyxv22IaVH";
                    string authority = "https://login.microsoftonline.com/48f7ad87-a406-4c72-98f5-d1c996e7e6f2";
                    string crmUrl = "https://sibaja07.crm.dynamics.com/";

                    string accessToken = string.Empty;

                    ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
                    var authContext = new AuthenticationContext(authority);
                    var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
                    accessToken = result.AccessToken;

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(crmUrl);
                        httpClient.Timeout = TimeSpan.FromSeconds(300);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        string entityName = "incidents";

                        HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_new_facturacompaia,new_transporte,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_proyecciondeingreso&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000023 or new_preestado2 eq 100000022 or new_preestado2 eq 100000021 or new_preestado2 eq 100000008 or new_preestado2 eq 100000009 or new_preestado2 eq 100000010 or new_preestado2 eq 100000011))&$orderby=title desc");
                        httpResponseMessaje.EnsureSuccessStatusCode();

                        if (httpResponseMessaje.IsSuccessStatusCode)
                        {
                            string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                            DynamicsTrackingLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(jsonResponse)!;

                            foreach (var item in dynamicsObject.value!)
                            {
                                string shipperValue = item._new_shipper_value!;

                                if (shipperValue is not null)
                                {
                                    var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValue);

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

                            seruializedList = JsonConvert.SerializeObject(dynamicsObject);
                            redisListado = Encoding.UTF8.GetBytes(seruializedList);

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
        public async Task<BaseResponse<DynamicsTrackingLogin>> TrackingActivoByCliente(string cliente)
        {
            var response = new BaseResponse<DynamicsTrackingLogin>();
            string seruializedList;
            var cacheKey = $"CasosLogisticosActivos-{cliente}";
            var redisListado = await _distributedCache.GetAsync(cacheKey);

            if (redisListado != null)
            {
                seruializedList = Encoding.UTF8.GetString(redisListado);
                response.IsSuccess = true;
                response.Data = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(seruializedList);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            else
            {
                try
                {
                    string clientId = "04f616d1-fb10-4c4f-ba02-45d2562fa9a8";
                    string clientSecrets = "1cn8Q~reOm4kQQ5fuaMUbR_X.cmtbQwyxv22IaVH";
                    string authority = "https://login.microsoftonline.com/48f7ad87-a406-4c72-98f5-d1c996e7e6f2";
                    string crmUrl = "https://sibaja07.crm.dynamics.com/";

                    string accessToken = string.Empty;

                    ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
                    var authContext = new AuthenticationContext(authority);
                    var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
                    accessToken = result.AccessToken;

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(crmUrl);
                        httpClient.Timeout = TimeSpan.FromSeconds(300);
                        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        string entityName = "incidents";

                        HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_new_facturacompaia,title,new_lugarcolocacion,new_redestino,new_diasdetransito,new_barcodesalida,new_viajedesalida,_customerid_value,new_ofertatarifaid,new_proyecciondeingreso&$filter=((_customerid_value eq {cliente}) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc");
                        httpResponseMessaje.EnsureSuccessStatusCode();

                        if (httpResponseMessaje.IsSuccessStatusCode)
                        {
                            string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                            DynamicsTrackingLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingLogin>(jsonResponse)!;

                            foreach (var item in dynamicsObject.value!)
                            {
                                string shipperValue = item._new_shipper_value!;

                                if (shipperValue is not null)
                                {
                                    var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValue);

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

                            seruializedList = JsonConvert.SerializeObject(dynamicsObject);
                            redisListado = Encoding.UTF8.GetBytes(seruializedList);

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