using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.ControlInventario.Request;
using TrackX.Application.Dtos.ControlInventario.Response;

namespace TrackX.Application.Interfaces
{
    public interface IControlInventarioApplication
    {
        Task<BaseResponse<IEnumerable<ControlInventarioResponseDto>>> ListControlInventario(BaseFiltersRequest filters, string whs);
        Task<BaseResponse<IEnumerable<ControlInventarioResponseDto>>> ListControlInventarioCliente(BaseFiltersRequest filters, string cliente, string whs);
        Task<BaseResponse<ControlInventarioByIdResponseDto>> ControlInventarioById(int id);
        Task<BaseResponse<bool>> RegisterControlInventario(ControlInventarioRequestDto requestDto);
        Task<BaseResponse<bool>> EditControlInventario(int id, ControlInventarioRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveControlInventario(int id);
    }
}