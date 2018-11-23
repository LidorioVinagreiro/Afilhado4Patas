using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Diagnostics;
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
            /*string user = Options.SendGridUser;
            string key = Options.SendGridKey;
            var mailMessage = new MailMessage
            {
                From = new MailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                Subject = subject,
                Body = message
            };

            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(email);
            
            var smtpClient = new SmtpClient
            {
                Credentials = new NetworkCredential(user, key),
                Host = "smtp.sendgrid.net",
                Port = 587
            };
            return smtpClient.SendMailAsync(mailMessage);*/

            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, ""));
            return client.SendEmailAsync(msg);
        }        
    }
}