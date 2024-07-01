using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pod.Request;
using TrackX.Application.Dtos.Pod.Response;

namespace TrackX.Application.Interfaces
{
    public interface IPodApplication
    {
        Task<BaseResponse<IEnumerable<PodResponseDto>>> LisPod(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectPod();
        Task<BaseResponse<PodResponseDto>> PodById(int id);
        Task<BaseResponse<bool>> RegisterPod(PodRequestDto requestDto);
        Task<BaseResponse<bool>> EditPod(int id, PodRequestDto requestDto);
        Task<BaseResponse<bool>> RemovePod(int id);
    }
}