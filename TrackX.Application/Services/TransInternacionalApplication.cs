using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
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
                    string url = BuildUrl(entityName, numFilter, textFilter);

                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
                    httpResponseMessage.EnsureSuccessStatusCode();

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                        DynamicsTransInternacional apiResponse = JsonConvert.DeserializeObject<DynamicsTransInternacional>(jsonResponse)!;

                        var shipperValues = apiResponse.Value!
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
                                clienteMap[clientes.accountid!] = clientes.name!;
                            }
                        }

                        foreach (var item in apiResponse.Value!)
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
                        response.Data = apiResponse;
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

        public Task<BaseResponse<bool>> RegisterComentario(TransInternacionalRequestDto request)
        {
            throw new NotImplementedException();
        }

        private string BuildUrl(string entityName, int numFilter, string textFilter)
        {
            string filter = numFilter switch
            {
                1 => $"_customerid_value eq '{textFilter}' and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])",
                2 => $"contains(new_contenedor,'{textFilter}') and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])",
                3 => $"contains(new_bcf,'{textFilter}') and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])",
                4 => $"contains(new_factura,'{textFilter}') and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])",
                5 => $"contains(new_po,'{textFilter}') and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])",
                _ => $"Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001'])"
            };

            return $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_aplicacertificadodeorigen,new_aplicacertificadoreexportacion,new_cantequipo,_customerid_value,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_bcf,new_llevaexoneracion,new_peso,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title&$filter={filter} and Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01') and (new_preestado2 ne 100000012 or new_preestado2 ne 100000010 or new_preestado2 ne 100000022 or new_preestado2 ne 100000021 or new_preestado2 ne 100000019 or new_preestado2 ne 100000023) and (new_destino eq 100000030 or new_destino eq 100000003 or new_destino eq 100000012 or new_destino eq 100000008 or new_destino eq 100000002 or new_destino eq 100000001 or new_destino eq 100000000)&$orderby=new_eta asc";
        }
    }
}
