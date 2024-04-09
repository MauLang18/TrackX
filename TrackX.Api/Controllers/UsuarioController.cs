using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioApplication _usuarioApplication;
        private readonly IGenerateExcelApplication _generateExcelApplication;

        public UsuarioController(IUsuarioApplication usuarioApplication, IGenerateExcelApplication generateExcelApplication)
        {
            _usuarioApplication = usuarioApplication;
            _generateExcelApplication = generateExcelApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListUsuarios([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _usuarioApplication.ListUsuarios(filters);

            if ((bool)filters.Download!)
            {
                var columnNames = ExcelColumnNames.GetColumnsUsuarios();
                var fileBytes = _generateExcelApplication.GenerateToExcelGeneric(response.Data!, columnNames);
                return File(fileBytes, ContentType.ContentTypeExcel);
            }

            return Ok(response);
        }

        [HttpGet("Select")]
        public async Task<IActionResult> ListSelectUsuarios()
        {
            var response = await _usuarioApplication.ListSelectUsuarios();

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
        public async Task<IActionResult> RegisterUsuario([FromForm] UsuarioRequestDto requestDto)
        {
            var response = await _usuarioApplication.RegisterUsuario(requestDto);

            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditUsuario(int id, [FromForm] UsuarioRequestDto requestDto)
        {
            var response = await _usuarioApplication.EditUsuario(id, requestDto);

            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveUsuario(int id)
        {
            var response = await _usuarioApplication.RemoveUsuario(id);

            return Ok(response);
        }
    }
}