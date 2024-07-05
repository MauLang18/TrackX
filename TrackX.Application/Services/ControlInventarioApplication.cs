using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.ControlInventario.Request;
using TrackX.Application.Dtos.ControlInventario.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class ControlInventarioApplication : IControlInventarioApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IFileStorageLocalApplication _fileStorage;
    private readonly IClienteApplication _clienteApplication;

    public ControlInventarioApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage, IClienteApplication clienteApplication)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _fileStorage = fileStorage;
        _clienteApplication = clienteApplication;
    }

    public async Task<BaseResponse<IEnumerable<ControlInventarioResponseDto>>> ListControlInventario(BaseFiltersRequest filters, string whs)
    {
        var response = new BaseResponse<IEnumerable<ControlInventarioResponseDto>>();
        try
        {
            var ControlInventario = _unitOfWork.ControlInventario
                .GetAllQueryable()
                .AsQueryable();

            ControlInventario = ControlInventario.Where(x => x.Pol!.Contains(whs));

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        ControlInventario = ControlInventario.Where(x => x.NombreCliente!.Contains(filters.TextFilter));
                        break;
                    case 2:
                        ControlInventario = ControlInventario.Where(x => x.Pol!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                ControlInventario = ControlInventario.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                ControlInventario = ControlInventario.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, ControlInventario, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await ControlInventario.CountAsync();
            response.Data = _mapper.Map<IEnumerable<ControlInventarioResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<ControlInventarioResponseDto>>> ListControlInventarioCliente(BaseFiltersRequest filters, string cliente, string whs)
    {
        var response = new BaseResponse<IEnumerable<ControlInventarioResponseDto>>();
        try
        {
            var ControlInventario = _unitOfWork.ControlInventario
                .GetAllQueryable()
                .AsQueryable();


            ControlInventario = ControlInventario.Where(x => x.Pol!.Contains(whs) && x.Cliente!.Equals(cliente));

            if (filters.StateFilter is not null)
            {
                ControlInventario = ControlInventario.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                ControlInventario = ControlInventario.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, ControlInventario, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await ControlInventario.CountAsync();
            response.Data = _mapper.Map<IEnumerable<ControlInventarioResponseDto>>(items);
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

    public async Task<BaseResponse<ControlInventarioByIdResponseDto>> ControlInventarioById(int id)
    {
        var response = new BaseResponse<ControlInventarioByIdResponseDto>();
        try
        {
            var ControlInventario = await _unitOfWork.ControlInventario.GetByIdAsync(id);

            if (ControlInventario is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<ControlInventarioByIdResponseDto>(ControlInventario);
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

    public async Task<BaseResponse<bool>> RegisterControlInventario(ControlInventarioRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var ControlInventario = _mapper.Map<TbControlInventarioWhs>(requestDto);

            if (requestDto.ControlInventario is not null)
                ControlInventario.ControlInventario = await _fileStorage.SaveFile(AzureContainers.WHS, requestDto.ControlInventario);

            var nuevoValorCliente = await _clienteApplication.NombreCliente(requestDto.Cliente!);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                ControlInventario.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.ControlInventario.RegisterAsync(ControlInventario);
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

    public async Task<BaseResponse<bool>> EditControlInventario(int id, ControlInventarioRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var ControlInventarioEdit = await ControlInventarioById(id);

            if (ControlInventarioEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var ControlInventario = _mapper.Map<TbControlInventarioWhs>(requestDto);
            ControlInventario.Id = id;

            if (requestDto.ControlInventario is not null)
                ControlInventario.ControlInventario = await _fileStorage
                    .EditFile(AzureContainers.WHS, requestDto.ControlInventario, ControlInventarioEdit.Data!.ControlInventario!);

            if (requestDto.ControlInventario is null)
                ControlInventario.ControlInventario = ControlInventarioEdit.Data!.ControlInventario!;

            var nuevoValorCliente = await _clienteApplication.NombreCliente(requestDto.Cliente!);

            foreach (var datos in nuevoValorCliente.Data!.value!)
            {
                ControlInventario.NombreCliente = datos.name;
            }

            response.Data = await _unitOfWork.ControlInventario.EditAsync(ControlInventario);

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

    public async Task<BaseResponse<bool>> RemoveControlInventario(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var ControlInventario = await ControlInventarioById(id);

            if (ControlInventario.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.ControlInventario.RemoveAsync(id);

            await _fileStorage.RemoveFile(ControlInventario.Data!.ControlInventario!, AzureContainers.WHS);

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