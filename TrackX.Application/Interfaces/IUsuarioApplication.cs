using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;

namespace TrackX.Application.Interfaces;

public interface IUsuarioApplication
{
    Task<BaseResponse<IEnumerable<UsuarioResponseDto>>> ListUsuarios(BaseFiltersRequest filters);
    Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectUsuarios();
    Task<BaseResponse<UsuarioResponseDto>> UsuarioById(int id);
    Task<BaseResponse<bool>> RegisterUsuario(UsuarioRequestDto requestDto);
    Task<BaseResponse<bool>> EditUsuario(int id, UsuarioRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveUsuario(int id);
    Task<BaseResponse<bool>> ImportExcelUsuario(ImportRequest request);
}