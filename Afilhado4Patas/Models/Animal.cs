using Afilhado4Patas.Data;
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

        [Display(Name = "Sexo do Animal")]
        public string Sexo { get; set; }

        [Display(Name = "Data Nascimento")]
        public DateTime DataNasc { get; set; }

        [ForeignKey("Porte")]
        [Display(Name = "Porte")]
        public int PorteId { get; set; }
        public virtual Porte PorteAnimal { get; set; }

        [Display(Name = "Peso")]
        public double Peso { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Directoria")]
        public string DirectoriaAnimal { get; set; }

        [Display(Name = "Foto")]
        public string Foto { get; set; }

        [Display(Name = "Adotado")]
        public Boolean Adoptado { get; set; }

        [ForeignKey("Raca")]
        [Display(Name = "Raça")]
        public int? RacaId { get; set; }
        public virtual Raca RacaAnimal { get; set; }

        [Display(Name = "Ativo")]
        public Boolean Ativo { get; set; }

        public virtual List<Utilizadores> Adotantes { get; set; }
    }
}
