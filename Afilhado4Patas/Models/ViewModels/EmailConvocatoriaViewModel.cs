using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EmailConvocatoriaViewModel
    {
        public EmailConvocatoriaViewModel(string descricao, string nome, string animal)
        {
            Descricao = descricao;
            Nome = nome;
            NomeAnimal = animal;
        }
        
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string NomeAnimal { get; set; }
    }
}
