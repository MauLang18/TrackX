using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Logs.Request;
using TrackX.Application.Dtos.Logs.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class LogsApplication : ILogsApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public LogsApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<LogsResponseDto>>> ListLogs(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<LogsResponseDto>>();
            try
            {
                var logs = _unitOfWork.Logs
                    .GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            logs = logs.Where(x => x.Usuario!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            logs = logs.Where(x => x.Modulo!.Contains(filters.TextFilter));
                            break;
                        case 3:
                            logs = logs.Where(x => x.TipoMetodo!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    logs = logs.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    logs = logs.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, logs, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await logs.CountAsync();
                response.Data = _mapper.Map<IEnumerable<LogsResponseDto>>(items);
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

        public async Task<BaseResponse<LogsByIdResponseDto>> LogById(int id)
        {
            var response = new BaseResponse<LogsByIdResponseDto>();
            try
            {
                var log = await _unitOfWork.Logs.GetByIdAsync(id);

                if (log is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<LogsByIdResponseDto>(log);
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

        public async Task<BaseResponse<bool>> RegisterLog(LogsRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var log = _mapper.Map<TbLogs>(requestDto);
                response.Data = await _unitOfWork.Logs.RegisterAsync(log);
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

        public async Task<BaseResponse<bool>> RemoveLog(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var log = await LogById(id);

                if (log.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Logs.RemoveAsync(id);

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
}