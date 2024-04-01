using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Commons.Bases.Request;
using TrackX.Application.Dtos.Whs.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhsController : ControllerBase
    {
        private readonly IWhsApplication _WhsApplication;

        public WhsController(IWhsApplication WhsApplication)
        {
            _WhsApplication = WhsApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListWhs([FromQuery] BaseFiltersRequest filters, string whs)
        {
            var response = await _WhsApplication.ListWhs(filters, whs);

            return Ok(response);
        }

        [HttpGet("Cliente")]
        public async Task<IActionResult> ListWhsCliente([FromQuery] BaseFiltersRequest filters, string cliente, string whs)
        {
            var response = await _WhsApplication.ListWhsCliente(filters, cliente, whs);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> WhsById(int id)
        {
            var response = await _WhsApplication.WhsById(id);
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterWhs([FromForm] WhsRequestDto requestDto)
        {
            var response = await _WhsApplication.RegisterWhs(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditWhs(int id, [FromForm] WhsRequestDto requestDto)
        {
            var response = await _WhsApplication.EditWhs(id, requestDto);

            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveWhs(int id)
        {
            var response = await _WhsApplication.RemoveWhs(id);

            return Ok(response);
        }
    }
}