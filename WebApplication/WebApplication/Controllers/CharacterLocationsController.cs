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
    public class CharacterLocationsController : Controller
    {
        private readonly GameDBContext _context;

        public CharacterLocationsController(GameDBContext context)
        {
            _context = context;
        }

        // GET: CharacterLocations
        public async Task<IActionResult> Index()
        {
            var gameDBContext = _context.CharacterLocations.Include(c => c.Character).Include(c => c.Map);
            return View(await gameDBContext.ToListAsync());
        }

        // GET: CharacterLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterLocation = await _context.CharacterLocations
                .Include(c => c.Character)
                .Include(c => c.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterLocation == null)
            {
                return NotFound();
            }

            return View(characterLocation);
        }

        // GET: CharacterLocations/Create
        public IActionResult Create()
        {
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name");
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name");
            return View();
        }

        // POST: CharacterLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CharacterId,MapId,PositionX,PositionY")] CharacterLocation characterLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterLocation.CharacterId);
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", characterLocation.MapId);
            return View(characterLocation);
        }

        // GET: CharacterLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterLocation = await _context.CharacterLocations.FindAsync(id);
            if (characterLocation == null)
            {
                return NotFound();
            }
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterLocation.CharacterId);
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", characterLocation.MapId);
            return View(characterLocation);
        }

        // POST: CharacterLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CharacterId,MapId,PositionX,PositionY")] CharacterLocation characterLocation)
        {
            if (id != characterLocation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterLocationExists(characterLocation.Id))
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
            ViewData["CharacterId"] = new SelectList(_context.Characters, "Id", "Name", characterLocation.CharacterId);
            ViewData["MapId"] = new SelectList(_context.Maps, "Id", "Name", characterLocation.MapId);
            return View(characterLocation);
        }

        // GET: CharacterLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterLocation = await _context.CharacterLocations
                .Include(c => c.Character)
                .Include(c => c.Map)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterLocation == null)
            {
                return NotFound();
            }

            return View(characterLocation);
        }

        // POST: CharacterLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterLocation = await _context.CharacterLocations.FindAsync(id);
            _context.CharacterLocations.Remove(characterLocation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterLocationExists(int id)
        {
            return _context.CharacterLocations.Any(e => e.Id == id);
        }
    }
}
