using System.Collections.Generic;

namespace WebApplication.Models
{
    public class News
    {
        public News()
        {
            Comments = new List<Comment>();
            GameNews = new List<GameNews>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Picture { get; set; }
        public string Content { get; set; }
        public int AmountOfLikes { get; set; }
        public int AmountOfComments { get; set; }
        public int AmountOfReposts { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GameNews> GameNews { get; set; }
    }
}
