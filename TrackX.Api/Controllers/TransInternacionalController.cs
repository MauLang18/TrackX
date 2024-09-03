﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.TransInternacional.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransInternacionalController : ControllerBase
{
    private readonly ITransInternacionalApplication _transInternacionalApplication;

    public TransInternacionalController(ITransInternacionalApplication transInternacionalApplication)
    {
        _transInternacionalApplication = transInternacionalApplication;
    }

    [HttpGet]
    public async Task<IActionResult> ListTransInternacional()
    {
        var response = await _transInternacionalApplication.ListTransInternacional();

        return Ok(response);
    }

    public async Task<IActionResult> RegisterComentario([FromBody] TransInternacionalRequestDto request)
    {
        var response = await _transInternacionalApplication.RegisterComentario(request);

        return Ok(response);
    }
}