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
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Afilhado4PatasTest.ControllersTest
{
    public class FuncionarioControllerTest
    {

        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;
        public IHostingEnvironment hostingEnvironment;
        public ILogger<Utilizadores> logger;
        public UserManager<Utilizadores> userManager;

        public FuncionarioControllerTest(IServiceProvider serviceProvider, IHostingEnvironment hosting, ILogger<Utilizadores> Ilogger)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            userManager = serviceProvider.GetRequiredService<UserManager<Utilizadores>>();
            hostingEnvironment = hosting;
            logger = Ilogger;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            using (var context = new ApplicationDbContext(options))
            {
                var controller = new FuncionarioController(context, hostingEnvironment, logger, userManager);
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
                var controller = new FuncionarioController(context, hostingEnvironment, logger, userManager);
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
                var controller = new FuncionarioController(context, hostingEnvironment, logger, userManager);
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
                var controller = new FuncionarioController(context, hostingEnvironment, logger, userManager);
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
                var controller = new FuncionarioController(context, hostingEnvironment, logger, userManager);
                var result = controller.Doar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}