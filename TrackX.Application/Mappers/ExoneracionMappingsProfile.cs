using AutoMapper;
using TrackX.Application.Dtos.Exoneracion.Request;
using TrackX.Application.Dtos.Exoneracion.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class ExoneracionMappingsProfile : Profile
{
    public ExoneracionMappingsProfile()
    {
        CreateMap<TbExoneracion, ExoneracionResponseDto>()
            .ForMember(x => x.EstadoExoneracion, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbExoneracion, ExoneracionByIdResponseDto>()
            .ReverseMap();
        CreateMap<ExoneracionRequestDto, TbExoneracion>()
            .ReverseMap();
    }
}