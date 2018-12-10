using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Afilhado4Patas.Data
{
    public static class Seed
    {
        public static async Task CreateInitialData(IServiceProvider serviceProvider, IConfiguration Configuration, ApplicationDbContext contexto)
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
                    City = "Setubal"
                };
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
                contexto.PerfilTable.Add(perfilFuncionario);
                contexto.SaveChanges();
                funcionario2.PerfilId = perfilFuncionario.Id;
                contexto.SaveChanges();
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(funcionario2, "Funcionario");
                }
            }
        }
    }
}
