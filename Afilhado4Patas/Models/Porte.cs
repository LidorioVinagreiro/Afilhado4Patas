using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Porte
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Porte")]
        public string TipoPorte { get; set; }
    }
}
