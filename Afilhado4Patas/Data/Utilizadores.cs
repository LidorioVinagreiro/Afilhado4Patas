using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class Utilizadores : IdentityUser
    {

        [ForeignKey("Perfil")]
        public int PerfilId { get; set; }
        public virtual Perfil Perfil { get; set; }
        /*ADICIONAR PROPRIEDADES NOVAS DO IDENTITY TAREFA 1*/
    }
}
