using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Finance.Request;
using TrackX.Application.Dtos.Finance.Response;

namespace TrackX.Application.Interfaces
{
    public interface IFinanceApplication
    {
        Task<BaseResponse<IEnumerable<FinanceResponseDto>>> ListFinance(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<FinanceResponseDto>>> ListFinanceCliente(BaseFiltersRequest filters, string cliente);
        Task<BaseResponse<FinanceByIdResponeDto>> FinanceById(int id);
        Task<BaseResponse<bool>> RegisterFinance(FinanceRequestDto requestDto);
        Task<BaseResponse<bool>> EditFinance(int id, FinanceRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveFinance(int id);
    }
}