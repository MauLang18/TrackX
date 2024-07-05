using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Multimedia.Request;
using TrackX.Application.Dtos.Multimedia.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class MultimediaApplication : IMultimediaApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;
    private readonly IFileStorageLocalApplication _fileStorage;

    public MultimediaApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
        _fileStorage = fileStorage;
    }

    public async Task<BaseResponse<IEnumerable<MultimediaResponseDto>>> ListMultimedia(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<MultimediaResponseDto>>();
        try
        {
            var multimedias = _unitOfWork.Multimedia
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        multimedias = multimedias.Where(x => x.Nombre!.Contains(filters.TextFilter!));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                multimedias = multimedias.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                multimedias = multimedias.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, multimedias, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await multimedias.CountAsync();
            response.Data = _mapper.Map<IEnumerable<MultimediaResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListMultimediaSelect()
    {
        var response = new BaseResponse<IEnumerable<SelectResponse>>();

        try
        {
            var multimedias = await _unitOfWork.Multimedia.GetSelectAsync();

            if (multimedias is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<SelectResponse>>(multimedias);
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

    public async Task<BaseResponse<MultimediaByIdResponseDto>> MultimediaById(int id)
    {
        var response = new BaseResponse<MultimediaByIdResponseDto>();
        try
        {
            var multimedia = await _unitOfWork.Multimedia.GetByIdAsync(id);

            if (multimedia is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<MultimediaByIdResponseDto>(multimedia);
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

    public async Task<BaseResponse<bool>> RegisterMultimedia(MultimediaRequestDto request)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var multimedia = _mapper.Map<TbMultimedia>(request);

            if (request.Multimedia is not null)
                multimedia.Multimedia = await _fileStorage.SaveFile(AzureContainers.MULTIMEDIA, request.Multimedia);

            response.Data = await _unitOfWork.Multimedia.RegisterAsync(multimedia);

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

    public async Task<BaseResponse<bool>> EditMultimedia(int id, MultimediaRequestDto request)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var multimediaEdit = await MultimediaById(id);

            if (multimediaEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var multimedia = _mapper.Map<TbMultimedia>(request);
            multimedia.Id = id;

            if (request.Multimedia is not null)
                multimedia.Multimedia = await _fileStorage
                    .EditFile(AzureContainers.MULTIMEDIA, request.Multimedia, multimediaEdit.Data!.Multimedia!);

            if (request.Multimedia is null)
                multimedia.Multimedia = multimediaEdit.Data!.Multimedia;

            response.Data = await _unitOfWork.Multimedia.EditAsync(multimedia);

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

    public async Task<BaseResponse<bool>> RemoveMultimedia(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var multimedia = await MultimediaById(id);

            if (multimedia.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Multimedia.RemoveAsync(id);

            await _fileStorage.RemoveFile(multimedia.Data!.Multimedia!, AzureContainers.MULTIMEDIA);

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