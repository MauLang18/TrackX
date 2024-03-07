using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Rol.Request;
using TrackX.Application.Dtos.Rol.Response;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface IRolApplication
    {
        Task<BaseResponse<BaseEntityResponse<RolResponseDto>>> ListRoles(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<RolSelectResponseDto>>> ListSelectRol();
        Task<BaseResponse<RolResponseDto>> RolById(int id);
        Task<BaseResponse<bool>> RegisterRol(RolRequestDto requestDto);
        Task<BaseResponse<bool>> EditRol(int id, RolRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveRol(int id);
    }
}