using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication;

namespace WebApplication.Controllers
{
    public class ComunicationsController : Controller
    {
        private readonly GameDBContext _context;

        public ComunicationsController(GameDBContext context)
        {
            _context = context;
        }

        // GET: Comunications
        public async Task<IActionResult> Index()
        {
            var gameDBContext = _context.Comunications.Include(c => c.Character1).Include(c => c.Character2);
            return View(await gameDBContext.ToListAsync());
        }

        // GET: Comunications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comunication = await _context.Comunications
                .Include(c => c.Character1)
                .Include(c => c.Character2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comunication == null)
            {
                return NotFound();
            }

            return View(comunication);
        }

        // GET: Comunications/Create
        public IActionResult Create()
        {
            ViewData["Character1Id"] = new SelectList(_context.Characters, "Id", "Name");
            ViewData["Character2Id"] = new SelectList(_context.Characters.Where(c => c.Id != _context.Characters.First().Id), "Id", "Name");
            return View();
        }

        public JsonResult GetChar2(int id)
        {
            return Json(new SelectList(_context.Characters.Where(c => c.Id != id), "Id", "Name"));
        }

        // POST: Comunications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Character1Id,Character2Id")] Comunication comunication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comunication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Character1Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character1Id);
            ViewData["Character2Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character2Id);
            return View(comunication);
        }

        // GET: Comunications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comunication = await _context.Comunications.FindAsync(id);
            if (comunication == null)
            {
                return NotFound();
            }
            ViewData["Character1Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character1Id);
            ViewData["Character2Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character2Id);
            return View(comunication);
        }

        // POST: Comunications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Character1Id,Character2Id")] Comunication comunication)
        {
            if (id != comunication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comunication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComunicationExists(comunication.Id))
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
            ViewData["Character1Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character1Id);
            ViewData["Character2Id"] = new SelectList(_context.Characters, "Id", "Name", comunication.Character2Id);
            return View(comunication);
        }

        // GET: Comunications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comunication = await _context.Comunications
                .Include(c => c.Character1)
                .Include(c => c.Character2)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comunication == null)
            {
                return NotFound();
            }

            return View(comunication);
        }

        // POST: Comunications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comunication = await _context.Comunications.FindAsync(id);
            _context.Comunications.Remove(comunication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComunicationExists(int id)
        {
            return _context.Comunications.Any(e => e.Id == id);
        }
    }
}
