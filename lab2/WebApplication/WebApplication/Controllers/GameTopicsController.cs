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
    public class GameTopicsController : ControllerBase
    {
        private readonly SiteContext _context;

        public GameTopicsController(SiteContext context)
        {
            _context = context;
        }

        // GET: api/GameTopics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameTopic>>> GetGameTopic()
        {
            return await _context.GameTopic.ToListAsync();
        }

        // GET: api/GameTopics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameTopic>> GetGameTopic(int id)
        {
            var gameTopic = await _context.GameTopic.FindAsync(id);

            if (gameTopic == null)
            {
                return NotFound();
            }

            return gameTopic;
        }

        // PUT: api/GameTopics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameTopic(int id, GameTopic gameTopic)
        {
            if (id != gameTopic.Id)
            {
                return BadRequest();
            }

            _context.Entry(gameTopic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameTopicExists(id))
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

        // POST: api/GameTopics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameTopic>> PostGameTopic(GameTopic gameTopic)
        {
            _context.GameTopic.Add(gameTopic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGameTopic", new { id = gameTopic.Id }, gameTopic);
        }

        // DELETE: api/GameTopics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameTopic(int id)
        {
            var gameTopic = await _context.GameTopic.FindAsync(id);
            if (gameTopic == null)
            {
                return NotFound();
            }

            _context.GameTopic.Remove(gameTopic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GameTopicExists(int id)
        {
            return _context.GameTopic.Any(e => e.Id == id);
        }
    }
}
