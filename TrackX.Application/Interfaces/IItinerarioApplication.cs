using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Dtos.Itinerario.Response;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface IItinerarioApplication
    {
        Task<BaseResponse<BaseEntityResponse<ItinerarioResponseDto>>> ListItinerarios(BaseFiltersRequest filters);
        Task<BaseResponse<ItinerarioByIdResponseDto>> ItinerarioById(int id);
        Task<BaseResponse<bool>> RegisterItinerario(ItinerarioRequestDto requestDto);
        Task<BaseResponse<bool>> EditItinerario(int id, ItinerarioRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveItinerario(int id);
    }
}