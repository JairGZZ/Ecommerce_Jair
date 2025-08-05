using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Ecommerce_Jair.Server.Utils;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Ecommerce_Jair.Server.Services.implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IUserTokensRepository _userTokensRepository;


        public EmailService(IOptions<EmailSettings> emailSettings,IUserTokensRepository userTokensRepository)
        {
            _emailSettings = emailSettings.Value;
            _userTokensRepository = userTokensRepository;
        }

        public async Task<Result> SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Ecommerce_Jair", _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress("", recipientEmail));
            email.Subject = subject;
              
            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient(); 
            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.SmtpPort,
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _emailSettings.SenderEmail,
                _emailSettings.SenderPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return Result.Ok();






        }

        
    }
}
