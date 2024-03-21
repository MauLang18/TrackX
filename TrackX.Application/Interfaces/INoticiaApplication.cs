using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Dtos.Noticia.Response;

namespace TrackX.Application.Interfaces
{
    public interface INoticiaApplication
    {
        Task<BaseResponse<IEnumerable<NoticiaResponseDto>>> ListNoticias(BaseFiltersRequest filters);
        Task<BaseResponse<NoticiaByIdResponseDto>> NoticiaById(int id);
        Task<BaseResponse<bool>> RegisterNoticia(NoticiaRequestDto requestDto);
        Task<BaseResponse<bool>> EditNoticia(int id, NoticiaRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveNoticia(int id);
    }
}