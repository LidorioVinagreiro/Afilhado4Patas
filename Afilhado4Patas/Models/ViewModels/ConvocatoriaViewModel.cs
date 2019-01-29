using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class ConvocatoriaViewModel
    {
        [Display(Name = "Notas")]
        [Required(ErrorMessage = "Preencha este campo com notas sobre a convocação!")]
        [RegularExpression(@"^[a-zA-Zà-úÀ-Úâ-ûÂ-Ûã-õÃ-Õ! ]+$", ErrorMessage = "Este campo apenas nao deverá conter numeros")]
        [StringLength(50, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Notas { get; set; }

        [Required]
        public string UtilizadorId { get; set; }

        [Required]
        public string AnimalNome { get; set; }
    }
}
