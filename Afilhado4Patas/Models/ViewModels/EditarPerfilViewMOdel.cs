using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EditarPerfilViewModel
    {

        [Display(Name = "Primeiro Nome")]
        public string FirstName { get; set; }

        [Display(Name = "Ultimo Nome")]
        public string LastName { get; set; }

        [Display(Name = "Morada")]
        public string Street { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Código de Postal")]
        [RegularExpression(@"\d{4}-\d{3}", ErrorMessage = "Insira um código válido")]
        public string Postalcode { get; set; }

        [RegularExpression(@"\d{9}")]
        [Display(Name = "Nif")]
        public string NIF { get; set; }

        [Display(Name = "Foto")]
        public string Photo { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "A {0} tem de ter entre {2} a {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password Atual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [StringLength(100, ErrorMessage = "A {0} tem de ter entre {2} a {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("NewPassword", ErrorMessage = "A password e a password de confirmação não estão iguais.")]
        public string ConfirmPassword { get; set; }

    }
}
