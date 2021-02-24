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
    public class CharacterChoosesController : Controller
    {
        private readonly GameDBContext _context;

        public CharacterChoosesController(GameDBContext context)
        {
            _context = context;
        }

        // GET: CharacterChooses
        public async Task<IActionResult> Index()
        {
            var gameDBContext = _context.CharacterChooses.Include(c => c.Character).Include(p => p.Player);
            return View(await gameDBContext.ToListAsync());
        }

        // GET: CharacterChooses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterChoose = await _context.CharacterChooses
                .Include(c => c.Character)
                .Include(c => c.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterChoose == null)
            {
                return NotFound();
            }

            return View(characterChoose);
        }

        // GET: CharacterChooses/Create
        public IActionResult Create()
        {
            ViewData["CharacterId"] = new SelectList(_context.Characters.Where(c => c.Playable == 1), "Id", "Name");
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Nickname");
            return View();
        }

        // POST: CharacterChooses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,CharacterId")] CharacterChoose characterChoose)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterChoose);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterChoose.CharacterId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Login", characterChoose.PlayerId);
            return View(characterChoose);
        }

        // GET: CharacterChooses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterChoose = await _context.CharacterChooses.FindAsync(id);
            if (characterChoose == null)
            {
                return NotFound();
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterChoose.CharacterId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Login", characterChoose.PlayerId);
            return View(characterChoose);
        }

        // POST: CharacterChooses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,CharacterId")] CharacterChoose characterChoose)
        {
            if (id != characterChoose.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterChoose);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterChooseExists(characterChoose.Id))
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
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterChoose.CharacterId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Login", characterChoose.PlayerId);
            return View(characterChoose);
        }

        // GET: CharacterChooses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterChoose = await _context.CharacterChooses
                .Include(c => c.Character)
                .Include(c => c.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterChoose == null)
            {
                return NotFound();
            }

            return View(characterChoose);
        }

        // POST: CharacterChooses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterChoose = await _context.CharacterChooses.FindAsync(id);
            _context.CharacterChooses.Remove(characterChoose);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterChooseExists(int id)
        {
            return _context.CharacterChooses.Any(e => e.Id == id);
        }
    }
}
