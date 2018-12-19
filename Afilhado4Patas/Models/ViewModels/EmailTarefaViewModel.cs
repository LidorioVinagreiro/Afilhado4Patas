using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EmailTarefaViewModel
    {
        public EmailTarefaViewModel(string descricao, DateTime inicio, DateTime fim, string nome)
        {
            DataInicio = inicio;
            DataFim = fim;
            Descricao = descricao;
            Nome = nome;
        }
        
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Nome { get; set; }
    }
}
