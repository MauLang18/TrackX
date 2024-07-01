using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Origen.Request;
using TrackX.Application.Dtos.Origen.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class OrigenMappingsProfile : Profile
    {
        public OrigenMappingsProfile()
        {
            CreateMap<TbOrigen, OrigenResponseDto>()
                .ForMember(x => x.EstadoOrigen, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbOrigen, OrigenByIdResponseDto>()
                .ReverseMap();
            CreateMap<OrigenRequestDto, TbOrigen>()
                .ReverseMap();
            CreateMap<TbOrigen, SelectResponse>()
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Nombre))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Imagen))
                .ReverseMap();
        }
    }
}
