using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Nome do Animal")]
        public string NomeAnimal { get; set; }
        [Display(Name = "Data Nascimento")]
        public DateTime DataNasc { get; set; }
        [Display(Name = "Porte")]
        public string Porte { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public string Foto { get; set; }

        [ForeignKey("Raca")]
        public int? RacaId { get; set; }
        public Raca RacaAnimal { get; set; }

    }
}
