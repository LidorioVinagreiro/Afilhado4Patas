using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Models;
using System;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Afilhado4PatasTest.ControllersTest
{
    public class FuncionarioControllerTest
    {
        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;
        
        public FuncionarioControllerTest(IServiceProvider service)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            var userManager = service.GetRequiredService<UserManager<Utilizadores>>();
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
                context.Roles.AddRange(
                    new IdentityRole("Responsavel"),
                    new IdentityRole("Funcionario"),
                    new IdentityRole("Utilizador"));
                Utilizadores utilizador1 = new Utilizadores
                {
                    UserName = "funcionario1@gmail.com",
                    Email = "funcionario1@gmail.com",
                    EmailConfirmed = true,
                    Active = true
                };
                context.Utilizadores.AddRange(utilizador1);
                userManager.AddToRoleAsync(utilizador1, "Funcionario");
                context.Tarefa.AddRange(
                    new Tarefa {
                        Descricao = "Alimentar",
                        FuncionarioId = utilizador1.Id,
                        Completada = false,
                        Inicio = new DateTime(15,12,2018),
                        Fim = new DateTime(20, 12, 2018)
                    });
                context.SaveChanges();
            }
        }

        [Fact]
        public void MinhasTarefas_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
                var result = controller.MinhasTarefas("funcionario1@gmail.com");
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<Tarefa>>(viewResult.ViewData.Model);
                Assert.Equal(1, model.Count());
            }
        }




    }
}
