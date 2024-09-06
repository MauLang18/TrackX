using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
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
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TransInternacionalApplication(
            IClienteApplication clienteApplication,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _clienteApplication = clienteApplication;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<BaseResponse<DynamicsTransInternacional>> ListTransInternacional(int numFilter, string textFilter)
        {
            var response = new BaseResponse<DynamicsTransInternacional>();

            try
            {
                var clientId = _configuration["Authentication:ClientId"];
                var clientSecret = _configuration["Authentication:ClientSecret"];
                var authority = _configuration["Authentication:Authority"];
                var crmUrl = _configuration["Authentication:CrmUrl"];

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority!))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl!);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string entityName = "incidents";
                string url = BuildUrl(entityName, numFilter, textFilter);

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
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

                    var clienteMap = clientesResult.Data!.value!
                        .ToDictionary(clientes => clientes.accountid!, clientes => clientes.name!);

                    foreach (var item in apiResponse.Value!)
                    {
                        if (item._customerid_value != null && clienteMap.TryGetValue(item._customerid_value, out var clienteName))
                        {
                            item._customerid_value = clienteName;
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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterComentario(TransInternacionalRequestDto request)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var clientId = _configuration["Authentication:ClientId"];
                var clientSecret = _configuration["Authentication:ClientSecret"];
                var authority = _configuration["Authentication:Authority"];
                var crmUrl = _configuration["Authentication:CrmUrl"];

                var app = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority!))
                    .Build();

                var result = await app.AcquireTokenForClient(new[] { $"{crmUrl}/.default" }).ExecuteAsync();
                string accessToken = result.AccessToken;

                _httpClient.BaseAddress = new Uri(crmUrl!);
                _httpClient.Timeout = TimeSpan.FromSeconds(300);
                _httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                _httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var comentarioRecord = new
                {
                    new_descripcion1 = request.Comentario
                };

                string jsonContent = JsonConvert.SerializeObject(comentarioRecord);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                string url = $"api/data/v9.2/incidents({request.TransInternacionalId})";

                var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = content
                };

                HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(requestMessage);
                httpResponseMessage.EnsureSuccessStatusCode();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    response.IsSuccess = true;
                    response.Data = true;
                    response.Message = "Comentario actualizado exitosamente.";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Data = false;
                    response.Message = "Error al actualizar el comentario.";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Data = false;
                response.Message = ex.Message;
                WatchLogger.Log(ex.Message);
            }

            return response;
        }

        private static string BuildUrl(string entityName, int numFilter, string textFilter)
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

            return $"api/data/v9.2/{entityName}?$select=new_contenedor,new_factura,new_aplicacertificadodeorigen,new_aplicacertificadoreexportacion,new_cantequipo,_customerid_value,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_bcf,new_llevaexoneracion,new_peso,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,title,new_descripcion1&$filter={filter} and Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01') and (new_preestado2 ne 100000012 or new_preestado2 ne 100000010 or new_preestado2 ne 100000022 or new_preestado2 ne 100000021 or new_preestado2 ne 100000019 or new_preestado2 ne 100000023) and (new_destino eq 100000030 or new_destino eq 100000003 or new_destino eq 100000012 or new_destino eq 100000008 or new_destino eq 100000002 or new_destino eq 100000001 or new_destino eq 100000000)&$orderby=new_eta asc";
        }
    }
}