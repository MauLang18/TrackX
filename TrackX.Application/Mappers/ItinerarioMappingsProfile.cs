using AutoMapper;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Dtos.Itinerario.Response;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class ItinerarioMappingsProfile : Profile
    {
        public ItinerarioMappingsProfile()
        {
            CreateMap<TbItinerario, ItinerarioResponseDto>()
                .ForMember(x => x.EstadoItinerario, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbItinerario, ItinerarioByIdResponseDto>()
                .ReverseMap();
            CreateMap<BaseEntityResponse<TbItinerario>, BaseEntityResponse<ItinerarioResponseDto>>()
                .ReverseMap();
            CreateMap<ItinerarioRequestDto, TbItinerario>()
                .ReverseMap();
        }
    }
}