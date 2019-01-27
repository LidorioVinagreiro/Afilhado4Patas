using System;
using System.Collections.Generic;
using System.Text;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Data
{
    public class ApplicationDbContext : IdentityDbContext<Utilizadores>
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
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Raca> Racas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Porte> Portes { get; set; }
        public DbSet<Anexo> FicheirosAnimais { get; set; }
        public DbSet<Galeria> FotosAnimais { get; set; }
        public DbSet<Adocao> Adocoes { get; set; }
        public DbSet<Marcacao> Marcacoes { get; set; }
        public DbSet<PedidoAdocao> PedidosAdocao { get; set; }
        public DbSet<PedidoMarcacao> PedidosMarcacoes { get; set; }
        public DbSet<Adotante> Adotantes { get; set; }
        public DbSet<Amizades> Amizades { get; set; }
    }
}
