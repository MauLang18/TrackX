using AutoMapper;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Dtos.Noticia.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class NoticiaMappingsProfile : Profile
    {
        public NoticiaMappingsProfile()
        {
            CreateMap<TbNoticia, NoticiaResponseDto>()
                .ForMember(x => x.EstadoNoticia, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbNoticia, NoticiaByIdResponseDto>()
                .ReverseMap();
            CreateMap<NoticiaRequestDto, TbNoticia>()
                .ReverseMap();
        }
    }
}