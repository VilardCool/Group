using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly GameDBContext _context;

        public ChartsController(GameDBContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var characters = _context.Characters.ToList();
            List<object> hpCharacter = new List<object>();
            List<int> already = new List<int>();
            hpCharacter.Add(new[] { "Здоров'я", "Кількість персонажів" });

            foreach(var c in characters)
            {
                if (!already.Contains(c.Health))
                {
                    hpCharacter.Add(new object[] { c.Health.ToString(), _context.Characters.Where(h => h.Health == c.Health).Count() });
                    already.Add(c.Health);
                }
            }

            return new JsonResult(hpCharacter);
        }

        [HttpGet("JsonDataWeapon")]
        public JsonResult JsonDataWeapon(int id)
        {
            var weapons = _context.Weapons.Include(w => w.Type).ToList();
            List<object> amountOfWeapon = new List<object>();
            List<int> already = new List<int>();
            amountOfWeapon.Add(new[] { "Тип зброї", "Кількість моделей" });

            foreach (var w in weapons)
            {
                if (!already.Contains(w.TypeId))
                {
                    amountOfWeapon.Add(new object[] { Tyname(w.TypeId), _context.Weapons.Where(g => g.TypeId == w.TypeId).Count() });
                    already.Add(w.TypeId);
                }
            }

            return new JsonResult(amountOfWeapon);
        }

        private string Tyname(int id)
        {
            var query = (from p in _context.WeaponTypes
                         select p).ToList();
            foreach(var q in query)
            {
                if (q.Id == id) return q.Name;
            }
            return "";
        }
    }
}
