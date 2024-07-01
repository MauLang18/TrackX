using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Destino.Request;
using TrackX.Application.Dtos.Destino.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class DestinoMappingsProfile : Profile
    {
        public DestinoMappingsProfile()
        {
            CreateMap<TbDestino, DestinoResponseDto>()
                .ForMember(x => x.EstadoDestino, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbDestino, DestinoByIdResponseDto>()
                .ReverseMap();
            CreateMap<DestinoRequestDto, TbDestino>()
                .ReverseMap();
            CreateMap<TbDestino, SelectResponse>()
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Nombre))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Imagen))
                .ReverseMap();
        }
    }
}