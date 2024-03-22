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
        public async Task<IActionResult> IDTRA(string idtra)
        {
            var response = await _loginTrackingApplication.TrackingByIDTRA(idtra);

            return Ok(response);
        }

        [HttpGet("PO")]
        public async Task<IActionResult> PO(string po)
        {
            var response = await _loginTrackingApplication.TrackingByPO(po);

            return Ok(response);
        }

        [HttpGet("BCF")]
        public async Task<IActionResult> BCF(string bcf)
        {
            var response = await _loginTrackingApplication.TrackingByBCF(bcf);

            return Ok(response);
        }

        [HttpGet("CONTENEDOR")]
        public async Task<IActionResult> CONTENEDOR(string contenedor)
        {
            var response = await _loginTrackingApplication.TrackingByContenedor(contenedor);

            return Ok(response);
        }
    }
}