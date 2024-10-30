using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Bcf.Request;
using TrackX.Application.Dtos.Bcf.Response;

namespace TrackX.Application.Interfaces;

public interface IBcfApplication
{
    Task<BaseResponse<IEnumerable<BcfResponseDto>>> ListBcf(BaseFiltersRequest filters);
    Task<BaseResponse<bool>> RegisterBcf(BcfRequestDto requestDto);
}