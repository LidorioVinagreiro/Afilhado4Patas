﻿using System.Diagnostics;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                foreach (var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).ToList())
                {
                    animal.Adotantes.Add(_context.Utilizadores.Where(u => u.Id == adotantes.AdotanteId).FirstOrDefault());
                }
            }
            return View("../Shared/FichaAnimal", animal);
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
                return View("Perfil", userUpdated);
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
                return View("Perfil", user);
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
                    return View("Perfil", user);
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

        public ActionResult PedidoAdocao()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Categorias()
        {
            var categorias = (from categoriaAux in _context.Categorias
                              join a in _context.Racas on categoriaAux.Id equals a.CategoriaId
                              join b in _context.Animais on a.Id equals b.RacaId
                              orderby categoriaAux.Nome
                              select new Categoria { Id = categoriaAux.Id, Nome = categoriaAux.Nome }).Distinct().ToList();
            return Json(new SelectList(categorias, "Id", "Nome"));
        }

        [HttpGet]
        public JsonResult PorteCategoriaSexo(int Categoria, string Sexo)
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
                                      where a.Sexo.Equals(Sexo) && c.Id.Equals(Categoria)
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
                                      where c.Id.Equals(Categoria)
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
                                      where a.Sexo.Equals(Sexo)
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
                                      orderby porteAux.TipoPorte
                                      select new Porte { Id = porteAux.Id, TipoPorte = porteAux.TipoPorte }).Distinct().ToList();
                    retorno = Json(new SelectList(categorias, "Id", "TipoPorte"));
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult RacaCategoriaSexoPorte(int Categoria, string Sexo, int Porte)
        {
            JsonResult retorno = null;
            if (Categoria > 0)
            {
                if (!Sexo.Equals("Ambos"))
                { 
                    if(Porte > 0)
                    {
                        var racas = (from racaAux in _context.Racas
                                     join a in _context.Categorias on racaAux.CategoriaId equals a.Id
                                     join b in _context.Animais on racaAux.Id equals b.RacaId
                                     join c in _context.Portes on b.PorteId equals c.Id
                                     where b.Sexo.Equals(Sexo) && b.PorteId.Equals(Porte) && racaAux.CategoriaId.Equals(Categoria)
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
                                     where b.Sexo.Equals(Sexo) && racaAux.CategoriaId.Equals(Categoria)
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
                                     where b.PorteId.Equals(Porte) && racaAux.CategoriaId.Equals(Categoria)
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
                                     where racaAux.CategoriaId.Equals(Categoria)
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
                                     where b.Sexo.Equals(Sexo)  && b.PorteId.Equals(Porte)
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
                                     where b.Sexo.Equals(Sexo)
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
                                     where b.PorteId.Equals(Porte)
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
                                     orderby racaAux.NomeRaca
                                     select new Raca { Id = racaAux.Id, NomeRaca = racaAux.NomeRaca }).Distinct().ToList();
                        retorno = Json(new SelectList(racas, "Id", "NomeRaca"));
                    }
                }
            }
            return retorno;
        }        

        [HttpGet]
        public JsonResult TodosAnimais()
        {
            var animais = (from animal in _context.Animais
                           orderby animal.NomeAnimal
                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
            return Json(new SelectList(animais, "Id", "NomeAnimal"));
        }

        [HttpGet]
        public JsonResult AnimaisSexo(string Sexo)
        {
            var animais = (from animal in _context.Animais
                           where animal.Sexo.Equals(Sexo)
                           orderby animal.NomeAnimal
                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
            return Json(new SelectList(animais, "Id", "NomeAnimal"));
        }

        [HttpGet]
        public JsonResult AnimaisCategoriaSexo(int Categoria, string Sexo)
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
                                   where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo)
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
                                   where Categoria.Equals(b.Id)
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
                    retorno = AnimaisSexo(Sexo);
                }
                //Sexo nao selecionado
                else
                {
                    retorno = TodosAnimais();
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult AnimaisCategoriaSexoPorte(int Categoria, string Sexo, int Porte)
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
                                       where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    
                }
                //Sexo nao selecionado
                else
                {
                    if(Porte > 0)
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        var animais = (from animal in _context.Animais
                                       join a in _context.Racas on animal.RacaId equals a.Id
                                       join b in _context.Categorias on a.CategoriaId equals b.Id
                                       where Categoria.Equals(b.Id)
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
                                       where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        retorno = AnimaisSexo(Sexo);
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
                                       where animal.PorteId.Equals(Porte)
                                       orderby animal.NomeAnimal
                                       select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                        retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                    }
                    else
                    {
                        retorno = TodosAnimais();
                    }
                }
            }
            return retorno;
        }

        [HttpGet]
        public JsonResult AnimaisCategoriaSexoPorteRaca(int Categoria, string Sexo, int Porte, int Raca)
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
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte)
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
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.Sexo.Equals(Sexo)
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
                                           where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id) && animal.PorteId.Equals(Porte)
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
                                           where Categoria.Equals(b.Id) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where Categoria.Equals(b.Id)
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
                                           where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.Sexo.Equals(Sexo) && animal.PorteId.Equals(Porte)
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
                                           where animal.Sexo.Equals(Sexo) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            retorno = AnimaisSexo(Sexo);
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
                                           where animal.PorteId.Equals(Porte) && animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            var animais = (from animal in _context.Animais
                                           join a in _context.Racas on animal.RacaId equals a.Id
                                           join b in _context.Categorias on a.CategoriaId equals b.Id
                                           where animal.PorteId.Equals(Porte)
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
                                           where animal.RacaId.Equals(Raca)
                                           orderby animal.NomeAnimal
                                           select new Animal { Id = animal.Id, NomeAnimal = animal.NomeAnimal, Foto = animal.Foto }).Distinct().ToList();
                            retorno = Json(new SelectList(animais, "Id", "NomeAnimal"));
                        }
                        else
                        {
                            retorno = TodosAnimais();
                        }
                    }
                }
            }
            return retorno;
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