using AutoMapper;
using TrackX.Application.Dtos.Finance.Request;
using TrackX.Application.Dtos.Finance.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class FinanceMappingsProfile : Profile
{
    public FinanceMappingsProfile()
    {
        CreateMap<TbFinance, FinanceResponseDto>()
            .ForMember(x => x.EstadoFinance, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbFinance, FinanceByIdResponeDto>()
            .ReverseMap();
        CreateMap<FinanceRequestDto, TbFinance>()
            .ReverseMap();
    }
}