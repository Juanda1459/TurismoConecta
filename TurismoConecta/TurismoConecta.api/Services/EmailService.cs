// Services/EmailService.cs
using System.Net;
using System.Net.Mail;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpoHtml)
        {
            var host = _configuration["Smtp:Host"];
            var port = int.Parse(_configuration["Smtp:Port"] ?? "587");
            var usuario = _configuration["Smtp:User"];
            var password = _configuration["Smtp:Password"];
            var enableSsl = bool.Parse(_configuration["Smtp:EnableSsl"] ?? "true");

            using var mensaje = new MailMessage
            {
                From = new MailAddress(usuario!, "TurismoConecta"),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };
            mensaje.To.Add(destinatario);

            using var cliente = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(usuario, password),
                EnableSsl = enableSsl
            };

            await cliente.SendMailAsync(mensaje);
        }
    }
}