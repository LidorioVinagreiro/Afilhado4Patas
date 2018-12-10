using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;

namespace Afilhado4Patas.Models.ViewModels
{
    public class TarefaViewModel
    {
        [Display(Name = "Funcionario")]
        public string FuncionarioId { get; set; }
        [Display(Name = "Data de Inicio")]
        public DateTime Inicio { get; set; }
        [Display(Name = "Data de Fim")]
        public DateTime Fim { get; set; }
        [Display(Name = "Descricao")]
        public string Descricao { get; set; }
        [Display(Name = "Completada")]
        public bool Completada { get; set; }
    }
}
