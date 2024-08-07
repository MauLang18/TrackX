﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TrackX.Application.Dtos.Usuario.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthApplication _authApplication;

    public AuthController(IAuthApplication authApplication)
    {
        _authApplication = authApplication;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] TokenRequestDto requestDto, [FromQuery] string authType)
    {
        var response = await _authApplication.Login(requestDto, authType);

        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("LoginWithGoogle")]
    public async Task<IActionResult> LoginWithGoogle([FromBody] string credentials, [FromQuery] string authType)
    {
        var response = await _authApplication.LoginWithGoogle(credentials, authType);

        return Ok(response);
    }
}