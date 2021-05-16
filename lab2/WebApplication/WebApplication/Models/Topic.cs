using System.Collections.Generic;

namespace WebApplication.Models
{
    public class Topic
    {
        public Topic()
        {
            Comments = new List<Comment>();
            GameTopics = new List<GameTopic>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AmountOfLikes { get; set; }
        public int AmountOfComments { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<GameTopic> GameTopics { get; set; }
    }
}