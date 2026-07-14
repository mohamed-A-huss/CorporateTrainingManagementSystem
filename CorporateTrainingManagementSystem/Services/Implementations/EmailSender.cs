using CorporateTrainingManagementSystem.Settings;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CorporateTrainingManagementSystem.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(IOptions<SmtpSettings> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail,string subject,string htmlMessage)
        {
            using var client = new SmtpClient(_smtpSettings.Host,_smtpSettings.Port);

            client.EnableSsl = _smtpSettings.EnableSsl;

            client.UseDefaultCredentials = false;

            client.Credentials = new NetworkCredential(_smtpSettings.Email,_smtpSettings.Password);

            using var message = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Email,_smtpSettings.DisplayName),

                Subject = subject,

                Body = htmlMessage,

                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message);
        }
    }
}