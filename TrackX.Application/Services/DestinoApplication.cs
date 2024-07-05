using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Destino.Request;
using TrackX.Application.Dtos.Destino.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class DestinoApplication : IDestinoApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IFileStorageLocalApplication _fileStorage;

    public DestinoApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _fileStorage = fileStorage;
    }

    public async Task<BaseResponse<IEnumerable<DestinoResponseDto>>> ListDestinos(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<DestinoResponseDto>>();
        try
        {
            var destinos = _unitOfWork.Destino
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        destinos = destinos.Where(x => x.Nombre!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                destinos = destinos.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                destinos = destinos.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, destinos, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await destinos.CountAsync();
            response.Data = _mapper.Map<IEnumerable<DestinoResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectDestino()
    {
        var response = new BaseResponse<IEnumerable<SelectResponse>>();

        try
        {
            var destinos = await _unitOfWork.Destino.GetSelectAsync();

            destinos = destinos
                .Where(x => !string.IsNullOrEmpty(x.Nombre))
                .GroupBy(x => x.Nombre)
                .Select(g => g.First())
                .OrderBy(x => x.Nombre)
                .ToList();

            if (destinos is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<SelectResponse>>(destinos);
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

    public async Task<BaseResponse<DestinoByIdResponseDto>> DestinoById(int id)
    {
        var response = new BaseResponse<DestinoByIdResponseDto>();
        try
        {
            var destino = await _unitOfWork.Destino.GetByIdAsync(id);

            if (destino is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<DestinoByIdResponseDto>(destino);
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

    public async Task<BaseResponse<bool>> RegisterDestino(DestinoRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var destino = _mapper.Map<TbDestino>(requestDto);

            if (requestDto.Imagen is not null)
                destino.Imagen = await _fileStorage.SaveFile(AzureContainers.DESTINO, requestDto.Imagen);

            response.Data = await _unitOfWork.Destino.RegisterAsync(destino);
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

    public async Task<BaseResponse<bool>> EditDestino(int id, DestinoRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var destinoEdit = await DestinoById(id);

            if (destinoEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var destino = _mapper.Map<TbDestino>(requestDto);
            destino.Id = id;

            if (requestDto.Imagen is not null)
                destino.Imagen = await _fileStorage
                    .EditFile(AzureContainers.DESTINO, requestDto.Imagen, destinoEdit.Data!.Imagen!);

            if (requestDto.Imagen is null)
                destino.Imagen = destinoEdit.Data!.Imagen;

            response.Data = await _unitOfWork.Destino.EditAsync(destino);

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

    public async Task<BaseResponse<bool>> RemoveDestino(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var destino = await DestinoById(id);

            if (destino.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Destino.RemoveAsync(id);

            await _fileStorage.RemoveFile(destino.Data!.Imagen!, AzureContainers.DESTINO);

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