using System;
using System.Collections.Generic;
using System.Text;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizadores,IdentityRole,string>
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
        public DbSet<Tarefa> Tarefa { get; set; }
    }
}
