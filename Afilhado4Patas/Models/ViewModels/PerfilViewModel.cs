using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PerfilViewModel
    {
        [Display(Name = "Primeiro Nome")]
        [Required(ErrorMessage = "Preencha este campo com o seu Nome!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string FirstName { get; set; }
        [Display(Name = "Ultimo Nome")]
        [Required(ErrorMessage = "Preencha este campo com o(s) seu(s) Apelido(s)!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string LastName { get; set; }
        [Display(Name = "Morada")]
        [Required(ErrorMessage = "Preencha este campo com a sua Morada!")]
        [StringLength(30, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Street { get; set; }
        [Display(Name = "Cidade")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [Required(ErrorMessage = "Preencha este campo com a sua Cidade!")]
        [StringLength(30, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string City { get; set; }
        [Display(Name = "Código de Postal")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "Insira um código válido no formato XXXX-XXX")]
        public string Postalcode { get; set; }
        [RegularExpression(@"\d{9}", ErrorMessage = "Insira um Numero de Indentificação Fiscal com apenas numeros")]
        [StringLength(9, ErrorMessage = "O {0} deverá ter exatamente 9 digitos", MinimumLength = 9)]
        [Display(Name = "Nif")]
        public string NIF { get; set; }
        [Display(Name = "Foto")]
        public string Photo { get; set; }
        [Display(Name = "Data de Nascimento")]
        [DateGreatThen18]
        public DateTime Birthday { get; set; }
        [Display(Name = "Género")]
        public string Genre { get; set; }
    }
}
