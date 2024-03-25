using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;
using TrackX.Application.Services;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginTrackingController : ControllerBase
    {
        private readonly ILoginTrackingApplication _loginTrackingApplication;

        public LoginTrackingController(ILoginTrackingApplication loginTrackingApplication)
        {
            _loginTrackingApplication = loginTrackingApplication;
        }

        [HttpGet("IDTRA")]
        public async Task<IActionResult> IDTRA(string idtra, string cliente)
        {
            var response = await _loginTrackingApplication.TrackingByIDTRA(idtra,cliente);

            return Ok(response);
        }

        [HttpGet("PO")]
        public async Task<IActionResult> PO(string po, string cliente)
        {
            var response = await _loginTrackingApplication.TrackingByPO(po,cliente);

            return Ok(response);
        }

        [HttpGet("BCF")]
        public async Task<IActionResult> BCF(string bcf, string cliente)
        {
            var response = await _loginTrackingApplication.TrackingByBCF(bcf,cliente);

            return Ok(response);
        }

        [HttpGet("CONTENEDOR")]
        public async Task<IActionResult> CONTENEDOR(string contenedor, string cliente)
        {
            var response = await _loginTrackingApplication.TrackingByContenedor(contenedor,cliente);

            return Ok(response);
        }
    }
}