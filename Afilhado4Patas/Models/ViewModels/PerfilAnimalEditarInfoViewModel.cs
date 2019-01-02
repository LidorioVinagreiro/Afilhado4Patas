using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PerfilAnimalEditarInfoViewModel
    {
        [Display(Name = "Nome do Animal")]
        [Required(ErrorMessage = "Preencha este campo com o Nome do Animal!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string NomeAnimal { get; set; }
        [Display(Name = "Data Nascimento")]
        [DateGreatThen18]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataNasc { get; set; }
        [Display(Name = "Porte")]
        public string Porte { get; set; }
        [Display(Name = "Peso")]
        public int Peso { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public string DirectoriaAnimal { get; set; }
        public Boolean Adoptado { get; set; }
        [ForeignKey("Perfil")]
        public int? PadrinhoId { get; set; }
        public virtual Perfil Padrinho { get; set; }
        [ForeignKey("Raca")]
        public int? RacaId { get; set; }
        public Raca RacaAnimal { get; set; }
    }
}
