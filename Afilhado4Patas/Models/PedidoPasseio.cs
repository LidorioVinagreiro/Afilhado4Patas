using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class PedidoPasseio
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Utilizadores")]
        public int AdotanteId { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }

        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; }

        [Display(Name = "Data da Aprovação")]
        public DateTime DataAprovacao { get; set; }

        [Display(Name = "Data do Passeio")]
        public DateTime DataPasseio { get; set; }

        [Display(Name = "Hora do Passeio")]
        public string HoraPasseio { get; set; }
    
        [Display(Name = "Aprovação")]
        public string Aprovacao { get; set; }

        public virtual Utilizadores Adotante { get; set; }
        public virtual Animal Animal { get; set; }
    }
}
