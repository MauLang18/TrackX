using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Rol.Request;
using TrackX.Application.Dtos.Rol.Response;

namespace TrackX.Application.Interfaces;

public interface IRolApplication
{
    Task<BaseResponse<IEnumerable<RolResponseDto>>> ListRoles(BaseFiltersRequest filters);
    Task<BaseResponse<IEnumerable<RolSelectResponseDto>>> ListSelectRol();
    Task<BaseResponse<RolResponseDto>> RolById(int id);
    Task<BaseResponse<bool>> RegisterRol(RolRequestDto requestDto);
    Task<BaseResponse<bool>> EditRol(int id, RolRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveRol(int id);
}