using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Models.Utilizadores
{
    public class Utilizadores : IdentityUser
    {
        public Perfil Perfil { get; set; }
        public int idade { get; set; }
    }
}
