using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class EditarPerfilViewModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Utilizadores")]

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

        [Display(Name = "Idade")]
        public string Age { get { return ((DateTime.UtcNow - Birthday).TotalDays / 365).ToString(); } }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}