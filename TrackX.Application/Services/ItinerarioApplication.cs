using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Dtos.Itinerario.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class ItinerarioApplication : IItinerarioApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFileStorageLocalApplication _fileStorage;

        public ItinerarioApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponse<IEnumerable<ItinerarioResponseDto>>> ListItinerarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ItinerarioResponseDto>>();
            try
            {
                var itinerarios = _unitOfWork.Itinerario
                    .GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            itinerarios = itinerarios.Where(x => x.POL!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            itinerarios = itinerarios.Where(x => x.POD!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    itinerarios = itinerarios.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    itinerarios = itinerarios.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, itinerarios, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await itinerarios.CountAsync();
                response.Data = _mapper.Map<IEnumerable<ItinerarioResponseDto>>(items);
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

        public async Task<BaseResponse<ItinerarioByIdResponseDto>> ItinerarioById(int id)
        {
            var response = new BaseResponse<ItinerarioByIdResponseDto>();
            try
            {
                var itinerario = await _unitOfWork.Itinerario.GetByIdAsync(id);

                if (itinerario is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<ItinerarioByIdResponseDto>(itinerario);
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

        public async Task<BaseResponse<bool>> RegisterItinerario(ItinerarioRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var itinerario = _mapper.Map<TbItinerario>(requestDto);

                if (requestDto.Origen is not null)
                    itinerario.Origen = await _fileStorage.SaveFile(AzureContainers.ITINERARIOS, requestDto.Origen);

                if (requestDto.Destino is not null)
                    itinerario.Destino = await _fileStorage.SaveFile(AzureContainers.ITINERARIOS, requestDto.Destino);

                response.Data = await _unitOfWork.Itinerario.RegisterAsync(itinerario);
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

        public async Task<BaseResponse<bool>> EditItinerario(int id, ItinerarioRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var itinerarioEdit = await ItinerarioById(id);

                if (itinerarioEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var itinerario = _mapper.Map<TbItinerario>(requestDto);
                itinerario.Id = id;

                if (requestDto.Origen is not null)
                    itinerario.Origen = await _fileStorage
                        .EditFile(AzureContainers.ITINERARIOS, requestDto.Origen, itinerarioEdit.Data!.Origen!);

                if (requestDto.Origen is null)
                    itinerario.Origen = itinerarioEdit.Data!.Origen;

                if (requestDto.Destino is not null)
                    itinerario.Destino = await _fileStorage
                        .EditFile(AzureContainers.ITINERARIOS, requestDto.Destino, itinerarioEdit.Data!.Destino!);

                if (requestDto.Destino is null)
                    itinerario.Destino = itinerarioEdit.Data!.Destino;

                response.Data = await _unitOfWork.Itinerario.EditAsync(itinerario);

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

        public async Task<BaseResponse<bool>> RemoveItinerario(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var itinerario = await ItinerarioById(id);

                if (itinerario.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Itinerario.RemoveAsync(id);

                await _fileStorage.RemoveFile(itinerario.Data!.Origen!, AzureContainers.ITINERARIOS);
                await _fileStorage.RemoveFile(itinerario.Data!.Destino!, AzureContainers.ITINERARIOS);

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