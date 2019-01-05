using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Especie")]
        public string Nome { get; set; }
    }
}
