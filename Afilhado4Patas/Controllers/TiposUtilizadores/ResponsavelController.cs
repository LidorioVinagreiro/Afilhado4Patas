using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    [Authorize(Roles = "Responsavel")]
    public class ResponsavelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly EmailSender _emailSender;

        public ResponsavelController(ApplicationDbContext context, UserManager<Utilizadores> userManager, EmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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

        /****************************************************************************************************/
        /******************************************** Perfil ***********************************************/
        /****************************************************************************************************/

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
            return View(user);
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
            return View(modelo);
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
            return View(modelo);
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
            return View(editarPerfilViewModel);
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
            return View(editarPerfilViewModel);
        }

        /****************************************************************************************************/
        /******************************************** Tarefas ***********************************************/
        /****************************************************************************************************/

        public ActionResult ListaTarefas()
        {            
            return View(ListaTotalTarefasModel());
        }

        public ActionResult TarefasARealizar()
        {
            var tarefas = _context.Tarefa.Where(t => t.Completada == false).ToList();
            return View("ListaTarefas", tarefas);
        }

        public ActionResult EditarTarefa(int id)
        {
            Tarefa tarefa = _context.Tarefa.Where(u => u.Id == id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();

            TarefaViewModel tarefaModel = new TarefaViewModel
            {
                Id = tarefa.Id,
                Descricao = tarefa.Descricao,
                Fim = tarefa.Fim,
                Inicio = tarefa.Inicio,
                Utilizador = tarefa.Utilizador,
                FuncionarioId = tarefa.FuncionarioId,
                ListaFuncionarios = ListaTotalFuncionarios(),
                Completada = tarefa.Completada
            };
            return View(tarefaModel);
        }

        [HttpPost]
        public ActionResult EditarTarefa(TarefaViewModel tarefaModel)
        {
            if (ModelState.IsValid)
            {
                var tarefa = _context.Tarefa.Where(e => e.Id == tarefaModel.Id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();
                tarefa.Descricao = tarefaModel.Descricao;
                tarefa.Fim = tarefaModel.Fim;
                tarefa.FuncionarioId = tarefa.Utilizador.Id;
                tarefa.Utilizador = tarefa.Utilizador;
                _context.SaveChanges();
                return View("ListaTarefas", ListaTotalTarefasModel());
            }
            return View(tarefaModel);
        }

        public ActionResult ApagarTarefa(int id)
        {
            var tarefa = _context.Tarefa.FirstOrDefault(t => t.Id == id);
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
            
            return View("ListaTarefas", ListaTotalTarefasModel());
        }

        public ActionResult AdicionarTarefa()
        {
            TarefaViewModel tarefaModel = new TarefaViewModel();
            tarefaModel.ListaFuncionarios = ListaTotalFuncionarios();
            return View(tarefaModel);
        }

        [HttpPost]
        public ActionResult AdicionarTarefa(TarefaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Utilizadores funcionario = _context.Utilizadores.Where(u => u.Id == modelo.FuncionarioId).Include(p => p.Perfil).FirstOrDefault();
                Tarefa novaTarefa = new Tarefa
                {
                    FuncionarioId = modelo.FuncionarioId,
                    Descricao = modelo.Descricao,
                    Inicio = modelo.Inicio,
                    Fim = modelo.Fim,
                    Utilizador = funcionario,
                    Completada = false
                };
                _context.Tarefa.Add(novaTarefa);
                _context.SaveChanges();
                _emailSender.SendEmailAsync(funcionario.Email, "Atribuida Nova Tarefa",
                    "Foi lhe atribuida uma nova tarefa. Dirija-se ao Sistema de Informação para verificar a sua nova Tarefa");
                return View("ListaTarefas", ListaTotalTarefasModel());
            }
            return View(modelo);
        }

        public ActionResult Tarefa(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Tarefa tarefa = _context.Tarefa.Where(u => u.Id == id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();
            TarefaViewModel tarefaModel = new TarefaViewModel
            {
                Id = tarefa.Id,
                FuncionarioId = tarefa.FuncionarioId,
                Inicio = tarefa.Inicio,
                Fim = tarefa.Fim,
                Descricao = tarefa.Descricao,
                Completada = tarefa.Completada,
                Utilizador = tarefa.Utilizador,
                ListaFuncionarios = ListaTotalFuncionarios()
            };
            return View(tarefaModel);
        }

        public List<TarefaViewModel> ListaTotalTarefasModel()
        {
            List<Tarefa> tarefas = _context.Tarefa.Include(u => u.Utilizador).ThenInclude(p => p.Perfil).ToList();
            List<TarefaViewModel> tarefasModel = new List<TarefaViewModel>();
            List<Utilizadores> funcionarios = new List<Utilizadores>();
            foreach (var tarefa in tarefas)
            {
                TarefaViewModel t = new TarefaViewModel {
                    Id = tarefa.Id,
                    FuncionarioId = tarefa.FuncionarioId,
                    Inicio = tarefa.Inicio,
                    Fim = tarefa.Fim,
                    Descricao = tarefa.Descricao,
                    Completada = tarefa.Completada,
                    Utilizador = tarefa.Utilizador,
                    ListaFuncionarios = ListaTotalFuncionarios()
                };  
                tarefasModel.Add(t);
            }
            return tarefasModel;
        }

        /****************************************************************************************************/
        /******************************************** Funcionarios ******************************************/
        /****************************************************************************************************/

        public ActionResult ListaFuncionarios()
        {
            return View(ListaTotalFuncionarios());
        }

        public ActionResult AdicionarFuncionarios()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<ActionResult> AdicionarFuncionario(FuncionarioViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var novoFuncionario = new Utilizadores
                {
                    UserName = modelo.Email,
                    Email = modelo.Email,
                    EmailConfirmed = true,
                    Active = true
                };
                var user = await _userManager.FindByEmailAsync(novoFuncionario.Email);
                if (user == null)
                {
                    var createFuncionario = await _userManager.CreateAsync(novoFuncionario, modelo.Password);
                    Perfil perfilFuncionario = new Perfil
                    {
                        UtilizadorId = novoFuncionario.Id,
                        FirstName = modelo.Nome,
                        LastName = modelo.Apelido,
                        Genre = modelo.Genero,
                        Birthday = modelo.DataNascimento
                    };
                    _context.PerfilTable.Add(perfilFuncionario);
                    _context.SaveChanges();
                    novoFuncionario.PerfilId = perfilFuncionario.Id;
                    _context.SaveChanges();
                    if (createFuncionario.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(novoFuncionario, "Funcionario");
                    }
                }
                return View("ListaFuncionarios", ListaTotalFuncionarios());
            }
            return View(modelo);
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
            return View("ListaFuncionarios", ListaTotalFuncionarios());
        }

        public ActionResult VisualizarFuncionario(string id)
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
            FuncionarioViewModel funcionario = new FuncionarioViewModel
            {
                Utilizador = user,
                ListaTarefas = ListaTotalTarefasUtilizadorModel(user.Email)
            };            
            return View(funcionario);
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

        public List<Utilizadores> ListaTotalFuncionarios()
        {
            return (from users in _context.Utilizadores
                                               join a in _context.UserRoles on users.Id equals a.UserId
                                               join b in _context.Roles on a.RoleId equals b.Id
                                               where b.Name == "Funcionario" && users.Active == true
                                               select users).Include(p => p.Perfil).ToList();
        }

        public List<TarefaViewModel> ListaTotalTarefasUtilizadorModel(string id)
        {
            List<Tarefa> tarefas = (from tarefa in _context.Tarefa
                                    join a in _context.Utilizadores on tarefa.FuncionarioId equals a.Id
                                    where a.Email == id
                                    select tarefa).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).ToList();
            List<TarefaViewModel> tarefasModel = new List<TarefaViewModel>();
            List<Utilizadores> funcionarios = new List<Utilizadores>();
            foreach (var tarefa in tarefas)
            {
                TarefaViewModel t = new TarefaViewModel
                {
                    Id = tarefa.Id,
                    FuncionarioId = tarefa.FuncionarioId,
                    Inicio = tarefa.Inicio,
                    Fim = tarefa.Fim,
                    Descricao = tarefa.Descricao,
                    Completada = tarefa.Completada,
                    Utilizador = tarefa.Utilizador,
                    ListaFuncionarios = ListaTotalFuncionarios()
                };
                tarefasModel.Add(t);
            }
            return tarefasModel;
        }

    }
}