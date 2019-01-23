using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models;
using Afilhado4Patas.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Afilhado4Patas.Controllers.TiposUtilizadores
{
    /// <summary>
    /// Controller que contem todas as ações referentes a funcionalidades que nao requerem sessão iniciada
    /// </summary>
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicialização do controller
        /// </summary>
        /// <param name="context">Objeto da base dados</param>
        public GuestController(ApplicationDbContext context)
        {
            _context = context;
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
                foreach(var adotantes in _context.Adotantes.Where(a => a.AnimalId == animal.Id).ToList())
                {
                    animal.Adotantes.Add(_context.Utilizadores.Where(u => u.Id == adotantes.AdotanteId).FirstOrDefault());
                }
            }
            return View("../Shared/FichaAnimal", animal);
        }

        /// <summary>
        /// Ação que devolve a view de registo no site
        /// </summary>
        /// <returns>View de registo</returns>
        public IActionResult RegistoCompleto()
        {
            return View("../Shared/RegistoCompleto");
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
