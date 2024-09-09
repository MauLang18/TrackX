using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Mail.Request;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Interfaces;
using TrackX.Utilities.Static;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioApplication _usuarioApplication;
    private readonly IGenerateExcelApplication _generateExcelApplication;
    private readonly ISendEmailApplication _sendEmailApplication;

    public UsuarioController(IUsuarioApplication usuarioApplication, IGenerateExcelApplication generateExcelApplication, ISendEmailApplication sendEmailApplication)
    {
        _usuarioApplication = usuarioApplication;
        _generateExcelApplication = generateExcelApplication;
        _sendEmailApplication = sendEmailApplication;
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

    [HttpPost("Register")]
    public async Task<IActionResult> RegisterUsuario([FromForm] UsuarioRequestDto requestDto)
    {
        var response = await _usuarioApplication.RegisterUsuario(requestDto);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("Register/Web")]
    public async Task<IActionResult> RegisterWebUsuario([FromForm] UsuarioRequestDto requestDto)
    {
        var response = await _usuarioApplication.RegisterUsuarioWeb(requestDto);

        if (response == null || response!.Data!.Id <= 0)
        {
            return BadRequest("Error al registrar el usuario.");
        }

        var activationLink = $"https://api.logisticacastrofallas.com/api/Usuario/State/{response!.Data!.Id}";

        var requestMail = new MailRequestDto
        {
            Para = "rsibaja@castrofallas.com",
            Asunto = $"Activación de cuenta - {requestDto.Nombre} {requestDto.Apellido} - {requestDto.NombreEmpresa}",
            Contenido = $@"
            Se acaba de registrar el usuario {requestDto.Correo}

            Para activar su cuenta se puede utilizar el siguiente enlace:
            <a href='{activationLink}'>Activar cuenta</a>

            Información del usuario registrado:
            Nombre: {requestDto.Nombre}
            Apellido: {requestDto.Apellido}
            Contraseña: {requestDto.Pass}
            Correo: {requestDto.Correo}
            Tipo: {requestDto.Tipo}
            Cliente: {requestDto.Cliente}
            Rol: {requestDto.IdRol}
            Nombre de la empresa: {requestDto.NombreEmpresa}
            Teléfono: {requestDto.Telefono}
            Dirección: {requestDto.Direccion}
            País: {requestDto.Pais}
            Páginas: {requestDto.Paginas}
            Estado: {requestDto.Estado}
            
            Nota: Si no puedes hacer clic en el enlace, copia y pega la URL en tu navegador para activar tu cuenta.
        "
        };

        _sendEmailApplication.SendEmail(requestMail);

        return Ok(response);
    }


    [HttpPut("Edit/{id:int}")]
    public async Task<IActionResult> EditUsuario(int id, [FromForm] UsuarioRequestDto requestDto)
    {
        var response = await _usuarioApplication.EditUsuario(id, requestDto);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpGet("State/{id:int}")]
    public async Task<IActionResult> ChangeStateUsuario(int id)
    {
        var response = await _usuarioApplication.ChangeStateUsuario(id);

        return Ok(response);
    }

    [HttpPut("Remove/{id:int}")]
    public async Task<IActionResult> RemoveUsuario(int id)
    {
        var response = await _usuarioApplication.RemoveUsuario(id);

        return Ok(response);
    }

    [HttpPost("Import")]
    public async Task<IActionResult> ImportExcelUsuario(ImportRequest request)
    {
        var response = await _usuarioApplication.ImportExcelUsuario(request);

        return Ok(response);
    }
}