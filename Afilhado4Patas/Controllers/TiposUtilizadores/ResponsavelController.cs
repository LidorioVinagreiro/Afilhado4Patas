using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    [Authorize(Roles = "Responsavel")]
    public class ResponsavelController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResponsavelController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("../Guest/Index");
        }

        public IActionResult About()
        {
            return View("../Guest/About");
        }

        public IActionResult Contact()
        {
            return View("../Guest/Contact");
        }

        public IActionResult Adotar()
        {
            return View("../Guest/Adotar");
        }

        public IActionResult Doar()
        {
            return View("../Guest/Doar");
        }

        // GET: Perfil
        public ActionResult Perfil(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Utilizadores.Where(u => u.Email == id).Include(p => p.Perfil).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View("../Utilizador/Perfil",user);
        }
        
        // GET: PerfilEditarDadosPessoais
        public async Task<IActionResult> PerfilEditarDadosPessoais(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var editarPerfilViewModel = _context.Utilizadores.Where(e => e.Email == id).Include(p => p.Perfil).FirstOrDefault();
            Perfil perfil = editarPerfilViewModel.Perfil;

            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }
            PerfilViewModel modelo = new PerfilViewModel
            {
                FirstName = perfil.FirstName,
                LastName = perfil.LastName,
                Street = perfil.Street,
                City = perfil.City,
                Postalcode = perfil.Postalcode,
                NIF = perfil.NIF,
                Photo = perfil.Photo,
                Birthday = perfil.Birthday,
                Genre = perfil.Genre
            };
            return View("../Utilizador/PerfilEditarDadosPessoais", modelo);
        }

        //GET PerfilEditarMorada
        public async Task<IActionResult> PerfilEditarMorada(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editarPerfilViewModel = _context.Utilizadores.Where(e => e.Email == id).Include(p => p.Perfil).FirstOrDefault();
            Perfil perfil = editarPerfilViewModel.Perfil;

            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }
            PerfilViewModel modelo = new PerfilViewModel
            {
                FirstName = perfil.FirstName,
                LastName = perfil.LastName,
                Street = perfil.Street,
                City = perfil.City,
                Postalcode = perfil.Postalcode,
                NIF = perfil.NIF,
                Photo = perfil.Photo,
                Birthday = perfil.Birthday,
                Genre = perfil.Genre
            };
            return View("../Utilizador/PerfilEditarMorada", modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerfilEditarDadosPessoais(string id, PerfilViewModel editarPerfilViewModel)
        {
            Utilizadores user;
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user = _context.Utilizadores.Where(e => e.Email == id).Include(p => p.Perfil).FirstOrDefault();
                    var perfil = _context.PerfilTable.FirstOrDefault(p => p.UtilizadorId == user.Id);
                    perfil.FirstName = editarPerfilViewModel.FirstName;
                    perfil.LastName = editarPerfilViewModel.LastName;
                    perfil.NIF = editarPerfilViewModel.NIF;
                    perfil.Birthday = editarPerfilViewModel.Birthday;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                var userUpdated = _context.Utilizadores.Where(e => e.Email == id).Include(p => p.Perfil).FirstOrDefault();
                return View("Perfil", userUpdated);
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerfilEditarMorada(string id, PerfilViewModel editarPerfilViewModel)
        {
            Utilizadores user;
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user = _context.Utilizadores.Where(e => e.Email == id).Include(p => p.Perfil).FirstOrDefault();
                    var perfil = _context.PerfilTable.FirstOrDefault(p => p.UtilizadorId == user.Id);
                    perfil.City = editarPerfilViewModel.City;
                    perfil.Street = editarPerfilViewModel.Street;
                    perfil.Postalcode = editarPerfilViewModel.Postalcode;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return View("Perfil", user);
            }
            return View("Index");
        }

        public ActionResult Tarefas()
        {
            List<Tarefa> tarefas = _context.Tarefa.ToList();
            return View(tarefas);
        }

        public ActionResult AdicionarFuncionarios()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult AdicionarFuncionario()
        {
            List<string> lista = Request.Form["funcionario"].ToList();
            foreach (var elemento in lista)
            {
                var user = _context.Utilizadores.Where(u => u.Id == elemento).FirstOrDefault();
                user.Active = false;
                _context.SaveChanges();
            }
            //lista.items
            List<Utilizadores> funcionarios = (from users in _context.Utilizadores
                                               join a in _context.UserRoles on users.Id equals a.UserId
                                               join b in _context.Roles on a.RoleId equals b.Id
                                               where b.Name == "Funcionario" && users.Active == true
                                               select users).Include(p => p.Perfil).ToList();
            return View("ListaFuncionarios", funcionarios);
        }

        [HttpPost]
        public ActionResult RemoverFuncionarios()
        {
            List<string> lista = Request.Form["funcionario"].ToList();
            foreach (var elemento in lista)
            {
                var user = _context.Utilizadores.Where(u => u.Id == elemento).FirstOrDefault();
                user.Active = false;
                _context.SaveChanges();
            }
            //lista.items
            List<Utilizadores> funcionarios = (from users in _context.Utilizadores
                                               join a in _context.UserRoles on users.Id equals a.UserId
                                               join b in _context.Roles on a.RoleId equals b.Id
                                               where b.Name == "Funcionario" && users.Active == true
                                               select users).Include(p => p.Perfil).ToList();
            return View("ListaFuncionarios", funcionarios);
        }

        public ActionResult ListaFuncionarios()
        {
            List<Utilizadores> funcionarios = (from users in _context.Utilizadores
                                               join a in _context.UserRoles on users.Id equals a.UserId
                                               join b in _context.Roles on a.RoleId equals b.Id
                                               where b.Name == "Funcionario" && users.Active == true
                                               select users).Include(p => p.Perfil).ToList();
            return View(funcionarios);
        }


        private bool PerfilExists(int id)
        {
            return _context.PerfilTable.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}