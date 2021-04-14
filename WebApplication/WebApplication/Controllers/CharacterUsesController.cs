using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication.Controllers
{
    [Authorize]
    public class CharacterUsesController : Controller
    {
        private readonly GameDBContext _context;

        public CharacterUsesController(GameDBContext context)
        {
            _context = context;
        }

        // GET: CharacterUses
        public async Task<IActionResult> Index()
        {
            var gameDBContext = _context.CharacterUses.Include(c => c.Character).Include(c => c.Weapon);
            return View(await gameDBContext.ToListAsync());
        }

        // GET: CharacterUses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterUse = await _context.CharacterUses
                .Include(c => c.Character)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterUse == null)
            {
                return NotFound();
            }

            return View(characterUse);
        }

        // GET: CharacterUses/Create
        public IActionResult Create(int? characId, int? weapId)
        {
            if (characId == null) ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name");
            else ViewData["CharacterId"] = new SelectList(_context.Characters.Where(c => c.Id == characId), "Id", "Name");
            if (weapId == null) ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Model");
            else ViewData["WeaponId"] = new SelectList(_context.Weapons.Where(w => w.Id == weapId), "Id", "Model");
            return View();
        }

        // POST: CharacterUses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? characId, int? weapId, [Bind("Id,CharacterId,WeaponId")] CharacterUse characterUse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterUse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters.Where(c => c.Id == characId), "Id", "Name", characterUse.CharacterId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons.Where(w => w.Id == weapId), "Id", "Model", characterUse.WeaponId);
            return View(characterUse);
        }

        // GET: CharacterUses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterUse = await _context.CharacterUses.FindAsync(id);
            if (characterUse == null)
            {
                return NotFound();
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterUse.CharacterId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Model", characterUse.WeaponId);
            return View(characterUse);
        }

        // POST: CharacterUses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CharacterId,WeaponId")] CharacterUse characterUse)
        {
            if (id != characterUse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterUse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterUseExists(characterUse.Id))
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
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterUse.CharacterId);
            ViewData["WeaponId"] = new SelectList(_context.Weapons, "Id", "Model", characterUse.WeaponId);
            return View(characterUse);
        }

        // GET: CharacterUses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterUse = await _context.CharacterUses
                .Include(c => c.Character)
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterUse == null)
            {
                return NotFound();
            }

            return View(characterUse);
        }

        // POST: CharacterUses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterUse = await _context.CharacterUses.FindAsync(id);
            _context.CharacterUses.Remove(characterUse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterUseExists(int id)
        {
            return _context.CharacterUses.Any(e => e.Id == id);
        }
    }
}
