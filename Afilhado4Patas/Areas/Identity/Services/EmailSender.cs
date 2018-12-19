using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Afilhado4Patas.Areas.Identity.Services
{
    /// <summary>
    /// Email Sender que tem como função enviar emails para diferentes ações desde Confirmacao de Conta,
    /// Alterar Palavra-Passe, Conta Encerrada, Tarefas adicionadas, Tarefas Editadas e por fim Removidas
    /// </summary>
    public class EmailSender
    {
        /// <summary>
        /// Metodo que executa o envio de email e que recebe o endereço de email de destino,
        /// também o conteudo da email e o assunto para colocar no email a ser enviado
        /// </summary>
        /// <param name="context">Email de Destinatario</param>
        /// /// <param name="subject">Assunto do Email</param>
        /// /// <param name="message">Conteudo/Mensagem do Email</param>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(subject, message, email);
        }

        /// <summary>
        /// Metodo de Envio de Email que envia o email com 
        /// </summary>
        /// <param name="subject">Assunto do Email</param>
        /// /// <param name="message">Conteudo/Mensagem do Email</param>
        /// /// <param name="email">Email de Destinatario</param>
        public Task Execute(string subject, string message, string email)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("afilhados4patas@gmail.com", "Afilhados4Patas"),
                Subject = subject,
                HtmlContent = message                
            };
            msg.AddTo(new EmailAddress(email, ""));
            return client.SendEmailAsync(msg);
        }        
    }
}