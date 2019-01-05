using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Hosting;

namespace Afilhado4PatasTest.ControllersTest
{
    public class UtilizadorControllerTest
    {

        public string connectionString = "DataSource=:memory:";
        public SqliteConnection connection;
        public DbContextOptions<ApplicationDbContext> options;
        public ApplicationDbContext contextDB;
        public IHostingEnvironment hostingEnvironment;
        public UserManager<Utilizadores> userManager;

        public UtilizadorControllerTest(IServiceProvider serviceProvider, IHostingEnvironment hosting)
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;
            contextDB = new ApplicationDbContext(options);
            contextDB.Database.EnsureCreated();
            /*
            contextDB.Roles.AddRange(
                    new IdentityRole("Responsavel"),
                    new IdentityRole("Funcionario"),
                    new IdentityRole("Utilizador"));
                Utilizadores utilizador1 = new Utilizadores
                {
                    UserName = "danieldias@gmail.com",
                    Email = "danieldias@gmail.com",
                    EmailConfirmed = true,
                    Active = true
                };
                string password = "daniel123";
                GetMockUserManager().Object.CreateAsync(utilizador1, password);
                Perfil perfilResponsavel = new Perfil
                {
                    UtilizadorId = utilizador1.Id,
                    FirstName = "Daniel",
                    LastName = "Dias",
                    Genre = "Masculino",
                    Birthday = new DateTime(1990, 10, 10),
                    Street = "Rua Das Flores",
                    NIF = "123456789",
                    Postalcode = "2564-568",
                    City = "Setubal"
                };
            contextDB.PerfilTable.Add(perfilResponsavel);
            utilizador1.PerfilId = perfilResponsavel.Id;
                GetMockUserManager().Object.AddToRoleAsync(utilizador1, "Utilizador");                
                contextDB.SaveChanges();    */
            userManager = serviceProvider.GetRequiredService<UserManager<Utilizadores>>();
            hostingEnvironment = hosting;
        }

        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            using (contextDB)
            {
                var controller = new UtilizadorController(contextDB, hostingEnvironment, userManager);
                var result = controller.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task About_CanLoadFromContext()
        {
            using (contextDB)
            {
                var controller = new UtilizadorController(contextDB, hostingEnvironment, userManager);
                var result = controller.About();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Contact_CanLoadFromcontextDB()
        {
            using (contextDB)
            {
                var controller = new UtilizadorController(contextDB, hostingEnvironment, userManager);
                var result = controller.Contact();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Adotar_CanLoadFromcontextDB()
        {
            using (contextDB)
            {
                var controller = new UtilizadorController(contextDB, hostingEnvironment, userManager);
                var result = controller.Adotar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public async Task Doar_CanLoadFromcontextDB()
        {
            using (contextDB)
            {
                var controller = new UtilizadorController(contextDB, hostingEnvironment, userManager);
                var result = controller.Doar();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
            }
        }
    }
}
