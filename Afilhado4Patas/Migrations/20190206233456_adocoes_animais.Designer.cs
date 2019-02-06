﻿// <auto-generated />
using System;
using Afilhado4Patas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Afilhado4Patas.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190206233456_adocoes_animais")]
    partial class adocoes_animais
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Afilhado4Patas.Data.Utilizadores", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<bool>("Active");

                    b.Property<int?>("AnimalId");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<int?>("PerfilId");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PerfilId")
                        .IsUnique()
                        .HasFilter("[PerfilId] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Adocao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PedidoAdocaoId");

                    b.HasKey("Id");

                    b.HasIndex("PedidoAdocaoId");

                    b.ToTable("Adocoes");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Adotante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdotanteId");

                    b.Property<int?>("Adotante_UserId");

                    b.Property<int>("AnimalId");

                    b.HasKey("Id");

                    b.HasIndex("Adotante_UserId");

                    b.HasIndex("AnimalId");

                    b.ToTable("Adotantes");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Amizades", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Amigos");

                    b.Property<int?>("AnimalComumAosDoisId");

                    b.Property<bool?>("ExistePedido");

                    b.Property<int>("IdAnimalEmComum");

                    b.Property<int>("IdPerfilAceitar");

                    b.Property<int>("IdPerfilPediu");

                    b.Property<int?>("PossivelAmigoId");

                    b.HasKey("Id");

                    b.HasIndex("AnimalComumAosDoisId");

                    b.HasIndex("PossivelAmigoId");

                    b.ToTable("Amizades");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Anexo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnimalId");

                    b.Property<string>("FicheiroNome");

                    b.HasKey("Id");

                    b.ToTable("FicheirosAnimais");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Adoptado");

                    b.Property<bool>("Ativo");

                    b.Property<DateTime>("DataNasc");

                    b.Property<string>("Descricao");

                    b.Property<string>("DirectoriaAnimal");

                    b.Property<string>("Foto");

                    b.Property<string>("NomeAnimal");

                    b.Property<double>("Peso");

                    b.Property<int>("PorteId");

                    b.Property<int?>("RacaId");

                    b.Property<string>("Sexo");

                    b.HasKey("Id");

                    b.HasIndex("PorteId");

                    b.HasIndex("RacaId");

                    b.ToTable("Animais");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nome");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.FimSemana", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PedidoFimSemanaId");

                    b.HasKey("Id");

                    b.HasIndex("PedidoFimSemanaId");

                    b.ToTable("FinsSemanas");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Galeria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AnimalId");

                    b.Property<string>("FicheiroNome");

                    b.HasKey("Id");

                    b.ToTable("FotosAnimais");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Passeio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PedidoPasseioId");

                    b.HasKey("Id");

                    b.HasIndex("PedidoPasseioId");

                    b.ToTable("Passeios");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoAdocao", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdotanteId");

                    b.Property<string>("AdotanteId1");

                    b.Property<int>("AnimalId");

                    b.Property<string>("Aprovacao");

                    b.Property<DateTime>("DataAprovacao");

                    b.Property<DateTime>("DataPedido");

                    b.Property<string>("DiretoriaPedido");

                    b.Property<string>("Morada");

                    b.Property<string>("Motivo");

                    b.Property<string>("NomeFicheiroID");

                    b.Property<string>("OutrosAnimais");

                    b.Property<string>("TipoAdocao");

                    b.HasKey("Id");

                    b.HasIndex("AdotanteId1");

                    b.HasIndex("AnimalId");

                    b.ToTable("PedidosAdocao");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoFimSemana", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdotanteId");

                    b.Property<string>("AdotanteId1");

                    b.Property<int>("AnimalId");

                    b.Property<string>("Aprovacao");

                    b.Property<DateTime>("DataAprovacao");

                    b.Property<DateTime>("DataFim");

                    b.Property<DateTime>("DataInicio");

                    b.Property<DateTime>("DataPedido");

                    b.HasKey("Id");

                    b.HasIndex("AdotanteId1");

                    b.HasIndex("AnimalId");

                    b.ToTable("PedidosFimSemana");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoPasseio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AdotanteId");

                    b.Property<string>("AdotanteId1");

                    b.Property<int>("AnimalId");

                    b.Property<string>("Aprovacao");

                    b.Property<DateTime>("DataAprovacao");

                    b.Property<DateTime>("DataPasseio");

                    b.Property<DateTime>("DataPedido");

                    b.Property<string>("HoraPasseio");

                    b.HasKey("Id");

                    b.HasIndex("AdotanteId1");

                    b.HasIndex("AnimalId");

                    b.ToTable("PedidosPasseio");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Perfil", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Birthday");

                    b.Property<string>("City");

                    b.Property<string>("Directoria");

                    b.Property<string>("FirstName");

                    b.Property<string>("Genre");

                    b.Property<string>("LastName");

                    b.Property<string>("NIF");

                    b.Property<int?>("PerfilId");

                    b.Property<string>("Photo");

                    b.Property<string>("Postalcode");

                    b.Property<string>("Street");

                    b.Property<string>("UtilizadorId");

                    b.HasKey("Id");

                    b.HasIndex("PerfilId");

                    b.ToTable("PerfilTable");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Porte", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TipoPorte");

                    b.HasKey("Id");

                    b.ToTable("Portes");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Raca", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoriaId");

                    b.Property<string>("NomeRaca");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Racas");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Tarefa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completada");

                    b.Property<string>("Descricao");

                    b.Property<DateTime>("Fim");

                    b.Property<string>("FuncionarioId");

                    b.Property<DateTime>("Inicio");

                    b.Property<string>("UtilizadorId");

                    b.HasKey("Id");

                    b.HasIndex("UtilizadorId");

                    b.ToTable("Tarefa");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Afilhado4Patas.Data.Utilizadores", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Animal")
                        .WithMany("Adotantes")
                        .HasForeignKey("AnimalId");

                    b.HasOne("Afilhado4Patas.Models.Perfil", "Perfil")
                        .WithOne("Utilizador")
                        .HasForeignKey("Afilhado4Patas.Data.Utilizadores", "PerfilId");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Adocao", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.PedidoAdocao", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoAdocaoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Adotante", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Perfil", "Adotante_User")
                        .WithMany()
                        .HasForeignKey("Adotante_UserId");

                    b.HasOne("Afilhado4Patas.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Amizades", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Animal", "AnimalComumAosDois")
                        .WithMany()
                        .HasForeignKey("AnimalComumAosDoisId");

                    b.HasOne("Afilhado4Patas.Models.Perfil", "PossivelAmigo")
                        .WithMany()
                        .HasForeignKey("PossivelAmigoId");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Animal", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Porte", "PorteAnimal")
                        .WithMany()
                        .HasForeignKey("PorteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Afilhado4Patas.Models.Raca", "RacaAnimal")
                        .WithMany()
                        .HasForeignKey("RacaId");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.FimSemana", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.PedidoFimSemana", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoFimSemanaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Passeio", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.PedidoPasseio", "Pedido")
                        .WithMany()
                        .HasForeignKey("PedidoPasseioId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoAdocao", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores", "Adotante")
                        .WithMany()
                        .HasForeignKey("AdotanteId1");

                    b.HasOne("Afilhado4Patas.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoFimSemana", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores", "Adotante")
                        .WithMany()
                        .HasForeignKey("AdotanteId1");

                    b.HasOne("Afilhado4Patas.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.PedidoPasseio", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores", "Adotante")
                        .WithMany()
                        .HasForeignKey("AdotanteId1");

                    b.HasOne("Afilhado4Patas.Models.Animal", "Animal")
                        .WithMany()
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Perfil", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Perfil")
                        .WithMany("Amigos")
                        .HasForeignKey("PerfilId");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Raca", b =>
                {
                    b.HasOne("Afilhado4Patas.Models.Categoria", "CategoriaRaca")
                        .WithMany()
                        .HasForeignKey("CategoriaId");
                });

            modelBuilder.Entity("Afilhado4Patas.Models.Tarefa", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores", "Utilizador")
                        .WithMany()
                        .HasForeignKey("UtilizadorId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Afilhado4Patas.Data.Utilizadores")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Afilhado4Patas.Data.Utilizadores")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
