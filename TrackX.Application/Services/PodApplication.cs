using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pod.Request;
using TrackX.Application.Dtos.Pod.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services;

public class PodApplication : IPodApplication
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IOrderingQuery _orderingQuery;

    public PodApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _orderingQuery = orderingQuery;
    }

    public async Task<BaseResponse<IEnumerable<PodResponseDto>>> ListPod(BaseFiltersRequest filters)
    {
        var response = new BaseResponse<IEnumerable<PodResponseDto>>();
        try
        {
            var pod = _unitOfWork.Pod
                .GetAllQueryable()
                .AsQueryable();

            if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
            {
                switch (filters.NumFilter)
                {
                    case 1:
                        pod = pod.Where(x => x.Nombre!.Contains(filters.TextFilter));
                        break;
                }
            }

            if (filters.StateFilter is not null)
            {
                pod = pod.Where(x => x.Estado.Equals(filters.StateFilter));
            }

            if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
            {
                pod = pod.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                    && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                    .AddDays(1));
            }

            filters.Sort ??= "Id";

            var items = await _orderingQuery
                .Ordering(filters, pod, !(bool)filters.Download!).ToListAsync();

            response.IsSuccess = true;
            response.TotalRecords = await pod.CountAsync();
            response.Data = _mapper.Map<IEnumerable<PodResponseDto>>(items);
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

    public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectPod()
    {
        var response = new BaseResponse<IEnumerable<SelectResponse>>();

        try
        {
            var pod = await _unitOfWork.Pod.GetSelectAsync();

            pod = pod
                .Where(x => !string.IsNullOrEmpty(x.Nombre))
                .GroupBy(x => x.Nombre)
                .Select(g => g.First())
                .OrderBy(x => x.Nombre)
                .ToList();

            if (pod is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = _mapper.Map<IEnumerable<SelectResponse>>(pod);
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

    public async Task<BaseResponse<PodByIdResponseDto>> PodById(int id)
    {
        var response = new BaseResponse<PodByIdResponseDto>();
        try
        {
            var pod = await _unitOfWork.Pod.GetByIdAsync(id);

            if (pod is not null)
            {
                response.IsSuccess = true;
                response.Data = _mapper.Map<PodByIdResponseDto>(pod);
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

    public async Task<BaseResponse<bool>> RegisterPod(PodRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var pod = _mapper.Map<TbPod>(requestDto);

            response.Data = await _unitOfWork.Pod.RegisterAsync(pod);
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

    public async Task<BaseResponse<bool>> EditPod(int id, PodRequestDto requestDto)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var podEdit = await PodById(id);

            if (podEdit.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            var pod = _mapper.Map<TbPod>(requestDto);
            pod.Id = id;

            response.Data = await _unitOfWork.Pod.EditAsync(pod);

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

    public async Task<BaseResponse<bool>> RemovePod(int id)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var pod = await PodById(id);

            if (pod.Data is null)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.Data = await _unitOfWork.Pod.RemoveAsync(id);

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