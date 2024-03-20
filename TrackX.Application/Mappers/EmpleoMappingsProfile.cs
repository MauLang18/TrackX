using AutoMapper;
using TrackX.Application.Dtos.Empleo.Request;
using TrackX.Application.Dtos.Empleo.Response;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Response;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class EmpleoMappingsProfile : Profile
    {
        public EmpleoMappingsProfile()
        {
            CreateMap<TbEmpleo, EmpleoResponseDto>()
                .ForMember(x => x.EstadoEmpleo, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                .ReverseMap();
            CreateMap<TbEmpleo, EmpleoByIdResponseDto>()
                .ReverseMap();
            CreateMap<BaseEntityResponse<TbEmpleo>, BaseEntityResponse<EmpleoResponseDto>>()
                .ReverseMap();
            CreateMap<EmpleoRequestDto, TbEmpleo>()
                .ReverseMap();
        }
    }
}