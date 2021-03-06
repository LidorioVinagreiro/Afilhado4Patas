﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        /// <summary>
        /// Ação que devolve a view do dashboard do responsavel 
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
        /// <returns>View do dashboard do responsavel atual</returns>
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

        /// <summary>
        /// Ação que devolve a view com os dados de um utilizador
        /// </summary>
        /// <param name="id">Id do funcionario a mostrar</param>
        /// <returns>View com os dados de um utilizador</returns>
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
                return View("Dashboard", userUpdated);
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
                return View("Dashboard", user);
            }
            return View(editarPerfilViewModel);
        }

        /// <summary>
        /// Ação que devolve a view que edita a informação atual referente a foto de perfil do responsavel
        /// </summary>
        /// <param name="id">Id do reponsavel atual</param>
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
        /// Ação que altera a informação referente a foto de perfil do responsavel
        /// </summary>
        /// <param name="id">Id do responsavel atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do responsavel com a informação atualizada</returns>
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
        /// Ação que devolve a view que edita a informação atual referente a palavra passe do responsavel
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <returns>View de edição de palavra passe</returns>
        public async Task<IActionResult> PerfilEditarPalavraPasse(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View();
        }


        /// <summary>
        /// Ação que altera a informação referente a palvra passe do responsavel
        /// </summary>
        /// <param name="id">Id do utilizador atual</param>
        /// <param name="editarPerfilViewModel">Modelo da informação na view</param>
        /// <returns>View do perfil do responsavel com a informação atualizada</returns>
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
                var user = _context.Utilizadores.Where(u => u.Email == modelo.Email).FirstOrDefault();
                if (user == null)
                {
                    var novoFuncionario = new Utilizadores
                    {
                        UserName = modelo.Email,
                        Email = modelo.Email,
                        EmailConfirmed = true,
                        Active = true
                    };

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
                        return View("ListaFuncionarios", ListaTotalFuncionarios());
                    }
                }
                if (_context.Utilizadores.Where(u => u.Email == modelo.Email).ToList().Count > 0)
                {
                    ModelState.AddModelError("", "O email inserido já encontra em utilização, insira outro!");
                    return View(modelo);
                }
            }
            return View(modelo);
        }

        /// <summary>
        /// Ação que remove um funcionario e atualiza a view
        /// </summary>
        /// <returns>View com a lista de funcionarios atualizada</returns>
        public async Task<ActionResult> RemoverFuncionarios(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Utilizadores.Where(u => u.Id == id).Include(p => p.Perfil).FirstOrDefault();
            user.Active = false;
            _context.SaveChanges();

            //Envio de Email
            var emailModel = new EmailViewModel("", user.Perfil.FirstName);
            string body = await _razorView.RenderViewToStringAsync("/Views/Emails/RemoveAccount/RemoveAccount.cshtml", emailModel);
            await _emailSender.SendEmailAsync(user.Email, "Conta Terminada", body);

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

        /****************************************************************************************************/
        /******************************************** Animais ******************************************/
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
                    return RedirectToAction("DetalhesAnimal", "Responsavel", new { id = animal.Id });
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
                    return RedirectToAction("DetalhesAnimal", "Responsavel", new { id = model.animal.Id });
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
                    return RedirectToAction("DetalhesAnimal", "Responsavel", new { id = model.animal.Id });
                }
            }
            model.animal = _context.Animais.Where(a => a.Id == id).FirstOrDefault();
            return View(model);
        }

        /****************************************************************************************************/
        /******************************************** Adocoes ***********************************************/
        /****************************************************************************************************/

        /// <summary>
        /// Ação que mostra os pedidos de adoçao que se encontram em espera
        /// </summary>
        /// <returns>View com os pedidos de adocao pendentes</returns>
        public ActionResult PedidosAdocao()
        {
            List<PedidoAdocao> pedidos = _context.PedidosAdocao.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        /// <summary>
        /// Ação que mostra os pedidos de fim de semana que se encontram em espera
        /// </summary>
        /// <returns>View com os pedidos de fim de semana pendentes</returns>
        public ActionResult PedidosFimSemana()
        {
            List<PedidoFimSemana> pedidos = _context.PedidosFimSemana.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        /// <summary>
        /// Ação que mostra os pedidos de passeio que se encontram em espera
        /// </summary>
        /// <returns>View com os pedidos de passeio pendentes</returns>
        public ActionResult PedidosPasseio()
        {
            List<PedidoPasseio> pedidos = _context.PedidosPasseio.Where(p => p.Aprovacao.Equals("Em espera")).Include(a => a.Animal).ToList();
            foreach (var pedido in pedidos)
            {
                pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            }
            return View(pedidos);
        }

        /// <summary>
        /// Ação que mostra o pedido de adocao a ser analisado
        /// </summary>
        /// <param name="id">Id do pedido a ser analisado</param>
        /// <returns>View com os dados do pedido</returns>
        public ActionResult PedidosAdocaoAnalisar(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        /// <summary>
        /// Ação que mostra o pedido de fim de semana a ser analisado
        /// </summary>
        /// <param name="id">Id do pedido a ser analisado</param>
        /// <returns>View com os dados do pedido</returns>
        public ActionResult PedidosFimSemanaAnalisar(int id)
        {
            PedidoFimSemana pedido = _context.PedidosFimSemana.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        /// <summary>
        /// Ação que mostra o pedido de passeio a ser analisado
        /// </summary>
        /// <param name="id">Id do pedido a ser analisado</param>
        /// <returns>View com os dados do pedido</returns>
        public ActionResult PedidosPasseioAnalisar(int id)
        {
            PedidoPasseio pedido = _context.PedidosPasseio.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
            return View(pedido);
        }

        /// <summary>
        /// Ação que é mostrada quando um pedido de adoção é aceite
        /// </summary>
        /// <param name="id">Id do pedido aceite</param>
        /// <returns>View do pedido aceite</returns>
        public ActionResult AceitarPedidoAdocao(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            AprovacaoViewModel aprovacao = new AprovacaoViewModel
            {
                PedidoId = pedido.Id
            };
            return View("PedidoAdocaoAceite", aprovacao);
        }

        /// <summary>
        /// Ação que é mostrada quando um pedido de adoção é aceite
        /// </summary>
        /// <param name="aprovacao">Model do pedido aceite</param>
        /// <returns>View do pedido aceite</returns>
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

        /// <summary>
        /// Ação que é mostrada quando um pedido de fim de semana é aceite
        /// </summary>
        /// <param name="id">Id do pedido aceite</param>
        /// <returns>View do pedido aceite</returns>
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

        /// <summary>
        /// Ação que é mostrada quando um pedido de passeio é aceite
        /// </summary>
        /// <param name="id">Id do pedido aceite</param>
        /// <returns>View do pedido aceite</returns>
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

        /// <summary>
        /// Ação que é mostrada quando um pedido de passeio é rejeitado
        /// </summary>
        /// <param name="id">Id do pedido rejeitado</param>
        /// <returns>View do pedido rejeitado</returns>
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

        /// <summary>
        /// Ação que é mostrada quando um pedido de fim de semana é rejeitado
        /// </summary>
        /// <param name="id">Id do pedido rejeitado</param>
        /// <returns>View do pedido rejeitado</returns>
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

        /// <summary>
        /// Ação que é mostrada quando um pedido de adoção é rejeitado
        /// </summary>
        /// <param name="id">Id do pedido rejeitado</param>
        /// <returns>View do pedido rejeitado</returns>
        public ActionResult RejeitarPedidoAdocao(int id)
        {
            PedidoAdocao pedido = _context.PedidosAdocao.Where(p => p.Id == id).Include(a => a.Animal).FirstOrDefault();
            AprovacaoViewModel aprovacao = new AprovacaoViewModel
            {
                PedidoId = pedido.Id
            };
            return View("PedidoAdocaoRejeitado", aprovacao);
        }

        /// <summary>
        /// Ação que é mostrada quando um pedido de adoção é rejeitado
        /// </summary>
        /// <param name="aprovacao">Model do pedido rejeitado</param>
        /// <returns></returns>
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

        /// <summary>
        /// Acao que efetua uma reuniao com a pessoa que adotou um animal
        /// </summary>
        /// <returns>View com as adocoes disponiveis de escolher</returns>
        public ActionResult ConvocarReuniao()
        {
            List<Adocao> adocoes = _context.Adocoes.Include(p => p.Pedido).Where(a => a.Pedido.TipoAdocao == "Total").ToList();
            foreach (var adocao in adocoes)
            {
                adocao.Pedido.Adotante = _context.Utilizadores.Where(u => u.PerfilId == adocao.Pedido.AdotanteId).Include(p => p.Perfil).FirstOrDefault();
                adocao.Pedido.Animal = _context.Animais.Where(a => a.Id == adocao.Pedido.AnimalId).Include(p => p.PorteAnimal).FirstOrDefault();
            }
            return View(adocoes);
        }

        /// <summary>
        /// Acao que efetua uma reuniao com a pessoa que adotou um animal
        /// </summary>
        /// <param name="dicionario">Par de valores com o utilizador e com o animal</param>
        /// <returns>View com as adocoes disponiveis de escolher</returns>
        public ActionResult ConvocarReuniaoEspecifica(Dictionary<string, string> dicionario)
        {
            ConvocatoriaViewModel convocatoria = new ConvocatoriaViewModel
            {
                UtilizadorId = dicionario.First().Key,
                AnimalNome = dicionario.First().Value
            };
            return View(convocatoria);
        }

        /// <summary>
        /// Acao que notifica o utilizador atraves de um email
        /// </summary>
        /// <param name="convocatoria">Model com os dados da convocatoria da reuniao</param>
        /// <returns>View de reuniao registada</returns>
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

        /// <summary>
        /// Ação que efetua uma pesquisa de um animal no sistema
        /// </summary>
        /// <param name="pesquisa">Filtro da pesquisa dos animais</param>
        /// <returns>View com os animais pesquisados</returns>
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
            return PartialView("~/Views/Responsavel/GridAnimaisAdotados.cshtml", adocoesMostrar);
        }

        /// <summary>
        /// Metodo que cria um diretorio na pasta do projeto
        /// </summary>
        /// <param name="path">Caminho onde a nova pasta vai ser criada</param>
        /// <returns>Se o caminho existe ou não</returns>
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
        /// Metodo que devolve um select list em json com as racas agrupadas por categorias
        /// </summary>
        /// <param name="CategoriaID">Id da categoria</param>
        /// <returns>Json com as racas agrupadas por categorias</returns>
        [HttpGet]
        public JsonResult RacasPorCategoria(int CategoriaID)
        {
            var racas = (from racasAux in _context.Racas
                         where racasAux.CategoriaId == CategoriaID
                         orderby racasAux.NomeRaca
                         select new Raca { Id = racasAux.Id, NomeRaca = racasAux.NomeRaca }).ToList();
            return Json(new SelectList(racas, "Id", "NomeRaca"));
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

        /// <summary>
        /// Metodo que devolve os dados para a estatistica dos tipos de animal no sistema
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult PieAnimal()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.dataGroupTipoAnimais());
        }

        /// <summary>
        /// Metodo que devolve os dados para a estatistica dos utilizadores agrupados por sexo
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult PieMf()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.dataGroupMF());
        }

        /// <summary>
        ///  Metodo que devolve os dados para a estatistica do numero de adoções por mês
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult adocoesPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.adocoesPorMes());
        }

        /// <summary>
        ///  Metodo que devolve os dados para a estatistica do numero de apadrinhamentos por mês
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult apadrinhamentosPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.apadrinhamentosPorMes());
        }

        /// <summary>
        /// Metodo que devolve os dados para a estatistica do numero de pedidos de passeio por mês
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult numeroPedidosPasseioPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.numeroPedidosPasseioPorMes());
        }

        /// <summary>
        /// Metodo que devolve os dados para a estatistica do numero de pedidos de fins de semana por mês
        /// </summary>
        /// <returns>Json com os dados da estatistica</returns>
        [HttpGet]
        public JsonResult numeroPedidosFdsPorMes()
        {
            DataRetrive dataR = new DataRetrive(_context);
            return Json(dataR.numeroPedidosFdsPorMes());
        }
    }
}