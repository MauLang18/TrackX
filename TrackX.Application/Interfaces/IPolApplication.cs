﻿using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pol.Request;
using TrackX.Application.Dtos.Pol.Response;

namespace TrackX.Application.Interfaces
{
    public interface IPolApplication
    {
        Task<BaseResponse<IEnumerable<PolResponseDto>>> LisPol(BaseFiltersRequest filters);
        Task<BaseResponse<IEnumerable<SelectResponse>>> ListSelectPol();
        Task<BaseResponse<PolResponseDto>> PolById(int id);
        Task<BaseResponse<bool>> RegisterPol(PolRequestDto requestDto);
        Task<BaseResponse<bool>> EditPol(int id, PolRequestDto requestDto);
        Task<BaseResponse<bool>> RemovePol(int id);
    }
}