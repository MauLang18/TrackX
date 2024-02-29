using AutoMapper;
using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using BC = BCrypt.Net.BCrypt;

namespace TrackX.Application.Services
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioApplication(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BaseEntityResponse<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<BaseEntityResponse<UsuarioResponseDto>>();
            try
            {
                var usuarios = await _unitOfWork.Usuario.ListUsuarios(filters);

                if (usuarios is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<BaseEntityResponse<UsuarioResponseDto>>(usuarios);
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

        public async Task<BaseResponse<UsuarioResponseDto>> UsuarioById(int id)
        {
            var response = new BaseResponse<UsuarioResponseDto>();

            try
            {
                var usuario = await _unitOfWork.Usuario.GetByIdAsync(id);

                if (usuario is not null)
                {
                    response.IsSuccess = true;
                    response.Data = _mapper.Map<UsuarioResponseDto>(usuario);
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

        public async Task<BaseResponse<bool>> RegisterUsuario(UsuarioRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var account = _mapper.Map<TbUsuario>(requestDto);
                account.Pass = BC.HashPassword(account.Pass);

                response.Data = await _unitOfWork.Usuario.RegisterAsync(account);

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

        public async Task<BaseResponse<bool>> EditUsuario(int id, UsuarioRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var usuarioEdit = await UsuarioById(id);

                if (usuarioEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var usuario = _mapper.Map<TbUsuario>(requestDto);
                usuario.Id = id;
                usuario.Pass = BC.HashPassword(usuario.Pass);

                response.Data = await _unitOfWork.Usuario.EditAsync(usuario);

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

        public async Task<BaseResponse<bool>> RemoveUsuario(int id)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var usuario = await UsuarioById(id);

                if (usuario.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Usuario.RemoveAsync(id);

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