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
    /// <summary>
    /// Este controller fornece todas as açôes para os funcionarios do sitema
    /// </summary>
    [Authorize(Roles = "Funcionario")]
    public class FuncionarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base dados</param>
        public FuncionarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ação que devolve a view da pagina principal do site
        /// </summary>
        /// <returns>View principal do site</returns>
        public IActionResult Index()
        {
            return View("../Guest/Index");
        }

        /// <summary>
        /// Ação que devolve a view das informações adicionais sobre o site
        /// </summary>
        /// <returns>View do Sobre nós</returns>
        public IActionResult About()
        {
            return View("../Guest/About");
        }

        /// <summary>
        /// Ação que devolve a view com os contactos do nosso site
        /// </summary>
        /// <returns>View dos contactos</returns>
        public IActionResult Contact()
        {
            return View("../Guest/Contact");
        }


        /// <summary>
        /// Ação que devovle a view das adoções
        /// </summary>
        /// <returns>View de adoções</returns>
        public IActionResult Adotar()
        {
            return View("../Guest/Adotar");
        }

        /// <summary>
        /// Ação que devolve a view de doações
        /// </summary>
        /// <returns>View das doações</returns>
        public IActionResult Doar()
        {
            return View("../Guest/Doar");
        }

        /****************************************************************************************************/
        /******************************************** Perfil ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que devolve a view do perfil com a informação pessoal do funcionario atual
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <returns>View do perfil com a informação do funcionario</returns>
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

        /// <summary>
        /// Ação quedevole a view que permite a edição das informações no perfil do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <returns>View de edição dos dados pessoais do perfil</returns>
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

        /// <summary>
        /// Ação que devolve a view que edita a informação atual referente a morada no perfil do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <returns>View de edição da morada</returns>
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

        /// <summary>
        /// Ação que edita a informação no perfil do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil de funcionario com a informação atualizada</returns>
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

        /// <summary>
        /// Ação que altera a informação referente a morada do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil de funcionario com a informação atualizada</returns>
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
        /******************************************** Tarefa  ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que devolve a lista de tarefas associadas a um funcionário
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <returns>lista das tarefas associadas a um funcionario</returns>
        public ActionResult MinhasTarefas(string id)
        {
            return View(ListaTotalTarefasUtilizadorModel(id));
        }
        
        /// <summary>
        /// Ação que devolve a view com a informação referente a uma tarefa
        /// </summary>
        /// <param name="id">Id da terefa</param>
        /// <returns>View que mostra a informação de uma tarefa</returns>
        public ActionResult Tarefa(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Tarefa tarefa = _context.Tarefa.Where(t => t.Id == id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();
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

        /// <summary>
        /// Metodo que devolve uma lista com todas as tarefas associadas a um funcionario
        /// </summary>
        /// <param name="id">Id do funcionario</param>
        /// <returns>lista das tarefas de um utilizador</returns>
        public List<TarefaViewModel> ListaTotalTarefasUtilizadorModel(string id)
        {
            List<Tarefa> tarefas =  (from tarefa in _context.Tarefa
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

        /// <summary>
        /// Metodo que devolve a lista de todos os funcionarios na base de dados
        /// </summary>
        /// <returns>Lista de funcionários</returns>
        public List<Utilizadores> ListaTotalFuncionarios()
        {
            return (from users in _context.Utilizadores
                    join a in _context.UserRoles on users.Id equals a.UserId
                    join b in _context.Roles on a.RoleId equals b.Id
                    where b.Name == "Funcionario" && users.Active == true
                    select users).Include(p => p.Perfil).ToList();
        }

        /// <summary>
        /// Metodo que verifica se um perfil existe na base de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Perfil encontrado</returns>
        private bool PerfilExists(int id)
        {
            return _context.PerfilTable.Any(e => e.Id == id);
        }

        /// <summary>
        /// Ação que devolve a view de erro, caso ocorra um erro esta view e devolvida com a informação do erro
        /// </summary>
        /// <returns>View de erro</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}