using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pol.Request;
using TrackX.Application.Dtos.Pol.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class PolApplication : IPolApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public PolApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<PolResponseDto>>> ListPol(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<PolResponseDto>>();
            try
            {
                var pol = _unitOfWork.Pol
                    .GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            pol = pol.Where(x => x.Nombre!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    pol = pol.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    pol = pol.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, pol, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await pol.CountAsync();
                response.Data = _mapper.Map<IEnumerable<PolResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectPol()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();

            try
            {
                var pol = await _unitOfWork.Pol.GetSelectAsync();

                pol = pol.Where(x => !string.IsNullOrEmpty(x.Nombre)).GroupBy(x => x.Nombre).Select(g => g.First());

                if (pol is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<SelectResponse>>(pol);
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

        public async Task<BaseResponse<PolByIdResponseDto>> PolById(int id)
        {
            var response = new BaseResponse<PolByIdResponseDto>();
            try
            {
                var pol = await _unitOfWork.Pol.GetByIdAsync(id);

                if (pol is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<PolByIdResponseDto>(pol);
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

        public async Task<BaseResponse<bool>> RegisterPol(PolRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var pol = _mapper.Map<TbPol>(requestDto);

                response.Data = await _unitOfWork.Pol.RegisterAsync(pol);
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

        public async Task<BaseResponse<bool>> EditPol(int id, PolRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var polEdit = await PolById(id);

                if (polEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var pol = _mapper.Map<TbPol>(requestDto);
                pol.Id = id;

                response.Data = await _unitOfWork.Pol.EditAsync(pol);

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

        public async Task<BaseResponse<bool>> RemovePol(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var pol = await PolById(id);

                if (pol.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Pol.RemoveAsync(id);

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