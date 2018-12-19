using System.Net.Mail;
using System.Threading.Tasks;

namespace Afilhado4Patas.Areas.Identity.Services
{
    public class EmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }
        public async Task Execute(string subject, string message, string email)
        {
            /*var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                Subject = subject,
                HtmlContent = message                
            };
            msg.AddTo(new EmailAddress(email, ""));
            return client.SendEmailAsync(msg);*/

            MailMessage mail = new MailMessage(new MailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                new MailAddress(email));

            mail.Subject = subject;
            mail.Body = message;

            mail.IsBodyHtml = true;

            using (var mailClient = new GmailEmailService())
            {
                await mailClient.SendMailAsync(mail);
            }
        }        
    }
}