using Microsoft.IdentityModel.Protocols;
using System;
using System.Net.Mail;

namespace Afilhado4Patas.Areas.Identity.Services
{
    /// <summary>
    /// Utilizador Controller
    /// </summary>
    public class GmailEmailService : SmtpClient
    {
        public string UserName { get; set; }

        /// <summary>
        /// Construtor da classe GmailEmailService que recolhe os dados de configurações Google para envio de emails
        /// </summary>
        public GmailEmailService() :
            base(System.Environment.GetEnvironmentVariable("GmailHost"), Int32.Parse(System.Environment.GetEnvironmentVariable("GmailPort")))
        {
            this.UserName = System.Environment.GetEnvironmentVariable("GmailUserName");
            this.EnableSsl = Boolean.Parse(System.Environment.GetEnvironmentVariable("GmailSsl"));
            this.UseDefaultCredentials = false;
            this.Credentials = new System.Net.NetworkCredential(this.UserName, System.Environment.GetEnvironmentVariable("GmailPassword"));
        }
    }
}
