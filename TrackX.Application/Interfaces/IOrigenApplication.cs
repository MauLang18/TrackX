using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Origen.Request;
using TrackX.Application.Dtos.Origen.Response;

namespace TrackX.Application.Interfaces
{
    public interface IOrigenApplication
    {
        Task<BaseResponse<IEnumerable<OrigenResponseDto>>> ListOrigenes(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectOrigen();
        Task<BaseResponse<OrigenResponseDto>> OrigenById(int id);
        Task<BaseResponse<bool>> RegisterOrigen(OrigenRequestDto requestDto);
        Task<BaseResponse<bool>> EditOrigen(int id, OrigenRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveOrigen(int id);
    }
}