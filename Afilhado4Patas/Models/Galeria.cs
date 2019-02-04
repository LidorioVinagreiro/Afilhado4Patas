using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Galeria
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }

        [Display(Name = "Nome do Ficheiro")]
        public string FicheiroNome { get; set; }
    }
}
