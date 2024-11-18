using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Application.Dtos.Cotizacion.Response;

namespace TrackX.Application.Interfaces;

public interface ICotizacionApplication
{
    Task<BaseResponse<IEnumerable<CotizacionResponseDto>>> ListCotizacion(BaseFiltersRequest filters);
    Task<BaseResponse<bool>> RegisterCotizacion(CotizacionRequestDto requestDto);
}