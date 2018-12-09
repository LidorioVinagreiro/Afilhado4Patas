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
    public class UtilizadorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilizadorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Adotar()
        {
            return View();
        }

        public IActionResult Doar()
        {
            return View();
        }


        // GET: Perfil/Details/5
        public ActionResult Details(string id)
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

        // GET: Perfil/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Perfil/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Perfil/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editarPerfilViewModel = _context.Utilizadores.Where(e => e.Email == id).Include(p=> p.Perfil).FirstOrDefault();
            Perfil perfil = editarPerfilViewModel.Perfil;

            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }
            return View(new PerfilViewModel { FirstName=perfil.FirstName,LastName=perfil.LastName });
        }

        // POST: EditarPerfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,PerfilViewModel editarPerfilViewModel)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userid = _context.Utilizadores.FirstOrDefault(u => u.UserName == id).Id;
                    var perfil = _context.PerfilTable.FirstOrDefault(p => p.UtilizadorId == userid);
                    perfil.FirstName = editarPerfilViewModel.FirstName;
                    perfil.LastName = editarPerfilViewModel.LastName;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                //return RedirectToAction(nameof(Index));
                return View("Details");
            }
            return View("Index");
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