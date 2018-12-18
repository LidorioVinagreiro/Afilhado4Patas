using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class FuncionarioViewModel
    {
        [Required(ErrorMessage = "Preencha este campo com o seu Email!")]
        [EmailAddress(ErrorMessage = "Insira um email no formato exemplo@exemplo.com")]
        [StringLength(50, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha este campo com o seu Nome!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha este campo com o(s) seu(s) Apelido(s)!")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Apelido { get; set; }
        
        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Preencha este campo com a sua Data de Nascimento!")]
        [DateGreatThen18]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Selecione um dos campos Masculino ou Feminino")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Preencha este campo com a sua Password!")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "A sua password deverá ter letras (aA) e pelo menos 1 numero e não deverá ter caracteres que não letras ou numeros")]
        [StringLength(15, ErrorMessage = "A {0} deverá ter pelo menos {2} e um maximo de {1} caracteres de comprimento.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Palavra-Passe")]
        public string Password { get; set; }

        public Utilizadores Utilizador {get; set;}

        public List<TarefaViewModel> ListaTarefas { get; set; }
    }
}
