﻿using System;
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
    public class PlayersController : Controller
    {
        private readonly GameDBContext _context;

        public PlayersController(GameDBContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(int? id, string? server)
        {
            if (id == null) return View(await _context.Players.Include(p => p.Session).ToListAsync());
            ViewBag.SessionId = id;
            ViewBag.SessionServer = server;
            var gameDBContext = _context.Players.Where(p =>p.SessionId == id).Include(p => p.Session);
            return View(await gameDBContext.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Characters", new { id = player.Id});
        }

        // GET: Players/Create
        public IActionResult Create(int? sessionId)
        {
            if (sessionId == null) ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Server");
            else
            {
                ViewBag.SessionId = sessionId;
                ViewBag.SessionServer = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault().Server;
                ViewData["SessionId"] = new SelectList(_context.Sessions.Where(s => s.Id == sessionId), "Id", "Server");
            }
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? sessionId, [Bind("Id,Login,Password,Nickname,SessionId")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Players", new {id = sessionId, server = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault().Server });
            }
            return RedirectToAction("Index", "Players", new { id = sessionId, server = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault().Server });
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Server", player.SessionId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Login,Password,Nickname,SessionId")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            ViewData["SessionId"] = new SelectList(_context.Sessions, "Id", "Server", player.SessionId);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Session)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            if (_context.CharacterChooses.Any(c => c.PlayerId == id))
            {
                return RedirectToAction("Index", "CharacterChooses");
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
