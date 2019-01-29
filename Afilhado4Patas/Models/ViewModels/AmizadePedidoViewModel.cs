using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AmizadePedidoViewModel
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int idPerfilPediuAmizade { get; set; } = 0;
        [Required]
        public int idAnimalComum { get; set; } = 0;
        [Display(Name = "Nome")]
        public string Nome { get; set; } = "";
        [Display(Name = "Nome do Animal")]
        public string NomeAnimal { get; set; } = "";
        public Boolean Aceitar { get; set; } = false;

        public virtual Perfil PerfilPediuAmizade { get; set; }
        public virtual Animal AnimalComum { get; set; }
    }
}
