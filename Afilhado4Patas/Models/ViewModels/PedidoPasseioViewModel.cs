using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PedidoPasseioViewModel
    {
        public int AnimalId { get; set; }

        public DateTime DataPasseio { get; set; }

        public string HoraPasseio { get; set; }
    }
}
