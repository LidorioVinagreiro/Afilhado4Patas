using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Afilhado4PatasTest.ControllersTest
{
    public class FuncionariosControllerTest
    {
        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;

        public FuncionariosControllerTest()
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection).Options;

        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
                var result = controller.Perfil();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Perfil_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
                var result = controller.Perfil();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Tarefa_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
                var result = controller.Perfil();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task MinhasTarefas_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
                var result = controller.Perfil();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
