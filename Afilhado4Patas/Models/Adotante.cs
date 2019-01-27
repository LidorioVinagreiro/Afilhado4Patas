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

        [ForeignKey("Utilizadores")]
        public int AdotanteId { get; set; }

        public virtual Utilizadores Adotante_User { get; set; }

        [ForeignKey("Animal")]
        public int AnimalId { get; set; }

        public virtual Animal Animal { get; set; }
    }
}
