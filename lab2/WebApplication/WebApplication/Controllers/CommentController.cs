using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/comments")]
    public class CommentController : Controller
    {
        ApplicationContext db;
        public CommentController(ApplicationContext context)
        {
            db = context;
            if (!db.Comments.Any())
            {
                db.Comments.Add(new Comment { Content = "Test" });
                db.SaveChanges();
            }
        }
        [HttpGet]
        public IEnumerable<Comment> Get()
        {
            return db.Comments.ToList();
        }

        [HttpGet("{id}")]
        public Comment Get(int id)
        {
            Comment comment = db.Comments.FirstOrDefault(x => x.Id == id);
            return comment;
        }

        [HttpPost]
        public IActionResult Post(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Ok(comment);
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult Put(Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Update(comment);
                db.SaveChanges();
                return Ok(comment);
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.FirstOrDefault(x => x.Id == id);
            if (comment != null)
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
            }
            return Ok(comment);
        }
    }
}