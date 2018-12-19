using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EmailViewModel
    {
        public EmailViewModel(string emailUrl, string nome)
        {
            EmailUrl = emailUrl;
            Nome = nome;
        }

        public string EmailUrl { get; set; }
        public string Nome { get; set; }
    }
}
