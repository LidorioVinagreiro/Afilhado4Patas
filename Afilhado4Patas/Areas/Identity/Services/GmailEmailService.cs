﻿using Microsoft.IdentityModel.Protocols;
using System;
using System.Net.Mail;

namespace Afilhado4Patas.Areas.Identity.Services
{
    public class GmailEmailService : SmtpClient
    {
        // Gmail user-name
        public string UserName { get; set; }

        public GmailEmailService() :
            base(System.Environment.GetEnvironmentVariable("GmailHost"), Int32.Parse(System.Environment.GetEnvironmentVariable("GmailPort")))
        {
            //Get values from web.config file:
            this.UserName = System.Environment.GetEnvironmentVariable("GmailUserName");
            this.EnableSsl = Boolean.Parse(System.Environment.GetEnvironmentVariable("GmailSsl"));
            this.UseDefaultCredentials = false;
            this.Credentials = new System.Net.NetworkCredential(this.UserName, System.Environment.GetEnvironmentVariable("GmailPassword"));
        }
    }
}
