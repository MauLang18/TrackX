using AutoMapper;
using TrackX.Application.Dtos.Bcf.Request;
using TrackX.Application.Dtos.Bcf.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class BcfMappingsProfile : Profile
{
    public BcfMappingsProfile()
    {
        CreateMap<TbBcf, BcfResponseDto>()
            .ForMember(x => x.EstadoBcf, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbBcf, BcfByIdResponseDto>()
            .ReverseMap();
        CreateMap<BcfRequestDto, TbBcf>()
            .ReverseMap();
    }
}