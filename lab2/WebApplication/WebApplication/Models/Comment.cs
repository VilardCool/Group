using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int AmountOfLikes { get; set; }

    }
}
