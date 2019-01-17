using Afilhado4Patas.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PerfilEditarMoradaViewModel
    {
        [Display(Name = "Morada")]
        [Required(ErrorMessage = "Preencha este campo com a sua Morada!")]
        [StringLength(30, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Street { get; set; }

        [Display(Name = "Cidade")]
        [RegularExpression(@"^[a-zA-Zà-úÀ-Úâ-ûÂ-Ûã-õÃ-Õ ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [Required(ErrorMessage = "Preencha este campo com a sua Cidade!")]
        [StringLength(30, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string City { get; set; }

        [Display(Name = "Código de Postal")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "Insira um código válido no formato XXXX-XXX")]
        public string Postalcode { get; set; }      
        
    }
}
