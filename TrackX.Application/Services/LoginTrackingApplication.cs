using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class LoginTrackingApplication : ILoginTrackingApplication
    {
        private readonly IClienteApplication _clienteApplication;

        public LoginTrackingApplication(IClienteApplication clienteApplication)
        {
            _clienteApplication = clienteApplication;
        }

        [Obsolete]
        public async Task<BaseResponse<DynamicsLoginTracking>> TrackingByIDTRA(string idtra, string cliente)
        {
            var response = new BaseResponse<DynamicsLoginTracking>();

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

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(title,'{idtra}'))&$orderby=title asc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsLoginTracking dynamicsObject = JsonConvert.DeserializeObject<DynamicsLoginTracking>(jsonResponse)!;

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

        [Obsolete]
        public async Task<BaseResponse<DynamicsLoginTracking>> TrackingByPO(string po, string cliente)
        {
            var response = new BaseResponse<DynamicsLoginTracking>();

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

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_po,'{po}'))&$orderby=title asc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsLoginTracking dynamicsObject = JsonConvert.DeserializeObject<DynamicsLoginTracking>(jsonResponse)!;

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

        [Obsolete]
        public async Task<BaseResponse<DynamicsLoginTracking>> TrackingByBCF(string bcf, string cliente)
        {
            var response = new BaseResponse<DynamicsLoginTracking>();

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

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_bcf,'{bcf}'))&$orderby=title asc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsLoginTracking dynamicsObject = JsonConvert.DeserializeObject<DynamicsLoginTracking>(jsonResponse)!;

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

        [Obsolete]
        public async Task<BaseResponse<DynamicsLoginTracking>> TrackingByContenedor(string contenedor, string cliente)
        {
            var response = new BaseResponse<DynamicsLoginTracking>();

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

                    HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_contenedor,'{contenedor}'))&$orderby=title asc");
                    httpResponseMessaje.EnsureSuccessStatusCode();

                    if (httpResponseMessaje.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                        DynamicsLoginTracking dynamicsObject = JsonConvert.DeserializeObject<DynamicsLoginTracking>(jsonResponse)!;

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