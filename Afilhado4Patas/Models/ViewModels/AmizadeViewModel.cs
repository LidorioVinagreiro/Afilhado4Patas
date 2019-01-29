using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AmizadeViewModel
    {
        public int idPerfilPossivelAmizade { get; set; } = 0;
        public int idAnimalComum { get; set; } = 0;
        public string Nome { get; set; } = "";
        public string NomeAnimal { get; set; } = "";
        public Boolean Aceitar { get; set; } = false;

        public virtual Animal AnimalEmComum { get; set; }
        public virtual Perfil PerfilPossivelAmizade { get; set; }
    }
}
