using AutoMapper;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Commons.Bases.Response;

namespace TrackX.Application.Mappers
{
    public class UsuarioMappingsProfile : Profile
    {
        public UsuarioMappingsProfile()
        {
            CreateMap<TbUsuario, UsuarioResponseDto>()
                .ReverseMap();
            CreateMap<BaseEntityResponse<TbUsuario>, BaseEntityResponse<UsuarioResponseDto>>()
                .ReverseMap();
            CreateMap<UsuarioRequestDto, TbUsuario>()
                .ReverseMap();
            CreateMap<TokenRequestDto, TbUsuario>();
        }
    }
}