using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Afilhado4Patas.Data;
using Afilhado4Patas.Models.ViewModels;

namespace Afilhado4Patas.Controllers
{
    public class EditarPerfilController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EditarPerfilController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EditarPerfil
        public async Task<IActionResult> Index()
        {
            return View(await _context.EditarPerfilViewModel.ToListAsync());
        }

        // GET: EditarPerfil/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editarPerfilViewModel = await _context.EditarPerfilViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }

            return View(editarPerfilViewModel);
        }

        // GET: EditarPerfil/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EditarPerfil/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Street,City,Postalcode,NIF,Photo,Birthday,OldPassword,NewPassword,ConfirmPassword")] EditarPerfilViewModel editarPerfilViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(editarPerfilViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(editarPerfilViewModel);
        }

        // GET: EditarPerfil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editarPerfilViewModel = await _context.EditarPerfilViewModel.FindAsync(id);
            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }
            return View(editarPerfilViewModel);
        }

        // POST: EditarPerfil/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Street,City,Postalcode,NIF,Photo,Birthday,OldPassword,NewPassword,ConfirmPassword")] EditarPerfilViewModel editarPerfilViewModel)
        {
            if (id != editarPerfilViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editarPerfilViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditarPerfilViewModelExists(editarPerfilViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(editarPerfilViewModel);
        }

        // GET: EditarPerfil/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var editarPerfilViewModel = await _context.EditarPerfilViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (editarPerfilViewModel == null)
            {
                return NotFound();
            }

            return View(editarPerfilViewModel);
        }

        // POST: EditarPerfil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var editarPerfilViewModel = await _context.EditarPerfilViewModel.FindAsync(id);
            _context.EditarPerfilViewModel.Remove(editarPerfilViewModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EditarPerfilViewModelExists(int id)
        {
            return _context.EditarPerfilViewModel.Any(e => e.Id == id);
        }
    }
}
