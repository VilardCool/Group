using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GameController : Controller
    {
        ApplicationContext db;
        public GameController(ApplicationContext context)
        {
            db = context;
            if (!db.Games.Any())
            {
                db.Games.Add(new Game { Title = "Demo", Content = "Test"});
                db.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return db.Games.ToList();
        }

        [HttpGet("{id}")]
        public Game Get(int id)
        {
            Game game = db.Games.FirstOrDefault(x => x.Id == id);
            return game;
        }

        [HttpPost]
        public IActionResult Post(Game game)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(game);
                db.SaveChanges();
                return Ok(game);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(Game game)
        {
            if (ModelState.IsValid)
            {
                db.Update(game);
                db.SaveChanges();
                return Ok(game);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Game game = db.Games.FirstOrDefault(x => x.Id == id);
            if (game != null)
            {
                db.Games.Remove(game);
                db.SaveChanges();
            }
            return Ok(game);
        }
    }
}