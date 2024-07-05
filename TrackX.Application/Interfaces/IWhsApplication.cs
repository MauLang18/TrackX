using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Whs.Request;
using TrackX.Application.Dtos.Whs.Response;

namespace TrackX.Application.Interfaces;

public interface IWhsApplication
{
    Task<BaseResponse<IEnumerable<WhsResponseDto>>> ListWhs(BaseFiltersRequest filters, string whs);
    Task<BaseResponse<IEnumerable<WhsResponseDto>>> ListWhsCliente(BaseFiltersRequest filters, string cliente, string whs);
    Task<BaseResponse<WhsResponseByIdDto>> WhsById(int id);
    Task<BaseResponse<bool>> RegisterWhs(WhsRequestDto requestDto);
    Task<BaseResponse<bool>> EditWhs(int id, WhsRequestDto requestDto);
    Task<BaseResponse<bool>> RemoveWhs(int id);
}