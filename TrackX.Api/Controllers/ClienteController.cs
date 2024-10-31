using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly IClienteApplication _clienteApplication;

    public ClienteController(IClienteApplication clienteApplication)
    {
        _clienteApplication = clienteApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListClientesName(string name)
    {
        var response = await _clienteApplication.CodeCliente(name);

        return Ok(response);
    }

    [HttpGet("Idtra")]
    public async Task<IActionResult> ListClientesIdtra(string idtra)
    {
        var response = await _clienteApplication.ClienteIdtra(idtra);

        return Ok(response);
    }

    [HttpGet("Code")]
    public async Task<IActionResult> ListClientesCode(string code)
    {
        var response = await _clienteApplication.NameCliente(code);

        return Ok(response);
    }
}