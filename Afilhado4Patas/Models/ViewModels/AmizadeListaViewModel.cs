using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.ViewModels
{
    public class AmizadeListaViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int AmigoId { get; set; }
        public Boolean Amigos { get; set; }
        public virtual Perfil Amigo { get; set; }
    }
}
