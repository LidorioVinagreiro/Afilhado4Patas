using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Marcacao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PedidoAdocao")]
        public int PedidoMarcacaoId { get; set; }

        public virtual PedidoMarcacao Pedido { get; set; }        
           
    }
}
