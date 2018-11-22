using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Afilhado4Patas.Areas.Identity.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        public Task Execute(string subject, string message, string email)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                Subject = subject,
                Body = message
            };

            mailMessage.To.Add(email);

            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential("azure_9ff8de35d7d8dad2f48f48ed257df722@azure.com", "Swpv1819"),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}