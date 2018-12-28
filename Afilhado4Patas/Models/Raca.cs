using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Raca
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Categoria")]
        public int? CategoriaId { get; set; }
        public Categoria CategoriaRaca { get; set; }
        [Display(Name = "Raça")]
        public string NomeRaca { get; set; }

    }
}
