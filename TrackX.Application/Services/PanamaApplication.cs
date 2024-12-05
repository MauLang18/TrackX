using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Panama.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class PanamaApplication : IPanamaApplication
{
    private readonly IClienteApplication _clienteApplication;
    private readonly ISecretService _secretService;
    private readonly HttpClient _httpClient;
    private readonly IFileStorageLocalApplication _fileStorage;

    public PanamaApplication(IClienteApplication clienteApplication, ISecretService secretService, HttpClient httpClient, IFileStorageLocalApplication fileStorage)
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

    private static string BuildUrl(string entityName, int numFilter, string textFilter, int type)
    {
        if (string.IsNullOrEmpty(textFilter))
        {
            numFilter = 0;
        }

        string typeFilters = type switch 
        {
            //Varios
            1 => $"(new_preestado2 eq 100000002 or new_preestado2 eq 100000003 or new_preestado2 eq 100000018) and (Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01'))",
            //hub
            2 => $"(new_preestado2 eq 100000026) and (Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01'))",
            //movimiento
            3 => $"(new_preestado2 eq 100000024) and (Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01'))",
            //general
            _ => $"(new_poe eq 100000054 or new_poe eq 100000051 or new_poe eq 100000015 or new_poe eq 100000074 or new_poe eq 100000009 or new_poe eq 100000038 or new_poe eq 100000041 or new_poe eq 100000052 or new_poe eq 100000010 or new_poe eq 100000011 or new_poe eq 100000042 or new_poe eq 100000057) and (new_preestado2 eq 100000000 or new_preestado2 eq 100000001 or new_preestado2 eq 100000015 or new_preestado2 eq 100000014 or new_preestado2 eq 100000017 or new_preestado2 eq 100000002 or new_preestado2 eq 100000003 or new_preestado2 eq 100000024 or new_preestado2 eq 100000025 or new_preestado2 eq 100000004 or new_preestado2 eq 100000026 or new_preestado2 eq 100000013 or new_preestado2 eq 100000018 or new_preestado2 eq 100000009) and (Microsoft.Dynamics.CRM.OnOrAfter(PropertyName='createdon',PropertyValue='2024-01-01'))"
        };

        string typeData = type switch
        {
            //Varios
            1 => $"new_contenedor,new_actualizacionovernight,new_aplicacertificadodeorigen,_customerid_value,new_comentariosovernight,new_commodity,new_contidadbultos,new_eta,new_fechastatus,new_peso,new_po,new_poe,new_statuscliente,new_tamaoequipo,title",
            //hub
            2 => $"new_contenedor,new_actualizacionovernight,new_aplicacertificadodeorigen,_customerid_value,new_comentariosovernight,new_commodity,new_contidadbultos,new_eta,new_fechastatus,new_peso,new_po,new_poe,new_statuscliente,new_tamaoequipo,title",
            //movimiento
            3 => $"new_contenedor,new_actualizacionovernight,new_aplicacertificadodeorigen,_customerid_value,new_comentariosovernight,new_commodity,new_contidadbultos,new_eta,new_fechastatus,new_peso,new_po,new_poe,new_statuscliente,new_tamaoequipo,title",
            //general
            _ => $"new_dmcsalidaenlace,new_dmcsalida,new_tienlace,new_ti,new_dmcentradaenlace,new_dmcentrada,new_cartaliberacionnaviera,new_blnavieraswb,new_ducatenlace,new_manifiestoenlace,new_cartaporte,new_fecharealizarcertreexportacion,new_fechadmcsalida,new_fechacargahubpanama,new_fechati,new_fechadmcentrada,new_pagonavierarealizado,new_fechallegadacrc,new_fechasalidapty,new_ultimodialibrecontenedor,new_decertificadodereexportacion,new_contenedor,new_factura,new_aplicacertificadodeorigen,new_aplicacertificadoreexportacion,new_bloriginal,new_borradordecertificadodeorigen,new_cantequipo,new_cartadesglosecargos,new_cartatrazabilidad,new_certificadoorigen,new_certificadoreexportacion,_customerid_value,new_commodity,new_contidadbultos,new_draftbl,new_entregabloriginal,new_entregacartatrazabilidad,new_entregatraduccion,new_eta,new_exoneracion,new_facturacomercial,new_fechablimpreso,new_liberacionmovimientoinventario,new_fechaliberacionfinanciera,new_fechastatus,new_bcf,new_listadeempaque,new_peso,new_po,new_poe,new_pol,new_preestado2,new_statuscliente,new_tamaoequipo,new_traducciondefacturas,title"
        };

        string filter = numFilter switch
        {
            1 => $"(_customerid_value eq '{textFilter}') and {typeFilters}",
            2 => $"(contains(new_contenedor,'{textFilter}')) and {typeFilters}",
            3 => $"(contains(new_bcf,'{textFilter}')) and {typeFilters}",
            4 => $"(contains(new_factura,'{textFilter}')) and {typeFilters}",
            5 => $"(contains(new_po,'{textFilter}')) and {typeFilters}",
            6 => $"(contains(title,'{textFilter}')) and {typeFilters}",
            7 => $"(contains(new_dmcentrada,'{textFilter}')) and {typeFilters}",
            8 => $"(Microsoft.Dynamics.CRM.On(PropertyName='new_fechadmcentrada',PropertyValue='{textFilter}')) and {typeFilters}",
            9 => $"(contains(new_dmcsalida,'{textFilter}')) and {typeFilters}",
            10 => $"(Microsoft.Dynamics.CRM.On(PropertyName='new_fechadmcentrada',PropertyValue='{textFilter}')) and {typeFilters}",
            11 => $"(contains(new_ti,'{textFilter}')) and {typeFilters}",
            12 => $"(Microsoft.Dynamics.CRM.On(PropertyName='new_fechati',PropertyValue='{textFilter}')) and {typeFilters}",
            _ => $"{typeFilters}"
        };

        return $"api/data/v9.2/{entityName}?$select={typeData}&$filter=({filter})&$orderby=new_eta asc";
    }

    public async Task<BaseResponse<Dynamics<DynamicsPanama>>> ListPanama(int numFilter, string textFilter, int type)
    {
        var response = new BaseResponse<Dynamics<DynamicsPanama>>();
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

            string url = BuildUrl(entityName, numFilter, textFilter, type);

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                Dynamics<DynamicsPanama> apiResponse = JsonConvert.DeserializeObject<Dynamics<DynamicsPanama>>(jsonResponse)!;

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

    public async Task<BaseResponse<bool>> RegisterComentario(PanamaRequestDto request)
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

            var field = request.FieldName;

            var comentarioRecord = new JObject
            {
                [field!] = request.Comentario
            };

            string jsonContent = JsonConvert.SerializeObject(comentarioRecord);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"api/data/v9.2/incidents({request.PanamaId})";

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

    public async Task<BaseResponse<bool>> UpdateDocuments(PanamaDocumentRequestDto request)
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

            string url = $"api/data/v9.2/incidents({request.PanamaId})";

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

    public async Task<BaseResponse<bool>> RemoveDocuments(PanamaRemoveDocumentRequestDto request)
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

            await _fileStorage.RemoveFile(request.FileUrl!, AzureContainers.DOCUMENTOS);

            var field = request.FieldName;

            var comentarioRecord = new JObject
            {
                [field!] = ""
            };

            string jsonContent = JsonConvert.SerializeObject(comentarioRecord);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"api/data/v9.2/incidents({request.PanamaId})";

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