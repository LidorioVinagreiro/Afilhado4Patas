using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PerfilEditarPalavraPasseViewModel
    {
        [Required(ErrorMessage = "Preencha este campo com a sua Password Antiga!")]
        [PasswordAtLeast1LetterAnd1Number]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
        [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra-Passe Antiga")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Preencha este campo com a sua nova Password!")]
        [PasswordAtLeast1LetterAnd1Number]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
        [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Palavra-Passe")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Preencha este campo com a sua nova Password!")]
        [PasswordAtLeast1LetterAnd1Number]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme Nova Palavra-Passe")]
        [Compare("NewPassword", ErrorMessage = "As palavras-passes inseridas não são iguais")]
        public string ConfirmNewPassword { get; set; }
    }
}
