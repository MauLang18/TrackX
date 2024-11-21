﻿using TrackX.Application.Commons.Bases.Response;
using TrackX.Application.Dtos.Cotizacion.Request;
using TrackX.Domain.Entities;

namespace TrackX.Application.Interfaces;

public interface ICotizacionApplication
{
    Task<BaseResponse<Dynamics<DynamicsCotizacion>>> ListCotizacion(int numFilter, string textFilter);
    Task<BaseResponse<bool>> RegisterCotizacion(CotizacionRequestDto requestDto);
}