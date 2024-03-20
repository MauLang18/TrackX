using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.Noticia.Request;
using TrackX.Application.Interfaces;
using TrackX.Infrastructure.Commons.Bases.Request;

namespace TrackX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiaController : ControllerBase
    {
        private readonly INoticiaApplication _noticiaApplication;

        public NoticiaController(INoticiaApplication noticiaApplication)
        {
            _noticiaApplication = noticiaApplication;
        }

        [HttpGet]
        public async Task<IActionResult> ListNoticias([FromQuery] BaseFiltersRequest filters)
        {
            var response = await _noticiaApplication.ListNoticias(filters);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> NoticiaById(int id)
        {
            var response = await _noticiaApplication.NoticiaById(id);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterNoticia([FromForm] NoticiaRequestDto requestDto)
        {
            var response = await _noticiaApplication.RegisterNoticia(requestDto);
            return Ok(response);
        }

        [HttpPut("Edit/{id:int}")]
        public async Task<IActionResult> EditNoticia(int id, [FromForm] NoticiaRequestDto requestDto)
        {
            var response = await _noticiaApplication.EditNoticia(id, requestDto);

            return Ok(response);
        }

        [HttpPut("Remove/{id:int}")]
        public async Task<IActionResult> RemoveNoticia(int id)
        {
            var response = await _noticiaApplication.RemoveNoticia(id);

            return Ok(response);
        }
    }
}