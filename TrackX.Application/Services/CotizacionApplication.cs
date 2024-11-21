﻿using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class CotizacionApplication : ICotizacionApplication
{
    private readonly IClienteApplication _clienteApplication;
    private readonly ISecretService _secretService;
    private readonly HttpClient _httpClient;
    private readonly IFileStorageLocalApplication _fileStorage;

    public CotizacionApplication(
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

    [Obsolete]
    private async Task<string?> GetQuoteIdByQuoAsync(string Quo)
    {
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

            string entityName = "quotes";
            string requestUri = $"api/data/v9.2/{entityName}?$select=quoteid,quotenumber&$filter=quotenumber eq {Quo}";

            HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(requestUri);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse);

                return dynamicsObject?.value?.FirstOrDefault()?.quoteid;
            }
        }
        catch (Exception ex)
        {
            WatchLogger.Log($"Error en GetQuoIdByQuoAsync: {ex.Message}");
        }
        return null;
    }

    public async Task<BaseResponse<Dynamics<DynamicsCotizacion>>> ListCotizacion(int numFilter, string textFilter)
    {
        var response = new BaseResponse<Dynamics<DynamicsCotizacion>>();
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

            string entityName = "quotes";

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
                Dynamics<DynamicsCotizacion> apiResponse = JsonConvert.DeserializeObject<Dynamics<DynamicsCotizacion>>(jsonResponse)!;

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



    private static string BuildUrl(string entityName, int numFilter, string textFilter)
    {
        if (string.IsNullOrEmpty(textFilter))
        {
            numFilter = 0;
        }

        string permisosFilter = "(new_servicioalcliente eq true or new_clienteweb eq true or new_almacenfiscal eq true or new_consolidadoradecarga eq true)";

        string filter = numFilter switch
        {
            1 => $"_customerid_value eq '{textFilter}' and {permisosFilter}",
            2 => $"quotenumber eq '{textFilter}'", // and {permisosFilter}
            _ => $"{permisosFilter}"
        };

        return $"api/data/v9.2/{entityName}?$select=quoteid,_customerid_value,quotenumber,new_servicioalcliente,new_clienteweb,new_almacenfiscal,new_consolidadoradecarga,new_enlacecotizacion&filter={filter}";
    }

    [Obsolete]
    public async Task<BaseResponse<bool>> RegisterCotizacion(CotizacionRequestDto request)
    {
        var response = new BaseResponse<bool>();

        var Config = await GetConfigAsync();

        try
        {
            string? quoteId = await GetQuoteIdByQuoAsync(request.Quo!);
            if (quoteId == null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró el Quote ID en Dynamics para el QUO proporcionado.";
                return response;
            }

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

            var documento = await _fileStorage.SaveFile(AzureContainers.DOCUMENTOS, request.Cotizacion!);

            var comentarioRecord = new
            {
                new_enlacecotizacion = documento
            };

            string jsonContent = JsonConvert.SerializeObject(comentarioRecord);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"api/data/v9.2/quotes({quoteId})";

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