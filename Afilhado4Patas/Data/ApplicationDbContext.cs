using System;
using System.Collections.Generic;
using System.Text;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);            
        }

        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Perfil> PerfilTable { get; set; }
    }
}
