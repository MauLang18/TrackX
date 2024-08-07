﻿using AutoMapper;
using TrackX.Application.Commons.Select;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Dtos.Usuario.Response;
using TrackX.Domain.Entities;
using TrackX.Utilities.Static;

namespace TrackX.Application.Mappers;

public class UsuarioMappingsProfile : Profile
{
    public UsuarioMappingsProfile()
    {
        CreateMap<TbUsuario, UsuarioResponseDto>()
            .ForMember(x => x.Rol, x => x.MapFrom(y => y.IdRolNavigation.Nombre))
            .ForMember(x => x.EstadoUsuario, x => x.MapFrom(y => y.Estado.Equals((int)StateTypes.Activo) ? "Activo" : "Inactivo"))
            .ReverseMap();
        CreateMap<TbUsuario, SelectResponse>()
            .ForMember(x => x.Description, x => x.MapFrom(y => y.NombreEmpresa))
            .ForMember(x => x.Id, x => x.MapFrom(y => y.Cliente))
            .ReverseMap();
        CreateMap<UsuarioRequestDto, TbUsuario>()
            .ReverseMap();
        CreateMap<TokenRequestDto, TbUsuario>();
    }
}