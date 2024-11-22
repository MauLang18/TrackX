using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class TransInternacionalApplication : ITransInternacionalApplication
{
    private readonly IClienteApplication _clienteApplication;
    private readonly ISecretService _secretService;
    private readonly HttpClient _httpClient;
    private readonly IFileStorageLocalApplication _fileStorage;

    public TransInternacionalApplication(
        IClienteApplication clienteApplication,
        ISecretService secretService,
        HttpClient httpClient,
        IFileStorageLocalApplication fileStorage)
    {
        _clienteApplication = clienteApplication;
        _secretService = secretService;
        _httpClient = httpClient;
        _fileStorage = fileStorage;
    }

    private async Task<AuthenticationConfig?> GetConfigAsync()
    {
        var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
        var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
        return SecretResponse?.Data?.Data;
    }

    public async Task<BaseResponse<Dynamics<DynamicsTransInternacional>>> ListTransInternacional(int numFilter, string textFilter)
    {
        var response = new BaseResponse<Dynamics<DynamicsTransInternacional>>();
        var cliente = "";
        var Config = await GetConfigAsync();

        try
        {
            string clientId = Config!.ClientId!;
            string clientSecret = Config!.ClientSecret!;
            string authority = Config!.Authority!;
            string crmUrl = Config!.CrmUrl!;

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

            if (numFilter == 1 && !string.IsNullOrEmpty(textFilter))
            {
                var nuevoValorCliente = await _clienteApplication.CodeCliente(textFilter);

                foreach (var item in nuevoValorCliente.Data!.value!)
                {
                    cliente = item.name;
                    textFilter = item.accountid!;
                }
            }

            string url = BuildUrl(entityName, numFilter, textFilter);

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                Dynamics<DynamicsTransInternacional> apiResponse = JsonConvert.DeserializeObject<Dynamics<DynamicsTransInternacional>>(jsonResponse)!;

                if (numFilter != 1 || string.IsNullOrEmpty(textFilter))
                {
                    var shipperValues = apiResponse.value!
                    .Where(item => item._customerid_value != null)
                    .Select(item => item._customerid_value)
                    .Distinct()
                    .ToList();

                    var clientesResult = await _clienteApplication.NombreCliente(shipperValues!);

                    var clienteMap = clientesResult.Data!.value!
                        .ToDictionary(clientes => clientes.accountid!, clientes => clientes.name!);

                    foreach (var item in apiResponse.value!)
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
                }
                else
                {
                    foreach (var item in apiResponse.value!)
                    {
                        item._customerid_value = cliente;
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

        var Config = await GetConfigAsync();

        try
        {
            string clientId = Config!.ClientId!;
            string clientSecret = Config!.ClientSecret!;
            string authority = Config!.Authority!;
            string crmUrl = Config!.CrmUrl!;

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
                new_observacionesgenerales = request.Comentario
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
        if (string.IsNullOrEmpty(textFilter))
        {
            numFilter = 0;
        }

        string preestadoFilter = "(new_preestado2 ne 100000011 and new_preestado2 ne 100000009 and new_preestado2 ne 100000010 and new_preestado2 ne 100000019 and new_preestado2 ne 100000023 and new_preestado2 ne 100000022 and new_preestado2 ne 100000010 and new_preestado2 ne 100000021 and new_preestado2 ne 100000012 and new_preestado2 ne 100000011)";

        string filter = numFilter switch
        {
            1 => $"_customerid_value eq '{textFilter}' and {preestadoFilter}",
            2 => $"contains(new_contenedor,'{textFilter}') and {preestadoFilter}",
            3 => $"contains(new_bcf,'{textFilter}') and {preestadoFilter}",
            4 => $"contains(new_factura,'{textFilter}') and {preestadoFilter}",
            5 => $"contains(new_po,'{textFilter}') and {preestadoFilter}",
            6 => $"contains(title,'{textFilter}') and {preestadoFilter}",
            _ => $"{preestadoFilter}"
        };

        return $"api/data/v9.2/{entityName}?$select=new_tipoaforo,new_duaanticipados,new_duanacional,new_numerorecibo,new_nombrepedimentador,new_borradordeimpuestos,new_documentodenacionalizacion,new_certificadodeorigen,new_duaanticipados,new_duanacional,new_contenedor,new_factura,new_aplicacertificadodeorigen,new_aplicacertificadoreexportacion,new_bloriginal,new_cantequipo,new_cartadesglosecargos,new_cartatrazabilidad,new_certificadoreexportacion,_customerid_value,new_commodity,new_confirmacinzarpe,new_contidadbultos,new_draftbl,new_ejecutivocomercial,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_etd1,new_exoneracion,new_facturacomercial,new_fechabldigittica,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_bcf,new_listadeempaque,new_llevaexoneracion,new_observacionesgenerales,new_permisos,new_peso,new_po,new_poe,new_pol,new_preestado2,new_tamaoequipo,new_tipoaforo,title&$filter=({filter}) and (Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01') and (new_destino eq 100000030 or new_destino eq 100000003 or new_destino eq 100000012 or new_destino eq 100000008 or new_destino eq 100000002 or new_destino eq 100000001 or new_destino eq 100000000) and new_tipoaforo eq null and Microsoft.Dynamics.CRM.ContainValues(PropertyName='new_servicio',PropertyValues=['100000001']))&$orderby=new_eta desc";
    }

    public async Task<BaseResponse<bool>> UpdateDocuments(TransInternacionalDocumentRequestDto request)
    {
        var response = new BaseResponse<bool>();

        var Config = await GetConfigAsync();

        try
        {
            string clientId = Config!.ClientId!;
            string clientSecret = Config!.ClientSecret!;
            string authority = Config!.Authority!;
            string crmUrl = Config!.CrmUrl!;

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

            var documento = await _fileStorage.SaveFile(AzureContainers.DOCUMENTOS, request.File!);

            var field = request.FieldName;

            var comentarioRecord = new JObject
            {
                [field!] = documento
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
}