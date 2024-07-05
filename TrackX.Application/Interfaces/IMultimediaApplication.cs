using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Multimedia.Request;
using TrackX.Application.Dtos.Multimedia.Response;

namespace TrackX.Application.Interfaces;

public interface IMultimediaApplication
{
    Task<BaseResponse<IEnumerable<MultimediaResponseDto>>> ListMultimedia(BaseFiltersRequest filters);
    Task<BaseResponse<IEnumerable<SelectResponse>>> ListMultimediaSelect();
    Task<BaseResponse<MultimediaByIdResponseDto>> MultimediaById(int id);
    Task<BaseResponse<bool>> RegisterMultimedia(MultimediaRequestDto request);
    Task<BaseResponse<bool>> EditMultimedia(int id, MultimediaRequestDto request);
    Task<BaseResponse<bool>> RemoveMultimedia(int id);
}