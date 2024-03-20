﻿using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Request;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Infrastructure.Persistences.Interfaces
{
    public interface IEmpleoRepository
    {
        Task<BaseEntityResponse<TbEmpleo>> ListEmpleos(BaseFiltersRequest filters);
    }
}