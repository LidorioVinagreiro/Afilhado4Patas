using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AmizadeViewModel
    {
        [Required]
        public int idPerfilPossivelAmizade { get; set; } = 0;
        [Required]
        public int idAnimalComum { get; set; } = 0;
        [Display(Name = "Nome")]
        public string Nome { get; set; } = "";
        [Display(Name = "Nome do Animal")]
        public string NomeAnimal { get; set; } = "";
        public Boolean Aceitar { get; set; } = false;

        public virtual Animal AnimalEmComum { get; set; }
        public virtual Perfil PerfilPossivelAmizade { get; set; }
    }
}
