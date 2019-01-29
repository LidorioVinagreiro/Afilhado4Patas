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
    public class EditarAnimalViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [RegularExpression(@"^[a-zA-Zà-úÀ-Úâ-ûÂ-Ûã-õÃ-Õ ]+$", ErrorMessage = "Nome não é válido")]
        [Display(Name = "Nome do Animal")]
        public string NomeAnimal { get; set; }

        [Required(ErrorMessage = "Selecione um dos campos Macho ou Fêmea")]
        public string Sexo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/mm/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data Nascimento")]
        [DateGreaterThen0LessThen30]
        public DateTime DataNasc { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Porte")]
        public int PorteId { get; set; }
        public List<SelectListItem> Portes { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Peso")]
        [Range(0.1, 200, ErrorMessage = "Peso deve estar compreendido entre 0.1 Kilogramas e 200 Kilogramas")]
        public double Peso { get; set; }

        [Display(Name = "Descrição")]
        [StringLength(50, ErrorMessage = "A descrição não deve ter mais de 50 caracters")]
        public string Descricao { get; set; }
        public List<SelectListItem> Categorias { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [ForeignKey("Categoria")]
        [Display(Name = "Categoria")]
        public int? CategoriaId { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [ForeignKey("Raca")]
        [Display(Name = "Raça")]
        public int? RacaId { get; set; }

        [Display(Name ="Ativo")]
        public Boolean Ativo { get; set; }

        [Display(Name = "Adoptado")]
        public Boolean Adoptado { get; set; }
    }
}
