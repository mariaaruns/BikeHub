using BikeHub.Service.Interface;
using BikeHub.Shared.Common;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace BikeHub.Service
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            using var message = new MailMessage();
            message.From = new MailAddress(_settings.SenderEmail, _settings.SenderName);
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using var client = new SmtpClient(_settings.SmtpServer, _settings.Port);
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            client.EnableSsl = true;

            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
               
                throw;
            }
            
        }

    }
}
