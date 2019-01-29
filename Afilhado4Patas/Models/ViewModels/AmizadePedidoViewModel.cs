using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AmizadePedidoViewModel
    {
        public int id { get; set; }
        public int idPerfilPediuAmizade { get; set; } = 0;
        public int idAnimalComum { get; set; } = 0;
        public string Nome { get; set; } = "";
        public string NomeAnimal { get; set; } = "";
        public Boolean Aceitar { get; set; } = false;

        public virtual Perfil PerfilPediuAmizade { get; set; }
        public virtual Animal AnimalComum { get; set; }
    }
}
