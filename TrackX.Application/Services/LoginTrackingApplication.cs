using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class LoginTrackingApplication : ILoginTrackingApplication
{
    private readonly IClienteApplication _clienteApplication;
    private readonly ISecretService _secretService;

    public LoginTrackingApplication(IClienteApplication clienteApplication, ISecretService secretService)
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
    private async Task<string> GetAccessToken()
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

    [Obsolete]
    public async Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByIDTRA(string idtra, string cliente)
    {
        var response = new BaseResponse<Dynamics<DynamicsLoginTracking>>();

        var Config = await GetConfigAsync();

        try
        {
            string accessToken = await GetAccessToken();

            string crmUrl = Config!.CrmUrl!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_bookingno,new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(title,'{idtra}'))&$orderby=title asc");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    Dynamics<DynamicsLoginTracking> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsLoginTracking>>(jsonResponse)!;

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
    public async Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByPO(string po, string cliente)
    {
        var response = new BaseResponse<Dynamics<DynamicsLoginTracking>>();

        var Config = await GetConfigAsync();

        try
        {
            string accessToken = await GetAccessToken();

            string crmUrl = Config!.CrmUrl!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_bookingno,new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_po,'{po}'))&$orderby=title asc");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    Dynamics<DynamicsLoginTracking> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsLoginTracking>>(jsonResponse)!;

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
    public async Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByBCF(string bcf, string cliente)
    {
        var response = new BaseResponse<Dynamics<DynamicsLoginTracking>>();

        var Config = await GetConfigAsync();

        try
        {
            string accessToken = await GetAccessToken();

            string crmUrl = Config!.CrmUrl!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_bookingno,new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_bcf,'{bcf}'))&$orderby=title asc");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    Dynamics<DynamicsLoginTracking> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsLoginTracking>>(jsonResponse)!;

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
    public async Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByContenedor(string contenedor, string cliente)
    {
        var response = new BaseResponse<Dynamics<DynamicsLoginTracking>>();

        var Config = await GetConfigAsync();

        try
        {
            string accessToken = await GetAccessToken();

            string crmUrl = Config!.CrmUrl!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_bookingno,new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_contenedor,'{contenedor}'))&$orderby=title asc");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    Dynamics<DynamicsLoginTracking> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsLoginTracking>>(jsonResponse)!;

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
    public async Task<BaseResponse<Dynamics<DynamicsLoginTracking>>> TrackingByBooking(string booking, string cliente)
    {
        var response = new BaseResponse<Dynamics<DynamicsLoginTracking>>();

        var Config = await GetConfigAsync();

        try
        {
            string accessToken = await GetAccessToken();

            string crmUrl = Config!.CrmUrl!;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(crmUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(300);
                httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_bookingno,new_fechayhoraoficializacion,new_peso,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=((_customerid_value eq {cliente}) and contains(new_bookingno,'{booking}'))&$orderby=title asc");
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    Dynamics<DynamicsLoginTracking> dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsLoginTracking>>(jsonResponse)!;

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