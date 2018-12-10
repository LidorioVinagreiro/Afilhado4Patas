using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AdicionarFuncionarioViewModel
    {
        public AdicionarFuncionarioViewModel()
        {
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            public string Nome { get; set; }

            [Required]
            public string Apelido { get; set; }

            [Required]
            public DateTime DataNascimento { get; set; }

            [Required]
            public string Genero { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Palavra-Passe")]
            public string Password { get; set; }
        }
    }
}
