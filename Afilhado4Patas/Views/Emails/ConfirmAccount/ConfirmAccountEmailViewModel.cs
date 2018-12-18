using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Views.Emails.ConfirmAccount
{
    public class ConfirmAccountEmailViewModel
    {
        public ConfirmAccountEmailViewModel(string confirmEmailUrl, string nome)
        {
            ConfirmEmailUrl = confirmEmailUrl;
            Nome = nome;
        }

        public string ConfirmEmailUrl { get; set; }
        public string Nome { get; set; }
    }
}
