using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameNewsController : ControllerBase
    {
        private readonly SiteContext _context;

        public GameNewsController(SiteContext context)
        {
            _context = context;
        }

        // GET: api/GameNews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameNews>>> GetGameNews()
        {
            return await _context.GameNews.ToListAsync();
        }

        // GET: api/GameNews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameNews>> GetGameNews(int id)
        {
            var gameNews = await _context.GameNews.FindAsync(id);

            if (gameNews == null)
            {
                return NotFound();
            }

            return gameNews;
        }

        // PUT: api/GameNews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameNews(int id, GameNews gameNews)
        {
            if (id != gameNews.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameNews).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameNewsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GameNews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameNews>> PostGameNews(GameNews gameNews)
        {
            _context.GameNews.Add(gameNews);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameNews", new { id = gameNews.Id }, gameNews);
        }

        // DELETE: api/GameNews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameNews(int id)
        {
            var gameNews = await _context.GameNews.FindAsync(id);
            if (gameNews == null)
            {
                return NotFound();
            }

            _context.GameNews.Remove(gameNews);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameNewsExists(int id)
        {
            return _context.GameNews.Any(e => e.Id == id);
        }
    }
}
