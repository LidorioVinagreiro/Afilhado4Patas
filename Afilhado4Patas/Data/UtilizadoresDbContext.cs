using Afilhado4Patas.Models.Utilizadores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4Patas.Data
{
    public class UtilizadoresDbContext : IdentityDbContext<Utilizadores>
    {
        
    }
}
