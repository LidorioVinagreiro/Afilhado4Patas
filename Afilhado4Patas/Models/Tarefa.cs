using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Funcionario")]
        public string FuncionarioId { get; set; }
        [Display(Name = "Data de Inicio")]
        public DateTime Inicio { get; set; }
        [Display(Name = "Data de Fim")]
        public DateTime Fim { get; set; }
        [Display(Name = "Descricao")]
        public string Descricao { get; set; }
        public bool Completada { get; set; }
    }
}
