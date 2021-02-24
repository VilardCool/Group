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
    public class WeaponsController : Controller
    {
        private readonly GameDBContext _context;

        public WeaponsController(GameDBContext context)
        {
            _context = context;
        }

        // GET: Weapons
        public async Task<IActionResult> Index(int? id, int? typeId, string? name)
        {
            if (id == null && typeId==null) return View(await _context.Weapons.ToListAsync());
            ViewBag.WeaponTypeId = typeId;
            ViewBag.WeaponTypeName = name;
            if (id == null) return View(await _context.Weapons.Where(w => w.TypeId == typeId).Include(w => w.Type).ToListAsync());
            return View(await _context.Weapons.Include(c => c.CharacterUses).Where(c => c.CharacterUses.Any(p => p.CharacterId == id)).Include(c => c.Type).ToListAsync());
        }

        // GET: Weapons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons
                .Include(w => w.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }

        // GET: Weapons/Create
        public IActionResult Create(int weaponTypeId)
        {
            //ViewData["TypeId"] = new SelectList(_context.WeaponTypes, "Id", "Name");
            ViewBag.WeaponTypeId = weaponTypeId;
            ViewBag.WeaponTypeName = _context.WeaponTypes.Where(w => w.Id == weaponTypeId).FirstOrDefault().Name;
            return View();
        }

        // POST: Weapons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int weaponTypeId, [Bind("Id,TypeId,Model,Damage,Magazine,RateOfFire")] Weapon weapon)
        {
            weapon.TypeId = weaponTypeId;
            if (ModelState.IsValid)
            {
                _context.Add(weapon);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Weapons", new { id = weaponTypeId, name = _context.WeaponTypes.Where(w => w.Id == weaponTypeId).FirstOrDefault().Name });
            }
            //ViewData["TypeId"] = new SelectList(_context.WeaponTypes, "Id", "Name", weapon.TypeId);
            //return View(weapon);
            return RedirectToAction("Index", "Weapons", new { id = weaponTypeId, name = _context.WeaponTypes.Where(w => w.Id == weaponTypeId).FirstOrDefault().Name });
        }

        // GET: Weapons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons.FindAsync(id);
            if (weapon == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.WeaponTypes, "Id", "Name", weapon.TypeId);
            return View(weapon);
        }

        // POST: Weapons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TypeId,Model,Damage,Magazine,RateOfFire")] Weapon weapon)
        {
            if (id != weapon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weapon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeaponExists(weapon.Id))
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
            ViewData["TypeId"] = new SelectList(_context.WeaponTypes, "Id", "Name", weapon.TypeId);
            return View(weapon);
        }

        // GET: Weapons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weapon = await _context.Weapons
                .Include(w => w.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weapon == null)
            {
                return NotFound();
            }

            return View(weapon);
        }

        // POST: Weapons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weapon = await _context.Weapons.FindAsync(id);
            _context.Weapons.Remove(weapon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaponExists(int id)
        {
            return _context.Weapons.Any(e => e.Id == id);
        }
    }
}
