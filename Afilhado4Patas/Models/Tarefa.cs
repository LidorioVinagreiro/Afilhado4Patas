using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;

namespace Afilhado4Patas.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Funcionario")]
        public string FuncionarioId { get; set; }
        public virtual Utilizadores Utilizador { get; set; }
        [Display(Name = "Data de Inicio")]
        public DateTime Inicio { get; set; }
        [Display(Name = "Data de Fim")]
        public DateTime Fim { get; set; }
        [Display(Name = "Descricao")]
        public string Descricao { get; set; }
        public bool Completada { get; set; }
 
    }
}
