using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PedidoAdocaoViewModel
    {
        [Display(Name = "Selecione o tipo de adoção que pretende")]
        [Required(ErrorMessage = "Selecione um dos campos Adoção Total ou Apadrinhamento")]
        public string TipoAdocao { get; set; }

        [Display(Name = "Nome do Adotante")]
        [RegularExpression(@"^[a-zA-Zà-úÀ-Úâ-ûÂ-Ûã-õÃ-Õ ]+$", ErrorMessage = "Use apenas letras neste campo")]
        [StringLength(30, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string NomeAdotante { get; set; }

        [Display(Name = "Mora em")]
        [Required(ErrorMessage = "Preencha este campo com a sua Morada!")]
        [StringLength(30, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Morada { get; set; }

        [Display(Name = "Sexo do Animal")]
        public string Sexo { get; set; }

        [Display(Name = "Especie do Animal")]
        public string Categoria { get; set; }
        
        [Display(Name = "Porte do Animal")]
        public string Porte { get; set; }

        [Display(Name = "Raça do Animal")]
        public string Raca { get; set; }

        [Display(Name = "Animal")]
        [Required(ErrorMessage = "Selecione este campo")]
        public int AnimalId { get; set; }

        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "Insira um numero telefonico")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Insira um Numero de Telefone com apenas numeros e exatamente 9 digitos")]
        [StringLength(9, ErrorMessage = "O {0} deverá ter exatamente 9 digitos", MinimumLength = 9)]
        public string Telefone { get; set; }

        [Display(Name = "Telefone Alternativo")]
        [RegularExpression(@"\d{9}", ErrorMessage = "Insira um Numero de Telefone com apenas numeros e exatamente 9 digitos")]
        [StringLength(9, ErrorMessage = "O {0} deverá ter exatamente 9 digitos", MinimumLength = 9)]
        public string TelefoneSecundario { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Preencha este campo com o seu Email!")]
        [EmailAddress(ErrorMessage = "Insira um email no formato exemplo@exemplo.com")]
        [StringLength(50, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Email { get; set; }

        [Display(Name = "Email Alternativo")]
        [EmailAddress(ErrorMessage = "Insira um email no formato exemplo@exemplo.com")]
        [StringLength(50, ErrorMessage = "O {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string EmailSecundario { get; set; }

        [Display(Name = "Identificação Pessoal")]
        [DataType(DataType.Upload)]
        [Required(ErrorMessage = "Por favor escolha um ficheiro")]
        [IDTypeValidation]
        public IFormFile File { get; set; }

        [Display(Name = "Por que você quer adotar?")]
        [Required(ErrorMessage = "Preencha este campo com a Motivacao para a adoçao!")]
        [StringLength(50, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Motivacao { get; set; }

        [Display(Name = "Há outros animais na casa? ")]
        [Required(ErrorMessage = "Preencha este campo com a informação de outros animais em sua casa!")]
        [StringLength(50, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string OutrosAnimais { get; set; }                
    }
}
