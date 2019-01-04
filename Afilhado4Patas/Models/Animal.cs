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
        
        [ForeignKey("Raca")]
        [Display(Name = "Porte")]
        public int PorteId { get; set; }
        public virtual Porte PorteAnimal { get; set; }

        [Display(Name ="Peso")]
        public int Peso { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name ="Directoria")]
        public string DirectoriaAnimal { get; set; }

        [Display(Name ="Foto")]
        public string Foto { get; set; }

        [Display(Name ="Adoptado")]
        public Boolean Adoptado { get; set; }

        [ForeignKey("Perfil")]
        [Display(Name ="Padrinho")]
        public int? PadrinhoId { get; set; }
        public virtual Perfil Padrinho { get; set; }

        [ForeignKey("Raca")]
        [Display(Name ="Raca")]
        public int? RacaId { get; set; }
        public virtual Raca RacaAnimal { get; set; }

        [Display(Name ="Ativo")]
        public Boolean Ativo { get; set; }
    }
}
