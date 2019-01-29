using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class PedidoPasseioViewModel
    {
        [Display(Name = "Animal")]
        [Required(ErrorMessage = "Selecione este campo")]
        public int AnimalId { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data do Passeio")]
        [Date3DaysFromNow]
        public DateTime DataPasseio { get; set; }

        [Required(ErrorMessage = "É necessário preencher este campo")]
        [Display(Name = "Data do Passeio")]
        [RegularExpression(@"^(([01]?[0-9]|2[0-3]):[0-5][0-9])+$", ErrorMessage = "Horas não são válidas")]
        public string HoraPasseio { get; set; }
    }
}
