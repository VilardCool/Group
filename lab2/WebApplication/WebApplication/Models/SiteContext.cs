using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.Models
{
    public class SiteContext : DbContext
    {
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<News> News { get; set; }

        public SiteContext(DbContextOptions<SiteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WebApplication.Models.GameNews> GameNews { get; set; }

        public DbSet<WebApplication.Models.GameTopic> GameTopic { get; set; }

    }
}
