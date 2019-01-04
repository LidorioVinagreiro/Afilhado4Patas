using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class RegistarAnimalViewModel
    {
        [Required(ErrorMessage ="É necessário preencher este campo")]
        [RegularExpression(@"^[a-zA-Z ]+$",ErrorMessage ="Nome não é válido")]
        [Display(Name = "Nome do Animal")]
        public string NomeAnimal { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data Nascimento")]
        [DateGreaterThanZeroLessThan100]
        public DateTime DataNasc { get; set; }

        [RegularExpression(@"(pequeno)|(médio)|(grande)",ErrorMessage ="Deve inserir pequeno|médio|grande")]
        [Display(Name = "Porte")]
        public string Porte { get; set; }

        public List<SelectListItem> Portes { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name ="Peso")]
        [Range(1,200,ErrorMessage ="Peso deve estar compreendido entre 1 e 200")]
        public int Peso { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50,ErrorMessage ="A descrição não deve ter mais de 50 caracters")]
        public string Descricao { get; set; }

        public List<SelectListItem> Categorias { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [ForeignKey("Categoria")]
        [Display(Name ="Categoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [ForeignKey("Raca")]
        [Display(Name = "Raça")]
        public int RacaId { get; set; }

    }
}
