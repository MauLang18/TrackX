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
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    color: #333;
                    line-height: 1.6;
                }}
                .container {{
                    width: 80%;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f4f4f4;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                h1 {{
                    color: #0056b3;
                }}
                p {{
                    margin: 10px 0;
                }}
                a {{
                    color: #007bff;
                    text-decoration: none;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 0.9em;
                    color: #888;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Activación de Cuenta</h1>
                <p>Se acaba de registrar el usuario <strong>{requestDto.Correo}</strong></p>
                <p>Para activar su cuenta, por favor utilice el siguiente enlace:</p>
                <p><a href='{activationLink}'>Activar cuenta</a></p>
                <h2>Información del Usuario Registrado:</h2>
                <p><strong>Nombre:</strong> {requestDto.Nombre}</p>
                <p><strong>Apellido:</strong> {requestDto.Apellido}</p>
                <p><strong>Contraseña:</strong> {requestDto.Pass}</p>
                <p><strong>Correo:</strong> {requestDto.Correo}</p>
                <p><strong>Tipo:</strong> {requestDto.Tipo}</p>
                <p><strong>Cliente:</strong> {requestDto.Cliente}</p>
                <p><strong>Rol:</strong> {requestDto.IdRol}</p>
                <p><strong>Nombre de la Empresa:</strong> {requestDto.NombreEmpresa}</p>
                <p><strong>Teléfono:</strong> {requestDto.Telefono}</p>
                <p><strong>Dirección:</strong> {requestDto.Direccion}</p>
                <p><strong>País:</strong> {requestDto.Pais}</p>
                <p><strong>Páginas:</strong> {requestDto.Paginas}</p>
                <p><strong>Estado:</strong> {requestDto.Estado}</p>
                <div class='footer'>
                    <p>Nota: Si no puedes hacer clic en el enlace, copia y pega la URL en tu navegador para activar tu cuenta.</p>
                </div>
            </div>
        </body>
        </html>"
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
        var data = await _usuarioApplication.UsuarioById(id);

        if (data == null || data.Data!.Id <= 0)
        {
            return NotFound("Usuario no encontrado.");
        }

        // Crear el contenido del correo
        var requestMail = new MailRequestDto
        {
            Para = data.Data!.Correo,
            Asunto = "Tu cuenta ha sido activada",
            Contenido = $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    color: #333;
                    line-height: 1.6;
                }}
                .container {{
                    width: 80%;
                    margin: 0 auto;
                    padding: 20px;
                    background-color: #f4f4f4;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }}
                h1 {{
                    color: #28a745;
                }}
                p {{
                    margin: 10px 0;
                }}
                a {{
                    color: #007bff;
                    text-decoration: none;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 0.9em;
                    color: #888;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h1>Cuenta Activada Exitosamente</h1>
                <p>Hola <strong>{data.Data!.Nombre} {data.Data!.Apellido}</strong>,</p>
                <p>Tu cuenta ha sido activada exitosamente. Ahora puedes iniciar sesión y utilizar todos los servicios disponibles.</p>
                <h2>Detalles de tu Cuenta:</h2>
                <p><strong>Nombre:</strong> {data.Data!.Nombre}</p>
                <p><strong>Apellido:</strong> {data.Data!.Apellido}</p>
                <p><strong>Correo:</strong> {data.Data!.Correo}</p>
                <p><strong>Teléfono:</strong> {data.Data!.Telefono}</p>
                <p><strong>Dirección:</strong> {data.Data!.Direccion}</p>
                <p><strong>País:</strong> {data.Data!.Pais}</p>
                <p><strong>Nombre de la Empresa:</strong> {data.Data!.NombreEmpresa}</p>
                <div class='footer'>
                    <p>Si tienes alguna pregunta o necesitas ayuda, no dudes en contactarnos.</p>
                </div>
            </div>
        </body>
        </html>"
        };

        _sendEmailApplication.SendEmail(requestMail);

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