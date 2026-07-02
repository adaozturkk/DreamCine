using DreamCine.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DreamCine.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            using var smtpClient = new SmtpClient(_config["EmailSettings:Server"], int.Parse(_config["EmailSettings:Port"]))
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_config["EmailSettings:SenderEmail"], _config["EmailSettings:Password"])
            };

            using var mailMessage = new MailMessage()
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(_config["EmailSettings:SenderEmail"]),
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
