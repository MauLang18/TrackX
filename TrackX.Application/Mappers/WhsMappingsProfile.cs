using AutoMapper;
using TrackX.Application.Dtos.Whs.Request;
using TrackX.Application.Dtos.Whs.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class WhsMappingsProfile : Profile
    {
        public WhsMappingsProfile()
        {
            CreateMap<TbWhs, WhsResponseDto>()
                .ForMember(x => x.EstadoWhs, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbWhs, WhsResponseByIdDto>()
                .ReverseMap();
            CreateMap<WhsRequestDto, TbWhs>()
                .ReverseMap();
        }
    }
}