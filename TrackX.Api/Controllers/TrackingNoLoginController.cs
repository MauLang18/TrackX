using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingNoLoginController : ControllerBase
    {
        private readonly ITrackingNoLoginApplication _trackingNoLoginApplication;

        public TrackingNoLoginController(ITrackingNoLoginApplication trackingNoLoginApplication)
        {
            _trackingNoLoginApplication = trackingNoLoginApplication;
        }

        [HttpGet("IDTRA")]
        public async Task<IActionResult> IDTRA(string idtra)
        {
            var response = await _trackingNoLoginApplication.TrackingByIDTRA(idtra);

            return Ok(response);
        }
    }
}
