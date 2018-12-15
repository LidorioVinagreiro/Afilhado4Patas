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
    public class UtilizadorControllerTest
    {
        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;

        public UtilizadorControllerTest()
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
                var controller = new UtilizadorController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
