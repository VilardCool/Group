using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class GameTopic
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int TopicId { get; set; }

        public virtual Game Game { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
