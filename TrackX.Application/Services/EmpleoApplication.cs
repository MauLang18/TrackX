using AutoMapper;
using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Dtos.Empleo.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;

namespace TrackX.Application.Services
{
    public class EmpleoApplication : IEmpleoApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageLocalApplication _fileStorageLocalApplication;

        public EmpleoApplication(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageLocalApplication fileStorageLocalApplication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageLocalApplication = fileStorageLocalApplication;
        }

        public async Task<BaseResponse<BaseEntityResponse<EmpleoResponseDto>>> ListEmpleos(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<EmpleoResponseDto>>();
            try
            {
                var empleos = await _unitOfWork.Empleo.ListEmpleos(filters);

                if (empleos is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<EmpleoResponseDto>>(empleos);
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
}