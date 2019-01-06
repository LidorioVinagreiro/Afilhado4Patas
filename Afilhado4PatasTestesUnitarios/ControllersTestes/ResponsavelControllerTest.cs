using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Controllers.TiposUtilizadores;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<Utilizadores> _userManager;
        private readonly EmailSender _emailSender;
        private readonly RazorView _razorView;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ApplicationDbContext context;

        public ResponsavelControllerTest()
        {
            connection = new SqliteConnection(connectionString);
            connection.Open();
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection).Options;
            context = new ApplicationDbContext(options);
        }


        [Fact]
        public async Task Index_CanLoadFromContext()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var result = controller.Index();
            //var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task About_CanLoadFromContext()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var result = controller.About();
                //var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Contact_CanLoadFromContext()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var result = controller.Contact();
                //var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Adotar_CanLoadFromContext()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var result = controller.Adotar();
                //var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Doar_CanLoadFromContext()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var result = controller.Doar();
                //var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Tarefa_ReturnViewResult_WhenModelStateValid()
        {
                var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
                var tarefa = context.Tarefa.ToList().FirstOrDefault();

                var result = controller.Tarefa(tarefa.Id);
            //var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<ViewResult>(result);
            //Assert.Equal(viewResult, result);
        }

        [Fact]
        public async Task Funcionario_ReturnViewResult_WhenModelStateValid()
        {
            var controller = new ResponsavelController(context, _userManager, _emailSender, _razorView, _hostingEnvironment);
            var funcionario = (from users in context.Utilizadores
                join a in context.UserRoles on users.Id equals a.UserId
                join b in context.Roles on a.RoleId equals b.Id
                where b.Name == "Funcionario" && users.Active == true
                select users).Include(p => p.Perfil).ToList().FirstOrDefault();

            var result = controller.VisualizarFuncionario(funcionario.Id);
            //var viewResult = Assert.IsType<ViewResult>(result);

            Assert.IsType<ViewResult>(result);
        }
    }
}
