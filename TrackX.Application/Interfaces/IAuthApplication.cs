using TrackX.Application.Commons.Bases;
using TrackX.Application.Dtos.Usuario.Request;

namespace TrackX.Application.Interfaces
{
    public interface IAuthApplication
    {
        Task<BaseResponse<string>> Login(TokenRequestDto requestDto, string authType);
        Task<BaseResponse<string>> LoginWithGoogle(string credentials, string authType);
    }
}