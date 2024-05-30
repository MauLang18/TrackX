using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Dtos.Itinerario.Response;

namespace TrackX.Application.Interfaces
{
    public interface IItinerarioApplication
    {
        Task<BaseResponse<IEnumerable<ItinerarioResponseDto>>> ListItinerarios(BaseFiltersItinerarioRequest filters);
        Task<BaseResponse<ItinerarioByIdResponseDto>> ItinerarioById(int id);
        Task<BaseResponse<bool>> RegisterItinerario(ItinerarioRequestDto requestDto);
        Task<BaseResponse<bool>> EditItinerario(int id, ItinerarioRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveItinerario(int id);
    }
}