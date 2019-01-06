using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PerfilEditarDadosPessoaisViewModel
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

        [RegularExpression(@"\d{9}", ErrorMessage = "Insira um Numero de Indentificação Fiscal com apenas numeros e exatamente 9 digitos")]
        [StringLength(9, ErrorMessage = "O {0} deverá ter exatamente 9 digitos", MinimumLength = 9)]
        [Display(Name = "Nif")]
        public string NIF { get; set; }

        [Display(Name = "Foto")]
        public string Photo { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DateGreatThen18LessThen120]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }
    }
}
