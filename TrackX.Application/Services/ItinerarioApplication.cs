using AutoMapper;
using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Dtos.Itinerario.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Application.Services
{
    public class ItinerarioApplication : IItinerarioApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ItinerarioApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<ItinerarioResponseDto>>> ListItinerarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<ItinerarioResponseDto>>();
            try
            {
                var itinerarios = await _unitOfWork.Itinerario.ListItinerarios(filters);

                if (itinerarios is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<ItinerarioResponseDto>>(itinerarios);
                    response.Message = ReplyMessage.MESSAGE_QUERY;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
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
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            }

            return response;
        }

        public async Task<BaseResponse<bool>> RegisterItinerario(ItinerarioRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var itinerario = _mapper.Map<TbItinerario>(requestDto);
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
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
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
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
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
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
            }

            return response;
        }
    }
}