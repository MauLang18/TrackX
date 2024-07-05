using Microsoft.AspNetCore.Mvc;
using TrackX.Application.Dtos.Mail.Request;
using TrackX.Application.Interfaces;

namespace TrackX.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MailController : ControllerBase
{
    private readonly ISendEmailApplication _sendEmailApplication;

    public MailController(ISendEmailApplication sendEmailApplication)
    {
        _sendEmailApplication = sendEmailApplication;
    }

    [HttpPost("Send")]
    public IActionResult SendMail(MailRequestDto requestDto)
    {
        _sendEmailApplication.SendEmail(requestDto);

        return Ok();
    }
}