using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EmailFimSemanaViewModel
    {
        public EmailFimSemanaViewModel(string descricao, string nome, string animal, DateTime inicio, DateTime fim)
        {
            Descricao = descricao;
            Nome = nome;
            NomeAnimal = animal;
            Inicio = inicio;
            Fim = fim;
        }
        
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string NomeAnimal { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
    }
}
