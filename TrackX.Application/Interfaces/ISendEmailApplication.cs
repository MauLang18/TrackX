using TrackX.Application.Dtos.Mail.Request;

namespace TrackX.Application.Interfaces;

public interface ISendEmailApplication
{
    void SendEmail(MailRequestDto request);
}