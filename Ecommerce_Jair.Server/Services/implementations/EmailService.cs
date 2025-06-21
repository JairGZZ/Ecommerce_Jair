using Ecommerce_Jair.Server.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Ecommerce_Jair.Server.Services.implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Ecommerce_Jair", _configuration["EmailSettings:SenderEmail"]));
            email.To.Add(new MailboxAddress("", recipientEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient(); 
            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:SmtpPort"]),
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:SenderPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
