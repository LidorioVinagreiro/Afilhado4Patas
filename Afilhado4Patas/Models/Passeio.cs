using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Passeio
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PedidoPasseio")]
        public int PedidoPasseioId { get; set; }

        public virtual PedidoPasseio Pedido { get; set; }        
           
    }
}
