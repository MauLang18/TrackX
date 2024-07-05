using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FacturaLoginController : ControllerBase
{
    private readonly IFacturaLoginApplication _facturaLoginApplication;

    public FacturaLoginController(IFacturaLoginApplication facturaLoginApplication)
    {
        _facturaLoginApplication = facturaLoginApplication;
    }

    [HttpGet("Factura")]
    public async Task<IActionResult> Factura(string factura, string cliente)
    {
        var response = await _facturaLoginApplication.TrackingByFactura(factura, cliente);

        return Ok(response);
    }
}