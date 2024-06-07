using AutoMapper;
using TrackX.Application.Dtos.Logs.Request;
using TrackX.Application.Dtos.Logs.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers
{
    public class LogsMappingsProfle : Profile
    {
        public LogsMappingsProfle()
        {
            CreateMap<TbLogs, LogsResponseDto>()
                .ForMember(x => x.EstadoLogs, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Exito" : "Error"))
                .ReverseMap();
            CreateMap<TbLogs, LogsByIdResponseDto>()
                .ReverseMap();
            CreateMap<LogsRequestDto, TbLogs>()
                .ReverseMap();
        }
    }
}