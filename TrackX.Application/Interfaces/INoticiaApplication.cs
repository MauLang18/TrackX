using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Dtos.Noticia.Response;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Interfaces
{
    public interface INoticiaApplication
    {
        Task<BaseResponse<BaseEntityResponse<NoticiaResponseDto>>> ListNoticias(BaseFiltersRequest filters);
        Task<BaseResponse<NoticiaByIdResponseDto>> NoticiaById(int id);
        Task<BaseResponse<bool>> RegisterNoticia(NoticiaRequestDto requestDto);
        Task<BaseResponse<bool>> EditNoticia(int id, NoticiaRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveNoticia(int id);
    }
}