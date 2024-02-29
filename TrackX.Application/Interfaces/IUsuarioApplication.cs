using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface IUsuarioApplication
    {
        Task<BaseResponse<BaseEntityResponse<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters);
        Task<BaseResponse<UsuarioResponseDto>> UsuarioById(int id);
        Task<BaseResponse<bool>> RegisterUsuario(UsuarioRequestDto requestDto);
        Task<BaseResponse<bool>> EditUsuario(int id, UsuarioRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveUsuario(int id);
    }
}