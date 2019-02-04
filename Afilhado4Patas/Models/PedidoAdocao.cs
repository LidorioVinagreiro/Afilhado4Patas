using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class PedidoAdocao
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Utilizadores")]
        public int AdotanteId { get; set; }
        public virtual Utilizadores Adotante { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }

        [Display(Name = "Morada")]
        public string Morada { get; set; }

        [Display(Name = "Motivo")]
        public string Motivo { get; set; }

        [Display(Name = "Outros Animais")]
        public string OutrosAnimais { get; set; }

        [Display(Name = "Diretoria")]
        public string DiretoriaPedido { get; set; }

        [Display(Name = "Data do Pedido")]
        public DateTime DataPedido { get; set; }

        [Display(Name = "Data da Aprovação")]
        public DateTime DataAprovacao { get; set; }

        [Display(Name = "Tipo de Adoção")]
        public string TipoAdocao { get; set; }

        [Display(Name = "Aprovação")]
        public string Aprovacao { get; set; }
    }
}
