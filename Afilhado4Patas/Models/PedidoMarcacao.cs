using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class PedidoMarcacao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Utilizadores")]
        public int AdotanteId { get; set; }
        public virtual Utilizadores Adotante { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }

        public DateTime DataPedido { get; set; }

        public DateTime DataAprovacao { get; set; }
        
        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }
    }
}
