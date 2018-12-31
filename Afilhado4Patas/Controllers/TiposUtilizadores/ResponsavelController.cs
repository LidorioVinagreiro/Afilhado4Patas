using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    /// <summary>
    /// Controller que contem todas as ações referentes ao responsavel do canil
    /// </summary>
    [Authorize(Roles = "Responsavel")]
    public class ResponsavelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Utilizadores> _userManager;
        private readonly EmailSender _emailSender;
        private readonly RazorView _razorView;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base de dados</param>
        /// <param name="userManager">Lista de utilizadores</param>
        /// <param name="emailSender">Objeto que contem a ação de envio de email</param>
        /// <param name="razorView">Objeto que constroi as paginas para o corpo dos emails</param>
        public ResponsavelController(ApplicationDbContext context, UserManager<Utilizadores> userManager, EmailSender emailSender, RazorView razorView, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
            _razorView = razorView;
            _hostingEnvironment = hostingEnvironment;
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
        /// Ação que devolve a view do perfil com a informação pessoal do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
        /// <returns>View do perfil com a informação do responsavel</returns>
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
        /// Ação quedevole a view que permite a edição das informações no perfil do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
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
            PerfilEditarDadosPessoaisViewModel modelo = new PerfilEditarDadosPessoaisViewModel
            {
                FirstName = perfil.FirstName,
                LastName = perfil.LastName,
                NIF = perfil.NIF,
                Birthday = perfil.Birthday
            };
            return View(modelo);
        }

        /// <summary>
        /// Ação que devolve a view que edita a informação atual referente a morada no perfil do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
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
            PerfilEditarMoradaViewModel modelo = new PerfilEditarMoradaViewModel
            {
                Street = perfil.Street,
                City = perfil.City,
                Postalcode = perfil.Postalcode
            };
            return View(modelo);
        }

        /// <summary>
        /// Ação que edita a informação no perfil do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do responsavel com a informação atualizada</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerfilEditarDadosPessoais(string id, PerfilEditarDadosPessoaisViewModel editarPerfilViewModel)
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
        /// Ação que altera a informação referente a morada do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do responsavel com a informação atualizada</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PerfilEditarMorada(string id, PerfilEditarMoradaViewModel editarPerfilViewModel)
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

        public ActionResult PerfilEditarFotoPerfil(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> PerfilEditarFotoPerfil(string id, ImagemPerfilUploadViewModel model)
        {
            Utilizadores user;
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    user = _context.Utilizadores.Where(u => u.Email == id).Include(p => p.Perfil).FirstOrDefault();
                    user.Perfil.Photo = model.File.FileName;
                    _context.SaveChanges();
                    var filePath = user.Perfil.Directoria + "\\" + model.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.File.CopyToAsync(fileStream);
                    return View("Perfil", user);
                }
            }
            return View();
        }

        public async Task<IActionResult> PerfilEditarPalavraPasse(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PerfilEditarPalavraPasse(string id, PerfilEditarPalavraPasseViewModel model)
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
                    user = _context.Utilizadores.Where(u => u.Email == id).FirstOrDefault();
                    if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                    {
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                        return View("PalavraPasseEditada");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Palavra-Passe antiga inserida não coincide com a sua palavra-passe");
                        model.ConfirmNewPassword = "";
                        model.NewPassword = "";
                        model.OldPassword = "";
                        return View(model);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            model.ConfirmNewPassword = "";
            model.NewPassword = "";
            model.OldPassword = "";
            return View(model);
        }

        /****************************************************************************************************/
        /******************************************** Tarefas ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Açao que devolve uma view com a listagem de todas as tarefas na base de dados
        /// </summary>
        /// <returns>View que mostra a lista de tarefas na base de dados</returns>
        public ActionResult ListaTarefas()
        {
            return View(ListaTotalTarefasModel());
        }

        /// <summary>
        /// Ação que devolve a view com a listagem das tarefas que se encontram por realizar
        /// </summary>
        /// <returns>View com a lista de tarefas por concluir</returns>
        public ActionResult TarefasARealizar()
        {
            var tarefas = _context.Tarefa.Where(t => t.Completada == false).ToList();
            return View("ListaTarefas", tarefas);
        }

        /// <summary>
        /// Ação que devolve a view de edição de uma tarefa
        /// </summary>
        /// <param name="id">Id da view view a editar</param>
        /// <returns>View de edição com a informação da tarefa</returns>
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

        /// <summary>
        /// Ação que altera a informação de uma determinada tarefa e devolve a view com a lista de tarefas ja com a tarefa atualizada
        /// </summary>
        /// <param name="tarefaModel">Modelo da view de edição de tarefa</param>
        /// <returns>view com a nova listagem de tarefas</returns>
        [HttpPost]
        public async Task<IActionResult> EditarTarefa(TarefaViewModel tarefaModel)
        {
            if (ModelState.IsValid)
            {
                var tarefa = _context.Tarefa.Where(e => e.Id == tarefaModel.Id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();
                tarefa.Descricao = tarefaModel.Descricao;
                tarefa.Fim = tarefaModel.Fim;
                tarefa.FuncionarioId = tarefa.Utilizador.Id;
                tarefa.Utilizador = tarefa.Utilizador;
                _context.SaveChanges();
                //Envio de Email
                var emailTarefaModel = new EmailTarefaViewModel(tarefa.Descricao, tarefa.Inicio, tarefa.Fim, tarefa.Utilizador.Perfil.FirstName);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/Tarefas/TarefaEditada.cshtml", emailTarefaModel);
                await _emailSender.SendEmailAsync(tarefa.Utilizador.Email, "Tarefa Editada", body);

                return View("ListaTarefas", ListaTotalTarefasModel());
            }
            tarefaModel.ListaFuncionarios = ListaTotalFuncionarios();
            return View(tarefaModel);
        }

        /// <summary>
        /// Ação que remove uma tarefa da base de dados
        /// </summary>
        /// <param name="id">Id da tarefa</param>
        /// <returns>View da lista de tarefas atualizada</returns>
        public async Task<ActionResult> ApagarTarefa(int id)
        {
            var tarefa = _context.Tarefa.Where(t => t.Id == id).Include(u => u.Utilizador).ThenInclude(p => p.Perfil).FirstOrDefault();
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();

            //Envio de Email
            var emailTarefaModel = new EmailTarefaViewModel(tarefa.Descricao, tarefa.Inicio, tarefa.Fim, tarefa.Utilizador.Perfil.FirstName);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/Tarefas/TarefaRemovida.cshtml", emailTarefaModel);
            await _emailSender.SendEmailAsync(tarefa.Utilizador.Email, "Tarefa Removida", body);

            return View("ListaTarefas", ListaTotalTarefasModel());
        }

        /// <summary>
        /// Ação que devolve a view de adicionar tarefa
        /// </summary>
        /// <returns>View de adicionar tarefa</returns>
        public ActionResult AdicionarTarefa()
        {
            TarefaViewModel tarefaModel = new TarefaViewModel();
            tarefaModel.ListaFuncionarios = ListaTotalFuncionarios();
            return View(tarefaModel);
        }

        /// <summary>
        /// Ação que altera a informação de uma tarefa e devolve a lista de tarefas atualizada
        /// </summary>
        /// <param name="modelo">Modelo da view de adicionar tarefa</param>
        /// <returns>View da lista de tarefas com a nova tarefa adicionada</returns>
        [HttpPost]
        public async Task<ActionResult> AdicionarTarefa(TarefaViewModel modelo)
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

                //Envio de Email
                var emailTarefaModel = new EmailTarefaViewModel(novaTarefa.Descricao, novaTarefa.Inicio, novaTarefa.Fim, novaTarefa.Utilizador.Perfil.FirstName);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/Tarefas/TarefaAtribuida.cshtml", emailTarefaModel);
                await _emailSender.SendEmailAsync(novaTarefa.Utilizador.Email, "Tarefa Atribuida", body);

                return View("ListaTarefas", ListaTotalTarefasModel());
            }
            modelo.ListaFuncionarios = ListaTotalFuncionarios();
            return View(modelo);
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

        /// <summary>
        /// Metodo que devolve a lista de todas as tarefas na base de dados
        /// </summary>
        /// <returns>Lista de tarefas na base de dados</returns>
        public List<TarefaViewModel> ListaTotalTarefasModel()
        {
            List<Tarefa> tarefas = _context.Tarefa.Include(u => u.Utilizador).ThenInclude(p => p.Perfil).ToList();
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

        /****************************************************************************************************/
        /******************************************** Funcionarios ******************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que devovle a view com a listagem de todos os funcionarios
        /// </summary>
        /// <returns>View da listagem de funcionarios</returns>
        public ActionResult ListaFuncionarios()
        {
            return View(ListaTotalFuncionarios());
        }

        /// <summary>
        /// Ação que devolve a view com o formulario de adição de um funcionario
        /// </summary>
        /// <returns></returns>
        public ActionResult AdicionarFuncionario()
        {
            return View();
        }

        /// <summary>
        /// Ação que adiciona um funcionario e devovle a view de listagem de funcionarios com o novo funcionario
        /// </summary>
        /// <param name="modelo">Modelo da view de adicionar funcionarios</param>
        /// <returns>View da lista de funcionarios atualizada</returns>
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

        /// <summary>
        /// Ação que remove um funcionario e atualiza a view
        /// </summary>
        /// <returns>View com a lista de funcionarios atualizada</returns>
        [HttpPost]
        public async Task<ActionResult> RemoverFuncionarios()
        {
            List<string> lista = Request.Form["funcionario"].ToList();
            foreach (var elemento in lista)
            {
                var user = _context.Utilizadores.Where(u => u.Id == elemento).Include(p => p.Perfil).FirstOrDefault();
                user.Active = false;
                _context.SaveChanges();
                //Envio de Email
                var emailModel = new EmailViewModel("", user.Perfil.FirstName);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/RemoveAccount/RemoveAccount.cshtml", emailModel);
                await _emailSender.SendEmailAsync(user.Email, "Conta Terminada", body);
            }
            return View("ListaFuncionarios", ListaTotalFuncionarios());
        }

        /// <summary>
        /// Ação que permite ver a pagina de perfil de um funcionario
        /// </summary>
        /// <param name="id">Id do funcionario</param>
        /// <returns>View com a informação do funcionario</returns>
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

        /// <summary>
        /// Metodo que devolve a lista de todos os funcionarios na base de dados
        /// </summary>
        /// <returns>Lista de todos os funcionarios</returns>
        public List<Utilizadores> ListaTotalFuncionarios()
        {
            return (from users in _context.Utilizadores
                    join a in _context.UserRoles on users.Id equals a.UserId
                    join b in _context.Roles on a.RoleId equals b.Id
                    where b.Name == "Funcionario" && users.Active == true
                    select users).Include(p => p.Perfil).ToList();
        }

        /// <summary>
        /// Metodo que devolve todas as tarefas referentes a um funcionario
        /// </summary>
        /// <param name="id">Id do funcionario</param>
        /// <returns>Lista de tarefas associadas ao funcionario</returns>
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