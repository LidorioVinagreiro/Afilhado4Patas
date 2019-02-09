using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Areas.Identity.Services;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.Estatisticas;
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
        private readonly EmailSender _emailSender;
        private readonly RazorView _razorView;
        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base dados</param>
        public FuncionarioController(
            ApplicationDbContext context,
            IHostingEnvironment hostingEnvironment,
            ILogger<Utilizadores> logger,
            UserManager<Utilizadores> userManager, 
            RazorView razorView,
            EmailSender emailSender)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _userManager = userManager;
            _emailSender = emailSender;
            _razorView = razorView;
        }
        /// <summary>
        /// Ação que devolve a view da pagina principal do site
        /// </summary>
        /// <returns>View principal do site</returns>
        public IActionResult Index()
        {
            return View("../Shared/Index");
        }

        /// <summary>
        /// Ação que devolve a view das informações adicionais sobre o site
        /// </summary>
        /// <returns>View do Sobre nós</returns>
        public IActionResult About()
        {
            return View("../Shared/About");
        }

        /// <summary>
        /// Ação que devolve a view com os contactos do nosso site
        /// </summary>
        /// <returns>View dos contactos</returns>
        public IActionResult Contact()
        {
            return View("../Shared/Contact");
        }


        /// <summary>
        /// Ação que devovle a view das adoções
        /// </summary>
        /// <returns>View de adoções</returns>
        public IActionResult Adotar()
        {
            return View("../Shared/Adotar");
        }

        /// <summary>
        /// Ação que devolve a view de doações
        /// </summary>
        /// <returns>View das doações</returns>
        public IActionResult Doar()
        {
            return View("../Shared/Doar");
        }

        /// <summary>
        /// Ação que devolve a view dos animais
        /// </summary>
        /// <returns>View dos animais</returns>
        public IActionResult Animais()
        {
            return View("../Shared/Animais");
        }

        /// <summary>
        /// Ação que devolve a view de ficha de um animal
        /// </summary>
        /// <returns>View da ficha de um animal</returns>
        public IActionResult FichaAnimal(int id)
        {
            Animal animal = _context.Animais.Where(a => a.Id == id).Include(b => b.RacaAnimal).ThenInclude(c => c.CategoriaRaca).FirstOrDefault();
            animal.PorteAnimal = _context.Portes.Where(p => p.Id == animal.PorteId).FirstOrDefault();
            if (animal.Adoptado)
            {
                animal.Adotantes = new List<Utilizadores>();
                foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).Include(u => u.Adotante_User).ToList())
                {
                    Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                    animal.Adotantes.Add(user);
                }
            }
            return View("../Shared/FichaAnimal", animal);
        }

        public ActionResult Dashboard(string id)
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

        public ActionResult VisualizarUtilizador(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        /****************************************************************************************************/
        /******************************************** Perfil ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que devolve a view do perfil com a informação pessoal do funcionario
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
            PerfilEditarMoradaViewModel modelo = new PerfilEditarMoradaViewModel
            {
                Street = perfil.Street,
                City = perfil.City,
                Postalcode = perfil.Postalcode
            };
            return View(modelo);
        }

        /// <summary>
        /// Ação que edita a informação no perfil do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do funcionario com a informação atualizada</returns>
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
                return View("Dashboard", userUpdated);
            }
            return View(editarPerfilViewModel);
        }

        /// <summary>
        /// Ação que altera a informação referente a morada do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do funcionario com a informação atualizada</returns>
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
                return View("Dashboard", user);
            }
            return View(editarPerfilViewModel);
        }

        /// <summary>
        /// Ação que devolve a view que edita a informação atual referente a foto de perfil do funcionario
        /// </summary>
        /// <param name="id">Id do funcionario atual</param>
        /// <returns>View de edição de foto de perfil</returns>
        public ActionResult PerfilEditarFotoPerfil(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }

        /// <summary>
        /// Ação que altera a informação referente a foto de perfil do funcionario
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do funcionario com a informação atualizada</returns>
        [HttpPost]
        public async Task<ActionResult> PerfilEditarFotoPerfil(string id, ImagemUploadViewModel model)
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
                    return View("Dashboard", user);
                }
            }
            return View();
        }

        /// <summary>
        /// Ação que devolve a view que edita a informação atual referente a palavra passe do funcionario
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <returns>View de edição de palavra passe</returns>
        public async Task<IActionResult> PerfilEditarPalavraPasse(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(new PerfilEditarPalavraPasseViewModel());
        }


        /// <summary>
        /// Ação que altera a informação referente a palvra passe do funcionario
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do funcionario com a informação atualizada</returns>
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
               
        /// <summary>
        /// Ação que devolve a view de registo de um animall
        /// </summary>
        /// <returns>View de registo de um animal</returns>
        public ActionResult RegistoAnimal()
        {
            var model = new RegistarAnimalViewModel();
            model.Categorias = _context.Categorias.Select(categ => new SelectListItem()
            {
                Value = categ.Id.ToString(),
                Text = categ.Nome
            }).ToList();
            model.Portes = _context.Portes.Select(porte => new SelectListItem()
            {
                Value = porte.Id.ToString(),
                Text = porte.TipoPorte
            }).ToList();
            return View(model);
        }

        /// <summary>
        /// Ação que regista um animal
        /// </summary>
        /// <param name="model">Modelo da informação na view</param>
        /// <returns>View de confirmacao do registo do animal</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistoAnimal([Bind] RegistarAnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Animal entradaAnimal = new Animal
                {
                    NomeAnimal = model.NomeAnimal,
                    Sexo = model.Sexo,
                    Descricao = model.Descricao,
                    DataNasc = model.DataNasc,
                    PorteId = model.PorteId,
                    Peso = model.Peso,
                    RacaId = model.RacaId,
                    Adoptado = false,
                    Ativo = true
                };
                _context.Animais.Add(entradaAnimal);
                _context.SaveChanges();
                entradaAnimal.DirectoriaAnimal = _hostingEnvironment.WebRootPath + "\\Animais\\" + entradaAnimal.Id;
                CreateFolder(entradaAnimal.DirectoriaAnimal);
                CreateFolder(entradaAnimal.DirectoriaAnimal + "\\Anexos");
                CreateFolder(entradaAnimal.DirectoriaAnimal + "\\Galeria");
                _context.SaveChanges();
                return View("RegistoAnimalCompleto");
            }

            model.Categorias = _context.Categorias.Select(categ => new SelectListItem()
            {
                Value = categ.Id.ToString(),
                Text = categ.Nome
            }).ToList();
            model.Portes = _context.Portes.Select(porte => new SelectListItem()
            {
                Value = porte.Id.ToString(),
                Text = porte.TipoPorte
            }).ToList();
            return View(model);
        }

        /// <summary>
        /// Ação que devolve a view de lista de animais
        /// </summary>
        /// <returns>View da lista dos animais</returns>
        public ActionResult ListaAnimais()
        {
            List<Animal> model = new List<Animal>();
            foreach (var animal in _context.Animais.ToList())
            {
                animal.RacaAnimal = _context.Racas.Where(r => r.Id == animal.RacaId).Include(c => c.CategoriaRaca).FirstOrDefault();
                animal.PorteAnimal = _context.Portes.Where(p => p.Id == animal.PorteId).FirstOrDefault();
                if (animal.Adoptado)
                {
                    animal.Adotantes = new List<Utilizadores>();
                    foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).Include(u => u.Adotante_User).ToList())
                    {
                        Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                        animal.Adotantes.Add(user);
                    }
                }
                model.Add(animal);
            }
            return View(model);
        }

        /// <summary>
        /// Ação que devolve a view de lista de animais apos remover um determinado animal
        /// </summary>
        /// <param name="id">Id do animal a ser removido</param>
        /// <returns>View da lista dos animais</returns>
        public ActionResult ApagarAnimal(int id)
        {
            Animal removerAnimal = _context.Animais.Where(animal => animal.Id == id).FirstOrDefault();
            removerAnimal.Ativo = false;
            _context.SaveChanges();

            List<Animal> model = new List<Animal>();
            foreach (var animal in _context.Animais.ToList())
            {
                animal.RacaAnimal = _context.Racas.Where(r => r.Id == animal.RacaId).Include(c => c.CategoriaRaca).FirstOrDefault();
                animal.PorteAnimal = _context.Portes.Where(p => p.Id == animal.PorteId).FirstOrDefault();
                if (animal.Adoptado)
                {
                    animal.Adotantes = new List<Utilizadores>();
                    foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).Include(u => u.Adotante_User).ToList())
                    {
                        Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                        animal.Adotantes.Add(user);
                    }
                }
                model.Add(animal);
            }
            return View("ListaAnimais", model);
        }

        /// <summary>
        /// Ação que devolve a view de lista de animais apos ativacao de um animal que teria sido removido
        /// </summary>
        /// <param name="id">Id do animal a ser ativado</param>
        /// <returns>View da lista dos animais</returns>
        public ActionResult AtivarAnimal(int id)
        {
            Animal removerAnimal = _context.Animais.Where(animal => animal.Id == id).FirstOrDefault();
            removerAnimal.Ativo = true;
            _context.SaveChanges();

            List<Animal> model = new List<Animal>();
            foreach (var animal in _context.Animais.ToList())
            {
                animal.RacaAnimal = _context.Racas.Where(r => r.Id == animal.RacaId).Include(c => c.CategoriaRaca).FirstOrDefault();
                animal.PorteAnimal = _context.Portes.Where(p => p.Id == animal.PorteId).FirstOrDefault();
                if (animal.Adoptado)
                {
                    animal.Adotantes = new List<Utilizadores>();
                    foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).Include(u => u.Adotante_User).ToList())
                    {
                        Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                        animal.Adotantes.Add(user);
                    }
                }
                model.Add(animal);
            }
            return View("ListaAnimais", model);
        }

        /// <summary>
        /// Ação que devolve a view de um perfil detalhado de um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <returns>View do perfil detalhado do animal</returns>
        public ActionResult DetalhesAnimal(int id)
        {
            Animal model = _context.Animais.Where(animal => animal.Id == id).FirstOrDefault();
            model.RacaAnimal = _context.Racas.Where(r => r.Id == model.RacaId).Include(c => c.CategoriaRaca).FirstOrDefault();
            model.PorteAnimal = _context.Portes.Where(p => p.Id == model.PorteId).FirstOrDefault();
            model.Adotantes = new List<Utilizadores>();
            if (model.Adoptado)
            {
                foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == model.Id).Include(u => u.Adotante_User).ToList())
                {
                    Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                    model.Adotantes.Add(user);
                }
            }
            List<string> anexos = new List<string>();
            foreach (var anexo in Directory.GetFiles(_hostingEnvironment.WebRootPath + "\\Animais\\" + model.Id + "\\Anexos\\").ToList())
            {
                var anexo_temp = Path.GetFileName(anexo);
                anexos.Add(anexo_temp);
            }
            List<string> fotos = new List<string>();
            foreach (var foto in Directory.GetFiles(_hostingEnvironment.WebRootPath + "\\Animais\\" + model.Id + "\\Galeria\\").ToList())
            {
                var foto_temp = Path.GetFileName(foto);
                fotos.Add(foto_temp);
            }
            DetalhesAnimal detalhes = new DetalhesAnimal
            {
                Animal = model,
                FicheirosAnexos = anexos,
                FicheirosGaleria = fotos,
                CaminhoAnexos = _hostingEnvironment.WebRootPath + "\\Animais\\" + model.Id + "\\Anexos\\",
                CaminhoGaleria = _hostingEnvironment.WebRootPath + "\\Animais\\" + model.Id + "\\Galeria\\"
            };            
            return View(detalhes);
        }

        /// <summary>
        /// Ação que devolve a view de editar um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <returns>View de editar animal</returns>
        public ActionResult EditarAnimal(int id)
        {
            Animal animal = _context.Animais.Where(a => a.Id == id).Include(raca => raca.RacaAnimal).ThenInclude(c => c.CategoriaRaca).FirstOrDefault();
            animal.PorteAnimal = _context.Portes.Where(p => p.Id == animal.PorteId).FirstOrDefault();
            if (animal.Adoptado)
            {
                animal.Adotantes = new List<Utilizadores>();
                foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).Include(u => u.Adotante_User).ToList())
                {
                    Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                    animal.Adotantes.Add(user);
                }
            }
            EditarAnimalViewModel modelo = new EditarAnimalViewModel
            {
                Categorias = _context.Categorias.Select(categ => new SelectListItem()
                {
                    Value = categ.Id.ToString(),
                    Text = categ.Nome
                }).ToList(),
                Portes = _context.Portes.Select(porte => new SelectListItem()
                {
                    Value = porte.Id.ToString(),
                    Text = porte.TipoPorte
                }).ToList(),
                Id = animal.Id,
                NomeAnimal = animal.NomeAnimal,
                Sexo = animal.Sexo,
                Peso = animal.Peso,
                Descricao = animal.Descricao,
                DataNasc = animal.DataNasc,
                PorteId = animal.PorteId,
                Ativo = animal.Ativo,
                Adoptado = animal.Adoptado
            };
            if (!(animal.RacaAnimal is null))
            {
                modelo.CategoriaId = animal.RacaAnimal.CategoriaId;
            }

            if (!(animal.RacaAnimal is null))
            {
                modelo.RacaId = animal.RacaAnimal.Id;
            }
            return View(modelo);
        }

        /// <summary>
        /// Ação que edita um animal
        /// </summary>
        /// <param name="model">Modelo da informação na view</param>
        /// <returns>View de lista de animais apos edicao do mesmo</returns>
        [HttpPost]
        public ActionResult EditarAnimal(EditarAnimalViewModel model)
        {
            if (ModelState.IsValid)
            {
                Animal animal = _context.Animais.Where(a => a.Id == model.Id).FirstOrDefault();
                animal.NomeAnimal = model.NomeAnimal;
                animal.DataNasc = model.DataNasc;
                animal.PorteId = model.PorteId;
                animal.Descricao = model.Descricao;
                animal.Peso = model.Peso;
                animal.RacaId = model.RacaId;
                _context.SaveChanges();

                List<Animal> lista = new List<Animal>();
                foreach (var auxiliar in _context.Animais.ToList())
                {
                    auxiliar.RacaAnimal = _context.Racas.Where(r => r.Id == auxiliar.RacaId).Include(c => c.CategoriaRaca).FirstOrDefault();
                    auxiliar.PorteAnimal = _context.Portes.Where(p => p.Id == auxiliar.PorteId).FirstOrDefault();
                    if (auxiliar.Adoptado)
                    {
                        auxiliar.Adotantes = new List<Utilizadores>();
                        foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == auxiliar.Id).Include(u => u.Adotante_User).ToList())
                        {
                            Utilizadores user = _context.Utilizadores.Where(x => x.PerfilId == adotantes.AdotanteId).FirstOrDefault();
                            auxiliar.Adotantes.Add(user);
                        }
                    }
                    lista.Add(auxiliar);
                }
                return View("ListaAnimais", lista);
            }            
            ModelState.AddModelError("", "Informação errada EditarAnimal HTTPPOST");
            model.Categorias = _context.Categorias.Select(categ => new SelectListItem()
            {
                Value = categ.Id.ToString(),
                Text = categ.Nome
            }).ToList();
            model.Portes = _context.Portes.Select(porte => new SelectListItem()
            {
                Value = porte.Id.ToString(),
                Text = porte.TipoPorte
            }).ToList();
            return View(model);
        }

        /// <summary>
        /// Ação que devolve a view de editar foto de perfil de um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <returns>View de editar foto de perfil de um animal</returns>
        public ActionResult EditarAnimalFotoPerfil(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            AnimalImageUploadViewModel model = new AnimalImageUploadViewModel
            {
                animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault()
            };
            return View(model);
        }

        /// <summary>
        /// Ação que edita a foto de perfil de um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <param name="model">Modelo da informação na view</param>
        /// <returns>View de detalhes do animal apos edicao da foto</returns>
        [HttpPost]
        public async Task<ActionResult> EditarAnimalFotoPerfil(int id, AnimalImageUploadViewModel model)
        {
            Animal animal = null;
            if (model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
                    animal.Foto = model.File.FileName;
                    _context.SaveChanges();
                    var filePath = animal.DirectoriaAnimal + "\\" + model.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.File.CopyToAsync(fileStream);
                    return RedirectToAction("DetalhesAnimal", "Funcionario", new { id = animal.Id });
                }
            }
            model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// Ação que devolve a view de anexar fotos a uma galeria de fotos de um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <returns>View de anexar fotos a uma galeria de fotos do animal</returns>
        public ActionResult AdicionarAnexosFotosAnimal(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            AnimalImageUploadViewModel model = new AnimalImageUploadViewModel
            {
                animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault()
            };
            return View(model);
        }

        /// <summary>
        /// Ação que anexa uma foto á galeria de um animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <param name="model">Modelo da informação na view</param>
        /// <returns>View de detalhes do animal apos anexo de foto á galeria</returns>
        [HttpPost]
        public async Task<ActionResult> AdicionarAnexosFotosAnimal(int id, AnimalImageUploadViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    Galeria anexo = new Galeria
                    {
                        AnimalId = id,
                        FicheiroNome = model.File.FileName
                    };
                    model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
                    _context.FotosAnimais.Add(anexo);
                    _context.SaveChanges();
                    var filePath = model.animal.DirectoriaAnimal + "\\Galeria\\" + model.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.File.CopyToAsync(fileStream);
                    return RedirectToAction("DetalhesAnimal", "Funcionario", new { id = model.animal.Id });
                }
            }
            model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

        /// <summary>
        /// Ação que devolve a view de anexar ficheiros a uma ficha um determinado animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <returns>View de anexar ficheiros a uma ficha um determinado animal</returns>
        public ActionResult AdicionarAnexosFicheirosAnimal(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            AnimalFileUploadViewModel model = new AnimalFileUploadViewModel
            {
                animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault()                
            };
            return View(model);
        }

        /// <summary>
        /// Ação que anexa um ficheiro á ficha de um animal
        /// </summary>
        /// <param name="id">Id do animal</param>
        /// <param name="model">Modelo da informação na view</param>
        /// <returns>View de detalhes do animal apos anexo do ficheiro á ficha do animal</returns>
        [HttpPost]
        public async Task<ActionResult> AdicionarAnexosFicheirosAnimal(int id, AnimalFileUploadViewModel model)
        {
            if (model == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (model.File != null)
                {
                    Anexo anexo = new Anexo
                    {
                        AnimalId = id,
                        FicheiroNome = model.File.FileName
                    };
                    model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
                    _context.FicheirosAnimais.Add(anexo);
                    _context.SaveChanges();
                    var filePath = model.animal.DirectoriaAnimal + "\\Anexos\\" + model.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.File.CopyToAsync(fileStream);
                    return RedirectToAction("DetalhesAnimal", "Funcionario", new { id = model.animal.Id });
                }
            }
            model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }
        
        /****************************************************************************************************/
        /******************************************** Adocoes ***********************************************/
        /****************************************************************************************************/
        
        public ActionResult PedidosAdocao()
        {
            List<PedidoAdocao> pedidos = _context.PedidosAdocao.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        public ActionResult PedidosFimSemana()
        {
            List<PedidoFimSemana> pedidos = _context.PedidosFimSemana.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        public ActionResult PedidosPasseio()
        {
            List<PedidoPasseio> pedidos = _context.PedidosPasseio.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        public ActionResult PedidosAdocaoAnalisar(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        public ActionResult PedidosFimSemanaAnalisar(int id)
        {
            PedidoFimSemana pedido = _context.PedidosFimSemana.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        public ActionResult PedidosPasseioAnalisar(int id)
        {
            PedidoPasseio pedido = _context.PedidosPasseio.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        public ActionResult AceitarPedidoAdocao(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            AprovacaoViewModel aprovacao = new AprovacaoViewModel
            {
                PedidoId = pedido.Id       
            };
            return View("PedidoAdocaoAceite", aprovacao);            
        }

        [HttpPost]
        public async Task<ActionResult> AceitarPedidoAdocao(AprovacaoViewModel aprovacao)
        {
            if (ModelState.IsValid)
            {
                PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == aprovacao.PedidoId).FirstOrDefault();
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                pedido.Aprovacao = "Aprovado";
                pedido.DataAprovacao = DateTime.Now;
                Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();
                animal.Adoptado = true;
                Adocao novaAdocao = new Adocao
                {
                    PedidoAdocaoId = pedido.Id
                };
                _context.Adocoes.Add(novaAdocao);
                Adotante novoAdotante = new Adotante
                {
                    AnimalId = pedido.AnimalId,
                    AdotanteId = pedido.AdotanteId
                };
                _context.Adotantes.Add(novoAdotante);
                _context.SaveChanges();

                //Envio de Email
                EmailAdocaoViewModel emailAprovacaoModel = new EmailAdocaoViewModel(aprovacao.Notas, pedido.Adotante.Perfil.FirstName, animal.NomeAnimal);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoAdocaoAprovado.cshtml", emailAprovacaoModel);
                await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Adocao de " + animal.NomeAnimal + " Aprovado", body);
                return View("AdocaoAceite");
            }
            return View("PedidoAdocaoAceite", aprovacao);
        }

        public async Task<ActionResult> AceitarPedidoFimSemana(int id)
        {
            PedidoFimSemana pedido = _context.PedidosFimSemana.Where(p => p.Id == id).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            pedido.Aprovacao = "Aprovado";
            pedido.DataAprovacao = DateTime.Now;
            FimSemana fimSemana = new FimSemana
            {
                PedidoFimSemanaId = pedido.Id
            };
            _context.FinsSemanas.Add(fimSemana);
            _context.SaveChanges();

            Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();

            //Envio de Email
            EmailFimSemanaViewModel emailFimSemanaModel = new EmailFimSemanaViewModel("", pedido.Adotante.Perfil.FirstName, animal.NomeAnimal, pedido.DataInicio, pedido.DataFim);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoFimSemanaAprovado.cshtml", emailFimSemanaModel);
            await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Fim de Semana de " + animal.NomeAnimal + " Aprovado", body);
                       
            List<PedidoFimSemana> pedidos = _context.PedidosFimSemana.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            return View("PedidosFimSemana", pedidos);
        }        

        public async Task<ActionResult> AceitarPedidoPasseio(int id)
        {
            PedidoPasseio pedido = _context.PedidosPasseio.Where(p => p.Id == id).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            pedido.Aprovacao = "Aprovado";
            pedido.DataAprovacao = DateTime.Now;
            Passeio passeio = new Passeio
            {
                PedidoPasseioId = pedido.Id
            };
            _context.Passeios.Add(passeio);
            _context.SaveChanges();
            
            Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();

            //Envio de Email
            EmailPasseioViewModel emailPasseioModel = new EmailPasseioViewModel("", pedido.Adotante.Perfil.FirstName, animal.NomeAnimal, pedido.DataPasseio, pedido.HoraPasseio);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoPasseioAprovado.cshtml", emailPasseioModel);
            await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Passeio de " + animal.NomeAnimal + " Aprovado", body);

            List<PedidoPasseio> pedidos = _context.PedidosPasseio.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            return View("PedidosPasseio", pedidos);
        }

        public ActionResult RejeitarPedidoAdocao(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            AprovacaoViewModel aprovacao = new AprovacaoViewModel
            {
                PedidoId = pedido.Id
            };
            return View("PedidoAdocaoRejeitado", aprovacao);
        }

        public async Task<ActionResult> RejeitarPedidoPasseio(int id)
        {
            PedidoPasseio pedido = _context.PedidosPasseio.Where(p => p.Id == id).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            pedido.Aprovacao = "Reprovado";
            pedido.DataAprovacao = DateTime.Now;
            Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();
            _context.SaveChanges();

            //Envio de Email
            EmailPasseioViewModel emailPasseioModel = new EmailPasseioViewModel("", pedido.Adotante.Perfil.FirstName, animal.NomeAnimal, pedido.DataPasseio, pedido.HoraPasseio);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoPasseioReprovado.cshtml", emailPasseioModel);
            await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Passeio de " + animal.NomeAnimal + " Reprovado", body);

            List<PedidoPasseio> pedidos = _context.PedidosPasseio.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            return View("PedidosPasseio", pedidos);
        }

        public async Task<ActionResult> RejeitarPedidoFimSemana(int id)
        {
            PedidoFimSemana pedido = _context.PedidosFimSemana.Where(p => p.Id == id).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            pedido.Aprovacao = "Reprovado";
            pedido.DataAprovacao = DateTime.Now;
            Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();
            _context.SaveChanges();

            //Envio de Email
            EmailFimSemanaViewModel emailFimSemanaModel = new EmailFimSemanaViewModel("", pedido.Adotante.Perfil.FirstName, animal.NomeAnimal, pedido.DataInicio, pedido.DataFim);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoFimSemanaReprovado.cshtml", emailFimSemanaModel);
            await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Fim de Semana de " + animal.NomeAnimal + " Reprovado", body);

            List<PedidoFimSemana> pedidos = _context.PedidosFimSemana.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            return View("PedidosFimSemana", pedidos);
        }
        
        [HttpPost]
        public async Task<ActionResult> RejeitarPedidoAdocao(AprovacaoViewModel aprovacao)
        {
            if (ModelState.IsValid)
            {
                PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == aprovacao.PedidoId).FirstOrDefault();
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                pedido.Aprovacao = "Reprovado";
                pedido.DataAprovacao = DateTime.Now;
                Animal animal = _context.Animais.Where(a => a.Id == pedido.AnimalId).FirstOrDefault();
                _context.SaveChanges();

                //Envio de Email
                EmailAdocaoViewModel emailAprovacaoModel = new EmailAdocaoViewModel(aprovacao.Notas, pedido.Adotante.Perfil.FirstName, animal.NomeAnimal);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/AprovacoesPedidos/PedidoAdocaoReprovado.cshtml", emailAprovacaoModel);
                await _emailSender.SendEmailAsync(pedido.Adotante.Email, "Pedido de Adocao de " + animal.NomeAnimal + " Reprovado", body);
                return View("AdocaoRejeitada");
            }
            return View("PedidoAdocaoRejeitado", aprovacao);
        }

        // *************************** CONVOCATORIA


        public ActionResult ConvocarReuniao()
        {
            List<Adocao> adocoes = _context.Adocoes.Include(p => p.Pedido).Where(a => a.Pedido.TipoAdocao == "Total").ToList();
            foreach(var adocao in adocoes)
            {
                adocao.Pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == adocao.Pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                adocao.Pedido.Animal = _context.Animais.Where(a => a.Id == adocao.Pedido.AnimalId).Include(p => p.PorteAnimal).FirstOrDefault();
            }
            return View(adocoes);
        }

        public ActionResult ConvocarReuniaoEspecifica(Dictionary<string, string> dicionario)
        {
            ConvocatoriaViewModel convocatoria = new ConvocatoriaViewModel
            {
                UtilizadorId = dicionario.First().Key,
                AnimalNome = dicionario.First().Value
            };
            return View(convocatoria);
        }

        [HttpPost]
        public async Task<ActionResult> ConvocarReuniaoEspecifica(ConvocatoriaViewModel convocatoria)
        {
            if (ModelState.IsValid)
            {
                Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == convocatoria.UtilizadorId).Include(p => p.Perfil).FirstOrDefault();
                //Envio de Email
                EmailConvocatoriaViewModel emailConvocatoriaModel = new EmailConvocatoriaViewModel(convocatoria.Notas, utilizador.Perfil.FirstName, convocatoria.AnimalNome);
                string body = await _razorView.RenderViewToStringAsync("/Views/Emails/Convocatoria/Convocatoria.cshtml", emailConvocatoriaModel);
                await _emailSender.SendEmailAsync(utilizador.Email, "Convocatoria para o(a) " + convocatoria.AnimalNome, body);
                return View("ConvocatoriaRegistada");
            }
            return View(convocatoria);
        }

        public PartialViewResult PesquisarAnimal(string pesquisa)
        {
            List<Adocao> adocoes = _context.Adocoes.Include(p => p.Pedido).Where(a => a.Pedido.TipoAdocao == "Total").ToList();
            List<Adocao> adocoesMostrar = new List<Adocao>();
            foreach (var adocao in adocoes)
            {
                adocao.Pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == adocao.Pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                adocao.Pedido.Animal = _context.Animais.Where(a => a.Id == adocao.Pedido.AnimalId).Include(p => p.PorteAnimal).FirstOrDefault();
                if (adocao.Pedido.Animal.NomeAnimal.ToLower().Contains(pesquisa))
                {
                    adocoesMostrar.Add(adocao);
                }
            }
            return PartialView("~/Views/Funcionario/GridAnimaisAdotados.cshtml", adocoesMostrar);
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
        public JsonResult RacasPorCategoria(int CategoriaID)
        {
            var racas = (from racasAux in _context.Racas
                         where racasAux.CategoriaId == CategoriaID
                         orderby racasAux.NomeRaca
                         select new Raca { Id = racasAux.Id, NomeRaca = racasAux.NomeRaca }).ToList();
            return Json(new SelectList(racas, "Id", "NomeRaca"));
        }

        /************************************************************************************+
         *****************************Estatisticas***********************************************
         *****************************************************************************************/
         [HttpGet]
        public JsonResult PieAnimal() {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.dataGroupTipoAnimais());
        }

        [HttpGet]
        public JsonResult PieMf()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.dataGroupMF());
        }
        [HttpGet]
        public JsonResult adocoesPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.adocoesPorMes());
        }
        [HttpGet]
        public JsonResult apadrinhamentosPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.apadrinhamentosPorMes());
        }
        [HttpGet]
        public JsonResult numeroPedidosPasseioPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.numeroPedidosPasseioPorMes());
        }

        [HttpGet]
        public JsonResult numeroPedidosFdsPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.numeroPedidosFdsPorMes());
        }

        public IActionResult EstatisticasResponsavel()
        {
            DataRetrive data = new DataRetrive(_context);
            return View(data);
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

        [HttpGet]
        public JsonResult EventosCalendarioAnimal(int id)
        {
            var listCalendario = new List<object>();

            var resSelectFins = new List<object>();
            var resSelectPass = new List<object>();

            /*var selectPasseios = (from passeio in _context.PedidosPasseio
                                    where passeio.Aprovacao == "Aprovado"
                                    select passeio).Include(u => u.Adotante).ThenInclude(u => u.Perfil);

            var selectFinsDeSemana = (from fimDeSemana in _context.PedidosFimSemana
                                        where fimDeSemana.Aprovacao == "Aprovado"
                                        select fimDeSemana).Include(u => u.Adotante).ThenInclude(u => u.Perfil);*/

            var adotantes = _context.Adotantes.Where(u => u.AnimalId == id).ToList();

            //Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault();
            //var selectFinsDeSemana = new List<object>();
            //var selectPasseios = new List<object>();
            foreach (var item in adotantes)
            {
                var selectFinsDeSemana = _context.FinsSemanas.Include(u => u.Pedido).Where(a => a.Pedido.DataInicio >= DateTime.Today && a.Pedido.AdotanteId == item.AdotanteId).ToList();
                var selectPasseios = _context.Passeios.Include(u => u.Pedido).Where(a => a.Pedido.DataPasseio >= DateTime.Today && a.Pedido.AdotanteId == item.AdotanteId).ToList();
                resSelectFins.Union(selectFinsDeSemana);
                resSelectPass.Union(selectPasseios);
            }
            //var selectFinsDeSemana = _context.FinsSemanas.Include(u => u.Pedido).Where(a => a.Pedido.DataInicio >= DateTime.Today && a.Pedido.AdotanteId == utilizador.PerfilId).ToList();

            //var selectPasseios = _context.Passeios.Include(u => u.Pedido).Where(a => a.Pedido.DataPasseio >= DateTime.Today && a.Pedido.AdotanteId == utilizador.PerfilId).ToList();

            

            foreach (FimSemana item in resSelectFins)
            {

                listCalendario.Add(
                        new
                        {
                            title = "Fim de Semana com " + item.Pedido.Adotante.Perfil.FirstName + " " + item.Pedido.Adotante.Perfil.LastName,
                            start = item.Pedido.DataInicio,
                            end = item.Pedido.DataFim
                        }
                    );

            }

            foreach (Passeio item in resSelectPass)
            {
                listCalendario.Add(
                    new
                    {
                        title = item.Pedido.HoraPasseio,
                        start = item.Pedido.DataPasseio,
                        end = ""
                    }
                );
            }

            return Json(listCalendario);
        }
    }
}