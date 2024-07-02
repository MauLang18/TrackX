using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pol.Request;
using TrackX.Application.Dtos.Pol.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class PolMappingsProfile : Profile
    {
        public PolMappingsProfile()
        {
            CreateMap<TbPol, PolResponseDto>()
                .ForMember(x => x.EstadoPol, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ForMember(x => x.EstadoWHS, x => x.MapFrom(y => y.WHS.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbPol, PolByIdResponseDto>()
                .ReverseMap();
            CreateMap<TbPol, PolByWhsResponseDto>()
                .ReverseMap();
            CreateMap<PolRequestDto, TbPol>()
                .ReverseMap();
            CreateMap<TbPol, SelectResponse>()
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Nombre))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Nombre))
                .ReverseMap();
        }
    }
}