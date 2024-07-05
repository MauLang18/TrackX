using AutoMapper;
using TrackX.Application.Dtos.ControlInventario.Request;
using TrackX.Application.Dtos.ControlInventario.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class ControlInventarioMappingsProfile : Profile
{
    public ControlInventarioMappingsProfile()
    {
        CreateMap<TbControlInventarioWhs, ControlInventarioResponseDto>()
            .ForMember(x => x.EstadoControlInventario, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbControlInventarioWhs, ControlInventarioByIdResponseDto>()
            .ReverseMap();
        CreateMap<ControlInventarioRequestDto, TbControlInventarioWhs>()
            .ReverseMap();
    }
}