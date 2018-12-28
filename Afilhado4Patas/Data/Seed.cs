using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Afilhado4Patas.Data
{
    public static class Seed
    {
        public static async Task CreateInitialData(IServiceProvider serviceProvider, IConfiguration Configuration, ApplicationDbContext contexto, IHostingEnvironment hostingEnvironment)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<Utilizadores>>();            
            string[] roleNames = { "Responsavel", "Funcionario", "Utilizador" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                // creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var responsavel = new Utilizadores
            {
                UserName = "afilhados4Patas@gmail.com",
                Email = "afilhados4Patas@gmail.com",
                EmailConfirmed = true,
                Active = true
            };
            string responsavelPassword = "afilhados4patas";
            var user = await UserManager.FindByEmailAsync(responsavel.Email);
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(responsavel, responsavelPassword);
                Perfil perfilResponsavel = new Perfil
                {
                    UtilizadorId = responsavel.Id,
                    FirstName = "Afihados",
                    LastName = "4Patas",
                    Genre = "Masculino",
                    Birthday = new DateTime(1990, 10, 10),
                    Street = "Rua Das Flores",
                    NIF = "123456789",
                    Postalcode = "2564-568",
                    City = "Setubal",
                };
                CreateFolder(responsavel, hostingEnvironment);
                perfilResponsavel.Directoria = hostingEnvironment.WebRootPath + "\\Utilizadores\\" + responsavel.Id;
                contexto.PerfilTable.Add(perfilResponsavel);
                contexto.SaveChanges();
                responsavel.PerfilId = perfilResponsavel.Id;
                contexto.SaveChanges();
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(responsavel, "Responsavel");
                }
            }

            var funcionario = new Utilizadores
            {
                UserName = "funcionario@gmail.com",
                Email = "funcionario@gmail.com",
                EmailConfirmed = true,
                Active = true
            };
            string funcionarioPassword = "funcionario1";
            var funcionarUser = await UserManager.FindByEmailAsync(funcionario.Email);
            if (funcionarUser == null)
            {
                var createPowerUser = await UserManager.CreateAsync(funcionario, funcionarioPassword);
                Perfil perfilFuncionario = new Perfil
                {
                    UtilizadorId = funcionario.Id,
                    FirstName = "Daniel",
                    LastName = "Dias",
                    Genre = "Masculino",
                    Birthday = new DateTime(1995, 10, 10),
                    Street = "Praceta das Flores",
                    NIF = "145236987",
                    Postalcode = "2564-405",
                    City = "Lisboa"
                };
                CreateFolder(funcionario, hostingEnvironment);
                perfilFuncionario.Directoria = hostingEnvironment.WebRootPath + "\\Utilizadores\\" + funcionario.Id;
                contexto.PerfilTable.Add(perfilFuncionario);
                contexto.SaveChanges();
                funcionario.PerfilId = perfilFuncionario.Id;
                contexto.SaveChanges();
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(funcionario, "Funcionario");
                }
            }

            var funcionario2 = new Utilizadores
            {
                UserName = "funcionario2@gmail.com",
                Email = "funcionario2@gmail.com",
                EmailConfirmed = true,
                Active = true
            };
            string funcionario2Password = "funcionario2";
            var funcionarUser2 = await UserManager.FindByEmailAsync(funcionario2.Email);
            if (funcionarUser2 == null)
            {
                var createPowerUser = await UserManager.CreateAsync(funcionario2, funcionario2Password);
                Perfil perfilFuncionario = new Perfil
                {
                    UtilizadorId = funcionario.Id,
                    FirstName = "Lidorio",
                    LastName = "Vinagreiro",
                    Genre = "Masculino",
                    Birthday = new DateTime(1995, 10, 10),
                    Street = "Praceta das Flores",
                    NIF = "145236987",
                    Postalcode = "2564-405",
                    City = "Lisboa"
                };
                CreateFolder(funcionario2, hostingEnvironment);
                perfilFuncionario.Directoria = hostingEnvironment.WebRootPath + "\\Utilizadores\\" + funcionario2.Id;
                contexto.PerfilTable.Add(perfilFuncionario);
                contexto.SaveChanges();
                funcionario2.PerfilId = perfilFuncionario.Id;
                contexto.SaveChanges();
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(funcionario2, "Funcionario");
                }
            }

            //Categorias
            if (contexto.Categorias.Count() == 0)
            {
                contexto.Categorias.Add(criarCategoria("Caes"));
                contexto.Categorias.Add(criarCategoria("Gatos"));
                contexto.Categorias.Add(criarCategoria("Passaros"));
                contexto.Categorias.Add(criarCategoria("Cavalos"));
                contexto.SaveChanges();
            }

            //Racas
            if (contexto.Racas.Count() == 0)
            {
                //Caes
                int categoria = retornarCategoriaId("Caes", contexto);
                contexto.Racas.Add(criarRaca("Bulldogue", categoria));
                contexto.Racas.Add(criarRaca("Yorkshire Terrier", categoria));
                contexto.Racas.Add(criarRaca("Labrador Retriever", categoria));
                contexto.Racas.Add(criarRaca("Pastor-Alemao", categoria));
                contexto.Racas.Add(criarRaca("Buldogue Frances", categoria));
                contexto.Racas.Add(criarRaca("Pug", categoria));
                contexto.Racas.Add(criarRaca("Beagle", categoria));
                contexto.Racas.Add(criarRaca("Rottweiler", categoria));
                contexto.Racas.Add(criarRaca("Golden Retriever", categoria));
                contexto.Racas.Add(criarRaca("Bull Terrier", categoria));
                contexto.Racas.Add(criarRaca("Cocker Spaniel", categoria));
                contexto.Racas.Add(criarRaca("Chihuahua", categoria));
                contexto.Racas.Add(criarRaca("Husky", categoria));
                contexto.Racas.Add(criarRaca("Pit Bull Terrier", categoria));
                contexto.Racas.Add(criarRaca("Pinsher", categoria));
                contexto.Racas.Add(criarRaca("Boxer", categoria));
                contexto.Racas.Add(criarRaca("Salsicha", categoria));
                contexto.Racas.Add(criarRaca("Border Collie", categoria));
                contexto.Racas.Add(criarRaca("Doberman", categoria));
                contexto.Racas.Add(criarRaca("Akita", categoria));
                contexto.Racas.Add(criarRaca("Chow-Chow", categoria));
                contexto.Racas.Add(criarRaca("Sao-Bernardo", categoria));
                contexto.Racas.Add(criarRaca("Dogue Alemao", categoria));
                contexto.Racas.Add(criarRaca("Dalmata", categoria));
                contexto.Racas.Add(criarRaca("Shar-pei", categoria));
                contexto.Racas.Add(criarRaca("Cane Corso", categoria));
                contexto.Racas.Add(criarRaca("Terra Nova", categoria));
                contexto.Racas.Add(criarRaca("Dogue Bordeus", categoria));
                contexto.Racas.Add(criarRaca("Mastim", categoria));
                contexto.Racas.Add(criarRaca("Galgo", categoria));
                contexto.Racas.Add(criarRaca("N/A", categoria));

                ///Gatos
                categoria = retornarCategoriaId("Gatos", contexto);
                contexto.Racas.Add(criarRaca("Persa", categoria));
                contexto.Racas.Add(criarRaca("Siames", categoria));
                contexto.Racas.Add(criarRaca("Siberiano", categoria));
                contexto.Racas.Add(criarRaca("N/A", categoria));

                //Cavalos
                categoria = retornarCategoriaId("Cavalos", contexto);
                contexto.Racas.Add(criarRaca("Puro Sangue", categoria));
                contexto.Racas.Add(criarRaca("Cavalo Arabe", categoria));
                contexto.Racas.Add(criarRaca("Lusitano", categoria));
                contexto.Racas.Add(criarRaca("Mustangue", categoria));
                contexto.Racas.Add(criarRaca("N/A", categoria));

                //Passaros
                categoria = retornarCategoriaId("Passaros", contexto);
                contexto.Racas.Add(criarRaca("Caturra", categoria));
                contexto.Racas.Add(criarRaca("Piriquito", categoria));
                contexto.Racas.Add(criarRaca("Mandarim", categoria));
                contexto.Racas.Add(criarRaca("Papagaio", categoria));
                contexto.Racas.Add(criarRaca("N/A", categoria));
                contexto.SaveChanges();
            }

        }

        private static Raca criarRaca(string nome, int categoria)
        {
            Raca raca = new Raca
            {
                NomeRaca = nome,
                CategoriaId = categoria
            };
            return raca;
        }

        private static Categoria criarCategoria(string nome)
        {
            Categoria categoria = new Categoria
            {
                Nome = nome
            };
            return categoria;
        }

        private static int retornarCategoriaId(string nome, ApplicationDbContext contexto)
        {
            return contexto.Categorias.Where(c => c.Nome == nome).FirstOrDefault().Id;
        }

        private static bool CreateFolder(Utilizadores user, IHostingEnvironment hostingEnvironment)
        {
            string pathUtilizadores = hostingEnvironment.WebRootPath + "\\Utilizadores";
            string pathUser = pathUtilizadores + "\\" + user.Id;
            if (Directory.Exists(pathUtilizadores) && !Directory.Exists(pathUser))
            {
                Directory.CreateDirectory(pathUser);
                return Directory.Exists(pathUser);
            }
            return Directory.Exists(pathUser);
        }
    }
}
