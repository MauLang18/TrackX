using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using TrackX.Application.Dtos.Mail.Request;
using TrackX.Application.Interfaces;
using TrackX.Domain.Entities;
using TrackX.Infrastructure.Secret;

namespace TrackX.Application.Services
{
    public class SendEmailApplication : ISendEmailApplication
    {
        private readonly ISecretService _secretService;

        public SendEmailApplication(ISecretService secretService)
        {
            _secretService = secretService;
        }

        public void SendEmail(MailRequestDto request)
        {
            var secretJson = _secretService.GetSecret("TrackX/data/Email").Result;

            var secretData = JsonConvert.DeserializeObject<SecretResponse<EmailConfig>>(secretJson);

            var emailConfig = secretData?.Data?.Data;

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(emailConfig!.UserName));
            email.To.Add(MailboxAddress.Parse(request.Para));
            email.Subject = request.Asunto;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Contenido
            };

            using var smtp = new SmtpClient();
            smtp.Connect(emailConfig.Host,
                int.Parse(emailConfig.Port!),
                SecureSocketOptions.StartTls);

            smtp.Authenticate(emailConfig.UserName, emailConfig.Password);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}