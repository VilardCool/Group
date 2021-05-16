using Microsoft.EntityFrameworkCore;

namespace WebApplication.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<GameNews> GameNews { get; set; }
        public DbSet<GameTopic> GameTopics { get; set; }

    }
}