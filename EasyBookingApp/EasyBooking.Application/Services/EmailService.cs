using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using EasyBooking.Application.Contracts;

namespace EasyBooking.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");
            var host = smtpSettings["Host"] ?? "smtp.gmail.com";
            var port = int.Parse(smtpSettings["Port"] ?? "587");
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");
            var userName = smtpSettings["UserName"] ?? "EasyBookingValidation@gmail.com";
            var password = smtpSettings["Password"] ?? "tydx ikyy nkaf iumd";
            var fromEmail = smtpSettings["FromEmail"] ?? "EasyBookingValidation@gmail.com";
            var fromName = smtpSettings["FromName"] ?? "EasyBooking";

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(userName, password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(fromEmail, fromName);
                    message.To.Add(new MailAddress(to));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}