using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AprovacaoViewModel
    {
        [Display(Name = "Notas")]
        [Required(ErrorMessage = "Preencha este campo com notas sobre a aprovacao/rejeicao!")]
        [StringLength(50, ErrorMessage = "A {0} deverá ter um maximo de {1} caracteres de comprimento.")]
        public string Notas { get; set; }
        
        [Required]
        public int PedidoId { get; set; }


    }
}
