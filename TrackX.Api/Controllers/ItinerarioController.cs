using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Itinerario.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItinerarioController : ControllerBase
    {
        private readonly IItinerarioApplication _itinerarioApplication;

        public ItinerarioController(IItinerarioApplication itinerarioApplication)
        {
            _itinerarioApplication = itinerarioApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListItinerarios([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _itinerarioApplication.ListItinerarios(filters);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ItinerarioById(int id)
        {
            var response = await _itinerarioApplication.ItinerarioById(id);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterItinerario([FromBody] ItinerarioRequestDto requestDto)
        {
            var response = await _itinerarioApplication.RegisterItinerario(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditItinerario(int id, [FromBody] ItinerarioRequestDto requestDto)
        {
            var response = await _itinerarioApplication.EditItinerario(id, requestDto);

            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveItinerario(int id)
        {
            var response = await _itinerarioApplication.RemoveItinerario(id);

            return Ok(response);
        }
    }
}