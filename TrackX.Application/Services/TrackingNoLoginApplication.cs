using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class TrackingNoLoginApplication : ITrackingNoLoginApplication
{
    [Obsolete]
    public async Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByIDTRA(string idtra)
    {
        var response = new BaseResponse<DynamicsTrackingNoLogin>();

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

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(title,'{idtra}')&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();

                if (httpResponseMessaje.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                    // Deserializar la cadena JSON a un objeto Dynamics
                    DynamicsTrackingNoLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingNoLogin>(jsonResponse)!;

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
    public async Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByPO(string po)
    {
        var response = new BaseResponse<DynamicsTrackingNoLogin>();

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

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(new_po,'{po}')&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();

                if (httpResponseMessaje.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                    // Deserializar la cadena JSON a un objeto Dynamics
                    DynamicsTrackingNoLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingNoLogin>(jsonResponse)!;

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
    public async Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByBCF(string bcf)
    {
        var response = new BaseResponse<DynamicsTrackingNoLogin>();

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

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_bcf,new_confirmacinzarpe,new_destino,new_eta,new_etd1,new_instcliente,new_ingreso,new_ingresoabodegas,new_barcodesalida,new_origen,new_poe,new_pol,new_transporte,title,new_preestado2,modifiedon&$filter=contains(new_bcf,'{bcf}')&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();

                if (httpResponseMessaje.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                    // Deserializar la cadena JSON a un objeto Dynamics
                    DynamicsTrackingNoLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingNoLogin>(jsonResponse)!;

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
    public async Task<BaseResponse<DynamicsTrackingNoLogin>> TrackingByContenedor(string contenedor)
    {
        var response = new BaseResponse<DynamicsTrackingNoLogin>();

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

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=new_fechayhoraoficializacion,new_etadestino,new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_eta,new_etd1,modifiedon,new_incoterm,new_origen,new_po,new_poe,new_pol,new_preestado2,new_seal,_new_shipper_value,new_statuscliente,new_tamaoequipo,new_transporte,new_ingreso,new_new_facturacompaia,new_ingresoabodegas,new_instcliente,new_barcodesalida,title&$filter=contains(new_contenedor,'{contenedor}')&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();

                if (httpResponseMessaje.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                    DynamicsTrackingNoLogin dynamicsObject = JsonConvert.DeserializeObject<DynamicsTrackingNoLogin>(jsonResponse)!;

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