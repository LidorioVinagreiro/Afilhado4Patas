using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EmailPasseioViewModel
    {
        public EmailPasseioViewModel(string descricao, string nome, string animal, DateTime dia, string horas)
        {
            Descricao = descricao;
            Nome = nome;
            NomeAnimal = animal;
            Dia = dia;
            Horas = horas;
        }
        
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public string NomeAnimal { get; set; }
        public DateTime Dia { get; set; }
        public string Horas { get; set; }
    }
}
