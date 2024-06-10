using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Ordering;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Persistences.Interfaces;
using TrackX.Utilities.Static;
using WatchDog;
using BC = BCrypt.Net.BCrypt;

namespace TrackX.Application.Services
{
    public class UsuarioApplication : IUsuarioApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileStorageLocalApplication _fileStorage;
        private readonly IOrderingQuery _orderingQuery;
        private readonly IClienteApplication _clienteApplication;

        public UsuarioApplication(IUnitOfWork unitOfWork, IMapper mapper, IFileStorageLocalApplication fileStorage, IOrderingQuery orderingQuery, IClienteApplication clienteApplication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorage = fileStorage;
            _orderingQuery = orderingQuery;
            _clienteApplication = clienteApplication;
        }

        public async Task<BaseResponse<IEnumerable<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<UsuarioResponseDto>>();
            try
            {
                var usuarios = _unitOfWork.Usuario
                    .GetAllQueryable()
                    .Include(x => x.IdRolNavigation)
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            usuarios = usuarios.Where(x => x.Correo!.Contains(filters.TextFilter));
                            break;
                        case 2:
                            usuarios = usuarios.Where(x => x.NombreEmpresa!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    usuarios = usuarios.Where(x => x.Estado.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    usuarios = usuarios.Where(x => x.FechaCreacionAuditoria >= Convert.ToDateTime(filters.StartDate)
                        && x.FechaCreacionAuditoria <= Convert.ToDateTime(filters.EndDate)
                        .AddDays(1));
                }

                filters.Sort ??= "Id";

                var items = await _orderingQuery
                    .Ordering(filters, usuarios, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await usuarios.CountAsync();
                response.Data = _mapper.Map<IEnumerable<UsuarioResponseDto>>(items);
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

        public async Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUsuarios()
        {
            var response = new BaseResponse<IEnumerable<SelectResponse>>();

            try
            {
                var usuarios = await _unitOfWork.Usuario.GetSelectAsync();

                usuarios = usuarios.Where(x => !string.IsNullOrEmpty(x.Cliente)).GroupBy(x => x.Cliente).Select(g => g.First());

                if (usuarios is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                usuarios = usuarios.Where(x => !string.IsNullOrEmpty(x.NombreEmpresa) && x.NombreEmpresa != "CustomCodeCR");

                response.IsSuccess = true;
                response.Data = _mapper.Map<IEnumerable<SelectResponse>>(usuarios);
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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
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

                if (requestDto.Imagen is not null)
                    account.Imagen = await _fileStorage.SaveFile(AzureContainers.USUARIOS, requestDto.Imagen);

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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
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

                if (requestDto.Pass is not null)
                    usuario.Pass = BC.HashPassword(requestDto.Pass);

                if (requestDto.Pass is null)
                    usuario.Pass = usuarioEdit.Data!.Pass!;

                if (requestDto.Imagen is not null)
                    usuario.Imagen = await _fileStorage
                        .EditFile(AzureContainers.USUARIOS, requestDto.Imagen, usuarioEdit.Data!.Imagen!);

                if (requestDto.Imagen is null)
                    usuario.Imagen = usuarioEdit.Data!.Imagen;

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
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
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

                await _fileStorage.RemoveFile(usuario.Data!.Imagen!, AzureContainers.USUARIOS);

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