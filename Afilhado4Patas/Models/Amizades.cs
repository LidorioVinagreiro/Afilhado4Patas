using Afilhado4Patas.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models
{
    public class Amizades
    {
        [Key]
            public int Id { get; set; }
        [ForeignKey("Perfil")]
            public int IdPerfilPediu { get; set; }
        [ForeignKey("Perfil")]
            public int IdPerfilAceitar { get; set; }
        [ForeignKey("Animal")]
            public int IdAnimalEmComum { get; set; }

        public bool Amigos { get; set; }
        public bool? ExistePedido { get; set; }
        public virtual Animal AnimalComumAosDois { get; set; }
        //o valor de amigos tem que ser preenchido?
        public virtual Perfil PossivelAmigo { get; set; }

        public Amizades() {
            IdPerfilAceitar = 0;
            IdPerfilPediu = 0;
            Amigos = false;
            ExistePedido = null;
        }
    }
}
