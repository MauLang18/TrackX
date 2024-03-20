using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Dtos.Empleo.Response;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface IEmpleoApplication
    {
        Task<BaseResponse<BaseEntityResponse<EmpleoResponseDto>>> ListEmpleos(BaseFiltersRequest filters);
        Task<BaseResponse<EmpleoByIdResponseDto>> EmpleoById(int id);
        Task<BaseResponse<bool>> RegisterEmpleo(EmpleoRequestDto requestDto);
        Task<BaseResponse<bool>> EditEmpleo(int id, EmpleoRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveEmpleo(int id);
    }
}