using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Dtos.Empleo.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class EmpleoApplication : IEmpleoApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileStorageLocalApplication _fileStorageLocalApplication;
    private readonly IOrderingQuery _orderingQuery;

    public EmpleoApplication(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageLocalApplication fileStorageLocalApplication, IOrderingQuery orderingQuery)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileStorageLocalApplication = fileStorageLocalApplication;
        _orderingQuery = orderingQuery;
    }

    public async Task<BaseResponse<IEnumerable<EmpleoResponseDto>>> ListEmpleos(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<EmpleoResponseDto>>();
        try
        {
            var empleos = _unitOfWork.Empleo
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        empleos = empleos.Where(x => x.Titulo!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                empleos = empleos.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                empleos = empleos.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, empleos, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await empleos.CountAsync();
            response.Data = _mapper.Map<IEnumerable<EmpleoResponseDto>>(items);
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

    public async Task<BaseResponse<EmpleoByIdResponseDto>> EmpleoById(int id)
    {
        var response = new BaseResponse<EmpleoByIdResponseDto>();

        try
        {
            var empleo = await _unitOfWork.Empleo.GetByIdAsync(id);

            if (empleo is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<EmpleoByIdResponseDto>(empleo);
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

    public async Task<BaseResponse<bool>> RegisterEmpleo(EmpleoRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var empleo = _mapper.Map<TbEmpleo>(requestDto);

            if (requestDto.Imagen is not null)
                empleo.Imagen = await _fileStorageLocalApplication.SaveFile(AzureContainers.EMPLEOS, requestDto.Imagen);

            response.Data = await _unitOfWork.Empleo.RegisterAsync(empleo);

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

    public async Task<BaseResponse<bool>> EditEmpleo(int id, EmpleoRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var empleoEdit = await EmpleoById(id);

            if (empleoEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var empleo = _mapper.Map<TbEmpleo>(requestDto);
            empleo.Id = id;

            if (requestDto.Imagen is not null)
                empleo.Imagen = await _fileStorageLocalApplication
                    .EditFile(AzureContainers.EMPLEOS, requestDto.Imagen, empleoEdit.Data!.Imagen!);

            if (requestDto.Imagen is null)
                empleo.Imagen = empleoEdit.Data!.Imagen!;

            response.Data = await _unitOfWork.Empleo.EditAsync(empleo);

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

    public async Task<BaseResponse<bool>> RemoveEmpleo(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var empleo = await EmpleoById(id);

            if (empleo.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Empleo.RemoveAsync(id);

            await _fileStorageLocalApplication.RemoveFile(empleo.Data!.Imagen!, AzureContainers.EMPLEOS);

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