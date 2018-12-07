using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Afilhado4Patas.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Afilhado4Patas.Controllers
{
    public class PerfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PerfilController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Perfil
        public ActionResult Index()
        {
            return View();
        }

        // GET: Perfil/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.Utilizadores.FirstOrDefault(u => u.PerfilId == id);
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Perfil/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Perfil/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Perfil/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}