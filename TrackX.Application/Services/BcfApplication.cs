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
using TrackX.Application.Dtos.Bcf.Request;
using TrackX.Application.Dtos.Bcf.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Infrastructure.Secret;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class BcfApplication : IBcfApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IClienteApplication _clienteApplication;
    private readonly IOrderingQuery _orderingQuery;
    private readonly ISecretService _secretService;
    private readonly HttpClient _httpClient;

    public BcfApplication(IUnitOfWork unitOfWork, IMapper mapper, IClienteApplication clienteApplication, IOrderingQuery orderingQuery, ISecretService secretService, HttpClient httpClient)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _clienteApplication = clienteApplication;
        _orderingQuery = orderingQuery;
        _secretService = secretService;
        _httpClient = httpClient;
    }

    public async Task<BaseResponse<IEnumerable<BcfResponseDto>>> ListBcf(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<BcfResponseDto>>();
        try
        {
            var Bcf = _unitOfWork.Bcf
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        Bcf = Bcf.Where(x => x.IDTRA!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        Bcf = Bcf.Where(x => x.NombreCliente!.Contains(filters.TextFilter!));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                Bcf = Bcf.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                Bcf = Bcf.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, Bcf, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await Bcf.CountAsync();
            response.Data = _mapper.Map<IEnumerable<BcfResponseDto>>(items);
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

    public async Task<BaseResponse<BcfByIdResponseDto>> BcfById(int id)
    {
        var response = new BaseResponse<BcfByIdResponseDto>();
        try
        {
            var Bcf = await _unitOfWork.Bcf.GetByIdAsync(id);

            if (Bcf is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<BcfByIdResponseDto>(Bcf);
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
            response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            WatchLogger.Log(ex.Message);
        }

        return response;
    }

    [Obsolete]
    public async Task<BaseResponse<bool>> RegisterBcf(BcfRequestDto request)
    {
        var response = new BaseResponse<bool>();

        try
        {
            string? incidentId = await GetIncidentIdByIDTRAAsync(request.Idtra!);
            if (incidentId == null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontró el Incident ID en Dynamics para el IDTRA proporcionado.";
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

            var lastBcf = await _unitOfWork.Bcf.GetLastBcfAsync();
            var newBcfCode = GenerateNextBcfCode(lastBcf!);

            var comentarioRecord = new
            {
                new_bcf = newBcfCode
            };

            string jsonContent = JsonConvert.SerializeObject(comentarioRecord);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string url = $"api/data/v9.2/incidents({incidentId})";

            var requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = content
            };

            HttpResponseMessage httpResponseMessage = await _httpClient.SendAsync(requestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var Bcf = _mapper.Map<TbBcf>(request);
                Bcf.BCF = newBcfCode;

                if (request.Cliente is not null)
                {
                    var shipperValuesList = new List<string> { request.Cliente! };
                    var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

                    foreach (var datos in nuevoValorCliente.Data!.value!)
                    {
                        Bcf.NombreCliente = datos.name;
                    }
                }

                await _unitOfWork.Bcf.RegisterAsync(Bcf);
                await _unitOfWork.SaveChangesAsync();

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

    public async Task<BaseResponse<bool>> EditBcf(int id, BcfRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var BcfEdit = await BcfById(id);

            if (BcfEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var Bcf = _mapper.Map<TbBcf>(requestDto);
            Bcf.Id = id;

            var shipperValuesList = new List<string> { requestDto.Cliente! };
            var nuevoValorCliente = await _clienteApplication.NombreCliente(shipperValuesList);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                Bcf.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.Bcf.EditAsync(Bcf);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_UPDATE;
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
    private async Task<string?> GetIncidentIdByIDTRAAsync(string IDTRA)
    {
        try
        {
            string accessToken = await GetAccessTokenAsync();
            using var httpClient = await ConfigureHttpClientAsync(accessToken);

            string entityName = "incidents";
            string requestUri = $"api/data/v9.2/{entityName}?$select=incidentid,title&$filter=contains(title,'{IDTRA}')";

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
            WatchLogger.Log($"Error en GetIncidentIdByIDTRAAsync: {ex.Message}");
        }
        return null;
    }

    private string GenerateNextBcfCode(string lastBcf)
    {
        var currentYear = DateTime.Now.Year.ToString();

        if (lastBcf != null && lastBcf.StartsWith("GCF" + currentYear))
        {
            var lastConsecutive = int.Parse(lastBcf.Substring(9));
            var nextConsecutive = lastConsecutive + 1;
            return $"GCF{currentYear}-{nextConsecutive:D6}";
        }

        return $"GCF{currentYear}-000001";
    }
}