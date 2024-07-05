using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Multimedia.Request;
using TrackX.Application.Dtos.Multimedia.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class MultimediaMappingsProfile : Profile
{
    public MultimediaMappingsProfile()
    {
        CreateMap<TbMultimedia, MultimediaResponseDto>()
            .ForMember(x => x.EstadoMultimedia, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbMultimedia, MultimediaByIdResponseDto>()
            .ReverseMap();
        CreateMap<MultimediaRequestDto, TbMultimedia>()
            .ReverseMap();
        CreateMap<TbMultimedia, SelectResponse>()
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.Description, x => x.MapFrom(y => y.Multimedia))
            .ReverseMap();
    }
}