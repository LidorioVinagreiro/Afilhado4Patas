using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class PedidoFimSemana
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Utilizadores")]
        public int AdotanteId { get; set; }
        public virtual Utilizadores Adotante { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }

        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; }

        [Display(Name = "Data da Aprovação")]
        public DateTime DataAprovacao { get; set; }

        [Display(Name = "Data de Inicio")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data de Fim")]
        public DateTime DataFim { get; set; }

        [Display(Name = "Aprovação")]
        public string Aprovacao { get; set; }
    }
}
