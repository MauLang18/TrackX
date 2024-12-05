using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Panama.Request;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface IPanamaApplication
{
    Task<BaseResponse<Dynamics<DynamicsPanama>>> ListPanama(int numFilter, string textFilter, int type);
    Task<BaseResponse<bool>> RegisterComentario(PanamaRequestDto request);
    Task<BaseResponse<bool>> UpdateDocuments(PanamaDocumentRequestDto request);
    Task<BaseResponse<bool>> RemoveDocuments(PanamaRemoveDocumentRequestDto request);
}