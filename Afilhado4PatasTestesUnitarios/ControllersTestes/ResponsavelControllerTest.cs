using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Afilhado4PatasTestesUnitarios.ControllersTestes
{
    public class ResponsavelControllerTest
    {
        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;

        public ResponsavelControllerTest()
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
                var controller = new GuestController(context);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task About_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new GuestController(context);
                var result = controller.About();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Contact_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new GuestController(context);
                var result = controller.Contact();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Adotar_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new GuestController(context);
                var result = controller.Adotar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Doar_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new GuestController(context);
                var result = controller.Doar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
