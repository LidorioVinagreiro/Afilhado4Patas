using System.Net.Mail;
using System.Threading.Tasks;

namespace Afilhado4Patas.Areas.Identity.Services
{
    /// <summary>
    /// Classe Email Sender
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Metodo que executa a chamada de envio de email com as informações de assunto do email, 
        /// mensagem/conteudo do email e o email de destinatario
        /// </summary>
        /// <param name="email">Email de Destinatario</param>  
        /// <param name="subject">Assunto do Email</param>
        /// <param name="message">Mensagem/Conteudo do Email</param>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        /// <summary>
        /// Metodo que envia emails com base nas informãcoes recebidas
        /// </summary> 
        /// <param name="subject">Assunto do Email</param>
        /// <param name="message">Mensagem/Conteudo do Email</param>
        /// <param name="email">Email de Destinatario</param> 
        public async Task Execute(string subject, string message, string email)
        {
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