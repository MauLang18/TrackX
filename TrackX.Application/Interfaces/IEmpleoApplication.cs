using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Dtos.Empleo.Response;

namespace TrackX.Application.Interfaces;

public interface IEmpleoApplication
{
    Task<BaseResponse<IEnumerable<EmpleoResponseDto>>> ListEmpleos(BaseFiltersRequest filters);
    Task<BaseResponse<EmpleoByIdResponseDto>> EmpleoById(int id);
    Task<BaseResponse<bool>> RegisterEmpleo(EmpleoRequestDto requestDto);
    Task<BaseResponse<bool>> EditEmpleo(int id, EmpleoRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveEmpleo(int id);
}