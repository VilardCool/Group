using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Game
    {
        public Game()
        {
            Comments = new List<Comment>();
            GameNews = new List<GameNews>();
            GameTopics = new List<GameTopic>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public string File { get; set; }
        public int AmountOfLikes { get; set; }
        public int AmountOfComments { get; set; }
        public int AmountOfReposts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GameNews> GameNews { get; set; }
        public virtual ICollection<GameTopic> GameTopics { get; set; }
    }
}
