using System;
using System.Collections.Generic;
using System.Text;
using Afilhado4Patas.Models.Utilizadores;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Utilizadores> Utilizadores { get; set; }
    }
}
