using AutoMapper;
using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Dtos.Noticia.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Application.Services
{
    public class NoticiaApplication : INoticiaApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageLocalApplication _fileStorageLocalApplication;

        public NoticiaApplication(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageLocalApplication fileStorageLocalApplication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageLocalApplication = fileStorageLocalApplication;
        }

        public async Task<BaseResponse<BaseEntityResponse<NoticiaResponseDto>>> ListNoticias(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<NoticiaResponseDto>>();
            try
            {
                var noticias = await _unitOfWork.Noticia.ListNoticias(filters);

                if (noticias is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<NoticiaResponseDto>>(noticias);
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

        public async Task<BaseResponse<NoticiaByIdResponseDto>> NoticiaById(int id)
        {
            var response = new BaseResponse<NoticiaByIdResponseDto>();

            try
            {
                var noticia = await _unitOfWork.Noticia.GetByIdAsync(id);

                if (noticia is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<NoticiaByIdResponseDto>(noticia);
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

        public async Task<BaseResponse<bool>> RegisterNoticia(NoticiaRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var noticia = _mapper.Map<TbNoticia>(requestDto);

                if (requestDto.Imagen is not null)
                    noticia.Imagen = await _fileStorageLocalApplication.SaveFile(AzureContainers.NOTICIAS, requestDto.Imagen);

                response.Data = await _unitOfWork.Noticia.RegisterAsync(noticia);

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

        public async Task<BaseResponse<bool>> EditNoticia(int id, NoticiaRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var noticiaEdit = await NoticiaById(id);

                if (noticiaEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var noticia = _mapper.Map<TbNoticia>(requestDto);
                noticia.Id = id;

                if (requestDto.Imagen is not null)
                    noticia.Imagen = await _fileStorageLocalApplication
                        .EditFile(AzureContainers.NOTICIAS, requestDto.Imagen, noticiaEdit.Data!.Imagen!);

                if (requestDto.Imagen is null)
                    noticia.Imagen = noticiaEdit.Data!.Imagen!;

                response.Data = await _unitOfWork.Noticia.EditAsync(noticia);

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

        public async Task<BaseResponse<bool>> RemoveNoticia(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var noticia = await NoticiaById(id);

                if (noticia.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Noticia.RemoveAsync(id);

                await _fileStorageLocalApplication.RemoveFile(noticia.Data!.Imagen!, AzureContainers.NOTICIAS);

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