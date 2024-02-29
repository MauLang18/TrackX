﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Interfaces;
using TrackX.Infrastructure.Commons.Bases.Request;

namespace TrackX.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplication _usuarioApplication;

        public UsuarioController(IUsuarioApplication usuarioApplication)
        {
            _usuarioApplication = usuarioApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListUsuarios([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _usuarioApplication.ListUsuarios(filters);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> UsuarioById(int id)
        {
            var response = await _usuarioApplication.UsuarioById(id);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUsuario([FromBody] UsuarioRequestDto requestDto)
        {
            var response = await _usuarioApplication.RegisterUsuario(requestDto);

            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditUsuario(int id, [FromBody] UsuarioRequestDto requestDto)
        {
            var response = await _usuarioApplication.EditUsuario(id, requestDto);

            return Ok(response);
        }

        [HttpDelete("Remove/{id:int}")]
        public async Task<IActionResult> RemoveUsuario(int id)
        {
            var response = await _usuarioApplication.RemoveUsuario(id);

            return Ok(response);
        }
    }
}