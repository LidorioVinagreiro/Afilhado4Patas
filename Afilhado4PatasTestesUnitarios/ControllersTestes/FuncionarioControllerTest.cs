using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Models;
using System;
using Xunit;
using Microsoft.Data.Sqlite;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Afilhado4PatasTest.ControllersTest
{
    public class FuncionarioControllerTest
    {
        
        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;
        
        public FuncionarioControllerTest()
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context);
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
                var controller = new FuncionarioController(context);
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
                var controller = new FuncionarioController(context);
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
                var controller = new FuncionarioController(context);
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
                var controller = new FuncionarioController(context);
                var result = controller.Doar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
