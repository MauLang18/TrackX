using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Pod.Request;
using TrackX.Application.Dtos.Pod.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class PodMappingsProfile : Profile
    {
        public PodMappingsProfile()
        {
            CreateMap<TbPod, PodResponseDto>()
                            .ForMember(x => x.EstadoPod, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
                            .ReverseMap();
            CreateMap<TbPod, PodByIdResponseDto>()
                .ReverseMap();
            CreateMap<PodRequestDto, TbPod>()
                .ReverseMap();
            CreateMap<TbPod, SelectResponse>()
                .ForMember(x => x.Description, x => x.MapFrom(y => y.Nombre))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.Nombre))
                .ReverseMap();
        }
    }
}