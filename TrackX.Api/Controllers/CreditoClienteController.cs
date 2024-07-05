using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CreditoClienteController : ControllerBase
{
    private readonly ICreditoClienteApplication _creditoClienteApplication;

    public CreditoClienteController(ICreditoClienteApplication creditoClienteApplication)
    {
        _creditoClienteApplication = creditoClienteApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListCreditoCliente(string code)
    {
        var response = await _creditoClienteApplication.CreditoCliente(code);

        return Ok(response);
    }
}