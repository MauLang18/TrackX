using AutoMapper;
using TrackX.Application.Dtos.Rol.Request;
using TrackX.Application.Dtos.Rol.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class RolMappingsProfile : Profile
    {
        public RolMappingsProfile() 
        {
            CreateMap<TbRol, RolResponseDto>()
                .ForMember(x => x.EstadoRol, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbRol, RolSelectResponseDto>()
                .ReverseMap();
            CreateMap<RolRequestDto, TbRol>()
                .ReverseMap();
        }
    }
}