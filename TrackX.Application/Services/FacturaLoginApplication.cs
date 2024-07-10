using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class FacturaLoginApplication : IFacturaLoginApplication
{
    private readonly IClienteApplication _clienteApplication;

    public FacturaLoginApplication(IClienteApplication clienteApplication)
    {
        _clienteApplication = clienteApplication;
    }

    [Obsolete]
    public async Task<BaseResponse<DynamicsFacturas>> TrackingByFactura(string factura, string cliente)
    {
        var response = new BaseResponse<DynamicsFacturas>();

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

                HttpResponseMessage httpResponseMessaje = await httpClient.GetAsync($"api/data/v9.2/{entityName}?$select=title,new_contenedor,_new_shipper_value,new_commodity,new_servicio&$filter=((_customerid_value eq {cliente}) and contains(new_new_facturacompaia,'{factura}'))&$orderby=title asc");
                httpResponseMessaje.EnsureSuccessStatusCode();

                if (httpResponseMessaje.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessaje.Content.ReadAsStringAsync();

                    DynamicsFacturas dynamicsObject = JsonConvert.DeserializeObject<DynamicsFacturas>(jsonResponse)!;

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