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
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
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
            var poweruser = new Utilizadores
            {
                UserName = "afilhados4Patas@gmail.com",
                Email = "afilhados4Patas@gmail.com",
                EmailConfirmed = true
            };
            string userPassword = "afilhados4patas";
            var user = await UserManager.FindByEmailAsync(poweruser.Email);
            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(poweruser, "Responsavel");
                }
            }

            var funcionario = new Utilizadores
            {
                UserName = "funcionario@gmail.com",
                Email = "funcionario@gmail.com",
                EmailConfirmed = true
            };
            string funcionarioPassword = "funcionario";
            var funcionarUser = await UserManager.FindByEmailAsync(funcionario.Email);
            if (funcionarUser == null)
            {
                var createPowerUser = await UserManager.CreateAsync(funcionario, funcionarioPassword);
                if (createPowerUser.Succeeded)
                {
                    await UserManager.AddToRoleAsync(funcionario, "Funcionario");
                }
            }
        }
    }
}
