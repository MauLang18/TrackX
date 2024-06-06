using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Logs.Request;
using TrackX.Application.Dtos.Logs.Response;

namespace TrackX.Application.Interfaces
{
    public interface ILogsApplication
    {
        Task<BaseResponse<IEnumerable<LogsResponseDto>>> ListLogs(BaseFiltersRequest filters);
        Task<BaseResponse<LogsByIdResponseDto>> LogById(int id);
        Task<BaseResponse<bool>> RegisterLog(LogsRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveLog(int id);
    }
}