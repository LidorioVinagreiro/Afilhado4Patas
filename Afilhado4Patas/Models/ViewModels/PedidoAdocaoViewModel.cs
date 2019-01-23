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
        public string TipoAdocao { get; set; }

        [Display(Name = "Nome do Adotante")]
        public string NomeAdotante { get; set; }

        [Display(Name = "Mora em")]
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
        public int Animal { get; set; }

        [Display(Name = "Telefone")]
        public string Telefone { get; set; }

        [Display(Name = "Telefone Alternativo")]
        public string TelefoneSecundario { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Email Alternativo")]
        public string EmailSecundario { get; set; }

        [Display(Name = "Por que você quer adotar?")]
        public string Motivacao { get; set; }

        [Display(Name = "Há outros animais na casa? ")]
        public string OutrosAnimais { get; set; }
                
        public List<Animal> animais;
    }
}
