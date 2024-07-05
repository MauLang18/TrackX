using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Exoneracion.Request;
using TrackX.Application.Dtos.Exoneracion.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class ExoneracionApplication : IExoneracionApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IFileStorageLocalApplication _fileStorage;
    private readonly IClienteApplication _clienteApplication;

    public ExoneracionApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage, IClienteApplication clienteApplication)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _fileStorage = fileStorage;
        _clienteApplication = clienteApplication;
    }

    public async Task<BaseResponse<IEnumerable<ExoneracionResponseDto>>> ListExoneracion(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<ExoneracionResponseDto>>();
        try
        {
            var Exoneracion = _unitOfWork.Exoneracion
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        Exoneracion = Exoneracion.Where(x => x.StatusExoneracion!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        Exoneracion = Exoneracion.Where(x => x.ClasificacionArancelaria!.Contains(filters.TextFilter));
                        break;
                    case 3:
                        Exoneracion = Exoneracion.Where(x => x.Categoria!.Contains(filters.TextFilter));
                        break;
                    case 4:
                        Exoneracion = Exoneracion.Where(x => x.Idtra!.Contains(filters.TextFilter));
                        break;
                    case 5:
                        Exoneracion = Exoneracion.Where(x => x.Producto!.Contains(filters.TextFilter));
                        break;
                    case 6:
                        Exoneracion = Exoneracion.Where(x => x.TipoExoneracion!.Contains(filters.TextFilter));
                        break;
                    case 7:
                        Exoneracion = Exoneracion.Where(x => x.NombreCliente!.Contains(filters.TextFilter!));
                        break;

                }
            }

            if (filters.StateFilter is not null)
            {
                Exoneracion = Exoneracion.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                Exoneracion = Exoneracion.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, Exoneracion, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await Exoneracion.CountAsync();
            response.Data = _mapper.Map<IEnumerable<ExoneracionResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<ExoneracionResponseDto>>> ListExoneracionCliente(BaseFiltersRequest filters, string cliente)
    {
        var response = new BaseResponse<IEnumerable<ExoneracionResponseDto>>();
        try
        {
            var Exoneracion = _unitOfWork.Exoneracion
                .GetAllQueryable()
                .AsQueryable();


            Exoneracion = Exoneracion.Where(x => x.Cliente!.Equals(cliente));

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        Exoneracion = Exoneracion.Where(x => x.StatusExoneracion!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                Exoneracion = Exoneracion.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                Exoneracion = Exoneracion.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, Exoneracion, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await Exoneracion.CountAsync();
            response.Data = _mapper.Map<IEnumerable<ExoneracionResponseDto>>(items);
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

    public async Task<BaseResponse<ExoneracionByIdResponseDto>> ExoneracionById(int id)
    {
        var response = new BaseResponse<ExoneracionByIdResponseDto>();
        try
        {
            var Exoneracion = await _unitOfWork.Exoneracion.GetByIdAsync(id);

            if (Exoneracion is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<ExoneracionByIdResponseDto>(Exoneracion);
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

    public async Task<BaseResponse<bool>> RegisterExoneracion(ExoneracionRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var Exoneracion = _mapper.Map<TbExoneracion>(requestDto);

            if (requestDto.Solicitud is not null)
                Exoneracion.Solicitud = await _fileStorage.SaveFile(AzureContainers.EXONERACIONES, requestDto.Solicitud);

            if (requestDto.Autorizacion is not null)
                Exoneracion.Autorizacion = await _fileStorage.SaveFile(AzureContainers.EXONERACIONES, requestDto.Autorizacion);

            var nuevoValorCliente = await _clienteApplication.NombreCliente(requestDto.Cliente!);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                Exoneracion.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.Exoneracion.RegisterAsync(Exoneracion);
            if (response.Data)
            {
                response.IsSuccess = true;
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

    public async Task<BaseResponse<bool>> EditExoneracion(int id, ExoneracionRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var ExoneracionEdit = await ExoneracionById(id);

            if (ExoneracionEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var Exoneracion = _mapper.Map<TbExoneracion>(requestDto);
            Exoneracion.Id = id;

            if (requestDto.Solicitud is not null)
                Exoneracion.Solicitud = await _fileStorage
                    .EditFile(AzureContainers.EXONERACIONES, requestDto.Solicitud, ExoneracionEdit.Data!.Solicitud!);

            if (requestDto.Solicitud is null)
                Exoneracion.Solicitud = ExoneracionEdit.Data!.Solicitud!;

            if (requestDto.Autorizacion is not null)
                Exoneracion.Autorizacion = await _fileStorage
                    .EditFile(AzureContainers.EXONERACIONES, requestDto.Autorizacion, ExoneracionEdit.Data!.Autorizacion!);

            if (requestDto.Autorizacion is null)
                Exoneracion.Autorizacion = ExoneracionEdit.Data!.Autorizacion!;

            var nuevoValorCliente = await _clienteApplication.NombreCliente(requestDto.Cliente!);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                Exoneracion.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.Exoneracion.EditAsync(Exoneracion);

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

    public async Task<BaseResponse<bool>> RemoveExoneracion(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var Exoneracion = await ExoneracionById(id);

            if (Exoneracion.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Exoneracion.RemoveAsync(id);

            await _fileStorage.RemoveFile(Exoneracion.Data!.Solicitud!, AzureContainers.EXONERACIONES);
            await _fileStorage.RemoveFile(Exoneracion.Data!.Autorizacion!, AzureContainers.EXONERACIONES);

            if (response.Data)
            {
                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;
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
}