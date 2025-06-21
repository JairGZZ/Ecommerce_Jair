using MimeKit;

namespace Ecommerce_Jair.Server.Services.Interfaces
{
    // 1. Define la interfaz
    public interface IEmailService
    {
        /// <summary>
        /// Envía un correo electrónico.
        /// </summary>
        /// <param name="recipientEmail">Correo del destinatario.</param>
        /// <param name="subject">Asunto del mensaje.</param>
        /// <param name="body">Cuerpo del mensaje (HTML permitido).</param>
        Task SendEmailAsync(string recipientEmail, string subject, string body);
    }

    

}
