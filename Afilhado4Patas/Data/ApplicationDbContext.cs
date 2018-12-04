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
        public DbSet<Perfil> PerfilTable { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Perfil>().HasOne(u => u.Utilizador).WithOne(p => p.Perfil).OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(builder);
            
        }
    }
}
