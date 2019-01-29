using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Adotante
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Perfil")]
        public int AdotanteId { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }

        public virtual Perfil Adotante_User { get; set; }
        public virtual Animal Animal { get; set; }
    }
}
