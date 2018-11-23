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
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

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
                Credentials = new NetworkCredential(Options.SendGridUser, Options.SendGridKey),
                Host = "smtp.sendgrid.net",
                Port = 587
            };

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}