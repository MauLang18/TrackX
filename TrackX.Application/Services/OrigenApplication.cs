using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Origen.Request;
using TrackX.Application.Dtos.Origen.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class OrigenApplication : IOrigenApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IFileStorageLocalApplication _fileStorage;

        public OrigenApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery, IFileStorageLocalApplication fileStorage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
            _fileStorage = fileStorage;
        }

        public async Task<BaseResponse<IEnumerable<OrigenResponseDto>>> ListOrigenes(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<OrigenResponseDto>>();
            try
            {
                var origenes = _unitOfWork.Origen
                    .GetAllQueryable()
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            origenes = origenes.Where(x => x.Nombre!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    origenes = origenes.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    origenes = origenes.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, origenes, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await origenes.CountAsync();
                response.Data = _mapper.Map<IEnumerable<OrigenResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectOrigen()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();

            try
            {
                var origenes = await _unitOfWork.Origen.GetSelectAsync();

                origenes = origenes
                    .Where(x => !string.IsNullOrEmpty(x.Nombre))
                    .GroupBy(x => x.Nombre)
                    .Select(g => g.First())
                    .OrderBy(x => x.Nombre)
                    .ToList();

                if (origenes is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<SelectResponse>>(origenes);
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

        public async Task<BaseResponse<OrigenByIdResponseDto>> OrigenById(int id)
        {
            var response = new BaseResponse<OrigenByIdResponseDto>();
            try
            {
                var origen = await _unitOfWork.Origen.GetByIdAsync(id);

                if (origen is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<OrigenByIdResponseDto>(origen);
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

        public async Task<BaseResponse<bool>> RegisterOrigen(OrigenRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var origen = _mapper.Map<TbOrigen>(requestDto);

                if (requestDto.Imagen is not null)
                    origen.Imagen = await _fileStorage.SaveFile(AzureContainers.ORIGEN, requestDto.Imagen);

                response.Data = await _unitOfWork.Origen.RegisterAsync(origen);
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

        public async Task<BaseResponse<bool>> EditOrigen(int id, OrigenRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var origenEdit = await OrigenById(id);

                if (origenEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var origen = _mapper.Map<TbOrigen>(requestDto);
                origen.Id = id;

                if (requestDto.Imagen is not null)
                    origen.Imagen = await _fileStorage
                        .EditFile(AzureContainers.ORIGEN, requestDto.Imagen, origenEdit.Data!.Imagen!);

                if (requestDto.Imagen is null)
                    origen.Imagen = origenEdit.Data!.Imagen;

                response.Data = await _unitOfWork.Origen.EditAsync(origen);

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

        public async Task<BaseResponse<bool>> RemoveOrigen(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var origen = await OrigenById(id);

                if (origen.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Origen.RemoveAsync(id);

                await _fileStorage.RemoveFile(origen.Data!.Imagen!, AzureContainers.ORIGEN);

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