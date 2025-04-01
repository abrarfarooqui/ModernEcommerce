using Ecommerce.Application.Services.Interface;
using System.Net.Mail;
using System.Net;
using ECommerce.Application.Common.Utility;

namespace Ecommerce.Application.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_emailSettings.smtpHost, _emailSettings.smtpPort))
            {
                client.Credentials = new NetworkCredential(_emailSettings.smtpUser, _emailSettings.smtpPass);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Set to true if the email body contains HTML
                };

                mailMessage.To.Add(toEmail);

                try
                {
                    client.SendMailAsync(mailMessage);
                }
                catch (SmtpException ex)
                {
                    // Log or handle the exception as needed
                    throw new Exception($"Email sending failed: {ex.Message}", ex);
                }
            }
        }
    }
}
