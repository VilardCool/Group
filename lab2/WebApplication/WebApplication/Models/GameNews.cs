using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class GameNews
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int NewsId { get; set; }

        public virtual Game Game { get; set; }
        public virtual News News { get; set; }
    }
}
