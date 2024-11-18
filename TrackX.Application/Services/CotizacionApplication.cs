using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Application.Dtos.Cotizacion.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class CotizacionApplication : ICotizacionApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClienteApplication _clienteApplication;
    private readonly IOrderingQuery _orderingQuery;
    private readonly ISecretService _secretService;
    private readonly IFileStorageLocalApplication _fileStorageLocalApplication;
    private readonly HttpClient _httpClient;

    public CotizacionApplication(IUnitOfWork unitOfWork, IMapper mapper, IClienteApplication clienteApplication, IOrderingQuery orderingQuery, ISecretService secretService, HttpClient httpClient, IFileStorageLocalApplication fileStorageLocalApplication)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _clienteApplication = clienteApplication;
        _orderingQuery = orderingQuery;
        _secretService = secretService;
        _httpClient = httpClient;
        _fileStorageLocalApplication = fileStorageLocalApplication;
    }

    public async Task<BaseResponse<IEnumerable<CotizacionResponseDto>>> ListCotizacion(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<CotizacionResponseDto>>();
        try
        {
            var Cotizacion = _unitOfWork.Cotizacion
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        Cotizacion = Cotizacion.Where(x => x.QUO!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        Cotizacion = Cotizacion.Where(x => x.NombreCliente!.Contains(filters.TextFilter!));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                Cotizacion = Cotizacion.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                Cotizacion = Cotizacion.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, Cotizacion, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await Cotizacion.CountAsync();
            response.Data = _mapper.Map<IEnumerable<CotizacionResponseDto>>(items);
            response.Message = ReplyMessage.MESSAGE_QUERY;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    [Obsolete]
    public async Task<BaseResponse<bool>> RegisterCotizacion(CotizacionRequestDto request)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var Cotizacion = _mapper.Map<TbCotizacion>(request);

            if (!string.IsNullOrEmpty(request.Cliente))
            {
                var shipperValuesList = new List<string> { request.Cliente! };
                var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

                foreach (var datos in nuevoValorCliente.Data!.value!)
                {
                    Cotizacion.NombreCliente = datos.name;
                }
            }

            await _unitOfWork.Cotizacion.RegisterAsync(Cotizacion);
            await _unitOfWork.SaveChangesAsync();

            if (request.Cotizacion is not null)
            {
                Cotizacion.Cotizacion = await _fileStorageLocalApplication.SaveFile(AzureContainers.COTIZACION, request.Cotizacion!);
            }

            string? quoteId = await GetQuoteIdByQUOAsync(request.Quo!);
            if (quoteId == null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró el Quote ID en Dynamics para el QUO proporcionado.";
                return response;
            }

            var Config = await GetConfigAsync();
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
                new_bcf = Cotizacion.Cotizacion
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
                response.Message = ReplyMessage.MESSAGE_SAVE;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_FAILED;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    private async Task<AuthenticationConfig?> GetConfigAsync()
    {
        var secretJson = await _secretService.GetSecret("TrackX/data/Authentication");
        var SecretResponse = JsonConvert.DeserializeObject<SecretResponse<AuthenticationConfig>>(secretJson);
        return SecretResponse?.Data?.Data;
    }

    [Obsolete]
    private async Task<string> GetAccessTokenAsync()
    {
        var Config = await GetConfigAsync();

        string clientId = Config!.ClientId!;
        string clientSecret = Config!.ClientSecret!;
        string authority = Config!.Authority!;
        string crmUrl = Config!.CrmUrl!;

        Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential credentials = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, clientSecret);
        var authContext = new AuthenticationContext(authority);
        var result = await authContext.AcquireTokenAsync(crmUrl, credentials);
        return result.AccessToken;
    }

    private async Task<HttpClient> ConfigureHttpClientAsync(string accessToken)
    {
        var Config = await GetConfigAsync(); // Espera el resultado aquí

        string crmUrl = Config!.CrmUrl!; // Ahora puedes acceder a CrmUrl

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(crmUrl),
            Timeout = TimeSpan.FromSeconds(300)
        };

        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
        httpClient.DefaultRequestHeaders.Add("OData-Version", "4.0");
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return httpClient;
    }

    [Obsolete]
    private async Task<string?> GetQuoteIdByQUOAsync(string QUO)
    {
        try
        {
            string accessToken = await GetAccessTokenAsync();
            using var httpClient = await ConfigureHttpClientAsync(accessToken);

            string entityName = "quotes";
            string requestUri = $"api/data/v9.2/{entityName}?$select=quoteid,title&$filter=contains(title,'{QUO}')";

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var dynamicsObject = JsonConvert.DeserializeObject<Dynamics<DynamicsTrackingNoLogin>>(jsonResponse);

                return dynamicsObject?.value?.FirstOrDefault()?.incidentid;
            }
        }
        catch (Exception ex)
        {
            WatchLogger.Log($"Error en GetQuoteIdByQUOAsync: {ex.Message}");
        }
        return null;
    }
}