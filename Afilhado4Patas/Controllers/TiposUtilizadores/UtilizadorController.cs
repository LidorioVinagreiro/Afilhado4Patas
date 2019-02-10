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
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    /// <summary>
    /// Utilizador Controller
    /// </summary>
    [Authorize(Roles = "Utilizador")]
    public class UtilizadorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserManager<Utilizadores> _userManager;

        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base dados</param>
        public UtilizadorController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment, UserManager<Utilizadores> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
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

        public ActionResult VisualizarAmigo(string id)
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

        public ActionResult ApagarAmigo(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var amigo = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            var amizade = _context.Amizades.Where(a => (a.IdPerfilAceitar == amigo.PerfilId && a.IdPerfilPediu == user.PerfilId) || (a.IdPerfilPediu == amigo.PerfilId && a.IdPerfilAceitar == user.PerfilId)).FirstOrDefault();
            _context.Amizades.Remove(amizade);
            _context.SaveChanges();

            if (user == null)
            {
                return NotFound();
            }
            return View("Dashboard", user);
        }

        /****************************************************************************************************/
        /******************************************** Perfil ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que devolve a view do perfil com a informação pessoal do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <returns>View do perfil com a informação do utilizador</returns>
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
        /// Ação quedevole a view que permite a edição das informações no perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
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
        /// Ação que devolve a view que edita a informação atual referente a morada no perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
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
        /// Ação que edita a informação no perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do utilizador com a informação atualizada</returns>
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
        /// Ação que altera a informação referente a morada do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do utilizador com a informação atualizada</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PerfilEditarMorada(string id, PerfilEditarMoradaViewModel editarPerfilViewModel)
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
        /// Ação que devolve a view que edita a informação atual referente a foto de perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
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
        /// Ação que altera a informação referente a foto de perfil do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do utilizador com a informação atualizada</returns>
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
        /// Ação que devolve a view que edita a informação atual referente a palavra passe do utilizador
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
        /// Ação que altera a informação referente a palvra passe do utilizador
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do utilizador com a informação atualizada</returns>
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
        /******************************************** Adocoes ***********************************************/
        /****************************************************************************************************/

        public ActionResult MeusAnimais(string id)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == id).FirstOrDefault();
            List<Adotante> adotantes = _context.Adotantes.Where(a => a.AdotanteId == utilizador.PerfilId).ToList();
            List<Animal> lista_animais = new List<Animal>();
            foreach (var adotante in adotantes)
            {
                lista_animais.Add(_context.Animais.Where(a => a.Id == adotante.AnimalId).Include(p => p.PorteAnimal).FirstOrDefault());
            }
            return View(lista_animais);
        }
        
        public ActionResult PedidoAdocao(string id)
        {
            if (id != null)
            {
                Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
                PedidoAdocaoViewModel pedido = new PedidoAdocaoViewModel
                {
                    NomeAdotante = utilizador.Perfil.FirstName,
                    Email = utilizador.Email
                };
                return View(pedido);
            }
            return NotFound();
        }

        public ActionResult PedidoAdocaoEspecifico(int id)
        {
            if (id > 0)
            {
                Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault();
                Animal animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
                PedidoAdocaoViewModel pedido = new PedidoAdocaoViewModel
                {
                    NomeAdotante = utilizador.Perfil.FirstName,
                    Email = utilizador.Email,
                    AnimalId = animal.Id                    
                };
                return View(pedido);
            }
            return NotFound();
        }

        public ActionResult PedidoFimSemana(int id)
        {
            if (id > 0)
            {
                PedidoFimSemanaViewModel pedido = new PedidoFimSemanaViewModel
                {
                    AnimalId = id         
                };
                return View(pedido);
            }
            return NotFound();
        }

        public ActionResult PedidoPasseio(int id)
        {
            if (id > 0)
            {
                PedidoPasseioViewModel pedido = new PedidoPasseioViewModel
                {
                    AnimalId = id
                };
                return View(pedido);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedidoAdocao(PedidoAdocaoViewModel PedidoAdocao)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault();

            if (utilizador == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PedidoAdocao novoPedido = new PedidoAdocao
                    {
                        TipoAdocao = PedidoAdocao.TipoAdocao,
                        AdotanteId = utilizador.Perfil.Id,
                        AnimalId = PedidoAdocao.AnimalId,
                        DataPedido = DateTime.Now,
                        Morada = PedidoAdocao.Morada,
                        Motivo = PedidoAdocao.Motivacao,
                        OutrosAnimais = PedidoAdocao.OutrosAnimais,
                        Aprovacao = "Em espera"
                    };
                    _context.PedidosAdocao.Add(novoPedido);
                    _context.SaveChanges();
                    novoPedido.DiretoriaPedido = _hostingEnvironment.WebRootPath + "\\PedidosAdocao\\" + novoPedido.Id;
                    novoPedido.NomeFicheiroID = PedidoAdocao.File.FileName;
                    CreateFolder(novoPedido.DiretoriaPedido);
                    _context.SaveChanges();
                    var filePath = novoPedido.DiretoriaPedido + "\\" + PedidoAdocao.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await PedidoAdocao.File.CopyToAsync(fileStream);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return View("PedidoRegistado", "O seu pedido de adoção foi realizado com sucesso!");
            }
            PedidoAdocao.NomeAdotante = (_context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault()).Perfil.FirstName;
            return View(PedidoAdocao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedidoAdocaoEspecifico(PedidoAdocaoViewModel PedidoAdocao)
        {
            Utilizadores utilizador =_context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault();

            if (utilizador == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PedidoAdocao novoPedido = new PedidoAdocao
                    {
                        TipoAdocao = PedidoAdocao.TipoAdocao,
                        AdotanteId = utilizador.Perfil.Id,
                        AnimalId = PedidoAdocao.AnimalId,
                        DataPedido = DateTime.Now,
                        Morada = PedidoAdocao.Morada,
                        Motivo = PedidoAdocao.Motivacao,
                        OutrosAnimais = PedidoAdocao.OutrosAnimais,
                        Aprovacao = "Em espera"
                    };
                    _context.PedidosAdocao.Add(novoPedido);
                    _context.SaveChanges();
                    novoPedido.DiretoriaPedido = _hostingEnvironment.WebRootPath + "\\PedidosAdocao\\" + novoPedido.Id;
                    novoPedido.NomeFicheiroID = PedidoAdocao.File.FileName;
                    CreateFolder(novoPedido.DiretoriaPedido);
                    _context.SaveChanges();
                    var filePath = novoPedido.DiretoriaPedido + "\\" + PedidoAdocao.File.FileName;
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    await PedidoAdocao.File.CopyToAsync(fileStream);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return View("PedidoRegistado", "O seu pedido de adoção foi realizado com sucesso!");
            }
            PedidoAdocao.NomeAdotante = (_context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault()).Perfil.FirstName;
            return View(PedidoAdocao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedidoFimSemana(PedidoFimSemanaViewModel PedidoFimSemana)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault();

            if (utilizador == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PedidoFimSemana novoPedido = new PedidoFimSemana
                    {                        
                        AdotanteId = utilizador.Perfil.Id,
                        AnimalId = PedidoFimSemana.AnimalId,
                        DataPedido = DateTime.Now,
                        DataInicio = PedidoFimSemana.DataInicio,
                        DataFim = PedidoFimSemana.DataFim,
                        Aprovacao = "Em espera"
                    };
                    _context.PedidosFimSemana.Add(novoPedido);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return View("PedidoRegistado", "O seu pedido de Fim de Semana foi realizado com sucesso!");
            }
            return View(PedidoFimSemana);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PedidoPasseio(PedidoPasseioViewModel PedidoPasseio)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).Include(p => p.Perfil).FirstOrDefault();

            if (utilizador == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    PedidoPasseio novoPedido = new PedidoPasseio
                    {
                        AdotanteId = utilizador.Perfil.Id,
                        AnimalId = PedidoPasseio.AnimalId,
                        DataPedido = DateTime.Now,
                        DataPasseio = PedidoPasseio.DataPasseio,
                        HoraPasseio = PedidoPasseio.HoraPasseio,
                        Aprovacao = "Em espera"
                    };
                    _context.PedidosPasseio.Add(novoPedido);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return View("PedidoRegistado", "O seu pedido de Passeio foi realizado com sucesso!");
            }
            return View(PedidoPasseio);
        }

        public ActionResult MeusPedidos(string id)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            List<PedidoAdocao> pedidosAdocao = _context.PedidosAdocao.Where(p => p.AdotanteId == utilizador.PerfilId).Include(a => a.Animal).ToList();
            return View(pedidosAdocao);
        }

        public ActionResult MeusPedidosFimSemana(string id)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            List<PedidoFimSemana> pedidosFimSemana = _context.PedidosFimSemana.Where(p => p.AdotanteId == utilizador.PerfilId).Include(a => a.Animal).ToList();
            return View(pedidosFimSemana);
        }

        public ActionResult MeusPedidosPasseio(string id)
        {
            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            List<PedidoPasseio> pedidosPasseio = _context.PedidosPasseio.Where(p => p.AdotanteId == utilizador.PerfilId).Include(a => a.Animal).ToList();
            return View(pedidosPasseio);
        }


        /****************************************************************************************************/
        /***************************************** CONSULTAS SQL ********************************************/
        /****************************************************************************************************/


        [HttpGet]
        public JsonResult AnimaisTipoAdocao(string TipoAdocao)
        {
            var animais = (from animal in _context.Animais
                           where AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                           orderby animal.NomeAnimal
                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
            return Json(new SelectList(animais, "Id", "NomeAnimal"));
        }

        [HttpGet]
        public JsonResult Categorias(string TipoAdocao, string Sexo)
        {
            JsonResult retorno = null;
            if (!Sexo.Equals("Ambos"))
            {
                var categorias = (from categoriaAux in _context.Categorias
                                  join a in _context.Racas on categoriaAux.Id equals a.CategoriaId
                                  join b in _context.Animais on a.Id equals b.RacaId
                                  where AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false) && b.Sexo.Equals(Sexo)
                                  orderby categoriaAux.Nome
                                  select new Categoria { Id = categoriaAux.Id, Nome = categoriaAux.Nome }).Distinct().ToList();
                retorno = Json(new SelectList(categorias, "Id", "Nome"));
            }
            else
            {
                var categorias = (from categoriaAux in _context.Categorias
                                  join a in _context.Racas on categoriaAux.Id equals a.CategoriaId
                                  join b in _context.Animais on a.Id equals b.RacaId
                                  where AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                  orderby categoriaAux.Nome
                                  select new Categoria { Id = categoriaAux.Id, Nome = categoriaAux.Nome }).Distinct().ToList();
                retorno = Json(new SelectList(categorias, "Id", "Nome"));
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult AnimaisTipoAdocaoSexo(string TipoAdocao, string Sexo)
        {
            JsonResult retorno = null;
            if (!Sexo.Equals("Ambos"))
            {
                var animais = (from animal in _context.Animais
                               orderby animal.NomeAnimal
                               where AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false) && animal.Sexo.Equals(Sexo)
                               select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
            }
            else
            {
                var animais = (from animal in _context.Animais
                               orderby animal.NomeAnimal
                               where AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                               select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
            }
            return retorno;
        }

        public List<int> AnimaisNaoMostrar(string TipoAdocao)
        {
            List<int> animaisAdotados = new List<int>();
            if (TipoAdocao.Equals("Total"))
            {
                var adocoes = _context.Adocoes.ToList();
                foreach(var adocao in adocoes)
                {
                    animaisAdotados.Add(_context.PedidosAdocao.Where(p => p.Id == adocao.PedidoAdocaoId).FirstOrDefault().AnimalId);
                }
            }
            else
            {
                var adocoes = _context.Adocoes.ToList();
                foreach (var adocao in adocoes)
                {
                    if(_context.PedidosAdocao.Where(p => p.Id == adocao.PedidoAdocaoId).FirstOrDefault().TipoAdocao.Equals("Total"))
                    {
                        animaisAdotados.Add(_context.PedidosAdocao.Where(p => p.Id == adocao.PedidoAdocaoId).FirstOrDefault().AnimalId);
                    }
                }
            }
            return animaisAdotados;
        }

        [HttpGet]
        public JsonResult AnimaisSexo(string TipoAdocao, string Sexo)
        {
            var animais = (from animal in _context.Animais
                           where animal.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                           orderby animal.NomeAnimal
                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
            return Json(new SelectList(animais, "Id", "NomeAnimal"));
        }

        [HttpGet]
        public JsonResult PorteCategoriaSexo(string TipoAdocao, int Categoria, string Sexo)
        {
            JsonResult retorno = null;
            if (Categoria > 0)
            {
                if (!Sexo.Equals("Ambos"))
                {
                    var categorias = (from porteAux in _context.Portes
                                      join a in _context.Animais on porteAux.Id equals a.PorteId
                                      join b in _context.Racas on a.RacaId equals b.Id
                                      join c in _context.Categorias on b.CategoriaId equals c.Id
                                      where a.Sexo.Equals(Sexo) && c.Id.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(a.Id).Equals(false)
                                      orderby porteAux.TipoPorte
                                      select new Porte { Id = porteAux.Id, TipoPorte = porteAux.TipoPorte }).Distinct().ToList();
                    retorno = Json(new SelectList(categorias, "Id", "TipoPorte"));
                }
                else
                {
                    var categorias = (from porteAux in _context.Portes
                                      join a in _context.Animais on porteAux.Id equals a.PorteId
                                      join b in _context.Racas on a.RacaId equals b.Id
                                      join c in _context.Categorias on b.CategoriaId equals c.Id
                                      where c.Id.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(a.Id).Equals(false)
                                      orderby porteAux.TipoPorte
                                      select new Porte { Id = porteAux.Id, TipoPorte = porteAux.TipoPorte }).Distinct().ToList();
                    retorno = Json(new SelectList(categorias, "Id", "TipoPorte"));
                }
            }
            else
            {
                if (!Sexo.Equals("Ambos"))
                {
                    var categorias = (from porteAux in _context.Portes
                                      join a in _context.Animais on porteAux.Id equals a.PorteId
                                      join b in _context.Racas on a.RacaId equals b.Id
                                      join c in _context.Categorias on b.CategoriaId equals c.Id
                                      where a.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(a.Id).Equals(false)
                                      orderby porteAux.TipoPorte
                                      select new Porte { Id = porteAux.Id, TipoPorte = porteAux.TipoPorte }).Distinct().ToList();
                    retorno = Json(new SelectList(categorias, "Id", "TipoPorte"));
                }
                else
                {
                    var categorias = (from porteAux in _context.Portes
                                      join a in _context.Animais on porteAux.Id equals a.PorteId
                                      join b in _context.Racas on a.RacaId equals b.Id
                                      join c in _context.Categorias on b.CategoriaId equals c.Id
                                      where AnimaisNaoMostrar(TipoAdocao).Contains(a.Id).Equals(false)
                                      orderby porteAux.TipoPorte
                                      select new Porte { Id = porteAux.Id, TipoPorte = porteAux.TipoPorte }).Distinct().ToList();
                    retorno = Json(new SelectList(categorias, "Id", "TipoPorte"));
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult RacaCategoriaSexoPorte(string TipoAdocao, int Categoria, string Sexo, int Porte)
        {
            JsonResult retorno = null;
            if (Categoria > 0)
            {
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.Sexo.Equals(Sexo) && b.PorteId.Equals(Porte) && racaAux.CategoriaId.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                    else
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.Sexo.Equals(Sexo) && racaAux.CategoriaId.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }

                }
                else
                {
                    if (Porte > 0)
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.PorteId.Equals(Porte) && racaAux.CategoriaId.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                    else
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where racaAux.CategoriaId.Equals(Categoria) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                }
            }
            else
            {
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.Sexo.Equals(Sexo) && b.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                    else
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                }
                else
                {
                    if (Porte > 0)
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                    else
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where AnimaisNaoMostrar(TipoAdocao).Contains(b.Id).Equals(false)
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult AnimaisCategoriaSexo(string TipoAdocao, int Categoria, string Sexo)
        {
            JsonResult retorno = null;
            //Categoria selecionada
            if (Categoria > 0)
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    var animais = (from animal in _context.Animais
                                   join a in _context.Racas on animal.RacaId equals a.Id
                                   join b in _context.Categorias on a.CategoriaId equals b.Id
                                   where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                   orderby animal.NomeAnimal
                                   select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                    retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                }
                //Sexo nao selecionado
                else
                {
                    var animais = (from animal in _context.Animais
                                   join a in _context.Racas on animal.RacaId equals a.Id
                                   join b in _context.Categorias on a.CategoriaId equals b.Id
                                   where Categoria.Equals(b.Id) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                   orderby animal.NomeAnimal
                                   select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                    retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                }
            }
            else
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    retorno = AnimaisSexo(TipoAdocao, Sexo);
                }
                //Sexo nao selecionado
                else
                {
                    retorno = AnimaisTipoAdocao(TipoAdocao);
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult AnimaisCategoriaSexoPorte(string TipoAdocao, int Categoria, string Sexo, int Porte)
        {
            JsonResult retorno = null;
            //Categoria selecionada
            if (Categoria > 0)
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }

                }
                //Sexo nao selecionado
                else
                {
                    if (Porte > 0)
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }

                }
            }
            else
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        retorno = AnimaisSexo(TipoAdocao, Sexo);
                    }

                }
                //Sexo nao selecionado
                else
                {
                    if (Porte > 0)
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        retorno = AnimaisTipoAdocao(TipoAdocao);
                    }
                }
            }
            return retorno;
        }
        [HttpGet]
        public JsonResult AnimaisCategoriaSexoPorteRaca(string TipoAdocao, int Categoria, string Sexo, int Porte, int Raca)
        {
            JsonResult retorno = null;
            //Categoria selecionada
            if (Categoria > 0)
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }

                    }
                    else
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                    }

                }
                //Sexo nao selecionado
                else
                {
                    if (Porte > 0)
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                    }
                    else
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                    }
                }
            }
            else
            {
                //Sexo Selecionado
                if (!Sexo.Equals("Ambos"))
                {
                    if (Porte > 0)
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                    }
                    else
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.Sexo.Equals(Sexo) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            retorno = AnimaisSexo(TipoAdocao, Sexo);
                        }
                    }
                }
                //Sexo nao selecionado
                else
                {
                    if (Porte > 0)
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.PorteId.Equals(Porte) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                    }
                    else
                    {
                        if (Raca > 0)
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.RacaId.Equals(Raca) && AnimaisNaoMostrar(TipoAdocao).Contains(animal.Id).Equals(false)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            retorno = AnimaisTipoAdocao(TipoAdocao);
                        }
                    }
                }
            }
            return retorno;
        }

        /*****************************************************************************************************
         * ****************************************** Amizades ***********************************************
         ******************************************************************************************************/
        /// <summary>
        /// Esta ação devolve todos os utilizadores que partilhem o mesmo animal (padrinhos) do actual utilizador
        /// Os utilizadores devolvidos são utilizadores que :
        /// -não são amigos
        /// -são padrinhos do mesmo animal
        /// -não existe um pedido de amizade por uma as partes (pedido feito pelo utilizador ou feito por um dos padrinhos do animal)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> pedirAmizade()
        {
            Utilizadores idUser = await _userManager.GetUserAsync(this.User);

            Perfil utilizadorPerfil = _context.PerfilTable
                .Where(a => a.UtilizadorId == idUser.Id)
                .FirstOrDefault();

            var listaAnimaisDoUser = _context.Adotantes
           .Where(utilizador => utilizador.AdotanteId == utilizadorPerfil.Id)
           .Include(pessoa => pessoa.Adotante_User) 
           .Include(animal => animal.Animal);

            //o adotante_user está a null nao sei pq... |||||ATENCAO|||||
            foreach (Adotante item in listaAnimaisDoUser) {
                item.Adotante_User = _context.PerfilTable.Where(p => p.Id == item.AdotanteId).FirstOrDefault();
            }
            
            var listaPessoas = (from adotantes in _context.Adotantes
                                join adotante in listaAnimaisDoUser
                                on adotantes.AnimalId equals adotante.AnimalId
                                where adotantes.AdotanteId != utilizadorPerfil.Id
                                select new Amizades
                                {
                                    IdPerfilPediu = utilizadorPerfil.Id,
                                    IdPerfilAceitar = adotantes.AdotanteId,
                                    IdAnimalEmComum = adotantes.AnimalId,
                                    AnimalComumAosDois = adotante.Animal,
                                    ExistePedido = false,
                                    Amigos = false 
                                }).Distinct().Include(possivelAmigo => possivelAmigo.PossivelAmigo).AsEnumerable<Amizades>();

            var listaPedidosEAmigos = (from amizades in _context.Amizades
                                      where (amizades.IdPerfilPediu == utilizadorPerfil.Id || amizades.IdPerfilAceitar == utilizadorPerfil.Id) &&
                                      (
                                      (amizades.Amigos == false && amizades.ExistePedido == true) ||
                                      (amizades.Amigos == true)
                                      )
                                      select amizades).AsEnumerable<Amizades>();

            List<Amizades> model;
            if (listaPedidosEAmigos.Count() > 0)
            {
                IEnumerable<Amizades> listaPadrinhosSemPedido = listaPessoas.Except(listaPedidosEAmigos,new Amizades.AmizadesComparer());
                model = listaPadrinhosSemPedido.ToList();
                foreach (Amizades item in model)
                {
                    item.PossivelAmigo = _context.PerfilTable.Where(x => x.Id == item.IdPerfilAceitar).FirstOrDefault();
                }
            }
            else {
                model = listaPessoas.ToList();
                foreach (Amizades item in model) {
                    item.PossivelAmigo = _context.PerfilTable.Where(x => x.Id == item.IdPerfilAceitar).FirstOrDefault();
                }
            }

            List<AmizadeViewModel> modeloView = new List<AmizadeViewModel>();
            foreach (Amizades item in model)
            {
                modeloView.Add(new AmizadeViewModel
                {
                    idAnimalComum = item.IdAnimalEmComum,
                    idPerfilPossivelAmizade = item.IdPerfilAceitar,
                    Nome = item.PossivelAmigo.FirstName,
                    NomeAnimal = item.AnimalComumAosDois.NomeAnimal,
                    AnimalEmComum = _context.Animais.Where(a => a.Id == item.IdAnimalEmComum).FirstOrDefault(),
                    PerfilPossivelAmizade = _context.PerfilTable.Where(p => p.Id == item.IdPerfilAceitar).Include(u => u.Utilizador).FirstOrDefault()
                });
            }

            return View("pedirAmizade", modeloView);
        }
        /// <summary>
        /// Esta ação permite ao utilizador fazer um pedido de amizade a um determinado utilizador
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> pedirAmizade(AmizadeViewModel model) {
            if (ModelState.IsValid)
            {
                Utilizadores user = await _userManager.GetUserAsync(this.User);
                Perfil idPerfilUser = _context.PerfilTable.Where(u => u.UtilizadorId == user.Id).FirstOrDefault();
                Amizades amizade = new Amizades
                {
                    IdPerfilAceitar = model.idPerfilPossivelAmizade,
                    IdPerfilPediu = idPerfilUser.Id,
                    IdAnimalEmComum = model.idAnimalComum,
                    ExistePedido = true,
                    Amigos = false
                };
                _context.Amizades.Add(amizade);
                await _context.SaveChangesAsync();
                return RedirectToAction("pedirAmizade");
            }
            return NotFound();
        }

        /// <summary>
        /// Esta ação retorna todos os pedidos de amizade feitos por outros utilizadores
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> pedidosAmizade()
        {
            Utilizadores idUser = await _userManager.GetUserAsync(this.User);

            Perfil utilizadorPerfil = _context.PerfilTable
                .Where(a => a.UtilizadorId == idUser.Id)
                .FirstOrDefault();



            var listaPedidos = _context.Amizades
                .Where(x => x.IdPerfilAceitar == utilizadorPerfil.Id && x.Amigos == false && x.ExistePedido == true);
                
            foreach(var item in listaPedidos) {
                item.AnimalComumAosDois = _context.Animais.Where(a => a.Id == item.IdAnimalEmComum).FirstOrDefault();
                }
 
            List<AmizadePedidoViewModel> model = new List<AmizadePedidoViewModel>();
            foreach (Amizades item in listaPedidos) {
                model.Add(new AmizadePedidoViewModel
                {
                    id = item.Id,
                    idPerfilPediuAmizade = item.IdPerfilPediu,
                    idAnimalComum = item.AnimalComumAosDois.Id,
                    Aceitar = item.Amigos,
                    Nome=_context.PerfilTable.Where( a => a.Id == item.IdPerfilPediu).FirstOrDefault().FirstName,
                    NomeAnimal = item.AnimalComumAosDois.NomeAnimal
                });
            }

            foreach (var item in model) {
                item.PerfilPediuAmizade = _context.PerfilTable.Where(p => p.Id == item.idPerfilPediuAmizade).Include(u => u.Utilizador).FirstOrDefault();
                item.AnimalComum = _context.Animais.Where(a => a.Id == item.idAnimalComum).FirstOrDefault();
            }

            return View("pedidosAmizade",model);
        }
        /// <summary>
        /// Esta ação aceita o pedido de amizade de um utilizador
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> pedidosAmizadeAceitar(AmizadePedidoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Utilizadores user = await _userManager.GetUserAsync(this.User);
                user.Perfil = _context.PerfilTable.Where(p => p.UtilizadorId == user.Id).FirstOrDefault();
                _context.Amizades.Where(a => a.Id == model.id).FirstOrDefault().Amigos = true;
                _context.SaveChanges();
                return RedirectToAction("pedidosAmizade");
            }
            return NotFound();
        }
        /// <summary>
        /// Esta ação recusa o pedido de amizade de outro utilizador
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> pedidosAmizadeRecusar(AmizadePedidoViewModel model)
        {
            if (ModelState.IsValid)
            {
                Utilizadores user = await _userManager.GetUserAsync(this.User);
                user.Perfil = _context.PerfilTable.Where(p => p.UtilizadorId == user.Id).FirstOrDefault();
                Amizades amigos = _context.Amizades.Where(a => a.Id == model.id).FirstOrDefault();
                _context.Amizades.Remove(amigos);
                _context.SaveChanges();
                return RedirectToAction("pedidosAmizade");
            }
            return NotFound();
        }

        /// <summary>
        /// Esta ação devolve todos os utilizadores que são amigos
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> listaAmizades()
        {
            Utilizadores idUser = await _userManager.GetUserAsync(this.User);

            Perfil utilizadorPerfil = _context.PerfilTable
                .Where(a => a.UtilizadorId == idUser.Id)
                .FirstOrDefault();

            var listaAmigos = (from amigos in _context.Amizades
                               where (amigos.IdPerfilAceitar == utilizadorPerfil.Id ||
                               amigos.IdPerfilPediu == utilizadorPerfil.Id)
                               && amigos.Amigos == true
                               select new AmizadeListaViewModel
                               {
                                   Id = amigos.Id,
                                   AmigoId = (amigos.IdPerfilAceitar != utilizadorPerfil.Id) ? amigos.IdPerfilAceitar : amigos.IdPerfilPediu,
                                   Amigos = amigos.Amigos
                               });

            List<AmizadeListaViewModel> model = new List<AmizadeListaViewModel>();
            foreach (var item in listaAmigos) {
                item.Amigo = _context.PerfilTable.Where(p => p.Id == item.AmigoId).FirstOrDefault();
                model.Add(item);
            }
            return View("listaAmizades",model);
        }

        /// <summary>
        /// Esta ação apaga um utilizador da lista de amizades
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> listaAmizades(AmizadeListaViewModel model)
        {
            _context.Amizades.Remove(_context.Amizades.Where(a => a.Id == model.Id).FirstOrDefault());
            await _context.SaveChangesAsync();
            return RedirectToAction("listaAmizades");
        }
        

        /// <summary>
        /// Esta ação devolve todos os pedidos feitos pelo utilizador
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> listaPedidos()
        {
            Utilizadores idUser = await _userManager.GetUserAsync(this.User);

            Perfil utilizadorPerfil = _context.PerfilTable
                .Where(a => a.UtilizadorId == idUser.Id)
                .FirstOrDefault();



            var listaPedidos = _context.Amizades
                .Where(x => x.IdPerfilPediu == utilizadorPerfil.Id && x.Amigos == false && x.ExistePedido == true);

            foreach (var item in listaPedidos)
            {
                item.AnimalComumAosDois = _context.Animais.Where(a => a.Id == item.IdAnimalEmComum).FirstOrDefault();
            }

            List<AmizadePedidoViewModel> model = new List<AmizadePedidoViewModel>();
            foreach (Amizades item in listaPedidos)
            {
                model.Add(new AmizadePedidoViewModel
                {
                    id = item.Id,
                    idPerfilPediuAmizade = item.IdPerfilAceitar,
                    idAnimalComum = item.AnimalComumAosDois.Id,
                    Aceitar = item.Amigos,
                    Nome = _context.PerfilTable.Where(a => a.Id == item.IdPerfilAceitar).FirstOrDefault().FirstName,
                    NomeAnimal = item.AnimalComumAosDois.NomeAnimal
                });
            }

            foreach (var item in model) {
                item.PerfilPediuAmizade = _context.PerfilTable.Where(p => p.Id == item.idPerfilPediuAmizade).Include(u => u.Utilizador ).FirstOrDefault();
                item.AnimalComum = _context.Animais.Where(a => a.Id == item.idAnimalComum).FirstOrDefault();    
            }

            return View("listaPedidos", model);

        }

        /// <summary>
        /// Este ação serve para apagar um pedido de amizade feito pelo utilizador
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> listaPedidos(AmizadePedidoViewModel model)
        {
            Utilizadores user = await _userManager.GetUserAsync(this.User);
            user.Perfil = _context.PerfilTable.Where(p => p.UtilizadorId == user.Id).FirstOrDefault();
            Perfil amigoPerdido = _context.PerfilTable.Where(p => p.Id == model.idPerfilPediuAmizade).FirstOrDefault();

            _context.Amizades.Remove(_context.Amizades.Where(a=> a.Id == model.id).FirstOrDefault());
            _context.SaveChanges();
            return RedirectToAction("listaPedidos");

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

        /// <summary>
        /// Ação que devolve a view de erro, caso ocorra um erro esta view e devolvida com a informação do erro
        /// </summary>
        /// <returns>View de erro</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PadrinhosAnimal(int id)
        {
            var padrinhos = _context.Animais.Where(a => a.Id == id).Include(a => a.Adotantes).ToList();

            return View(padrinhos);
        }

        public JsonResult EventosCalendarioUtilizador()
        {
            var listCalendario = new List<object>();

            var resSelect = new List<object>();

            Utilizadores utilizador = _context.Utilizadores.Where(u => u.Id == _userManager.GetUserId(User)).FirstOrDefault();
            var selectFinsDeSemana = _context.FinsSemanas.Include(u => u.Pedido).Where(a => a.Pedido.DataInicio >= DateTime.Today && a.Pedido.AdotanteId == utilizador.PerfilId).ToList();

            var selectPasseios = _context.Passeios.Include(u => u.Pedido).Where(a => a.Pedido.DataPasseio >= DateTime.Today && a.Pedido.AdotanteId == utilizador.PerfilId).ToList();

            resSelect.Union(selectPasseios).Union(selectFinsDeSemana);

            foreach (var item in selectFinsDeSemana)
            {
                listCalendario.Add(
                        new
                        {
                            title = "Fim de Semana com " + utilizador.Perfil.FirstName + " " + utilizador.Perfil.LastName,
                            start = item.Pedido.DataInicio,
                            end = item.Pedido.DataFim
                        }
                    );
            }

            foreach (var item in selectPasseios)
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