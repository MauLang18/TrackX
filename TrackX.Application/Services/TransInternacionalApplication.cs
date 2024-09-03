using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class TransInternacionalApplication : ITransInternacionalApplication
{
    private readonly IClienteApplication _clienteApplication;

    public TransInternacionalApplication(IClienteApplication clienteApplication)
    {
        _clienteApplication = clienteApplication;
    }

    [Obsolete]
    public async Task<BaseResponse<DynamicsTransInternacional>> ListTransInternacional(int numFilter, string textFilter)
    {
        var response = new BaseResponse<DynamicsTransInternacional>();

        try
        {
            string clientId = "04f616d1-fb10-4c4f-ba02-45d2562fa9a8";
            string clientSecrets = "1cn8Q~reOm4kQQ5fuaMUbR_X.cmtbQwyxv22IaVH";
            string authority = "https://login.microsoftonline.com/48f7ad87-a406-4c72-98f5-d1c996e7e6f2";
            string crmUrl = "https://sibaja07.crm.dynamics.com/";

            ClientCredential credentials = new ClientCredential(clientId, clientSecrets);
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
                string url;

                switch (numFilter)
                {
                    case 1:
                        url = "new_cliente";
                        break;
                    case 2:
                        //contenedor
                        url = $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,_customerid_value,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etadestino,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_llevaexoneracion&$filter=(contains(new_contenedor,'{textFilter}') and (createdon gt 2023-12-31T00:00:00.000Z) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc";
                        break;
                    case 3:
                        //bcf
                        url = $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,_customerid_value,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etadestino,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_llevaexoneracion&$filter=(contains(new_bcf,'{textFilter}') and (createdon gt 2023-12-31T00:00:00.000Z) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc";
                        break;
                    case 4:
                        //factura
                        url = $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,_customerid_value,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etadestino,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_llevaexoneracion&$filter=(contains(new_factura,'{textFilter}') and (createdon gt 2023-12-31T00:00:00.000Z) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc";
                        break;
                    case 5:
                        //po
                        url = $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,_customerid_value,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etadestino,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_llevaexoneracion&$filter=(contains(new_po,'{textFilter}') and (createdon gt 2023-12-31T00:00:00.000Z) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000002 or new_preestado2 eq 100000017 or new_preestado2 eq 100000003 or new_preestado2 eq 100000004 or new_preestado2 eq 100000005 or new_preestado2 eq 100000007 or new_preestado2 eq 100000018 or new_preestado2 eq 100000028 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000026 or new_preestado2 eq 100000015 or new_preestado2 eq 100000027 or new_preestado2 eq 100000014))&$orderby=title desc";
                        break;
                    default:
                        url = $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_bcf,new_cantequipo,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_destino,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,_customerid_value,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etadestino,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_llevaexoneracion&$filter=(createdon gt 2023-12-31 and new_servicio eq '100000001')";
                        break;
                }

                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                    DynamicsTransInternacional dynamicsObject = JsonConvert.DeserializeObject<DynamicsTransInternacional>(jsonResponse)!;

                    var shipperValues = dynamicsObject.value!
                        .Where(item => item._customerid_value != null)
                        .Select(item => item._customerid_value)
                        .Distinct()
                        .ToList();

                    var clientesResult = await _clienteApplication.NombreCliente(shipperValues!);

                    var clienteMap = new Dictionary<string, string>();
                    foreach (var clientes in clientesResult.Data!.value!)
                    {
                        if (!clienteMap.ContainsKey(clientes.name!))
                        {
                            clienteMap[clientes.name!] = clientes.name!;
                        }
                    }

                    foreach (var item in dynamicsObject.value!)
                    {
                        if (item._customerid_value != null && clienteMap.ContainsKey(item._customerid_value))
                        {
                            item._customerid_value = clienteMap[item._customerid_value];
                        }
                        else
                        {
                            item._customerid_value = "";
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

    public Task<BaseResponse<bool>> RegisterComentario(TransInternacionalRequestDto request)
    {
        throw new NotImplementedException();
    }
}