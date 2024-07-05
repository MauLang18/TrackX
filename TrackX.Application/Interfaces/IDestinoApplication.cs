using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Destino.Request;
using TrackX.Application.Dtos.Destino.Response;

namespace TrackX.Application.Interfaces;

public interface IDestinoApplication
{
    Task<BaseResponse<IEnumerable<DestinoResponseDto>>> ListDestinos(BaseFiltersRequest filters);
    Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectDestino();
    Task<BaseResponse<DestinoByIdResponseDto>> DestinoById(int id);
    Task<BaseResponse<bool>> RegisterDestino(DestinoRequestDto requestDto);
    Task<BaseResponse<bool>> EditDestino(int id, DestinoRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveDestino(int id);
}