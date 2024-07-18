using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Exoneracion.Request;
using TrackX.Application.Dtos.Exoneracion.Response;

namespace TrackX.Application.Interfaces;

public interface IExoneracionApplication
{
    Task<BaseResponse<IEnumerable<ExoneracionResponseDto>>> ListExoneracion(BaseFiltersRequest filters);
    Task<BaseResponse<IEnumerable<ExoneracionResponseDto>>> ListExoneracionCliente(BaseFiltersRequest filters, string cliente);
    Task<BaseResponse<ExoneracionByIdResponseDto>> ExoneracionById(int id);
    Task<BaseResponse<bool>> RegisterExoneracion(ExoneracionRequestDto requestDto);
    Task<BaseResponse<bool>> EditExoneracion(int id, ExoneracionRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveExoneracion(int id);
    Task<BaseResponse<bool>> ImportExcelExoneracion(ImportRequest request);
}