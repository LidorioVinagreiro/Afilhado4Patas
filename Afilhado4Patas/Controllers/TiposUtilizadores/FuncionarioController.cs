using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    /// <summary>
    /// Este controller fornece todas as açôes para os funcionarios do sitema
    /// </summary>
    [Authorize(Roles = "Funcionario")]
    public class FuncionarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<Utilizadores> _logger;
        private readonly UserManager<Utilizadores> _userManager;
        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base dados</param>
        public FuncionarioController(
            ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment,
            ILogger<Utilizadores> logger,
            UserManager<Utilizadores> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _userManager = userManager;
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
            return View(new PerfilEditarPalavraPasseViewModel());
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
        /****************************************************************************************************/
        /******************************************** Animais ***********************************************/
        /****************************************************************************************************/

        public ActionResult RegistarAnimal() {
            var model = new RegistarAnimalViewModel();
            model.Categorias = _context.Categorias.Select(categ => new SelectListItem()
            {
                Value = categ.Id.ToString(),
                Text = categ.Nome
            }).ToList();
            return View("RegistoAnimal", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistarAnimal([Bind] RegistarAnimalViewModel model) {
            if (ModelState.IsValid)
            {
                Animal entradaAnimal = new Animal
                {
                    NomeAnimal = model.NomeAnimal,
                    Descricao = model.Descricao,
                    DataNasc = model.DataNasc,
                    Porte = model.Porte,
                    Peso = model.Peso,
                    RacaId = model.RacaId,
                    Adoptado = false,
                    Ativo = false
                };
                _context.Animais.Add(entradaAnimal);
                _context.SaveChanges();               
                entradaAnimal.DirectoriaAnimal = _hostingEnvironment.WebRootPath+"\\Animais\\" + entradaAnimal.Id;
                if (!CreateFolder(entradaAnimal.DirectoriaAnimal)) {
                    _logger.LogInformation("Falha ao criar a pasta do animal");
                }
                _context.SaveChanges();
                return View("RegistoCompleto");
            }
            model.Categorias = _context.Categorias.Select(categ => new SelectListItem()
            {
                Value = categ.Id.ToString(),
                Text = categ.Nome
            }).ToList();
            return View("RegistoAnimal", model);
        }

        public ActionResult VerListaAnimais() {
            List<Animal> model = _context.Animais.ToList();
            return View("ListaAnimais", model);
        }

        public ActionResult ApagarAnimal(int id)
        {
            Animal removerAnimal = _context.Animais.Where(animal => animal.Id == id).FirstOrDefault();
            removerAnimal.Ativo = false;
            _context.SaveChanges();
            List<Animal> model = _context.Animais.ToList();
            return View("ListaAnimais", model);
        }


        public ActionResult DetalhesAnimal(int id)
        {
            Animal model = _context.Animais.Where(animal => animal.Id == id).FirstOrDefault();
            return View("DetalhesAnimal", model);
        }

        public ActionResult EditarAnimal(int id) {
            Animal model = _context.Animais.Where(animal => animal.Id == id).Include(raca => raca.RacaAnimal).FirstOrDefault();
            EditarAnimalViewModel modelo = new EditarAnimalViewModel
            {
                Categorias = _context.Categorias.Select(categ => new SelectListItem()
                {
                    Value = categ.Id.ToString(),
                    Text = categ.Nome
                }).ToList(),
                NomeAnimal = model.NomeAnimal,
                Peso = model.Peso,
                Descricao = model.Descricao,
                DataNasc = model.DataNasc,
                Porte = model.Porte,
                Ativo = model.Ativo,
                Adoptado = model.Adoptado
            }; 
            if(!(model.RacaAnimal is null)){
                modelo.CategoriaId = model.RacaAnimal.CategoriaId;
            }

            if (!(model.RacaAnimal is null))
            {
                modelo.RacaId = model.RacaAnimal.Id;
            }
            return View("EditarAnimal", modelo);
        }

        [HttpPost]
        public ActionResult EditarAnimal(Animal model) {
            if (ModelState.IsValid)
            {
                _context.Animais.Update(model);
                _context.SaveChanges();
                return View("ListaAnimais",_context.Animais.ToList());
            }
            else {
                ModelState.AddModelError("", "Informação errada EditarAnimal HTTPPOST");
                return View("EditarAnimal", model);
            }
        }

        private bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return Directory.Exists(path);
            }
            return Directory.Exists(path);
        }

        [HttpGet]
        public JsonResult RacasPorCategoria(int CategoriaID) {
            var racas = (from racasAux in _context.Racas
                         where racasAux.CategoriaId == CategoriaID
                         orderby racasAux.NomeRaca
                                     select new Raca { Id = racasAux.Id, NomeRaca = racasAux.NomeRaca }).ToList();
            return Json(new SelectList(racas,"Id","NomeRaca"));
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