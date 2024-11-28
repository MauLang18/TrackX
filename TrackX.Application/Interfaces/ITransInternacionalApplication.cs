using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ITransInternacionalApplication
{
    Task<BaseResponse<Dynamics<DynamicsTransInternacional>>> ListTransInternacional(int numFilter, string textFilter);
    Task<BaseResponse<bool>> RegisterComentario(TransInternacionalRequestDto request);
    Task<BaseResponse<bool>> UpdateDocuments(TransInternacionalDocumentRequestDto request);
    Task<BaseResponse<bool>> RemoveDocuments(TransInternacionalRemoveDocumentRequestDto request);
}