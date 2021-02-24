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
    public class WeaponTypesController : Controller
    {
        private readonly GameDBContext _context;

        public WeaponTypesController(GameDBContext context)
        {
            _context = context;
        }

        // GET: WeaponTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.WeaponTypes.ToListAsync());
        }

        // GET: WeaponTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weaponType == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Weapons", new {typeId = weaponType.Id, name = weaponType.Name });
        }

        // GET: WeaponTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WeaponTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] WeaponType weaponType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weaponType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weaponType);
        }

        // GET: WeaponTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponType = await _context.WeaponTypes.FindAsync(id);
            if (weaponType == null)
            {
                return NotFound();
            }
            return View(weaponType);
        }

        // POST: WeaponTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] WeaponType weaponType)
        {
            if (id != weaponType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weaponType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeaponTypeExists(weaponType.Id))
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
            return View(weaponType);
        }

        // GET: WeaponTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weaponType = await _context.WeaponTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weaponType == null)
            {
                return NotFound();
            }

            return View(weaponType);
        }

        // POST: WeaponTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weaponType = await _context.WeaponTypes.FindAsync(id);
            _context.WeaponTypes.Remove(weaponType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeaponTypeExists(int id)
        {
            return _context.WeaponTypes.Any(e => e.Id == id);
        }
    }
}
