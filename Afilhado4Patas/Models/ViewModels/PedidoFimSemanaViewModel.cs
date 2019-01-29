using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PedidoFimSemanaViewModel
    {
        [Display(Name = "Animal")]
        [Required(ErrorMessage = "Selecione este campo")]
        public int AnimalId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data Inicio do Pedido")]
        [Date3DaysFromNow]
        public DateTime DataInicio { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data Fim do Pedido")]
        [DateGreater1DayThenStartDate("DataInicio")]
        public DateTime DataFim { get; set; }
    }
}
