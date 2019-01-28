using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class FimSemana
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PedidoFimSemana")]
        public int PedidoFimSemanaId { get; set; }

        public virtual PedidoFimSemana Pedido { get; set; }        
           
    }
}
